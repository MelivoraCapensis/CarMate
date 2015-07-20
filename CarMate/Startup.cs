using System;
using System.Threading.Tasks;
using CarMate.Classes;
using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CarMate.Startup))]

namespace CarMate
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            //app.UseHangfireDashboard();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                AuthorizationFilters = new[] {new MyRestrictiveAuthorizationFilter()}
            });
            app.UseHangfireServer();

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
