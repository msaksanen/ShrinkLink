using Microsoft.EntityFrameworkCore;
using ShrinkLinkDb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkDb
{
    public class ShrinkLinkContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Link>? Links { get; set; }

        public ShrinkLinkContext(DbContextOptions<ShrinkLinkContext> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>().HasIndex(l => l.ShortId);
            modelBuilder.Entity<Link>().HasIndex(l => l.Hash);
            modelBuilder.Entity<Link>()
               .HasMany(l => l.Users)
               .WithMany(u => u.Links)
               .UsingEntity(
                     x => x.HasOne(typeof(User)).WithMany().OnDelete(DeleteBehavior.NoAction),
                     z => z.HasOne(typeof(Link)).WithMany().OnDelete(DeleteBehavior.NoAction));

        }
       
    }
}
