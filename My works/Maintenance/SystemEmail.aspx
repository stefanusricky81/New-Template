<%@ Page Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="SystemEmail.aspx.cs" Inherits="Maintenance_SystemEmail" Title="Bit By Bit Intranet - System Email Maintenance" %>
<%@ Reference VirtualPath="~/UserControl/Grid/EditForm/SystemEmailEdit.ascx" %>
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
        <h1>System Email Maintenance</h1>
    </div>
</div>
<telerik:RadAjaxPanel ID="rapSystemEmail" runat="server" LoadingPanelID="ralpSystemEmail">
    <asp:Literal ID="litMessage" runat="server" Visible="false" /> 
    <div class="block" style="padding-bottom:20px;">
        <div class="table-responsive">
            <telerik:RadGrid ID="rgSystemEmail" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                OnNeedDataSource="rgSystemEmail_NeedDataSource"
                OnUpdateCommand="rgSystemEmail_UpdateCommand"
                OnItemDataBound="rgSystemEmail_ItemDataBound" 
                OnPreRender="rgSystemEmail_PreRender" 
                AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="true" ShowFooter="false" PageSize="25" Width="100%" >
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" CommandItemDisplay="None">
                    <EditFormSettings UserControlName="/UserControl/Grid/EditForm/SystemEmailEdit.ascx" EditFormType="WebUserControl" />
                    <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                         <telerik:GridTemplateColumn DataField="Id" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%" HeaderText="" UniqueName="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEditSystemEmail" runat="server" CommandName="Edit"><i class="gi gi-edit" title="Edit"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="Name" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="15%" HeaderText="Name" SortExpression="Name" UniqueName="Name" />
                        <telerik:GridBoundColumn DataField="Subject" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="15%" HeaderText="Subject" SortExpression="Subject" UniqueName="Subject" />
                        <telerik:GridBoundColumn DataField="From" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="15%" HeaderText="From" SortExpression="From" UniqueName="From" />
                        <telerik:GridBoundColumn DataField="Body" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="25%" HeaderText="Body" SortExpression="Body" UniqueName="Body" />
                        <telerik:GridBoundColumn DataField="Description" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="25%" HeaderText="Description" SortExpression="Description" UniqueName="Description" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</telerik:RadAjaxPanel>
    
<telerik:RadAjaxLoadingPanel ID="ralpSystemEmail" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>
</asp:Content>

