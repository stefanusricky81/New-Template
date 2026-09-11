<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientTicketReportDetail.aspx.cs" Inherits="Reports_ClientTicketReportDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Client Ticket Report Details</h1>
        </div>
    </div>
    <div class="block" style="padding-bottom: 20px;">
        <div class="table-responsive scroll">
            <telerik:RadGrid ID="rgClientTicketReportDetail" Skin="3b" EnableEmbeddedSkins="false" OnNeedDataSource="rgClientTicketReportDetail_NeedDataSource"
                runat="server" AutoGenerateColumns="false" AllowSorting="true" OnItemDataBound="rgClientTicketReportDetail_ItemDataBound" 
                AllowPaging="true" ShowFooter="false" PageSize="20" Width="150%" ClientSettings-Resizing-AllowColumnResize="true"> 
                <MasterTableView ShowHeadersWhenNoRecords="true" DataKeyNames="ID" TableLayout="Fixed" Width="150%">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" HeaderStyle-Width="3%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket #" SortExpression="ID" UniqueName="ID" HeaderTooltip="Ticket #" />
                        <telerik:GridBoundColumn DataField="TICKETRECEIVED" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Received" SortExpression="TICKETRECEIVED" UniqueName="TICKETRECEIVED" HeaderTooltip="Ticket Received" />
                        <telerik:GridBoundColumn DataField="DESCR" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Description" SortExpression="DESCR" UniqueName="CompDESCRany" HeaderTooltip="Ticket Description" />
                        <telerik:GridBoundColumn DataField="REQUESTOR" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Requestor" SortExpression="REQUESTOR" UniqueName="REQUESTOR" HeaderTooltip="Requestor" />
                        <telerik:GridBoundColumn DataField="CONTACTNAME" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Client contact" SortExpression="CONTACTNAME" UniqueName="CONTACTNAME" HeaderTooltip="Client contact name" />
                        <telerik:GridBoundColumn DataField="TICKETPRIORITY" HeaderStyle-Width="3%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Priority" SortExpression="TICKETPRIORITY" UniqueName="TICKETPRIORITY" HeaderTooltip="Ticket Priority" />
                        <telerik:GridBoundColumn DataField="TICKETSUMMARY" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Summary" SortExpression="TICKETSUMMARY" UniqueName="TICKETSUMMARY" HeaderTooltip="Ticket Summary" />
                        <telerik:GridBoundColumn DataField="RESPONSETIMENEW" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Response Time" SortExpression="RESPONSETIMENEW" UniqueName="RESPONSETIMENEW" HeaderTooltip="Response Time" />
                        <telerik:GridBoundColumn DataField="TICKETCLOSED" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Closed" SortExpression="TICKETCLOSED" UniqueName="TICKETCLOSED" HeaderTooltip="Ticket Closed" />
                        <telerik:GridBoundColumn DataField="TECHNICIAN" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Technician" SortExpression="TECHNICIAN" UniqueName="TECHNICIAN" HeaderTooltip="Technician" />
                        <telerik:GridBoundColumn DataField="TICKETSTATUS" HeaderStyle-Width="3%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Status" SortExpression="TICKETSTATUS" UniqueName="TICKETSTATUS" HeaderTooltip="Ticket Status" />
                        <telerik:GridBoundColumn DataField="RECEIVEDMETHOD" HeaderStyle-Width="3%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Receive Via" SortExpression="RECEIVEDMETHOD" UniqueName="RECEIVEDMETHOD" HeaderTooltip="Receive Via" />
                        <telerik:GridBoundColumn DataField="TICKETCATEGORY" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Category" SortExpression="TICKETCATEGORY" UniqueName="TICKETCATEGORY" HeaderTooltip="Ticket Category" />
                        <telerik:GridBoundColumn DataField="AllUpdateNotes" HeaderStyle-Width="75%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="All Update Note" SortExpression="AllUpdateNotes" UniqueName="AllUpdateNotes" HeaderTooltip="All Update Note" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
    
</asp:Content>

