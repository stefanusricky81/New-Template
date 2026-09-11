using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Typeahead_Client : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region public methods

    /// <summary>
    /// clear all items from combo box
    /// </summary>
    public void ClearItems()
    {
        txtClient.Value = "";
    }

    #endregion

    #region public properties

    /// <summary>
    /// set selectd client to be used for query
    /// </summary>
    public int? SelectedClientId
    {
        set { txtClientHidden.Text = value.HasValue ? value.Value.ToString() : ""; }
    }

    /// <summary>
    /// get selected client id
    /// </summary>
    public int? ClientId
    {
        get
        {
            if (!String.IsNullOrEmpty(txtClientHidden.Text))
            {
                int _id = -1;
                if (int.TryParse(txtClientHidden.Text, out _id))
                    return _id;
                else
                    return null;
            }
            else
                return null;
        }
        set
        {
            txtClient.Value = "";
            txtClientHidden.Text = "";
            if (!value.HasValue)
                return;

            var objClient = new DesktopShared.EntityClasses.ClientEntity(value.Value);
            if (objClient.Fields.State != SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched)
                return;

            txtClient.Value = objClient.Company.Trim();
            txtClientHidden.Text = value.Value.ToString();
        }
    }

    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { txtClient.Attributes.Add("tabindex", value.ToString()); }
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
    /// get html text box
    /// </summary>
    public System.Web.UI.HtmlControls.HtmlInputText HtmlTextBox
    {
        get { return txtClient; }
    }

    /// <summary>
    /// get hidden text box for auto postback 
    /// </summary>
    public TextBox HiddenTextBox
    {
        get { return txtClientHidden; }
    }

    #endregion

    #region custom validators

    /// <summary>
    /// validate client selected
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvClient_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = ClientId.HasValue;
    }

    #endregion
}