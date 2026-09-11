using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class Menu : System.Web.UI.Page
    {
        private Model model = new Model();

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] useridentity = HttpContext.Current.User.Identity.Name.Split(';');

            DataSet menu = model.GetMenu(useridentity[0]);

            if (menu.Tables.Count > 0)
            {
                DataList1.DataSource = menu;
                DataList1.DataBind();
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                System.Web.HttpBrowserCapabilities browser = Request.Browser;
                string strURL = "Manual/CS Tool User Manual.pdf";
                string sBrowser = browser.Browser;
                string sURL = "http://210.5.41.105/csforest/";
                string sFileName = "CS Tool User Manual.pdf";
                WebRequest req = WebRequest.Create(sURL);
                WebResponse response = req.GetResponse();
                Stream stream = response.GetResponseStream();
                FileStream sourceFile = new FileStream(Server.MapPath(strURL), FileMode.Open);
                float FileSize;
                FileSize = sourceFile.Length;
                byte[] getContent = new byte[(int)FileSize];
                sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                sourceFile.Close();
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;

                string sFormat = sFileName.Substring(sFileName.IndexOf('.') + 1);
                Response.ContentType = sFormat;

                sFileName = strURL.Replace("Manual/", "");
                sFileName = sFileName.Replace(" ", "");

                Response.AddHeader("Content-Length", getContent.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + sFileName);
                Response.AppendHeader("Pragma", "no-cache");
                Response.BinaryWrite(getContent);
                Response.Flush();

                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
            }
        }
    }
}