using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(perfDemo.Startup))]
namespace perfDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
