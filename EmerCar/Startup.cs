using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmerCar.Startup))]
namespace EmerCar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
