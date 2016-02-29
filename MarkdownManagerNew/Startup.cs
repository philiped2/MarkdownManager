using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MarkdownManagerNew.Startup))]
namespace MarkdownManagerNew
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
