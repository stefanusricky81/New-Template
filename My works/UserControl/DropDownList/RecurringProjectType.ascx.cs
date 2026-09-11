using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class UserControl_DropDownList_RecurringProjectType : System.Web.UI.UserControl
{    
    private string _defaultText = "";
    private string _defaultValue = "-1";

    private bool _displayDefaultValue = true;
    private bool _isLoaded = false;

    private int _selectedId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack && !_isLoaded)
            Populate();
    }

    #region public methods

    /// <summary>
    /// populate drop down list
    /// </summary>
    public void Populate()
    {
        #region get collection of active recurring project types
        string connectionString = BitByBit.Configuration.GetConfigString("ConnectionString.SQL Server (SqlClient)");

        SqlConnection connection = new SqlConnection(connectionString);
        DataTable dt = new DataTable();

        connection.Open();
        SqlDataAdapter da = new SqlDataAdapter("Select Id, Name From RecurringProjectType Order By Name", connection);
        da.SelectCommand.CommandType = CommandType.Text;
       
        da.Fill(dt);
        connection.Close();
        #endregion

        #region bind drop down list

        ddlRecurringProjectType.DataSource = dt;
        ddlRecurringProjectType.DataTextField = "Name";
        ddlRecurringProjectType.DataValueField = "Id";
        ddlRecurringProjectType.DataBind();

        #endregion

        #region selected / default values

        //find by selected id
        if (_selectedId > 0)
        {
            ListItem li = ddlRecurringProjectType.Items.FindByValue(_selectedId.ToString());
            if (li != null)
                li.Selected = true;
        }

        //dispaly default value
        if (_displayDefaultValue)
            ddlRecurringProjectType.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

        
        ddlRecurringProjectType.Enabled = false;  // temp
        #endregion

        _isLoaded = true;

    }

    #endregion

    #region public properties

    /// <summary>
    /// set drop down list width
    /// </summary>
    public Unit Width
    {
        set { ddlRecurringProjectType.Width = value; }
    }

    /// <summary>
    /// get drop down list control
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlRecurringProjectType;
    }

    /// <summary>
    /// get/set recurring project type id
    /// </summary>
    public int? RecurringProjectTypeId
    {
        get
        {
            try
            {
                if (ddlRecurringProjectType.SelectedIndex > 0)
                    return Convert.ToInt32(ddlRecurringProjectType.SelectedValue);
                else
                    return null;
            }
            catch { return null; }

        }
        set
        {
            if (value.HasValue)
            {
                _selectedId = value.Value;
                Populate();
            }
            else
                ddlRecurringProjectType.ClearSelection();
        }
    }

    /// <summary>
    /// get selected Recurring Project Type name
    /// </summary>
    public string RecurringProjectTypeName
    {
        get { return ddlRecurringProjectType.SelectedItem.Text.Trim(); }
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
        set { rfvRecurringProjectType.ValidationGroup = value; }
    }

    /// <summary>
    /// set selection is requried
    /// </summary>
    public bool IsRequired
    {
        set { rfvRecurringProjectType.Visible = value; }
    }

    /// <summary>
    /// set error message for required field validator
    /// </summary>
    public string RequiredErrorMessage
    {
        set { rfvRecurringProjectType.ErrorMessage = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set { ddlRecurringProjectType.CssClass = value; }
    }

    #endregion
}