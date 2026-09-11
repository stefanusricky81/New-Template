using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class MovieDetailsAddFav
    {
        public string Response { get; set; }
        public static List<MovieDetailsAddFav> AddFav(int userid, string movid, string comments)
        {
            List<MovieDetailsAddFav> GroupResponse = null;
            string strSql = "Insert into [QuickBite].[dbo].[Favourite]([UserName],[FavId],[FavType],[datecreated],[status]) Values (@UserName,@FavId,@FavType,getdate(),1)";

            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    cmd.Parameters.Add(new SqlParameter("UserName", userid));
                    cmd.Parameters.Add(new SqlParameter("FavId", movid));
                    cmd.Parameters.Add(new SqlParameter("FavType", comments));
                    cmd.ExecuteScalar();
                    MovieDetailsAddFav details = new MovieDetailsAddFav();
                    GroupResponse = new List<MovieDetailsAddFav>();
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