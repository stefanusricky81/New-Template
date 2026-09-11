<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientAddProjectTickets.aspx.cs" Inherits="Client_ClientAddProjectTickets" %>
<%@ Register TagPrefix="ctrl" TagName="ClientDetailTabs" Src="~/UserControl/Client/ClientDetailTabs.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Employee" Src="~/UserControl/DropDownList/Employee.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style>
        .RadGrid_3b .rgRow td,.RadGrid_3b .rgAltRow td,.RadGrid_3b .rgEditRow td,.RadGrid_3b .rgFooter td,.RadGrid_3b .rgFilterRow td,.RadGrid_3b .rgResizeCol,.RadGrid_3b .rgGroupHeader td
        {
	        padding:4px 5px !important;
	        white-space: nowrap;
            overflow:hidden;
        }
        .RadGrid td.rgPagerCell { padding: 10px; }
        .RadGrid_3b .rgPagerCell .rgPagerLabel { padding: 7px 0 7px;}
        .RadDropDownList .rddlInner {height:25px; margin-top:6px;}
        .RadGrid_3b .RadComboBox .rcbInput {width:50px !important;}
        /*selected*/
        .RadGrid_3b .rgMasterTable .rgSelectedCell,
        .RadGrid_3b .rgSelectedRow > td,
        .RadGrid_3b td.rgEditRow .rgSelectedRow,
        .RadGrid_3b .rgSelectedRow td.rgSorted {
            color: #ffffff;
            background: #07313a;
            border-color: #ffffff;}
        .RadGrid_3b .rgSelectedRow.rgHoveredRow {
            background: #07313a; }
        .RadGrid_3b .rgSelectedCell a,
        .RadGrid_3b .rgSelectedRow a {
            color: #ffffff; }
    </style>
    <asp:PlaceHolder ID="phTabletPagerCss" runat="server" Visible="false">
        <style>
            .RadGrid_3b .rgPagerCell .rgNumPart span { width:25px; }
            .RadGrid_3b .rgPagerCell .rgPagePrev, .rgPageFirst, .rgPageNext, .rgPageLast {display:none; } 
        </style>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <asp:Literal ID="litMessage" runat="server" />
    <asp:Panel ID="pnlMain" runat="server">
        <ctrl:ClientDetailTabs ID="ClientTabs" runat="server" SelectedTabIndex="6" />
        <div class="block">
            <div class="row">
                <asp:HyperLink ID="hlBacktoList" runat="server"><i class="gi gi-list" title="Back to Ticket Project list"></i>  Back to Ticket Project list</asp:HyperLink>
            </div>
        </div>
        <div class="block">
            <div class="block-title">
                <h2><strong>Search Criteria</strong></h2>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="col-md-4">
                        <label>Assigned To</label>
                    </div>
                    <div class="col-md-8">
                        <ddl:Employee ID="ddlEmployeeAssignedTo" runat="server" DisplayChosenScript="true" DefaultValue="" SetSize="false" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="form-group form-actions" style="margin-left:8px">
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                </div>
            </div> 
        </div>
        <div class="block" style="padding-bottom: 20px; overflow-x:scroll">
            <div class="row">
                <div style="float:right;">
                    <asp:LinkButton ID="lbAddTicket" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbAddTicket_Click">Add Ticket</asp:LinkButton>
                </div>
            </div><br />
            <div class="row">
                <div class="form-group">
                    <div class="col-sm-12">
                        <telerik:RadGrid ID="rgAllTicketClient" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                                AllowPaging="true" ShowFooter="false" PageSize="25" Width="100%" OnNeedDataSource="rgAllTicketClient_NeedDataSource" OnItemDataBound="rgAllTicketClient_ItemDataBound" >
                                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" 
                                    DataKeyNames="Id"
                                    Width="100%" AllowSorting="true" CommandItemDisplay="None">
                                    <NoRecordsTemplate>
                                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                    </NoRecordsTemplate>
                                    <Columns>
                                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="3%" HeaderText="" UniqueName="ActionColumn">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbBulk" runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="Id" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="#" UniqueName="Id" />
                                        <telerik:GridBoundColumn DataField="PriorityName" HeaderStyle-Width="3%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="P#" SortExpression="PriorityName" UniqueName="PriorityName" HeaderTooltip="Ticket Priority" />
                                        <telerik:GridBoundColumn DataField="Summary" HeaderStyle-Width="18%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Summary" SortExpression="Summary" UniqueName="Summary" HeaderTooltip="Ticket Summary" />
                                        <telerik:GridBoundColumn DataField="ClientName" HeaderStyle-Width="8%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Company" SortExpression="ClientName" UniqueName="ClientName" HeaderTooltip="Company Name" />
                                        <telerik:GridBoundColumn DataField="ClientName" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Company" SortExpression="ClientName" UniqueName="ExportClientName" HeaderTooltip="Company Name" Visible="false"/>
                                        <telerik:GridBoundColumn DataField="DispositionName" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Status" SortExpression="DispositionName" UniqueName="DispositionName" HeaderTooltip="Ticket Status" />
                                        <telerik:GridBoundColumn DataField="DateEntered" HeaderStyle-Width="8%" DataType="System.DateTime" ItemStyle-HorizontalAlign="Left" HeaderText="Entered" SortExpression="DateEntered" UniqueName="DateEntered" HeaderTooltip="Date Entered" DataFormatString="{0:MM/dd/yy HH:mm}" />
                                        <telerik:GridBoundColumn DataField="LastUpdated" HeaderStyle-Width="8%" DataType="System.DateTime" ItemStyle-HorizontalAlign="Left" HeaderText="Updated" SortExpression="LastUpdated" UniqueName="LastUpdated" HeaderTooltip="Date Last Updated" DataFormatString="{0:MM/dd/yy HH:mm}" />
                                        <telerik:GridBoundColumn DataField="AssignedToUserName" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Assigned" SortExpression="AssignedToUserName" UniqueName="AssignedToUserName" HeaderTooltip="Assigned To" />
                                        <telerik:GridBoundColumn DataField="Description" HeaderStyle-Width="12%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Description" SortExpression="Description" UniqueName="Description" HeaderTooltip="Description" />                                
                                    </Columns>
                                </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" />
                        </telerik:RadGrid>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>