using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftwareDesign.Data;

[assembly: HostingStartup(typeof(SoftwareDesign.Areas.Identity.IdentityHostingStartup))]
namespace SoftwareDesign.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SoftwareDesignContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SoftwareDesignContextConnection")));
            });
        }
    }
}