using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControl_DropDownList_SmartClient : System.Web.UI.UserControl
{
    private string _cssClass = "form-control select-chosen";

    protected void Page_Load(object sender, EventArgs e)
    {
    }


    #region public methods

    /// <summary>
    /// clear all items from combo box
    /// </summary>
    public void ClearItems()
    {
        rcbClient.Text = "";
        rcbClient.Items.Clear();
        rcbClient.Items.Add(new RadComboBoxItem("", ""));
    }

    public void rcbClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        // Raise event to RadGrid parent
        if (ClientChanged != null)
            ClientChanged(this, EventArgs.Empty);
    }

    #endregion

    #region public properties
    public event EventHandler ClientChanged;

    /// <summary>
    /// set/set selected client id
    /// </summary>
    public int SelectedClientId
    {
        get
        {
            if (!String.IsNullOrEmpty(rcbClient.SelectedValue))
            {
                try { return Convert.ToInt32(rcbClient.SelectedValue); }
                catch { return -1; }

            }
            else
            {
                return -1;
            }
        }
        set
        {
            rcbClient.Text = "";
            rcbClient.Items.Clear();

            DesktopShared.EntityClasses.ClientEntity client = new DesktopShared.EntityClasses.ClientEntity(value);
            if (client.Fields.State == EntityState.Fetched)
            {
                rcbClient.Items.Insert(0, new RadComboBoxItem(client.Company, client.Pclient.ToString()));
                rcbClient.SelectedIndex = 0;
            }
        }
    }

    /// <summary>
    /// get selected client name
    /// </summary>
    public string SelectedClientName
    {
        get
        {
            if (!String.IsNullOrEmpty(rcbClient.SelectedValue))
                return rcbClient.Text;
            else
                return "";
        }
    }

    /// <summary>
    /// set is required
    /// </summary>
    public bool IsRequired
    {
        set { cvClient.Visible = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ValidationErrorMessage
    {
        set { cvClient.ErrorMessage = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { cvClient.ValidationGroup = value; }
    }

    /// <summary>
    /// set combo box control width
    /// </summary>
    public Unit Width
    {
        set { rcbClient.Width = value; }
    }

    /// <summary>
    /// get combo box
    /// </summary>
    public RadComboBox ComboBox
    {
        get { return rcbClient; }
    }
    public short TabIndex
    {
        set { rcbClient.TabIndex = value; }
    }

    public RadComboBox GetComboBox()
    {
        return rcbClient;
    }

    public string CssClass
    {
        set
        {
            rcbClient.CssClass = value;
            _cssClass = value;
        }
        //set { _cssClass = value; } // ddlClient.CssClass = value; }
    }

    public bool AutoPostback
    {
        set { rcbClient.AutoPostBack = value; }
    }


    #endregion

    #region custom validators

    /// <summary>
    /// validate client selected from combo box
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvClient_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (SelectedClientId < 1)
            ClearItems();

        args.IsValid = SelectedClientId > 0;
    }

    #endregion
}