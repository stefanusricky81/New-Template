using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_ListBox_lbClientContact : System.Web.UI.UserControl
{
    private string _defaultText = "";
    private string _cssClass = "form-control select-chosen";
    private int _clientId = 0;
    private bool _useEmailAsDataValue = false;
    private bool _useClientContactIdAsDataValue = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (lbClientContact.Items.Count == 0)
            Populate();
    }

    public void Populate(List<string> selectedTextValues = null)
    {
        if (ClientId.HasValue)
        {
            DataTable dtResult = DesktopShared.Client.GetActiveContacts(ClientId.Value, IsSilent);
            dtResult.DefaultView.Sort = "FirstName ASC";

            lbClientContact.DataSource = dtResult;
            lbClientContact.DataTextField = "FullName";
            if (_useEmailAsDataValue)
                lbClientContact.DataValueField = "Email";
            else if (_useClientContactIdAsDataValue)
                lbClientContact.DataValueField = "PclientContact";
            else
                lbClientContact.DataValueField = "Pusers";

            lbClientContact.DataBind();

            //iterate through collection and trim text
            foreach (ListItem li in lbClientContact.Items)
            {
                li.Text = li.Text.Trim();
                li.Value = li.Value.Trim();
            }

            if (selectedTextValues != null)
            {
                foreach (string contact in selectedTextValues)
                {
                    ListItem li = lbClientContact.Items.FindByText(contact.Trim());
                    if (li != null)
                        li.Selected = true;
                }
            }

            //css class
            if (!String.IsNullOrEmpty(_cssClass))
                lbClientContact.CssClass = _cssClass;
        }
    }

    #region public properties
    public bool UseEmailAsDataValue
    {
        set { _useEmailAsDataValue = value; }
    }
    public bool UseClientContactIdAsDataValue
    {
        set { _useClientContactIdAsDataValue = value; }
    }

    public int? ClientId
    {
        get
        {
            object obj = this.ViewState["cid_lbcc"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_lbcc"] = value; }
    }

    public string IsSilent
    {
        get
        {
            object obj = this.ViewState["lbcc_slnt"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["lbcc_slnt"] = value; }
    }
    /// <summary>
    /// get selected values
    /// </summary>
    public List<string> SelectedValues
    {
        get
        {
            List<string> _vals = new List<string>();
            foreach (ListItem _li in lbClientContact.Items)
            {
                if (_li.Selected)
                    _vals.Add(_li.Value.Trim());
            }
            return _vals;
        }
    }

    public List<string> SelectedText
    {
        get
        {
            List<string> _vals = new List<string>();
            foreach (ListItem _li in lbClientContact.Items)
            {
                if (_li.Selected)
                    _vals.Add(_li.Text.Trim());
            }
            return _vals;
        }
    }

    /// <summary>
    /// get/set user id
    /// </summary>
    public int UserId
    {
        get
        {
            object obj = this.ViewState["uid_lcc"];
            return (obj == null) ? DesktopShared.User.UserID : (int)obj;
        }
        set { this.ViewState["uid_lcc"] = value; }
    }

    /// <summary>
    /// get/set user tags only (exclude public tags)
    /// </summary>
    public bool DisplayClientContactOnly
    {
        get
        {
            object obj = this.ViewState["dcco_lb"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["dcco_lb"] = value; }
    }

    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { lbClientContact.TabIndex = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set { _cssClass = value; }
    }

    /// <summary>
    /// get list box
    /// </summary>
    /// <returns></returns>
    public ListBox GetListBox()
    {
        return lbClientContact;
    }

    /// <summary>
    /// set show chosen script
    /// </summary>
    public bool DisplayChosenScript
    {
        set { litJs.Visible = value; }
    }

    public string ValidationGroup
    {
        set
        {
            rfvClientContact.Visible = true;
            rfvClientContact.ValidationGroup = value;
        }
    }

    public string RequiredErrorMessage
    {
        set { rfvClientContact.ErrorMessage = value; }
    }

    #endregion

    #region private methods

    /// <summary>
    /// display script used by chosen plugin
    /// </summary>
    private void DisplayChosenPluginJs()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("SetChosenTicketTag_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenTicketTag_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenTicketTag_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", lbClientContact.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_multiple: \"Select Contact ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}