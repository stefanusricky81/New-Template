using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using BitByBit;
using SD.LLBLGen.Pro.ORMSupportClasses;
using DesktopShared;
using System.Text;
using System.Collections.Generic;

public partial class Maintenance_ClientServer : BasePage
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();
        if (!this.IsPostBack)
        {
            SetUpPage();
        }
    }

    #region private methods

    /// <summary>
    /// get boolean selection from drop down list
    /// </summary>
    /// <param name="ddl"></param>
    /// <returns></returns>
    private bool? GetBoolSelection(DropDownList ddl)
    {
        if (ddl.SelectedIndex == 0)
            return null;
        return ddl.SelectedIndex == 1;
    }

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        string _cssClass = "form-control select-chosen";
        bool _displayChosenScript = true;
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            _cssClass = "form-control";
            rgServer.RenderMode = RenderMode.Lightweight;
            phTabletPagerCss.Visible = true;
            _displayChosenScript = false;
        }

        ddlActive.CssClass = _cssClass;
        ddlLocal.CssClass = _cssClass;
        ddlOffsite.CssClass = _cssClass;
        ddlReplica.CssClass = _cssClass;
        ddlSingleBackup.CssClass = _cssClass;
        ddlBackupServer.CssClass = _cssClass;
        ddlNoBackup.CssClass = _cssClass;
        ddlBackupType.CssClass = _cssClass;
        ddlBackupType.DisplayChosenScript = _displayChosenScript;
        ddlOs.CssClass = _cssClass;
        ddlOs.DisplayChosenScript = _displayChosenScript;
        ddlOsVersion.CssClass = _cssClass;
        ddlOsVersion.DisplayChosenScript = _displayChosenScript;
        ddlType.CssClass = _cssClass;
        ddlImported.CssClass = _cssClass;
    }

    /// <summary>
    /// set up page
    /// </summary>
    private void SetUpPage()
    {
        txtName.Focus();

        #region tab index

        short _tabIndex = 0;
        ucClient.TabIndex = ++_tabIndex;
        txtName.TabIndex = ++_tabIndex;
        ddlActive.TabIndex = ++_tabIndex;
        ddlActive.TabIndex = ++_tabIndex;
        ddlLocal.TabIndex = ++_tabIndex;
        ddlOffsite.TabIndex = ++_tabIndex;
        ddlReplica.TabIndex = ++_tabIndex;
        ddlSingleBackup.TabIndex = ++_tabIndex;
        ddlBackupServer.TabIndex = ++_tabIndex;
        ddlNoBackup.TabIndex = ++_tabIndex;
        ddlBackupType.TabIndex = ++_tabIndex;
        ddlOs.TabIndex = ++_tabIndex;
        ddlOsVersion.TabIndex = ++_tabIndex;
        ddlType.TabIndex = ++_tabIndex;
        ddlImported.TabIndex = ++_tabIndex;
        btnSubmit.TabIndex = ++_tabIndex;
        btnClear.TabIndex = ++_tabIndex;

        #endregion

        //specified client server entity
        string _reqId = BitByBit.Web.Request.GetString("Id").Trim();
        if (!String.IsNullOrWhiteSpace(_reqId))
        {
            int _id = 0;
            if (int.TryParse(_reqId, out _id))
            {
                var objCLientServer = new DesktopShared.EntityClasses.ClientServerEntity(_id);
                if (objCLientServer.Fields.State == EntityState.Fetched)
                {
                    txtName.Text = objCLientServer.Name.Trim();
                    ucClient.ClientId = objCLientServer.ClientId;
                    ListItem _li = ddlBackupServer.Items.FindByValue("");
                    if (_li != null)
                    {
                        ddlBackupServer.ClearSelection();
                        _li.Selected = true;
                    }
                    _li = ddlNoBackup.Items.FindByValue("");
                    if (_li != null)
                    {
                        ddlNoBackup.ClearSelection();
                        _li.Selected = true;
                    }

                    RebindGrid();

                    if (rgServer.Items.Count == 1)
                    {
                        rgServer.Items[0].Edit = true;
                        rgServer.Rebind();
                    }
                }
            }
        }
        else if (BitByBit.Web.Request.GetString("clear") != "y")
            RebindGrid();


        ddlBackupType.TableType = "Client_Server_Backup_Type";
        ddlBackupType.PopulateDropDownList();
        ddlOs.TableType = "Client_Server_Os";
        ddlOs.PopulateDropDownList();
        ddlOsVersion.TableType = "Client_Server_OsVersion";
        ddlOsVersion.PopulateDropDownList();
    }

    /// <summary>
    /// rebind document grid
    /// </summary>
    private void RebindGrid()
    {
        phSearchResults.Visible = true;
        rgServer.Visible = true;
        rgServer.EditIndexes.Clear();
        rgServer.DataSource = null;
        rgServer.Rebind();
        litMessage.Text = "";
    }


    #endregion

    #region protected events

    /// <summary>
    /// search button click
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            ResetSearch = true;
            RebindGrid();
        }
    }

    /// <summary>
    /// clear button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("ClientServer.aspx?clear=y");
    }

    /// <summary>
    /// clear client on button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClearClient_Click(object sender, EventArgs e)
    {
        phSearchResults.Visible = false;
        ucClient.ClearItems();
        ucClient.ClientId = null;
    }

    #region telerik grid

    /// <summary>
    /// document grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgServer_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int _currentPage = ResetSearch ? 1 : rgServer.CurrentPageIndex + 1;
        int _resultCount = 0;
        int _resultsPerPage = rgServer.PageSize;

        int? _clientId = ucClient.ClientId;
        string _name = txtName.Text.Trim();
        bool? _active = GetBoolSelection(ddlActive);
        bool? _local = GetBoolSelection(ddlLocal);
        bool? _offsite = GetBoolSelection(ddlOffsite);
        bool? _replica = GetBoolSelection(ddlReplica);
        bool? _singleBackup = GetBoolSelection(ddlSingleBackup);
        bool? _backupServer = GetBoolSelection(ddlBackupServer);
        bool? _noBackup = GetBoolSelection(ddlNoBackup);
        bool? _noBackupApproved = null;
        int? _backupTypeId = ddlBackupType.SupportingTableId;
        int? _osId = ddlOs.SupportingTableId;
        int? _osVersionId = ddlOsVersion.SupportingTableId;
        bool? _isSever = GetBoolSelection(ddlType);
        bool? _imported = GetBoolSelection(ddlImported);

        var _dtCount = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchClientServersCount(_clientId, _name, _active, _local, _offsite, _replica, _singleBackup, _backupServer, 
            _noBackup, _noBackupApproved, _backupTypeId, _osId, _osVersionId, _isSever, _imported);
        if (_dtCount.Rows.Count > 0)
            _resultCount = Convert.ToInt32(_dtCount.Rows[0][0].ToString());

        var _dtServer = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchClientServers(_clientId, _name, _active, _local, _offsite, _replica, _singleBackup, _backupServer, 
            _noBackup, _noBackupApproved, _backupTypeId, _osId, _osVersionId, _isSever, _imported,
            _resultsPerPage, _currentPage, SortColumnName, SortOperator == SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending ? "asc" : "desc");

        rgServer.VirtualItemCount = _resultCount;
        rgServer.DataSource = _dtServer;
        ResetSearch = false;
    }

    /// <summary>
    /// document grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgServer_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = e.Item as GridDataItem;
            DataRowView _drv = (DataRowView)e.Item.DataItem;
            
            TelerikHelper.AddLabelToCell(_drv["Name"].ToString().Trim(), _gdi["Name"]);
            TelerikHelper.AddLabelToCell(_drv["ClientCompany"].ToString().Trim(), _gdi["ClientCompany"]);
            TelerikHelper.AddLabelToCell(_drv["Description"].ToString().Trim(), _gdi["Description"]);
            TelerikHelper.AddLabelToCell(BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(_drv["Active"], true) ? "Yes" : "No", _gdi["Active"]);
            TelerikHelper.AddLabelToCell(BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(_drv["Local"], true) ? "Yes" : "No", _gdi["Local"]);
            TelerikHelper.AddLabelToCell(BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(_drv["Offsite"], true) ? "Yes" : "No", _gdi["Offsite"]);
            TelerikHelper.AddLabelToCell(BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(_drv["Replica"], true) ? "Yes" : "No", _gdi["Replica"]);
            TelerikHelper.AddLabelToCell(BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(_drv["SingleBackup"], true) ? "Yes" : "No", _gdi["SingleBackup"]);
            TelerikHelper.AddLabelToCell(BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(_drv["BackupServer"], true) ? "Yes" : "No", _gdi["BackupServer"]);
            TelerikHelper.AddLabelToCell(_drv["NoBackupDisplay"].ToString().Trim(), _gdi["NoBackupDisplay"]);
            TelerikHelper.AddLabelToCell(_drv["BackupTypeName"].ToString().Trim(), _gdi["BackupTypeName"]);
            TelerikHelper.AddLabelToCell(_drv["OsName"].ToString().Trim(), _gdi["OsName"]);
            TelerikHelper.AddLabelToCell(_drv["OsVersionName"].ToString().Trim(), _gdi["OsVersionName"]);
            TelerikHelper.AddLabelToCell(BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(_drv["AutomatedImport"], true) ? "Yes" : "No", _gdi["AutomatedImport"]);
            TelerikHelper.AddLabelToCell(BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(_drv["IsServer"], true) ? "Server" : "Workstation", _gdi["IsServer"]);
            //
            //
            /*DateTime _created = DateTime.Now;
            string _createdDisplay = "";
            if (DateTime.TryParse(_drv["Created"].ToString().Trim(), out _created))
                _createdDisplay = _created.ToString("MM/dd/yy HH:mm");
            TelerikHelper.AddLabelToCell(_createdDisplay, _gdi["Created"]);*/

            LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
            btnDelete.Attributes.Add("onclick", "return confirm('Do you want to delete this server?');");
        }

        #endregion

        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            UserControl_Grid_EditForm_ClientServerEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientServerEdit;
            ucEditForm.DisplayClient = true;
            if (!e.Item.OwnerTableView.IsItemInserted) //edit/view
            {
                DataRowView _drv = (DataRowView)e.Item.DataItem;
                DesktopShared.EntityClasses.ClientServerEntity objServer = new DesktopShared.EntityClasses.ClientServerEntity(Convert.ToInt32(_drv["Id"].ToString()));
                ucEditForm.LoadValues(objServer);
            }
            else
                ucEditForm.LoadValues(null);
        }

        #endregion
    }

    /// <summary>
    /// document grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgServer_ItemCommand(object sender, GridCommandEventArgs e)
    {

        #region delete

        if (e.CommandName == "Delete")
        {
            try
            {
                int _id = int.Parse((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());

                var sentinelOneAgents = new DesktopShared.CollectionClasses.SentinelOneAgentCollection();
                var filter = new PredicateExpression();
                filter.Add(DesktopShared.HelperClasses.SentinelOneAgentFields.ClientServerId == _id);
                var sentinelOneAgentEntity = new DesktopShared.EntityClasses.SentinelOneAgentEntity();
                sentinelOneAgentEntity.ClientServerId = null;
                sentinelOneAgentEntity.ClientId = null;
                sentinelOneAgents.UpdateMulti(sentinelOneAgentEntity, filter);

                var objClientServer = new DesktopShared.EntityClasses.ClientServerEntity(_id);
                objClientServer.ArcticWolfEndpoint.DeleteMulti();
                objClientServer.SplashtopEndpoint.DeleteMulti();
                objClientServer.Delete();
                RebindGrid();
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Server has been deleted - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
            }
            catch (Exception ex)
            {
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, ex.Message.Trim(), DesktopShared.Bootstrap.Alert.AlertType.Danger);
            }
        }

        #endregion

    }

    /// <summary>
    /// grid on update command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgServer_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = (GridEditableItem)e.Item;
        int id = int.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["Id"].ToString());
        UserControl_Grid_EditForm_ClientServerEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientServerEdit;
        ucEditForm.SaveValues(id);
        RebindGrid();
        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Server has been updated - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
    }

    /// <summary>
    /// grid on insert command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgServer_InsertCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = (GridEditableItem)e.Item;
        UserControl_Grid_EditForm_ClientServerEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientServerEdit;
        ucEditForm.SaveValues(null);
        RebindGrid();
        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Server has been added - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgServer_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;
            
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(175);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(200);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(250);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgServer.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
        }
    }

    /// <summary>
    /// grid on sort command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgServer_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        GridSortExpression sortExpression = new GridSortExpression();

        //these columns will sort descending on first click
        var _nonAscList = new List<string> { "Id" };
        bool _sortAscending = !_nonAscList.Contains(e.SortExpression.Trim());

        SortColumnName = e.SortExpression.Trim();
        switch (e.OldSortOrder)
        {
            case GridSortOrder.None:
                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.Ascending : GridSortOrder.Descending;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
                break;
            case GridSortOrder.Ascending:

                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.Descending : GridSortOrder.None;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
                break;
            case GridSortOrder.Descending:

                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.None : GridSortOrder.Ascending;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
                break;
        }
        SortOperator = (sortExpression.SortOrder == GridSortOrder.Ascending) ? SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending : SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Descending;

        e.Canceled = true;
        rgServer.CurrentPageIndex = 0;
        rgServer.Rebind();

        if (!e.Item.OwnerTableView.SortExpressions.ContainsExpression(e.SortExpression))
        {
            sortExpression = new GridSortExpression();
            sortExpression.FieldName = e.SortExpression;
            sortExpression.SortOrder = GridSortOrder.Ascending;
            e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
        }
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set sort column name
    /// </summary>
    private string SortColumnName
    {
        get
        {
            object obj = this.ViewState["scn_cs"];
            return (obj == null) ? "Name" : (string)obj;
        }
        set { this.ViewState["scn_cs"] = value; }
    }

    /// <summary>
    /// get/set sort operator
    /// </summary>
    private SD.LLBLGen.Pro.ORMSupportClasses.SortOperator SortOperator
    {
        get
        {
            object obj = this.ViewState["so_cs"];
            return (obj == null) ? SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending : (SD.LLBLGen.Pro.ORMSupportClasses.SortOperator)obj;
        }
        set { this.ViewState["so_cs"] = value; }
    }


    /// <summary>
    /// get/set reset search
    /// </summary>
    public bool ResetSearch
    {
        get
        {
            object obj = this.ViewState["rs_cs"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["rs_cs"] = value; }
    }

    #endregion   
}
