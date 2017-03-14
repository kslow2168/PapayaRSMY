using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PapayaX2.Startup))]
namespace PapayaX2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
