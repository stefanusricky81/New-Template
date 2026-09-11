using DesktopShared.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Client_ClientNotes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            SetUpPage();
            DesktopShared.EntityClasses.ClientEntity objClient = null;
            if (!GetClientId(ref objClient))
                return;

            LoadValues(objClient);
        }
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        try {
            DesktopShared.EntityClasses.ClientEntity objClient = new ClientEntity(SelectedClientId.Value);

            objClient.ClientNotes = txtNotes.Content;
            objClient.Save();
            objClient.Refetch();

            LoadValues(objClient);
        }
        catch (Exception ex)
        { }
    }

    #region private method
    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, bool hideMainPanel = false)
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, "");
        pnlMain.Visible = !hideMainPanel;
    }

    private bool GetClientId(ref DesktopShared.EntityClasses.ClientEntity objClient)
    {
        try
        {
            int _id = -1;
            if (int.TryParse(BitByBit.Web.Request.GetString("ClientID").Trim(), out _id))
            {
                SelectedClientId = _id;
                objClient = new ClientEntity(_id);
                if (objClient.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched)
                    return true;
            }

            DisplayMessage(String.Format("Unable to fetch client.  ID = {0}", _id > 0 ? _id.ToString() : "N/A"), Bootstrap.Alert.AlertType.Danger, false, true);
            return false;
        }
        catch
        { return false; }
    }

    private void SetUpPage()
    {
        txtNotes.Focus();

        #region tab index

        short _tabIndex = 0;

        txtNotes.TabIndex = ++_tabIndex;
        #endregion
    }

    private void LoadValues(DesktopShared.EntityClasses.ClientEntity objClient)
    {
        try
        {
            txtNotes.Content = objClient.ClientNotes;
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message, Bootstrap.Alert.AlertType.Warning);
        }

    }
    #endregion

    #region private properties
    private int? SelectedClientId
    {
        get
        {
            object obj = this.ViewState["scid"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["scid"] = value; }
    }
    #endregion
}