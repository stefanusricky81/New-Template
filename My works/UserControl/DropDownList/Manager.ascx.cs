using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class UserControl_DropDownList_Manager : System.Web.UI.UserControl
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
        //get data table of managers
        DataTable dtResult = DesktopShared.User.GetBBBManagers();

        #region bind drop down list

        ddlManager.DataSource = dtResult;
        ddlManager.DataTextField = "FullName";
        ddlManager.DataValueField = "Pusers";
        ddlManager.DataBind();

        #endregion

        #region selected / default values

        //find by selected id
        if (_selectedId > 0)
        {
            ListItem li = ddlManager.Items.FindByValue(_selectedId.ToString());
            if (li != null)
                li.Selected = true;
        }

        //dispaly default value
        if (_displayDefaultValue)
            ddlManager.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

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
        set { ddlManager.Width = value; }
    }

    /// <summary>
    /// get drop down list control
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlManager;
    }

    /// <summary>
    /// get/set Manager id
    /// </summary>
    public int? ManagerId
    {
        get
        {
            try
            {
                if (ddlManager.SelectedIndex > 0)
                    return Convert.ToInt32(ddlManager.SelectedValue);
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
                ddlManager.ClearSelection();
        }
    }

    /// <summary>
    /// get selected manager name
    /// </summary>
    public string ManagerName
    {
        get { return ddlManager.SelectedItem.Text.Trim(); }
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
        set{ rfvManager.ValidationGroup = value; }
    }

    /// <summary>
    /// set selection is requried
    /// </summary>
    public bool IsRequired
    {
        set { rfvManager.Visible = value; }
    }

    /// <summary>
    /// set error message for required field validator
    /// </summary>
    public string RequiredErrorMessage
    {
        set { rfvManager.ErrorMessage = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set { ddlManager.CssClass = value; }
    }

    #endregion
}