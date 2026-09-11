using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;

public partial class UserControl_DropDownList_Employee : System.Web.UI.UserControl
{
    private string _defaultText = "";
    private string _defaultValue = "-1";
    private bool _useEmployeeIdAsDataValue = false;
    private bool _useEmailAsDataValue = false;
    private bool _displayDefaultValue = true;
    private bool _setSize = true;
    private bool _hideQueueEmployee = false;
    private int _clientId = 0;
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
        if (ddlEmployee.Items.Count == 0)
            Populate();
    }

    #region public methods

    /// <summary>
    /// populate drop down list
    /// </summary>
    public void Populate()
    {
        DataTable dtEmployee = null;
        if (EmployeeTypeToDisplay == EmployeeType.Sales)
            dtEmployee = DesktopShared.Employee.GetActiveSalesEmployees();
        else if (EmployeeTypeToDisplay == EmployeeType.ActivityTracking)
            dtEmployee = DesktopShared.Employee.GetActiveEmployeesForActivityTracking();
        else if (EmployeeTypeToDisplay == EmployeeType.ActivityRole)
            dtEmployee = DesktopShared.Employee.GetDataTable("", "", true, null, null, null, "", "ActivityRole");
        else if (EmployeeTypeToDisplay == EmployeeType.ProjectManager)
            dtEmployee = DesktopShared.Employee.GetProjectManagersOrTechLeads("P");
        else if (EmployeeTypeToDisplay == EmployeeType.TechLead)
            dtEmployee = DesktopShared.Employee.GetProjectManagersOrTechLeads("T");
        else if (EmployeeTypeToDisplay == EmployeeType.AssignTo)
            dtEmployee = DesktopShared.Employee.GetAssignToActiveEmployees();
        else
            dtEmployee = DesktopShared.Employee.GetActiveEmployees();
        
        #region help desk client users

        if (_clientId > 0)
        {
            DataTable dtHelpdeskClientUser = DesktopShared.Client.GetHelpdeskContacts(_clientId);
            foreach (DataRow _userRow in dtHelpdeskClientUser.Rows)
            {
                DataRow _employeeRow = dtEmployee.NewRow();

                _employeeRow[0] = _userRow[0];
                _employeeRow[1] = _userRow[1];
                _employeeRow[2] = _userRow[2];
                _employeeRow[3] = _userRow[3];
                
                //fullname = "Employee Name - User Name"
                _employeeRow[4] = "[*" + _userRow[11].ToString().Trim() + "*] - " + _userRow[4].ToString().Trim();

                _employeeRow[5] = _userRow[5];
                _employeeRow[6] = _userRow[6];
                _employeeRow[7] = _userRow[7];
                _employeeRow[8] = _userRow[8];
                _employeeRow[9] = _userRow[9];
                _employeeRow[10] = _userRow[10];
                _employeeRow[11] = _userRow[11];

                dtEmployee.Rows.Add(_employeeRow);
            }
        }

        #endregion

        if (_hideQueueEmployee)
        {
            for (int i = dtEmployee.Rows.Count - 1; i >= 0; i--)
            {
                if (dtEmployee.Rows[i]["LastName"].ToString().Trim().ToLower().Contains("queue"))
                    dtEmployee.Rows[i].Delete();
            }
            dtEmployee.AcceptChanges();
        }

        if (EmployeeTypeToDisplay != EmployeeType.ProjectManager && EmployeeTypeToDisplay != EmployeeType.TechLead)
        {
            dtEmployee.Columns.Add(
                new DataColumn("PusersAndClientID", System.Type.GetType("System.String"), "Pusers + '|' + FkClient"));
            dtEmployee.DefaultView.Sort = "UseHelpDesk ASC, FirstName ASC";
        }

        ddlEmployee.DataSource = dtEmployee;
        ddlEmployee.DataTextField = "FullName";
        if (_useEmployeeIdAsDataValue)
            ddlEmployee.DataValueField = "EmployeeId";
        else if (_useEmailAsDataValue)
            ddlEmployee.DataValueField = "Email";
        else
            ddlEmployee.DataValueField = "PusersAndClientID";

        ddlEmployee.DataBind();

        if (_selectedId > 0)
        {
            bool delimiterCheck = true;

            #region employee id as value field
            
            //value = [pemployee]

            ListItem liEmployee = ddlEmployee.Items.FindByValue(_selectedId.ToString());
            if (liEmployee != null)
            {
                liEmployee.Selected = true;
                delimiterCheck = false;
            }
            

            #endregion

            #region pusers as value field

            //value = [pusers]|[fkclient]
            if (delimiterCheck)
            {
                foreach (ListItem li in ddlEmployee.Items)
                {
                    int nPos = li.Value.IndexOf("|");
                    if (nPos > 0)
                    {
                        int liValue = Convert.ToInt32(li.Value.Substring(0, nPos));
                        if (liValue == _selectedId)
                        {
                            li.Selected = true;
                            break;
                        }
                    }
                }
            }

            #endregion
        }

        if (_displayDefaultValue)
            ddlEmployee.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

        if (_setSize)
        {
            //ddlEmployee.Width = Unit.Pixel(100);
            ddlEmployee.Height = Unit.Pixel(20);
        }

        if (!String.IsNullOrEmpty(_cssClass))
            ddlEmployee.CssClass = _cssClass;
    }

    #endregion

    #region public properties

    /// <summary>
    /// set hide queue employees
    /// </summary>
    public bool HideQueueEmployees
    {
        set { _hideQueueEmployee = value; }
    }

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
        set { ddlEmployee.Width = value; }
    }

    /// <summary>
    /// get drop down list control
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlEmployee;
    }

    /// <summary>
    /// get/set client id - this is used for help desk clients
    /// help desk client users are added to drop down list 
    /// </summary>
    public int HelpDeskClientId
    {
        get
        {
            if (!String.IsNullOrEmpty(ddlEmployee.SelectedValue))
            {
                string _selectedValue = ddlEmployee.SelectedValue.Trim();
                int nPos = _selectedValue.IndexOf("|");
                
                if (nPos > 0)
                    return Convert.ToInt32(_selectedValue.Substring(nPos+1));
                else
                    return -1;
            }
            else
                return -1;
        }
        set { _clientId = value; }
    }

    /// <summary>
    /// get/set employee id
    /// </summary>
    public int EmployeeId
    {
        get
        {
            if (!String.IsNullOrEmpty(ddlEmployee.SelectedValue))
            {
                string _selectedValue = ddlEmployee.SelectedValue.Trim();
                int nPos = _selectedValue.IndexOf("|");

                if (nPos > 0)
                    return Convert.ToInt32(_selectedValue.Substring(0, nPos));
                else
                {
                    try { return Convert.ToInt32(ddlEmployee.SelectedValue); }
                    catch { return -1; }

                }
            }
            else
                return -1;
        }
        set
        {
            _selectedId = value;
            Populate();
        }
    }

    /// <summary>
    /// get selected value
    /// </summary>
    public string SelectedValue
    {
        get { return ddlEmployee.SelectedValue;  }
    }

    /// <summary>
    /// get selected employee name
    /// </summary>
    public string EmployeeName
    {
        get { return ddlEmployee.SelectedItem.Text.Trim(); }
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
    /// set use employee id as data value
    /// </summary>
    public bool UseEmployeeIdAsDataValue
    {
        set { _useEmployeeIdAsDataValue = value; }
    }

    /// <summary>
    /// set use employee email as data value
    /// </summary>
    public bool UseEmailAsDataValue
    {
        set { _useEmailAsDataValue = value; }
    }

    /// <summary>
    /// set validation group (this makes a selection required)
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            rfvEmployee.Visible = true;
            rfvEmployee.ValidationGroup = value;
        }
    }

    public bool Enabled
    {
        set 
        {
            ddlEmployee.Enabled = value;
        }
    }

    /// <summary>
    /// set error message for required field validator
    /// </summary>
    public string RequiredErrorMessage
    {
        set { rfvEmployee.ErrorMessage = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set
        {
            ddlEmployee.CssClass = value;
            _cssClass = value;
        }
    }

    /// <summary>
    /// get/set employee type to display
    /// </summary>
    public EmployeeType EmployeeTypeToDisplay
    {
        get
        {
            try 
            {
                if (this.ViewState["EmployeeTypeToDisplay"] == null)
                    return EmployeeType.All;
                return (EmployeeType)Enum.Parse(typeof(EmployeeType), this.ViewState["EmployeeTypeToDisplay"].ToString(), true); 
            
            }
            catch
            {
                return EmployeeType.All; }
        }
        set { this.ViewState["EmployeeTypeToDisplay"] = value.ToString(); }
    }

    /// <summary>
    /// set display of required field validator
    /// </summary>
    public ValidatorDisplay RequiredFieldDisplay
    {
        set { rfvEmployee.Display = value; } 
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
        set { ddlEmployee.TabIndex = value;  }
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
        sb.Append(String.Format("SetChosenEmployee_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenEmployee_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenEmployee_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlEmployee.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select an Employee ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion

    #region public enum

    /// <summary>
    /// enum for employee type
    /// </summary>
    public enum EmployeeType
    {
        ActivityTracking,
        All,
        Sales,
        TechLead,
        ProjectManager,
        ActivityRole,
        AssignTo
    }

    #endregion
}


