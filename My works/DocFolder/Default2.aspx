<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="DocFolder_Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script type="text/javascript">  
        function afterClientCheck(tree, eventArgs) {
            var node = eventArgs.get_node();
            if (node.get_checked()) {
                for (var i = 0; i < tree.get_allNodes().length; i++) {
                    if (tree.get_allNodes()[i] != node)
                        tree.get_allNodes()[i].set_checked(false);
                }
            }
        }
        function treeExpandAllNodes() {
            var treeView = $find("<%= rtvFolder.ClientID %>");
            var nodes = treeView.get_allNodes();
            for (var i = 0; i < nodes.length; i++) {

                if (nodes[i].get_nodes() != null) {
                    nodes[i].expand();
                }
            }
            }
        function treeCollapseAllNodes() {
            var treeView = $find("<%= rtvFolder.ClientID %>");
            var nodes = treeView.get_allNodes();
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].get_nodes() != null) {
                    nodes[i].collapse();
                }
            }
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
    
    <div class="content-header">
        <div class="header-section">
            <h1>Manage File<asp:Literal ID="litHeaderName" runat="server" /></h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" Visible="false" />
    <asp:ValidationSummary ID="vsProfile" runat="server" ValidationGroup="vgDoc" CssClass="validationSummary" />
    <div class="block">
        <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSearch">
            <ajaxToolkit:CollapsiblePanelExtender ID="cpeTicketSearchCriteria" 
                    runat="Server"
                    TargetControlID="pnlSearchCriteria"
                    ExpandControlID="pnlShowHideTicketSearchCriteria"
                    CollapseControlID="pnlShowHideTicketSearchCriteria" 
                    BehaviorID="cpeTicketSearchCriteria"
                    TextLabelID="lblTicketSearchCriteriaHeader"
                    ImageControlID="imgTicketSearchCriteriaHeader"    
                    ExpandedImage="~/Images/collapse_blue.jpg"
                    CollapsedImage="~/Images/expand_blue.jpg"
                    SuppressPostBack="true"
                    CollapsedText="Search [click to display]"
                    ExpandedText="Search [click to hide]" />
            <div class="block-title">
                <asp:Panel ID="pnlShowHideTicketSearchCriteria" runat="server" CssClass="collapsePanelHeaderTicket" Height="35px">
                    <div style="float: left;">
                        <h2><strong>
                            <asp:Label ID="lblTicketSearchCriteriaHeader" runat="server" />
                            </strong>
                        </h2>                            
                    </div>
                    <div style="float: right; vertical-align: middle; margin:10px 10px 0px 0px">
                        <asp:ImageButton ID="imgSearchCriteriaTicketHeader" runat="server" ImageUrl="~/Images/expand_blue.jpg" />
                    </div>
                </asp:Panel>
            </div>
            <asp:Panel ID="pnlSearchCriteria" style="overflow:hidden" runat="server">
            <div class="block-title">
                <h2 style="display: none;"><strong>Search Criteria</strong></h2>
                <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="validationSummary" ValidationGroup="vgSearch" />
            </div>
            <div class="row">
                <div class="form-group col-sm-3">
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="txtSearchDocName" Text="Folder/File Name" />
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-3">
                    <asp:TextBox ID="txtSearchDocName" runat="server" MaxLength="50" CssClass="form-control" />
                   
                </div>
                <div class="form-group col-sm-3">
                     <asp:DropDownList ID="ddlSearchBy" runat="server" CssClass="form-control">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="File" Value="file" />
                        <asp:ListItem Text="Folder" Value="folder" />
                    </asp:DropDownList>
                </div>
                <div class="form-group col-sm-6">
                    <asp:LinkButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-sm btn-primary" ValidationGroup="vgSearch"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="btnClear" runat="server" OnClick="btnClear_Click" CssClass="btn btn-sm btn-warning" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="block">
                        <div class="block-title">
                            <h2><strong>Search Result</strong></h2>
                        </div>
                        <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                            <telerik:RadListView ID="lvDocument" runat="server" />
                            <telerik:RadGrid ID="rgDocument" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                                AllowPaging="true" ShowFooter="false" PageSize="5" Width="100%" OnSortCommand="rgDocument_SortCommand"
                                OnItemCommand="rgDocument_ItemCommand" OnNeedDataSource="rgDocument_NeedDataSource" >
                                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="pid,stype,fexist,PathLoc" Width="100%" AllowSorting="true">
                                    <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                                    <NoRecordsTemplate>
                                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                    </NoRecordsTemplate>
                                    <Columns>
                                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%" UniqueName="Name">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgIconFolder" ImageUrl="~/Images/folder.png" Visible='<%# Eval("stype").ToString()=="Folder" ? true:false %>' CommandName="GotoRoot" CommandArgument='<%# Eval("pid").ToString() %>' runat="server" />
                                                <asp:HyperLink ID="imgIconFile" Visible='<%# Eval("stype").ToString()=="Folder" ? false:true %>' runat="server" ImageUrl="~/Images/document.png"  Target="_blank" NavigateUrl='<%# Eval("surl") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="File/Folder" HeaderStyle-Width="30%" UniqueName="sname" SortExpression="sname">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlName" Visible='<%# Eval("stype").ToString()=="Folder" ? false:true %>' runat="server" Text='<%# Eval("sname") %>' Target="_blank" NavigateUrl='<%# Eval("surl") %>' />
                                                <asp:LinkButton ID="lbRoot" CommandName="GotoRoot" CommandArgument='<%# Eval("pid").ToString() %>' runat="server" Visible='<%# Eval("stype").ToString()=="Folder" ? true:false %>' Text='<%# Eval("sname") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="Location" HeaderStyle-Width="30%" UniqueName="PathLoc" SortExpression="PathLoc">
                                            <ItemTemplate>
                                                <%--<asp:LinkButton ID="lbPath" CommandName='<%# Eval("PathLoc") %>' CommandArgument='<%# Eval("pid").ToString() %>' runat="server" Text='<%# Eval("PathLoc") %>' />--%>
                                                <asp:LinkButton ID="lbPath" CommandName="GotoRoot" CommandArgument='<%# Eval("pid").ToString() %>' runat="server" Text='<%# Eval("PathLoc") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <%--<telerik:GridBoundColumn DataField="PathLoc" HeaderStyle-Width="65%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Path" SortExpression="PathLoc" UniqueName="PathLoc" />--%>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" />
                            </telerik:RadGrid>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </asp:Panel>
        </asp:Panel>
    </div>
   
    <div class="row">
        <div class="col-md-12">
            <div class="block">
                <div class="block-title">
                    <h2><strong>File Information</strong></h2>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="col-md-4">
                                <asp:LinkButton ID="lbNewRoot" OnCommand="Decision_Command" CommandArgument="New Root"  runat="server">New Root Folder</asp:LinkButton>
                            </div>
                            <div class="col-md-4">
                                <a href="javascript:treeExpandAllNodes();">Expand All</a>
                            </div>
                            <div class="col-md-4">
                                <a href="javascript:treeCollapseAllNodes();">Collapse All</a>
                            </div>
                        </div>
                    </div>
                </div><br />
                <div class="form-horizontal form-bordered">
                    <div class="form-group">
                        <telerik:RadTreeView RenderMode="Lightweight" ID="rtvFolder" runat="server"  Width="100%" Height="100%" 
                            OnNodeDataBound="rtvFolder_NodeDataBound" OnContextMenuItemClick="rtvFolder_ContextMenuItemClick" OnNodeEdit="rtvFolder_NodeEdit" 
                                OnClientNodeChecked="afterClientCheck" EnableDragAndDrop="true" OnNodeDrop="rtvFolder_NodeDrop">
                            <NodeTemplate>
                                <asp:Image ID="imgIcon" ImageUrl='<%# Eval("stype").ToString()=="Folder" ? "~/Images/folder.png":"~/Images/document.png" %>' runat="server" />
                                <asp:HyperLink ID="hlName" Visible='<%# Eval("stype").ToString()=="Folder" ? false:true %>' runat="server" Text='<%# Eval("FolderName") %>' Target="_blank" NavigateUrl='<%# Eval("documenturl") %>' />

                                <asp:Label ID="lblName" Visible='<%# Eval("stype").ToString()=="Folder" ? true:false %>' runat="server" Text='<%# Eval("FolderName") %>' /> &nbsp;
                                        
                                <asp:LinkButton ID="lbAddFolder" OnCommand="Decision_Command" CommandArgument="AddFolder" runat="server" Visible='<%# Eval("stype").ToString()=="Folder" ? true:false %>'><asp:Image  ID="imgAddfolder" runat="server" ImageUrl="~/Images/newfolder.png" /></asp:LinkButton> &nbsp;

                                <asp:LinkButton ID="lbRenameFolder" OnCommand="Decision_Command" CommandArgument='<%# Eval("stype").ToString()=="Folder" ? "RenameFolders":"RenameFile" %>' runat="server" >
                                    <asp:Image  ID="imgRanameFolder" runat="server" ImageUrl="~/Images/rename.png" />
                                </asp:LinkButton> &nbsp;

                                <asp:LinkButton ID="lbAddFile" OnCommand="Decision_Command" CommandArgument="AddFile" runat="server" Visible='<%# Eval("stype").ToString()=="Folder" ? true:false %>'><asp:Image  ID="imgAddFile" runat="server" ImageUrl="~/Images/newdocument.png" /></asp:LinkButton>&nbsp;

                                <asp:LinkButton ID="lbDeleteFolder" OnCommand="Decision_Command" CommandArgument='<%# Eval("stype").ToString()=="Folder" ? "DeleteFolders":"DeleteFile" %>' runat="server">
                                    <asp:Image  ID="imgDeleteFolder" runat="server" ImageUrl="~/Images/delete.png" />
                                </asp:LinkButton>
                                
                                <%--<asp:LinkButton ID="lbRenameFile" OnCommand="Decision_Command" CommandArgument="RenameFile" runat="server" Visible='<%# Eval("stype").ToString()=="Folder" ? false:true %>'><asp:Image  ID="imgRenameFile" runat="server" ImageUrl="~/Images/rename.png" /></asp:LinkButton>--%>
                                <%--<asp:LinkButton ID="lbDeleteFile" OnCommand="Decision_Command" CommandArgument="DeleteFile" runat="server" Visible='<%# Eval("stype").ToString()=="Folder" ? false:true %>'><asp:Image  ID="imgDeleteFile" runat="server" ImageUrl="~/Images/delete.png" /></asp:LinkButton>--%>
                            </NodeTemplate>
                            <ContextMenus>
                                <telerik:RadTreeViewContextMenu ID="MainContextMenu" runat="server" RenderMode="Lightweight" EnableScreenBoundaryDetection="false" >
                                    <Items>
                                        <telerik:RadMenuItem Value="Rename" Text="Edit Document" ImageUrl="../Images/rename.png">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="Delete" Text="Delete Document" ImageUrl="../Images/delete.png">
                                        </telerik:RadMenuItem>
                                    </Items>
                                </telerik:RadTreeViewContextMenu>
                                <telerik:RadTreeViewContextMenu ID="MouseOverContextMenu" runat="server" RenderMode="Native" Skin="Vista" EnableScreenBoundaryDetection="false">
                                    <Items>
                                        <telerik:RadMenuItem Value="AddNewFolder" Text="Add Sub Folder" ImageUrl="../Images/newfolder.png" />
                                        <telerik:RadMenuItem Value="RenameFolder" Text="Edit" ImageUrl="../Images/rename.png" />
                                        <telerik:RadMenuItem Value="DeleteFolder" Text="Delete" ImageUrl="../Images/delete.png" />
                                        <telerik:RadMenuItem Value="AddDoc" Text="Add File" ImageUrl="../Images/newdocument.png" />
                                    </Items>
                                </telerik:RadTreeViewContextMenu>
