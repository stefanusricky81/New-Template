<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TicketActive.aspx.cs" Inherits="UserControl_Grid_EditForm_TicketActive" Async="true" %>

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
                //GetRadWindow().BrowserWindow.location.href = '';
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
        <h2 style="padding-bottom:10px;">Update Previous Active Ticket</h2>
        
        <table border="0" class="gridEditForm" width="100%" style="width:100%">
            <tr>
                <td style="padding-left:12px;" colspan="2">
                    <asp:ValidationSummary ID="vsTicket" runat="server" CssClass="ValidationSummary" ValidationGroup="vgTicketAdd" />
                     <asp:CustomValidator ID="cvActiveTicket" runat="server" ControlToValidate="tbExternalNotes" ValidationGroup="vgTicketAdd" Display="None"
                        OnServerValidate="cvActiveTicket_ServerValidate" ErrorMessage="You must enter External or Internal notes" ValidateEmptyText="true" />
                </td>
            </tr>
            <asp:MultiView ID="mvMain" runat="server">
                <asp:View ID="viewMainDefault" runat="server">
                    <tr>
                        <th>Ticket:</th>
                        <td><asp:Literal ID="litTicketBeingUpdated" runat="server" /></td>
                    </tr>
                    <tr>
                        <th>Time:</th>
                        <td><asp:Literal ID="litTimeSpent" runat="server" /></td>
                    </tr>
                    <tr>
                        <th>External Notes:</th>
                        <td><asp:TextBox ID="tbExternalNotes" runat="server" Width="400" Rows="6" TextMode="MultiLine" /></td>
                    </tr>
                    <tr>
                        <th>Internal Notes:</th>
                        <td><asp:TextBox ID="tbInternalNotes" runat="server" Width="400" Rows="6" TextMode="MultiLine" /></td>
                    </tr>
                     <tr>
                        <th>Project:</th>
                        <td>
                            <uc:ProjectTaskDDL id="ddlTimesheetProject" runat="server" Width="400" CssClass="dropdown" DefaultValue="" DefaultText="" ValidationGroup="vgTicketAdd"/>
                        </td>
                    </tr>
                    <tr>
                        <th>Activity:</th>
                        <td><uc:TaskActionDDL ID="ddlActivity" runat="server"  Width="400" CssClass="dropdown" DefaultValue="" DefaultText="" ValidationGroup="vgTicketAdd" /></td>
                    </tr>
                </asp:View>
                <asp:View ID="viewMainError" runat="server">
                    <tr>
                        <td colspan="2" style="color:Red;">Error: <asp:Literal ID="litErrorMessage" runat="server" /></td>
                    </tr>
                </asp:View>
            </asp:MultiView>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" OnClick="btnSubmit_Click" CssClass="actionbutton" CausesValidation="true" ValidationGroup="vgTicketAdd"/>&nbsp;
                    <asp:Button ID="btnClose" runat="server" Text="CLOSE" OnClick="btnClose_Click" CssClass="actionbutton2" CausesValidation="false"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
