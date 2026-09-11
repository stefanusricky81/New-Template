using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;


public partial class Client_ClientContactDetail : BasePage
{
     /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ucEditForm.evButtonClick += new EventHandler(btnUpdate_Click); //event handler for update/add button click on user control

        if (!this.IsPostBack)
        {
            if (GetId())
                LoadData(); //display entity
            else
                RecordNotFound(); //unable to locate entity
        }
    }

    #region private methods / evnts

    /// <summary>
    /// load client contact
    /// entity
    /// </summary>
    private void LoadData()
    {
        if (ClientContactId > 0)
        {
            DesktopShared.EntityClasses.ClientContactEntity clientcont = 
                new DesktopShared.EntityClasses.ClientContactEntity(ClientContactId);

            if (clientcont.Fields.State == EntityState.Fetched)
                ucEditForm.LoadValues(ClientContactId);
            else
                RecordNotFound();
        }
        else
        {
            litHeaderText.Text = "Add";
            Page.Title = "Bit By Bit Intranet - Client Contact Add";

            ucEditForm.LoadValues(0);
        }
    }

    /// <summary>
    /// update/add button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnUpdate_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            int _newContactId = ucEditForm.SaveValues(ClientContactId);
            litMessage.Visible = true;

            if (ClientContactId > 0)
                litMessage.Text = "<b>Update is complete.</b>";
            else
            {
                ClientContactId = _newContactId;
                litMessage.Text = "<b>Insert is complete.</b>";
            }

            LoadData();
        }
    }

    /// <summary>
    /// record not found
    /// </summary>
    private void RecordNotFound()
    {
        ucEditForm.Visible = false;
        pnlNotFound.Visible = true;
        litMessage.Visible = false;

        litErrorId.Text = ClientContactId.ToString(); 
    }

    /// <summary>
    /// get id from query string
    /// </summary>
    /// <returns></returns>
    private bool GetId()
    {
        try { ClientContactId = Convert.ToInt32(BitByBit.Web.Request.GetString("Id")); }
        catch { ClientContactId = -1; }

        return ClientContactId > -1;
    }

    #endregion

    #region private properties

    /// <summary>
    /// get/set  client contact id
    /// </summary>
    private int ClientContactId
    {
        get
        {
            object obj = this.ViewState["ClientContactId"];
            if (obj == null)
                return 0;
            else
                return (int)obj;
        }
        set { this.ViewState["ClientContactId"] = value; }
    }

    private int PclientContact
    {
        get
        {
            object obj = this.ViewState["PclientContact"];
            if (obj == null)
                return 0;
            else
                return (int)obj;
        }
        set { this.ViewState["PclientContact"] = value; }
    }

    #endregion
}