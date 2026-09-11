<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Base.master" AutoEventWireup="true" CodeFile="AccountType.aspx.cs" Inherits="Maintenance_AccountType" %>
<%@ Register TagPrefix="grid" TagName="Product" Src="~/UserControl/Grid/Product.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <h2>Account Type Maintenance</h2>

    <asp:Panel ID="pnlGrid" runat="server">
        <grid:Product ID="gridProduct" runat="server" />
    </asp:Panel>
    
    <telerik:RadAjaxManagerProxy ID="rampProduct" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="pnlGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="ralpProduct" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    
    <telerik:RadAjaxLoadingPanel ID="ralpProduct" runat="server" Transparency="25" BackColor="#E0E0E0">
        <img alt="Loading..." src="/Images/loading.gif" style="border: 0px; padding-top:90px;" />
    </telerik:RadAjaxLoadingPanel>
</asp:Content>

