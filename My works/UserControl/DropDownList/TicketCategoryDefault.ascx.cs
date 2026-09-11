using DesktopShared;
using DesktopShared.CollectionClasses;
using DesktopShared.DaoClasses;
using DesktopShared.FactoryClasses;
using DesktopShared.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_DropDownList_TicketCategoryDefault : System.Web.UI.UserControl
{
    private short _tabIndex = 10;
    private bool _displayDefaultValue = true;
    private bool? _active = null;
    private int? _clientid = null;
    private bool _setSize = true;
    private int? _selectedId = null;
    private string _defaultText = "";
    private string _defaultValue = "";
    private string _cssClass = "form-control select-chosen";

    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (ddlTicketCategoryDefault.Items.Count != 0)
            PopulateDropDownList(Client);
    }

    #region public methods

    /// <summary>
    /// populate drop down list
    /// </summary>
    public void PopulateDropDownList(int? cid)
    {
        Client = cid;
        #region bind drop down list
        ddlTicketCategoryDefault.DataSource = GetActive("Y", cid);
        ddlTicketCategoryDefault.DataTextField = "CategoryName";
        ddlTicketCategoryDefault.DataValueField = "Id";
        ddlTicketCategoryDefault.DataBind();

        #endregion

        #region width / css class

        if (!String.IsNullOrEmpty(_cssClass))
            ddlTicketCategoryDefault.CssClass = _cssClass;

        #endregion

        #region selected item

        if (_selectedId.HasValue)
        {
            ListItem li = ddlTicketCategoryDefault.Items.FindByValue(_selectedId.Value.ToString());
            if (li != null)
                li.Selected = true;
        }

        #endregion

        #region show default list item

        if (_displayDefaultValue)
        {
            ListItem liDefault = new ListItem(_defaultText, _defaultValue);
            ddlTicketCategoryDefault.Items.Insert(0, liDefault);
        }

        #endregion

        //fixed height
        if (_setSize)
            ddlTicketCategoryDefault.Height = Unit.Pixel(20);
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
    /// set width for drop down list
    /// </summary>
    public Unit Width
    {
        set { ddlTicketCategoryDefault.Width = value; }
    }

    /// <summary>
    /// get/set ticket type id
    /// </summary>
    public int? TicketCategoryId
    {
        get
        {
            int _id = 0;
            if (Int32.TryParse(ddlTicketCategoryDefault.SelectedValue.Trim(), out _id))
                return _id;
            else
                return (int?)null;
        }
        set
        {
            _selectedId = value;
            PopulateDropDownList(Client);
        }
    }

    /// <summary>
    /// get selected ticket type name
    /// </summary>
    public string TicketCategoryName
    {
        get { return ddlTicketCategoryDefault.SelectedItem.Text; }
    }

    /// <summary>
    /// set active value
    /// </summary>
    public bool? Active
    {
        set { _active = value; }
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
    /// set dipslay default list item
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
        set { _tabIndex = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { rfvTicketCategoryDefault.ValidationGroup = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvTicketCategoryDefault.ErrorMessage = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvTicketCategoryDefault.Visible = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set
        {
            ddlTicketCategoryDefault.CssClass = value;
            _cssClass = value;
        }
    }

    /// <summary>
    /// get drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlTicketCategoryDefault;
    }

    /// <summary>
    /// set show chosen script
    /// </summary>
    public bool DisplayChosenScript
    {
        set { litJs.Visible = value; }
    }

    public int? Client
    {
        get
        {
            try
            {
                if (this.ViewState["ClientID"] != null)
                    return null;
                else
                    return Convert.ToInt32(this.ViewState["ClientID"]);
            }
            catch
            {
                return null;
            }
        }
        set { this.ViewState["ClientID"] = value.ToString(); }
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
        sb.Append(String.Format("SetChosenTicketType_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenTicketType_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenTicketType_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlTicketCategoryDefault.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("search_contains: true,");
        sb.Append("placeholder_text_single: \"Select a Ticket Category ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    //private ClientTicketCategoryCollection GetActive(string flag, int? cid)
    //{
    //    ClientTicketCategoryCollection  _ticketcategory = new ClientTicketCategoryCollection();

    //    IPredicateExpression _orFilter = new PredicateExpression();

    //    if (!string.IsNullOrEmpty(flag))
    //        _orFilter.AddWithAnd(ClientTicketCategoryFields.Active == flag);

    //    _orFilter.AddWithAnd(ClientTicketCategoryFields.CategoryDefault == "Y");

    //    if(cid !=null)
    //        _orFilter.AddWithOr(ClientTicketCategoryFields.ClientId == cid);

    //    //sort expression
    //    ISortExpression _ticketcategorySort = new SortExpression();
    //    _ticketcategorySort.Add(TicketCategoryFields.CategoryName | SortOperator.Ascending);

    //    //fetch
    //    _ticketcategory.GetMulti(_orFilter, 0, _ticketcategorySort);

    //    //return
    //    return _ticketcategory;
    //}

    //private DataTable GetActive2(string flag, int? cid)
    //{
    //    //dynamic list 
    //    DataTable _dynamicList = new DataTable();

    //    //dao
    //    TypedListDAO _dao = new TypedListDAO();

    //    #region result set fields

    //    ResultsetFields _fields = new ResultsetFields(2);
    //    int _fieldCount = -1;

    //    _fields.DefineField(TicketCategoryFields.Id, ++_fieldCount, "ID");
    //    _fields.DefineField(TicketCategoryFields.CategoryName, ++_fieldCount, "CategoryName");

    //    #endregion

    //    #region relations
    //    IRelationCollection _relations = new RelationCollection();
    //    IEntityRelation _relation;

    //    _relation = new EntityRelation(RelationType.OneToMany);
    //    _relation.AddEntityFieldPair(EntityFieldFactory.Create(TicketCategoryFieldIndex.Id),
    //                                 EntityFieldFactory.Create(ClientTicketCategoryFieldIndex.TicketCategoryId));
    //    _relations.Add(_relation, JoinHint.Left);
    //    #endregion

    //    #region filter

    //    IPredicateExpression _filter = new PredicateExpression();

    //    if (!string.IsNullOrEmpty(flag))
    //        _filter.AddWithAnd(TicketCategoryFields.Active == flag);

    //    _filter.AddWithAnd(TicketCategoryFields.CategoryDefault == "Y");

    //    if (cid != null)
    //        _filter.AddWithOr(ClientTicketCategoryFields.ClientId == cid);

    //    #endregion

    //    ISortExpression _sorter = new SortExpression();
    //    _sorter.Add(TicketCategoryFields.CategoryName | SortOperator.Ascending);

    //    //fetch
    //    _dao.GetMultiAsDataTable(_fields, _dynamicList, 0, _sorter, _filter, _relations, false, null, null, 0, 0);

    //    return _dynamicList;
    //}
    private DataTable GetActive(string flag, int? cid)
    {
        string connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString.SQL Server (SqlClient)"];
        System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString);
        DataTable dt = new DataTable();

        try
        {
            using (connection)
            {
                connection.Open();
                System.Data.SqlClient.SqlDataAdapter adp = new System.Data.SqlClient.SqlDataAdapter("proc_GetDefaultTicketCategory", connection);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;

                adp.SelectCommand.Parameters.AddWithValue("@clientid", cid);

                dt.Reset();
                adp.Fill(dt);
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        finally { connection.Close(); }

        return dt;
    }
    #endregion
}