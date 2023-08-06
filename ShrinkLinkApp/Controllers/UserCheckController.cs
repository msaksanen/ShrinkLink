using Microsoft.AspNetCore.Mvc;
using ShrinkLinkApp.Helpers;
using ShrinkLinkCore.DataTransferObjects;

namespace ShrinkLinkApp.Controllers
{
    public class UserCheckController : Controller
    {
        private readonly EmailChecker _emailChecker;
        private readonly BirthDateChecker _birthDateChecker;
        private readonly ExpDateChecker _expDateChecker;

        public UserCheckController(EmailChecker emailChecker, BirthDateChecker birthDateChecker, ExpDateChecker expDateChecker)
        {
            _emailChecker = emailChecker;
            _birthDateChecker = birthDateChecker;
            _expDateChecker = expDateChecker;   
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            return Json(await _emailChecker.CheckEmail(email.ToLower(), HttpContext));
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckBirthDate(DateTime? birthdate)
        {

            var result = _birthDateChecker.Check(birthdate);
            if (result == 1)
                return Json(true);
            if (result == 2)
                return Json("Registration is for adults only");

            return Json("Input correct date of birth");

            // return Json(_birthDateChecker.Check(birthdate));
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckExpDate(DateTime? expDateTime)
        {
            var result = _expDateChecker.Check(expDateTime, HttpContext);
            if (result == 1)
                return Json(true);
            if (result == 2)
                return Json("The expiration time of a short URL is no more than 1 year");
            if (result == -1)
                return Json("The expiration time cannot be in the past or now ");
           
            return Json("Input correct expiration time for a short URL");
        }
    }
}
