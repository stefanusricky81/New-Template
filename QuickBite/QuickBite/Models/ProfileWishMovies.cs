using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class ProfileWishMovies
    {
        public string relaseDate { get; set; }
        public string Thumbnails { get; set; }
        public string title { get; set; }
        public string genre { get; set; }
        public int userRate { get; set; }
        public int ID { get; set; }

        public static List<ProfileWishMovies> getWish(int emails, string type)
        {
            List<ProfileWishMovies> Banner = null;

            string strSql = "  select B.ID, B.title,B.thumbnail,B.[relaseDate],C.title as genre, userRate from [QuickBite].[dbo].[Wishlist] A inner join [QuickBite].[dbo].[Movie] B  on A.WishID=B.ID inner join [QuickBite].[dbo].[Genre] C on B.genreID = C.ID inner join [QuickBite].[dbo].[Rating] D on A.UserName =D.UserID where A.WishType='Wish' and A.Status='true' and D.UserID=@users ";
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSql, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("users", SqlDbType.Int).Value = emails;
                        int status = (int)cmd.ExecuteScalar();

                        Banner = new List<ProfileWishMovies>();
                       // ProfileWishMovies details = new ProfileWishMovies();
                        ProfileWishMovies details1 = new ProfileWishMovies();
                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                ProfileWishMovies details = new ProfileWishMovies();
                                details.ID = rd.GetInt32(0);
                                details.Thumbnails = rd.GetString(1);
                                details.title = rd.GetString(2);
                                details.relaseDate = rd.GetDateTime(3).ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("en-US"));
                                details.genre = rd.GetString(4);
                                details.userRate = rd.GetInt32(5);
                                Banner.Add(details);
                            } 
                        }

                        if (status == 0)
                            {
                                details1.Thumbnails = "No Data";
                                details1.title = "No Data";
                                Banner.Add(details1);
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