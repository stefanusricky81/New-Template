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

public partial class UserControl_Grid_Product : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //records per page event handler
        DropDownList ddlRecordsPerPage = ucRecordsPerPage.GetDropDownList();
        ddlRecordsPerPage.SelectedIndexChanged += new EventHandler(ddlRecordsPerPage_SelectedChange);

        if (!this.IsPostBack)
        {
            SetShowHide(); //hide/show grid
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
    protected void rgProduct_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        var _products = DesktopShared.Product.GetTypedList();
        rgProduct.DataSource = _products;
        DisplayMessage("");
        #region paging

        if (TicketsPerPage > 0)
            rgProduct.PageSize = TicketsPerPage;
        else
        {
            if (_products.Rows.Count > 0)
                rgProduct.PageSize = _products.Rows.Count;
        }

        #endregion
    }

    /// <summary>
    /// grid on item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgProduct_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            rgProduct.MasterTableView.NoMasterRecordsText = "";
            AbstractProductEdit ucProductEdit  = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as AbstractProductEdit;
            if (!e.Item.OwnerTableView.IsItemInserted) //edit
            {
                DesktopShared.TypedListClasses.ProductRow objProduct = (DesktopShared.TypedListClasses.ProductRow)((DataRowView)e.Item.DataItem).Row;
                ucProductEdit.LoadValues(objProduct);
            }
            else //add
            {
                ucProductEdit.LoadValues(null);
            }
        }

        #endregion

        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DesktopShared.TypedListClasses.ProductRow objProduct = (DesktopShared.TypedListClasses.ProductRow)((DataRowView)e.Item.DataItem).Row;

            TelerikHelper.AddLabelToCell(objProduct.Name, item["Name"]);
            TelerikHelper.AddLabelToCell(objProduct.Description, item["Description"]);
        }

        #endregion
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgProduct_PreRender(object sender, EventArgs e)
    {
        #region default - show header, footer, pager.  set no records text

        rgProduct.ShowHeader = true;
        rgProduct.ShowFooter = true;

        rgProduct.PagerStyle.AlwaysVisible = true;
        rgProduct.PagerStyle.Visible = true;

        rgProduct.MasterTableView.NoMasterRecordsText = "No records found.";

        #endregion

        GridCommandItem commandItem = null; //add link button 

        if (rgProduct.MasterTableView.Items.Count > 0)
        {
            commandItem = (GridCommandItem)rgProduct.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;
        }

        #region edit or add mode

        //edit = rgProduct.EditItems.Count > 0
        //add = rgProduct.MasterTableView.IsItemInserted

        if (rgProduct.EditItems.Count > 0 || rgProduct.MasterTableView.IsItemInserted) 
        {
            foreach (GridDataItem item in rgProduct.MasterTableView.Items)
            {
                item.Visible = false; //hide all rows not being edited
            }

            rgProduct.ShowHeader = false; //hide grid header 
            rgProduct.ShowFooter = false; //hide grid footer

            //hide grid pager
            rgProduct.PagerStyle.AlwaysVisible = false;
            rgProduct.PagerStyle.Visible = false;
            
            //hide add link button
            if (commandItem != null)
                commandItem.Visible = false;
        }

        #endregion
    }

    /// <summary>
    /// grid on update command -> edit entity
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgProduct_UpdateCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            int Id = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues [editedItem.ItemIndex]["Id"]);            
            AbstractProductEdit ucProductEdit = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as AbstractProductEdit;
            ucProductEdit.SaveValues(Id);
        }
    }

    /// <summary>
    /// grid on insert command -> add entity
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgProduct_InsertCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
            AbstractProductEdit ucProductEdit = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as AbstractProductEdit;
            ucProductEdit.SaveValues(null);
        }
    }

    /// <summary>
    /// grid on delete command -> delete entity
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgProduct_DeleteCommand(object source, GridCommandEventArgs e)
    {
        int Id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"]);
        DesktopShared.EntityClasses.ProductEntity objProduct = new DesktopShared.EntityClasses.ProductEntity(Id);

        bool _error = false;
        try { objProduct.Delete(); }
        catch { _error = true;  }

        ResetGrid();
        if (_error)
            DisplayMessage("Product is in use and cannot be deleted.", true);
        else
            DisplayMessage("Product has been deleted.");
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
        rgProduct.Rebind();
    }

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

            string _onClickFunction = "showHideInfo(this, '" + divProduct.ClientID + "');";
            _onClickFunction += "return false;";
            hlShowHide.Attributes.Add("onclick", _onClickFunction);

            divProduct.Attributes.CssStyle.Add("display", _divDisplay);

            SetShowHideDone = true;
        }
    }

    #endregion

    #region public methods

    /// <summary>
    /// reset grid
    /// </summary>
    public void ResetGrid()
    {
        rgProduct.Visible = true;
        pnlHeader.Visible = true;

        rgProduct.EditIndexes.Clear();
        rgProduct.DataSource = null;
        rgProduct.Rebind();
        DisplayMessage("");
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        rgProduct.CurrentPageIndex = 0;
        rgProduct.EditIndexes.Clear();
        rgProduct.Visible = false;
        pnlHeader.Visible = false;
    }

    #endregion

    #region private methods

    private void DisplayMessage(string message, bool isError = false)
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            litMessage.Text = "";
            return;
        }

        litMessage.Text = String.Format("<p{1}>{0}</p>", message.Trim(), isError ? " style=\"color:red; font-weight:bold\"" : "");
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
    /// get/set show hide done (stored in viewstate)
    /// </summary>
    public bool SetShowHideDone
    {
        get
        {
            object obj = this.ViewState["SetShowHideDoneForProduct"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["SetShowHideDoneForProduct"] = value; }
    }

    /// <summary>
    /// get/set hide grid (stored in viewstate)
    /// </summary>
    public bool HideGrid
    {
        get
        {
            object obj = this.ViewState["HideGridForProduct"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["HideGridForProduct"] = value; }
    }

    
    #endregion

}
