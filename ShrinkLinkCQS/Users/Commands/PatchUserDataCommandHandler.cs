using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShrinkLinkDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Users.Commands
{
    internal class PatchUserDataCommandHandler : IRequestHandler<PatchUserDataCommand, int?>
    {
        private readonly ShrinkLinkContext _context;
        private readonly IMapper _mapper;

        public PatchUserDataCommandHandler(ShrinkLinkContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int?> Handle(PatchUserDataCommand cmd, CancellationToken cts)
        {
            if (_context.Users != null && _context.Users.Any())
            {
                var sourceUser = await _context.Users.FirstOrDefaultAsync(entity => entity.Id.Equals(cmd.UserId));
                if (sourceUser != null && cmd.patchData!=null)
                {
                    Dictionary<string, object?> nameValuePropertiesPairs = new Dictionary<string, object?>();
                    foreach (var patch in cmd.patchData)
                    {
                        if (patch.PropertyName != null)
                            nameValuePropertiesPairs.Add(patch.PropertyName, patch.PropertyValue);
                    }
                    var dbEntityEntry = _context.Users.Entry(sourceUser);
                    dbEntityEntry.CurrentValues.SetValues(nameValuePropertiesPairs);
                    dbEntityEntry.State = EntityState.Modified;
                }
            }
            var res = await _context.SaveChangesAsync(cts);
            return res;
        }
    }
}
