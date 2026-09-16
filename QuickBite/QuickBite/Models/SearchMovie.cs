using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class SearchMovie
    {
        public string title { get; set; }
        public int id { get; set; }
        public string thumbnails { get; set; }
        public static List<SearchMovie> getAll(string id, string type,string searchname)
        {
            List<SearchMovie> All = null;
            string title="";
            string cast="";
            string name = "";

            if (type == "Movie")
            {
                cast = "%";
                name = "A.title,A.ID,A.Thumbnail";
                title = "%"+searchname+"%";
            }
            else if (type == "Cast")
            {
                title = "%";
                name = "D.Name,D.ID,D.Thumbnail";
                cast = "%" + searchname + "%";
            }

            string strSql = "SELECT "+name+" FROM [QuickBite].[dbo].[Movie] A inner join  [QuickBite].[dbo].[CastGroup] B on A.title=B.CastGroupName inner join  [QuickBite].[dbo].[ActorGroups] C on A.CastGroupId=C.CastGroupId inner join  [QuickBite].[dbo].[Cast] D on D.ID =C.CastID  where  A.title like '" + title + "' and D.Name like '" + cast + "'";
            try
            {
                using (SqlConnection cn = new SqlConnection("x"))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        All = new List<SearchMovie>();
                        SearchMovie details = new SearchMovie();
                        while (rd.Read())
                        {
                            details.title = rd.GetString(0);
                            details.id = rd.GetInt32(1);
                            details.thumbnails = rd.GetString(2);
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
