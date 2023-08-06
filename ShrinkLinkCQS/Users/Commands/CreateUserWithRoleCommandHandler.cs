﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShrinkLinkDb;
using ShrinkLinkDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Users.Commands
{
    public class CreateUserWithRoleCommandHandler : IRequestHandler<CreateUserWithRoleCommand, int?>
    {
        private readonly ShrinkLinkContext _context;
        private readonly IMapper _mapper;

        public CreateUserWithRoleCommandHandler(ShrinkLinkContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int?> Handle(CreateUserWithRoleCommand cmd, CancellationToken cts)
        {
            if (_context.Roles != null && _context.Roles.Any() && _context.Users != null)
            {
                var entity = _mapper.Map<User>(cmd.UserDto);
                entity.IsFullBlocked = false;
                var role = await _context.Roles
                           .FirstOrDefaultAsync(role => role.Name != null && role.Name.Equals(cmd.RoleName));

                if (entity != null && role != null)
                {
                    entity.Roles?.Add(role);
                    await _context.Users.AddAsync(entity, cts);
                }
            }
            var res = await _context.SaveChangesAsync(cts);
            return res;
        }
    }
}