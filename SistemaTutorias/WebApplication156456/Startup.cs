using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApplication156456.Startup))]
namespace WebApplication156456
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