<%--                                <telerik:RadTreeViewContextMenu ID="FolderContextMenu" runat="server" RenderMode="Lightweight">
                                    <Items>
                                        <telerik:RadMenuItem Value="AddNewFolder" Text="New Sub Folder" ImageUrl="../Images/newfolder.png">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="AddDoc" Text="New Document" ImageUrl="../Images/newdocument.png">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="RenameFolder" Text="Edit Folder" ImageUrl="../Images/rename.png">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="DeleteFolder" Text="Delete Folder" ImageUrl="../Images/delete.png">
                                        </telerik:RadMenuItem>
                                    </Items>
                                </telerik:RadTreeViewContextMenu>--%>
                            </ContextMenus>
			            </telerik:RadTreeView>
                    </div>
                </div><br />
                <%--<div class="row">
                    <div class="col-md-12">
                        <span style="font-size:smaller">
                        * You can only move documents by dragging and dropping. If you want to move the folder to the following <a href="../File/Default.aspx" target="_blank">url</a></span>
                    </div>        
                </div>--%>      
            </div>
        </div>
    </div>

    <div id="myModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel"><asp:Label ID="lblModalTitle" runat="server" /></h4>
                    <h5><asp:Label ID="lblStructure" runat="server" /></h5> 
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group">
                            <label class="col-md-12 control-label">File Name<span class="text-danger">*</span></label>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:TextBox runat="server" ID="txtDocName" MaxLength="200" CssClass="form-control" Placeholder="Enter File Name ..." />
                                <asp:RequiredFieldValidator ID="rfvDocName" runat="server" ControlToValidate="txtDocName" ForeColor="Red" ErrorMessage="File Name is required" ValidationGroup="vgDoc" Display="None">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <label class="col-md-12 control-label">File Link<span class="text-danger">*</span></label>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:TextBox runat="server" ID="txtFileLink" MaxLength="200" CssClass="form-control" Placeholder="Enter File Link including http:// or https://" />
                                <asp:RequiredFieldValidator ID="rfvFileLink" runat="server" ControlToValidate="txtFileLink" ForeColor="Red" ErrorMessage="File link is required" ValidationGroup="vgDoc" Display="None">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vgDoc" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div id="myModalAddFolder" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabelAddFolder"><asp:Label ID="lblModalTitleAddFolder" runat="server" /></h4>
                    <h5><asp:Label ID="lblStructureAddFolder" runat="server" /></h5> 
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group">
                            <label class="col-md-12 control-label">Folder Name<span class="text-danger">*</span></label>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:TextBox runat="server" ID="txtFolderName" MaxLength="50" CssClass="form-control" Placeholder="Enter Folder Name ..." />
                                <asp:RequiredFieldValidator ID="rfvFolderName" runat="server" ControlToValidate="txtFolderName" ForeColor="Red" ErrorMessage="Folder Name is required" ValidationGroup="vgAddFolder" Display="None">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbAddFolder" OnClick="lbAddFolder_Click" runat="server" ValidationGroup="vgAddFolder" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div id="myModalDelete" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myModalDeleteLabel"><asp:Label ID="lblModalDeleteTitle" runat="server" /></h4>
                    </div>
                    <div class="modal-body">
                        <h3><asp:Label ID="lblModalDeleteWording" runat="server" /></h3>
                        
                        <asp:Button ID="btnNo" runat="server" Text="Cancel" OnCommand="Decision_Command" Visible="false" CommandArgument="No Delete" />
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnYes" runat="server" Text="Ok" class="btn btn-primary" OnCommand="Decision_Command" CommandArgument="Yes Delete Doc" />
                        <asp:Button ID="btnYesDeleteFolder" runat="server" Text="Ok" class="btn btn-primary" OnCommand="Decision_Command" CommandArgument="Yes Delete Folder" />
                        <asp:Button ID="btnRename" runat="server" Text="Submit" class="btn btn-primary" OnCommand="Decision_Command" CommandArgument="Rename Folder" />
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>
        </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" Runat="Server">
    
</asp:Content>

