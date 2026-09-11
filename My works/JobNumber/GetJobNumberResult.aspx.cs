using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Client_GetJobNumberResult : System.Web.UI.Page
{
    private static string sStrConnection = ConfigurationManager.AppSettings["ConnectionString.SQL Server (SqlClient)"];
    public static SqlConnection GetConnection()
    {
        return new SqlConnection(sStrConnection);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int i = 0;
        if (Request["n"] != null)
        {
            i = Convert.ToInt32(Request["n"].ToString());
            DisplayMessage(String.Format("Job number has been {0} - {1}.", "added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
        }
        else
            Response.Redirect("~/JobNumber/GetJobNumber.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/JobNumber/GetJobNumber.aspx");
    }

    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    protected void rgJobNumber_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int _jobnum = Convert.ToInt32(Request["n"].ToString());
        if(getdata(_jobnum)!=null)
            rgJobNumber.DataSource = getdata(_jobnum);
        else
            DisplayMessage("Incorrect job number", Bootstrap.Alert.AlertType.Danger);
    }

    protected DataTable getdata(int jobnum)
    {
        DataTable dt = new DataTable();
        string query = string.Empty;

        query = "select a.pbbbjob, b.company, a.empl_code,a.fk_client FROM[bitbybit].[dbo].[bbbJob] a " +
                "inner join [bitbybit].[dbo].[client] b on a.fk_client= b.pclient where a.pbbbjob = @pjob";
        try
        {
            using (GetConnection())
            {
                GetConnection().Open();
                SqlDataAdapter adp = new SqlDataAdapter(query, GetConnection());
                adp.SelectCommand.CommandType = CommandType.Text;
                adp.SelectCommand.Parameters.Add("@pjob", SqlDbType.Int).Value = jobnum;

                dt.Reset();
                adp.Fill(dt);
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        return dt;
    }
}