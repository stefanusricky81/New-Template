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

public partial class UserControl_Grid_TicketFile : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //ajax manager, set conditional postback script to cancel ajax on grid file upload
        Telerik.Web.UI.RadAjaxManager.GetCurrent(this.Page).ClientEvents.OnRequestStart = "conditionalPostback";
    }

    #region public methods

    /// <summary>
    /// rebind grid
    /// </summary>
    public void RebindGrid()
    {
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
        if (TicketId > 0)
        {
            rgTicketFile.Visible = true;
            DataTable dtResult = DesktopShared.Ticket.FileUpload.GetFilesForTicket(TicketId);
            rgTicketFile.DataSource = dtResult;
        }
        else
        {
            rgTicketFile.Visible = false;
        }
    }

    /// <summary>
    /// grid on item created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketFile_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            if (e.Item.OwnerTableView.IsItemInserted)
            {
                GridEditFormItem editForm = e.Item as GridEditFormItem;

                AbstractTicketFileEdit ucTicketFileEdit =
                    e.Item.FindControl(GridEditFormItem.EditFormUserControlID)
                    as AbstractTicketFileEdit;
                
                Button btnClose = (Button)ucTicketFileEdit.FindControl("btnClose");
                if (btnClose != null)
                    btnClose.Focus();
            }
        }

        #endregion
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketFile_PreRender(object sender, EventArgs e)
    {
        rgTicketFile.ShowHeader = true;
        rgTicketFile.PagerStyle.AlwaysVisible = true;
        rgTicketFile.PagerStyle.Visible = true;
        rgTicketFile.MasterTableView.NoMasterRecordsText = "No records found.";

        GridCommandItem commandItem = null;

        if (rgTicketFile.MasterTableView.Items.Count > 0)
        {
            commandItem = (GridCommandItem)rgTicketFile.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;
        }

        if (rgTicketFile.EditItems.Count > 0 || rgTicketFile.MasterTableView.IsItemInserted)
        {
            foreach (GridDataItem item in rgTicketFile.MasterTableView.Items)
            {
                item.Visible = false;
            }

            rgTicketFile.ShowHeader = false;
            rgTicketFile.PagerStyle.AlwaysVisible = false;
            rgTicketFile.PagerStyle.Visible = false;

            if (commandItem != null)
                commandItem.Visible = false;
        }   
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketFile_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            rgTicketFile.MasterTableView.NoMasterRecordsText = "";
        }

        #endregion

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

        #region GridCommandItem

        if (e.Item is GridCommandItem)
        {
            GridCommandItem cmdItem = (GridCommandItem)e.Item;
            cmdItem.Visible = ShowUploadLink;

            cmdItem.FindControl("RebindGridButton").Visible = false;
        }

        #endregion

    }

    /// <summary>
    /// grid on insert command
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTicketFile_InsertCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;

            AbstractTicketFileEdit ucTicketFileEdit =
                e.Item.FindControl(GridEditFormItem.EditFormUserControlID)
                as AbstractTicketFileEdit;
            
            ucTicketFileEdit.SaveValues(TicketId);
        }
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
                litMessage.Text = String.Format("Unable to get File - {0}", _errorMessage);
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
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set show upload link (stored in view state)
    /// </summary>
    public bool ShowUploadLink
    {
        get
        {
            object obj = this.ViewState["ShowUploadLink"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["ShowUploadLink"] = value;
        }
    }

    /// <summary>
    /// get/set ticket id (stored in view state)
    /// </summary>
    public int TicketId
    {
        get
        {
            object obj = this.ViewState["TicketId"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["TicketId"] = value;
            rgTicketFile.DataSource = null;
            rgTicketFile.Rebind();
        }
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


   
}
