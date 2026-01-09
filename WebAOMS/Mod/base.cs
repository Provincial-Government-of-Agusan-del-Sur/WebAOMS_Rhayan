using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebAOMS.Models;
using System.IO;
namespace WebAOMS.Mod
{
   public class AuthorizeAdminOrOwnerOfPostAttribute : AuthorizeAttribute
{
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // The user is not authenticated
                base.HandleUnauthorizedRequest(filterContext);
            }
            else if (!this.Roles.Split(',').Any(filterContext.HttpContext.User.IsInRole))
            {
                // The user is not in any of the listed roles => 
                // show the unauthorized view
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Unauthorized.cshtml"
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
        var authorized = base.AuthorizeCore(httpContext);
        if (!authorized)
        {
            // The user is not authenticated
            return false;
        }

        var user = httpContext.User;
        if (user.IsInRole("Admin") || user.IsInRole("Controller"))
        {
            // Administrator => let him in
            return true;
        }


        var rd = httpContext.Request.RequestContext.RouteData;
        var id = rd.Values["id"] as string;
        if (string.IsNullOrEmpty(id))
        {
            // No id was specified => we do not allow access
            return false;
        }

        return IsOwnerOfPost(user.Identity.Name, id);
    }

    private bool IsOwnerOfPost(string username, string postId)
    {
        // TODO: you know what to do here
        throw new NotImplementedException();
    }
}
}