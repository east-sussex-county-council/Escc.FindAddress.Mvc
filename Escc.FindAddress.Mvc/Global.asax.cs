#if DEBUG 
using System;
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
        }
    }
}
#endif