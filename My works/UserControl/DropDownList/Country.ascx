<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Country.ascx.cs" Inherits="UserControl_DropDownList_Country" %>

<asp:DropDownList ID="ddlCountry" runat="server" />
<asp:RequiredFieldValidator ID="rfvCountry" ControlToValidate="ddlCountry" ErrorMessage="Country is required" ForeColor="red" runat="server" Display="Dynamic">
    <asp:Literal ID="litError" runat="server" Text="*" />
</asp:RequiredFieldValidator>
<asp:PlaceHolder ID="phChosenScript" runat="server" Visible="false">
    <telerik:RadScriptBlock ID="rsbCountry" runat="server">
    <script type='text/javascript'>
        $(document).ready(function () {
            SetChosenCountries_<%=ddlCountry.ClientID%>();
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenCountries_<%=ddlCountry.ClientID%>)
        });

        function SetChosenCountries_<%=ddlCountry.ClientID%>(sender, args) {
            $('#<%=ddlCountry.ClientID%>').chosen({
                allow_single_deselect: true,
                placeholder_text_single: "Select a Country ...",
                width: "100%"
            });
        }
    </script>
    </telerik:RadScriptBlock>   
</asp:PlaceHolder>

