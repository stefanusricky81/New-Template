using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (Request["msisdn"] != string.Empty || Request["msisdn"] != null)
            //    Master.Modal.Visible = false;
   
            BindFreeWallPaper();
            BindFreeVideo();
            BindPremiumVideo();
            BindBanner();
        }
    }

    public void BindFreeWallPaper()
    {
        DataSet ds = Users.LoadWallPaper("new", "");

        if (ds.Tables.Count > 0)
        {
            dlFreeWallPaper.DataSource = ds;
            dlFreeWallPaper.DataBind();
        }
    }

    public void BindFreeVideo()
    {
        DataSet ds = Users.LoadFreeVideo("new", "");

        if (ds.Tables.Count > 0)
        {
            dlFreeVideo.DataSource = ds;
            dlFreeVideo.DataBind();
        }
    }

    public void BindPremiumVideo()
    {
        DataSet ds = Users.LoadPremiumVideo("new", "");

        if (ds.Tables.Count > 0)
        {
            dlPremiumVideo.DataSource = ds;
            dlPremiumVideo.DataBind();
        }
    }

    public void BindBanner()
    {
        DataSet ds = Users.LoadBanner();

        if (ds.Tables.Count > 0)
        {
            rptBanner.DataSource = ds;
            rptBanner.DataBind();
        }
    }

    protected void btnFreeWallpaper_Click(object sender, EventArgs e)
    {
        Response.Redirect("FreeWallPaper.aspx?msisdn=" + Request["msisdn"]);
    }

    protected void btnFreeVideo_Click(object sender, EventArgs e)
    {
        Response.Redirect("FreeVideo.aspx?msisdn=" + Request["msisdn"]);
    }

    protected void btnPremiumVideo_Click(object sender, EventArgs e)
    {
        Response.Redirect("Premiumvideo.aspx?msisdn=" + Request["msisdn"]);
    }
}