using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class singlewallpaper : System.Web.UI.Page
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
            if (Request["msisdn"] == null ||Request["msisdn"] ==string.Empty)
            {
                msisdn = string.Empty;
                btnDownload.Visible = false;
                Buy();
            }
            else
            {
                msisdn = Request["msisdn"];

                DataSet ds = Users.IsRegistered(msisdn);
                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    Response.Redirect("Default.aspx?msisdn=" + Request["msisdn"]);
            }
            BindFreeWallPaper();
        }
    }

    public void BindFreeWallPaper()
    {
        DataSet ds = Users.LoadWallPaper("get", Request["cat"]);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                imgWallPaper.ImageUrl = ds.Tables[0].Rows[0]["WallPaperAddress"].ToString();
                lblFreeWallPaper.Text = ds.Tables[0].Rows[0]["ArtistName"].ToString();
            }
            else
                Response.Redirect("Default.aspx");
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        DataSet ds = Users.LoadWallPaper("get", Request["cat"]);

        if (ds.Tables.Count > 0)
        {
            Users.UpdateWallPaper(Request["cat"]);

            //Response.Redirect();
            string filename = MapPath(ds.Tables[0].Rows[0]["WallPaperAddress"].ToString());
            Response.ContentType = "image/JPEG";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + "");

            Response.TransmitFile(filename);
            Response.End();
        }

        
    }

    protected void btnHome_Click(object sender, EventArgs e)
    {
        string msisdn = string.Empty;

        if (Request["msisdn"] != null)
            msisdn = Request["msisdn"];
        else
            msisdn = string.Empty;

        Response.Redirect("Default.aspx?msisdn=" + msisdn);
    }

    protected void Buy()
    {
        string desc = string.Empty;
        string img = string.Empty;
        string callback = string.Empty;

        DataSet ds = Users.GetDetailWallPaperFree(Request["cat"]);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                desc = ds.Tables[0].Rows[0]["WallPaperDescription"].ToString();
                img = ds.Tables[0].Rows[0]["WallPaperLogo"].ToString();
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
        //Users.UpdateWallPaper(Request["cat"]);
        Tools.WriteLog(DateTime.Now.ToString() + ", " + url, "Subscribe");
    }
}
