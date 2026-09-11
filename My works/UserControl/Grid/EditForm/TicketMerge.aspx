<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TicketMerge.aspx.cs" Inherits="UserControl_Grid_EditForm_TicketMerge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ticket Merge</title>
    <link rel="Stylesheet" type="text/css" href="/Css/Main.css" />
    <style type="text/css">   
         
    </style> 
    <telerik:radscriptblock id="RadScriptBlock1" runat="server">
        <script type="text/javascript">

            function Close() {
                GetRadWindow().BrowserWindow.location.reload();
                GetRadWindow().close();
            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)

                return oWindow;
            }

        </script>
    </telerik:radscriptblock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="rsmMain" runat="server" EnablePartialRendering="true" />
    <div style="text-align:left; padding:10px 10px 0 10px;">
        <h2 style="padding-bottom:10px;">Ticket Merge</h2>

        <table border="0" class="gridEditForm" width="100%" style="width:100%">
            <tr>
                <td style="padding-left:12px;">
                    <asp:ValidationSummary ID="vsTicket" runat="server" CssClass="ValidationSummary" ValidationGroup="vgTicketMerge" />
                </td>
            </tr>
            <tr>
                <td>
                    The following <asp:Literal ID="litTicketsText" runat="server" Text="ticket" /> will be merged: <asp:Literal ID="litIds" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                     Merge Master Ticket #:
                    <asp:TextBox ID="tbMasterTicketId" runat="server" Width="100" />
                    <asp:RequiredFieldValidator ID="rvMasterTicketId" runat="server" ControlToValidate="tbMasterTicketId" 
                        ErrorMessage="Merge Master Ticket # is required" ValidationGroup="vgTicketMerge">
                        <span class="error">*</span>
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="revMasterTicketId" 
                        ControlToValidate="tbMasterTicketId" ValidationExpression="\d+"
                        ErrorMessage="Invalid Merge Master Ticket #" ValidationGroup="vgTicketMerge">
                        <span class="error">*</span>
                    </asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="cvMasterTicketId" runat="server"
                            ControlToValidate="tbMasterTicketId" ValidationGroup="vgTicketMerge"
                            OnServerValidate="cvMasterTicketId_ServerValidate" ErrorMessage="Merge Master Ticket # could not be fetched">
                            <span class="error">*</span>
                        </asp:CustomValidator>
                </td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" OnClick="btnSubmit_Click" CssClass="actionbutton" CausesValidation="true" ValidationGroup="vgTicketMerge"/>
                    <asp:Button ID="btnClose" runat="server" Text="CLOSE" OnClick="btnClose_Click" CssClass="actionbutton2" CausesValidation="false"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
