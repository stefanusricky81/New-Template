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
    public class NotificationPushController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<NotificationPush> SendUsers(string message)
        {
            return SendUser(message);
        }
        public List<NotificationPush> SendUser(string message)
        {
            List<NotificationPush> details = NotificationPush.SendUsers(message);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
