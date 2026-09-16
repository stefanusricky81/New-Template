using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuickBite.Models
{
    public class LoginRegistration
    {
       // public string user_email { get; set; }
        public string user_status { get; set; }

        public static List<LoginRegistration> RegisterUser(string firstname, string email, string password, string regtype)
        {
            List<LoginRegistration> Register = new List<LoginRegistration>();
            Guid id = Guid.NewGuid();
            string strSql = "IF NOT EXISTS(SELECT * FROM [Users] WHERE [UserName]=@User_Email) ";
            strSql += "INSERT INTO [Users]([UserName],[FullName],[Password],[status],[datecreated],[Guid],[SecretKey],[RegisterType],[NotificationPush]) ";
            strSql += "VALUES(@User_Email,@First_Name,@Password,@Locked,GETDATE(),@Guid,@SecretKey,@RegisterType,@ads)";
            try
            {
                using (SqlConnection cn = new SqlConnection("x"))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSql, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("User_Email", SqlDbType.NVarChar).Value = email;
                        cmd.Parameters.Add("First_Name", SqlDbType.NVarChar).Value = firstname;
                        cmd.Parameters.Add("Password", SqlDbType.NVarChar).Value = password;
                        cmd.Parameters.Add("Locked", SqlDbType.Bit).Value = true;
                        cmd.Parameters.Add("ads", SqlDbType.Bit).Value = true;
                        cmd.Parameters.Add("RegisterType", SqlDbType.NVarChar).Value = regtype;
                        cmd.Parameters.Add("Guid", SqlDbType.NVarChar).Value = id.ToString("n");
                        cmd.Parameters.Add("SecretKey", SqlDbType.NVarChar).Value = Tools.Encryption(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "-" + id.ToString("n")); ;
                        int status = Convert.ToInt32(cmd.ExecuteNonQuery());
                        LoginRegistration details = new LoginRegistration();

                        if (status == -1)
                            details.user_status = "user is already exists";
                        else
                            details.user_status = "success";
                        //details.user_email = email;
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
