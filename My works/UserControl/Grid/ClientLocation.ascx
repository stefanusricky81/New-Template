<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientLocation.ascx.cs" Inherits="UserControl_Grid_ClientLocation" %>
<%@ Reference VirtualPath="~/UserControl/Grid/EditForm/ClientLocationEdit.ascx" %>
<div class="table-responsive">
    <telerik:RadGrid ID="rgClientLocation" runat="server" Skin="3b" EnableEmbeddedSkins="false"
        OnNeedDataSource="rgClientLocation_NeedDataSource"
        OnInsertCommand="rgClientLocation_InsertCommand"
        OnDeleteCommand="rgClientLocation_DeleteCommand"
        OnItemDataBound="rgClientLocation_ItemDataBound"  
        OnUpdateCommand="rgClientLocation_UpdateCommand" 
        OnPreRender="rgClientLocation_PreRender" 
        OnItemCommand="rgClientLocation_ItemCommand"
        AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="true" ShowFooter="false" PageSize="25" Width="100%" >
        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" CommandItemDisplay="Top">
            <EditFormSettings UserControlName="/UserControl/Grid/EditForm/ClientLocationEdit.ascx" EditFormType="WebUserControl" />
            <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
            <NoRecordsTemplate>
                <div style="padding: 8px 0 8px 5px;">No Records found.</div>
            </NoRecordsTemplate>
            <CommandItemTemplate>
                <div style="margin:2px 5px;">
                    <asp:LinkButton ID="btnAddNewRecord" runat="server" CommandName="InitInsert"><i class="gi gi-circle_plus"></i></i>  Add New Location</asp:LinkButton>
                </div>
            </CommandItemTemplate>
            <Columns>
                <telerik:GridTemplateColumn DataField="Id" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" HeaderText="" UniqueName="Action">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEditLocation" runat="server" CommandName="Edit" style="padding-right:10px;"><i class="gi gi-edit" title="Edit"></i></asp:LinkButton>
                        <asp:LinkButton ID="btnDeleteLocation" runat="server" CommandName="Delete" OnClientClick="javascript:if(!confirm('Confirm Deletion?')){return false;}">
                            <i class="gi gi-circle_remove" title="Delete Location"></i></asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn> 
                <telerik:GridBoundColumn DataField="Name" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="30%" HeaderText="Name" SortExpression="Name" UniqueName="Name" />
                <telerik:GridBoundColumn DataField="addr1" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="30%" HeaderText="Address" SortExpression="addr1" UniqueName="addr1" />
                <telerik:GridBoundColumn DataField="city" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="30%" HeaderText="City" SortExpression="city" UniqueName="city" />
                <telerik:GridBoundColumn DataField="statename" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="30%" HeaderText="State" SortExpression="statename" UniqueName="statename" />
                <telerik:GridBoundColumn DataField="zip" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="30%" HeaderText="Zip" SortExpression="zip" UniqueName="zip" />
                <telerik:GridBoundColumn DataField="Description" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="45%" HeaderText="Description" HeaderTooltip="Description" SortExpression="Description" UniqueName="Description" />
                <telerik:GridBoundColumn DataField="Active" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="15%" HeaderText="Active" SortExpression="Active" UniqueName="Active" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>
