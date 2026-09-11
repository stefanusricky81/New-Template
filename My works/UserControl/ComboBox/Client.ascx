<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Client.ascx.cs" Inherits="UserControl_ComboBox_Client" %>

<telerik:RadScriptBlock runat="Server" ID="RadScriptBlock1">

    <script type="text/javascript">

        function OnClientItemsRequesting(sender, eventArgs)
        {
            var context = eventArgs.get_context();
            context["FilterString"] = eventArgs.get_text();

        }
        
    </script>

</telerik:RadScriptBlock>
        
<telerik:RadComboBox ID="rcbClient" runat="server" 
    Skin="BitByBit"
    EnableEmbeddedSkins="false" 
    Height="200"
    Width="305"
    EmptyMessage="[type client name]"
    OnClientItemsRequesting="OnClientItemsRequesting"
    EnableItemCaching="true"
    EnableLoadOnDemand="true" 
    >
    <ExpandAnimation Type="none" />
    <CollapseAnimation Type="none" />
    <WebServiceSettings Path="/WebService/Clients.asmx" Method="GetClients" />
    
</telerik:RadComboBox>
<asp:Literal ID="litDebug" runat="server" />
<asp:CustomValidator ID="cvClient" runat="server" ControlToValidate="rcbClient" ValidateEmptyText="true"
    ErrorMessage="Client is required" Visible="false" OnServerValidate="cvClient_ServerValidate">
    <span class="error">*</span>
</asp:CustomValidator>
