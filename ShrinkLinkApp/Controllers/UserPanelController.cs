using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ShrinkLinkCore.Abstractions;
using ShrinkLinkCore.DataTransferObjects;
using ShrinkLinkCore.TableObjects;
using ShrinkLinkCQS.Links.Queries;
using ShrinkLinkCQS.Users.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Policy;
using ShrinkLinkDb.Entities;
using AutoMapper;
using ShrinkLinkApp.Models;
using Humanizer;
using ShrinkLinkCQS.Links.Commands;
using ShrinkLinkApp.Helpers;
using ShrinkLinkCQS.Users.Commands;

namespace ShrinkLinkApp.Controllers
{
    public class UserPanelController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IAdminPanelService _adminPanelService;
        private readonly ModelUserBuilder _modelBuilder;
        private readonly MD5 _md5;
       public UserPanelController(IMediator mediator, IAdminPanelService adminPanelService, IMapper mapper, 
           ModelUserBuilder modelUserBuilder, MD5 md5)
        {
            _mediator = mediator;
            _adminPanelService = adminPanelService;
            _mapper = mapper;
            _modelBuilder = modelUserBuilder;  
            _md5 = md5;
        }

        [HttpGet]
        public async Task<IActionResult> UrlManager(string url, string shortid, string expdate,
             SortState sortOrder = SortState.ExpirationDateAsc, int page = 1)
        {
            try
            {
                string? sUserId = string.Empty;
                var inpObj = new UrlMgrInpObj() { ExpDate = expdate, ShortId = shortid, URL = url, 
                    SortOrder = sortOrder, Page = page };
                if (User.Identities.Any(identity => identity.IsAuthenticated))
                {
                    sUserId = User.FindFirst("UId")?.Value;
                    if (sUserId == null)
                        return Unauthorized();
                }  
                var model = await _adminPanelService.GenerateURLManagerModel(inpObj, sUserId);

                if (model == null)
                    return BadRequest();

                var location = new Uri($"{Request.Scheme}://{Request.Host}");
                var locurl = location.AbsoluteUri;
                ViewData["LocalUrl"]= locurl;
                return View(model);
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UrlEdit(string? shortid)
        {
            if(shortid == null)
                return BadRequest(); 

            var link =await _mediator.Send(new GetLinkByShortIdQuery() { ShortId = shortid });
            if (link == null)   
                return BadRequest();

            string? sUserId = string.Empty;
            Guid UId = Guid.Empty;  
            if (User.Identities.Any(identity => identity.IsAuthenticated))
            {
                sUserId = User.FindFirst("UId")?.Value;
                if (sUserId == null)
                    return Unauthorized();
            }
            if (!Guid.TryParse(sUserId, out UId))
                return BadRequest();

            var user = await _mediator.Send(new GetUserByIdQuery() { UserId = UId });

            if (user == null)
                return BadRequest();

            var model=_mapper.Map<UrlEditModel>((user,link));
            var location = new Uri($"{Request.Scheme}://{Request.Host}");
            var locurl = location.AbsoluteUri;
            model.LocalURL = locurl;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UrlEdit(UrlEditModel model)
        {
            if (model == null) return BadRequest(); 
            if (model.ExpDateTime ==default && model.LinkId==default)
                return BadRequest();
            var res = await _mediator.Send(new PatchLinkCommand()
            { Id = model.LinkId , nameValuePropertiesPairs = new Dictionary<string, object?>() { ["ExpirationDate"] = model.ExpDateTime} });

            if (res == null || res==0 ) return BadRequest();

            return Redirect("~/UserPanel/UrlManager");

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RemoveUserUrl(string linkid, string id)
        {
            if (string.IsNullOrEmpty(linkid) && string.IsNullOrEmpty(id))
                return BadRequest();
            var res1 = Guid.TryParse(linkid, out Guid linkId);
            var res2 = Guid.TryParse(id, out Guid userId);
            if ((res1 && res2)!=true) 
                return BadRequest();
            var res = await _mediator.Send(new RemoveLinkFromUserByIdCommand() { LinkId = linkId, UserId = userId });

            if (res==0)
                 return BadRequest();  
            else
                return Redirect("~/UserPanel/UrlManager");

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AccSettings(string? id)
        {
            var model = await _modelBuilder.BuildById(HttpContext, id);
            if (model != null)
                return View(model);

            return NotFound();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AccSettingsEdit(string? id)
        {
            var model = await _modelBuilder.BuildById(HttpContext, id);
            if (model != null)
                return View(model);

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AccSettingsEdit(UserModel model)
        {
            try
            {
                if (model != null)
                {
                    var dto = _mapper.Map<UserDto>(model);
                    var sourceDto = await _mediator.Send(new GetUserByIdQuery() { UserId = dto.Id });
                    if (sourceDto == null)
                        return NotFound();
                    dto.RegistrationDate = sourceDto.RegistrationDate;
                    dto.LastLogin = sourceDto.LastLogin;
                    PatchMaker<UserDto> patchMaker = new();
                    var patchList = patchMaker.Make(dto, sourceDto);
                    var res = await _mediator.Send(new PatchUserDataCommand() { UserId = dto.Id, patchData = patchList });
                    if (res == null || res == 0)
                        return BadRequest();

                    return RedirectToAction("AccSettings", "UserPanel", new { id = model.Id });
                }
                else
                {
                    //return BadRequest();
                    return new BadRequestObjectResult("Model is null");
                }
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string? id)
        {
            var model = await _modelBuilder.BuildById(HttpContext, id);
            if (model != null)
            {
                var chModel = _mapper.Map<ChangePasswordModel>(model);
                return View(chModel);
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (model != null && model.Id != null && model.Password != null && model.OldPassword != null)
            {
                try
                {
                    var check = await _mediator.Send(new CheckUserPwdHashByUserIdQuery()
                    { UserId = (Guid)model.Id, passwordHash = _md5.CreateMd5(model.OldPassword, 1) });
                    if (check==true)
                    {
                        var result = await _mediator.Send(new ChangeUserPasswordCommand()
                        { UserId = (Guid)model.Id, passwordHash = _md5.CreateMd5(model.Password, 1) });
                        if (result > 0)
                        {
                            model.SystemInfo = "The password has been changed successfully.";
                            return View(model);
                        }
                    }
                    else
                    {
                        model.OldPwdInfo = "You have entered incorrect password";
                        return View(model);
                    }

                }
                catch (Exception e)
                {
                    Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                    return BadRequest();
                }
            }

            ChangePasswordModel model1 = new() { SystemInfo = "Something went wrong (." };
            return View(model1);
        }
    }

}
