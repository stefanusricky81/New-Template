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
using SD.LLBLGen.Pro.ORMSupportClasses;
using Telerik.Web.UI;

public partial class UserControl_Grid_ClientBackupSet : System.Web.UI.UserControl
{
    private string _highlight1 = "#FFC1C1";
    private string _highlight2 = "#FEE5AC";

    protected void Page_Load(object sender, EventArgs e)
    {
        //records per page event handler
        DropDownList ddlRecordsPerPage = ucRecordsPerPage.GetDropDownList();
        ddlRecordsPerPage.SelectedIndexChanged += new EventHandler(ddlRecordsPerPage_SelectedChange);

        if (!this.IsPostBack)
        {
            SetShowHide(); //hide/show grid click todo: needed? 
            SetUpRecordsPerPage(); //set value for drop down list control for number of records to display
        }
    }

    #region protected methods / events

    #region telerik grid events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgClientBackupSet_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DesktopShared.TypedListClasses.BackupSet_TypedList backups = DesktopShared.Backup.GetTypedList(SearchClientId, null, SearchBackupTypeId, SearchActive, SearchServerName, SearchBackupServerName, SearchCreatedLastDays);
        backups.DefaultView.Sort = "Company, Name";
        rgClientBackupSet.DataSource = backups;

        #region paging

        if (TicketsPerPage > 0)
            rgClientBackupSet.PageSize = TicketsPerPage;
        else
        {
            if (backups.Rows.Count > 0)
                rgClientBackupSet.PageSize = backups.Rows.Count;
        }

