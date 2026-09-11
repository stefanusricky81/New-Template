using System;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ConnectionDatabase
/// </summary>
public class ClassConnectionDatabase
{
    SqlConnection conn = new SqlConnection();

    public bool ConnectionDatabase()
    {
        conn.ConnectionString = ConfigurationManager.ConnectionStrings["LittleSuperstarConecction"].ConnectionString;
        try
        {
            conn.Open();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public void RefreshConn()
    {
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
        }
        conn.Open();
    }

    public SqlConnection getconn()
    {
        return conn;
    }

    public void CloseConnection()
    {
        conn.Close();
    }

    public void Dispose()
    {
        conn.Dispose();
    }
}