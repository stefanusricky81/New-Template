using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class MovieRate
    {
        public string Response { get; set; }
        public static List<MovieRate> PostRate(string movieid, int userrate, int userid)
        {
            List<MovieRate> GroupResponse = null;
            string strSql = "Insert into [QuickBite].[dbo].[Rating]([movieID],[userRate],[userID],[datecreated]) Values (@movieid,@userrate,@userid,getdate())";

            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    cmd.Parameters.Add(new SqlParameter("movieid", movieid));
                    cmd.Parameters.Add(new SqlParameter("userrate", userrate));
                    cmd.Parameters.Add(new SqlParameter("userid", userid));
                    cmd.ExecuteScalar();
                    MovieRate details = new MovieRate();
                    GroupResponse = new List<MovieRate>();
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