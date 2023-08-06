using MediatR;
using ShrinkLinkCQS.Users.Queries;
using System.Security.Claims;

namespace ShrinkLinkApp.Helpers
{

    public class EmailChecker
    {
        private readonly IMediator _mediator;

        public EmailChecker(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> CheckEmail(string email, HttpContext context)
        {
            var curEmail = context.User.FindFirst(ClaimTypes.Email)?.Value;
            if (curEmail != null && curEmail.Equals(email))
                return true;

            var res = await _mediator.Send(new CheckUserEmailQuery() { Email = email });
            if (res == false || res==null)
                return true;

            return false;
        }
    }
}
