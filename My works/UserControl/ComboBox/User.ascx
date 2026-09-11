<%@ Control Language="C#" AutoEventWireup="true" CodeFile="User.ascx.cs" Inherits="UserControl_ComboBox_User" %>
<telerik:RadScriptBlock runat="Server" ID="RadScriptBlock1">
    <script type="text/javascript">

        function OnClientItemsRequesting(sender, eventArgs) {
            //min of 3 characters
            if (sender.get_text().length < 3)
                eventArgs.set_cancel(true)

            var context = eventArgs.get_context();
            context["FilterString"] = eventArgs.get_text();
        }
        
    </script>

</telerik:RadScriptBlock>
        
<telerik:RadComboBox ID="rcbUser" runat="server" 
    Skin="BitByBit"
    EnableEmbeddedSkins="false" 
    Height="200"
    Width="305"
    EmptyMessage="[type user name]"
    OnClientItemsRequesting="OnClientItemsRequesting"
    EnableItemCaching="true"
    EnableLoadOnDemand="true" 
    >
    <ExpandAnimation Type="none" />
    <CollapseAnimation Type="none" />
    <WebServiceSettings Path="/WebService/Users.asmx" Method="GetUsers" />
    
</telerik:RadComboBox>

<asp:CustomValidator ID="cvUser" runat="server" ControlToValidate="rcbUser" ValidateEmptyText="true" ErrorMessage="User is required" Visible="false" OnServerValidate="cvUser_ServerValidate">
    <span class="error">*</span>
</asp:CustomValidator>
