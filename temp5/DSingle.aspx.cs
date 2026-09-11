using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class DSingle : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindPremiumVideo();
            BindUpNext();
        }
    }

    public void BindPremiumVideo()
    {
        Session["vtypePremium"] = "video/mp4";
        DataSet ds = Users.LoadPremiumVideo("get", Request["cat"]);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["urlPremium"] = ds.Tables[0].Rows[0]["VideoAddress"].ToString();
                Session["VideoLogo"] = ds.Tables[0].Rows[0]["VideoLogo"].ToString();
                Session["VideoTitle"] = ds.Tables[0].Rows[0]["VideoTitle"].ToString();
            }
            else
                Response.Redirect("Default.aspx");
        }
    }

    public void BindUpNext()
    {
        DataSet ds = new DataSet();

        if (Request["cat"] != null)
            ds = Users.LoadPremiumVideo("upnext", Request["cat"]);
        else
            ds = Users.LoadPremiumVideo("upnext", "");

        if (ds.Tables.Count > 0)
        {
            dlUpnext.DataSource = ds;
            dlUpnext.DataBind();
        }
    }

    protected void btnDownload_Clicka(object sender, EventArgs e)
    {
        DataSet ds = Users.LoadPremiumVideo("get", Request["cat"]);

        if (ds.Tables.Count > 0)
        {
            Users.UpdateWallPaper(Request["cat"]);

            //Response.Redirect();
            string filename = MapPath(ds.Tables[0].Rows[0]["VideoAddress"].ToString());
            Response.ContentType = "video/mp4";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + "");

            Response.TransmitFile(filename);
            Response.End();
        }


    }

    protected void btnHome_Clicka(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}
