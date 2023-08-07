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

namespace ShrinkLinkCQS.Links.Queries
{
    public class GetLinksByUserIdQueryHandler : IRequestHandler<GetLinksByUserIdQuery, IEnumerable<LinkDto>?>
    {
        private readonly ShrinkLinkContext _context;
        private readonly IMapper _mapper;
        public GetLinksByUserIdQueryHandler(ShrinkLinkContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<LinkDto>?> Handle(GetLinksByUserIdQuery request,
                     CancellationToken cts)
        {
            if (_context.Links != null && _context.Links.Any() && _context.Users!=null && _context.Users.Any())
            {
                var list = await _context.Links
                          .AsNoTracking()
                          .Include(r => r.Users)
                          .AsNoTracking()
                          .ToListAsync();

                var lst = from link in list
                          from user in link.Users!
                          where user.Id.Equals(request.UserId)
                          select _mapper.Map<LinkDto>(link);
                return lst;
            }
            return null;
        }
    }
}
