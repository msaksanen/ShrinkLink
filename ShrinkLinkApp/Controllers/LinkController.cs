using AutoMapper;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ShrinkLinkApp.Helpers;
using ShrinkLinkApp.Models;
using ShrinkLinkCore.Abstractions;
using ShrinkLinkCore.DataTransferObjects;
using System.Diagnostics.Metrics;

namespace ShrinkLinkApp.Controllers
{
    public class LinkController : Controller
    {
        private readonly IMapper _mapper;
        private readonly URLChecker _urlChecker;
        private readonly MD5 _md5;
        private readonly ILinkService _linkService;
        private readonly IConfiguration _configuration;

        public LinkController(IMapper mapper, URLChecker urlChecker, ILinkService linkService, MD5 md5, 
                              IConfiguration configuration)
        {
            _mapper = mapper;
            _urlChecker = urlChecker;
            _linkService = linkService;
            _md5 = md5;
            _configuration = configuration; 
        }

        [HttpPost]
        public async Task<IActionResult> Shorten (LinkInputModel model)
        {
            ShortenModel respone = new();
            try
            {
                if (model == null || String.IsNullOrEmpty(model?.URL))
                {
                    respone.SystemFlag = 0;
                    respone.SystemInfo = "Input any weblink";
                    return View(respone);
                }
                if (!_urlChecker.IsValidURL(model.URL))
                {
                    respone.SystemFlag = 0;
                    respone.SystemInfo = "Invalid URL";
                    return View(respone);
                }

                LinkDto link = new()
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.Now,
                    Hash = _md5.CreateMd5(model.URL),
                    URL = model.URL,
                };

                if (model.ExpDateTime != default(DateTime) && model.ExpDateTime != null && model.ExpDateTime > DateTime.Now)
                {
                    link.ExpirationDate = model.ExpDateTime!.Value;
                }
                else
                {
                    var optsExp = _configuration["ShrinkLink:ExpiryTimeMin"];
                    if (int.TryParse(optsExp, out var exp) && exp > 0)
                        link.ExpirationDate.AddMinutes(exp);
                    else
                        link.ExpirationDate = DateTime.Now.AddDays(1);
                }

                var newLinkObj = await _linkService.GenerateShorLinkAsync(link);

                if (newLinkObj != null && newLinkObj?.GenResult?.CountResult!=null) 
                {
                    Log.Warning($"Short Id generation collision\nOccurence:{newLinkObj.GenResult.CountResult} time(s)\n" +
                                $"ShortId length:{newLinkObj.GenResult.Length} symbols\nItems:{newLinkObj.GenResult.Text}");
                }


                respone = _mapper.Map<ShortenModel>(newLinkObj);

                var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
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
    }
}
