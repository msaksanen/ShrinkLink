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
    public class GetLinkByHashQueryHandler : IRequestHandler<GetLinkByHashQuery, LinkDto?>
    {
        private readonly ShrinkLinkContext _context;
        private readonly IMapper _mapper;
        public GetLinkByHashQueryHandler(ShrinkLinkContext context, IMapper mapper)
        {
          _context = context;
          _mapper = mapper;
        }
        public async Task<LinkDto?> Handle(GetLinkByHashQuery request,
                     CancellationToken cts)
        {
            if (_context.Links != null && _context.Links.Any())
            {
                var entity = await _context.Links.AsNoTracking()
                   .FirstOrDefaultAsync(entity => entity.Hash!=null && entity.Hash.Equals(request.Hash), cts);

                if (entity == null) return null;

                var dto = _mapper.Map<LinkDto>(entity);
            }
            return null;
        }
    }
}
