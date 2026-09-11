using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;
namespace QuickBite.Controllers
{
    public class SearchMovieController : ApiController
    {
        public IEnumerable<SearchMovie> getAll(string id, string type, string searchname)
        {
            return getFavByType(id, type, searchname);
        }

        public List<SearchMovie> getFavByType(string id, string type, string searchname)
        {
            List<SearchMovie> details = SearchMovie.getAll(id, type, searchname);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;

        }
    }
}
