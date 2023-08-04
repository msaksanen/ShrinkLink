using AutoMapper;
using MediatR;
using ShrinkLinkDb.Entities;
using ShrinkLinkDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShrinkLinkCQS.Links.Commands
{
    public class PatchLinkCommandHandler : IRequestHandler<PatchLinkCommand, int?>
    {

        private readonly ShrinkLinkContext _context;
        private readonly IMapper _mapper;
        public PatchLinkCommandHandler(ShrinkLinkContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int?> Handle(PatchLinkCommand request,
                     CancellationToken cts)
        {
            if (_context.Links != null && _context.Links.Any())
            {
                var entity = await _context.Links
                   .FirstOrDefaultAsync(entity => entity.Id.Equals(request.Id), cts);

                if (entity == null) return null;

                var dbEntityEntry = _context.Links.Entry(entity);
                dbEntityEntry.CurrentValues.SetValues(request.nameValuePropertiesPairs);
                dbEntityEntry.State = EntityState.Modified;

                var res = await _context.SaveChangesAsync(cts);

                return res;

            }
            return null;
        }
    }
}