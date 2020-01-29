using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WatsonTracker.Startup))]
namespace WatsonTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
