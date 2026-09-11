<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientEmailBroadcast.ascx.cs" Inherits="UserControl_Grid_ClientEmailBroadcast" %>
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
            <td style="width:33%; text-align:center;">
               &nbsp;
            </td>
            <td style="width:32%">
                Records Per Page:&nbsp;
                <uc:RecordPerPageDDL ID="ucRecordsPerPage" runat="server" />
            </td>
            <td style="width:2%">
                <div class="showHide">
                    <asp:HyperLink ID="hlShowHide" runat="server" />
                </div>	
            </td>    
        </tr>
    </table>
</asp:Panel>

<div ID="divClientEmailBroadcast" runat="server" style="border-style:none;">

<telerik:RadGrid ID="rgEmailBroadcast" AllowSorting="true" AutoGenerateColumns="false"
    Skin="BitByBit" runat="server" AllowPaging="true" PageSize="25"
    OnPreRender="rgEmailBroadcast_PreRender"
    OnNeedDataSource="rgEmailBroadcast_NeedDataSource" 
    OnItemCommand="rgEmailBroadcast_ItemCommand"
    AllowMultiRowSelection="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    ShowFooter="true" PagerStyle-Mode="NumericPages"
    PagerStyle-AlwaysVisible="true" EnableEmbeddedSkins="false"  
    MasterTableView-NoMasterRecordsText="No Records found.">

  
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" >
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
        
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" CommandItemSettings-ShowAddNewRecordButton="false"
        InsertItemPageIndexAction="ShowItemOnCurrentPage" CommandItemDisplay="Top" Width="100%">


        <CommandItemTemplate>
            <table class="rgCommandRow" style="width:100%">
                <tr>
                    <td style="width:75%" align="left">
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
            <telerik:GridBoundColumn HeaderText="Contact_Id" DataField="pclient_contact" SortExpression="pclient_contact"
                HeaderStyle-Width="5%" UniqueName="pclient_contact" Visible="false">
            </telerik:GridBoundColumn>

            <telerik:GridBoundColumn HeaderText="Client" DataField="Client_Id" SortExpression="Client_Id"
                HeaderStyle-Width="5%" UniqueName="Client_Id" Visible="false">
            </telerik:GridBoundColumn>

            <telerik:GridBoundColumn HeaderText="Company" DataField="Company" SortExpression="Company"
                HeaderStyle-Width="20%" UniqueName="Company">
            </telerik:GridBoundColumn>

            <telerik:GridBoundColumn HeaderText="First Name" DataField="ContactFirst" SortExpression="ContactFirst"
                HeaderStyle-Width="15%" UniqueName="ContactFirst">
            </telerik:GridBoundColumn>

            <telerik:GridBoundColumn HeaderText="Last Name" DataField="ContactLast" SortExpression="ContactLast"
                HeaderStyle-Width="15%" UniqueName="ContactLast">
            </telerik:GridBoundColumn>

            <telerik:GridBoundColumn HeaderText="Email" DataField="ContactEmail" SortExpression="ContactEmail"
                HeaderStyle-Width="15%" UniqueName="ContactEmail" Visible="false">
            </telerik:GridBoundColumn>

            <telerik:GridHyperLinkColumn HeaderText="Email" DataNavigateUrlFields="ContactEmail" 
                SortExpression="ContactEmail" DataTextField="ContactEmail"
                DataNavigateUrlFormatString="mailto:{0}" HeaderStyle-Width="20%">
            </telerik:GridHyperLinkColumn>
        </Columns>
        
    </MasterTableView>
    
</telerik:RadGrid>


</div>
