<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProgrammingProject.ascx.cs" Inherits="UserControl_Grid_ProgrammingProject" %>

<style type="text/css">
div.RadGrid .rgRow td,
div.RadGrid .rgAltRow td
{
    padding-right: 1px !important;
    white-space:nowrap;
	overflow:hidden;
    text-overflow:ellipsis;
}
</style>

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

<div ID="divProgrammingProject" runat="server" style="border-style:none;">

<telerik:RadGrid ID="rgProgrammingProject" runat="server"
    OnItemCreated="rgProgrammingProject_ItemCreated"
    OnNeedDataSource="rgProgrammingProject_NeedDataSource" 
    OnItemDataBound="rgProgrammingProject_ItemDataBound"  
    OnDeleteCommand="rgProgrammingProject_DeleteCommand"
    PageSize="50"
    Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false" 
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="false" PagerStyle-Position="Bottom">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" >
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
    
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="auto" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="Id" CommandItemDisplay="Top" Width="100%">
        
        <CommandItemTemplate>
            <table class="rgCommandRow" style="width:100%">
                <tr>    
                    <td style="width:50%">
                        <asp:LinkButton ID="lbAdd" runat="server" PostBackUrl="/ProgrammingProject/Detail.aspx?ProgrammingProjectId=0">
                            <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/AddRecord.gif"/>Add New
                        </asp:LinkButton>
                    </td>
                    <td style="width:50%" align="right">
                        <asp:LinkButton ID="lbRefresh" runat="server" CommandName="RefreshGrid">
                            <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Refresh.gif" />Refresh
                        </asp:LinkButton>
                    </td>
            </table>
        </CommandItemTemplate>

        <Columns>
            <telerik:GridEditCommandColumn UniqueName="Edit" HeaderStyle-Width="5%" EditText="Edit">
            </telerik:GridEditCommandColumn>
            <telerik:GridBoundColumn UniqueName="ProductionFqdn" DataField="ProductionFqdn" SortExpression="ProductionFqdn" HeaderText="Production FQDN" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="ClientCompany" DataField="ClientCompany" SortExpression="ClientCompany" HeaderText="Client" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="ProductionServerName" DataField="ProductionServerName" SortExpression="ProductionServerName" HeaderText="Server Name" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="ProgrammingProjectServerOsName" DataField="ProgrammingProjectServerOsName" SortExpression="ProgrammingProjectServerOsName" HeaderText="OS" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="ProgrammingProjectServerWebPlatformName" DataField="ProgrammingProjectServerWebPlatformName" SortExpression="ProgrammingProjectServerWebPlatformName" HeaderText="Platform" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="ProgrammingProjectServerDatabaseName" DataField="ProgrammingProjectServerDatabaseName" SortExpression="ProgrammingProjectServerDatabaseName" HeaderText="DB" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="ProgrammingProjectDevelopmentLanguageName" DataField="ProgrammingProjectDevelopmentLanguageName" SortExpression="ProgrammingProjectDevelopmentLanguageName" HeaderText="Language" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridButtonColumn UniqueName="Delete" CommandName="Delete" Text="Delete" ConfirmText="Confirm Delete" 
                ItemStyle-ForeColor="#DB2929" HeaderStyle-Width="5%">
            </telerik:GridButtonColumn>
        </Columns>
        
    </MasterTableView>
    
</telerik:RadGrid>

</div>