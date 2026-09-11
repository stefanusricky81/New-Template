<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientBackupSet.ascx.cs" Inherits="UserControl_Grid_ClientBackupSet" %>

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

<div ID="divClientBackupSet" runat="server" style="border-style:none;">

<telerik:RadGrid ID="rgClientBackupSet" runat="server"
    OnNeedDataSource="rgClientBackupSet_NeedDataSource" 
    OnItemDataBound="rgClientBackupSet_ItemDataBound" 
    OnPreRender="rgClientBackupSet_PreRender"
    OnUpdateCommand="rgClientBackupSet_UpdateCommand"
    OnInsertCommand="rgClientBackupSet_InsertCommand" 
    OnItemCommand="rgClientBackupSet_ItemCommand" 
    OnItemCreated="rgClientBackupSet_ItemCreated"
    PageSize="50"
    Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false" 
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="true" PagerStyle-Position="Bottom">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" >
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
    
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" InsertItemPageIndexAction="ShowItemOnCurrentPage"
        DataKeyNames="Id,Active" CommandItemDisplay="Top" EditMode="EditForms" Width="100%" 
        CommandItemSettings-AddNewRecordText="Add Backup task">

        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/BackupSetEdit.ascx" EditFormType="WebUserControl">
            <EditColumn UniqueName="EditCommandColumn1"></EditColumn>
        </EditFormSettings>
                       
        <Columns>
            <telerik:GridEditCommandColumn UniqueName="Edit" HeaderStyle-Width="4%" EditText="Edit" />
            <telerik:GridBoundColumn UniqueName="Company" DataField="Company" SortExpression="Company" HeaderText="Client" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="Server" DataField="Server" SortExpression="Server" HeaderText="Server Name" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="SupportingTableDescription" DataField="SupportingTableDescription" SortExpression="SupportingTableDescription" HeaderText="Backup Type" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="Name" DataField="Name" SortExpression="Name" HeaderText="Backup Set Name" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="12%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="Description" DataField="Description" SortExpression="Description" HeaderText="Backup Set Description" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="18%" ItemStyle-VerticalAlign="Top" /> 
            <telerik:GridBoundColumn UniqueName="BackupServer" DataField="BackupServer" SortExpression="BackupServer" HeaderText="Backup Server" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" /> 
            <telerik:GridBoundColumn UniqueName="Created" DataField="Created" SortExpression="Created" HeaderText="Created" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-VerticalAlign="Top" /> 
            <telerik:GridBoundColumn UniqueName="Active" DataField="Active" SortExpression="Active" HeaderText="Active" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridButtonColumn UniqueName="Deactivate" HeaderStyle-Width="7%" />
            <telerik:GridButtonColumn UniqueName="Delete" CommandName="Delete" Text="Delete" ConfirmText="Confirm Delete" ItemStyle-ForeColor="#DB2929" HeaderStyle-Width="5%" />
        </Columns>
        
    </MasterTableView>
    
</telerik:RadGrid>

</div>