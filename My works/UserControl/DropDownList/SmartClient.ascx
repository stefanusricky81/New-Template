<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SmartClient.ascx.cs" Inherits="UserControl_DropDownList_SmartClient" %>
<telerik:RadScriptBlock runat="Server" ID="RadScriptBlock1">

    <script type="text/javascript">

        function OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            context["FilterString"] = eventArgs.get_text();

        }

    </script>
</telerik:RadScriptBlock>
<link href="../../Css/SmartClient.css" rel="stylesheet" />
<telerik:RadComboBox RenderMode="Lightweight" ID="rcbClient" runat="server"
    EmptyMessage="Select a Client" MarkFirstMatch="true" EnableLoadOnDemand="true" OnClientItemsRequesting="OnClientItemsRequesting"
    Skin="Metro" OnSelectedIndexChanged="rcbClient_SelectedIndexChanged">
    <CollapseAnimation Type="OutExpo" />
    <WebServiceSettings Path="/WebService/Clients.asmx" Method="GetClients" />
</telerik:RadComboBox>
<asp:Literal ID="litDebug" runat="server" />
<asp:CustomValidator ID="cvClient" runat="server" ControlToValidate="rcbClient" ValidateEmptyText="true"
    ErrorMessage="Client is required" Visible="false" OnServerValidate="cvClient_ServerValidate">
    <span class="error">*</span>
</asp:CustomValidator>
