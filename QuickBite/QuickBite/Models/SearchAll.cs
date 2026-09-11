using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class SearchAll
    {
        public string titleMovie { get; set; }
        public string titleGenre { get; set; }
        public int Year { get; set; }
        public string thumbnails { get; set; }
        public int movieid { get; set; }
        public static List<SearchAll> getAll(string MovTitle, string year, string genre)
        {
            List<SearchAll> All = null;
            
            if (MovTitle=="ALL")
                MovTitle = "%";
            if (year == "ALL")
                year = "%";
            if (genre == "ALL")
                genre = "%";

            string strSql = "SELECT A.title,B.title,Year(A.[relaseDate]) as years,thumbnail,A.ID FROM [QuickBite].[dbo].[Movie] A inner join [QuickBite].[dbo].Genre B on A.genreID=B.ID where Year([relaseDate]) like '" + year + "' and A.title like '" + MovTitle + "' and B.title like '" + genre + "'";
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        All = new List<SearchAll>();
                       // SearchAll details = new SearchAll();
                        while (rd.Read())
                        {
                            SearchAll details = new SearchAll();
                            details.titleMovie = rd.GetString(0);
                            details.titleGenre = rd.GetString(1);
                            details.Year = rd.GetInt32(2);
                            details.thumbnails = rd.GetString(3);
                            details.movieid = rd.GetInt32(4);
                            All.Add(details);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return All;
        }

    }
}