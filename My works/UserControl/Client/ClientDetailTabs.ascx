<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientDetailTabs.ascx.cs" Inherits="UserControl_Client_ClientDetailTabs" %>

<asp:Literal ID="litMessage" runat="server" Visible="false" />
<div class="content-header" style="padding-top: 10px; padding-left: 10px;">
    <h4>
        <p>
            <asp:Label ID ="lblClientNameLabel" runat="server" Text="Client Name:" Font-Bold="true"></asp:Label>
            <asp:Label ID="lblClientName" runat="server"></asp:Label>
        </p>
    </h4>
</div>
<div class="row">
    <div class="col-md-12">
        <telerik:RadTabStrip ID="tabstripModeldetail" runat="server"
            Font-Bold="true" Font-Size="Large" CssClass="RadTabStrip" OnTabClick="tabstripModeldetail_TabClick" RenderMode="Lightweight">
            <Tabs>
                <telerik:RadTab Text="Client" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Billing" Font-Size="Large" />
                <telerik:RadTab Text="Contacts" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Timesheet Projects" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Products" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Renewals" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Projects" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Documents" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="TAM" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Settings" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Servers" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Workstations" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Master Client" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Reports" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Tickets" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Dashboard" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Invoice" Font-Size="Large"></telerik:RadTab>                
                <telerik:RadTab Text="Payments" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Microsoft Licenses" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Supported Users" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Client Notes" Font-Size="Large"></telerik:RadTab>
                <telerik:RadTab Text="Client Services" Font-Size="Large"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>

    </div>
</div>
<style>
    .RadTabStripTop_Default .rtsLast .rtsOut, .RadTabStripTop_Default .rtsLI .rtsAfter, .RadTabStripTop_Default .rtsLevel .rtsLink, .RadTabStripTop_Default .rtsLevel .rtsOut, .RadTabStripBottom_Default .rtsLevel .rtsLink, .RadTabStripBottom_Default .rtsLevel .rtsOut, .RadTabStripTop_Default_Baseline .rtsLevel, .RadTabStripBottom_Default_Baseline .rtsLevel {
        background: none!important;
    }
    .rtsLI .rtsLink {
        border-radius: 4px 4px 0 0!important;
        border-width: 1px!important;
        border-style: solid!important;
        /*background: #fefefe!important;*/ /* Old browsers */
        /*background: -moz-linear-gradient(top, #fefefe 0%, #eaeaea 100%)!important;*/ /* FF3.6-15 */
        /*background: -webkit-linear-gradient(top, #fefefe 0%,#eaeaea 100%)!important;*/ /* Chrome10-25,Safari5.1-6 */
        /*background: linear-gradient(to bottom, #fefefe 0%,#eaeaea 100%)!important;*/ /* W3C, IE10+, FF16+, Chrome26+, Opera12+, Safari7+ */
        filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#fefefe', endColorstr='#eaeaea',GradientType=0 )!important; /* IE6-9 */
        padding: 5px 5px!important;
        border-color: #d1d1d1!important;
    }
    .rtsLI .rtsLink:hover, .rtsLI .rtsLink.selected {
        background-color: #c4c4c4!important;
        background-image: linear-gradient(#d8d8d8,#c4c4c4)!important;
        border-color: #ababab!important;
    }
    .rtsSelected {
        background-color: #ffffff!important;
        background-image: none!important;
    }
</style>
