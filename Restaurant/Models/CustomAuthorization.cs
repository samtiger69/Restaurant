using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Restaurant.Models
{
    public class CustomAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            //var v = HttpContext.Current.User.Identity.Name;
        }

    }
}