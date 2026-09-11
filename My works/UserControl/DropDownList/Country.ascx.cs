using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_DropDownList_Country : System.Web.UI.UserControl
{
    private int _ddWidth = 0;
    private short _tabIndex = 10;
    private bool _showDefault = false;
    private bool? _active = true;
    private int? _selectedId = null;
    private string _defaultValue = "";
    private string _defaultText = "";
    private string _cssClass = "";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ddlCountry.Items.Count == 0)
            PopulateDropDownList();
    }

    #region public methods

    /// <summary>
    /// bind Country drop down list
    /// </summary>
    public void PopulateDropDownList()
    {
        //clear items
        ddlCountry.ClearSelection();
        ddlCountry.Items.Clear();

        //tab index
        ddlCountry.TabIndex = _tabIndex;

        #region bind drop down list

        ddlCountry.DataSource = CRM.Maintenance.GetCountries();
        ddlCountry.DataTextField = "Name";
        ddlCountry.DataValueField = "Id";
        ddlCountry.DataBind();
        ddlCountry.DataSource = null;

        #endregion

        #region width / css class

        if (_ddWidth > 0)
            ddlCountry.Width = _ddWidth;
        if (!String.IsNullOrEmpty(_cssClass))
            ddlCountry.CssClass = _cssClass;

        #endregion

        #region selected item

        if (_selectedId.HasValue)
        {
            ListItem li = ddlCountry.Items.FindByValue(_selectedId.Value.ToString());
            if (li != null)
                li.Selected = true;
        }

        #endregion

        #region show default list item

        if (_showDefault)
        {
            ListItem liDefault = new ListItem(_defaultText, _defaultValue);
            ddlCountry.Items.Insert(0, liDefault);
            if (!_selectedId.HasValue)
                liDefault.Selected = true;
        }

        #endregion
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set selected Country ID
    /// </summary>
    public int? CountryId
    {
        get
        {
            int _id = 0;
            if (Int32.TryParse(ddlCountry.SelectedValue.Trim(), out _id))
                return _id;
            else
                return (int?)null;
        }
        set
        {
            _selectedId = value;
            if (value.HasValue)
            {
                ListItem _li = ddlCountry.Items.FindByValue(value.Value.ToString());
                if (_li != null)
                {
                    ddlCountry.ClearSelection();
                    _li.Selected = true;
                    return;
                }
            }
            PopulateDropDownList();
        }
    }

    /// <summary>
    /// get selected us Country name
    /// </summary>
    public string CountryName
    {
        get { return ddlCountry.SelectedItem.Text; }
    }

    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { _tabIndex = value; }
    }

    /// <summary>
    /// set default value for list item
    /// </summary>
    public string DefaultValue
    {
        set { _defaultValue = value; }
    }

    /// <summary>
    /// set default text for list item
    /// </summary>
    public string DefaultText
    {
        set { _defaultText = value; }
    }

    /// <summary>
    /// set display default list item
    /// </summary>
    public bool ShowDefaultEntry
    {
        set { _showDefault = value; }
    }

    /// <summary>
    /// set css class of drop down list
    /// </summary>
    public string CssClass
    {
        set { _cssClass = value; }
    }

    /// <summary>
    /// set show chosen script
    /// </summary>
    public bool DisplayChosenScript
    {
        set { phChosenScript.Visible = value;  }
    }

    /// <summary>
    /// set width of drop down list
    /// </summary>
    public int ControlWidth
    {
        set { _ddWidth = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { rfvCountry.ValidationGroup = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvCountry.Visible = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvCountry.ErrorMessage = value; }
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
        set { rfvCountry.Display = value; }
    }

    /// <summary>
    /// set validator fore color
    /// </summary>
    public System.Drawing.Color ValidatorForeColor
    {
        set { rfvCountry.ForeColor = value; }
    }

    /// <summary>
    /// set active status of collection
    /// </summary>
    public bool? Active
    {
        set { _active = value;  }
    }
    /// <summary>
    /// get drop down list control
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlCountry;
    }

    /// <summary>
    /// get bool of whether drop down list has any records
    /// </summary>
    public bool HasRecords
    {
        get
        {
            int _count = _showDefault ? 1 : 0;
            return ddlCountry.Items.Count > _count;
        }
    }

    /// <summary>
    /// set drop down list enabled
    /// </summary>
    public bool Enabled
    {
        set { ddlCountry.Enabled = value; }
    }

    #endregion
   
}