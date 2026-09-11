using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class ProfileBanner
    {
        public string name { get; set; }
        public string urlLink { get; set; }
        public string banner { get; set; }
        public string promotiondate { get; set; }
       public static List<ProfileBanner> AllDetails()
            {
                List<ProfileBanner> Banner = null;

                string strSql = "SELECT  A.[name],A.[urlLink],A.[banner],C.publishdate FROM [QuickBite].[dbo].[Banner] A inner join Movie C on C.ID=A.movieID WHERE A.[status]=0 order by A.id desc";
                try
                {
                    using (SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                    {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand(strSql, cn);
                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            Banner = new List<ProfileBanner>();
                            while (rd.Read())
                            {
                                ProfileBanner details = new ProfileBanner();
                                details.name = rd.GetString(0);
                                details.urlLink = rd.GetString(1);
                                details.banner = rd.GetString(2);
                                details.promotiondate = rd.GetDateTime(3).ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("en-US"));
                                Banner.Add(details);
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