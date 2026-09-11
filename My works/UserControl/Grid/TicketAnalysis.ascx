<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketAnalysis.ascx.cs" Inherits="UserControl_Grid_TicketAnalysis" %>

<telerik:RadGrid ID="rgTicketAnalysis" runat="server"
    OnNeedDataSource="rgTicketAnalysis_NeedDataSource" 
    OnItemDataBound="rgTicketAnalysis_ItemDataBound"
    OnDetailTableDataBind="rgTicketAnalysis_DetailTableDataBind"
    Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="false" AutoGenerateColumns="false" AllowMultiRowSelection="false"
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="true" 
    MasterTableView-NoMasterRecordsText="No Records found.">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="false">
        <Selecting AllowRowSelect="false" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
        
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="fixed" Width="100%" DataKeyNames="Date">
        <DetailTables>
            <telerik:GridTableView DataKeyNames="Date,DisplayValue" Name="Week" Width="100%">
                 <DetailTables>
                    <telerik:GridTableView DataKeyNames="Date" Name="Day" Width="100%">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="DisplayValue" DataField="DisplayValue" 
                                SortExpression="DisplayValue" HeaderText="Date"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" 
                                ItemStyle-VerticalAlign="Top">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="UpdatedTickets" DataField="UpdatedTickets" 
                                SortExpression="UpdatedTickets" HeaderText="Updated"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="NewTickets" DataField="NewTickets" 
                                SortExpression="NewTickets" HeaderText="New"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ActiveTickets" DataField="ActiveTickets" 
                                SortExpression="ActiveTickets" HeaderText="Active"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="PhoneTickets" DataField="PhoneTickets" 
                                SortExpression="PhoneTickets" HeaderText="Via Phone"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="EmailTickets" DataField="EmailTickets" 
                                SortExpression="EmailTickets" HeaderText="Via Email"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="WebTickets" DataField="WebTickets" 
                                SortExpression="WebTickets" HeaderText="Via Web"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="VmTickets" DataField="VmTickets" 
                                SortExpression="VmTickets" HeaderText="Via VM"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="AfterHoursTickets" DataField="AfterHoursTickets" 
                                SortExpression="AfterHoursTickets" HeaderText="After Hours"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="AverageResponse" DataField="AverageResponse" 
                                SortExpression="AverageResponse" HeaderText="Avg Resp" DataFormatString="{0:F}"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                                ItemStyle-VerticalAlign="Top" DataType="System.Double" Aggregate="Avg" FooterAggregateFormatString="Avg: {0:F}">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="ClosedTickets" DataField="ClosedTickets" 
                                SortExpression="ClosedTickets" HeaderText="Total Closed"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="SameDayClosedTickets" DataField="SameDayClosedTickets" 
                                SortExpression="SameDayClosedTickets" HeaderText="Same Day Closed"
                                HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </telerik:GridTableView>
                </DetailTables>
                <Columns>
                    <telerik:GridBoundColumn UniqueName="DisplayValue" DataField="DisplayValue" 
                        SortExpression="DisplayValue" HeaderText="Date"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" 
                        ItemStyle-VerticalAlign="Top">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="UpdatedTickets" DataField="UpdatedTickets" 
                        SortExpression="UpdatedTickets" HeaderText="Updated"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                        ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="NewTickets" DataField="NewTickets" 
                        SortExpression="NewTickets" HeaderText="New"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                        ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="ActiveTickets" DataField="ActiveTickets" 
                        SortExpression="ActiveTickets" HeaderText="Active"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                        ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="PhoneTickets" DataField="PhoneTickets" 
                        SortExpression="PhoneTickets" HeaderText="Via Phone"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                        ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="EmailTickets" DataField="EmailTickets" 
                        SortExpression="EmailTickets" HeaderText="Via Email"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                        ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="WebTickets" DataField="WebTickets" 
                        SortExpression="WebTickets" HeaderText="Via Web"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                        ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="VmTickets" DataField="VmTickets" 
                        SortExpression="VmTickets" HeaderText="Via VM"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                        ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="AfterHoursTickets" DataField="AfterHoursTickets" 
                        SortExpression="AfterHoursTickets" HeaderText="After Hours"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                        ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="AverageResponse" DataField="AverageResponse" 
                        SortExpression="AverageResponse" HeaderText="Avg Resp"  DataFormatString="{0:F}"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                        ItemStyle-VerticalAlign="Top" DataType="System.Double" Aggregate="Avg" FooterAggregateFormatString="Avg: {0:F}">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="ClosedTickets" DataField="ClosedTickets" 
                        SortExpression="ClosedTickets" HeaderText="Total Closed"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                        ItemStyle-VerticalAlign="Top">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="SameDayClosedTickets" DataField="SameDayClosedTickets" 
                        SortExpression="SameDayClosedTickets" HeaderText="Same Day Closed"
                        HeaderStyle-HorizontalAlign="Center" 
                        ItemStyle-VerticalAlign="Top">
                    </telerik:GridBoundColumn>
                </Columns>
            </telerik:GridTableView>
        </DetailTables>
        <Columns>
            <telerik:GridBoundColumn UniqueName="DisplayValue" DataField="DisplayValue" 
                SortExpression="DisplayValue" HeaderText="Date"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" 
                ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="UpdatedTickets" DataField="UpdatedTickets" 
                SortExpression="UpdatedTickets" HeaderText="Updated"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="NewTickets" DataField="NewTickets" 
                SortExpression="NewTickets" HeaderText="New"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="ActiveTickets" DataField="ActiveTickets" 
                SortExpression="ActiveTickets" HeaderText="Active"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="PhoneTickets" DataField="PhoneTickets" 
                SortExpression="PhoneTickets" HeaderText="Via Phone"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="EmailTickets" DataField="EmailTickets" 
                SortExpression="EmailTickets" HeaderText="Via Email"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="WebTickets" DataField="WebTickets" 
                SortExpression="WebTickets" HeaderText="Via Web"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="VmTickets" DataField="VmTickets" 
                SortExpression="VmTickets" HeaderText="Via VM"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="AfterHoursTickets" DataField="AfterHoursTickets" 
                SortExpression="AfterHoursTickets" HeaderText="After Hours"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="AverageResponse" DataField="AverageResponse" 
                SortExpression="AverageResponse" HeaderText="Avg Resp"  DataFormatString="{0:F}"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                ItemStyle-VerticalAlign="Top" DataType="System.Double" Aggregate="Avg" FooterAggregateFormatString="Avg: {0:F}">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="ClosedTickets" DataField="ClosedTickets" 
                SortExpression="ClosedTickets" HeaderText="Total Closed"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"
                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="SameDayClosedTickets" DataField="SameDayClosedTickets" 
                SortExpression="SameDayClosedTickets" HeaderText="Same Day Closed"
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-VerticalAlign="Top" Aggregate="Sum" FooterText="Total: ">
            </telerik:GridBoundColumn>
        </Columns>

    </MasterTableView>

</telerik:RadGrid>


