using DesktopShared.CollectionClasses;
using DesktopShared.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class DocFolder_Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindToDataSet();

            #region tabindex
            short _tabIndex = 0;

            txtSearchDocName.TabIndex = ++_tabIndex;
            btnSearch.TabIndex = ++_tabIndex;

            #endregion
            txtSearchDocName.Focus();
            Access();
        }
        //rgDocument.DataBind();
    }

    #region private methods

    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    private void BindToDataSet()
    {
        rtvFolder.DataSource = DesktopShared.Folder.GetFolderAndDocument();
        rtvFolder.DataTextField = "foldername";
        rtvFolder.DataFieldID = "id";
        rtvFolder.DataFieldParentID = "parentid";
        rtvFolder.DataValueField = "id";

        rtvFolder.DataNavigateUrlField = "documenturl";
        rtvFolder.DataBind();
    }

    private void BindGrid()
    {
        rgDocument.DataSource = DesktopShared.DocFolder.getsearchfolderandfile(txtSearchDocName.Text, ddlSearchBy.SelectedValue);
    }

    private void expandnodebyValue(string nodevalue)
    {
        RadTreeNode expandnode = rtvFolder.FindNodeByValue(nodevalue);
        if (expandnode != null)
        {
            expandnode.Expanded = true;
            expandnode.ExpandParentNodes();
            expandnode.ExpandChildNodes();
            expandnode.Selected = true;
        }
    }

    private void expandnodebytext(string nodevalue)
    {
        RadTreeNode expandnode = rtvFolder.FindNodeByText(nodevalue);
        if (expandnode != null)
        {
            expandnode.Expanded = true;
            expandnode.ExpandParentNodes();
            expandnode.ExpandChildNodes();
            expandnode.Selected = true;
        }
    }

    private void Access()
    {
        foreach (RadTreeNode node in rtvFolder.GetAllNodes())
        {
            LinkButton lbaddfolder = (LinkButton)node.FindControl("lbAddFolder");
            LinkButton lbrenamefolder = (LinkButton)node.FindControl("lbRenameFolder");
            LinkButton lbaddfile = (LinkButton)node.FindControl("lbAddFile");
            LinkButton lbdeletefolder = (LinkButton)node.FindControl("lbDeleteFolder");
            if (DesktopShared.Folder.CheckRole(DesktopShared.User.UserID, Convert.ToInt32(WebConfigurationManager.AppSettings["RoleFolderFileManager"])) == 0)
            {
                lbaddfolder.Visible = false;
                lbrenamefolder.Visible = false;
                lbaddfile.Visible = false;
                lbdeletefolder.Visible = false;
                lbNewRoot.Visible = false;
            }
        }
    }

    #region for folder

    private void funcdelete(int _pid, string _text)
    {
        if (DesktopShared.Folder.DeleteFolder(_pid) == true)
        {
            BindToDataSet();
            DisplayMessage(String.Format("Folder has been {0} - {1}", "Delete", DateTime.Now), Bootstrap.Alert.AlertType.Success);
            clear();
        }
        else
        {
            DisplayMessage(String.Format("Something went wrong, when deleted {0}", _text), Bootstrap.Alert.AlertType.Danger);
            return;
        }
    }

    private void StartNodeInEditMode(string nodeValue)
    {
        //find the node by its Value and edit it when page loads
        string js = "Sys.Application.add_load(editNode); function editNode(){ ";
        js += "var tree = $find(\"" + rtvFolder.ClientID + "\");";
        js += "var node = tree.findNodeByValue('" + nodeValue + "');";
        js += "if (node) node.startEdit();";
        js += "Sys.Application.remove_load(editNode);};";

        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "nodeEdit", js, true);
    }

    #endregion
    private void clear()
    {
        txtDocName.Text = string.Empty;
        txtFileLink.Text = string.Empty;
    }

    bool IsValidURL(string URL)
    {
        //string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
        Regex Rgx = new Regex(@"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)");
        return Rgx.IsMatch(URL);
    }
    /// <summary>
    /// Not used first
    /// </summary>
    /// <param name="treeNode"></param>
    /// <param name="searchString"></param>
    /// <param name="foundFirst"></param>

    #endregion

    #region protected method
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        pnlGrid.Visible = true;
        BindGrid();
        rgDocument.DataBind();
    }

    protected void lbAddFolder_Click(object sender, EventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string path = string.Empty;

        FolderName = txtFolderName.Text;
        if (DesktopShared.Folder.CheckFolderName(FolderParentId, FolderName) > 0)
        {
            DisplayMessage("Folder name already exists", Bootstrap.Alert.AlertType.Warning);
            return;
        }
        if (DesktopShared.Folder.SaveFolder(FolderParentId, _auditUserId, FolderName, FolderPath) == true)
        {
            BindToDataSet();
            DisplayMessage(String.Format("Folder has been {0} - {1}", "added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
            clear();
        }
        if (FolderParentId != null)
            expandnodebyValue(FolderParentId.ToString());
        else
            expandnodebytext(FolderName.ToString());
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearchDocName.Text = string.Empty;
        rgDocument.DataSource = null;
        rgDocument.DataBind();
        pnlGrid.Visible = false;
    }
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        int id;
        string path = string.Empty;

        if (IsValidURL(txtFileLink.Text) == false)
        {
            DisplayMessage("Invalid Url, add http:// or https://", Bootstrap.Alert.AlertType.Warning);
            return;
        }
        id = ParentId == null ? 0 : (int)ParentId;
        DocumentName = txtDocName.Text.Trim();
        if (DesktopShared.DocFolder.CheckDocumentName(id, DocumentName) > 0)
        {
            DisplayMessage("The document name already exists", Bootstrap.Alert.AlertType.Warning);
            return;
        }

        if (DesktopShared.DocFolder.SaveDocument(DocumentId.Value, id, DocumentName, txtFileLink.Text, _auditUserId, lblStructure.Text) == true)
        {
            string _status = string.Empty;

            if (DocumentId.Value == 0)
                _status = "added";
            else
                _status = "updated";
            BindToDataSet();
            DisplayMessage(String.Format("Document has been {0} - {1}", _status, DateTime.Now), Bootstrap.Alert.AlertType.Success);
            clear();

        }
        else
            DisplayMessage(String.Format("Something went wrong"), Bootstrap.Alert.AlertType.Danger);

        if (DocumentId.Value == 0)
            expandnodebyValue(ParentId.ToString().Trim());
        else
            expandnodebyValue((int.Parse(DocumentId.Value.ToString()) + DesktopShared.Folder.GetMaxIDFolder()).ToString());
    }

    protected void rgDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string str = e.CommandArgument.ToString().Trim();
        int _docid = 0;

        switch (str)
        {
            #region Modal Pop up
            case "Yes Delete Doc":
                if (DocumentId != null)
                    _docid = (int)DocumentId;

                if (DesktopShared.DocFolder.DeleteDocument(_docid) == true)
                {
                    DisplayMessage(String.Format("Document has been {0} - {1}", "Delete", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindToDataSet();
                    clear();
                    expandnodebyValue(ParentId.ToString());
                }
                else
                    DisplayMessage(String.Format("Something went wrong"), Bootstrap.Alert.AlertType.Danger);
                break;
            case "Yes Delete Folder":
                if (DesktopShared.Folder.CheckChildID(FolderID) == 0 && DesktopShared.Folder.Checkdocument(FolderID) == 0)
                    funcdelete(FolderID, FolderName);
                else
                    DesktopShared.Folder.DeleteFolderAndDocument(FolderID);

                DisplayMessage(String.Format("Folder has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                BindToDataSet();
                
                expandnodebyValue(ParentId.ToString());
                break;
            case "Rename Folder":
                if (DesktopShared.Folder.EditFolder(FolderID, _auditUserId, FolderName, FolderPath) == true)
                {
                    BindToDataSet();
                    DisplayMessage(String.Format("Folder has been {0} - {1}", "Update", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    clear();
                }
                else
                    DisplayMessage(String.Format("Something went wrong"), Bootstrap.Alert.AlertType.Danger);
                expandnodebyValue(FolderID.ToString());
                break;
            #endregion

            #region LinkButton
            case "RenameFolders":
                System.Collections.Generic.IList<RadTreeNode> renamefolders = rtvFolder.SelectedNodes;
                StartNodeInEditMode(renamefolders[0].Value);
                break;
            case "New Root":
                System.Collections.Generic.IList<RadTreeNode> selectNewRoot = rtvFolder.SelectedNodes;
                FolderParentId = null;
                if (true)
                {
                    lblModalTitleAddFolder.Text = "Add Root Folder";
                    txtFolderName.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddFolder", "$('#myModalAddFolder').modal('show');", true);
                }

                //int _count = 0;
                //System.Collections.Generic.IList<RadTreeNode> selectrootfolder = rtvFolder.GetAllNodes();
                //for (int i = 0; i <= selectrootfolder.Count - 1; i++)
                //{
                //    if (selectrootfolder[i].Level == 0)
                //    {
                //        if (selectrootfolder[i].Text.Contains("New Root Folder"))
                //        {
                //            ++_count;
                //        }
                //    }
                //}
                //FolderName = string.Format("New Root Folder {0}", _count + 1);
                //if (DesktopShared.Folder.SaveFolder(null, _auditUserId, FolderName, string.Empty) == true)
                //{
                //    BindToDataSet();

                //    RadTreeNode Newrootfolder = rtvFolder.FindNodeByText(FolderName);
                //    Newrootfolder.Selected = true;

                //    DisplayMessage(String.Format("Folder has been {0} - {1}", "added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                //    clear();
                //}
                break;
            case "AddFolder":
                System.Collections.Generic.IList<RadTreeNode> selectAddfolder = rtvFolder.SelectedNodes;
                FolderParentId = int.Parse(selectAddfolder[0].Value);
                if (selectAddfolder[0].ParentNode != null)
                    FolderPath = selectAddfolder[0].FullPath;
                else
                    FolderPath = selectAddfolder[0].Text;
                if (true)
                {
                    lblModalTitleAddFolder.Text = "Add Folder";
                    lblStructureAddFolder.Text = selectAddfolder[0].FullPath.Trim();
                    txtFolderName.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddFolder", "$('#myModalAddFolder').modal('show');", true);
                }
                expandnodebyValue(selectAddfolder[0].Value);
                break;
            case "AddFile":
                System.Collections.Generic.IList<RadTreeNode> selectfile = rtvFolder.SelectedNodes;
                string parentnode = selectfile[0].Text;
                ParentId = int.Parse(selectfile[0].Value);
                DocumentId = 0;
                if (true)
                {
                    lblModalTitle.Text = "Add Document";
                    lblStructure.Text = selectfile[0].FullPath.Trim();
                    txtFileLink.Text = string.Empty;
                    txtDocName.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#myModal').modal('show');", true);
                }
                expandnodebyValue(selectfile[0].Value);
                break;
            case "DeleteFile":
                System.Collections.Generic.IList<RadTreeNode> deletefile = rtvFolder.SelectedNodes;
                ParentId = int.Parse(deletefile[0].ParentNode.Value);
                DocumentId = int.Parse(deletefile[0].Value) - DesktopShared.Folder.GetMaxIDFolder();
                if (true)
                {
                    lblModalDeleteTitle.Text = "Delete Document";
                    btnYes.Visible = true;
                    btnYesDeleteFolder.Visible = false;
                    btnRename.Visible = false;
                    lblModalDeleteWording.Text = "Are you sure want to delete document " + deletefile[0].Text + " ?";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
            case "DeleteFolders":
                System.Collections.Generic.IList<RadTreeNode> deletefolder = rtvFolder.SelectedNodes;
                if (deletefolder[0].ParentNode != null)
                    ParentId = int.Parse(deletefolder[0].ParentNode.Value);
                else
                    ParentId = null;
                FolderID = int.Parse(deletefolder[0].Value);
                FolderName = deletefolder[0].Text;
                lblModalTitle.Text = "Delete Folder";
                btnYes.Visible = false;
                btnYesDeleteFolder.Visible = true;
                btnRename.Visible = false;
                if (DesktopShared.Folder.CheckChildID(FolderID) == 0 && DesktopShared.Folder.Checkdocument(FolderID) == 0)
                    lblModalDeleteWording.Text = "Are you sure want to delete this folder " + FolderName + " ?";
                else
                    lblModalDeleteWording.Text = "Are you sure you want to delete this " + FolderName + " also with branches and documents ?";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                break;
            case "RenameFile":
                System.Collections.Generic.IList<RadTreeNode> renamefile = rtvFolder.SelectedNodes;
                DocumentId = int.Parse(renamefile[0].Value) - DesktopShared.Folder.GetMaxIDFolder();
                if (true)
                {
                    txtFileLink.Text = renamefile[0].NavigateUrl.ToString().Trim();
                    txtDocName.Text = renamefile[0].Text.Trim();
                    lblModalTitle.Text = "Update Document";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#myModal').modal('show');", true);
                }
                break;
                #endregion
        }
    }

    protected void rtvFolder_NodeDataBound(object sender, RadTreeNodeEventArgs e)
    {
        e.Node.Target = "_blank";

        //foreach (RadTreeNode node in rtvFolder.GetAllNodes())
        //{
        //    if (node.NavigateUrl == string.Empty)
        //        node.ToolTip = "Folder : " + node.Text;
        //    else
        //        node.ToolTip = "Document : " + node.Text;
        //}
        if (DesktopShared.Folder.CheckRole(DesktopShared.User.UserID, Convert.ToInt32(WebConfigurationManager.AppSettings["RoleFolderFileManager"])) == 0)
        {
            rtvFolder.ContextMenus[0].FindItemByValue("Delete").Visible = false;
            rtvFolder.ContextMenus[0].FindItemByValue("Rename").Visible = false;

            rtvFolder.ContextMenus[1].FindItemByValue("AddNewFolder").Visible = false;
            rtvFolder.ContextMenus[1].FindItemByValue("RenameFolder").Visible = false;
            rtvFolder.ContextMenus[1].FindItemByValue("DeleteFolder").Visible = false;
            rtvFolder.ContextMenus[1].FindItemByValue("AddDoc").Visible = false;
        }
        if (e.Node.NavigateUrl == string.Empty)
        {
            e.Node.ContextMenuID = "MouseOverContextMenu";
            e.Node.AllowDrag = false;
        }
        else
            e.Node.AllowDrop = false;
    }

    protected void rtvFolder_ContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
    {
        RadTreeNode clickedNode = e.Node;
        bool showModal = true;
        int _auditUserId = DesktopShared.User.UserID;
        FolderName = string.Format("New Folder {0}", clickedNode.Nodes.Count + 1);
        switch (e.MenuItem.Value)
        {
            case "AddNewFolder":
                FolderParentId = int.Parse(e.Node.Value);
                if (e.Node.ParentNode != null)
                    FolderPath = e.Node.FullPath;
                else
                    FolderPath = e.Node.Text;
                if (true)
                {
                    lblModalTitleAddFolder.Text = "Add Folder";
                    lblStructureAddFolder.Text = e.Node.FullPath.Trim();
                    txtFolderName.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddFolder", "$('#myModalAddFolder').modal('show');", true);
                }
                //string _path = string.Empty;

                //if (clickedNode.ParentNode != null)
                //    _path = clickedNode.FullPath;
                //else
                //    _path = clickedNode.Text;
                //if (SaveFolder(int.Parse(e.Node.Value), _auditUserId, FolderName, _path) == true)
                //{
                //    BindToDataSet();
                //    DisplayMessage(String.Format("Folder has been {0} - {1}", "added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                //    clear();
                //}
                expandnodebyValue(e.Node.Value);
                break;
            case "AddDoc":
                ParentId = int.Parse(e.Node.Value);
                DocumentId = 0;
                if (showModal)
                {
                    lblModalTitle.Text = "Add Document";
                    lblStructure.Text = e.Node.FullPath.Trim();
                    txtFileLink.Text = string.Empty;
                    txtDocName.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#myModal').modal('show');", true);
                }
                expandnodebyValue(e.Node.Value);
                break;
            case "Rename":
                DocumentId = int.Parse(e.Node.Value) - DesktopShared.Folder.GetMaxIDFolder();
                if (showModal)
                {
                    txtFileLink.Text = e.Node.NavigateUrl.ToString().Trim();
                    txtDocName.Text = e.Node.Text.Trim();
                    lblModalTitle.Text = "Update Document";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#myModal').modal('show');", true);
                }
                expandnodebyValue(DocumentId.ToString());
                break;
            case "RenameFolder":
                StartNodeInEditMode(clickedNode.Value);
                break;
            case "Delete":
                DocumentId = int.Parse(e.Node.Value) - DesktopShared.Folder.GetMaxIDFolder();
                if (showModal)
                {
                    lblModalDeleteTitle.Text = "Delete Document";
                    btnYes.Visible = true;
                    btnYesDeleteFolder.Visible = false;
                    btnRename.Visible = false;
                    lblModalDeleteWording.Text = "Are you sure want to delete document " + e.Node.Text + " ?";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
            case "DeleteFolder":
                FolderID = int.Parse(e.Node.Value);
                FolderName = e.Node.Text;
                lblModalTitle.Text = "Delete Folder";
                btnYes.Visible = false;
                btnYesDeleteFolder.Visible = true;
                btnRename.Visible = false;
                if (DesktopShared.Folder.CheckChildID(FolderID) == 0 && DesktopShared.Folder.Checkdocument(FolderID) == 0)
                    lblModalDeleteWording.Text = "Are you sure want to delete this folder " + FolderName + " ?";
                else
                    lblModalDeleteWording.Text = "Are you sure you want to delete this " + FolderName + " also with branches and documents ?";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                break;
        }
    }

    protected void rgDocument_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string _pid = string.Empty;
        try
        {
            rtvFolder.CollapseAllNodes();
            foreach (GridDataItem item in rgDocument.MasterTableView.Items)
            {
                if (e.CommandName == "GotoRoot")
                {
                    if (item.GetDataKeyValue("stype").ToString() == "Folder")
                        _pid = e.CommandArgument.ToString().Trim();
                    else
                        _pid = item.GetDataKeyValue("fexist").ToString();
                    RadTreeNode node = rtvFolder.FindNodeByValue(_pid.ToString());
                    node.ExpandParentNodes();
                    node.ExpandChildNodes();
                    node.Selected = true;
                    node.Focus();
                    return;
                }
            }
            if (e.CommandName != "Page")
            {
                //txtSearchDocName.Text = e.CommandName;
                //ddlSearchBy.SelectedValue = "path";
                rgDocument.DataSource = DesktopShared.DocFolder.getsearchfolderandfile(e.CommandName, "path");
                rgDocument.Rebind();
            }
        }
        catch
        {
        }
    }

    protected void rtvFolder_NodeDrop(object sender, RadTreeNodeDragDropEventArgs e)
    {
        int nodeid = 0;

        if (int.Parse(e.DraggedNodes[0].Value) - DesktopShared.Folder.GetMaxIDFolder() > 0)
        {
            nodeid = int.Parse(e.DraggedNodes[0].Value) - DesktopShared.Folder.GetMaxIDFolder();
            DesktopShared.DocFolder.moveDoc(nodeid, int.Parse(e.DestDragNode.Value));
        }
        else
        {
            DesktopShared.Folder.movefolder(int.Parse(e.DraggedNodes[0].Value), int.Parse(e.DestDragNode.Value));
        }
        BindToDataSet();
        expandnodebyValue(e.DraggedNodes[0].Value);
        //RadTreeNode node = rtvFolder.FindNodeByValue(e.DraggedNodes[0].Value);
        //node.ExpandParentNodes();
        //node.ExpandChildNodes();
        //node.Selected = true;
    }

    protected void rtvFolder_NodeEdit(object sender, RadTreeNodeEditEventArgs e)
    {
        int _userid = DesktopShared.User.UserID;
        FolderID = int.Parse(e.Node.Value);
        FolderName = e.Text.Trim();
        bool showModal = true;
        if (e.Node.ParentNode != null)
        {
            FolderParentId = int.Parse(e.Node.ParentNode.Value);
            FolderPath = e.Node.ParentNode.FullPath;
        }
        else
            FolderParentId = null;

        if (FolderParentId != null)
        {
            if (DesktopShared.Folder.CheckFolderName(FolderParentId, FolderName) > 0)
            {
                DisplayMessage(String.Format("Foldername has been exist"), Bootstrap.Alert.AlertType.Warning);
                return;
            }
        }
        else
        {
            if (DesktopShared.Folder.CheckParentFolderName(FolderName) > 0)
            {
                DisplayMessage(String.Format("Foldername has been exist"), Bootstrap.Alert.AlertType.Warning);
                return;
            }
        }

        if (showModal)
        {
            lblModalDeleteTitle.Text = "Rename Folder";
            lblModalDeleteWording.Text = "Rename " + e.Node.Text + " to " + e.Text + " ?";
            btnYes.Visible = false;
            btnYesDeleteFolder.Visible = false;
            btnRename.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
        }
    }

    #endregion

    #region private properties
    /// <summary>
    /// get/set document id
    /// </summary>
    private int? DocumentId
    {
        get
        {
            object obj = this.ViewState["DocID"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["DocID"] = value; }
    }

    /// <summary>
    /// get/set document id
    /// </summary>
    private int? ParentId
    {
        get
        {
            object obj = this.ViewState["ParentId"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["ParentId"] = value; }
    }

    private string DocumentName
    {
        get
        {
            object obj = this.ViewState["DocName"];
            return (obj == null) ? string.Empty : (string)obj;
        }
        set { this.ViewState["DocName"] = value; }
    }

    private string ParentDocumentNameBefore
    {
        get
        {
            object obj = this.ViewState["ParentDocNameBefore"];
            return (obj == null) ? string.Empty : (string)obj;
        }
        set { this.ViewState["ParentDocNameBefore"] = value; }
    }

    #region folder
    private int FolderID
    {
        get
        {
            object obj = this.ViewState["FolderID"];
            return (obj == null) ? 0 : (int)obj;
        }
        set { this.ViewState["FolderID"] = value; }
    }

    private string FolderName
    {
        get
        {
            object obj = this.ViewState["FolderName"];
            return (obj == null) ? string.Empty : (string)obj;
        }
        set { this.ViewState["FolderName"] = value; }
    }

    private string FolderPath
    {
        get
        {
            object obj = this.ViewState["FolderPath"];
            return (obj == null) ? string.Empty : (string)obj;
        }
        set { this.ViewState["FolderPath"] = value; }
    }

    private int? FolderParentId
    {
        get
        {
            object obj = this.ViewState["FolderParentId"];
            return (obj == null) ? null : (int?)obj;
        }
        set { this.ViewState["FolderParentId"] = value; }
    }
    #endregion

    /// <summary>
    /// get/set sort column name
    /// </summary>
    private string SortColumnName
    {
        get
        {
            object obj = this.ViewState["scn_rtg"];
            return (obj == null) ? "Id" : (string)obj;
        }
        set { this.ViewState["scn_rtg"] = value; }
    }
    /// <summary>
    /// get/set sort operator
    /// </summary>
    private SD.LLBLGen.Pro.ORMSupportClasses.SortOperator SortOperator
    {
        get
        {
            object obj = this.ViewState["so_rtg"];
            return (obj == null) ? SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Descending : (SD.LLBLGen.Pro.ORMSupportClasses.SortOperator)obj;
        }
        set { this.ViewState["so_rtg"] = value; }
    }
    #endregion

    protected void rgDocument_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        GridSortExpression sortExpression = new GridSortExpression();
        //these columns will sort descending on first click
        var _nonAscList = new List<string> { "Id" };
        bool _sortAscending = !_nonAscList.Contains(e.SortExpression.Trim());

        SortColumnName = e.SortExpression.Trim();
        switch (e.OldSortOrder)
        {
            case GridSortOrder.None:
                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.Ascending : GridSortOrder.Descending;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
                break;
            case GridSortOrder.Ascending:

                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.Descending : GridSortOrder.None;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
                break;
            case GridSortOrder.Descending:

                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.None : GridSortOrder.Ascending;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
                break;
        }
        SortOperator = (sortExpression.SortOrder == GridSortOrder.Ascending) ? SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending : SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Descending;

        e.Canceled = true;
        rgDocument.CurrentPageIndex = 0;
        rgDocument.Rebind();

        if (!e.Item.OwnerTableView.SortExpressions.ContainsExpression(e.SortExpression))
        {
            sortExpression = new GridSortExpression();
            sortExpression.FieldName = e.SortExpression;
            sortExpression.SortOrder = GridSortOrder.Ascending;
            e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
        }
    }
}