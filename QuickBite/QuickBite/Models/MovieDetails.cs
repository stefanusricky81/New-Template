using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class MovieDetails
    {
        public string title { get; set; }
        public string review { get; set; }
        public string url { get; set; }
        public string screenShotOne { get; set; }
        public string screenShotTwo { get; set; }
        public string screenShotThree { get; set; }
        public string screenShotFour { get; set; }
        public string screenShotFive { get; set; }
        public string screenShotSix { get; set; }
        public string screenShotSeven { get; set; }
        public string screenShotEight { get; set; }
        public string screenShotNine { get; set; }
        public string screenShotTen { get; set; }
        public string crew { get; set; }
        public string name { get; set; }
        public string thumbnail { get; set; }
        public string thumbnailsCast { get; set; }
        public int CastId { get; set; }
        public string GenreTitle { get; set; }
        public string releasedate { get; set; }
        public int commentcount { get; set; }
        public int movierate { get; set; }

        public int Likecount { get; set; }
        public bool UserLikeOrNot { get; set; }
        public string Duration { get; set; }

        public static List<MovieDetails> getMovies(string id, string userid)
        {
            List<MovieDetails> Login = null;
            string strSql = "select D.title,D.thumbnail,D.review,D.url,D.[screenShotOne],D.[screenShotTwo],D.[screenShotThree],D.[screenShotFour],";
	        strSql = strSql +"D.[screenShotFive] ,D.[screenShotSix] ,D.[screenShotSeven] ,D.[screenShotEight],D.[screenShotNine],D.[screenShotTen], D.crew,C.name,C.thumbnail,C.Id,";
            strSql = strSql + " E.title,D.relaseDate,(select count(comment.movieID) from comment where movieID=" + userid + ")as commentcount,(select count(rating.userrate) from rating where movieID=" + userid + ")as movierate,";
            strSql = strSql + "(select count(Id) from Likes where movieID=" + userid + ")as Likecount,";
            strSql = strSql + "(select isnull(statusLike,0) from Likes where userid=" + id + " and movieID=" + userid + ")as UserLikeOrNot,";
            strSql = strSql + "isnull(D.Duration,0) ";

	        strSql = strSql +" from [QuickBite].[dbo].[ActorGroups] A inner join [QuickBite].[dbo].[CastGroup] B on A.CastGroupID=B.ID inner join ";
            strSql = strSql + " [QuickBite].[dbo].[Cast] C on A.CastID=C.ID inner join [QuickBite].[dbo].[Movie] D on A.CastGroupID=D.CastGroupID inner join [QuickBite].[dbo].[Genre] E on D.genreID=E.ID  where D.ID=" + userid;
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        Login = new List<MovieDetails>();
                        MovieDetails details = new MovieDetails();
                        if (rd.Read())
                        {
                            details.title = rd.GetString(0);
                            details.thumbnail = rd.GetString(1);
                            details.review = rd.GetString(2);
                            details.url = rd.GetString(3);
                            details.screenShotOne = rd.GetString(4);
                            details.screenShotTwo = rd.GetString(5);
                            details.screenShotThree = rd.GetString(6);
                            details.screenShotFour = rd.GetString(7);
                            details.screenShotFive = rd.GetString(8);
                            details.screenShotSix = rd.GetString(9);
                            details.screenShotSeven = rd.GetString(10);
                            details.screenShotEight = rd.GetString(11);
                            details.screenShotNine = rd.GetString(12);
                            details.screenShotTen = rd.GetString(13);
                            details.crew = rd.GetString(14);
                            details.name = rd.GetString(15);
                            details.thumbnailsCast = rd.GetString(16);
                            details.CastId = rd.GetInt32(17);
                            details.GenreTitle = rd.GetString(18);
                            details.releasedate = rd.GetDateTime(19).ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("en-US"));
                            details.commentcount= rd.GetInt32(20);
                            details.movierate = rd.GetInt32(21);
                            details.Likecount= rd.GetInt32(22);
                            details.UserLikeOrNot = rd.IsDBNull(23) ? false : rd.GetBoolean(23);
                            details.Duration = rd.GetString(24);
                         }
                        Login.Add(details);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return Login;
        }
    }
}
