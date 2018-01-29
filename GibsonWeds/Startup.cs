using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GibsonWeds.Startup))]
namespace GibsonWeds
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
