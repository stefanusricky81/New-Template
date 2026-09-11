<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Product.ascx.cs" Inherits="UserControl_Grid_Product" %>
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

<div ID="divProduct" runat="server" style="border-style:none;">
<telerik:RadGrid ID="rgProduct" runat="server"
    OnNeedDataSource="rgProduct_NeedDataSource" 
    OnItemDataBound="rgProduct_ItemDataBound" 
    OnUpdateCommand="rgProduct_UpdateCommand" 
    OnInsertCommand="rgProduct_InsertCommand" 
    OnDeleteCommand="rgProduct_DeleteCommand"
    OnPreRender="rgProduct_PreRender"
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
        CommandItemSettings-AddNewRecordText="Add Product">
        
        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/ProductEdit.ascx" EditFormType="WebUserControl">
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