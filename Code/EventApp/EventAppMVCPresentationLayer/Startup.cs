using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventAppMVCPresentationLayer.Startup))]
namespace EventAppMVCPresentationLayer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
