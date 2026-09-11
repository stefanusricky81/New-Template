using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuickBite.Models;

namespace QuickBite.Controllers
{
    public class MovieDetailsCommentController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<MovieDetailsComment> PostComment(string userid, string movieid, string comments)
        {
            return Validate(userid, movieid, comments);
        }

        public List<MovieDetailsComment> Validate(string userid, string movieid, string comments)
        {
            List<MovieDetailsComment> details = MovieDetailsComment.PostComment(userid, movieid, comments);
            var detail = details.ToList();
            if (detail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return detail;
        }
    }
}
