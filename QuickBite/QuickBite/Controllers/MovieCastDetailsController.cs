using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;
namespace QuickBite.Controllers
{
    public class MovieCastDetailsController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<MovieCastDetails> GetCast(string Castid)
        {
            return Validate(Castid);
        }

        public List<MovieCastDetails> Validate(string Castid)
        {
            List<MovieCastDetails> details = MovieCastDetails.GetCast(Castid);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
