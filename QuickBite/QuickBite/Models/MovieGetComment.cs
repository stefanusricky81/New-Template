using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class MovieGetComment
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Comments { get; set; }
        public string title { get; set; }
        public static List<MovieGetComment> GetComment(string id)
        {
            List<MovieGetComment> GroupList = null;

            string strSql = " Select B.Username,B.FullName,A.Comments,C.title  FROM [QuickBite].[dbo].[Comment] A inner join [QuickBite].[dbo].Users B on A.userID = B.userID inner join [QuickBite].[dbo].Movie c on A.movieID = C.ID where A.movieID=" + id + " order by A.datecreated desc";
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        GroupList = new List<MovieGetComment>();
                        while (rd.Read())
                        {
                            MovieGetComment details = new MovieGetComment();
                            details.Username = rd.GetString(0);
                            details.FullName = rd.GetString(1);
                            details.Comments = rd.GetString(2);
                            details.title = rd.GetString(3);
                            GroupList.Add(details);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return GroupList;
        }
    }
}