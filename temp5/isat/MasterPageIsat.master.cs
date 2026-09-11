using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["msisdn"] != null && Request["msisdn"] != string.Empty && Request["msisdn"] != "" || Request["msisdn"] != "0")
            {
                int i = Users.IsRegister(Request["msisdn"].ToString());
                if (i < 1)
                    Response.Redirect("DefaultIsat.aspx");
            }
            BindArtist();
        }
    }

    public void BindArtist()
    {
        DataSet ds = Users.GetArtist();

        if (ds.Tables.Count > 0)
        {
            rptArtist.DataSource = ds;
            rptArtist.DataBind();
        }
    }
}
