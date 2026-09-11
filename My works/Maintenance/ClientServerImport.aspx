<%@ Page Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientServerImport.aspx.cs" Inherits="Maintenance_ClientServerImport" Title="Bit By Bit Intranet - Client Server Import" %>
<%@ Register TagPrefix="uc" TagName="Client" Src="~/UserControl/Typeahead/Client.ascx" %>
<%@ Reference VirtualPath="~/UserControl/Grid/EditForm/ClientServerEdit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">


    <div class="content-header">
    <div class="header-section">
        <h1>Client Server Import</h1>
    </div>
</div>
<telerik:RadAjaxPanel ID="rapTimesheet" runat="server" LoadingPanelID="ralpServer" EnableAJAX="true">
    <asp:Literal ID="litMessage" runat="server" />
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnImport">
        <asp:Literal ID="litDebug" runat="server" />
        <div class="block" style="display:none;">
            <div class="form-group form-actions">
                <asp:LinkButton ID="btnImport" runat="server" OnClick="btnImport_Click" CssClass="btn btn-sm btn-primary" Visible="false"><i class="hi hi-import"></i> Import</asp:LinkButton>
            </div>
        </div>
    </asp:Panel>

</telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="ralpServer" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>
</asp:Content>

