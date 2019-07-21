using System.Web.Http;

namespace UI.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
