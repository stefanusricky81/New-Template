using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class FreeContent : System.Web.UI.Page
{
    int pos;
    PagedDataSource adsource;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ViewState["vs"] = 0;
        }

        BindFreeVideo();
        BindFreeWallPaper();
    }

    public void BindFreeVideo()
    {
        DataSet ds = new DataSet();

        if (Request["cat"] != null)
            ds = Users.LoadFreeVideo("all", Request["cat"]);
        else
            ds = Users.LoadFreeVideo("alldata", "");

        adsource = new PagedDataSource();
        adsource.DataSource = ds.Tables[0].DefaultView;
        adsource.PageSize = 4;
        adsource.AllowPaging = true;
        adsource.CurrentPageIndex = pos;

        if (ds.Tables.Count > 0)
        {
            dlFreeVideo.DataSource = adsource;
            dlFreeVideo.DataBind();
        }
    }

    protected void btnfirst_Click(object sender, EventArgs e)
    {
        pos = 1;
        this.ViewState["vs"] = pos;
        BindFreeVideo();
    }

    protected void btnprevious_Click(object sender, EventArgs e)
    {
        pos = (int)this.ViewState["vs"];
        pos -= 1;
        this.ViewState["vs"] = pos;
        BindFreeVideo();
    }

    protected void btnnext_Click(object sender, EventArgs e)
    {
        pos = (int)this.ViewState["vs"];
        pos += 1;
        this.ViewState["vs"] = pos;
        BindFreeVideo();
    }

    protected void btnlast_Click(object sender, EventArgs e)
    {
        pos = adsource.PageCount - 1;
        this.ViewState["vs"] = pos;
        BindFreeVideo();
    }

    public void BindFreeWallPaper()
    {
        DataSet ds = new DataSet();

        if (Request["cat"] != null)
            ds = Users.LoadWallPaper("all", Request["cat"]);
        else
            ds = Users.LoadWallPaper("alldata", "");

        adsource = new PagedDataSource();
        adsource.DataSource = ds.Tables[0].DefaultView;
        adsource.PageSize = 4;
        adsource.AllowPaging = true;
        adsource.CurrentPageIndex = pos;

        if (ds.Tables.Count > 0)
        {
            dlFreeWallPaper.DataSource = adsource;
            dlFreeWallPaper.DataBind();
        }
    }

    protected void btnFreeWallpaper_Click(object sender, EventArgs e)
    {
        Response.Redirect("FreeWallPaperIsat.aspx?msisdn=" + Request["msisdn"]);
    }

    protected void btnFreeVideo_Click(object sender, EventArgs e)
    {
        Response.Redirect("FreeVideoIsat.aspx?msisdn=" + Request["msisdn"]);
    }
}