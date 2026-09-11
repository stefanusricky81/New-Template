using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(e_Recruitment.Startup))]
namespace e_Recruitment
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
