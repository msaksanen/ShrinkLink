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

namespace ShrinkLinkCQS.Links.Commands
{
    public class AddLinkIdToUserByIdCommandHandler : IRequestHandler<AddLinkIdToUserByIdCommand, int>
    {
        private readonly ShrinkLinkContext _context;
        private readonly IMapper _mapper;

        public AddLinkIdToUserByIdCommandHandler(ShrinkLinkContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> Handle(AddLinkIdToUserByIdCommand cmd, CancellationToken cts)
        {
            int result = 0;
            if (_context.Links != null && _context.Links.Any() &&
                _context.Users != null && _context.Users.Any())
            {
                var list = await _context.Links
                          .AsNoTracking()
                          .Include(r => r.Users)
                          .AsNoTracking()
                          .ToListAsync(cts);

                var userLinkList = from link in list
                                   from user in link.Users!
                                   where user.Id.Equals(cmd.UserId)
                                   select _mapper.Map<LinkDto>(link);

                if (userLinkList != null && !userLinkList.Any(r => r.Id.Equals(cmd.LinkId)))
                {
                    var link = await _context.Links
                                      .FirstOrDefaultAsync(r => r.Id.Equals(cmd.LinkId), cts);

                    var user = await _context.Users.FirstOrDefaultAsync(entity => entity.Id.Equals(cmd.UserId), cts);

                    _context.Links.Include(r => r.Users).Load();

                    if (link != null && user != null)
                    {
                        link.Users?.Add(user);
                        result = await _context.SaveChangesAsync(cts);
                    }
                }
            }
            return result;
        }
    }

}
