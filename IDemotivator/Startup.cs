using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IDemotivator.Startup))]
namespace IDemotivator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
