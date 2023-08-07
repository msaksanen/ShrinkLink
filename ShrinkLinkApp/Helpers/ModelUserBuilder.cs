using AutoMapper;
using MediatR;
using ShrinkLinkApp.Models;
using ShrinkLinkCQS.Users.Queries;
using System.Security.Claims;

namespace ShrinkLinkApp.Helpers
{
    public class ModelUserBuilder
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public ModelUserBuilder(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
    
        }

        internal async Task<UserModel?> BuildById(HttpContext context, string? id = "")
        {
            string userId;
            if (string.IsNullOrEmpty(id))
            {
                var UserIdClaim = context.User.FindFirst("UId");
                userId = UserIdClaim!.Value;
            }
            else
                userId = id;

            var result = Guid.TryParse(userId, out Guid guid_id);

            if (result)
            {
                var usr = await _mediator.Send(new GetUserByIdQuery() { UserId = guid_id });
                if (usr != null)
                {
                    var baseUserModel = _mapper.Map<UserModel>(usr);
                    var roles = context.User.FindAll(ClaimsIdentity.DefaultRoleClaimType).Select(c => c.Value).ToList();
                    if (roles != null) baseUserModel.RoleNames = roles;

                    return baseUserModel;
                }
            }
            return null;
        }

    }
}
