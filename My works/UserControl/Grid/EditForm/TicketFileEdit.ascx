<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketFileEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_TicketFileEdit" %>
<div id="divMain" runat="server" style="background-color:#fafbfc; padding:5px 5px 5px 5px; width:99%;">
     <table border="0" class="gridEditForm" width="100%" style="width:100%">
        <tr>
            <td colspan="2">
                <asp:ValidationSummary ID="vsFileUpload" runat="server" 
                        CssClass="ValidationSummary" ValidationGroup="vgFileUpload" />
            </td>
        </tr>
         <tr>
             <td colspan="2">Allowed Files: <em><asp:Literal ID="litAllowedFileExtensions" runat="server" /></em></td>
         </tr>
        <tr>
            <th class="Required" style="width:80px;">File One:</th>
            <td>
                <asp:FileUpload ID="fuOne" runat="server" EnableViewState="true"  />
                <asp:RequiredFieldValidator ID="rfvFileOne" runat="server"
                    ErrorMessage="File One is required" 
                    ControlToValidate="fuOne" ValidationGroup="vgFileUpload">
                    <span class="error">*</span>
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvFuOne" runat="server" ControlToValidate="fuOne" ValidationGroup="vgFileUpload" OnServerValidate="cvFu_ServerValidate"><span class="error">*</span></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th style="width:80px;">File Two:</th>
            <td>
                <asp:FileUpload ID="fuTwo" runat="server" EnableViewState="true" />
                <asp:CustomValidator ID="cvFuTwo" runat="server" ControlToValidate="fuTwo" ValidationGroup="vgFileUpload" OnServerValidate="cvFu_ServerValidate"><span class="error">*</span></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th style="width:80px;">File Three:</th>
            <td>
                <asp:FileUpload ID="fuThree" runat="server" EnableViewState="true" />
                <asp:CustomValidator ID="cvFuThree" runat="server" ControlToValidate="fuThree" ValidationGroup="vgFileUpload" OnServerValidate="cvFu_ServerValidate"><span class="error">*</span></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th style="width:80px;">File Four:</th>
            <td>
                <asp:FileUpload ID="fuFour" runat="server" EnableViewState="true" />
                <asp:CustomValidator ID="cvFuFour" runat="server" ControlToValidate="fuFour" ValidationGroup="vgFileUpload" OnServerValidate="cvFu_ServerValidate"><span class="error">*</span></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center">
                <asp:button id="btnAdd" text="UPLOAD" runat="server" CssClass="actionbutton" 
                    ValidationGroup="vgFileUpload" CommandName="PerformInsert" />
                    &nbsp;
                    <asp:Button ID="btnClose" Text="CLOSE" runat="server" CssClass="actionbutton2" 
                        CommandName="Cancel" />
            </td>
        </tr>
    </table>
</div>