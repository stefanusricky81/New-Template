using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class MovieDetailsAddWish
    {
        public string Response { get; set; }
        public static List<MovieDetailsAddWish> AddWish(string userid, string FavId, string FavTypes)
        {
            List<MovieDetailsAddWish> GroupResponse = null;
            string strSql = "Insert into [QuickBite].[dbo].[Wishlist]([UserName],[WishId],[WishType],[datecreated],[status]) Values (@UserName,@FavId,@FavType,getdate(),1)";

            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    cmd.Parameters.Add(new SqlParameter("UserName", userid));
                    cmd.Parameters.Add(new SqlParameter("FavId", FavId));
                    cmd.Parameters.Add(new SqlParameter("FavType", FavTypes));
                    cmd.ExecuteScalar();
                    MovieDetailsAddWish details = new MovieDetailsAddWish();
                    GroupResponse = new List<MovieDetailsAddWish>();
                    details.Response = "success";
                    GroupResponse.Add(details);
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return GroupResponse;
        }
    }
}