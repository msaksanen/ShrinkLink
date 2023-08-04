using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShrinkLinkCore.DataTransferObjects;
using ShrinkLinkDb;
using ShrinkLinkDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Links.Commands
{
    public class UpdateLinkCommandHandler : IRequestHandler<UpdateLinkCommand, int?>
    {

        private readonly ShrinkLinkContext _context;
        private readonly IMapper _mapper;
        public UpdateLinkCommandHandler(ShrinkLinkContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int?> Handle(UpdateLinkCommand request,
                     CancellationToken cts)
        {
            if (_context.Links != null && _context.Links.Any() && request.dto!=null)
            {
                var entity = await _context.Links.AsNoTracking()
                   .FirstOrDefaultAsync(entity => entity.Id.Equals(request.dto!.Id), cts);

                if (entity == null) return null;
                var updEnt = _mapper.Map<Link>(request.dto);
                entity = updEnt;

                var res = await _context.SaveChangesAsync(cts);

                return res;

            }
            return null;
        }
    }
}
