using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class ProfileFavMovies
    {
        public string Thumbnails { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public static List<ProfileFavMovies> getFav(string emails,string type)
        {
            List<ProfileFavMovies> Banner = null;
             
            string col;
            if (type == "Movie" || type == "MOVIE" || type == "movie")
                col = "b.title";
            else
                col = "b.name";

            string strSql = " select thumbnail,B.ID," + col + " from [QuickBite].[dbo].[Favourite] A inner join [QuickBite].[dbo].[" + type + "] B on A.FavId=B.ID where A.FavType=@table and A.status='true' and A.UserName=@users";
            try
            {
                using (SqlConnection cn = new SqlConnection("x"))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSql, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("table", SqlDbType.NVarChar).Value = type;
                        cmd.Parameters.Add("users", SqlDbType.NVarChar).Value = emails;
                        string status = (string)cmd.ExecuteScalar();

                        Banner = new List<ProfileFavMovies>();
                      

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                ProfileFavMovies details = new ProfileFavMovies();
                                details.Thumbnails = rd.GetString(0);
                                details.ID = rd.GetInt32(1);
                                details.Name = rd.GetString(2);
                                Banner.Add(details);
                            }
                        }

                        ProfileFavMovies detailsa = new ProfileFavMovies();
                        if (status == null){
                            Banner = null;
                            detailsa.Thumbnails = "No Data";
                            detailsa.ID = 0;
                         Banner.Add(detailsa);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return Banner;
        }
    }
}
