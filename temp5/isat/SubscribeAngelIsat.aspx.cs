using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;

public partial class SubscribeAngle : System.Web.UI.Page
{
    string content = string.Empty;
    string dest = string.Empty;
    string url = string.Empty;
    string urlThanks = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnSubscribe_Click(object sender, EventArgs e)
    {
        string desc = string.Empty;
        string img = string.Empty;
        string callback = string.Empty;

        url = "http://202.152.162.239/isatlp/next/direct/?sdc=98890&kc=REG+ANGEL&cb=http://202.43.173.205/smob/wap/isat/promo/landing/thankmsg.html&desc=Top+Model+Forest+Niaga&img=http://202.152.162.239/ad/mcp/images_library/CPlogotype.jpg&price=1430&servicename=TOP+MODEL";
        Tools.WriteLog(DateTime.Now.ToString() + ", " + url, "Subscribe");

        Response.Redirect(url);

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("DefaultIsat.aspx");
    }
}