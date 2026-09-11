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

public partial class UserControl_DropDownList_ActiveStatus2 : System.Web.UI.UserControl
{
    private string _cssClass = "form-control select-chosen";
    private string _chosentPlaceholderText = "Select a Status ...";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
    }

    #region public properties

    /// <summary>
    /// set drop down list width
    /// </summary>
    public Unit Width
    {
        set { ddlActiveStatus.Width = value; }
    }

    /// <summary>
    /// get drop down list control
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlActiveStatus;
    }

    /// <summary>
    /// get/set active 
    /// </summary>
    public bool? Active
    {
        get
        {
            string _selectedValue = ddlActiveStatus.SelectedValue.Trim();
            if (_selectedValue.Length == 0)
                return null;

            return _selectedValue == "1";
        }
        set
        {
            string _value = "";
            if (value.HasValue)
                _value = value.Value ? "1" : "0";
            ListItem _li = ddlActiveStatus.Items.FindByValue(_value);
            if (_li != null)
                _li.Selected = true;
        }
    }

    /// <summary>
    /// get selected active status name
    /// </summary>
    public string ActiveStatusName
    {
        get { return ddlActiveStatus.SelectedItem.Text.Trim(); }
    }

    /// <summary>
    /// set validation group 
    /// </summary>
    public string ValidationGroup
    {
        set { rfvActiveStatus.ValidationGroup = value; }
    }

    /// <summary>
    /// set selection is requried
    /// </summary>
    public bool IsRequired
    {
        set { rfvActiveStatus.Visible = value; }
    }

    /// <summary>
    /// set error message for required field validator
    /// </summary>
    public string RequiredErrorMessage
    {
        set { rfvActiveStatus.ErrorMessage = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set { ddlActiveStatus.CssClass = value; }
    }

    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { ddlActiveStatus.TabIndex = value; }
    }

    /// <summary>
    /// set default value for list item
    /// </summary>
    public string DefaultValue
    {
        set { ddlActiveStatus.Items[0].Value = value; }
    }

    /// <summary>
    /// set default text for list item
    /// </summary>
    public string DefaultText
    {
        set { ddlActiveStatus.Items[0].Text = value; }
    }

    /// <summary>
    /// set show chosen script
    /// </summary>
    public bool DisplayChosenScript
    {
        set { litJs.Visible = value; }
    }

    /// <summary>
    /// set text to use for placeholder for chosen pluging
    /// </summary>
    public string ChosentPlaceholderText
    {
        set { _chosentPlaceholderText = value; }
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
        sb.Append(String.Format("SetChosenActiveStatus_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenActiveStatus_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenActiveStatus_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlActiveStatus.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append(String.Format("placeholder_text_single: \"{0}\",", _chosentPlaceholderText));
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion

}
