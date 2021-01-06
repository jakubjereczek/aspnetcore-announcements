using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnnouncementsApp.Models
{
    public class AnnouncementContext : DbContext {
        public AnnouncementContext(DbContextOptions<AnnouncementContext> options) : base(options)
        {

        }

        public DbSet<Announcement> Announcement { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announcement>().HasKey(c => new
            {
                c.AnnouncementId
            });
        }

    }

}
