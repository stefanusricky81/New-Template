<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchActivity.ascx.cs" Inherits="UserControl_Grid_SearchActivity" %>

<telerik:radscriptblock id="RadScriptBlock1" runat="server">
    <script type="text/javascript">
        //disable ajax for export button click
        function conditionalPostback(e, sender) {
            var theRegexp = new RegExp("\.btnExport$", "ig");
            if (sender.EventTarget.match(theRegexp)) {
                sender.EnableAjax = false;
            }
        }
    </script>
</telerik:radscriptblock>

<asp:Panel ID="pnlHeader" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="GridHeaderTable">
        <tr>
            <th style="width:33%"><asp:Literal ID="litHeader" runat="server"></asp:Literal></th>
            <td style="width:33%; text-align:center;">&nbsp;</td>
            <td style="width:34%">&nbsp;</td>
        </tr>
    </table>
</asp:Panel>

<telerik:RadGrid ID="rgSearchActivity" runat="server"
    OnNeedDataSource="rgSearchActivity_NeedDataSource" 
    OnItemDataBound="rgSearchActivity_ItemDataBound"
    OnItemCreated="rgSearchActivity_ItemCreated"
    OnPreRender="rgSearchActivity_PreRender"
    OnItemCommand="rgSearchActivity_ItemCommand"
    PageSize="500"
    Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false" 
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="false" PagerStyle-Position="Bottom">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" >
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
    
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="fixed" CommandItemSettings-ShowAddNewRecordButton="false" InsertItemPageIndexAction="ShowItemOnCurrentPage" 
        DataKeyNames="UserId" CommandItemDisplay="Top" EditMode="EditForms" Width="100%">

        <CommandItemTemplate>
            <table class="rgCommandRow" style="width:100%">
                <tr>
                    <td style="width:70%" align="left">
                        <uc:GridExportTypeDDL ID="ucGridExportType" runat="server" />&nbsp;
                        <asp:LinkButton ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click"/>
                    </td>
                    <td style="width:15%" align="right">
                        <asp:LinkButton ID="lbRefresh" runat="server" CommandName="RefreshGrid">
                            <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Refresh.gif" />Refresh
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </CommandItemTemplate> 
                                
        <Columns>
            <telerik:GridBoundColumn UniqueName="UserFirst" DataField="UserFirst" SortExpression="UserFirst" HeaderText="Employee" HeaderTooltip="Employee First Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String"/>
            <telerik:GridBoundColumn UniqueName="ActiveTicketStartTime" DataField="ActiveTicketStartTime" SortExpression="ActiveTicketStartTime" HeaderText="Start" HeaderTooltip="Start Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.DateTime" />
            <telerik:GridBoundColumn UniqueName="ActiveTicketEndTime" DataField="ActiveTicketEndTime" SortExpression="ActiveTicketEndTime" HeaderText="End" HeaderTooltip="End Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.DateTime" />
            <telerik:GridBoundColumn UniqueName="TicketSummary" DataField="TicketSummary" SortExpression="TicketSummary" HeaderText="T. Summary" HeaderTooltip="Ticket Summary" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String"/>
            <telerik:GridBoundColumn UniqueName="TicketEstimate" DataField="TicketEstimate" SortExpression="TicketEstimate" HeaderText="T. Estimate" HeaderTooltip="Ticket Estimate" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String"/>
            <telerik:GridBoundColumn UniqueName="ClientCompany" DataField="ClientCompany" SortExpression="ClientCompany" HeaderText="T. Client" HeaderTooltip="Ticket Client Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String"/>
            <telerik:GridBoundColumn UniqueName="TicketDateEntered" DataField="TicketDateEntered" SortExpression="TicketDateEntered" HeaderText="T. Entered" HeaderTooltip="Ticket Entered" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.DateTime" />
            <telerik:GridBoundColumn UniqueName="TicketHistoryCreated" DataField="TicketHistoryCreated" SortExpression="TicketHistoryCreated" HeaderText="T. Updated Ext." HeaderTooltip="Last External Ticket Update" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.DateTime" />
            <telerik:GridBoundColumn UniqueName="TicketHistoryNotes" DataField="TicketHistoryNotes" SortExpression="TicketHistoryNotes" HeaderText="T. Updated Ext. Notes" HeaderTooltip="Last External Ticket Update Notes" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String" />            
            <telerik:GridBoundColumn UniqueName="InternalTicketHistoryCreated" DataField="InternalTicketHistoryCreated" SortExpression="InternalTicketHistoryCreated" HeaderText="T. Updated Int." HeaderTooltip="Last Internal Ticket Update" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.DateTime" />
                       <telerik:GridBoundColumn UniqueName="InternalTicketHistoryNotes" DataField="InternalTicketHistoryNotes" SortExpression="InternalTicketHistoryNotes" HeaderText="T. Updated Int. Notes" HeaderTooltip="Last Interal Ticket Update Notes" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String" />
        </Columns>
        
    </MasterTableView>
    
</telerik:RadGrid>