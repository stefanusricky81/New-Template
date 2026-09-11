using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.IO;

public partial class UserControl_Grid_TicketFileResponsive : System.Web.UI.UserControl
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
            rgTicketFile.RenderMode = RenderMode.Lightweight;
            phTabletPagerCss.Visible = true;
        }
    }

    #endregion

    #region public methods

    /// <summary>
    /// rebind grid
    /// </summary>
    public void RebindGrid()
    {
        litMessage.Text = "";
        rgTicketFile.Visible = true;
        rgTicketFile.EditIndexes.Clear();
        rgTicketFile.DataSource = null;
        rgTicketFile.Rebind();
    }

    #endregion

    #region protected methods / events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTicketFile_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (TicketId.HasValue)
        {
            rgTicketFile.Visible = true;
            DataTable dtResult = DesktopShared.Ticket.FileUpload.GetFilesForTicket(TicketId.Value);
            rgTicketFile.DataSource = dtResult;
        }
        else
        {
            rgTicketFile.Visible = false;
        }
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketFile_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;
            rgTicketFile.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgTicketFile.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgTicketFile.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(250);
            rgTicketFile.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgTicketFile.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);

        }
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketFile_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region GridItemType.Item or GridItemType.AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            #region uploaded by

            TableCell tcUploadedBy = item["UploadedBy"];
            tcUploadedBy.Text = drv["UserFirst"].ToString().Trim() + " " + drv["UserLast"].ToString().Trim();

            #endregion

            #region file

            LinkButton btnViewFile = e.Item.FindControl("btnViewFile") as LinkButton;
            btnViewFile.Text = drv["Name"].ToString().Trim();

            #endregion
        }

        #endregion
    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketFile_ItemCommand(object sender, GridCommandEventArgs e)
    {
        #region view file

        if (e.CommandName == "ViewFile")
        {
            int _id = int.Parse((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pcscdefectfile"].ToString());
            var objFile = new DesktopShared.EntityClasses.CscdefectfileEntity(_id);
            string _errorMessage = "";
            System.IO.MemoryStream _ms = null;

            #region azure

            if (objFile.AzureStorage)
            {
                string _containerName = objFile.AzureContainerName.Trim();
                DesktopShared.AzureHelper.ContainerName _containerNameEnum = DesktopShared.AzureHelper.ContainerName.client;
                if (!Enum.TryParse(_containerName, out _containerNameEnum))
                {
                    _errorMessage = "Unable to parse Container Name";
                    RebindGrid();
                    litMessage.Text = String.Format("Unable to get File - {0}", _errorMessage);
                    return;
                }

                _ms = DesktopShared.AzureHelper.GetBlobAsStream(_containerNameEnum, objFile.AzureFileId, ref _errorMessage);
            }

            #endregion

            #region local files

            else
            {
                string _file = BitByBit.Configuration.GetConfigString("TicketUploadPath");
                if ((objFile.FkClient ?? 0) > 0)
                    _file += String.Format("{0}\\{1}", objFile.FkClient.Value, objFile.Name.Trim());
                else
                {
                    var objCscProjects = objFile.CscProjects;
                    if ((objCscProjects != null) && (objCscProjects.Fields.State == EntityState.Fetched))
                    {
                        //data/651/project_2101/cscproject_117/files/BitxBit_TicketError_20090403_105344.JPG
                        _file = _file.Replace("files\\", "");
                        _file += String.Format("{0}\\project_{1}\\cscproject_{2}\\files\\{3}", objCscProjects.FkClient, objFile.FkProject, objFile.FkCscprojects, objFile.Name.Trim());
                    }
                }

                if (System.IO.File.Exists(_file))
                {
                    _ms = new MemoryStream();
                    using (FileStream _fs = new FileStream(_file, FileMode.Open, FileAccess.Read))
                        _fs.CopyTo(_ms);
                }
                else
                    _errorMessage = String.Format("File does not exist - {0}", _file);

            }

            #endregion

            if (_ms == null)
            {
                RebindGrid();
                if (evItemCommand != null)
                    evItemCommand(false, String.Format("Unable to get File: {0}", _errorMessage));
                return;
            }
            else
            {
                string _headerValue = String.Format("attachment;filename={0}", objFile.Name.Trim());
                Response.ContentType = MimeMapping.GetMimeMapping(objFile.Name.Trim());
                Response.AddHeader("content-disposition", _headerValue);
                Response.Buffer = true;
                _ms.WriteTo(Response.OutputStream);
                Response.End();
            }
        }

        #endregion

        #region delete 

        if (e.CommandName == "Delete")
        {
            int _id = int.Parse((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pcscdefectfile"].ToString());
            var objFile = new DesktopShared.EntityClasses.CscdefectfileEntity(_id);
            string _fileName = objFile.Name.Trim();
            objFile.Delete();
            DesktopShared.Ticket.History.Add(TicketId.Value, DesktopShared.User.UserID, "", String.Format("The following attachment was deleted: {0}", _fileName), false, false, DesktopShared.Ticket.History.Type.Id.AttachmentDeleted);

            if (evDeleteCompleted != null)
                evDeleteCompleted(this, e);
        }

        #endregion

        if (evItemCommand != null)
            evItemCommand(true, "");
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
            object obj = this.ViewState["tid_tfg"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["tid_tfg"] = value; }
    }

    /// <summary>
    /// get list of ids for files to email
    /// </summary>
    public List<int> FileIdsToEmail
    {
        get
        {
            List<int> _list = new List<int>();

            foreach (GridDataItem gridDataItem in rgTicketFile.MasterTableView.Items)
            {
                CheckBox chkBox = gridDataItem.FindControl("chkEmailFile") as CheckBox;
                if (chkBox.Checked)
                    _list.Add(Convert.ToInt32(gridDataItem.GetDataKeyValue("pcscdefectfile").ToString()));
            }
            return _list;
        }
    }

    #endregion

    #region public events

    /// <summary>
    /// delegate for item command
    /// </summary>
    /// <param name="message"></param>
    public delegate void ItemCommandHandler(bool success, string message);

    /// <summary>
    /// add event handler for item command
    /// </summary>
    public event ItemCommandHandler evItemCommand;

    /// <summary>
    /// event handler for deletion completed
    /// </summary>
    public event EventHandler evDeleteCompleted;

    #endregion
}
