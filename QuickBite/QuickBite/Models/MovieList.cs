using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class MovieList
    {
        public string Thumbnails { get; set; }
        public int Id { get; set; }
        public static List<MovieList> AllDetails()
        {
            List<MovieList> GroupList = null;

            string strSql = "SELECT Id,Thumbnail FROM [QuickBite].[dbo].[Movie] WHERE [status]=1 order by datecreated desc";
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        GroupList = new List<MovieList>();
                        while (rd.Read())
                        {
                            MovieList details = new MovieList();
                            details.Id = rd.GetInt32(0);
                            details.Thumbnails = rd.GetString(1);
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