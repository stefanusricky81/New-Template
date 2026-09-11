using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;

public partial class UserControl_DropDownList_ProjectTask : System.Web.UI.UserControl
{
    private string _defaultText = "All";
    private string _defaultValue = "";
    private bool _displayDefaultValue = true;
    private int _selectedClientId = 0;
    private int _selectedProjectTaskId = 0;
    private bool _showDeleted = false;
    private string _cssClass = "";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (ddlProjectTask.Items.Count == 0)
            Populate();
    }

    #region public methods

    /// <summary>
    /// populdate drop down list
    /// </summary>
    public void Populate()
    {
        Populate(EmployeeId);
    }

    /// <summary>
    /// populate drop down list
    /// </summary>
    public void Populate(int employeeId)
    {
        //clear items
        ddlProjectTask.ClearSelection();
        ddlProjectTask.Items.Clear();

        DataTable dtProjectTask = DesktopShared.Employee.GetEmployeeProjects(employeeId, _selectedClientId, ClientMaxLength);
        dtProjectTask.DefaultView.Sort = "DisplayOrder ASC, Company ASC, Descr1 ASC";

        ddlProjectTask.DataSource = dtProjectTask;
        ddlProjectTask.DataTextField = "Display";
        ddlProjectTask.DataValueField = "DropDownValue";
        ddlProjectTask.DataBind();

        if (!String.IsNullOrEmpty(_cssClass))
            ddlProjectTask.CssClass = _cssClass;
        if (!_cssClass.Contains("form-control"))
        {
            //ddlProjectTask.Width = Unit.Pixel(100);
            ddlProjectTask.Height = Unit.Pixel(20);
        }

        if (_displayDefaultValue)
            ddlProjectTask.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

        if (_selectedProjectTaskId > 0)
        {
            //select based on middle value
            //2479|3761|3899 (ListItem value)
            //3761 (selected value)

            foreach (ListItem li in ddlProjectTask.Items)
            {
                int nPos = li.Value.IndexOf("|");
                int lastNpos = li.Value.LastIndexOf("|");
                lastNpos--;

                if (nPos > 0 && lastNpos > 0)
                {
                    int liValue = Convert.ToInt32(li.Value.Substring(nPos + 1, lastNpos - nPos).Trim());
                    if (liValue == _selectedProjectTaskId)
                    {
                        li.Selected = true;
                        break;
                    }
                }
            }
        }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set max length for client display
    /// </summary>
    public int? ClientMaxLength
    {
        get
        {
            object obj = this.ViewState["cml"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cml"] = value; }
    }

    /// <summary>
    /// get/set employee id
    /// </summary>
    public int EmployeeId
    {
        get
        {
            object obj = this.ViewState["EmployeeIdForProjectTask"];
            if (obj == null)
                return DesktopShared.User.EmployeeID;
            else
                return (int)obj;
        }
        set
        {
            this.ViewState["EmployeeIdForProjectTask"] = value;
            Populate(value);
        }
    }

    /// <summary>
    /// set drop down list width
    /// </summary>
    public Unit Width
    {
        set { ddlProjectTask.Width = value; }
    }

    /// <summary>
    /// get drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlProjectTask;
    }

    /// <summary>
    /// get/set selected client id
    /// </summary>
    public int SelectedClientId
    {
        get
        { 
            if (!String.IsNullOrEmpty(ddlProjectTask.SelectedValue))
            {
                string _selectedValue = ddlProjectTask.SelectedValue.Trim();
                int nPos = _selectedValue.IndexOf("|");

                if (nPos > 0)
                    return Convert.ToInt32(_selectedValue.Substring(0, nPos));
                else
                    return -1;
            }
            else
                return -1;
        }
        set
        {
            _selectedClientId = value;
            Populate();
        }
    }

    /// <summary>
    /// get/set selected project task id
    /// </summary>
    public int SelectedProjtaskId
    {
        get
        {
            if (!String.IsNullOrEmpty(ddlProjectTask.SelectedValue))
            {
                string _selectedValue = ddlProjectTask.SelectedValue.Trim();
                int nPos = _selectedValue.IndexOf("|");
                int lastNpos = _selectedValue.LastIndexOf("|");
                lastNpos--;

                if (nPos > 0 && lastNpos>0)
                    return Convert.ToInt32(_selectedValue.Substring(nPos+1,lastNpos-nPos));
                else
                    return -1;
            }
            else
                return -1;
        }
        set
        {
            //_selectedId = value;
            _selectedProjectTaskId = value;
            Populate();
        }
    }

    /// <summary>
    /// get/set selectd project id
    /// </summary>
    public int SelectedProjectId
    {
        get
        {
            if (!String.IsNullOrEmpty(ddlProjectTask.SelectedValue))
            {
                string _selectedValue = ddlProjectTask.SelectedValue.Trim();
                int nPos = _selectedValue.LastIndexOf("|");

                if (nPos > 0)
                    return Convert.ToInt32(_selectedValue.Substring(nPos + 1));
                else
                    return -1;
            }
            else
                return -1;
        }
        set
        {
            //_selectedId = value;
            Populate();
        }
    }

    /// <summary>
    /// set text for default drop down list item
    /// </summary>
    public string DefaultText
    {
        set { _defaultText = value; }
    }

    /// <summary>
    /// set value for default drop down list item
    /// </summary>
    public string DefaultValue
    {
        set { _defaultValue = value; }
    }

    /// <summary>
    /// set display default value
    /// </summary>
    public bool DisplayDefaultValue
    {
        set { _displayDefaultValue = value; }
    }

    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { ddlProjectTask.TabIndex = value; }
    }

    /// <summary>
    /// set show deleted
    /// </summary>
    public bool ShowDeleted
    {
        set { _showDeleted = value; }
    }

    /// <summary>
    /// set css class of drop down list
    /// </summary>
    public string CssClass
    {
        set { _cssClass = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            rfvProjectTask.Visible = true;
            rfvProjectTask.ValidationGroup = value;
        }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvProjectTask.Visible = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvProjectTask.ErrorMessage = value; }
    }

    /// <summary>
    /// set display asterisk for error
    /// </summary>
    public bool DisplayErrorAsterisk
    {
        set { litError.Visible = value; }
    }

    /// <summary>
    /// set validator display type
    /// </summary>
    public ValidatorDisplay validatorDisplay
    {
        set { rfvProjectTask.Display = value; }
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
        sb.Append(String.Format("SetChosenProjectTask_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenProjectTask_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenProjectTask_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlProjectTask.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select a Project ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}
