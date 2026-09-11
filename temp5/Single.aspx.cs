using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Single : System.Web.UI.Page
{
    string content = string.Empty;
    string dest = string.Empty;
    string urlThanksPages = string.Empty;
    string url = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnBuy_Click(sender, e);
            //BindPremiumVideo();
        }
    }

    public void BindPremiumVideo()
    {
        Session["vtypePremium"] = "video/mp4";
        DataSet ds = Users.LoadPremiumVideo("get", Request["cat"]);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["urlPremium"] = ds.Tables[0].Rows[0]["VideoAddress"].ToString();
                Session["VideoLogo"] = ds.Tables[0].Rows[0]["VideoLogo"].ToString();
                Session["VideoTitle"] = ds.Tables[0].Rows[0]["VideoTitle"].ToString();
                lblFreeWallPaper.Text = ds.Tables[0].Rows[0]["ArtistName"].ToString();
            }
            else
                Response.Redirect("Default.aspx");
        }
    }

    protected void btnBuy_Click(object sender, EventArgs e)
    {
        //Response.Redirect("Subscribe.aspx?cat=" + Request["cat"]);

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
}
