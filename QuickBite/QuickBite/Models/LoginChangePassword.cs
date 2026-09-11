using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class LoginChangePassword
    {
        public string username { get; set; }
        public string status { get; set; }

        public static List<LoginChangePassword> UpdatePassword(string username, string oldpassword, string newpassword)
        {
            List<LoginChangePassword> Register = new List<LoginChangePassword>();

            string strSql = "UPDATE [User_Profile] SET [Password]=@NewPassword WHERE [User_Email]=@Username AND [Password]=@OldPassword";
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=210.5.41.102;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSql, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("UserName", SqlDbType.NVarChar).Value = username;
                        cmd.Parameters.Add("NewPassword", SqlDbType.NVarChar).Value = newpassword;
                        cmd.Parameters.Add("OldPassword", SqlDbType.NVarChar).Value = oldpassword;
                        int status = Convert.ToInt32(cmd.ExecuteNonQuery());
                        LoginChangePassword details = new LoginChangePassword();
                        if (status == 0)
                            details.status = "fail";
                        else if (status == 1)
                            details.status = "success";
                        details.username = username;
                        Register.Add(details);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Register;
        }
    }
}