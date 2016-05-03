using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(e_tms.Web.Startup))]
namespace e_tms.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
