using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;
namespace QuickBite.Controllers
{
    public class ProfileEditNameController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<ProfileEditName> SetName(int userid, string name)
        {
            return getFavByType(userid,name);
        }

        public List<ProfileEditName> getFavByType(int userid,string name)
        {
            List<ProfileEditName> details = ProfileEditName.SetName(userid,name);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
