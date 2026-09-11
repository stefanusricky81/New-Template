using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Telerik.Web.UI;

public partial class UserControl_Grid_KnowledgeBaseLog : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DropDownList ddlRecordsPerPage = ucRecordsPerPage.GetDropDownList();
        ddlRecordsPerPage.SelectedIndexChanged += new EventHandler(ddlRecordsPerPage_SelectedChange);

        litDivBegin.Text = "<div id=\"" + this.ClientID + "\">";

        if (!this.IsPostBack)
            SetUpRecordsPerPage();
    }

    protected void rgKnowledgeBaseLog_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (KnowledgeBaseId > 0)
        {
            pnlHeader.Visible = true;
            rgKnowledgeBaseLog.Visible = true;

            #region get KnowledgeBaseLogCollection

            DesktopShared.CollectionClasses.KnowledgeBaseLogCollection logs = 
                new DesktopShared.CollectionClasses.KnowledgeBaseLogCollection();

            IPredicateExpression logsFilter = new PredicateExpression();
            logsFilter.Add(DesktopShared.HelperClasses.KnowledgeBaseLogFields.KnowledgeId
                == KnowledgeBaseId);

            ISortExpression logsSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
            logsSort.Add(DesktopShared.HelperClasses.KnowledgeBaseLogFields.Id | SortOperator.Descending);

            logs.GetMulti(logsFilter, 0, logsSort, null);

            #endregion
           
            rgKnowledgeBaseLog.DataSource = logs;
            if (RecordsPerPage > 0)
                rgKnowledgeBaseLog.PageSize = RecordsPerPage;
            else
            {
                if (logs.Count > 0)
                    rgKnowledgeBaseLog.PageSize = logs.Count;
            }
        }
        else
        {
            pnlHeader.Visible = false;
            rgKnowledgeBaseLog.Visible = false;
        }
    }

    protected void rgKnowledgeBaseLog_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;

            item["User"].Text = item["User"].Text.ToLower();
        }
    }

    protected void ddlRecordsPerPage_SelectedChange(object sender, EventArgs e)
    {
        RecordsPerPage = ucRecordsPerPage.SelectedValue;
        rgKnowledgeBaseLog.Rebind();
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

    public int KnowledgeBaseId
    {
        get
        {
            object obj = this.ViewState["KnowledgeBaseIdForLog"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["KnowledgeBaseIdForLog"] = value;

            rgKnowledgeBaseLog.DataSource = null;
            rgKnowledgeBaseLog.Rebind();
        }
    }

    #endregion
}
