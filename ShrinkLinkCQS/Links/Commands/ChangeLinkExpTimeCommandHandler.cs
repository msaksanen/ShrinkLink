using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShrinkLinkCore.DataTransferObjects;
using ShrinkLinkCQS.Links.Queries;
using ShrinkLinkDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Links.Commands
{
    public class ChangeLinkExpTimeCommandHandler:IRequestHandler <ChangeLinkExpTimeCommand, int?>
    {

        private readonly ShrinkLinkContext _context;
        private readonly IMapper _mapper;
        public ChangeLinkExpTimeCommandHandler (ShrinkLinkContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int?> Handle(ChangeLinkExpTimeCommand request,
                     CancellationToken cts)
        {
            if (_context.Links != null && _context.Links.Any())
            {
                var entity = await _context.Links
                   .FirstOrDefaultAsync(entity => entity.Id.Equals(request.Id), cts);

                if (entity == null) return null;

                entity.ExpirationDate = request.ExpTime;
                var res = await _context.SaveChangesAsync(cts);

                return res;
        
            }
            return null;
        }
    }
}
