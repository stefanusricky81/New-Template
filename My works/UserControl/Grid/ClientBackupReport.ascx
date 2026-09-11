<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientBackupReport.ascx.cs" Inherits="UserControl_Grid_ClientBackupReport" %>
<telerik:RadNotification ID="rnConfirmation" runat="server" Width="250" Height="100" EnableRoundedCorners="true" VisibleTitlebar="false" Pinned="false" Position="Center" Skin="Black" AnimationDuration="150" Animation="FlyIn"/>
<asp:Panel ID="pnlHeader" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="GridHeaderTable">
        <tr>
            <th style="width:33%"><asp:Literal ID="litHeader" runat="server"></asp:Literal></th>
            <td style="width:33%; text-align:center;">&nbsp;</td>
            <td style="width:32%">Records Per Page:&nbsp;<uc:RecordPerPageDDL ID="ucRecordsPerPage" runat="server" /></td>
            <td style="width:2%"><div class="showHide"><asp:HyperLink ID="hlShowHide" runat="server" /></div></td>
        </tr>
    </table>
</asp:Panel>

<div ID="divClientBackupReport" runat="server" style="border-style:none;">

<telerik:RadGrid ID="rgClientBackupReport" runat="server"
    OnNeedDataSource="rgClientBackupReport_NeedDataSource" 
    OnItemDataBound="rgClientBackupReport_ItemDataBound"
    OnItemCreated="rgClientBackupReport_ItemCreated"
    OnPreRender="rgClientBackupReport_PreRender" 
    OnItemCommand="rgClientBackupReport_ItemCommand"
    PageSize="50"
    Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false" 
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="true" PagerStyle-Position="Bottom">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" >
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
    
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="fixed" CommandItemSettings-ShowAddNewRecordButton="false" InsertItemPageIndexAction="ShowItemOnCurrentPage" 
        DataKeyNames="Id,IsFailure,BackupMonitorStatusName,StatusGridName,BackupSetId" CommandItemDisplay="Top" EditMode="EditForms" Width="100%">

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
            <telerik:GridBoundColumn UniqueName="Date" DataField="Date" SortExpression="Date" HeaderText="Monitor Date" HeaderTooltip="Monitor Date" DataFormatString="{0:MM/dd/yy}" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="5%"/>
            <telerik:GridBoundColumn UniqueName="ClientCompany" DataField="ClientCompany" SortExpression="ClientCompany" HeaderText="Client" HeaderTooltip="Client Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="15%"/>
            <telerik:GridBoundColumn UniqueName="Server" DataField="Server" SortExpression="Server" HeaderText="Protected Machine" HeaderTooltip="Protected Machine" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="15%"/>
            <telerik:GridBoundColumn UniqueName="BackupServer" DataField="BackupServer" SortExpression="BackupServer" HeaderText="BU Server" HeaderTooltip="Backup Server" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="10%"/>
            <telerik:GridBoundColumn UniqueName="BackupSetName" DataField="BackupSetName" SortExpression="BackupSetName" HeaderText="BU Set Name" HeaderTooltip="Backup Set Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="15%"/>
            <telerik:GridBoundColumn UniqueName="VeeamBackupTypeName" DataField="VeeamBackupTypeName" SortExpression="VeeamBackupTypeName" HeaderText="Veeam Job Type" HeaderTooltip="Veeam Job Type" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="15%"/>
            <telerik:GridBoundColumn UniqueName="BackupMonitorStatusName" DataField="BackupMonitorStatusName"  SortExpression="BackupMonitorStatusName" HeaderText="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="5%"/>
            <telerik:GridBoundColumn UniqueName="AppassureReportStartDateTime" DataField="AppassureReportStartDateTime" SortExpression="AppassureReportStartDateTime" HeaderText="BU Date" HeaderTooltip="Backed Up Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="8%"/>
            <telerik:GridBoundColumn UniqueName="FullName" DataField="FullName" SortExpression="FullName" HeaderText="Updated By" HeaderTooltip="Updated By" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="8%"/>
            <telerik:GridButtonColumn UniqueName="Delete" CommandName="Delete" Text="Delete" ConfirmText="Confirm Delete" ItemStyle-ForeColor="#DB2929" HeaderStyle-Width="4%" />
        </Columns>
        
    </MasterTableView>
    
</telerik:RadGrid>
</div>