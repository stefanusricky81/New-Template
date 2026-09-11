using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace cs_forest
{
    public class Model
    {
        private Tools tools = new Tools();

        #region For FIN

        #region Connection Server

        public string ConnStr(string source, string db)
        {
            string con = string.Empty;
            if (source == "105")
                con = "server=localhost;User id=dbuser;password=server9@9@;Max Pool Size=500;database=" + db;
            else if (source == "205")
                con = "server=103.253.113.205;User id=dbuser;password=server9@9@;Max Pool Size=200;database=" + db;

            return con;
        }

        #endregion
        
        #region For User

        public string[] Login(string username, string password)
        {
            string status = string.Empty;
            string roles = string.Empty;
            string locked = string.Empty;
            string userid = string.Empty;

            string strSql = "SELECT Roles,Locked,UserId FROM UserLogin WHERE cast(Username as varbinary)=cast(@Username as varbinary) AND cast(Password as varbinary)=cast(@Password as varbinary)";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr("205", "CustomerServicesIndonesia")))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    cmd.Parameters.Add(new SqlParameter("Username", username));
                    cmd.Parameters.Add(new SqlParameter("Password", password));
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            status = "success";
                            roles = rd.GetString(0).ToLower();
                            locked = rd.GetString(1);
                            userid = rd.GetValue(2).ToString();
                        }
                        else
                        {
                            status = "fail";
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

            }

            string[] array = new string[5];
            array[0] = status;
            array[1] = roles;
            array[2] = locked;
            array[3] = userid;

            return array;
        }

        public DataSet GetMenu(string userid)
        {
            DataSet ds = new DataSet();
            string strSql = "SELECT A.[Id],A.[Name],A.[Logo],A.[Source] FROM [Modules] AS A INNER JOIN [UserAccess] AS B ON A.[Id] = B.[Modules] WHERE B.[UserId] = " + userid;
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr("205", "CustomerServicesIndonesia")))
                {
                    cn.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(strSql, cn))
                    {
                        ds.Reset();
                        adp.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return ds;
        }

        public string[] GetTelcoMenu(string userid)
        {
            string id = string.Empty;
            string name = string.Empty;
            string source = string.Empty;

            string strSql = "SELECT A.[Id],A.[Name],A.[Logo],A.[Source] FROM [Modules] AS A INNER JOIN [UserAccess] AS B ON A.[Id] = B.[Modules] WHERE B.[UserId] = @UserId";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr("205", "CustomerServicesIndonesia")))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    cmd.Parameters.Add(new SqlParameter("UserId", userid));
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            id = rd["id"].ToString();
                            name = rd["name"].ToString();
                            source = rd["source"].ToString();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            string[] array = new string[3];
            array[0] = id;
            array[1] = name;
            array[2] = source;

            return array;
        }

        public void AddActivity(string userid, string page)
        {
            string strSql = "INSERT INTO [dbo].[UserActivity] ([DateCreated],[UserId],[Page]) VALUES (GETDATE(),@UserId,@Page)";

            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr("205", "CustomerServicesIndonesia")))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSql, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("UserId", SqlDbType.Int).Value = userid;
                        cmd.Parameters.Add("Page", SqlDbType.NChar).Value = page;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        public void UpdateLogin(string username, int mark)
        {
            string strSQL = "UPDATE [UserLogin] SET [LoginCounter]=@LoginCounter, LastLoginDate=getdate() WHERE Username=@username;";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr("205", "CustomerServicesIndonesia")))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("Username", SqlDbType.NVarChar).Value = username;
                        cmd.Parameters.Add("LoginCounter", SqlDbType.Int).Value = mark;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        public string CreateUser(string username, string password, string roles, string email)
        {
            string status = string.Empty;
            try
            {
                string strSQL = "INSERT INTO [dbo].[UserLogin] ([CreateDate],[Username],[Password],[Roles],[Email],[Locked],[FailLoginCount]) ";
                strSQL += "VALUES (GETDATE(),@Username,@Password,@Roles,@Email,@Locked,@FailLoginCount);SELECT MAX(UserId) FROM [dbo].[UserLogin]";
                using (SqlConnection cn = new SqlConnection(ConnStr("205", "CustomerServicesIndonesia")))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("Username", SqlDbType.NChar).Value = username;
                        cmd.Parameters.Add("Password", SqlDbType.NChar).Value = password;
                        cmd.Parameters.Add("Roles", SqlDbType.NChar).Value = roles;
                        cmd.Parameters.Add("Email", SqlDbType.NChar).Value = email;
                        cmd.Parameters.Add("Locked", SqlDbType.NChar).Value = "False";
                        cmd.Parameters.Add("FailLoginCount", SqlDbType.Int).Value = 0;
                        status = Convert.ToString((Int32)cmd.ExecuteScalar());
                    }
                }
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    status = ex.Errors[i].Message;
                }
            }
            return status;
        }

        public void AddUserAccess(string userid, string modules, int i)
        {
            string strSql = string.Empty;

            if (i == 0)
                strSql = "DELETE FROM [dbo].[UserAccess] WHERE [UserId]=@UserId;";

            strSql += "INSERT INTO [dbo].[UserAccess] ([CreateDate],[UserId],[Modules]) VALUES (GETDATE(),@UserId,@Modules)";

            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr("205", "CustomerServicesIndonesia")))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSql, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("UserId", SqlDbType.Int).Value = userid;
                        cmd.Parameters.Add("Modules", SqlDbType.NChar).Value = modules;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        public void UpdateLoginStatus(int userid, string status)
        {
            string strSQL = "UPDATE [UserLogin] SET [Locked]=@Locked WHERE UserId=@UserId;";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr("205", "CustomerServicesIndonesia")))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("UserId", SqlDbType.Int).Value = userid;
                        cmd.Parameters.Add("Locked", SqlDbType.NVarChar).Value = status;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        public bool UserAcccess(string id, string userid, string name, string source)
        {
            bool status = default(bool);
            string strSql = "SELECT Id FROM UserAccess WHERE Modules=@Id,N AND UserId=@UserId";
            strSql = "SELECT A.Name,A.Source,B.UserId,B.Modules FROM [Modules] AS A ";
            strSql += "INNER JOIN [UserAccess] AS B ON A.Id = B.Modules ";
            strSql += "WHERE B.Modules=@Id AND B.UserId=@UserId AND A.Name=@Name AND A.source=@Source";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr("205", "CustomerServicesIndonesia")))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    cmd.Parameters.Add(new SqlParameter("Id", id));
                    cmd.Parameters.Add(new SqlParameter("UserId", userid));
                    cmd.Parameters.Add(new SqlParameter("Name", name));
                    cmd.Parameters.Add(new SqlParameter("Source", source));
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                            status = true;
                        else
                            status = false;
                    }
                }
            }
            catch (SqlException ex)
            {

            }

            return status;
        }

        public DataSet SearchActivity(string userid, DateTime from, DateTime to)
        {
            string strSql = "SELECT B.Username,A.DateCreated AS [Date Access],A.Page FROM [UserActivity] AS A INNER JOIN [UserLogin] AS B ON A.UserId=B.UserId ";
            strSql += "WHERE A.UserId=" + userid + " AND (CAST(A.DateCreated AS DATE) >= '" + from.ToString("yyyy-MM-dd") + "' AND CAST(A.DateCreated AS DATE) <= '" + to.ToString("yyyy-MM-dd") + "')";
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr("205", "CustomerServicesIndonesia")))
                {
                    cn.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(strSql, cn))
                    {
                        ds.Reset();
                        adp.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {

            }

            return ds;
        }

        public string GetPassword(string email)
        {
            string password = string.Empty;
            string strSql = "SELECT Password FROM UserLogin WHERE Email=@Email";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr("205", "CustomerServicesIndonesia")))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    cmd.Parameters.Add(new SqlParameter("Email", email));
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            password = rd["Password"].ToString();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

            }
            return password;
        }

        public string UpdatePassword(string oldpassword, string newpassword, string userid)
        {
            string status = string.Empty;

            string strSQL = "UPDATE [UserLogin] SET [Password]=@Newpassword WHERE cast(Password as varbinary)=cast(@Oldpassword as varbinary) AND Userid=@Userid;";
            strSQL += "SELECT [Password] FROM [UserLogin] WHERE cast(Password as varbinary)=cast(@Newpassword as varbinary) AND Userid=@Userid";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr("205", "CustomerServicesIndonesia")))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("Newpassword", SqlDbType.NVarChar).Value = newpassword;
                        cmd.Parameters.Add("Oldpassword", SqlDbType.NVarChar).Value = oldpassword;
                        cmd.Parameters.Add("Userid", SqlDbType.NVarChar).Value = userid;
                        status = Convert.ToString(cmd.ExecuteScalar());
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return status;
        }

        public DataSet ddlUser(string roles)
        {
            DataSet ds = new DataSet();
            string strSql = "SELECT [Username], [UserId] FROM [UserLogin] WHERE [Roles] = '" + roles + "'";

            using (SqlConnection cn = new SqlConnection(ConnStr("205", "CustomerServicesIndonesia")))
            {
                cn.Open();
                SqlDataAdapter adp = new SqlDataAdapter(strSql, cn);
                
                ds.Reset();
                adp.Fill(ds);
            }
            return ds;
        }
        #endregion

        #region BlackList

        public void AddBlaclist(string msisdn, string user_id, string telco, string source)
        {
            string strSql = "IF NOT EXISTS(SELECT msisdn FROM [dbo].[tblBlackListedMSISDN] WHERE msisdn=@msisdn) INSERT INTO [dbo].[tblBlackListedMSISDN] ([msisdn],[telco],[datecreated],[blockedBy],[status]) VALUES (@msisdn,@telco,GETDATE(),@blockedBy,@status) ";
            using (SqlConnection cn = new SqlConnection(ConnStr(source, "smsadminv2")))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(strSql, cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("msisdn", SqlDbType.NChar).Value = msisdn;
                    cmd.Parameters.Add("telco", SqlDbType.NChar).Value = telco;
                    cmd.Parameters.Add("blockedBy", SqlDbType.NChar).Value = user_id;
                    cmd.Parameters.Add("status", SqlDbType.NChar).Value = "blocked";
                    cmd.ExecuteNonQuery();
                }
            }

        }

        public DataSet SearchBlacklist(string msisdn, string telco, string source)
        {
            DataSet ds = new DataSet();
            string strSql = "SELECT [id],[msisdn],[telco],[datecreated],[unblockedBy],[blockedBy],[status] FROM [tblBlackListedMSISDN] WHERE [msisdn]='" + msisdn + "'";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, "smsadminv2")))
                {
                    cn.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(strSql, cn))
                    {
                        ds.Reset();
                        adp.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return ds;
        }

        public void UpdateBlacklist(int id, string status, string user_id, string source)
        {
            string strSql = "UPDATE [tblBlackListedMSISDN] SET [status]=@status,[unblockedBy]=@unblockedBy WHERE [id]=@id";

            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, "smsadminv2")))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSql, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("status", SqlDbType.NChar).Value = status;
                        cmd.Parameters.Add("unblockedBy", SqlDbType.NChar).Value = user_id;
                        cmd.Parameters.Add("id", SqlDbType.Int).Value = id;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }
        #endregion

        #region MOMT

        public DataSet SearchMT(string source, string msisdn, string name)
        {
            DataSet ds = new DataSet();

            string strSql = string.Empty;

            string tblMT = string.Empty;
            string database = string.Empty;

            strSql = "SELECT CONVERT(VARCHAR(19),A.[datecreated]) AS [Date],A.[msg_id],RTRIM(A.[msisdn]) AS msisdn,B.[Keyword],RTRIM(A.[statusmsg]) AS Message,CAST(A.price AS FLOAT)/100 AS [Price],RTRIM(A.[gateway]) AS telco,RTRIM(A.[DNStatus]) AS DN FROM ";
            tblMT = "tblMT_" + name;
            database = "MOMT_NEW";

            strSql += "[dbo].[" + tblMT + "] AS A INNER JOIN [smsadminv2].[dbo].[tblProvDet] AS B ON A.provid=B.provid ";

            strSql += "WHERE msisdn = '" + msisdn + "' ORDER BY A.datecreated DESC";

            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, database)))
                {
                    cn.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(strSql, cn))
                    {
                        ds.Reset();
                        adp.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return ds;
        }

        public DataSet SearchMO(string source, string msisdn, string name)
        {
            DataSet ds = new DataSet();

            string strSql = string.Empty;

            string tblMO = string.Empty;
            string database = string.Empty;

            strSql = "SELECT CONVERT(VARCHAR(19),A.[datecreated]) AS [Date],RTRIM(A.[msg_id]) AS [Message Id],RTRIM(A.[msisdn]) AS Msisdn,RTRIM(telconame) AS Telco,RTRIM(B.keyword) AS Keyword,RTRIM(A.[msg]) AS Message,RTRIM(A.[source]) AS Source FROM ";
            tblMO = "tblMO_" + name;
            database = "MOMT_NEW";

            strSql += "[dbo].[" + tblMO + "] AS A INNER JOIN [smsadminv2].[dbo].[tblProvDet] AS B ON A.provid=B.provid ";

            strSql += "WHERE A.msisdn = '" + msisdn + "' ORDER BY A.datecreated DESC";

            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, database)))
                {
                    cn.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(strSql, cn))
                    {
                        ds.Reset();
                        adp.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {

            }

            return ds;
        }

        #endregion

        #region smsadminv2

        public void Unsubscribe(string source, int userid, string userlogin, string name)
        {
            Tools tl = new Tools();
            string strSQL = string.Empty;

            strSQL = "UPDATE [tblSubscriber] SET [status]=1,[unsubby]=@unsubby,date_unsubs=GETDATE() WHERE userid=@userid and status=0;";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, "smsadminv2")))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("userid", SqlDbType.NVarChar).Value = userid;
                        cmd.Parameters.Add("unsubby", SqlDbType.NVarChar).Value = userlogin;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                tl.process_log(DateTime.Now.ToString() + "| " + strSQL, "Err");
            }
        }

        public DataSet SearchSubscriber(string source, string msisdn, string name)
        {
            DataSet ds = new DataSet();

            string tblMO = string.Empty;
            string tblMT = string.Empty;

            tblMO = "tblMO_" + name;
            tblMT = "tblMT_" + name;

            string strSql = "SELECT s.userid,s.msisdn,p.shortcode,p.keyword,s.mo_id,s.groupid,isnull(s.unsubby,'n/a') as unsubby,CASE WHEN s.status = 0 THEN 'REG' ELSE 'STOP' END AS status,s.date_reg,s.date_unsubs,p.provider_id,s.provid,s.telconame as Gateway,s.regtype";
            strSql += ",(select count(a.msisdn) from [MOMT_NEW].[dbo].[" + tblMO + "] as a where a.msisdn='" + msisdn + "' and a.provid=s.provid) as mo";
            strSql += ",(select count(a.msisdn) from [MOMT_NEW].[dbo].[" + tblMT + "] as a where a.msisdn='" + msisdn + "' and a.provid=s.provid) as mt";
            strSql += ",(select CAST(SUM(a.price) AS NVARCHAR) + '.00' from [MOMT_NEW].[dbo].[" + tblMT + "] as a where a.msisdn='" + msisdn + "' and a.DNStatus='Delivered' and a.provid=s.provid) as revenue ";
            strSql += "FROM [smsadminv2].[dbo].[tblSubscriber] AS s INNER JOIN [smsadminv2].[dbo].[tblProvDet] AS p ON s.provid = p.provid ";
            strSql += "WHERE (LTRIM(RTRIM(s.msisdn)) = '" + msisdn + "') AND p.shortcode='" + name + "'  ORDER BY s.date_unsubs DESC";

            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, "smsadminv2")))
                {
                    cn.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(strSql, cn))
                    {
                        ds.Reset();
                        adp.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {

            }

            return ds;
        }

        public void UnsubscribeAll(string source, string msisdn, string userlogin, string name)
        {
            Tools tl = new Tools();

            string strSQL = string.Empty;

            strSQL = "UPDATE [tblSubscriber] SET [status]=1,[unsubby]=@unsubby,date_unsubs=GETDATE() WHERE msisdn=@msisdn and status=0;";

            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, "smsadminv2")))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("msisdn", SqlDbType.NVarChar).Value = msisdn;
                        cmd.Parameters.Add("unsubby", SqlDbType.NVarChar).Value = userlogin;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                tl.process_log(DateTime.Now.ToString() + "| " + strSQL, "Err");
            }
        }

        #endregion

        #region Other

        public string GetTelco(string module_id)
        {
            string status = string.Empty;
            if (module_id == "1")
                status = "Indosat";
            else if (module_id == "2")
                status = "Smartfren";
            else if (module_id == "3")
                status = "XL";
            else if (module_id == "4")
                status = "Three";
            else if (module_id == "5")
                status = "Telkomsel";
            return status;
        }

        #endregion

        #endregion

        #region Old

        public DataSet SearchMOGlobePH(string source, string msisdn)
        {
            DataSet ds = new DataSet();

            string strSql = "SELECT [MoId], [DateCreated], [TransactionId], [Msisdn], [Message], ISNULL([MoType],'SMS') AS [MoType] FROM [tblMO] WHERE ([Msisdn] = '" + msisdn + "') ORDER BY [DateCreated]";

            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, "GameWorld")))
                {
                    cn.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(strSql, cn))
                    {
                        ds.Reset();
                        adp.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {

            }

            return ds;
        }

        public DataSet SearchMTGlobePH(string source, string msisdn)
        {
            DataSet ds = new DataSet();

            string strSql = string.Empty;
            strSql = "SELECT [MtId] ,[DateCreated] ,[GameId],[Msisdn] ,Coalesce([Message],[Message2]) as 'Message',[Keyword],[MTStatus]  FROM [GameWorld].[dbo].[tblMT] WHERE [Msisdn]='" + msisdn + "'";

            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, "GameWorld")))
                {
                    cn.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(strSql, cn))
                    {
                        ds.Reset();
                        adp.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {

            }

            return ds;
        }

        public DataSet SearchGlobe(string msisdn)
        {
            DataSet ds = new DataSet();
            string strSql = "SELECT CONVERT(VARCHAR(19),[datecreated]) AS [Date],REPLACE([tblName],'tblMT_','') AS Client,[msg_id] AS [Message Id],[Msisdn],[Status],[msg] AS [Message],[Route],[transid] AS [Transaction Id],[msgtype] AS [Type],[split] ";
            strSql += "FROM [dbo].[AllMTGlobe] WHERE [msisdn]='" + msisdn + "' ";
            strSql += "ORDER BY [datecreated] DESC";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr("106", "smsglobe")))
                {
                    cn.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(strSql, cn))
                    {
                        ds.Reset();
                        adp.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {

            }
            return ds;
        }

        public void Unsubscribe101(string source, int userid, string userlogin)
        {
            Tools tl = new Tools();
            string strSQL = "UPDATE [Subscriber] SET [Status]=0,[UnsubscribeBy]=@unsubby,[DateUnsubscribe]=GETDATE() WHERE [SubId]=@userid AND [Status]=1;";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, "SMSAdmin")))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("userid", SqlDbType.Int).Value = userid;
                        cmd.Parameters.Add("unsubby", SqlDbType.NVarChar).Value = userlogin;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                tl.process_log(DateTime.Now.ToString() + "| " + strSQL, "Err");
            }
        }

        public void UnsubscribeGlobePH(string source, int subsid, string userlogin)
        {
            Tools tl = new Tools();
            string strSQL = "UPDATE [tblSubscriber] SET [Status]=0,[Status2]=4,[UnsubBy]=@unsubby WHERE [SubsId]=@SubsId ";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, "GameWorld")))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("SubsId", SqlDbType.Int).Value = subsid;
                        cmd.Parameters.Add("unsubby", SqlDbType.NVarChar).Value = userlogin;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                tl.process_log(DateTime.Now.ToString() + "| " + strSQL, "Err");
            }
        }

        public void UnsubscribeGlobePHMsisdn(string source, string sMsisdn, string userlogin)
        {
            Tools tl = new Tools();
            string strSQL = "UPDATE [tblSubscriber] SET [Status]=0,[Status2]=4,[UnsubBy]=@unsubby WHERE [Msisdn]=@msisdn ";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, "GameWorld")))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("msisdn", SqlDbType.NVarChar).Value = sMsisdn;
                        cmd.Parameters.Add("unsubby", SqlDbType.NVarChar).Value = userlogin;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                tl.process_log(DateTime.Now.ToString() + "| " + strSQL, "Err");
            }
        }

        public string CheckGlobePHMsisdn(string sMsisdn, string source)
        {
            Tools tl = new Tools();
            string strSQL = "SELECT SubsId from [tblSubscriber] WHERE [Msisdn]=@msisdn ";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, "GameWorld")))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("msisdn", SqlDbType.NVarChar).Value = sMsisdn;
                        object n = cmd.ExecuteScalar();
                        cmd.Connection.Close();
                        if (n == null)
                            return null;
                        return Convert.ToString(n);
                    }
                }
            }
            catch (SqlException ex)
            {
                tl.process_log(DateTime.Now.ToString() + "| " + strSQL, "Err");
                return null;
            }
        }

        public DataSet MySQLGlobe(string type, string value)
        {
            string strSql = "select id,smpp_user,sender_number,receiver_number,message,message_id,http_api_id,http_response,dn_required,dn_status,sms_sent_time,dn_received_time,sms_coding,udh from sms_log ";
            if (type == "msisdn")
                strSql += "where receiver_number = '" + value + "' order by sms_sent_time desc";
            else
                strSql += "where message_id = '" + value + "' order by sms_sent_time desc";

            DataSet ds = new DataSet();
            using (MySqlConnection connection = new MySqlConnection("Server=210.5.41.112;userid=engrjahed;password=engrjahed123;Database=kannel"))
            {
                MySqlCommand cmd = new MySqlCommand(strSql, connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                ds.Reset();
                adp.Fill(ds);
            }
            return ds;
        }

        public DataSet GetSMPPUser()
        {
            string strSql = "select id,system_id,password,system_type,allowed_id,status from smpp_info";
            DataSet ds = new DataSet();

            using (MySqlConnection connection = new MySqlConnection("Server=210.5.41.112;userid=engrjahed;password=engrjahed123;Database=kannel"))
            {
                MySqlCommand cmd = new MySqlCommand(strSql, connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                ds.Reset();
                adp.Fill(ds);
                connection.Close();
            }
            return ds;
        }

        public void DeleteSMPPUser(string id)
        {
            string strSql = "delete from smpp_info where id=@id";
            try
            {
                using (MySqlConnection connection = new MySqlConnection("Server=210.5.41.112;userid=engrjahed;password=engrjahed123;Database=kannel"))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(strSql, connection);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetSMPPRoute()
        {

            string strSql = "select id,route_name,smpp_user_id,http_api_id,prefix,operator_name,country_name from smpp_route";
            DataSet ds = new DataSet();


            using (MySqlConnection connection = new MySqlConnection("Server=210.5.41.112;userid=engrjahed;password=engrjahed123;Database=kannel"))
            {
                MySqlCommand cmd = new MySqlCommand(strSql, connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                ds.Reset();
                adp.Fill(ds);
                connection.Close();
            }
            return ds;
        }

        public DataSet GetSMPPHttpRoute()
        {

            string strSql = "select id,route_name,smpp_user_id,http_api_id,prefix,operator_name,country_name from smpp_route";
            DataSet ds = new DataSet();


            using (MySqlConnection connection = new MySqlConnection("Server=210.5.41.113;userid=engrjahed;password=engrjahed123;Database=kannel"))
            {
                MySqlCommand cmd = new MySqlCommand(strSql, connection);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                ds.Reset();
                adp.Fill(ds);
                connection.Close();
            }
            return ds;
        }

        public void DeleteSMPPRoute(string id)
        {
            string strSql = "delete from smpp_route where id=@id";
            try
            {
                using (MySqlConnection connection = new MySqlConnection("Server=210.5.41.112;userid=engrjahed;password=engrjahed123;Database=kannel"))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(strSql, connection);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public void AddSMPPAccount(string systemid, string password, string systemtype, string ipaddress)
        {
            string strSql = "insert into smpp_info (system_id,password,system_type,allowed_id,status) ";
            strSql += "values(@system_id,@password,@system_type,@allowed_id,@status)";
            try
            {
                using (MySqlConnection connection = new MySqlConnection("Server=210.5.41.112;userid=engrjahed;password=engrjahed123;Database=kannel"))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(strSql, connection);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@system_id", systemid);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@system_type", systemtype);
                    cmd.Parameters.AddWithValue("@allowed_id", ipaddress);
                    cmd.Parameters.AddWithValue("@status", "N");
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public void UpdateSMPPAccount(string id, string systemid, string password, string systemtype, string ipaddress)
        {
            string strSql = "update smpp_info set system_id=@system_id,password=@password,system_type=@system_type,allowed_id=@allowed_id,status=@status ";
            strSql += "where id=@id";
            try
            {
                using (MySqlConnection connection = new MySqlConnection("Server=210.5.41.112;userid=engrjahed;password=engrjahed123;Database=kannel"))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(strSql, connection);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@system_id", systemid);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@system_type", systemtype);
                    cmd.Parameters.AddWithValue("@allowed_id", ipaddress);
                    cmd.Parameters.AddWithValue("@status", "N");
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public void UpdateSMPPRoute(string id, string route, string user_id, string api_id, string prefix, string operator_name, string country)
        {
            string strSql = "update smpp_route set route_name=@route_name,smpp_user_id=@smpp_user_id,http_api_id=@http_api_id,prefix=@prefix,operator_name=@operator_name,country_name=@country_name ";
            strSql += "where id=@id";
            try
            {
                using (MySqlConnection connection = new MySqlConnection("Server=210.5.41.112;userid=engrjahed;password=engrjahed123;Database=kannel"))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(strSql, connection);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@route_name", route);
                    cmd.Parameters.AddWithValue("@smpp_user_id", user_id);
                    cmd.Parameters.AddWithValue("@http_api_id", api_id);
                    cmd.Parameters.AddWithValue("@prefix", prefix);
                    cmd.Parameters.AddWithValue("@operator_name", operator_name);
                    cmd.Parameters.AddWithValue("@country_name", country);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public void AddSMPPRoute(string route, string user_id, string api_id, string prefix, string operator_name, string country)
        {
            string strSql = "insert into smpp_route (route_name,smpp_user_id,http_api_id,prefix,operator_name,country_name) ";
            strSql += "values(@route_name,@smpp_user_id,@http_api_id,@prefix,@operator_name,@country_name)";
            try
            {
                using (MySqlConnection connection = new MySqlConnection("Server=210.5.41.112;userid=engrjahed;password=engrjahed123;Database=kannel"))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(strSql, connection);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@route_name", route);
                    cmd.Parameters.AddWithValue("@smpp_user_id", user_id);
                    cmd.Parameters.AddWithValue("@http_api_id", api_id);
                    cmd.Parameters.AddWithValue("@prefix", prefix);
                    cmd.Parameters.AddWithValue("@operator_name", operator_name);
                    cmd.Parameters.AddWithValue("@country_name", country);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public void UpdateSMPPHttpRoute(string id, string route, string user_id, string api_id, string prefix, string operator_name, string country)
        {
            string strSql = "update smpp_route set route_name=@route_name,smpp_user_id=@smpp_user_id,http_api_id=@http_api_id,prefix=@prefix,operator_name=@operator_name,country_name=@country_name ";
            strSql += "where id=@id";
            try
            {
                using (MySqlConnection connection = new MySqlConnection("Server=210.5.41.113;userid=engrjahed;password=engrjahed123;Database=kannel"))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(strSql, connection);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@route_name", route);
                    cmd.Parameters.AddWithValue("@smpp_user_id", user_id);
                    cmd.Parameters.AddWithValue("@http_api_id", api_id);
                    cmd.Parameters.AddWithValue("@prefix", prefix);
                    cmd.Parameters.AddWithValue("@operator_name", operator_name);
                    cmd.Parameters.AddWithValue("@country_name", country);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public void AddSMPPHttpRoute(string route, string user_id, string api_id, string prefix, string operator_name, string country)
        {
            string strSql = "insert into smpp_route (route_name,smpp_user_id,http_api_id,prefix,operator_name,country_name) ";
            strSql += "values(@route_name,@smpp_user_id,@http_api_id,@prefix,@operator_name,@country_name)";
            try
            {
                using (MySqlConnection connection = new MySqlConnection("Server=210.5.41.113;userid=engrjahed;password=engrjahed123;Database=kannel"))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(strSql, connection);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@route_name", route);
                    cmd.Parameters.AddWithValue("@smpp_user_id", user_id);
                    cmd.Parameters.AddWithValue("@http_api_id", api_id);
                    cmd.Parameters.AddWithValue("@prefix", prefix);
                    cmd.Parameters.AddWithValue("@operator_name", operator_name);
                    cmd.Parameters.AddWithValue("@country_name", country);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public void AddBlaclistPH(string msisdn, string user_id, string source)
        {
            string strSql = "IF NOT EXISTS(SELECT msisdn FROM [dbo].[BlackList] WHERE msisdn=@msisdn) INSERT INTO [dbo].[BlackList] ([msisdn],[datecreated],[blockedby]) VALUES (@msisdn,GETDATE(),@blockedBy); ";
            strSql += "UPDATE [GameWorld].[dbo].[tblSubscriber] SET Status2=3 WHERE [Msisdn]=@msisdn ";
            using (SqlConnection cn = new SqlConnection(ConnStr(source, "GameWorld")))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(strSql, cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("msisdn", SqlDbType.NChar).Value = msisdn;
                    cmd.Parameters.Add("blockedBy", SqlDbType.NChar).Value = user_id;
                    cmd.ExecuteNonQuery();
                }
            }

        }

        public void UnsubBlaclistPH(string msisdn, string source)
        {
            string strSql = "UPDATE [GameWorld].[dbo].[tblSubscriber] SET Status2=4 WHERE [Msisdn]=@msisdn ";
            using (SqlConnection cn = new SqlConnection(ConnStr(source, "GameWorld")))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(strSql, cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("msisdn", SqlDbType.NChar).Value = msisdn;
                    cmd.ExecuteNonQuery();
                }
            }

        }

        public DataSet SearchBlacklistPH(string msisdn, string source)
        {
            DataSet ds = new DataSet();
            string strSql = "SELECT [DateCreated],[MSISDN] FROM [BlackList] WHERE [msisdn]='" + msisdn + "'";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnStr(source, "GameWorld")))
                {
                    cn.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(strSql, cn))
                    {
                        ds.Reset();
                        adp.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return ds;
        }

        public void CreateTable(string sTableNam, string sSource)
        {
            string sSqlCommand = "CREATE TABLE " + sTableNam + " ([MSISDN] [nvarchar] (23)) ";
            using (SqlConnection cn = new SqlConnection(ConnStr(sSource, "GameWorld")))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(sSqlCommand, cn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DropTable(string sTableName, string sSource)
        {
            string sSqlCommand = "DROP TABLE [GameWorld].[dbo]." + sTableName;
            using (SqlConnection cn = new SqlConnection(ConnStr(sSource, "GameWorld")))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(sSqlCommand, cn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void BulkInsertPHBlk(string sTableName, string sSource, string sUserID)
        {
            string sSqlCommand = "INSERT INTO [GameWorld].[dbo].[BlackList] SELECT msisdn,GETDATE()," + sUserID + " FROM " + sTableName + " T1 WHERE NOT EXISTS(SELECT ID FROM [GameWorld].[dbo].[BlackList] T2 WHERE T1.msisdn =T2.msisdn)";
            using (SqlConnection cn = new SqlConnection(ConnStr(sSource, "GameWorld")))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(sSqlCommand, cn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion
    }
}