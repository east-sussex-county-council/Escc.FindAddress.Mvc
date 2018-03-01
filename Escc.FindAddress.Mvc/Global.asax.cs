// Build this file in debug only as it's for local testing, but we don't want it to be included in the NuGet package
#if DEBUG
using Escc.FindAddress.Mvc.Test;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Escc.FindAddress.Mvc
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new
                {
                    controller = "Test",
                    action = "Index"
                }
            );

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
#endif