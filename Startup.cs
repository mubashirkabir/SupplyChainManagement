using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SupplyChainManagement.Startup))]
namespace SupplyChainManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
