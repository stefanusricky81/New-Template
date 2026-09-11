using DesktopShared;
using DesktopShared.DaoClasses;
using DesktopShared.EntityClasses;
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
using Telerik.Web.UI;

public partial class Maintenance_CategoryBulkUpdate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucCreatedStart.SelectedDate = DateTime.Now.AddMonths(-1);
            ucCreatedEnd.SelectedDate = DateTime.Now;
        }
    }

    #region Protected method

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            BindGrid();
            rgTicket.DataBind();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("CategoryBulkUpdate.aspx");
    }

    protected void rgTicket_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgTicket_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            DataRowView _row = (DataRowView)e.Item.DataItem;
            GridDataItem _gdi = e.Item as GridDataItem;

            if(_row["Status"].ToString().Trim()=="77" || _row["Status"].ToString().Trim() == "59")
                DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell("Closed", _gdi["StatusName"]);
            if (_row["Status"].ToString().Trim() == "76" || _row["Status"].ToString().Trim() == "58")
                DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell("Open", _gdi["StatusName"]);

            //dropdown category inside grid
            DropDownList list = _gdi.FindControl("ddlCategoryUpdate") as DropDownList;
            list.DataSource = DesktopShared.TicketCategories.GetActive("Y");
            list.DataTextField = "CategoryName";
            list.DataValueField = "Id";
            list.DataBind();
            list.Items.Insert(0, new ListItem(String.Empty, String.Empty));

            list.SelectedValue = _row["CategoryID"].ToString().Trim();

            DropDownList listTicketType = _gdi.FindControl("ddlTicketTypeUpdate") as DropDownList;
            listTicketType.DataSource = DesktopShared.Ticket.TypeHelper.Get(true);
            listTicketType.DataTextField = "Name";
            listTicketType.DataValueField = "Id";
            listTicketType.DataBind();
            listTicketType.Items.Insert(0, new ListItem(String.Empty, String.Empty));

            listTicketType.SelectedValue = _row["TICKETTYPEID"].ToString().Trim();
        }
    }

    protected void cvCriteriaEntered_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;

        if (!String.IsNullOrWhiteSpace(txtTicketNumber.Text))
            return;
        if (!String.IsNullOrWhiteSpace(txtDescription.Text))
            return;
        if (ddlTicketDisposition.TicketDispositionId.HasValue)
            return;
        if (ddlClient.ClientId.HasValue)
            return;
        if (ddlTicketDesignationDDL.TicketDesignationId.HasValue)
            return;
        if (ddlEmployeeAssignedTo.EmployeeId > 0)
            return;
        if (ddlTicketType.TicketTypeId.HasValue)
            return;
        if (ucCreatedStart.SelectedDate.HasValue)
            return;
        if (ucCreatedEnd.SelectedDate.HasValue)
            return;
        if (ddlTicketCategory.TicketCategoryId.HasValue)
            return;
        args.IsValid = false;
    }

    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        int _usercode = DesktopShared.User.UserID;
        string str = e.CommandName.ToString().Trim();
        int _count = 0;
        int? categoryid = null;
        int? tickettypeid = null;
        switch (str)
        {
            case "Update":
                foreach (GridEditableItem item in rgTicket.Items)
                {
                    _count++;
                    int _ticketID = (int)item.GetDataKeyValue("TicketID");

                    DropDownList list = item.FindControl("ddlCategoryUpdate") as DropDownList;
                    DropDownList listTicketType = item.FindControl("ddlTicketTypeUpdate") as DropDownList;

                    CscDefectsEntity ticket = new CscDefectsEntity(_ticketID);

                    if (list.SelectedValue != string.Empty)
                        categoryid = Convert.ToInt16(list.SelectedValue);
                    if (listTicketType.SelectedValue != string.Empty)
                        tickettypeid = Convert.ToInt16(listTicketType.SelectedValue);

                    ticket.TicketCategoryId = list.SelectedValue.Trim() == string.Empty ? null : categoryid;
                    ticket.TicketTypeId = listTicketType.SelectedValue.Trim() == string.Empty ? null : tickettypeid;
                    ticket.Lastupdated = DateTime.Now;
                    ticket.Save();
                }
                DisplayMessage(String.Format("{0} Category has been {1} - {2}", _count, "Updated", DateTime.Now), Bootstrap.Alert.AlertType.Success);

                BindGrid();
                rgTicket.DataBind();

                break;
            case "Assign":
                foreach (GridDataItem _assign in rgTicket.MasterTableView.Items)
                {
                    _count++;
                    CheckBox chk = (CheckBox)_assign.FindControl("cboxSelect");
                    if (chk.Checked == true)
                    {
                        int _ticketID = (int)_assign.GetDataKeyValue("TicketID");

                        CscDefectsEntity ticket = new CscDefectsEntity(_ticketID);

                        ticket.TicketCategoryId = ddlTicketCategoryUpdateTop.TicketCategoryId;
                        ticket.Lastupdated = DateTime.Now;
                        ticket.Save();
                    }
                }


                DisplayMessage(String.Format("{0} Category has been {1} into - {2}", _count, "Updated", ddlTicketCategoryUpdateTop.TicketCategoryName, DateTime.Now), Bootstrap.Alert.AlertType.Success);

                BindGrid();
                rgTicket.DataBind();
                break;

            case "AssignType":
                foreach (GridDataItem _assign in rgTicket.MasterTableView.Items)
                {
                    
                    CheckBox chk = (CheckBox)_assign.FindControl("cboxSelect");
                    if (chk.Checked == true)
                    {
                        _count++;

                        int _ticketID = (int)_assign.GetDataKeyValue("TicketID");

                        CscDefectsEntity ticket = new CscDefectsEntity(_ticketID);

                        ticket.TicketTypeId = ddlTicketTypeTop.TicketTypeId;
                        ticket.Lastupdated = DateTime.Now;
                        ticket.Save();
                    }
                }


                DisplayMessage(String.Format("{0} Ticket Type has been {1} into - {2}", _count, "Updated", ddlTicketTypeTop.TicketTypeName, DateTime.Now), Bootstrap.Alert.AlertType.Success);

                BindGrid();
                rgTicket.DataBind();
                break;

            case "AssignBoth":
                foreach (GridDataItem _assign in rgTicket.MasterTableView.Items)
                {

                    CheckBox chk = (CheckBox)_assign.FindControl("cboxSelect");
                    if (chk.Checked == true)
                    {
                        _count++;

                        int _ticketID = (int)_assign.GetDataKeyValue("TicketID");

                        CscDefectsEntity ticket = new CscDefectsEntity(_ticketID);

                        ticket.TicketTypeId = ddlTicketTypeTop.TicketTypeId;
                        ticket.TicketCategoryId = ddlTicketCategoryUpdateTop.TicketCategoryId;
                        ticket.Lastupdated = DateTime.Now;
                        ticket.Save();
                    }
                }


                DisplayMessage(String.Format("{0} Category/Ticket Type has been {1} into - {2}", _count, "Updated", ddlTicketCategoryUpdateTop.TicketCategoryName + " " + ddlTicketTypeTop.TicketTypeName, DateTime.Now), Bootstrap.Alert.AlertType.Success);

                BindGrid();
                rgTicket.DataBind();
                break;
        }
    }
    #endregion

    #region Private method
    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    private void BindGrid()
    {
        phSearchResults.Visible = true;
        int id = 0;
        id = txtTicketNumber.Text == string.Empty ? 0 : Convert.ToInt16(txtTicketNumber.Text.Trim());

        //rgTicket.DataSource = Search(id, ddlTicketStatus.Status, txtDescription.Text,ddlTicketDisposition.TicketDispositionId
        //    , ddlClient.ClientId == null ? 0 : Convert.ToInt16(ddlClient.ClientId),
        //    ddlTicketDesignationDDL.TicketDesignationId, ddlEmployeeAssignedTo.EmployeeId, ucCreatedStart.SelectedDate, ucCreatedEnd.SelectedDate, ddlTicketCategory.TicketCategoryId);
        rgTicket.DataSource = GetData();
    }
    #region not use
    /*
    private DataTable Search(int ticketid, DesktopShared.Ticket.Status ticketstatus, string desc, int? dispotition, int client, int? designation, int? assignedto, DateTime? datestart, DateTime? dateend, int? category)
    {
        //dynamic list 
        DataTable _dynamicList = new DataTable();

        //dao
        TypedListDAO _dao = new TypedListDAO();

        ResultsetFields _fields = new ResultsetFields(10);
        int _fieldCount = -1;

        _fields.DefineField(CscDefectsFields.Pcscdefects, ++_fieldCount, "ID");
        _fields.DefineField(CscDefectsFields.Summary, ++_fieldCount, "Summary");
        _fields.DefineField(CscDefectsFields.Description, ++_fieldCount, "Description");
        _fields.DefineField(TicketCategoryFields.CategoryName, ++_fieldCount, "CategoryName");        
        _fields.DefineField(CscDefectsFields.FkStatus, ++_fieldCount, "Status");
        _fields.DefineField(TicketTypeFields.Name, ++_fieldCount, "TicketTypeName");
        _fields.DefineField(CscDefectsFields.DateEntered, ++_fieldCount, "DateEntered");
        _fields.DefineField(UsersFields.Last, ++_fieldCount, "AssignedToUser");
        _fields.DefineField(UsersFields.First, ++_fieldCount, "AssignedToUser2");
        _fields.DefineField(CscDefectsFields.TicketCategoryId, ++_fieldCount, "CategoryID");

        #region relations

        IRelationCollection _relations = new RelationCollection();
        IEntityRelation _relation;

        _relation = new EntityRelation(RelationType.OneToMany);
        _relation.AddEntityFieldPair(EntityFieldFactory.Create(CscDefectsFieldIndex.TicketCategoryId),
                                     EntityFieldFactory.Create(TicketCategoryFieldIndex.Id));
        _relations.Add(_relation, JoinHint.Right);

        _relation = new EntityRelation(RelationType.OneToMany);
        _relation.AddEntityFieldPair(EntityFieldFactory.Create(CscDefectsFieldIndex.TicketTypeId),
                                     EntityFieldFactory.Create(TicketTypeFieldIndex.Id));
        _relations.Add(_relation, JoinHint.Right);

        _relation = new EntityRelation(RelationType.OneToMany);
        _relation.AddEntityFieldPair(EntityFieldFactory.Create(UsersFieldIndex.Pusers),
                                    EntityFieldFactory.Create(CscDefectsFieldIndex.Assignedto));
        _relations.Add(_relation, JoinHint.Left);
        #endregion

        #region filter

        IPredicateExpression _filter = new PredicateExpression();

        if (ticketid != 0)
            _filter.AddWithAnd(CscDefectsFields.Pcscdefects == ticketid);

        //ticket status
        if (ticketstatus != DesktopShared.Ticket.Status.All)
        {
            int[] statusValues;

            if (ticketstatus == DesktopShared.Ticket.Status.Open)
                statusValues = new int[2] { 76, 58 };
            else
                statusValues = new int[2] { 77, 59 };

            _filter.AddWithAnd(CscDefectsFields.FkStatus == statusValues);
        }
        if (!String.IsNullOrEmpty(desc))
            _filter.AddWithAnd(CscDefectsFields.Description == desc);
        if (dispotition != null)
            _filter.AddWithAnd(CscDefectsFields.FkDisposition == dispotition);
        if (client > 0)
            _filter.AddWithOr(CscDefectsFields.FkClient == client);
        if (designation != null)
            _filter.AddWithAnd(TicketTypeFields.FkDesignationId == designation);
        if (assignedto != null)
            _filter.AddWithAnd(CscDefectsFields.Assignedto == assignedto);
        if (datestart != null)
            _filter.AddWithAnd(CscDefectsFields.Created >= datestart);
        if (dateend != null)
            _filter.AddWithAnd(CscDefectsFields.Created <= dateend);
        if (category != null)
            _filter.AddWithAnd(CscDefectsFields.TicketCategoryId == category);

        #endregion

        //sort expression
        ISortExpression _sorter = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        _sorter.Add(CscDefectsFields.DateEntered | SortOperator.Descending);

        //fetch
        _dao.GetMultiAsDataTable(_fields, _dynamicList, 0, _sorter, _filter, _relations, false, null, null, 0, 0);

        return _dynamicList;


        //DesktopShared.CollectionClasses.CscDefectsCollection _defect = new DesktopShared.CollectionClasses.CscDefectsCollection();

        //SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression _orFilter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression();
        //_orFilter.AddWithAnd(DesktopShared.HelperClasses.CscDefectsFields.FkStatus == 77);

        ////sort expression
        //SD.LLBLGen.Pro.ORMSupportClasses.ISortExpression _defectSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        //_defectSort.Add(DesktopShared.HelperClasses.CscDefectsFields.Pcscdefects | SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Descending);

        ////fetch
        //_defect.GetMulti(_orFilter, 4, _defectSort);

        ////return
        //return _defect;
    }
    */
    #endregion
    private DataTable GetData()
    {
        string connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString.SQL Server (SqlClient)"];
        System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString);
        string _status = string.Empty;
        string _desc = string.Empty; string _startdate = string.Empty; string _enddate = string.Empty;
        int? _ticketnumber = null; int? _disposition = null; int? _clientid = null; int? _designationid = null; int? _assignedto = null;
        int? _tickettypeid = null; int? _ticketcategoryid = null;
        DataTable dt = new DataTable();

        if (ddlTicketStatus.Status == DesktopShared.Ticket.Status.Open)
            _status = "76, 58";
        else if (ddlTicketStatus.Status == DesktopShared.Ticket.Status.Closed)
            _status = "77, 59";
        if (txtTicketNumber.Text.Trim() != string.Empty)
            _ticketnumber = Convert.ToInt32(txtTicketNumber.Text.Trim());
        if (txtDescription.Text.Trim() != string.Empty)
            _desc = txtDescription.Text.Trim();

        if (ddlTicketDisposition.TicketDispositionId.HasValue)
            _disposition = ddlTicketDisposition.TicketDispositionId;
        if (ddlClient.ClientId.HasValue)
            _clientid = ddlClient.ClientId;
        if (ddlTicketDesignationDDL.TicketDesignationId.HasValue)
            _designationid= ddlTicketDesignationDDL.TicketDesignationId;
        if (ddlEmployeeAssignedTo.EmployeeId > 0)
            _assignedto = ddlEmployeeAssignedTo.EmployeeId;
        if (ddlTicketType.TicketTypeId.HasValue)
            _tickettypeid = ddlTicketType.TicketTypeId;
        if (ucCreatedStart.SelectedDate.HasValue)
            _startdate = ucCreatedStart.SelectedDate.ToString().Trim();
        if (ucCreatedEnd.SelectedDate.HasValue)
            _enddate = ucCreatedEnd.SelectedDate.ToString().Trim();
        if (ddlTicketCategory.TicketCategoryId.HasValue)
            _ticketcategoryid = ddlTicketCategory.TicketCategoryId;

        try
        {
            using (connection)
            {
                connection.Open();
                System.Data.SqlClient.SqlDataAdapter adp = new System.Data.SqlClient.SqlDataAdapter("PROC_GETCATEGORYBULKUPDATE", connection);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;

                adp.SelectCommand.Parameters.AddWithValue("@STATUS", _status == string.Empty ? null : _status);
                adp.SelectCommand.Parameters.AddWithValue("@TICKETID", _ticketnumber);
                adp.SelectCommand.Parameters.AddWithValue("@DESC", _desc == string.Empty ? null : _desc);
                adp.SelectCommand.Parameters.AddWithValue("@DISPOSITION", _disposition);
                adp.SelectCommand.Parameters.AddWithValue("@CLIENTID", _clientid);
                adp.SelectCommand.Parameters.AddWithValue("@DESIGNATIONID", _designationid);
                adp.SelectCommand.Parameters.AddWithValue("@ASSIGNEDTO", _assignedto);
                adp.SelectCommand.Parameters.AddWithValue("@TICKETTYPE", _tickettypeid);
                adp.SelectCommand.Parameters.AddWithValue("@startdate", ucCreatedStart.SelectedDate);
                adp.SelectCommand.Parameters.AddWithValue("@enddate", ucCreatedEnd.SelectedDate);
                adp.SelectCommand.Parameters.AddWithValue("@TICKETCATEGORY", _ticketcategoryid);
                adp.SelectCommand.Parameters.AddWithValue("@Uncategories", cbUncategories.Checked == true ? 1 : 0);

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


    protected void cvCreatedDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!ucCreatedStart.SelectedDate.HasValue || !ucCreatedEnd.SelectedDate.HasValue)
            return;
        args.IsValid = ucCreatedEnd.SelectedDate.Value.Date >= ucCreatedStart.SelectedDate.Value.Date;
    }
}