using ShrinkLinkCQS.Users.Queries;
using ShrinkLinkDb.Entities;
using System.Security.Claims;

namespace ShrinkLinkApp.Helpers
{
    public class ExpDateChecker
    {
        public  int Check(DateTime? date, HttpContext context)
        {
            if (!context.User.Identities.Any(identity => identity.IsAuthenticated))
                return 1;

            if (date <= DateTime.Now.AddYears(1) && date>=DateTime.Now.AddMinutes(1))
                return 1;

            if (date >= DateTime.Now.AddYears(1))
                return 2;

            if (date <= DateTime.Now.AddMinutes(1))
                return -1;

            return 0;
        }
    }
}
