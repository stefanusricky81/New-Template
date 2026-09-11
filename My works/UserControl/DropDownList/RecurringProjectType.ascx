<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecurringProjectType.ascx.cs" Inherits="UserControl_DropDownList_RecurringProjectType" %>
<asp:DropDownList ID="ddlRecurringProjectType" runat="server" Width="100" Height="20" />

<asp:RequiredFieldValidator ID="rfvRecurringProjectType" runat="server" ControlToValidate="ddlRecurringProjectType"
    ErrorMessage="Recurring Project Type is required" Visible="false">
    <span class="error">*</span>
</asp:RequiredFieldValidator>
