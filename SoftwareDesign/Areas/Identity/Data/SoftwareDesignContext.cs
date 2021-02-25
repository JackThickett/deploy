using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftwareDesign.Models;

namespace SoftwareDesign.Data
{
    public class SoftwareDesignContext : IdentityDbContext<IdentityUser>
    {
        public SoftwareDesignContext(DbContextOptions<SoftwareDesignContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Quote>().ToTable("QuoteRequest");
        }
        public DbSet<Quote> QuoteRequests { get; set; }
    }
}
