using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_DropDownList_Team : System.Web.UI.UserControl
{
    private string _defaultText = "";
    private string _defaultValue = "-1";
    private bool _displayDefaultValue = true;
    private bool _setSize = true;
    private int _selectedId = 0;
    private string _cssClass = "";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (ddlTeam.Items.Count == 0)
            Populate();
    }

    #region public methods

    /// <summary>
    /// populate drop down list
    /// </summary>
    public void Populate()
    {
        //get collection of active teams
        DesktopShared.CollectionClasses.TeamCollection _teams = DesktopShared.Team.GetActiveTeams();

        //bind drop down list
        ddlTeam.DataSource = _teams;
        ddlTeam.DataTextField = "Name";
        ddlTeam.DataValueField = "Id";
        ddlTeam.DataBind();

        //find by selected id
        if (_selectedId > 0)
        {
            ListItem li = ddlTeam.Items.FindByValue(_selectedId.ToString());
            if (li != null)
                li.Selected = true;
        }

        //dispaly default value
        if (_displayDefaultValue)
            ddlTeam.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

        //fixed height
        if (_setSize)
            ddlTeam.Height = Unit.Pixel(20);

        //css class
        if (!String.IsNullOrEmpty(_cssClass))
            ddlTeam.CssClass = _cssClass;
    }

    #endregion

    #region public properties

    /// <summary>
    /// set fixed size / height
    /// </summary>
    public bool SetSize
    {
        set { _setSize = value; }
    }

    /// <summary>
    /// set drop down list width
    /// </summary>
    public Unit Width
    {
        set { ddlTeam.Width = value; }
    }

    /// <summary>
    /// get drop down list control
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlTeam;
    }

    /// <summary>
    /// get/set team id
    /// </summary>
    public int? TeamId
    {
        get
        {
            if (ddlTeam.SelectedIndex == 0 && (ddlTeam.SelectedValue == _defaultValue))
                return null;

            int _id = 0;
            if (int.TryParse(ddlTeam.SelectedValue, out _id))
                return _id;
            return null;

        }
        set
        {
            if (value.HasValue)
            {
                _selectedId = value.Value;
                Populate();
            }
            else
                ddlTeam.ClearSelection();
        }
    }

    /// <summary>
    /// get selected team name
    /// </summary>
    public string TeamName
    {
        get { return ddlTeam.SelectedItem.Text.Trim(); }
    }

    /// <summary>
    /// set text for default list item
    /// </summary>
    public string DefaultText
    {
        set { _defaultText = value; }
    }

    /// <summary>
    /// set value for default list item
    /// </summary>
    public string DefaultValue
    {
        set { _defaultValue = value; }
    }

    /// <summary>
    /// set display default list item
    /// </summary>
    public bool DisplayDefaultValue
    {
        set { _displayDefaultValue = value; }
    }

    /// <summary>
    /// set validation group 
    /// </summary>
    public string ValidationGroup
    {
        set { rfvTeam.ValidationGroup = value; }
    }

    /// <summary>
    /// set selection is requried
    /// </summary>
    public bool IsRequired
    {
        set { rfvTeam.Visible = value; }
    }

    /// <summary>
    /// set error message for required field validator
    /// </summary>
    public string RequiredErrorMessage
    {
        set { rfvTeam.ErrorMessage = value; }
    }

    /// set css class for drop down list control
    /// </summary>
    public string CssClass
    {
        set
        {
            ddlTeam.CssClass = value;
            _cssClass = value;
        }
    }

    /// <summary>
    /// set display of required field validator
    /// </summary>
    public ValidatorDisplay RequiredFieldDisplay
    {
        set { rfvTeam.Display = value; }
    }

    // <summary>
    /// set display asterisk for error
    /// </summary>
    public bool DisplayErrorAsterisk
    {
        set { litError.Visible = value; }
    }

    /// <summary>
    /// set tab index
    /// </summary>
    public short TabIndex
    {
        set { ddlTeam.TabIndex = value; }
    }
    /// <summary>
    /// set show chosen script
    /// </summary>
    public bool DisplayChosenScript
    {
        set { litJs.Visible = value; }
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
        sb.Append(String.Format("SetChosenTeam_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenTeam_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenTeam_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlTeam.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select a Team ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion

}