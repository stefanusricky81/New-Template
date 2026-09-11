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
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_ComboBox_SmartClient : System.Web.UI.UserControl
{
    //todo: once client add/ edit page is created, remove cache upon insert/update on those pages

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

    #endregion

    #region public properties

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
        set { cvClient.Visible = value;}
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
        get { return rcbClient;  }
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

    protected void rcbClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        divQuickLinks.Visible = true;
    }
}
