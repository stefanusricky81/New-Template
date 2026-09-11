<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_ProductEdit" %>

<div id="divMain" runat="server" style="background-color:#fafbfc; padding:5px 5px 5px 5px; width:99%;">
<table border="0" class="gridEditForm" width="100%" style="width:100%">
     <tr>
        <td colspan="2" style="text-align:center;">
            <asp:MultiView ID="mvButtonTop" runat="server">
                <asp:View ID="viewGridTop" runat="server">
                    <asp:button id="btnUpdateTop" text="UPDATE" runat="server" CssClass="actionbutton" ValidationGroup="vgProduct" CommandName="Update" />
                    <asp:button id="btnInsertTop" text="ADD" runat="server" CssClass="actionbutton" ValidationGroup="vgProduct" CommandName="PerformInsert" />&nbsp;
                    <asp:Button ID="btnCloseTop" Text="CLOSE" runat="server" CssClass="actionbutton2" CommandName="Cancel"/>
                </asp:View>
                <asp:View ID="viewPageTop" runat="server"></asp:View>
            </asp:MultiView>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:ValidationSummary ID="vsProduct" runat="server" CssClass="ValidationSummary" ValidationGroup="vgProduct" />
        </td>
    </tr>
    <asp:PlaceHolder ID="phProductId" runat="server">
        <tr>
            <th style="width:135px;">Id:</th>
            <td>
                <asp:HyperLink ID="hlProductId" runat="server" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <th style="width:135px;" class="Required">Name:</th>
        <td>
            <asp:TextBox ID="tbName" runat="server" CssClass="textbox" MaxLength="80" Width="400"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName" ErrorMessage="Name is required" ValidationGroup="vgProduct">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cvName" runat="server" ControlToValidate="tbName" ErrorMessage="Name is in use" ValidationGroup="vgProduct" OnServerValidate="cvName_ServerValidate">
                <span class="error">*</span>
            </asp:CustomValidator>
        </td>
    </tr>
     <tr>
        <th style="width:135px;">Description:</th>
        <td>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="textbox" Width="400" Height="200" TextMode="MultiLine" />
        </td>
    </tr>
    <tr>
        <th style="width:135px; vertical-align:top;">Active:</th>
        <td>
            <asp:CheckBox ID="chkActive" runat="server" />
        </td>
    </tr>
    <asp:PlaceHolder ID="phExistingFields" runat="server">
        <tr>
            <th style="width:135px;">Created:</th>
            <td><asp:Literal ID="litCreated" runat="server" /></td>
        </tr>
        <tr>
            <th style="width:135px;">Last Updated:</th>
            <td><asp:Literal ID="litLastUpdated" runat="server" /></td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <td colspan="2" style="text-align:center;">
            <asp:MultiView ID="mvButton" runat="server">
                <asp:View ID="viewGrid" runat="server">
                    <asp:button id="btnUpdate" text="UPDATE" runat="server" CssClass="actionbutton" ValidationGroup="vgProduct" CommandName="Update" />
                    <asp:button id="btnInsert" text="ADD" runat="server" CssClass="actionbutton" ValidationGroup="vgProduct" CommandName="PerformInsert" />&nbsp;
                    <asp:Button ID="btnClose" Text="CLOSE" runat="server" CssClass="actionbutton2" CommandName="Cancel"/>
                </asp:View>
                <asp:View ID="viewPage" runat="server">
                    <asp:Button ID="btnUpdatePage" runat="server" Text="UPDATE" CssClass="actionbutton" ValidationGroup="vgProduct" OnClick="btnUpdatePage_Click" />
                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
</table>    
</div>