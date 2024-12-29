using System;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace WebApplication1.Models
{
    public class CustomAuthenticationFilter : ActionFilterAttribute,IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (String.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["UserName"])))
            {
                filterContext.Result=new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result != null && filterContext.Result is HttpUnauthorizedResult) {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"controller","Practice" },
                            {"action","Login" }
                        }
                    );
            }
        }
    }
}