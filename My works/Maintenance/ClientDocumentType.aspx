<%@ Page Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientDocumentType.aspx.cs" Inherits="Maintenance_ClientDocumentType" Title="Bit By Bit Intranet - Client Document Types Maintenance" %>
<%@ Reference VirtualPath="~/UserControl/Grid/EditForm/ClientDocumentTypeEdit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style>
        .rgEditForm div:last-of-type {padding-left:1px!important; } 
         .RadGrid_3b .rgRow td,.RadGrid_3b .rgAltRow td,.RadGrid_3b .rgEditRow td,.RadGrid_3b .rgFooter td,.RadGrid_3b .rgFilterRow td,.RadGrid_3b .rgResizeCol,.RadGrid_3b .rgGroupHeader td
        {
	        padding:4px 5px !important;
	        white-space: nowrap;
            overflow:hidden;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMainContent" Runat="Server">
<div class="content-header">
    <div class="header-section">
        <h1>Client Document Types Maintenance</h1>
    </div>
</div>
<telerik:RadAjaxPanel ID="rapClientDocumentType" runat="server" LoadingPanelID="ralpClientDocumentType">
    <asp:Literal ID="litMessage" runat="server" Visible="false" /> 
    <div class="block" style="padding-bottom:20px;">
        <div class="table-responsive">
            <telerik:RadGrid ID="rgClientDocumentType" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                OnNeedDataSource="rgClientDocumentType_NeedDataSource"
                OnInsertCommand="rgClientDocumentType_InsertCommand"
                OnDeleteCommand="rgClientDocumentType_DeleteCommand"  
                OnUpdateCommand="rgClientDocumentType_UpdateCommand"
                OnItemDataBound="rgClientDocumentType_ItemDataBound" 
                OnPreRender="rgClientDocumentType_PreRender" 
                AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="true" ShowFooter="false" PageSize="25" Width="100%" >
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" CommandItemDisplay="Top">
                    <EditFormSettings UserControlName="/UserControl/Grid/EditForm/ClientDocumentTypeEdit.ascx" EditFormType="WebUserControl" />
                    <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                     <CommandItemTemplate>
                        <div style="margin:2px 5px;">
                            <asp:LinkButton ID="btnAddNewRecord" runat="server" CommandName="InitInsert"><i class="gi gi-circle_plus"></i></i> Add New Document Type</asp:LinkButton>
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                         <telerik:GridTemplateColumn DataField="Id" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%" HeaderText="" UniqueName="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEditClientDocumentType" runat="server" CommandName="Edit" style="padding-right:10px;"><i class="gi gi-edit" title="Edit"></i></asp:LinkButton>
                                <asp:LinkButton ID="btnDeleteClientDocumentType" runat="server" CommandName="Delete" OnClientClick="javascript:if(!confirm('Are you sure you want to Permanently Delete this Client Document Type')){return false;}">
                                    <i class="gi gi-circle_remove" title="Delete Document Type"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="Name" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="22%" HeaderText="Name" SortExpression="Name" UniqueName="Name" />
                        <telerik:GridBoundColumn DataField="Description" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60%" HeaderText="Description" SortExpression="Description" UniqueName="Description" />
                        <telerik:GridBoundColumn DataField="Active" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" HeaderText="Active" SortExpression="Active" UniqueName="Active" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</telerik:RadAjaxPanel>
    
<telerik:RadAjaxLoadingPanel ID="ralpClientDocumentType" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>
</asp:Content>

