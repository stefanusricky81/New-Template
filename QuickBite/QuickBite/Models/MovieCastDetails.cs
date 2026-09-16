using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class MovieCastDetails 
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string thumbnail { get; set; }
        public string ThumbnailCast { get; set; }
        public string Name { get; set; }
        public string dateBirth { get; set; }
        public string birthPlace { get; set; }
        public string award { get; set; }
        public string details { get; set; }

        public static List<MovieCastDetails> GetCast(string Castid)
        {
            List<MovieCastDetails> Login = null;
            string strSql = "select C.Id,D.title,D.thumbnail,C.Thumbnail,C.Name,[dateBirth] ,[birthPlace],[award] ,[details] from ";
            strSql = strSql + " [QuickBite].[dbo].[ActorGroups] A inner join ";
            strSql = strSql + "[QuickBite].[dbo].[CastGroup] B on A.CastGroupID=B.ID inner join ";
            strSql = strSql + "[QuickBite].[dbo].[Cast] C on A.CastID=C.ID inner join ";
            strSql = strSql + " [QuickBite].[dbo].[Movie] D on A.CastGroupID=D.CastGroupID where C.ID=" + Castid;
            try
            {
                using (SqlConnection cn = new SqlConnection("x"))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        Login = new List<MovieCastDetails>();
                        MovieCastDetails details = new MovieCastDetails();
                        if (rd.Read())
                        {
                            details.Id = rd.GetInt32(0);
                            details.title = rd.GetString(1);
                            details.thumbnail = rd.GetString(2);
                            details.ThumbnailCast = rd.GetString(3);
                            details.Name = rd.GetString(4);
                            details.dateBirth = rd.GetDateTime(5).ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("en-US"));
                            details.birthPlace = rd.GetString(6);
                            details.award = rd.GetString(7);
                            details.details = rd.GetString(8);
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
