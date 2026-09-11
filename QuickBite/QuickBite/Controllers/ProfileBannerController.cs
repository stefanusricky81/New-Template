using QuickBite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QuickBite.Controllers
{
    public class ProfileBannerController : ApiController
    {
        public IEnumerable<ProfileBanner> GetAllBanner()
        {
            return GetBannerByType();
        }

        public List<ProfileBanner> GetBannerByType()
        {
            List<ProfileBanner> details = ProfileBanner.AllDetails();
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
