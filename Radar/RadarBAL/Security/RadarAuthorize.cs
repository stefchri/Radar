using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;

namespace RadarBAL.Security
{
    class RadarAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext);
        }
        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            string urlOriginal = HttpContext.Current.Request.Url.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped);
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(urlOriginal);
            string urlEncoded = Convert.ToBase64String(byt);

            var values = new RouteValueDictionary(new
            {
                action = "Login",
                controller = "Account",
                returnUrl = urlEncoded
            });
            filterContext.Result = new RedirectToRouteResult(values);
        }
    }
}
