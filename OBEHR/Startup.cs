using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OBEHR.Startup))]
namespace OBEHR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
