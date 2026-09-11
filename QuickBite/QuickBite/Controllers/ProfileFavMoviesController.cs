using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;

namespace QuickBite.Controllers
{
    public class ProfileFavMoviesController : ApiController
    {
        public IEnumerable<ProfileFavMovies> getFav(string emails, string type)
        {
            return getFavByType( emails, type);
        }

        public List<ProfileFavMovies> getFavByType(string emails,string type)
        {
            List<ProfileFavMovies> details = ProfileFavMovies.getFav(emails, type);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
