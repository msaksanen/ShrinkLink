using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShrinkLinkCore.DataTransferObjects;
using ShrinkLinkDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Users.Queries
{
    public class GetUserDtoByEmailPwdHashQueryHandler : IRequestHandler<GetUserDtoByEmailPwdHashQuery, UserDto?>
    {
        private readonly ShrinkLinkContext _context;
        private readonly IMapper _mapper;

        public GetUserDtoByEmailPwdHashQueryHandler(ShrinkLinkContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto?> Handle(GetUserDtoByEmailPwdHashQuery request,
            CancellationToken cts)

        {

            if (_context.Users != null && _context.Users.Any())
            {
                var user = await _context.Users
                          .FirstOrDefaultAsync(u => u.Email != null && u.Email.Equals(request.Email)
                          && u.PasswordHash != null && u.PasswordHash.Equals(request.PasswordHash));
                return _mapper.Map<UserDto>(user);
            }

            return null;
        }
    }
}
