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

public partial class UserControl_Grid_ServerInfo : System.Web.UI.UserControl
{


    protected void Page_Load(object sender, EventArgs e)
    {
        DropDownList ddlRecordsPerPage = ucRecordsPerPage.GetDropDownList();
        ddlRecordsPerPage.SelectedIndexChanged += new EventHandler(ddlRecordsPerPage_SelectedChange);

        litDivBegin.Text = "<div id=\"" + this.ClientID + "\">";

        if (!this.IsPostBack)
            SetUpRecordsPerPage();
    }

    protected void rgServerInfo_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
            pnlHeader.Visible = true;
            
            rgServerInfo.Visible = true;

            #region get ServerInfoCollection

            
            
        
            DesktopShared.TypedListClasses.ServerIpAddressTypedList sip =
                    new DesktopShared.TypedListClasses.ServerIpAddressTypedList();

        
            IPredicateExpression sipFilter = new PredicateExpression();
        //    sipFilter.Add(DesktopShared.HelperClasses.ServerInfoFields.ServerInfoId
        //        == ServerInfoId);

            ISortExpression sipSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
            sipSort.Add(DesktopShared.HelperClasses.ServerFields.MachineName | SortOperator.Descending);

            sip.Fill(0,sipSort,false,sipFilter);

            #endregion

            rgServerInfo.DataSource = sip;
            if (RecordsPerPage > 0)
                rgServerInfo.PageSize = RecordsPerPage;
            else
            {
                if (sip.Count > 0)
                    rgServerInfo.PageSize = sip.Count;
            }
    }

    protected void rgServerInfo_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
//            GridEditableItem item = e.Item as GridEditableItem;

 //           item["User"].Text = item["User"].Text.ToLower();
        }

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
/*            UserControl_Grid_EditForm_ServerInfoEdit ucAuthInfoLogEdit = e.Item.FindControl
                (GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ServerInfoEdit;

            if (!e.Item.OwnerTableView.IsItemInserted)
            {
                DesktopShared.EntityClasses.ServerInfoEntity log =
                    (DesktopShared.EntityClasses.ServerInfoEntity)e.Item.DataItem;

                if (log.Created.HasValue)
                    ucAuthInfoLogEdit.Created = log.Created.Value;
                ucAuthInfoLogEdit.Description = log.Description.Trim();
                ucAuthInfoLogEdit.User = log.User.Trim();
                ucAuthInfoLogEdit.Note = log.Note.Trim();
            }
  */      }
    }

    protected void ddlRecordsPerPage_SelectedChange(object sender, EventArgs e)
    {
        RecordsPerPage = ucRecordsPerPage.SelectedValue;
        rgServerInfo.Rebind();
    }

    private void SetUpRecordsPerPage()
    {
        ucRecordsPerPage.SelectedValue = RecordsPerPage;
    }


    #region private variables held in Session state

    private int RecordsPerPage
    {
        get
        {
            object obj = this.ViewState["RecordsPerPage"];
            if (obj == null)
                return 10;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["RecordsPerPage"] = value;
        }
    }

    #endregion

    #region public variables held in ViewState
    /*
    public int ServerInfoId
    {
        get
        {
            object obj = this.ViewState["AuthInfoIdForLog"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["AuthInfoIdForLog"] = value;
            rgServerInfo.DataSource = null;
            rgServerInfo.Rebind();
        }
    }

    public string HeaderBGColor
    {
        set { tblHeader.Attributes.CssStyle.Add("background-color", value); }
    }
    */
    #endregion

}
