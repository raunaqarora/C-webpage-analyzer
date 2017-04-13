using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebappClient.Startup))]
namespace WebappClient
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
