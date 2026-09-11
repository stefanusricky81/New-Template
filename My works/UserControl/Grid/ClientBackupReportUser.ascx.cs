using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_Grid_ClientBackupReportUser: System.Web.UI.UserControl
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();
    }

    #region private methods

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            rgBackupReportUser.RenderMode = RenderMode.Lightweight;
        }
    }

    #endregion

    #region public methods

    /// <summary>
    /// rebind grid
    /// </summary>
    public void RebindGrid()
    {
        rgBackupReportUser.Visible = true;
        rgBackupReportUser.EditIndexes.Clear();
        rgBackupReportUser.DataSource = null;
        rgBackupReportUser.Rebind();
    }

    #endregion

    #region protected methods / events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgBackupReportUser_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (ClientId.HasValue)
        {
            rgBackupReportUser.Visible = true;

            DataTable _dt = new DataTable();
            _dt.Columns.Add("Id", typeof(System.Int32));
            _dt.Columns.Add("UserId", typeof(System.Int32));
            _dt.Columns.Add("EmployeeId", typeof(System.Int32));
            _dt.Columns.Add("FullName", typeof(string));

            DesktopShared.TypedListClasses.ClientBackupReportUsersTypedList _users = IsEmployee ? DesktopShared.Client.Report.Backup.GetEmployees(ClientId.Value) : DesktopShared.Client.Report.Backup.GetClientContacts(ClientId.Value);
            foreach (var objUser in _users)
            {
                DataRow _dr = _dt.NewRow();
                _dr[0] = objUser.Id;
                _dr[1] = objUser.UserId;
                _dr[2] = objUser.EmployeeId;
                if (IsEmployee)
                    _dr[3] = String.Format("{0} {1} ({2})", objUser.EmployeeFirst.Trim(), objUser.EmployeeLast.Trim(), objUser.EmployeeEmail.Trim());
                else
                    _dr[3] = String.Format("{0} {1} ({2})", objUser.UserFirst.Trim(), objUser.UserLast.Trim(), objUser.UserEmail.Trim());

                _dt.Rows.Add(_dr);
            }

            rgBackupReportUser.DataSource = _dt;
        }
        else
        {
            rgBackupReportUser.Visible = false;
        }
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgBackupReportUser_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;
            rgBackupReportUser.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgBackupReportUser.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(550);
        }
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgBackupReportUser_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region GridItemType.Item or GridItemType.AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem _gdi = e.Item as GridEditableItem;
            DataRowView _drv = (DataRowView)e.Item.DataItem;
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_drv["FullName"].ToString().Trim(), _gdi["FullName"]);
        }

        #endregion
    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgBackupReportUser_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName =="Remove")
        {
            int _id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());
            DesktopShared.EntityClasses.ClientBackupReportUserEntity objClientBackupReportUser = new DesktopShared.EntityClasses.ClientBackupReportUserEntity(_id);
            objClientBackupReportUser.Delete();
        }

        if (evItemCommand != null)
            evItemCommand(this, e);
    }


    #endregion

    #region public properties

    /// <summary>
    /// get/set client id
    /// </summary>
    public int? ClientId
    {
        get
        {
            object obj = this.ViewState["cid_cbrug"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_cbrug"] = value; }
    }

    /// <summary>
    /// get/set is employee
    /// </summary>
    public bool IsEmployee
    {
        get
        {
            object obj = this.ViewState["ie_cbrug"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["ie_cbrug"] = value; }
    }

    /// <summary>
    /// get record count
    /// </summary>
    public int RecordCount
    {
        get { return rgBackupReportUser.MasterTableView.Items.Count; }
    }

    #endregion

    #region public events

    /// <summary>
    /// add event handler for item command
    /// </summary>
    public event EventHandler evItemCommand;

    #endregion  
}
