using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;

namespace QuickBite.Controllers
{
    [BasicHttpAuthorizeAttribute(RequireAuthentication = false)]
   public class LoginForgotPasswordController : ApiController
    {
        [AcceptVerbs("GET", "POST")]

        public IEnumerable<LoginForgotPassword> ResetPassword(string username)
       {
           return Validate(username);
       }
        public List<LoginForgotPassword> Validate(string username)
        {
            List<LoginForgotPassword> details = LoginForgotPassword.ResetPassword(username);

            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
