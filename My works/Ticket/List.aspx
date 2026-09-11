<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="List.aspx.cs" Inherits="Ticket_List" %>
<%@ Register TagPrefix="grid" TagName="Ticket" Src="~/UserControl/Grid/TicketResponsive.ascx" %>
<%@ Register TagPrefix="lb" TagName="TicketView" Src="~/UserControl/ListBox/TicketView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <link rel="Stylesheet" type="text/css" href="../RadControls/Skin/ToolTip/ToolTip.BitByBit.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
<telerik:RadScriptBlock ID="rsbMain" runat="server">
    <script type="text/javascript">
        function OnRequestStart(e, sender) {
            var theRegexp = new RegExp("\.btnExport$|\.btnBulkClosed$", "ig");
            if (sender.EventTarget.match(theRegexp))
                sender.EnableAjax = false;
        }
    </script>
</telerik:RadScriptBlock>
<div class="content-header">
    <div class="header-section">
        <h1>My Views</h1>
    </div>
</div>
<telerik:RadAjaxPanel ID="rapTicket" runat="server" LoadingPanelID="ralpTicket" EnableAJAX="true" ClientEvents-OnRequestStart="OnRequestStart">
     <div class="breadcrumb breadcrumb-top">
        <a href="Search.aspx">Ticket Search</a> | 
        <a href="Add2.aspx">Add Ticket</a>
    </div>
    <asp:Panel ID="pnlContainer" runat="server">
        <asp:PlaceHolder ID="phSearchResults" runat="server">
            <asp:Literal ID="litDebug" runat="server" />
            <asp:Literal ID="litMessage" runat="server" />
            <div class="block" style="padding-bottom: 20px;">
                <div class="row" style="padding-bottom:30px;">
                    <div class="form-group col-sm-12">
                        <div class="input-group">
                            <lb:TicketView ID="lbTicketView" runat="server" ViewType="List" ResponsivePage="true" />
                            <span class="input-group-btn">
                                <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-sm btn-primary"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
                            </span>
                        </div>
                    </div>
                </div>
                <asp:PlaceHolder ID="phGrid1" runat="server" Visible="true">
                    <div class="block-title">
                        <h2><strong><asp:Literal ID="litGrid1Header" Text="OK" runat="server" /></strong></h2>
                    </div>
                    <grid:Ticket ID="gridTicket1" runat="server" BulkClosedTickets="true" />
                </asp:PlaceHolder>
            </div>
            <asp:PlaceHolder ID="phGrid2" runat="server" Visible="false">
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong><asp:Literal ID="litGrid2Header" runat="server" /></strong></h2>
                    </div>
                    <grid:Ticket ID="gridTicket2" runat="server" BulkClosedTickets="true" />
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phGrid3" runat="server" Visible="false">
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong><asp:Literal ID="litGrid3Header" runat="server" /></strong></h2>
                    </div>
                    <grid:Ticket ID="gridTicket3" runat="server" BulkClosedTickets="true" />
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phGrid4" runat="server" Visible="false">
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong><asp:Literal ID="litGrid4Header" runat="server" /></strong></h2>
                    </div>
                    <grid:Ticket ID="gridTicket4" runat="server" BulkClosedTickets="true" />
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phGrid5" runat="server" Visible="false">
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong><asp:Literal ID="litGrid5Header" runat="server" /></strong></h2>
                    </div>
                    <grid:Ticket ID="gridTicket5" runat="server" BulkClosedTickets="true" />
                </div>
            </asp:PlaceHolder>
        </asp:PlaceHolder>
    </asp:Panel>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="ralpTicket" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" Runat="Server"></asp:Content>