<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SmartClient.ascx.cs" Inherits="UserControl_ComboBox_SmartClient" %>

<telerik:RadScriptBlock runat="Server" ID="RadScriptBlock1">

    <script type="text/javascript">

        function OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            context["FilterString"] = eventArgs.get_text();

        }

    </script>

</telerik:RadScriptBlock>
<div class="navbar-form-custom">
    <div class="form-group">
        <telerik:RadComboBox ID="rcbClient" runat="server"
             CssClass="form-control"  AllowCustomText="true"
    ShowMoreResultsBox="false" EnableLoadOnDemand="true" EnableEmbeddedSkins="false" 
            Height="200"
            AutoPostBack="true"
            EmptyMessage="[type San name]"
            OnClientItemsRequesting="OnClientItemsRequesting"
            EnableItemCaching="true"
            
            OnSelectedIndexChanged="rcbClient_SelectedIndexChanged">
            <ExpandAnimation Type="OutQuart" />
            <CollapseAnimation Type="none" />
            <WebServiceSettings Path="/WebService/Clients.asmx" Method="GetClients" />

        </telerik:RadComboBox>
        <div runat="server" id="divQuickLinks" visible="false">
            <a href="#">Sandip</a>

        </div>
    </div>
</div>
<asp:Literal ID="litDebug" runat="server" />
<asp:CustomValidator ID="cvClient" runat="server" ControlToValidate="rcbClient" ValidateEmptyText="true"
    ErrorMessage="Client is required" Visible="false" OnServerValidate="cvClient_ServerValidate">
    <span class="error">*</span>
</asp:CustomValidator>
