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
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_Grid_TicketTagResponsive : System.Web.UI.UserControl
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
            rgTicketTag.RenderMode = RenderMode.Lightweight;
        }
    }

    #endregion

    #region public methods

    /// <summary>
    /// rebind grid
    /// </summary>
    public void RebindGrid()
    {
        rgTicketTag.Visible = true;
        rgTicketTag.EditIndexes.Clear();
        rgTicketTag.DataSource = null;
        rgTicketTag.Rebind();
    }

    #endregion

    #region protected methods / events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTicketTag_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (TicketId.HasValue)
        {
            rgTicketTag.Visible = true;
            rgTicketTag.DataSource = DesktopShared.Ticket.Tagged.GetTagsForTicket(TicketId.Value, DesktopShared.User.UserID);
        }
        else
        {
            rgTicketTag.Visible = false;
        }
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketTag_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;
            rgTicketTag.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgTicketTag.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(250);
            rgTicketTag.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
        }
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketTag_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region GridItemType.Item or GridItemType.AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem _gdi = e.Item as GridEditableItem;
            DesktopShared.TypedListClasses.TicketTagsRow objTag =(DesktopShared.TypedListClasses.TicketTagsRow)((DataRowView)e.Item.DataItem).Row;

            string publicClient = objTag.Public ? " (P" : "";
            if (objTag.ClientPublic)
            {
                if (publicClient.Length > 0)
                    publicClient += "C)";
                else
                    publicClient = " (C)";
            }
            else
            {
                if (publicClient.Length > 0)
                    publicClient += ")";
            }

            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(String.Format("{0}{1}", objTag.Name.Trim(), publicClient), _gdi["Name"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(objTag.Created.ToString(), _gdi["Created"]);
        }

        #endregion
    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketTag_ItemCommand(object sender, GridCommandEventArgs e)
    {
        #region delete

        if (e.CommandName == "Delete")
        {
            int taggedTicketValueId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TaggedTicketValueId"].ToString());
            DesktopShared.Ticket.Tagged.DeleteTagValue(taggedTicketValueId);
            RebindGrid();
        }

        #endregion

        if (evItemCommand != null)
            evItemCommand(this, e);
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set ticket id
    /// </summary>
    public int? TicketId
    {
        get
        {
            object obj = this.ViewState["tid_ttg"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["tid_ttg"] = value; }
    }

    #endregion

    #region public events

    /// <summary>
    /// add event handler for item command
    /// </summary>
    public event EventHandler evItemCommand;

    #endregion
}