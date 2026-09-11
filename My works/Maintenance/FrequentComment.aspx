<%@ Page Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="FrequentComment.aspx.cs" Inherits="Maintenance_FrequentComment" Title="Bit By Bit Intranet - Frequent Comments Maintenance" %>
<%@ Reference VirtualPath="~/UserControl/Grid/EditForm/FrequentCommentValueEdit.ascx" %>
<%@ Register TagPrefix="ddl" TagName="FrequentCommentType" Src="~/UserControl/DropDownList/FrequentCommentType.ascx" %>
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
        <h1>Frequent Comments Maintenance</h1>
    </div>
</div>
<telerik:RadAjaxPanel ID="rapFrequentCommentValue" runat="server" LoadingPanelID="ralpFrequentCommentValue">
    <asp:Literal ID="litMessage" runat="server" Visible="false" /> 
    <div class="block">
        <div class="form-horizontal">
            <div class="form-group">
                <label class="col-sm-2 control-label">Type</label>
                <div class="form-group col-sm-4">
                    <ddl:FrequentCommentType ID="ddlFrequentCommentType" runat="server" CssClass="form-control" ShowDefaultEntry="true" DefaultValue="" DefaultText="All" IsRequired="false" />
                </div>
            </div>
        </div>
    </div>
    <div class="block" style="padding-bottom:20px;">
        <div class="table-responsive">
            <telerik:RadGrid ID="rgFrequentCommentValue" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                OnNeedDataSource="rgFrequentCommentValue_NeedDataSource"
                OnInsertCommand="rgFrequentCommentValue_InsertCommand"
                OnDeleteCommand="rgFrequentCommentValue_DeleteCommand"  
                OnUpdateCommand="rgFrequentCommentValue_UpdateCommand"
                OnItemDataBound="rgFrequentCommentValue_ItemDataBound" 
                OnPreRender="rgFrequentCommentValue_PreRender" 
                OnItemCreated="rgFrequentCommentValue_ItemCreated"
                AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="true" ShowFooter="false" PageSize="25" Width="100%" >
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" CommandItemDisplay="Top">
                    <EditFormSettings UserControlName="/UserControl/Grid/EditForm/FrequentCommentValueEdit.ascx" EditFormType="WebUserControl" />
                    <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                     <CommandItemTemplate>
                        <div style="margin:2px 5px;">
                            <asp:LinkButton ID="btnAddNewRecord" runat="server" CommandName="InitInsert"><i class="gi gi-circle_plus"></i></i>  Add New Comment</asp:LinkButton>
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                         <telerik:GridTemplateColumn DataField="Id" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%" HeaderText="" UniqueName="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEditFrequentCommentValue" runat="server" CommandName="Edit" style="padding-right:10px;"><i class="gi gi-edit" title="Edit"></i></asp:LinkButton>
                                <asp:LinkButton ID="btnDeleteFrequentCommentValue" runat="server" CommandName="Delete" OnClientClick="javascript:if(!confirm('Are you sure you want to Permanently Delete this Comment')){return false;}">
                                    <i class="gi gi-circle_remove" title="Delete Document Type"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="Text" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60%" HeaderText="Text" SortExpression="Text" UniqueName="Text" />
                        <telerik:GridBoundColumn DataField="FrequentCommentTypeName" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="25%" HeaderText="Type" SortExpression="FrequentCommentTypeName" UniqueName="FrequentCommentTypeName" />
                        <telerik:GridBoundColumn DataField="Active" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="7%" HeaderText="Active" SortExpression="Active" UniqueName="Active" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</telerik:RadAjaxPanel>
    
<telerik:RadAjaxLoadingPanel ID="ralpFrequentCommentValue" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>
</asp:Content>

