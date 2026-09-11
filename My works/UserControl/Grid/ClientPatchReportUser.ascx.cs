using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControl_Grid_ClientPatchReportUser : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();
    }

    #region private methods
    private void ConfigureForDevice()
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            rgPatchReportUser.RenderMode = RenderMode.Lightweight;
        }
    }
    #endregion

    #region public methods

    public void RebindGrid()
    {
        rgPatchReportUser.Visible = true;
        rgPatchReportUser.EditIndexes.Clear();
        rgPatchReportUser.DataSource = null;
        rgPatchReportUser.Rebind();
    }

    #endregion

    #region protected methods / events
    protected void rgPatchReportUser_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (ClientId.HasValue)
        {
            rgPatchReportUser.Visible = true;

            DataTable _dt = new DataTable();
            _dt.Columns.Add("Id", typeof(System.Int32));
            _dt.Columns.Add("UserId", typeof(System.Int32));
            _dt.Columns.Add("EmployeeId", typeof(System.Int32));
            _dt.Columns.Add("FullName", typeof(string));

            DesktopShared.TypedListClasses.ClientPatchReportUsersTypedList _report= IsEmployee ? DesktopShared.PatchReport.GetAllUsers(ClientId.Value, "Patch", true, false) : DesktopShared.PatchReport.GetAllUsers(ClientId.Value, "Patch", false, true);
            foreach (var objReport in _report)
            {
                DataRow _dr = _dt.NewRow();
                _dr[0] = objReport.Id;
                _dr[1] = objReport.UserId;
                _dr[2] = objReport.EmployeeId;
                if (IsEmployee)
                    _dr[3] = String.Format("{0} {1} ({2})", objReport.EmployeeFirst.Trim(), objReport.EmployeeLast.Trim(), objReport.EmployeeEmail.Trim());
                else
                    _dr[3] = String.Format("{0} {1} ({2})", objReport.UserFirst.Trim(), objReport.UserLast.Trim(), objReport.UserEmail.Trim());

                _dt.Rows.Add(_dr);
            }

            rgPatchReportUser.DataSource = _dt;
        }
        else
            rgPatchReportUser.Visible = false;
    }

    protected void rgPatchReportUser_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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

    protected void rgPatchReportUser_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;
            rgPatchReportUser.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgPatchReportUser.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(550);
        }
    }

    protected void rgPatchReportUser_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            int _id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());
            DesktopShared.EntityClasses.ClientPatchReportUserEntity objClientPatchReportUser = new DesktopShared.EntityClasses.ClientPatchReportUserEntity(_id);
            objClientPatchReportUser.Delete();
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
            object obj = this.ViewState["cid_preport"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_preport"] = value; }
    }

    /// <summary>
    /// get/set is employee
    /// </summary>
    public bool IsEmployee
    {
        get
        {
            object obj = this.ViewState["ie_preport"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["ie_preport"] = value; }
    }

    /// <summary>
    /// get record count
    /// </summary>
    public int RecordCount
    {
        get { return rgPatchReportUser.MasterTableView.Items.Count; }
    }

    #endregion

    #region public events

    /// <summary>
    /// add event handler for item command
    /// </summary>
    public event EventHandler evItemCommand;

    #endregion  
}