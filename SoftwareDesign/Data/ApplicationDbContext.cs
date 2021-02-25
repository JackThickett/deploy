using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftwareDesign.Models;

namespace SoftwareDesign.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Quote>().ToTable("Quote");
            builder.Entity<Contract>().ToTable("Contract");
            builder.Entity<QuoteLog>().ToTable("QuoteLog");
        }

        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<QuoteLog> QuotesLog { get; set; }
    }
}
