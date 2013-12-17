using RadarBAL.Security;
using System;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
public class BasicAuthenticationMessageHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authHeader = request.Headers.Authorization;

        if (authHeader == null)
            return base.SendAsync(request, cancellationToken);

        if (authHeader.Scheme != "Basic")
            return base.SendAsync(request, cancellationToken);

        var encodedUserPass = authHeader.Parameter.Trim();
        var userPass = Encoding.ASCII.GetString(Convert.FromBase64String(encodedUserPass));
        var parts = userPass.Split(":".ToCharArray());
        var email = parts[0];
        var password = parts[1];

        if (!Membership.ValidateUser(email, password))
            return base.SendAsync(request, cancellationToken);

        var i = new RadarIdentity(email, "Basic");
        //var identity = new GenericIdentity(username, "Basic");
        string[] roles = Roles.Provider.GetRolesForUser(email);
        var p = new RadarPrincipal(i);
        //var principal = new GenericPrincipal(i, roles);
        Thread.CurrentPrincipal = p;

        if (HttpContext.Current != null)
            HttpContext.Current.User = p;

        return base.SendAsync(request, cancellationToken);
    }
}