<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BackupMonitorEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_BackupMonitorEdit" %>
<%@ Register TagPrefix="uc" TagName="BackupMonitorStatusTypeRBL" Src="~/UserControl/RadioButtonList/BackupMonitorStatus.ascx" %>

<div id="divMain" runat="server" style="padding:5px 5px 5px 5px; width:99%;">

<table border="1" class="gridEditForm" style="width:100%;">
    <tr>
        <td colspan="8">
            <asp:ValidationSummary ID="vsBackupMonitor" runat="server" 
                CssClass="ValidationSummary" ValidationGroup="vgBackupMonitor" Visible="false" />
        </td>
    </tr>
    <tr style="background: #D6DFEF;padding-top: 5px;padding-bottom: 5px">
        <td style="width:8%">&nbsp;</td>
        <td style="width:8%"><b>Client</b></td>
        <td style="width:8%"><b>Server</b></td>
        <td style="width:8%"><b>BU Set</b></td>
        <td style="width:8%;"><b>BU Server</b></td>
        <td style="width:20%"><b>BU Description</b></td>
        <td style="width:5%;"><b>Status</b></td>
        <td><b>Comment</b></td>
    </tr>
    <tr>
        <td style="width:8%;text-align:center;" valign="top">
            <asp:MultiView ID="mvButton" runat="server">
            
                <asp:View ID="viewGrid" runat="server">
                    <asp:button id="btnUpdate" text="UPDATE" runat="server" 
                        CssClass="actionbutton" ValidationGroup="vgBackupMonitor" CommandName="Update">
                    </asp:button>
                    <br />
                    <asp:Button ID="btnClose" Text="CANCEL" runat="server" 
                        CssClass="actionbutton2" CommandName="Cancel" Visible="true"/>
                </asp:View>
                
                <asp:View ID="viewPage" runat="server">
                    <asp:Button ID="btnUpdatePage" runat="server" Text="UPDATE" 
                        CssClass="actionbutton" ValidationGroup="vgBackupMonitor" 
                        OnClick="btnUpdatePage_Click" />
                </asp:View>
                
            </asp:MultiView>
        </td>
        <td style="width:8%;" valign="top"><asp:Label ID="lblClient" runat="server" /></td>
        <td style="width:8%;" valign="top"><asp:Label ID="lblServer" runat="server" /></td>
        <td style="width:8%;" valign="top"><asp:Label ID="lblBackupSet" runat="server" /></td>
        <td style="width:8%;" valign="top"><asp:Label ID="lblBackupSetBackupServer" runat="server" /></td>
        <td style="width:20%;" valign="top"><asp:Label ID="lblBackupDescription" runat="server" /></td>        
        <td style="width:5%;" valign="top">
            <uc:BackupMonitorStatusTypeRBL ID="ucBackupMonitorStatusRBL" runat="server" Width="120" IsRequired="true" DisplayDefaultValue="false" ValidationGroup="vgBackupMonitor" />
        </td>        
        <td style="width:35%;" valign="top">
            <asp:TextBox ID="tbComment" runat="server" CssClass="textbox" Width="400"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvComment" runat="server" ControlToValidate="tbComment" 
                ErrorMessage="Comment is required for Unsuccessful status" ValidationGroup="vgBackupMonitor" Enabled="false">
                <span class="error">* Comment is required</span>
            </asp:RequiredFieldValidator>
        </td>

    <asp:PlaceHolder ID="phExistingFields" runat="server" Visible="false">
                <asp:Literal ID="litCreated" runat="server" />
                <asp:Literal ID="litLastUpdated" runat="server" />
    </asp:PlaceHolder>


    </tr>
</table>

</div>