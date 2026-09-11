using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;

public partial class Subscribe : System.Web.UI.Page
{
    string content = string.Empty;
    string dest = string.Empty;
    string urlThanksPages = string.Empty;
    string url = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnSubscribe_Click(object sender, EventArgs e)
    {
        url = "http://202.152.162.239/isatlp/next/direct/?sdc=98890&kc=ANGEL+VD&cb=http://202.43.173.205/smob/wap/isat/promo/landing/thankmsg.html&desc=Top+Model+Forest+Niaga&img=http://202.152.162.239/ad/mcp/images_library/CPlogotype.jpg&price=3300&servicename=TOP+MODEL+VD";
        //url = "http://112.215.81.112:8080/Wap_action.jsp?";
        //urlThanksPages = "http://topmodelindo.com/isat/ThanksPremiumIsat.aspx";
        
        //dest = "93818";
        //content = "GIRL";

        //url += "dest=" + dest;
        //url += "&content=" + content + " " + Request["cat"] + " " + "WAP";
        //url += "&success=" + urlThanksPages;

        //Users.UpdateVideo(Request["cat"]);
        Tools.WriteLog(DateTime.Now.ToString() + ", " + url, "Subscribe");

        Response.Redirect(url);

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("DefaultIsat.aspx");
    }
}
