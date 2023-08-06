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
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly ShrinkLinkContext _context;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(ShrinkLinkContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request,
        CancellationToken cts)

        {
            if (_context.Users != null && _context.Users.Any())
            {
                var entity = await _context.Users.AsNoTracking()
                             .FirstOrDefaultAsync(entity => entity.Id.Equals(request.UserId), cts);

                if (entity == null) return null;

                var dto = _mapper.Map<UserDto>(entity);
                return dto;
            }
            return null;
        }
    }
}
