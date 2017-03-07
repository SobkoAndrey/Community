using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Community3.Startup))]
namespace Community3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
