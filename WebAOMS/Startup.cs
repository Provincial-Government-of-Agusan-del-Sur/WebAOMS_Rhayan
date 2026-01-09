using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAOMS.Startup))]
namespace WebAOMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }
    }
}
