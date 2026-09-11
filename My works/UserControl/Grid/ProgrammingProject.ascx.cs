using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Telerik.Web.UI;
using System.Data;

public partial class UserControl_Grid_ProgrammingProject : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //records per page event handler
        DropDownList ddlRecordsPerPage = ucRecordsPerPage.GetDropDownList();
        ddlRecordsPerPage.SelectedIndexChanged += new EventHandler(ddlRecordsPerPage_SelectedChange);

        if (!this.IsPostBack)
            SetUpRecordsPerPage(); //set value for drop down list control for number of records to display
    }

    #region protected methods / events

    #region telerik grid events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgProgrammingProject_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //get typed list
        DesktopShared.TypedListClasses.ProgrammingProjectTypedList _programmingProjectTl = DesktopShared.ProgrammingProject.Get(SearchClientId, SearchFqdn);
        
        //bind grid
        rgProgrammingProject.DataSource = _programmingProjectTl;

        #region paging

        if (TicketsPerPage > 0)
            rgProgrammingProject.PageSize = TicketsPerPage;
        else
        {
            if (_programmingProjectTl.Rows.Count > 0)
                rgProgrammingProject.PageSize = _programmingProjectTl.Rows.Count;
        }

        #endregion
    }

    /// <summary>
    /// grid on item created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgProgrammingProject_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
         #region GridItemType.Item or GridItemType.AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem item = e.Item as GridDataItem;

            #region edit

            int ID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());

            TableCell tcEdit = item["Edit"];
            tcEdit.Controls.Clear();

            HyperLink hlEdit = new HyperLink();
            hlEdit.ID = "hlEdit";
            hlEdit.Text = "Edit";
            hlEdit.NavigateUrl = String.Format("/ProgrammingProject/Detail.aspx?ProgrammingProjectId={0}", ID);

            tcEdit.Controls.Add(hlEdit);

            #endregion

        }

        #endregion
    }

    /// <summary>
    /// grid on item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgProgrammingProject_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            
        }

        #endregion

        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DesktopShared.TypedListClasses.ProgrammingProjectRow objProgrammingProject =
                (DesktopShared.TypedListClasses.ProgrammingProjectRow)
                ((DataRowView)e.Item.DataItem).Row;

            #region production fqdn

            string _fqdn = objProgrammingProject.ProductionFqdn.Trim();
            if (!_fqdn.Contains("http"))
                _fqdn = "http://" + _fqdn.Trim();

            TableCell tcProductionFqdn = item["ProductionFqdn"];
            tcProductionFqdn.Controls.Clear();

            HyperLink hlProductionFqdn = new HyperLink();
            hlProductionFqdn.ID = "hlProductionFqdn";
            hlProductionFqdn.Target = "_blank";
            hlProductionFqdn.Text = objProgrammingProject.ProductionFqdn.Trim();
            hlProductionFqdn.NavigateUrl = _fqdn;

            tcProductionFqdn.Controls.Add(hlProductionFqdn);
            
            #endregion

        }

        #endregion

    }

    /// <summary>
    /// grid on delete command -> delete entity
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgProgrammingProject_DeleteCommand(object source, GridCommandEventArgs e)
    {
        int Id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"]);

        DesktopShared.EntityClasses.ProgrammingProjectEntity objProgrammingProject = new DesktopShared.EntityClasses.ProgrammingProjectEntity(Id);
        objProgrammingProject.Delete();
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
        rgProgrammingProject.Rebind();
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

    #endregion

    #region public methods

    /// <summary>
    /// reset grid
    /// </summary>
    public void ResetGrid()
    {
        rgProgrammingProject.Visible = true;
        pnlHeader.Visible = true;

        rgProgrammingProject.EditIndexes.Clear();
        rgProgrammingProject.DataSource = null;
        rgProgrammingProject.Rebind();
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        rgProgrammingProject.CurrentPageIndex = 0;
        rgProgrammingProject.EditIndexes.Clear();
        rgProgrammingProject.Visible = false;
        pnlHeader.Visible = false;
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
            if (obj == null)
                return 25;
            else
                return (int)obj;
        }

        set
        {
            this.Session["TicketsPerPage"] = value;
        }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set search client id (stored in viewstate)
    /// </summary>
    public int? SearchClientId
    {
        get
        {
            object obj = this.ViewState["SearchClientIdForProgrammingProject"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchClientIdForProgrammingProject"] = value; }
    }

    /// <summary>
    /// get/set search fqdn (stored in viewstate)
    /// </summary>
    public string SearchFqdn
    {
        get
        {
            object obj = this.ViewState["SearchFqdnForProgrammingProject"];
            if (obj == null)
                return "";
            else
                return (string)obj;
        }
        set { this.ViewState["SearchFqdnForProgrammingProject"] = value; }
    }

    #endregion
}