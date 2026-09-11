using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;

namespace QuickBite.Controllers
{
    public class MovieGetCommentController : ApiController
    {
        public IEnumerable<MovieGetComment> GetComment(string id)
        {
            return GetMovie(id);
        }

        public List<MovieGetComment> GetMovie(string id)
        {
            List<MovieGetComment> details = MovieGetComment.GetComment(id);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
