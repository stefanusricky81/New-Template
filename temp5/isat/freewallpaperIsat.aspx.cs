using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class freewallpaper : System.Web.UI.Page
{
    int pos;
    PagedDataSource adsource;

    protected void Page_Load(object sender, EventArgs e)
    {
        string msisdn = string.Empty;

        if (!IsPostBack)
        {
            if (Request["msisdn"] == null || Request["msisdn"] == string.Empty)
            {
                Response.Redirect("SubscribeAngelIsat.aspx");
            }
            this.ViewState["vs"] = 0;
        }
        BindFreeWallPaper();
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
        btnfirst.Enabled = !adsource.IsFirstPage;
        btnprevious.Enabled = !adsource.IsFirstPage;
        btnlast.Enabled = !adsource.IsLastPage;
        btnnext.Enabled = !adsource.IsLastPage;

        if (ds.Tables.Count > 0)
        {
            dlFreeWallPaper.DataSource = adsource;
            dlFreeWallPaper.DataBind();
        }
    }

    protected void btnfirst_Click(object sender, EventArgs e)
    {
        pos = 1;
        this.ViewState["vs"] = pos;
        BindFreeWallPaper();
    }

    protected void btnprevious_Click(object sender, EventArgs e)
    {
        pos = (int)this.ViewState["vs"];
        pos -= 1;
        this.ViewState["vs"] = pos;
        BindFreeWallPaper();
    }

    protected void btnnext_Click(object sender, EventArgs e)
    {
        pos = (int)this.ViewState["vs"];
        pos += 1;
        this.ViewState["vs"] = pos;
        BindFreeWallPaper();
    }

    protected void btnlast_Click(object sender, EventArgs e)
    {
        pos = adsource.PageCount - 1;
        this.ViewState["vs"] = pos;
        BindFreeWallPaper();
    }
}
