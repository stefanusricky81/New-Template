using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class NotificationPush
    {
        public string message { get; set; }

        public static List<NotificationPush> SendUsers(string message)
        {
            List<NotificationPush> GroupLists = new List<NotificationPush>();
            Guid id = Guid.NewGuid();
            string strSql = "Select [Messages] from [QuickBite].[dbo].[Notification] where ActiveStatus='true'";
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSql, cn))
                    {
                        cmd.Parameters.Clear();
                        string status = (string)cmd.ExecuteScalar();
                        NotificationPush details = new NotificationPush();

                        if (status == "")
                            details.message = "not success";
                        else
                            details.message = status;
                        GroupLists.Add(details);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return GroupLists;
        }
    }
}