using System;
using LiveThinkCode.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(LiveThinkCode.Areas.Identity.IdentityHostingStartup))]
namespace LiveThinkCode.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public IdentityHostingStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<LiveThinkCodeIdentityDbContext>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new System.Version(8, 0, 21))));


                // services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //    .AddEntityFrameworkStores<LiveThinkCodeIdentityDbContext>();
            });
        }
    }
}