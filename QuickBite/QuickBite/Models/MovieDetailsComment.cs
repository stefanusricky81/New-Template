using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class MovieDetailsComment
    {
            public string Response { get; set; }
            public static List<MovieDetailsComment> PostComment(string userid, string movieid, string comments)
            {
                List<MovieDetailsComment> GroupResponse = null;
                string strSql = "Insert into [QuickBite].[dbo].[Comment]([userID],[movieID],[comments],[datecreated]) Values (@userID,@movieID,@comments,getdate())";

                try
                {
                    using (SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                    {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand(strSql, cn);
                        cmd.Parameters.Add(new SqlParameter("userID", userid));
                        cmd.Parameters.Add(new SqlParameter("movieID", movieid));
                        cmd.Parameters.Add(new SqlParameter("comments", comments));
                        cmd.ExecuteScalar();

                        GroupResponse = new List<MovieDetailsComment>();
                        MovieDetailsComment details = new MovieDetailsComment();
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