using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sonnetly.Startup))]
namespace Sonnetly
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
