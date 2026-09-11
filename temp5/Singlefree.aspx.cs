using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Singlefree : System.Web.UI.Page
{
    string content = string.Empty;
    string dest = string.Empty;
    string urlThanks = string.Empty;
    string url = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string msisdn = string.Empty;

        if (!IsPostBack)
        {
            if (Request["msisdn"] == null || Request["msisdn"] == string.Empty)
            {
                msisdn = string.Empty;
                Buy();
            }
            else
            {
                msisdn = Request["msisdn"];

                DataSet ds = Users.IsRegistered(msisdn);

                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    Response.Redirect("Default.aspx?msisdn=" + Request["msisdn"]);

                Users.UpdateVideo(Request["cat"]);
            }

            BindFreeVideo();
            BindUpNext();           
        }
    }

    public void BindFreeVideo()
    {
        Session["vtypeFree"] = "video/mp4";
        DataSet ds = Users.LoadFreeVideo("get", Request["cat"]);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["urlFree"] = ds.Tables[0].Rows[0]["VideoAddress"].ToString();
                lblFreeWallPaper.Text = ds.Tables[0].Rows[0]["ArtistName"].ToString();
            }
            else
                Response.Redirect("Default.aspx");
        } 
    }

    public void BindUpNext()
    {
        DataSet ds = new DataSet();

        if (Request["cat"] != null)
            ds = Users.LoadFreeVideo("upnext", Request["cat"]);
        else
            ds = Users.LoadFreeVideo("upnext", "");

        if (ds.Tables.Count > 0)
        {
            dlUpnext.DataSource = ds;
            dlUpnext.DataBind();
        }
    }

    protected void Buy()
    {
        string desc = string.Empty;
        string img = string.Empty;
        string callback = string.Empty;

        DataSet ds = Users.GetDetailVideoFree(Request["cat"]);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                desc = ds.Tables[0].Rows[0]["VideoDescription"].ToString();
                img = ds.Tables[0].Rows[0]["VideoLogo"].ToString();
            }
            else
                Response.Redirect("Default.aspx");
        }

        //content = "REG ANGEL";
        //dest = "93828";
        //url = "http://112.215.81.112:8080/Wap_action.jsp?";
        //http://www.gudangapp.com/xlp/?kc=REG+ANGEL&sdc=93828&cb=[callback]&desc=[description]&img=[imagepreview]&eid=a3f74
        
        url = "http://www.gudangapp.com/xlp/?kc=REG+ANGEL&sdc=93828";
        callback = "http://topmodelindo.com/ThanksPage.aspx";

        url += "&cb=" + callback;
        url += "&desc=" + desc;
        url += "&img=" + img;
        url += "&eid=a3f74";

        Response.Redirect(url);

        //WebClient wc = new WebClient();
        //string result = ASCIIEncoding.UTF8.GetString(wc.DownloadData(url));
        //Users.UpdateVideo(Request["cat"]);
        Tools.WriteLog(DateTime.Now.ToString() + ", " + url, "Subscribe");
    }
}
