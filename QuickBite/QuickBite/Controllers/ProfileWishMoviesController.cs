using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;
namespace QuickBite.Controllers
{
    public class ProfileWishMoviesController : ApiController
    {
        public IEnumerable<ProfileWishMovies> getWish(int emails, string type)
        {
            return getFavByType(emails, type);
        }

        public List<ProfileWishMovies> getFavByType(int emails, string type)
        {
            List<ProfileWishMovies> details = ProfileWishMovies.getWish(emails, type);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