        #endregion
    }

    /// <summary>
    /// grid on item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupSet_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            rgClientBackupSet.MasterTableView.NoMasterRecordsText = "";                        
            AbstractBackupSetEdit ucBackupSetEdit = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as AbstractBackupSetEdit;

            if (!e.Item.OwnerTableView.IsItemInserted)//edit existing
            {
                DesktopShared.TypedListClasses.BackupSet_Row _backupRow = (DesktopShared.TypedListClasses.BackupSet_Row)((DataRowView)e.Item.DataItem).Row;
                ucBackupSetEdit.LoadValues(_backupRow);
            }
            else //add new item
            {
                if (SearchClientId.HasValue) //default client id
                    ucBackupSetEdit.SelectedClientId = SearchClientId.Value;

                ucBackupSetEdit.LoadValues(0);
            }
        }

        #endregion

        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DesktopShared.TypedListClasses.BackupSet_Row _backup =(DesktopShared.TypedListClasses.BackupSet_Row)((DataRowView)e.Item.DataItem).Row;

            var backupType = _backup.SupportingTableDescription.Trim();
            if (!_backup.IsDattoBackupTypeIdNull())
                backupType += String.Format(" - {0}", (_backup.DattoBackupTypeId == DesktopShared.Backup.Datto.Job.Type.Id.Replication) ? "Replication" : "Local");

            TelerikHelper.AddLabelToCell(_backup.Company.Trim().Trim(), item["Company"]);
            TelerikHelper.AddLabelToCell(_backup.Server.Trim().Trim(), item["Server"]);
            TelerikHelper.AddLabelToCell(backupType, item["SupportingTableDescription"]);
            TelerikHelper.AddLabelToCell(_backup.Name.Trim().Trim(), item["Name"]);
            TelerikHelper.AddLabelToCell(_backup.Description.Trim().Trim(), item["Description"]);
            TelerikHelper.AddLabelToCell(_backup.Created.ToString("MM/dd/yy HH:mm"), item["Created"]);
            TelerikHelper.AddLabelToCell(_backup.Active.ToString().Trim(), item["Active"]);
        }

        #endregion

    }

    /// <summary>
    /// grid on update command -> edit entity (use EditForm/BackupSetEdit.ascx)
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgClientBackupSet_UpdateCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            int Id = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["Id"]);
            AbstractBackupSetEdit ucBackupSetEdit = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as AbstractBackupSetEdit;
            ucBackupSetEdit.SaveValues(Id);            
        }
    }

    /// <summary>
    /// grid on insert command -> add entity (use EditForm/BackupSetEdit.ascx)
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgClientBackupSet_InsertCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
            AbstractBackupSetEdit ucBackupSetEdit = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as AbstractBackupSetEdit;
            ucBackupSetEdit.SaveValues(0);
        }
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupSet_PreRender(object sender, EventArgs e)
    {
        rgClientBackupSet.ShowHeader = true;
        rgClientBackupSet.ShowFooter = true;
        rgClientBackupSet.PagerStyle.AlwaysVisible = true;
        rgClientBackupSet.PagerStyle.Visible = true;
        rgClientBackupSet.MasterTableView.NoMasterRecordsText = "No records found.";

        GridCommandItem commandItem = null; //add link button 

        if (rgClientBackupSet.MasterTableView.Items.Count > 0)
        {
            commandItem = (GridCommandItem)rgClientBackupSet.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;
        }

        #region add / editing ticket

        if (rgClientBackupSet.EditItems.Count > 0 || rgClientBackupSet.MasterTableView.IsItemInserted)
        {
            //hide all other line items while in edit mode
            foreach (GridDataItem item in rgClientBackupSet.MasterTableView.Items)
                item.Visible = false;

            rgClientBackupSet.ShowHeader = false; //hide grid header 
            rgClientBackupSet.ShowFooter = false; //hide grid footer

            //hide grid pager
            rgClientBackupSet.PagerStyle.AlwaysVisible = false;
            rgClientBackupSet.PagerStyle.Visible = false;

            //hide add link
            if (commandItem != null)
                    commandItem.Visible = false;
        }

        #endregion

        
    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupSet_ItemCommand(object sender, GridCommandEventArgs e)
    {
        bool _rebind = false;
        string _confirmationMessage = "";

        if ((e.CommandName == "Deactivate") || (e.CommandName == "Activate"))
        {
            int Id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"]);
            DesktopShared.EntityClasses.BackupSetEntity backupSet = new DesktopShared.EntityClasses.BackupSetEntity(Id);
            backupSet.Active = (e.CommandName == "Activate");
            backupSet.Save();
            _rebind = true;
            _confirmationMessage = String.Format("Backup task has been {0}", (e.CommandName == "Activate") ? "activated" : "deactivated");
        }
        else if (e.CommandName == "Delete")
        {
            int Id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"]);
            DesktopShared.EntityClasses.BackupSetEntity backupSet = new DesktopShared.EntityClasses.BackupSetEntity(Id);
            backupSet.BackupMonitor.DeleteMulti();
            backupSet.Delete();
            _rebind = true;
            _confirmationMessage = "Backup task has been deleted";
        }

        if (_rebind)
            ResetGrid();

        if (_confirmationMessage.Trim().Length > 0)
        {
            rnConfirmation.Text = _confirmationMessage;
            rnConfirmation.Show();
        }
    }

    /// <summary>
    /// grid on item created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupSet_ItemCreated(object sender, GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem item = e.Item as GridDataItem;

            #region activate/deactivate

            int _id = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"], -1);
            bool _active = BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Active"],true);

            TableCell tcDeactivate = item["Deactivate"];
            tcDeactivate.Controls.Clear();

            LinkButton btnAction = new LinkButton();
            btnAction.ID = "btnAction";
            btnAction.CommandArgument = _id.ToString();
            if (_active)
            {
                btnAction.Text = "Deactivate";
                btnAction.ToolTip = "Make this Backup Task inactive";
                btnAction.CommandName = "Deactivate";
                btnAction.OnClientClick = "return confirm(\"Confirm Deactivate?\");";
            }
            else
            {
                btnAction.Text = "Activate";
                btnAction.ToolTip = "Make this Backup Task active";
                btnAction.CommandName = "Activate";
                btnAction.OnClientClick = "return confirm(\"Confirm Activate?\");";
            }
            
            tcDeactivate.Controls.Add(btnAction);

            #endregion

        }

        #endregion
    }

    #endregion

    /// <summary>
    /// records per page drop list on selected index changed -> set number of records to display per grid page and rebind grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRecordsPerPage_SelectedChange(object sender, EventArgs e)
    {
        TicketsPerPage = ucRecordsPerPage.SelectedValue;
        rgClientBackupSet.Rebind();
    }

    #endregion

    #region private methods

    /// <summary>
    /// set up records per page -> set selected value in drop down list 
    /// </summary>
    private void SetUpRecordsPerPage()
    {
        ucRecordsPerPage.SelectedValue = TicketsPerPage;
    }
    
    /// <summary>
    /// set show/hide of grid via click -> todo: remove?
    /// </summary>
    private void SetShowHide()
    {
        //<a href="#" class="hide" onclick="showHideInfo(this, '<%=this.ClientID %>'); return false;"></a>

        if (!SetShowHideDone)
        {
            string _linkClass = "hide";
            string _divDisplay = "";

            if (HideGrid)
            {
                _linkClass = "show";
                _divDisplay = "none";
            }

            hlShowHide.Attributes.Add("href", "#");
            hlShowHide.Attributes.Add("class", _linkClass);

            string _onClickFunction = "showHideInfo(this, '" + divClientBackupSet.ClientID + "');";
            _onClickFunction += "return false;";
            hlShowHide.Attributes.Add("onclick", _onClickFunction);

            divClientBackupSet.Attributes.CssStyle.Add("display", _divDisplay);

            SetShowHideDone = true;
        }
    }

    #endregion

    #region private properties

    /// <summary>
    /// get/set tickets per page (stored in session)
    /// </summary>
    private int TicketsPerPage
    {
        get
        {
            object obj = this.Session["TicketsPerPage"];
            return (obj == null) ? 25 : (int)obj;
        }
        set { this.Session["TicketsPerPage"] = value; }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set show hide done (stored in viewstate) todo: needed? - use ajax CollapsiblePanelExtender instead?
    /// </summary>
    public bool SetShowHideDone
    {
        get
        {
            object obj = this.ViewState["SetShowHideDoneForClientBackupSet"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["SetShowHideDoneForClientBackupSet"] = value;}
    }

    /// <summary>
    /// get/set hide grid (stored in viewstate) todo: needed? - use ajax CollapsiblePanelExtender instead?
    /// </summary>
    public bool HideGrid
    {
        get
        {
            object obj = this.ViewState["HideGridForClientBackupSet"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["HideGridForClientBackupSet"] = value; }
    }

    #region search

    /// <summary>
    /// get/set search client id (stored in viewstate)
    /// </summary>
    public int? SearchClientId
    {
        get
        {
            object obj = this.ViewState["SearchClientIdForBackupSet"];
            return (obj == null) ? (int?) null :(int)obj;
        }
        set { this.ViewState["SearchClientIdForBackupSet"] = value; }
    }

    /// <summary>
    /// get/set search created last days
    /// </summary>
    public int? SearchCreatedLastDays
    {
        get
        {
            object obj = this.ViewState["scld_bs"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["scld_bs"] = value; }
    }

    /// <summary>
    /// get/set search backup type id (stored in viewstate)
    /// </summary>
    public int? SearchBackupTypeId
    {
        get
        {
            object obj = this.ViewState["SearchBackupTypeIdForBackupSet"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchBackupTypeIdForBackupSet"] = value; }
    }

    /// <summary>
    /// get/set search active (stored in viewstate)
    /// </summary>
    public bool? SearchActive
    {
        get
        {
            object obj = this.ViewState["SearchActiveForBackupSet"];
            return (obj == null) ? (bool?)null : (bool)obj;
        }
        set { this.ViewState["SearchActiveForBackupSet"] = value; }
    }

    /// <summary>
    /// get/set search server name (stored in viewstate)
    /// </summary>
    public string SearchServerName
    {
        get
        {
            object obj = this.ViewState["SearchServerNameForBackupSet"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["SearchServerNameForBackupSet"] = value; }
    }

    /// <summary>
    /// get/set search backup server name (stored in viewstate)
    /// </summary>
    public string SearchBackupServerName
    {
        get
        {
            object obj = this.ViewState["sbsn_cbs"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["sbsn_cbs"] = value; }
    }

    #endregion

    #endregion

    #region public methods

    /// <summary>
    /// reset grid
    /// </summary>
    public void ResetGrid()
    {
        rgClientBackupSet.Visible = true;
        pnlHeader.Visible = true;
        rgClientBackupSet.EditIndexes.Clear();
        rgClientBackupSet.DataSource = null;
        rgClientBackupSet.Rebind();
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        rgClientBackupSet.CurrentPageIndex = 0;
        rgClientBackupSet.EditIndexes.Clear();
        rgClientBackupSet.Visible = false;
        pnlHeader.Visible = false;
    }

    #endregion

    
}