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
        url = "http://112.215.81.112:8080/Wap_action.jsp?";
        urlThanksPages = "http://topmodelindo.com/ThanksPremium.aspx";
        
        dest = "93818";
        content = "GIRL";

        url += "dest=" + dest;
        url += "&content=" + content + " " + Request["cat"] + " " + "WAP";
        url += "&success=" + urlThanksPages;

        Users.UpdateVideo(Request["cat"]);
        Tools.WriteLog(DateTime.Now.ToString() + ", " + url, "Subscribe");

        Response.Redirect(url);

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}
