using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_DropDownList_Tier : System.Web.UI.UserControl
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
        //get collection of active tiers
        DesktopShared.CollectionClasses.TierCollection _tiers = DesktopShared.Tier.GetActiveTiers();

        #region bind drop down list

        ddlTier.DataSource = _tiers;
        ddlTier.DataTextField = "Name";
        ddlTier.DataValueField = "Id";
        ddlTier.DataBind();

        #endregion

        #region selected / default values

        //find by selected id
        if (_selectedId > 0)
        {
            ListItem li = ddlTier.Items.FindByValue(_selectedId.ToString());
            if (li != null)
                li.Selected = true;
        }

        //dispaly default value
        if (_displayDefaultValue)
            ddlTier.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

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
        set { ddlTier.Width = value; }
    }

    /// <summary>
    /// get drop down list control
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlTier;
    }

    /// <summary>
    /// get/set tier id
    /// </summary>
    public int? TierId
    {
        get
        {
            try
            {
                if (ddlTier.SelectedIndex > 0)
                    return Convert.ToInt32(ddlTier.SelectedValue);
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
                ddlTier.ClearSelection();
        }
    }

    /// <summary>
    /// get selected tier name
    /// </summary>
    public string TierName
    {
        get { return ddlTier.SelectedItem.Text.Trim(); }
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
        set { rfvTier.ValidationGroup = value; }
    }

    /// <summary>
    /// set selection is requried
    /// </summary>
    public bool IsRequired
    {
        set { rfvTier.Visible = value; }
    }

    /// <summary>
    /// set error message for required field validator
    /// </summary>
    public string RequiredErrorMessage
    {
        set { rfvTier.ErrorMessage = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set { ddlTier.CssClass = value; }
    }

    #endregion
}