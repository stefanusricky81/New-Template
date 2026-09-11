<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EmployeeActivity.ascx.cs" Inherits="UserControl_Grid_EmployeeActivity" %>
<telerik:RadGrid ID="rgEmployeeActivity" runat="server"
    OnNeedDataSource="rgEmployeeActivity_NeedDataSource" 
    OnItemDataBound="rgEmployeeActivity_ItemDataBound"
    OnItemCreated="rgEmployeeActivity_ItemCreated"
    OnPreRender="rgEmployeeActivity_PreRender"
    Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false" 
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="true" PagerStyle-Position="Bottom">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" >
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
    
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="fixed" CommandItemSettings-ShowAddNewRecordButton="false" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="Digit" CommandItemDisplay="Top" EditMode="EditForms" Width="975px">

        <CommandItemTemplate>
            <table class="rgCommandRow" style="width:100%">
                <tr>
                    <td style="width:70%" align="left">
                        <uc:GridExportTypeDDL ID="ucGridExportType" runat="server" />&nbsp;
                        <asp:LinkButton ID="btnExport" runat="server"
                            Text="Export" OnClick="btnExport_Click"/>
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
            <telerik:GridBoundColumn UniqueName="Id" DataField="Digit" SortExpression="Digit" HeaderText="Tick." HeaderTooltip="Ticket" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="Test" DataField="Even" SortExpression="Even" HeaderText="Mon. Date" HeaderTooltip="Monitor Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
            
        </Columns>
        
    </MasterTableView>
    
</telerik:RadGrid>