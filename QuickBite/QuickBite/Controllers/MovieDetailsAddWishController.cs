using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;
namespace QuickBite.Controllers
{
    public class MovieDetailsAddWishController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<MovieDetailsAddWish> AddWish(string userid, string FavId, string FavType)
        {
            return Validate(userid, FavId, FavType);
        }

        public List<MovieDetailsAddWish> Validate(string userid, string FavId, string FavType)
        {
            List<MovieDetailsAddWish> details = MovieDetailsAddWish.AddWish(userid, FavId, FavType);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
