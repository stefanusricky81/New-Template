using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class ProfileEditName
    {
        public string responses { get; set; }
        public static List<ProfileEditName> SetName(int userid, string name)
        {
            List<ProfileEditName> Banner = null;

            string strSql = " update [QuickBite].[dbo].[Users] set FullName=@fullname where userid=@userid";
            try
            {
                using (SqlConnection cn = new SqlConnection("x"))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSql, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("fullname", name);
                        cmd.Parameters.Add("userid", userid);
                        cmd.ExecuteScalar();

                        Banner = new List<ProfileEditName>();
                        ProfileEditName details = new ProfileEditName();
                        details.responses = "success";
                        Banner.Add(details);
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
