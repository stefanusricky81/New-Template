using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;

namespace QuickBite.Controllers
{
    public class MovieRateController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<MovieRate> PostRate(string movieid, int userrate, int userid)
        {
            return getFavByType(movieid, userrate, userid);
        }

        public List<MovieRate> getFavByType(string movieid, int userrate, int userid)
        {
            List<MovieRate> details = MovieRate.PostRate(movieid, userrate, userid);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
