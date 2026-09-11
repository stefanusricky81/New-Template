using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Admin
/// </summary>
public class Admin
{
    private static string sStrConnection = "server=localhost;User id=dbuser;password=server9@9@;database=TopModelIndo";
    //private static string sStrConnection = "server=117.102.120.139;User id=dbuser;password=server9@9@;database=TopModelIndo";

    public Admin()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static string[] Login(string username, string password)
    {
        string status = string.Empty;
        string roles = string.Empty;
        string locked = string.Empty;
        string userid = string.Empty;

        string strSql = "SELECT Roles,Locked,UserId FROM UserLogin WHERE cast(Username as varbinary)=cast(@Username as varbinary) AND cast(Password as varbinary)=cast(@Password as varbinary)";
        try
        {
            //ganti ke topmodel
            using (SqlConnection cn = new SqlConnection(sStrConnection))
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
            Tools.WriteLog(ex.Message, "Error\\SQL");
            status = "error";
        }

        string[] array = new string[5];
        array[0] = status;
        array[1] = roles;
        array[2] = locked;
        array[3] = userid;

        return array;
    }

    public static DataSet ddlArtist(string wap) //Drop Down List
    {
        DataSet ds = new DataSet();
        string strSql = "SELECT [id],[ArtistName] FROM [TopModelIndo].[dbo].[Category]  where WAP = '" + wap + "'";

        using (SqlConnection cn = new SqlConnection(sStrConnection))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter(strSql, cn);
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static string AddWallPaper(string id, string title, string description, string artist, string folder, string File, string Logo, string WAP)
    {
        string status = string.Empty;
        string strSQL = "INSERT INTO [dbo].[WallPaperList] ([WallPaperId],[DateCreated],[WallPaperTitle],[WallPaperDescription],[WallPaperArtist],[WallPaperAddress],[WallPaperCounter],[WallPaperLogo],WAP) " +
                        "VALUES(@WallPaperId,GETDATE(),@WallPaperTitle,@WallPaperDescription,@WallPaperCategories,@WallPaperAddress,0,@WallPaperLogo,@WAP)";
        try
        {
            using (SqlConnection cn = new SqlConnection(sStrConnection))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("WallPaperId", id);
                    cmd.Parameters.AddWithValue("WallPaperTitle", title);
                    cmd.Parameters.AddWithValue("WallPaperDescription", description);
                    cmd.Parameters.AddWithValue("WallPaperCategories", artist);
                    cmd.Parameters.AddWithValue("WallPaperAddress", "Content/" + folder + "/" + File);
                    cmd.Parameters.AddWithValue("WallPaperLogo", "Content/Logo/" + Logo);
                    cmd.Parameters.AddWithValue("WAP", WAP);
                    cmd.ExecuteNonQuery();
                    status = "success";
                }
            }
        }
        catch (SqlException ex)
        {
            status = "error";
            Tools.WriteLog(ex.Message, "Error\\SQL");
        }
        return status;
    }

    public static string AddVideo(string id, string title, string description, string artist, string folder, string File, string Logo, string WAP)
    {
        string status = string.Empty;
        string strSQL = "INSERT INTO [dbo].[VideoList] ([VideoId],[DateCreated],[VideoTitle],[VideoDescription],[VideoArtist],[VideoAddress],[VideoCounter],[VideoLogo], WAP) " +
                        "VALUES(@VideoId,GETDATE(),@VideoTitle,@VideoDescription,@VideoCategories,@VideoAddress,0,@VideoLogo,@WAP)";
        try
        {
            using (SqlConnection cn = new SqlConnection(sStrConnection))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("VideoId", id);
                    cmd.Parameters.AddWithValue("VideoTitle", title);
                    cmd.Parameters.AddWithValue("VideoDescription", description);
                    cmd.Parameters.AddWithValue("VideoCategories", artist);
                    cmd.Parameters.AddWithValue("VideoAddress", "Content/" + folder + "/" + File);
                    cmd.Parameters.AddWithValue("VideoLogo", "Content/Logo/" + Logo);
                    cmd.Parameters.AddWithValue("WAP", WAP);
                    cmd.ExecuteNonQuery();
                    status = "success";
                }
            }
        }
        catch (SqlException ex)
        {
            status = "error";
            Tools.WriteLog(ex.Message, "Error\\SQL");
        }
        return status;
    }

    public static string AddArtist(string Name, string WAP)
    {
        string status = string.Empty;
        string strSQL = "INSERT INTO [dbo].[Category] ([ArtistName],WAP) " +
                        "VALUES(@Name, @WAP)";
        try
        {
            using (SqlConnection cn = new SqlConnection(sStrConnection))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("Name", Name);
                    cmd.Parameters.AddWithValue("WAP", WAP);
                    cmd.ExecuteNonQuery();
                    status = "success";
                }
            }
        }
        catch (SqlException ex)
        {
            status = "error";
            Tools.WriteLog(ex.Message, "Error\\SQL");
        }
        return status;
    }

    public static string AddBanner(string id, string File, string WAP, string show)
    {
        string status = string.Empty;
        string strSQL = "INSERT INTO TopModelIndo.dbo.Banner(BannerId,BannerAddress,flag,WAP,date_created) " +
                        "VALUES(@BannerId,@BannerAddress,@flag,@WAP,GETDATE())";
        try
        {
            using (SqlConnection cn = new SqlConnection(sStrConnection))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("BannerId", id);
                    cmd.Parameters.AddWithValue("BannerAddress", "Content/Banner/" + File);
                    cmd.Parameters.AddWithValue("flg", show);
                    cmd.Parameters.AddWithValue("WAP", WAP);
                    cmd.ExecuteNonQuery();
                    status = "success";
                }
            }
        }
        catch (SqlException ex)
        {
            status = "error";
            Tools.WriteLog(ex.Message, "Error\\SQL");
        }
        return status;
    }

    public static void UpdateLogin(string username, int mark)
    {
        string strSQL = "UPDATE [UserLogin] SET [LoginCounter]=@LoginCounter WHERE Username=@username;";
        try
        {
            using (SqlConnection cn = new SqlConnection(sStrConnection))
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
            Tools.WriteLog(ex.Message, "Error\\SQL");
        }
    }

    public static DataSet LoadWallPaperListCount(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnection))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("SP_TMI_GET_COUNTWALLPAPERLIST", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadVideoListCount(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnection))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("SP_TMI_GET_COUNTVIDEOLIST", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static void UpdateVideoLogo(string filename, string id)
    {
        string strSQL = "UPDATE [TopModelIndo].[dbo].[VideoList] SET VideoLogo= @filename WHERE ID=@id;";
        try
        {
            using (SqlConnection cn = new SqlConnection(sStrConnection))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("filename", SqlDbType.NVarChar).Value = "Content/Logo/" + filename;
                    cmd.Parameters.Add("id", SqlDbType.Int).Value = id;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException ex)
        {
            Tools.WriteLog(ex.Message, "Error\\SQL");
        }
    }
}