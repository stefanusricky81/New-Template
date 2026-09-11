using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;
namespace QuickBite.Controllers
{
    public class MovieDetailsAddFavController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<MovieDetailsAddFav> AddFav(int userid, string FavId, string FavType)
        {
            return Validate(userid, FavId, FavType);
        }

        public List<MovieDetailsAddFav> Validate(int userid, string FavId, string FavType)
        {
            List<MovieDetailsAddFav> details = MovieDetailsAddFav.AddFav(userid, FavId, FavType);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
