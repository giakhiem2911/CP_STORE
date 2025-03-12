using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Project_64131000.Startup))]
namespace Project_64131000
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
