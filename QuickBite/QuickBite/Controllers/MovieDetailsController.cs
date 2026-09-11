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
    public class MovieDetailsController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<MovieDetails> getMovies(string id, string userid)
        {
            return Validate(id,userid);
        }

        public List<MovieDetails> Validate(string id, string userid)
        {
            List<MovieDetails> details = MovieDetails.getMovies(id,userid);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
