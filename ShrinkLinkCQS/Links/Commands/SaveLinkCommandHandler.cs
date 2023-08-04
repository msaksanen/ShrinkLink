using AutoMapper;
using MediatR;
using ShrinkLinkDb.Entities;
using ShrinkLinkDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Links.Commands
{
    public class SaveLinkCommandHandler : IRequestHandler<SaveLinkCommand, int?>
    {

        private readonly ShrinkLinkContext _context;
        private readonly IMapper _mapper;
        public SaveLinkCommandHandler(ShrinkLinkContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int?> Handle(SaveLinkCommand request,
                     CancellationToken cts)
        {
            if (_context.Links != null && _context.Links.Any() && request.Dto != null)
            {
                var entity = _mapper.Map<Link>(request.Dto);
                await _context.AddAsync(entity, cts);
                var res = await _context.SaveChangesAsync(cts);

                return res;

            }
            return null;
        }
    }
}
