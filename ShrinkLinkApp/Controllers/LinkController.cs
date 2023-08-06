using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using shortid;
using ShrinkLinkApp.Helpers;
using ShrinkLinkApp.Models;
using ShrinkLinkCore.Abstractions;
using ShrinkLinkCore.DataTransferObjects;
using ShrinkLinkCQS.Links.Queries;
using System.Diagnostics.Metrics;

namespace ShrinkLinkApp.Controllers
{
    public class LinkController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly URLChecker _urlChecker;
        private readonly MD5 _md5;
        private readonly ILinkService _linkService;
        private readonly IConfiguration _configuration;

        public LinkController(IMapper mapper, URLChecker urlChecker, ILinkService linkService, MD5 md5, 
                              IConfiguration configuration, IMediator mediator)
        {
            _mapper = mapper;
            _urlChecker = urlChecker;
            _linkService = linkService;
            _md5 = md5;
            _configuration = configuration; 
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Shorten (LinkInputModel model)
        {
            ShortenModel respone = new();
            try
            {
                if (model == null || String.IsNullOrEmpty(model?.URL))
                {
                    respone.SystemFlag = -1;
                    respone.SystemInfo = "Input any weblink";
                    return View(respone);
                }
                if (!_urlChecker.IsValidURL(model.URL))
                {
                    respone.SystemFlag = -1;
                    respone.SystemInfo = "Invalid URL";
                    return View(respone);
                }

                LinkDto link = new()
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.Now,
                    Hash = _md5.CreateMd5(model.URL),
                    URL = model.URL,
                    ExpirationDate = DateTime.Now
                    
                };

                int defExpTimeMin = 24 * 60 * 365;
                var optsExp = _configuration["ShrinkLink:ExpiryTimeMin"];
                if (int.TryParse(optsExp, out var exp) && exp > 0)
                    defExpTimeMin = exp;

                if (model.ExpDateTime != default(DateTime) && model.ExpDateTime != null 
                    && model.ExpDateTime >= DateTime.Now.AddMinutes(1) && model.ExpDateTime <= DateTime.Now.AddYears(1))
                {
                    link.ExpirationDate = model.ExpDateTime!.Value;
                }
                else
                {
                    link.ExpirationDate = DateTime.Now.AddMinutes(defExpTimeMin);
                }

                var newLinkObj = await _linkService.GenerateShorLinkAsync(link);

                if (newLinkObj != null && newLinkObj?.GenResult!=null && newLinkObj?.GenResult?.CountResult!=0) 
                {
                    Log.Warning($"Short Id generation collision\nOccurrence:{newLinkObj?.GenResult.CountResult} time(s)\n" +
                                $"ShortId length:{newLinkObj?.GenResult.Length} symbols\nItems:{newLinkObj?.GenResult.Text}");
                }


                respone = _mapper.Map<ShortenModel>(newLinkObj);

                var location = new Uri($"{Request.Scheme}://{Request.Host}");
                //var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
                var url = location.AbsoluteUri;
                respone.LocalURL = url; 
                return View(respone);
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> RedirectLink(string shortid)
        {
            try
           {
                var link = await _mediator.Send(new GetLinkByShortIdQuery() { ShortId = shortid });
                if (link!=null && !string.IsNullOrEmpty(link.URL) && link.ExpirationDate> DateTime.Now)
                    return Redirect(link.URL);

                var location = new Uri($"{Request.Scheme}://{Request.Host}");
                var url = location.AbsoluteUri;
                var model = new RedirectLinkModel()
                {
                    ShortLink = shortid,
                    LocalURL = url
                };

                if (link == null || string.IsNullOrEmpty(link.URL))
                {
                    model.SystemFlag = -1;
                    return View(model);  
                }

                model.ExpirationDate = link.ExpirationDate;

                if (model.ExpirationDate < DateTime.Now)
                {
                    model.SystemFlag = 1;
                    model.ShortLink = shortid;
                    return View(model);
                }

                model.SystemFlag = 100;
                return View(model);
            }
           catch (Exception e)
           {
                Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                return BadRequest();
           }
        }
    }
}
