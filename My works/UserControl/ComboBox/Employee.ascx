<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Employee.ascx.cs" Inherits="UserControl_ComboBox_Employee" %>

<telerik:RadComboBox ID="rcbEmployee" runat="server" Skin="BitByBit" AllowCustomText="false" 
    ShowMoreResultsBox="false" EnableLoadOnDemand="true" EnableEmbeddedSkins="false"
    OnItemsRequested="rcbEmployee_ItemsRequested" Width="305" EmptyMessage="[type employee name]"
    >
</telerik:RadComboBox>

<asp:CustomValidator ID="cvEmployee" runat="server" ControlToValidate="rcbEmployee" ValidateEmptyText="true"
    ErrorMessage="Employee is required" Visible="false" OnServerValidate="cvEmployee_ServerValidate">
    <span class="error">*</span>
</asp:CustomValidator>