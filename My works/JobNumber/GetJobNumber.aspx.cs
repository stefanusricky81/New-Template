using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Client_GetJobNumber : System.Web.UI.Page
{
    private string connectionString = ConfigurationManager.AppSettings["ConnectionString.SQL Server (SqlClient)"];

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ddlClient.ClientId != null)
        {
            string _emplcode = employecode().Trim();
            int _jobnum = insertjob(_emplcode);
            Response.Redirect("~/JobNumber/GetJobNumberResult.aspx?n=" + _jobnum);
        }
        else
            DisplayMessage("Please choose client first", Bootstrap.Alert.AlertType.Warning);
    }
    private string employecode()
    {
        SqlConnection connection = new SqlConnection(connectionString);

        string cmdStr = "select code from [bitbybit].[dbo].[employee] where pemployee = @fkempl";
        SqlCommand cmd = new SqlCommand(cmdStr, connection);
        cmd.Parameters.Add("@fkempl", System.Data.SqlDbType.Int).Value = DesktopShared.User.EmployeeID;
        try
        {
            cmd.Connection.Open();
            string n = cmd.ExecuteScalar().ToString();
            return n;
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
        finally
        {
            cmd.Connection.Close();
        }
    }

    private int insertjob(string employeecode)
    {
        SqlConnection connection = new SqlConnection(connectionString);
        string sql = "Insert Into bbbjob (fk_employee,fk_client,empl_code,last_updated,last_updatedby) " +
                    "output INSERTED.pbbbjob Values(@fkemployee,@fkclient,@emplcode,Getdate(),@lastupdateby); ";

        using (SqlCommand cmd = new SqlCommand(sql, connection))
        {
            cmd.Parameters.AddWithValue("@fkemployee", DesktopShared.User.EmployeeID);
            cmd.Parameters.AddWithValue("@fkclient", ddlClient.ClientId);
            cmd.Parameters.AddWithValue("@emplcode", employeecode);
            cmd.Parameters.AddWithValue("@lastupdateby", employeecode);
            connection.Open();

            int modified = (int)cmd.ExecuteScalar();

            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();

            return modified;
        }
       
    }
}