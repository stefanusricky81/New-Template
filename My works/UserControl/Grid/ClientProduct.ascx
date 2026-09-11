<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientProduct.ascx.cs" Inherits="UserControl_Grid_ClientProduct" %>
<asp:Literal ID="litMessage" runat="server" />
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

<div ID="divClientProduct" runat="server" style="border-style:none;">
<telerik:RadGrid ID="rgClientProduct" runat="server"
    OnNeedDataSource="rgClientProduct_NeedDataSource" 
    OnItemDataBound="rgClientProduct_ItemDataBound" 
    OnUpdateCommand="rgClientProduct_UpdateCommand" 
    OnInsertCommand="rgClientProduct_InsertCommand" 
    OnDeleteCommand="rgClientProduct_DeleteCommand"
    OnPreRender="rgClientProduct_PreRender"
    PageSize="50"
    Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false" 
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="true" PagerStyle-Position="Bottom">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" >
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
    
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" InsertItemPageIndexAction="ShowItemOnCurrentPage"
        DataKeyNames="Id" CommandItemDisplay="Top" EditMode="EditForms" Width="100%" 
        CommandItemSettings-AddNewRecordText="Add Acccount Type">
        
        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/ClientProductEdit.ascx" EditFormType="WebUserControl">
            <EditColumn UniqueName="EditCommandColumn1"></EditColumn>
        </EditFormSettings>
        
        <CommandItemTemplate>
            <table class="rgCommandRow" style="width:100%">
                <tr>
                    <td style="width:33%">
                         <asp:LinkButton ID="lbAddTicket" runat="server" CommandName="InitInsert">
                            <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/AddRecord.gif"/>Add Account Type
                        </asp:LinkButton>
                    </td>
                    <td style="width:33%" align="center">&nbsp;</td>
                    <td style="width:33%" align="right">
                        <asp:LinkButton ID="lbRefresh" runat="server" CommandName="RefreshGrid">
                            <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Refresh.gif" />Refresh
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </CommandItemTemplate> 
        <Columns>
            <telerik:GridEditCommandColumn UniqueName="Edit" HeaderStyle-Width="5%" EditText="Edit" />
            <telerik:GridBoundColumn UniqueName="Name" DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="25%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="Description" DataField="Description" SortExpression="Description" HeaderText="Description" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="35%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="Active" DataField="Active" SortExpression="Active" HeaderText="Active" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" DataType="System.Boolean"/>
            <telerik:GridBoundColumn UniqueName="Created" DataField="Created" SortExpression="Created" HeaderText="Created" DataFormatString="{0:MM/dd/yy}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridBoundColumn UniqueName="LastUpdated" DataField="LastUpdated" SortExpression="LastUpdated" HeaderText="Updated" DataFormatString="{0:MM/dd/yy}" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
            <telerik:GridButtonColumn UniqueName="Delete" CommandName="Delete" Text="Delete" ConfirmText="Confirm Delete" ItemStyle-ForeColor="#DB2929" HeaderStyle-Width="5%" />
        </Columns>
        
    </MasterTableView>
    
</telerik:RadGrid>

</div>