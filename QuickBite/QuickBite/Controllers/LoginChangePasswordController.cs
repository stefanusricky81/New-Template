using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;


namespace QuickBite.Controllers
{
    public class LoginChangePasswordController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<LoginChangePassword> ChangeUserPassword(string username, string oldpassword, string newpassword)
        {
            return UpdatePassword(username, oldpassword, newpassword);
        }
        public List<LoginChangePassword> UpdatePassword(string username, string oldpassword, string newpassword)
        {
            List<LoginChangePassword> details = LoginChangePassword.UpdatePassword(username, oldpassword, newpassword);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
