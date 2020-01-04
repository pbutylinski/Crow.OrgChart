using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;

namespace Crow.OrgChart
{
    public sealed class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        public string BasicRealm { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public BasicAuthenticationAttribute(IConfiguration config)
        {
            this.Username = config.GetValue<string>("SecurityUsername");
            this.Password = config.GetValue<string>("SecurityPassword");
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (string.IsNullOrEmpty(this.Username) && string.IsNullOrEmpty(this.Password))
            {
                return;
            }

            var req = filterContext.HttpContext.Request;
            var auth = req.Headers["Authorization"];

            if (!string.IsNullOrEmpty(auth))
            {
                var password = Convert.FromBase64String(auth.ToString().Substring(6));
                var cred = Encoding.ASCII.GetString(password).Split(':');
                var user = new { Name = cred[0], Pass = cred[1] };

                if (user.Name == this.Username && user.Pass == this.Password)
                {
                    return;
                }
            }

            filterContext.HttpContext.Response.Headers.Add("WWW-Authenticate", String.Format("Basic realm=\"{0}\"", BasicRealm ?? "Crow"));
            filterContext.Result = new UnauthorizedResult();
        }
    }
}
