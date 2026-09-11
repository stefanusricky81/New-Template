using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;
namespace QuickBite.Controllers
{
    public class SearchAllController : ApiController
    {
        public IEnumerable<SearchAll> getAll(string MovTitle, string year, string genre)
        {
            return getFavByType(MovTitle, year,genre);
        }

        public List<SearchAll> getFavByType(string MovTitle, string year, string genre)
        {
            List<SearchAll> details = SearchAll.getAll(MovTitle, year, genre);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
           
        }
    }
}
