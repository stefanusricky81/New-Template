using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class UserControl_DropDownList_EmployeeTicket : System.Web.UI.UserControl
{
    private string _defaultText = "";
    private string _defaultValue = "";
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
    /// populdate drop down list
    /// </summary>
    public void Populate()
    {
        Populate(UserId);
    }

    /// <summary>
    /// populdate drop down list
    /// </summary>
    public void Populate(int userId)
    {
        if (userId > 0)
        {
            #region data table of open tickets for user

            //get data table of open tickets for user
            DataTable dtTicket = DesktopShared.Ticket.GetUserTickets(userId);

            //add new column for display
            dtTicket.Columns.Add("Display");

            //iterate through rows and combine data for display column
            foreach (DataRow dr in dtTicket.Rows)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                //client name -> max of 10 characters 
                string _clientName = dr["CompanyName"].ToString().Trim();
                if (_clientName.Length > 10)
                    _clientName = _clientName.Substring(0, 10);
                sb.Append(_clientName + ": ");

                //ticket id -> pad left 5
                sb.Append(dr["Id"].ToString().Trim().PadLeft(5, '0') + " - ");

                //ticket summary -> max of 57
                string _summary = dr["Summary"].ToString().Trim();
                if (_summary.Length > 57)
                    _summary = _summary.Substring(0, 54) + " ...";
                sb.Append(_summary);

                dr["Display"] = sb.ToString();
            }

            #endregion

            //default sort by display
            dtTicket.DefaultView.Sort = "Display ASC";

            #region bind drop down list

            ddlTicket.DataSource = dtTicket;
            ddlTicket.DataTextField = "Display";
            ddlTicket.DataValueField = "Id";
            ddlTicket.DataBind();

            #endregion

            #region find by selected id

            if (_selectedId > 0)
            {
                ListItem li = ddlTicket.Items.FindByValue(_selectedId.ToString());
                if (li != null)
                    li.Selected = true;
            }

            #endregion

            #region display default value

            if (_displayDefaultValue)
                ddlTicket.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

            #endregion

            _isLoaded = true;
        }

    }

    #endregion

    #region public properties

    /// <summary>
    /// set width of drop down list control
    /// </summary>
    public Unit Width
    {
        set { ddlTicket.Width = value; }
    }

    /// <summary>
    /// get drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlTicket;
    }

    /// <summary>
    /// get/set user id
    /// </summary>
    public int UserId
    {
        get
        {
            object obj = this.ViewState["UserIDForTickets"];
            if (obj == null)
                return DesktopShared.User.UserID;
            else
                return (int)obj;
        }
        set
        { 
            this.ViewState["UserIDForTickets"] = value;
            Populate(value);
        }
    }

    /// <summary>
    /// set/set ticket tid
    /// on set -> bind grid
    /// </summary>
    public int TicketId
    {
        get
        {
            try
            {
                if (ddlTicket.SelectedIndex > 0)
                    return Convert.ToInt32(ddlTicket.SelectedItem.Value);
                else
                    return -1;
            }
            catch { return -1; }
        }
        set { _selectedId = value; }
    }


    /// <summary>
    /// get selected ticket name
    /// </summary>
    public string TicketName
    {
        get { return ddlTicket.SelectedIndex >= 0 ? ddlTicket.SelectedItem.Text : ""; }
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
    /// set validation group for required field validator
    /// </summary>
    public string ValidationGroup
    {
        set { rfvTicket.ValidationGroup = value; }
    }

    /// <summary>
    /// set selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvTicket.Visible = value; }
    }

    /// <summary>
    /// set css class for drop down list control
    /// </summary>
    public string CssClass
    {
        set { ddlTicket.CssClass = value; }
    }

    #endregion
}