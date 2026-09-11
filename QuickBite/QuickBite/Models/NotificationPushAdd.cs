using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class NotificationPushAdd
    {
        public string user_email { get; set; }
        public string user_status { get; set; }

        public static List<NotificationPushAdd> ChangeUsers(string username, string email, string type)
        {
            List<NotificationPushAdd> ChangeUsers = new List<NotificationPushAdd>();
            Guid id = Guid.NewGuid();
            string strSql = "Update [QuickBite].[dbo].[Users] set NotificationPush=@regtype where Username=@emails and FullName=@username";
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSql, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new SqlParameter("emails", email));
                        cmd.Parameters.Add(new SqlParameter("username", username));
                        cmd.Parameters.Add(new SqlParameter("regtype", type));
                       
                        int status = Convert.ToInt32(cmd.ExecuteNonQuery());
                        NotificationPushAdd details = new NotificationPushAdd();

                        if (status == -1)
                            details.user_status = "not success";
                        else
                            details.user_status = "success";
                        details.user_email = email;
                        ChangeUsers.Add(details);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ChangeUsers;
        }
    }
}