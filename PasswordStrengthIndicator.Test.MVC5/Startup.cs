using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PasswordStrengthIndicator.Test.MVC5.Startup))]
namespace PasswordStrengthIndicator.Test.MVC5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
