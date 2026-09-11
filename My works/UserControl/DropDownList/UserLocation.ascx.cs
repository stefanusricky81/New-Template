using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_DropDownList_UserLocation : System.Web.UI.UserControl
{
    private string _defaultText = "";
    private string _defaultValue = "-1";

    private bool _displayDefaultValue = true;
    private bool _isLoaded = false;

    private int? _selectedId = null;

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
        #region bind drop down list

        ddlUserLocation.DataSource = DesktopShared.User.Location.GetActive();
        ddlUserLocation.DataTextField = "Name";
        ddlUserLocation.DataValueField = "Id";
        ddlUserLocation.DataBind();

        #endregion

        #region selected / default values

        //find by selected id
        if (_selectedId.HasValue)
        {
            ListItem li = ddlUserLocation.Items.FindByValue(_selectedId.Value.ToString());
            if (li != null)
                li.Selected = true;
        }

        //dispaly default value
        if (_displayDefaultValue)
            ddlUserLocation.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

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
        set { ddlUserLocation.Width = value; }
    }

    /// <summary>
    /// get drop down list control
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlUserLocation;
    }

    /// <summary>
    /// get/set selected user location id
    /// </summary>
    public int? UserLocationId
    {
        get
        {
            if (ddlUserLocation.SelectedIndex > 0)
            {
                int _id = 0;
                Int32.TryParse(ddlUserLocation.SelectedValue, out _id);
                return _id > 0 ? _id : (int?)null;
            }
            else
                return null;
        }
        set
        {
            if (value.HasValue)
            {
                _selectedId = value;
                Populate();
            }
            else
                ddlUserLocation.ClearSelection();
        }
    }

    /// <summary>
    /// get selected user location name
    /// </summary>
    public string UserLocationName
    {
        get { return ddlUserLocation.SelectedItem.Text.Trim(); }
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
        set { rfvUserLocation.ValidationGroup = value; }
    }

    /// <summary>
    /// set selection is requried
    /// </summary>
    public bool IsRequired
    {
        set { rfvUserLocation.Visible = value; }
    }

    /// <summary>
    /// set error message for required field validator
    /// </summary>
    public string RequiredErrorMessage
    {
        set { rfvUserLocation.ErrorMessage = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set { ddlUserLocation.CssClass = value; }
    }

    #endregion

}