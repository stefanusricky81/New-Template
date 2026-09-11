<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Timesheet.ascx.cs" Inherits="UserControl_Grid_Timesheet" %>

<telerik:radscriptblock id="RadScriptBlock1" runat="server">
    <script type="text/javascript">
        //disable ajax for export button click
        function conditionalPostback(e, sender) {
            var theRegexp = new RegExp("\.btnExport$", "ig");
            if (sender.EventTarget.match(theRegexp))
            {
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
            <td style="width:32%">Records Per Page:&nbsp;<uc:RecordPerPageDDL ID="ucRecordsPerPage" runat="server" /></td>
            <td style="width:2%"><div class="showHide"><asp:HyperLink ID="hlShowHide" runat="server" /></div></td>
        </tr>
    </table>
</asp:Panel>

<div ID="divTimesheet" runat="server" style="border-style:none;">

<telerik:RadGrid ID="rgTimesheet" runat="server"
    OnNeedDataSource="rgTimesheet_NeedDataSource" 
    OnItemDataBound="rgTimesheet_ItemDataBound" 
    OnUpdateCommand="rgTimesheet_UpdateCommand" 
    OnInsertCommand="rgTimesheet_InsertCommand"
    OnItemCreated="rgTimesheet_ItemCreated"
    OnDeleteCommand="rgTimesheet_DeleteCommand"
    OnPreRender="rgTimesheet_PreRender"
    PageSize="50"
    Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false" 
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="true">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" >
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
    
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="Ptimesheet" CommandItemDisplay="Top" EditMode="EditForms" Width="100%" CommandItemSettings-AddNewRecordText="Add Timesheet">
        <CommandItemTemplate>
            <table class="rgCommandRow" style="width:100%">
                <tr>
                    <td style="width:33%">
                         <asp:LinkButton ID="lbAddTicket" runat="server" CommandName="InitInsert"><img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/AddRecord.gif"/>Add Timesheet</asp:LinkButton>
                    </td>
                    <td style="width:33%" align="center">
                        <uc:GridExportTypeDDL ID="ucGridExportType" runat="server" />&nbsp;<asp:LinkButton ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click"/>
                    </td>
                    <td style="width:33%" align="right">
                        <asp:LinkButton ID="lbRefresh" runat="server" CommandName="RefreshGrid"><img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Refresh.gif" />Refresh</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </CommandItemTemplate> 
            
        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/TimesheetEdit.ascx" EditFormType="WebUserControl">
            <EditColumn UniqueName="EditCommandColumn1"></EditColumn>
        </EditFormSettings>
        
        <Columns>
            <telerik:GridEditCommandColumn UniqueName="Edit" HeaderStyle-Width="5%" EditText="Edit" />
            <telerik:GridBoundColumn UniqueName="EmployeeCode" DataField="EmployeeCode" SortExpression="EmployeeCode" HeaderText="Employee" HeaderTooltip="Employee Code" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="Date" DataField="Date" SortExpression="Date" HeaderText="Date" HeaderTooltip="Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="Time" DataField="Time" SortExpression="Time" HeaderText="Start Time" HeaderTooltip="Start Time" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="Hrs" DataField="Hrs"  SortExpression="Hrs" HeaderText="Hours" HeaderTooltip="Hours" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="ClientCompany" DataField="ClientCompany" SortExpression="ClientCompany" HeaderText="Client" HeaderTooltip="Client Name" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="Descr1" DataField="Descr1" SortExpression="Descr1" HeaderText="Activity" HeaderTooltip="Activity" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="12%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="FkCscdefects" DataField="FkCscdefects" SortExpression="FkCscdefects" HeaderText="Ticket #" HeaderTooltip="Ticket # and Summary" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="Memo" DataField="Memo" SortExpression="Memo" HeaderText="Details" HeaderTooltip="Timesheet Details" HeaderStyle-Width="20%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="ProjectDescr" DataField="ProjectDescr" SortExpression="ProjectDescr" HeaderText="Project" HeaderTooltip="Project" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridButtonColumn UniqueName="Delete" CommandName="Delete" Text="Delete" ConfirmText="Confirm Delete" HeaderText="Action" HeaderTooltip="Action" ItemStyle-ForeColor="#DB2929" />
        </Columns>
        
    </MasterTableView>
    
</telerik:RadGrid>

<asp:PlaceHolder ID="phGrandTotal" runat="server">
    <br />
    <p style="font-weight:bold;">
        Total Hours: <asp:Literal ID="litGrandTotal" runat="server" />
    </p>
</asp:PlaceHolder>

</div>