<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketTags.ascx.cs" Inherits="UserControl_Grid_TicketTags" %>

 <table border="0" class="gridEditForm" width="100%" style="text-align:left;">
    <tr>
        <td colspan="2">
            <asp:ValidationSummary ID="vsAddTag" runat="server" CssClass="ValidationSummary" ValidationGroup="vgAddTag" />
        </td>
    </tr>
    <tr>
        <th style="width:100px;">Add Tag:
             <asp:CustomValidator ID="cvAddTag" runat="server"
                ControlToValidate="tbTagText" ValidationGroup="vgAddTag"
                ErrorMessage="Tag already exists for ticket" 
                OnServerValidate="cvAddTag_ServerValidate" ValidateEmptyText="true">
                <span class="error">*</span>
            </asp:CustomValidator>
        </th>
        <td>
            <asp:TextBox ID="tbTagText" runat="server" Width="200" />
            
            <asp:PlaceHolder ID="phOrText" runat="server"><em>or</em></asp:PlaceHolder>
            <uc:UserTicketTagsDDL ID="ucUserTicketTagsDDL" runat="server" DisplayDefaultValue="true" DefaultText="-- select --"/>&nbsp;
            <asp:Button ID="btnAddTag" runat="server" Text="ADD" OnClick="btnAddTag_Click" CssClass="actionbutton" ValidationGroup="vgAddTag"/>
        </td>
    </tr>
    <tr>
        <th style="width:100px;"></th>
        <td>
            <asp:Label ID="lblPublic" runat="server" Text="Public" ToolTip="Public - tag is available for all employees to use"/>:&nbsp;<asp:CheckBox ID="chkTagPublic" runat="server" />&nbsp;
            <asp:Label ID="lblClientPublic" runat="server" Text="Client Public" ToolTip="Client And Public - tag is available for all clients and employees to use"/>:&nbsp;<asp:CheckBox ID="chkTagClientPublic" runat="server" />
        </td>
    </tr>
</table>

<br /><br />

<telerik:RadGrid ID="rgTicketTags" runat="server"
    OnNeedDataSource="rgTicketTags_NeedDataSource"
    OnItemDataBound="rgTicketTags_ItemDataBound"
    OnItemCreated="rgTicketTags_ItemCreated"
    OnItemCommand="rgTicketTags_ItemCommand"
    Skin="BitByBit2" EnableEmbeddedSkins="false" 
    ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false"
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="false" 
    MasterTableView-NoMasterRecordsText="No Records found." PageSize="20">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="false">
        <Selecting AllowRowSelect="false" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
        
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="fixed" Width="100%" DataKeyNames="TaggedTicketValueId,Id">
        <Columns>
            <telerik:GridBoundColumn UniqueName="Name" DataField="Name" 
                SortExpression="Name" HeaderText="Tag"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50%"
                ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="Created" DataField="TaggedTicketValueCreated" 
                SortExpression="TaggedTicketValueCreated" HeaderText="Date" DataFormatString="{0:MM/dd/yy HH:mm}"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="25%" 
                ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridEditCommandColumn UniqueName="Action" HeaderText="Action"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="25%" 
                ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right">
            </telerik:GridEditCommandColumn>
        </Columns>

    </MasterTableView>
    
</telerik:RadGrid>