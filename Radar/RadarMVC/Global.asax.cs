using RadarBAL.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace RadarMVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_AuthenticateRequest(object sender, EventArgs e)
        //{
        //    IPrincipal principal = HttpContext.Current.User;
        //    if (principal != null && principal.Identity.IsAuthenticated && principal.Identity.AuthenticationType == "Forms")
        //    {
        //        FormsIdentity fIndent = (FormsIdentity)principal.Identity;
        //        RadarIdentity cIndent = new RadarIdentity(fIndent.Ticket);
        //        RadarPrincipal nPrincipal = new RadarPrincipal(cIndent);
        //        HttpContext.Current.User = nPrincipal;
        //        Thread.CurrentPrincipal = nPrincipal;
        //    }
        //}
    }
}