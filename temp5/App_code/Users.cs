using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Users
/// </summary>
public class Users
{
    private static string sStrConnection = "server=localhost;User id=dbuser;password=server9@9@;database=TopModelIndo";
    //private static string sStrConnection = "server=117.102.120.139;User id=dbuser;password=server9@9@;database=TopModelIndo";

    public Users()
	{
    }

    public static DataSet LoadWallPaper(string type, string artist)
    {
        DataSet ds = new DataSet();

        string strSql = string.Empty;

        if (type == "all")
            strSql = "SELECT A.[WallPaperId],A.[WallPaperArtist],A.[WallPaperTitle],A.[WallPaperDescription],'http://topmodelindo.com/' + A.[WallPaperLogo] as WallPaperLogo ,'http://topmodelindo.com/' + A.[WallPaperAddress] as WallPaperAddress, B.ArtistName " +
                     "FROM [dbo].[WallPaperList] A INNER JOIN [dbo].[Category] B ON A.WallPaperArtist = B.id " +
                     "WHERE [WallPaperArtist]='" + artist + "' and A.WAP = 'TM' ORDER BY [DateCreated] ASC";

        else if (type == "new")
            strSql = "SELECT TOP 4 A.[WallPaperId],A.[WallPaperArtist],A.[WallPaperTitle],A.[WallPaperDescription],'http://topmodelindo.com/' + A.[WallPaperLogo] as WallPaperLogo,'http://topmodelindo.com/' + A.[WallPaperAddress] as WallPaperAddress, B.ArtistName " +
                      "FROM [dbo].[WallPaperList] A " +
                      "INNER JOIN [dbo].[Category] B ON A.WallPaperArtist = B.id WHERE A.WAP = 'TM' ORDER BY A.[DateCreated] ASC";

        else if (type == "top")
            strSql = "SELECT TOP 4 A.[WallPaperId],A.[WallPaperArtist],A.[WallPaperTitle],A.[WallPaperDescription],'http://topmodelindo.com/' + A.[WallPaperLogo] as WallPaperLogo,'http://topmodelindo.com/' + A.[WallPaperAddress] as WallPaperAddress, B.ArtistName " +
                     "FROM [dbo].[WallPaperList] A INNER JOIN [dbo].[Category] B ON A.WallPaperArtist = B.id WHERE A.WAP = 'TM' " +
                     "ORDER BY A.[WallPaperCounter] ASC";

        else if (type == "get")
            strSql = "SELECT A.[WallPaperId],A.[WallPaperArtist],A.[WallPaperTitle],A.[WallPaperDescription],'http://topmodelindo.com/' + A.[WallPaperLogo] as WallPaperLogo,'http://topmodelindo.com/' + A.[WallPaperAddress] as WallPaperAddress, B.ArtistName " +
                     "FROM [dbo].[WallPaperList] A INNER JOIN [dbo].[Category] B ON A.WallPaperArtist = B.id " +
                     "WHERE A.[WallPaperId]='" + artist + "' and A.WAP = 'TM' ORDER BY A.[WallPaperCounter] ASC";

        else if (type == "alldata")
            strSql = "SELECT A.[WallPaperId],A.[WallPaperArtist],A.[WallPaperTitle],A.[WallPaperDescription],'http://topmodelindo.com/' + A.[WallPaperLogo] as WallPaperLogo,'http://topmodelindo.com/' + A.[WallPaperAddress] as WallPaperAddress, B.ArtistName " +
                     "FROM [dbo].[WallPaperList] A INNER JOIN [dbo].[Category] B ON A.WallPaperArtist = B.id WHERE A.WAP = 'TM' " +
                     "ORDER BY A.[DateCreated] ASC";
        else if (type == "also")
            strSql = "SELECT A.[WallPaperId],A.[WallPaperArtist],A.[WallPaperTitle],A.[WallPaperDescription],'http://topmodelindo.com/' + A.[WallPaperLogo] as WallPaperLogo,'http://topmodelindo.com/' + A.[WallPaperAddress] as WallPaperAddress, B.ArtistName " +
                     "FROM [dbo].[WallPaperList] A INNER JOIN [dbo].[Category] B ON A.WallPaperArtist = B.id " +
                     "WHERE [WallPaperArtist]<>'" + artist + "'and A.WAP = 'TM' ORDER BY [DateCreated] ASC";

        using (SqlConnection cn = new SqlConnection(sStrConnection))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter(strSql, cn);
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadFreeVideo(string type, string artist)
    {
        DataSet ds = new DataSet();

        string strSql = string.Empty;

        if (type == "all")
            strSql = "SELECT  A.[VideoId],A.[VideoArtist],A.[VideoTitle],A.[VideoDescription],'http://topmodelindo.com/' + A.[VideoLogo] as VideoLogo,'http://topmodelindo.com/' + A.[VideoAddress] as VideoAddress, B.ArtistName " +
			         "FROM [dbo].[VideoList] A INNER JOIN [dbo].Category B ON A.VideoArtist = b.id " +
                     "WHERE [VideoArtist]='" + artist + "' and VideoAddress like'%Free%' and A.WAP = 'TM' ORDER BY [DateCreated] ASC";

        else if (type == "new")
            strSql = "SELECT TOP 4 A.[VideoId],A.[VideoArtist],A.[VideoTitle],A.[VideoDescription],'http://topmodelindo.com/' + A.[VideoLogo] as VideoLogo,'http://topmodelindo.com/' + A.[VideoAddress] as VideoAddress, B.ArtistName " +
                     "FROM [dbo].[VideoList] A INNER JOIN [dbo].Category B ON A.VideoArtist = b.id " +
                     "where A.VideoAddress like'%Free%' and A.WAP = 'TM' ORDER BY A.[DateCreated] ASC ";

        else if (type == "top")
            strSql = "SELECT TOP 4 A.[VideoId],A.[VideoArtist],A.[VideoTitle],A.[VideoDescription],'http://topmodelindo.com/' + A.[VideoLogo] as VideoLogo,'http://topmodelindo.com/' + A.[VideoAddress] as VideoAddress, B.ArtistName " +
                     "FROM [dbo].[VideoList] A INNER JOIN [dbo].Category B ON A.VideoArtist = b.id " +
                     "where VideoAddress like'%Free%' and A.WAP = 'TM' ORDER BY [VideoCounter] ASC";
        
        else if (type == "get")
            strSql = "SELECT A.[VideoId],A.[VideoArtist],A.[VideoTitle],A.[VideoDescription],'http://topmodelindo.com/' + A.[VideoLogo] as VideoLogo,'http://topmodelindo.com/' + A.[VideoAddress] as VideoAddress, B.ArtistName " +
                     "FROM [dbo].[VideoList] A INNER JOIN [dbo].Category B ON A.VideoArtist = b.id " +
                     "WHERE [VideoId]='" + artist + "' and VideoAddress like'%Free%' and A.WAP = 'TM' ORDER BY [VideoCounter] ASC";
        
        else if (type == "alldata")
            strSql = "SELECT A.[VideoId],A.[VideoArtist],A.[VideoTitle],A.[VideoDescription],'http://topmodelindo.com/' + A.[VideoLogo] as VideoLogo,'http://topmodelindo.com/' + A.[VideoAddress] as VideoAddress, B.ArtistName " +
                     "FROM [dbo].[VideoList] A INNER JOIN [dbo].Category B ON A.VideoArtist = b.id " +
                     "where VideoAddress like'%Free%' and A.WAP = 'TM' ORDER BY [DateCreated] ASC";

        else if (type == "upnext")
            strSql = "SELECT  A.[VideoId],A.[VideoArtist],A.[VideoTitle],A.[VideoDescription],'http://topmodelindo.com/' + A.[VideoLogo] as VideoLogo,'http://topmodelindo.com/' + A.[VideoAddress] as VideoAddress, B.ArtistName, A.VideoCounter " +
                     "FROM [dbo].[VideoList] A INNER JOIN [dbo].Category B ON A.VideoArtist = b.id WHERE [VideoArtist] IN "+
                     "(SELECT [VideoArtist] from [dbo].[VideoList] where [VideoId]='" + artist + "') And [VideoId] NOT IN ('" + artist + "') " +
                     "and VideoAddress like'%Free%' and A.WAP = 'TM' ORDER BY [DateCreated] ASC";

        using (SqlConnection cn = new SqlConnection(sStrConnection))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter(strSql, cn);
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadPremiumVideo(string type, string artist)
    {
        DataSet ds = new DataSet();

        string strSql = string.Empty;

        if (type == "all")
            strSql = "SELECT  A.[VideoId],A.[VideoArtist],A.[VideoTitle],A.[VideoDescription],'http://topmodelindo.com/' + A.[VideoLogo] as VideoLogo,'http://topmodelindo.com/' + A.[VideoAddress] as VideoAddress, B.ArtistName " +
                     "FROM [dbo].[VideoList] A INNER JOIN [dbo].Category B ON A.VideoArtist = b.id " +
                     "WHERE [VideoArtist]='" + artist + "' and VideoAddress like'%Premium%' and A.WAP = 'TM' ORDER BY [DateCreated] ASC";
        
        else if (type == "new")
            strSql = "SELECT TOP 4 A.[VideoId],A.[VideoArtist],A.[VideoTitle],A.[VideoDescription],'http://topmodelindo.com/' + A.[VideoLogo] as VideoLogo,'http://topmodelindo.com/' + A.[VideoAddress] as VideoAddress, B.ArtistName " +
                     "FROM [dbo].[VideoList] A INNER JOIN [dbo].Category B ON A.VideoArtist = b.id " +
                     "where A.VideoAddress like'%Premium%' and A.WAP = 'TM' ORDER BY A.[DateCreated] ASC "; 
        
        else if (type == "top")
            strSql = "SELECT TOP 4 A.[VideoId],A.[VideoArtist],A.[VideoTitle],A.[VideoDescription],'http://topmodelindo.com/' + A.[VideoLogo] as VideoLogo,'http://topmodelindo.com/' + A.[VideoAddress] as VideoAddress, B.ArtistName " +
                     "FROM [dbo].[VideoList] A INNER JOIN [dbo].Category B ON A.VideoArtist = b.id " +
                     "where VideoAddress like'%Premium%' and A.WAP = 'TM' ORDER BY [VideoCounter] ASC";
        
        else if (type == "get")
            strSql = "SELECT A.[VideoId],A.[VideoArtist],A.[VideoTitle],A.[VideoDescription],'http://topmodelindo.com/' + A.[VideoLogo] as VideoLogo,'http://topmodelindo.com/' + A.[VideoAddress] as VideoAddress, B.ArtistName " +
                     "FROM [dbo].[VideoList] A INNER JOIN [dbo].Category B ON A.VideoArtist = b.id " +
                     "WHERE [VideoId]='" + artist + "' and VideoAddress like'%Premium%' and A.WAP = 'TM' ORDER BY [VideoCounter] ASC";

        else if (type == "alldata")
            strSql = "SELECT A.[VideoId],A.[VideoArtist],A.[VideoTitle],A.[VideoDescription],'http://topmodelindo.com/' + A.[VideoLogo] as VideoLogo,'http://topmodelindo.com/' + A.[VideoAddress] as VideoAddress, B.ArtistName " +
                     "FROM [dbo].[VideoList] A INNER JOIN [dbo].Category B ON A.VideoArtist = b.id " +
                     "where VideoAddress like'%Premium%' and A.WAP = 'TM' ORDER BY [DateCreated] ASC";

        else if (type == "upnext")
            strSql = "SELECT  A.[VideoId],A.[VideoArtist],A.[VideoTitle],A.[VideoDescription],'http://topmodelindo.com/' + A.[VideoLogo] as VideoLogo,'http://topmodelindo.com/' + A.[VideoAddress] as VideoAddress, B.ArtistName, A.VideoCounter " +
                     "FROM [dbo].[VideoList] A INNER JOIN [dbo].Category B ON A.VideoArtist = b.id WHERE [VideoArtist] IN " +
                     "(SELECT [VideoArtist] from [dbo].[VideoList] where [VideoId]='" + artist + "') And [VideoId] NOT IN ('" + artist + "') " +
                     "and VideoAddress like'%Premium%' and A.WAP = 'TM' ORDER BY [DateCreated] ASC";

        using (SqlConnection cn = new SqlConnection(sStrConnection))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter(strSql, cn);
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadBanner()
    {
        DataSet ds = new DataSet();

        string strSql = string.Empty;

        strSql = "SELECT BANNERADDRESS FROM TOPMODELINDO.DBO.BANNER WHERE WAP = 'TM' AND FLAG = 1 ORDER BY DATE_CREATED ASC";

        using (SqlConnection cn = new SqlConnection(sStrConnection))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter(strSql, cn);
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet IsRegistered(string msisdn) //Checking Registered Or Not
    {
        DataSet ds = new DataSet();

        if (string.IsNullOrEmpty(msisdn))
            return ds;

        string strSql = "select count(*) from smsadminv2.dbo.tblSubscriber where provid = 1357 and status = 0 and updateStatus = 0 and msisdn = '" + msisdn + "'";

        using (SqlConnection cn = new SqlConnection(sStrConnection))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter(strSql, cn);
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet IsRegisteredIsat(string msisdn) //Checking Registered Or Not
    {
        DataSet ds = new DataSet();

        if (string.IsNullOrEmpty(msisdn))
            return ds;

        string strSql = "select count(*) from smsadminv2.dbo.tblSubscriber where provid = 2358 and status = 0 and updateStatus = 0 and msisdn = '" + msisdn + "'";

        using (SqlConnection cn = new SqlConnection(sStrConnection))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter(strSql, cn);
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }
    public static DataSet urlPremiumVideo(string cat)
    {
        DataSet ds = new DataSet();

        if (string.IsNullOrEmpty(cat))
            return ds;

        string strSql = "select [VideoAddress] from [TopModelIndo].[dbo].[VideoList] where [VideoId] = '" + cat + "'";

        using (SqlConnection cn = new SqlConnection(sStrConnection))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter(strSql, cn);
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static void UpdateWallPaper(string WallPaperID)
    {
        if (string.IsNullOrEmpty(WallPaperID))
            return;

        SqlConnection cn = new SqlConnection(sStrConnection);
        string cmdStr = "UPDATE [WallPaperList] SET [WallPaperCounter]= [WallPaperCounter] + 1 WHERE WallPaperId=@WallPaperId";
        SqlCommand cmd = new SqlCommand(cmdStr, cn);
        cmd.Parameters.Add("@WallPaperID", System.Data.SqlDbType.NVarChar).Value = WallPaperID;
        try
        {
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return;

        }
        catch (Exception ex)
        {
            Tools.WriteLog(DateTime.Now.ToString() + ",UpdateWallPaper," + ex.Message, "Error\\SQL");
            return;
        }
        finally
        {
            cmd.Connection.Close();
        }     
    }

    public static void UpdateVideo(string VideoID)
    {
        if (string.IsNullOrEmpty(VideoID))
            return;

        SqlConnection cn = new SqlConnection(sStrConnection);
        string cmdStr = "UPDATE [VideoList] SET [VideoCounter]= [VideoCounter] + 1 WHERE VideoId=@VideoId";
        SqlCommand cmd = new SqlCommand(cmdStr, cn);
        cmd.Parameters.Add("@VideoId", System.Data.SqlDbType.NVarChar).Value = VideoID;
        try
        {
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return;

        }
        catch (Exception ex)
        {
            Tools.WriteLog(DateTime.Now.ToString() + ",UpdateVideo," + ex.Message, "Error\\SQL");
            return;
        }
        finally
        {
            cmd.Connection.Close();
        }        
    }

    public static DataSet GetDetailWallPaperFree(string _cat)
    {
        DataSet ds = new DataSet();

        string strSql = string.Empty;

        strSql = "SELECT [WallPaperId],[WallPaperArtist],[WallPaperTitle],[WallPaperDescription],[WallPaperLogo],[WallPaperAddress] " +
                     "FROM [dbo].[WallPaperList] WHERE [WallPaperId]='" + _cat + "'";

        using (SqlConnection cn = new SqlConnection(sStrConnection))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter(strSql, cn);
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet GetDetailVideoFree(string _cat)
    {
        DataSet ds = new DataSet();

        string strSql = string.Empty;

        strSql = "SELECT [VideoId],[VideoArtist],[VideoTitle],[VideoDescription],[VideoLogo],[VideoAddress] " +
                     "FROM [dbo].[VideoList] WHERE [VideoId]='" + _cat + "'";

        using (SqlConnection cn = new SqlConnection(sStrConnection))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter(strSql, cn);
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet GetArtist()
    {
        DataSet ds = new DataSet();

        string strSql = string.Empty;


        strSql = "SELECT[id],[ArtistName] FROM [dbo].[Category] ";

        using (SqlConnection cn = new SqlConnection(sStrConnection))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter(strSql, cn);
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static int IsRegister(string msisdn)
    {
        if (string.IsNullOrEmpty(msisdn))
            return -1;

        SqlConnection cn = new SqlConnection(sStrConnection);
        string cmdStr = "select count(*) from smsadminv2.dbo.tblSubscriber where provid = 1357 and status = 0 and updateStatus = 0 and msisdn = @msisdn";
        SqlCommand cmd = new SqlCommand(cmdStr, cn);
        cmd.Parameters.Add("@msisdn", System.Data.SqlDbType.NVarChar).Value = msisdn;
        try
        {
            cmd.Connection.Open();
            int n = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Connection.Close();
            return n;

        }
        catch (Exception ex)
        {
            Tools.WriteLog(DateTime.Now.ToString() + ",IsRegister," + ex.Message, "Err");
            return -1;
        }
        finally
        {
            cmd.Connection.Close();
        }
    }

}