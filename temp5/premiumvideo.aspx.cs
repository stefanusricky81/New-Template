using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class premiumvideo : System.Web.UI.Page
{
    int pos;
    PagedDataSource adsource;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ViewState["vs"] = 0;
        }
        BindPremiumVideo();
    }

    public void BindPremiumVideo()
    {
        DataSet ds = new DataSet();

         if(Request["cat"]!=null)
            ds = Users.LoadPremiumVideo("all", Request["cat"]);
        else
             ds = Users.LoadPremiumVideo("alldata", "");

        adsource = new PagedDataSource();
        adsource.DataSource = ds.Tables[0].DefaultView;
        adsource.PageSize = 4;
        adsource.AllowPaging = true;
        adsource.CurrentPageIndex = pos;
        btnfirst.Enabled = !adsource.IsFirstPage;
        btnprevious.Enabled = !adsource.IsFirstPage;
        btnlast.Enabled = !adsource.IsLastPage;
        btnnext.Enabled = !adsource.IsLastPage;

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
        BindPremiumVideo();
    }

    protected void btnprevious_Click(object sender, EventArgs e)
    {
        pos = (int)this.ViewState["vs"];
        pos -= 1;
        this.ViewState["vs"] = pos;
        BindPremiumVideo();
    }

    protected void btnnext_Click(object sender, EventArgs e)
    {
        pos = (int)this.ViewState["vs"];
        pos += 1;
        this.ViewState["vs"] = pos;
        BindPremiumVideo();
    }

    protected void btnlast_Click(object sender, EventArgs e)
    {
        pos = adsource.PageCount - 1;
        this.ViewState["vs"] = pos;
        BindPremiumVideo();
    }
}
