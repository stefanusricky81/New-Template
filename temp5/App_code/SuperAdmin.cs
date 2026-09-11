using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SuperAdmin
/// </summary>
public class SuperAdmin
{
    private static string sStrConnectionMOMT = "server=localhost;User id=dbuser;password=server9@9@;database=MOMT_NEW";
    private static string sStrConnectionTopModel = "server=localhost;User id=dbuser;password=server9@9@;database=TopModelIndo";

    //private static string sStrConnectionMOMT = "server=117.102.120.139;User id=dbuser;password=server9@9@;database=MOMT_NEW";
    //private static string sStrConnectionTopModel = "server=117.102.120.139;User id=dbuser;password=server9@9@;database=TopModelIndo";

    public SuperAdmin()
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
            using (SqlConnection cn = new SqlConnection(sStrConnectionTopModel))
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

    public static DataSet LoadReportTopModel(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnectionTopModel))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("GET_REPORT_TOPMODELINDO", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;

            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadSubs(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnectionTopModel))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("SP_TMI_GET_ACTIVESUBS", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadMOREG(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnectionTopModel))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("SP_MOMT_GET_REGSUBS", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadAllMOREG(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnectionTopModel))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("SP_MOMT_GET_ALLREGSUBS", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadMOUnREG(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnectionTopModel))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("SP_MOMT_GET_MOUNREG", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadAllMOUnREG(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnectionTopModel))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("SP_MOMT_GET_ALLMOUNREG", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadMOPullAndPush(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnectionTopModel))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("SP_MOMT_GET_MOPULLANDPUSH", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadAllMOPullAndPush(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnectionTopModel))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("SP_MOMT_GET_ALLMOPULLANDPUSH", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadReportRetryREG(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnectionTopModel))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("GET_REPORT_RETRYREG", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;

            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadReportRetryREN(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnectionTopModel))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("GET_REPORT_RETRYREN", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;

            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadCountDataRetry(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnectionTopModel))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("SP_COUNTFOR_RETRY", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }

    public static DataSet LoadCountDataRetryREN(DateTime start, DateTime end)
    {
        DataSet ds = new DataSet();

        using (SqlConnection cn = new SqlConnection(sStrConnectionTopModel))
        {
            cn.Open();
            SqlDataAdapter adp = new SqlDataAdapter("SP_COUNTFOR_RETRYREN", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            adp.SelectCommand.Parameters.Add("@datefrom", SqlDbType.DateTime).Value = start;
            adp.SelectCommand.Parameters.Add("@dateto", SqlDbType.DateTime).Value = end;
            ds.Reset();
            adp.Fill(ds);
        }
        return ds;
    }
}