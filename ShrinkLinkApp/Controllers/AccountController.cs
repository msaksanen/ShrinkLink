using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ShrinkLinkApp.Helpers;
using ShrinkLinkApp.Models;
using ShrinkLinkCore.DataTransferObjects;
using ShrinkLinkCQS.Users.Commands;
using System.Security.Claims;
using ShrinkLinkCQS.Users.Queries;
using ShrinkLinkCQS.Roles.Queries;
using Humanizer;

namespace ShrinkLinkApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly MD5  _md5;


        public AccountController( IMediator mediator,
                IMapper mapper, MD5 md5)
        {
            _mapper = mapper;
            _mediator = mediator;   
            _md5 = md5;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customerDto = _mapper.Map<UserDto>(model);
                    if (customerDto != null && !string.IsNullOrEmpty(model.Email))
                    {
                        customerDto.PasswordHash = _md5.CreateMd5(customerDto.PasswordHash,1);
                        var result = await _mediator.Send(new CreateUserWithRoleCommand()
                        { UserDto = customerDto, RoleName = "Customer" });
                        if (result > 0)
                        {
                            await Authenticate(model.Email, customerDto.Id);
                            return RedirectToAction("Welcome", "Account");
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                    return BadRequest();
                }
            }
            return View(model);
        }
        public IActionResult Welcome()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RestrictedLogin()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model.Email != null && model.Password != null)
            {
                var hash = _md5.CreateMd5(model.Password,1);
                var dto = await _mediator.Send(new GetUserDtoByEmailPwdHashQuery() { Email = model.Email, PasswordHash = hash });
                if (dto != null)             //(isPasswordCorrect)
                {
                    await Authenticate(model.Email, dto.Id);
                    return RedirectToAction("Index", "Home");
                }

                var emptyHash = _md5.CreateMd5("",1);
                dto = await _mediator.Send(new GetUserDtoByEmailPwdHashQuery() { Email = model.Email, PasswordHash = emptyHash });

                if (dto != null)
                {
                    //model.SystemInfo = "Your password has been reset";
                    //return View(model);
                    return RedirectToAction("PasswordRestore", "Account", new { id = dto.Id });
                }
                else
                {
                    model.SystemInfo = "Incorrect email or password";
                    return View(model);
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PasswordRestore(string id)
        {
            PasswordRestoreModel model = new();
            if (!string.IsNullOrEmpty(id) && Guid.TryParse(id, out Guid userId) &&
                (await _mediator.Send(new GetUserByIdQuery() { UserId = userId }) != null))
            {
                model.Id = userId;
                return View(model);
            }

            else
                return new BadRequestObjectResult("Something went wrong...");

        }

        [HttpPost]
        public async Task<IActionResult> PasswordRestore(PasswordRestoreModel model)
        {
            if (!ModelState.IsValid)
            {
                model.SystemInfo = "Input correct data, please";
                return View(model);
            }
            if (model.Id == null)
                return new BadRequestObjectResult("User Id is null");
            var user = await _mediator.Send(new GetUserByIdQuery() { UserId = (Guid)model.Id });
            if (user == null)
                return NotFound("User is not found");

            if (user.CodeWord != null && user.CodeWord.Equals(model.Codeword) == true &&
                user.BirthDate != null && user.BirthDate.Equals(model.BirthDate) == true &&
                user.Email != null && user.Email.Equals(model.Email) == true && model.Password != null)
            {
                var hash = _md5.CreateMd5(model.Password, 1);
                var res = await _mediator.Send(new ChangeUserPasswordCommand() { UserId = user.Id, passwordHash = hash });
                if (res > 0)
                {
                    await Authenticate(user.Email, user.Id);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    model.SystemInfo = "Password has not been changed";
                    return View(model);
                }
            }
            else
            {
                model.SystemInfo = "Input correct data";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var UserIdClaim = HttpContext.User.FindFirst("UId");
                var userId = UserIdClaim!.Value;
                var result = Guid.TryParse(userId, out Guid guid_id);
                if (result)
                {
                   await _mediator.Send(new ChangeUserStatusByIdCommand() { UserId = guid_id, State = 1 });
                }

                await HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserLoginPreview()
        {
            if (User.Identities.Any(identity => identity.IsAuthenticated))
            {
                var sUserId = User.FindFirst("UId");

                if (sUserId == null)
                {
                    return BadRequest();
                }

                try
                {
                    if (sUserId != null)
                    {
                            if (Guid.TryParse(sUserId!.Value, out Guid userId))
                            {
                                var user = await _mediator.Send(new GetUserByIdQuery() { UserId = userId });
                                var roles = User.FindAll(ClaimsIdentity.DefaultRoleClaimType).Select(c => c.Value).ToList();
                                if (user != null && roles != null)
                                {
                                    string fName = user.Name + " " + user.Surname;
                                    UserDataModel model = new()
                                    {
                                        Email = user.Email,
                                        FullName = fName,
                                        RoleNames = roles
                                    };
                                    return View(model);
                                }
                            }                        
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                }
            }

            return View();
        }
        private async Task Authenticate(string email, Guid userId)
        {
            string isfullBlocked = "false";
            try
            {
                var dto = await _mediator.Send(new GetUserByIdQuery() { UserId = userId });
                var roleList = await _mediator.Send(new GetRoleListByUserIdQuery() { UserId = userId });

                if (dto!=null && dto.Email != null && roleList != null) // && dto.Username != null
                {
                    if (dto.IsFullBlocked == true)
                        isfullBlocked = "true";


                    string id = dto.Id.ToString();
                    var claims = new List<Claim>()
            {
                //new Claim(ClaimsIdentity.DefaultNameClaimType, dto.Username),
                new Claim(ClaimTypes.Email, dto.Email),
                new Claim("UId",id),
                new Claim("FullBlocked", isfullBlocked)
            };
                    foreach (var role in roleList)
                        if (role.Name != null)
                            claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name));

                    var identity = new ClaimsIdentity(claims,
                            "ApplicationCookie",
                            ClaimsIdentity.DefaultNameClaimType,
                            ClaimsIdentity.DefaultRoleClaimType);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity));
                    await _mediator.Send(new ChangeUserStatusByIdCommand() { UserId = dto.Id, State = 0 });
                }
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
            }

        }
    }
}
