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
    public class LoginRegistrationController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<LoginRegistration> RegisterUser(string firstname, string email, string password, string regtype)
        {
            return AddUser(firstname, email, password, regtype);
        }
        public List<LoginRegistration> AddUser(string firstname, string email, string password, string regtype)
        {
            List<LoginRegistration> details = LoginRegistration.RegisterUser(firstname, email, password, regtype);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
