<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketHistory.ascx.cs" Inherits="UserControl_Grid_TicketHistory" %>

<telerik:RadGrid ID="rgTicketHistory" runat="server"
    OnNeedDataSource="rgTicketHistory_NeedDataSource" 
    OnItemDataBound="rgTicketHistory_ItemDataBound"
    Skin="BitByBit2" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false"
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="false" 
    MasterTableView-NoMasterRecordsText="No Records found." PageSize="20">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="false">
        <Selecting AllowRowSelect="false" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
        
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="fixed" Width="100%">
        <Columns>
            <telerik:GridBoundColumn UniqueName="Created" DataField="Created" 
                SortExpression="Created" HeaderText="Date" DataFormatString="{0:MM/dd/yy HH:mm}"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" 
                ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
             <telerik:GridBoundColumn UniqueName="Notes" DataField="Notes" 
                SortExpression="Notes" HeaderText="Notes"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="32%"
                ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="UpdatedBy" DataField="UserLast"  
                SortExpression="UserLast" HeaderText="Updated By" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%"
                ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="StatusName" DataField="StatusName"  
                SortExpression="StatusName" HeaderText="Status" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="EmployeeEmailed" DataField="InternalRecipients"  
                SortExpression="InternalRecipients" HeaderText="Employee Emailed" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="12%"
                ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
             <telerik:GridBoundColumn UniqueName="ClientEmailed" DataField="ExternalRecipients"  
                SortExpression="ExternalRecipients" HeaderText="Client Emailed" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="12%"
                ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="ViewableBy" DataField="InternalUsage"  
                SortExpression="InternalUsage" HeaderText="Viewable By" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
             <telerik:GridBoundColumn UniqueName="EmailFailure" DataField="EmailFailure"  
                SortExpression="EmailFailure" HeaderText="Email Failure" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
        </Columns>

    </MasterTableView>
    
</telerik:RadGrid>


