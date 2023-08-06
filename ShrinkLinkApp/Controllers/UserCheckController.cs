using Microsoft.AspNetCore.Mvc;
using ShrinkLinkApp.Helpers;

namespace ShrinkLinkApp.Controllers
{
    public class UserCheckController : Controller
    {
        private readonly EmailChecker _emailChecker;
        private readonly BirthDateChecker _birthDateChecker;

        public UserCheckController(EmailChecker emailChecker, BirthDateChecker birthDateChecker)
        {
            _emailChecker = emailChecker;
            _birthDateChecker = birthDateChecker;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            return Json(await _emailChecker.CheckEmail(email.ToLower(), HttpContext));
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckBirthDate(DateTime? birthdate)
        {

            var result = _birthDateChecker.Check(birthdate, HttpContext);
            if (result == 1)
                return Json(true);
            if (result == 2)
                return Json("Registration is for adults only");

            return Json("Input correct date of birth");

            // return Json(_birthDateChecker.Check(birthdate));
        }
    }
}
