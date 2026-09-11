using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class Login
    {
        public string account_status { get; set; }
        public string email { get; set; }
        public string register_type { get; set; }
        public string first_name { get; set; }
        public int Id { get; set; }

        public static List<Login> ValidateUser(string useremail, string password)
        {
            List<Login> Login = null;

            if (string.IsNullOrEmpty(useremail) || string.IsNullOrEmpty(password))
                return Login;

            string strSql = "SELECT [UserName],[FullName],[RegisterType],[userid] FROM [Users] WHERE [UserName]=@User_Email AND [Password]=@Password AND [status]=@Locked";
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13"))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    cmd.Parameters.Add(new SqlParameter("User_Email", useremail));
                    cmd.Parameters.Add(new SqlParameter("Password", password));
                    cmd.Parameters.Add(new SqlParameter("Locked", true));
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        Login = new List<Login>();
                        Login details = new Login();
                        if (rd.Read())
                        {
                            details.email = useremail;
                            details.first_name = rd.GetString(1);
                            details.account_status = "true";
                            details.register_type = rd.GetString(2);
                            details.Id = rd.GetInt32(3);
                        }
                        else
                        {
                            details.email = useremail;
                            details.first_name = "n/a";
                            details.account_status = "false";
                            details.register_type = "n/a";
                        }
                        Login.Add(details);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return Login;
        }
    }
}