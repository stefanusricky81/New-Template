<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Ticket.ascx.cs" Inherits="UserControl_ComboBox_Ticket" %>
        
<telerik:RadScriptBlock runat="Server" ID="RadScriptBlock1">
    <script type="text/javascript">

        function OnClientItemsRequesting(sender, eventArgs) {
            if (sender.get_text().length < 4) {
                eventArgs.set_cancel(true);
            }
        }
        
    </script>
</telerik:RadScriptBlock>

<telerik:RadComboBox ID="rcbTicket" runat="server" 
    Skin="BitByBit"
    EnableEmbeddedSkins="false" 
    Height="200"
    Width="305"
    EmptyMessage="[type ticket id or summary or client name - min 4 characters]"
    OnItemsRequested="rcbTicket_ItemsRequested"
    OnItemDataBound="rcbTicket_ItemDataBound"
    OnClientItemsRequesting="OnClientItemsRequesting"
    EnableItemCaching="true"
    EnableLoadOnDemand="true"
    MinFilterLength="3"
    >    
</telerik:RadComboBox>

<asp:CustomValidator ID="cvTicket" runat="server" ControlToValidate="rcbTicket" ValidateEmptyText="true"
    ErrorMessage="Ticket is required" Visible="false" OnServerValidate="cvTicket_ServerValidate">
    <span class="error">*</span>
</asp:CustomValidator>
