<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EmployeeScheduling.ascx.cs" Inherits="UserControl_Grid_EmployeeScheduling" %>
<div class="table-responsive" style="overflow:visible !important">
    <telerik:RadGrid ID="rgAvailability" runat="server" OnNeedDataSource="rgAvailability_NeedDataSource" AutoGenerateColumns="false"
        OnItemDataBound="rgAvailability_ItemDataBound"
        AllowPaging="true" PageSize="20">
        <MasterTableView>
            <Columns>
                <telerik:GridBoundColumn DataField="Date" HeaderStyle-Width="9%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Date" SortExpression="Date" UniqueName="Date" HeaderTooltip="Date" />
                <telerik:GridBoundColumn DataField="Start" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Start" SortExpression="Start" UniqueName="Start" HeaderTooltip="Start" />
                <telerik:GridBoundColumn DataField="End" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="End" SortExpression="End" UniqueName="End" HeaderTooltip="End" />
                <telerik:GridBoundColumn DataField="Subject" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Subject" SortExpression="Subject" UniqueName="Subject" HeaderTooltip="Subject" />
                                    
<%--                            <telerik:GridBoundColumn DataField="Employee" HeaderText="Employee" />
                <telerik:GridBoundColumn DataField="BusyTimes" HeaderText="Busy Times" />--%>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>