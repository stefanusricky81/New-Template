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

        content = "REG ANGEL WAP";
        dest = "93828";
        url = "http://112.215.81.112:8080/Wap_action.jsp?";
        urlThanks = "http://topmodelindo.com/ThanksPage.aspx";

        url += "dest=" + dest;
        url += "&content=" + content;
        url += "&success=" + urlThanks;

        //url = "http://www.gudangapp.com/xlp/?kc=REG+ANGEL+WAP&sdc=93828";
        //callback = "http://topmodelindo.com/ThanksPage.aspx";


        //url += "&cb=" + callback;
        //url += "&desc=" + "Top Model Indonesia";
        //url += "&img=" + img;
        //url += "&eid=a3f74";
        //Users.UpdateVideo(Request["cat"]);
        Tools.WriteLog(DateTime.Now.ToString() + ", " + url, "Subscribe");

        Response.Redirect(url);

        //WebClient wc = new WebClient();
        //string result = ASCIIEncoding.UTF8.GetString(wc.DownloadData(url));

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}