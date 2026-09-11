<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientProduct.ascx.cs" Inherits="UserControl_ComboBox_ClientProduct" %>

<telerik:RadScriptBlock runat="Server" ID="RadScriptBlock1">

    <script type="text/javascript">

        function OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            context["FilterString"] = eventArgs.get_text();

        }
        
    </script>


</telerik:RadScriptBlock>
        
<telerik:RadComboBox ID="_rcbProduct" runat="server" Skin="BitByBit" AutoPostBack="false" 
    EnableEmbeddedSkins="false" EmptyMessage="[type product name]" HighlightTemplatedItems="true"
    AllowCustomText="true" Width="450" Height="200" OnClientItemsRequesting="OnClientItemsRequesting"
    DropDownCssClass="multipleRowsColumns" DropDownWidth="950px">
    <ItemTemplate>
        <div>
            <asp:CheckBox runat="server" ID="chkProduct"/>
            <asp:Label runat="server" ID="lblProduct" AssociatedControlID="chkProduct">
                <%# Eval("Product")%>
            </asp:Label>
        </div>
    </ItemTemplate>
</telerik:RadComboBox>
