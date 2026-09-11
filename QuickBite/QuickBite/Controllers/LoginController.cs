using QuickBite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QuickBite.Controllers
{
    [BasicHttpAuthorizeAttribute(RequireAuthentication = false)]
    public class LoginController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<Login> ValidateUser(string useremail, string password)
        {
            return Validate(useremail, password);
        }

        public List<Login> Validate(string useremail, string password)
        {
            List<Login> details = Login.ValidateUser(useremail, password);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
