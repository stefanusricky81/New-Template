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
    public class NotificationPushAddController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
          public IEnumerable<NotificationPushAdd> ChangeUsers(string username, string email, string type)
        {
            return ChangeUser(username, email, type);
        }
        public List<NotificationPushAdd> ChangeUser(string username, string email, string type)
        {
            List<NotificationPushAdd> details = NotificationPushAdd.ChangeUsers(username, email, type);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
