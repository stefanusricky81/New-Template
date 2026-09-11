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


public partial class UserControl_Grid_KnowledgeBase : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DropDownList ddlRecordsPerPage = ucRecordsPerPage.GetDropDownList();
        ddlRecordsPerPage.SelectedIndexChanged += new EventHandler(ddlRecordsPerPage_SelectedChange);
        litDivBegin.Text = "<div id=\"" + this.ClientID + "\">";
    }

    #region grid events

    protected void rgKnowledgeBase_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataTable dtResult = DesktopShared.KnowledgeBase.GetKnowledgeBaseEntries(
           ClientId, SearchProductName, SearchKeywords, 
           SearchLastUpdatedStartDate, SearchLastUpdatedEndDate,
           SearchCreatedStartDate, SearchCreatedEndDate,
           SearchCreatedBy, SearchArticleno, SearchArchived);

        rgKnowledgeBase.Visible = true;
        rgKnowledgeBase.DataSource = dtResult;

        pnlHeader.Visible = true;
        SetUpRecordsPerPage();

        if (TicketsPerPage > 0)
            rgKnowledgeBase.PageSize = TicketsPerPage;
        else
        {
            if (dtResult.Rows.Count > 0)
                rgKnowledgeBase.PageSize = dtResult.Rows.Count;
        }
    }

    protected void rgKnowledgeBase_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            AbstractKnowledgeBaseEdit ucKBInfoEdit = e.Item.FindControl
                (GridEditFormItem.EditFormUserControlID) as AbstractKnowledgeBaseEdit;
            
            rgKnowledgeBase.MasterTableView.NoMasterRecordsText = "";

            #region edit existing

            if (!e.Item.OwnerTableView.IsItemInserted)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                ucKBInfoEdit.LoadValues(Convert.ToInt32(drv["Pknowledge"].ToString().Trim()));
            }

            #endregion

            #region add new item

            else
            {
                ucKBInfoEdit.LoadValues(0);
            }

            #endregion

        }

        #endregion

        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            #region subject

            TableCell tcSubject = item["subject"];
            string _subjectText = drv["subject"].ToString().Trim();

            HyperLink hlMemo = (HyperLink)tcSubject.Controls[0];
            int colWidth = 62;

            if (_subjectText.Length > colWidth)
                hlMemo.Text = _subjectText.Substring(0, colWidth) + " ... ";
            else
                hlMemo.Text = _subjectText;

            #endregion

        }

        #endregion

    }

    protected void rgKnowledgeBase_UpdateCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            AbstractKnowledgeBaseEdit ucKBInfoEdit = e.Item.FindControl
                (GridEditFormItem.EditFormUserControlID) as AbstractKnowledgeBaseEdit;

            int Id = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues
                [editedItem.ItemIndex]["Pknowledge"]);

            ucKBInfoEdit.SaveValues(Id);
        }
    }

    protected void rgKnowledgeBase_InsertCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            AbstractKnowledgeBaseEdit ucKBInfoEdit = e.Item.FindControl
                (GridEditFormItem.EditFormUserControlID) as AbstractKnowledgeBaseEdit;

            ucKBInfoEdit.SaveValues(0);
        }
    }

    protected void rgKnowledgeBase_DeleteCommand(object source, GridCommandEventArgs e)
    {
        int Id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues
            [e.Item.ItemIndex]["Pknowledge"]);

        DesktopShared.EntityClasses.KnowledgeEntity kbInfo =
            new DesktopShared.EntityClasses.KnowledgeEntity(Id);

        kbInfo.KnowledgeBaseLog.DeleteMulti();
        kbInfo.Delete();        
    }

    protected void rgKnowledgeBase_PreRender(object sender, EventArgs e)
    {
        rgKnowledgeBase.ShowHeader = true;
        rgKnowledgeBase.PagerStyle.AlwaysVisible = true;
        rgKnowledgeBase.PagerStyle.Visible = true;

        rgKnowledgeBase.MasterTableView.NoMasterRecordsText = "No records found.";

        GridCommandItem commandItem = null;

        if (rgKnowledgeBase.MasterTableView.Items.Count > 0)
        {
            commandItem = (GridCommandItem)rgKnowledgeBase.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;
        }

        if (rgKnowledgeBase.EditItems.Count > 0 || rgKnowledgeBase.MasterTableView.IsItemInserted)
        {
            foreach (GridDataItem item in rgKnowledgeBase.MasterTableView.Items)
            {
                item.Visible = false;
            }

            rgKnowledgeBase.ShowHeader = false;
            rgKnowledgeBase.PagerStyle.AlwaysVisible = false;
            rgKnowledgeBase.PagerStyle.Visible = false;

            if (commandItem != null)
                commandItem.Visible = false;
        }

        #region hide delete column if employee not admin

        foreach (GridColumn col in rgKnowledgeBase.MasterTableView.RenderColumns)
        {
            if (col.UniqueName == "Delete")
                col.Visible = DesktopShared.User.IsAdmin;

            int _clientColWidth = (DesktopShared.User.IsAdmin) ? 8 : 13;
            if (col.UniqueName == "FkClient")
                col.HeaderStyle.Width = Unit.Percentage(_clientColWidth);
        }

        #endregion
    }

    public void ResetGrid()
    {
        rgKnowledgeBase.EditIndexes.Clear();
        rgKnowledgeBase.DataSource = null;
        rgKnowledgeBase.Rebind();
    }

    public void ClearGrid()
    {
        rgKnowledgeBase.CurrentPageIndex = 0;
        rgKnowledgeBase.EditIndexes.Clear();
        rgKnowledgeBase.Visible = false;
        pnlHeader.Visible = false;
    }

    #endregion
    
    protected void ddlRecordsPerPage_SelectedChange(object sender, EventArgs e)
    {
        TicketsPerPage = ucRecordsPerPage.SelectedValue;
        rgKnowledgeBase.Rebind();
    }

    private void SetUpRecordsPerPage()
    {
        ucRecordsPerPage.SelectedValue = TicketsPerPage;
    }

    #region private variables held in Session state

    private int TicketsPerPage
    {
        get
        {
            object obj = this.Session["TicketsPerPage"];
            if (obj == null)
                return 50;
            else
                return (int)obj;
        }

        set
        {
            this.Session["TicketsPerPage"] = value;
        }
    }

    #endregion

    #region public variables held in ViewState

    public int ClientId
    {
        get
        {
            object obj = this.ViewState["ClientIdForKB"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["ClientIdForKB"] = value;
        }
    }

    public string SearchProductName
    {
        get
        {
            object obj = this.ViewState["SearchProductNameForKB"];
            if (obj == null)
                return "";
            else
                return (string)obj;
        }

        set
        {
           this.ViewState["SearchProductNameForKB"] = value;
        }
    }

    public string SearchKeywords
    {
        get
        {
            object obj = this.ViewState["SearchKeywordsForKB"];
            if (obj == null)
                return "";
            else
                return (string)obj;
        }

        set
        {
            this.ViewState["SearchKeywordsForKB"] = value;
        }
    }

    public int SearchCreatedBy
    {
        get
        {
            object obj = this.ViewState["SearchCreatedByForKB"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchCreatedByForKB"] = value;
        }
    }

    public int SearchArticleno
    {
        get
        {
            object obj = this.ViewState["SearchArticlenoForKB"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchArticlenoForKB"] = value;
        }
    }


    public int SearchProductId
    {
        get
        {
            object obj = this.ViewState["SearchProductIdForKB"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchProductIdForKB"] = value;
        }
    }

    public DateTime SearchCreatedStartDate
    {
        get
        {
            object obj = this.ViewState["SearchCreatedStartDateForKB"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }

        set
        {
            this.ViewState["SearchCreatedStartDateForKB"] = value;
        }
    }

    public DateTime SearchCreatedEndDate
    {
        get
        {
            object obj = this.ViewState["SearchCreatedEndDateForKB"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }

        set
        {
            this.ViewState["SearchCreatedEndDateForKB"] = value;
        }
    }

    public DateTime SearchLastUpdatedStartDate
    {
        get
        {
            object obj = this.ViewState["SearchLastUpdatedStartDateForKB"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }

        set
        {
            this.ViewState["SearchLastUpdatedStartDateForKB"] = value;
        }
    }

    public DateTime SearchLastUpdatedEndDate
    {
        get
        {
            object obj = this.ViewState["SearchLastUpdatedEndDateForKB"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }

        set
        {
            this.ViewState["SearchLastUpdatedEndDateForKB"] = value;
        }
    }

    public bool? SearchArchived
    {
        get
        {
            object obj = this.ViewState["SearchArchivedForKB"];
            if (obj == null)
                return null;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["SearchArchivedForKB"] = value;
        }
    }


    #endregion


}
