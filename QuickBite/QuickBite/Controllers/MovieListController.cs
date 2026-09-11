using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;

namespace QuickBite.Controllers
{
    public class MovieListController : ApiController
    {
        public IEnumerable<MovieList> GetAllMovie()
        {
            return GetMovieByType();
        }

        public List<MovieList> GetMovieByType()
        {
            List<MovieList> details = MovieList.AllDetails();
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
