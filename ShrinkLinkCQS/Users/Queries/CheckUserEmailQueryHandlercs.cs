using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShrinkLinkDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Users.Queries
{
    public class CheckUserEmailQueryHandler : IRequestHandler<CheckUserEmailQuery, bool?>
    {
        private readonly ShrinkLinkContext _context;
        private readonly IMapper _mapper;

        public CheckUserEmailQueryHandler(ShrinkLinkContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool?> Handle(CheckUserEmailQuery request,
            CancellationToken cts)

        {
            if (_context.Users != null && _context.Users.Any())
            {
                return await _context.Users.AnyAsync
                                  (user => user.Email != null && user.Email.Equals(request.Email));

            }
            return null;
        }
    }
}


