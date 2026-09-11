<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="Csat.aspx.cs" Inherits="Reports_Csat" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<%@ Register TagPrefix="ddl" TagName="CsatCompany" Src="~/UserControl/DropDownList/CsatCompany.ascx" %>
<%@ Register TagPrefix="ddl" TagName="CsatTeamMember" Src="~/UserControl/DropDownList/CsatTeamMember.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <telerik:radscriptblock id="RadScriptBlock1" runat="server">
    <script type="text/javascript">
        function OnRequestStart(e, sender) {
            var theRegexp = new RegExp("\.btnExport$", "ig");
            if (sender.EventTarget.match(theRegexp))
                sender.EnableAjax = false;
        }
    </script>
</telerik:radscriptblock>
    <style>
    .RadGrid_3b .rgRow td,.RadGrid_3b .rgAltRow td,.RadGrid_3b .rgEditRow td,.RadGrid_3b .rgFooter td,.RadGrid_3b .rgFilterRow td,.RadGrid_3b .rgResizeCol,.RadGrid_3b .rgGroupHeader td
    {
	    padding:4px 5px !important;
	    white-space: nowrap;
        overflow:hidden;
    }
    .RadGrid td.rgPagerCell { padding: 10px; }
    .RadGrid_3b .rgPagerCell .rgPagerLabel { padding: 7px 0 7px;}
    .RadDropDownList .rddlInner {height:25px; margin-top:6px;}
</style>
<asp:PlaceHolder ID="phTabletPagerCss" runat="server" Visible="false">
    <style>
        .RadGrid_3b .rgPagerCell .rgNumPart span { width:25px; }
        .RadGrid_3b .rgPagerCell .rgPagePrev, .rgPageFirst, .rgPageNext, .rgPageLast {display:none; } 
    </style>
</asp:PlaceHolder>
<div class="content-header">
    <div class="header-section">
        <h1>CSAT Report</h1>
    </div>
</div>
<telerik:RadAjaxPanel ID="rapReport" runat="server" LoadingPanelID="ralpReport" EnableAJAX="true" ClientEvents-OnRequestStart="OnRequestStart">
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <div class="block">
            <div class="block-title">
                <h2><strong>Search Criteria</strong></h2>
                <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="validationSummary" ValidationGroup="vgSearch" />
            </div>
            <div class="row">
                <div class="form-group col-sm-4">
                <label>Start Date</label>
                    <uc:DatePicker ID="ucStartDate" runat="server" IsRequired="false" PlaceHolderText="Enter Start Date ... " />    
                </div>
                 <div class="form-group col-sm-4">
                     <label>End Date</label>
                     <uc:DatePicker ID="ucEndDate" runat="server" IsRequired="false" PlaceHolderText="Enter End Date ... " />
                     <asp:CustomValidator ID="cvDates" runat="server" ControlToValidate="txtValidator" ValidateEmptyText="true" ValidationGroup="vgSearch" 
                         ErrorMessage="End Date must be greater than Start Date" Display="None" OnServerValidate="cvDates_ServerValidate" />
                 </div>
                <div class="form-group col-sm-4">
                    <label>Rating</label>
                    <asp:ListBox ID="lbRating" runat="server" SelectionMode="Multiple">
                        <asp:ListItem Text="Bad (2)" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Okay (3)" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Great (4)" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Amazing (5)" Value="5"></asp:ListItem>
                    </asp:ListBox>
                    <asp:Literal ID="litRatingJs" runat="server" Visible="false"/>
                </div>
                <div style="display:none;">
                    <asp:TextBox ID="txtValidator" runat="server" MaxLength="1" Enable="false"/>
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>Company</label>
                    <ddl:CsatCompany ID="ddlCsatCompany" runat="server" IsRequired="false" DisplayChosenScript="true" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Team Member</label>
                    <ddl:CsatTeamMember ID="ddlCsatTeamMember" runat="server" IsRequired="false" DisplayChosenScript="true" />
                </div>
            </div>
            <div class="form-group form-actions" style="padding-top:20px;">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgSearch"><i class="hi hi-search"></i> Search</asp:LinkButton>
                <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
            </div>
        </div>
        <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="false">
            <asp:Literal ID="litDebug" runat="server" />
            <asp:Literal ID="litMessage" runat="server" />
            <div class="block" style="padding-bottom: 20px;">
                <div class="block-title">
                    <h2><strong>Search Results</strong></h2>
                </div>
                <div class="table-responsive" style="padding-bottom: 10px">
                    <telerik:RadGrid ID="rgReport" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                        OnNeedDataSource="rgReport_NeedDataSource" 
                        OnItemDataBound="rgReport_ItemDataBound" 
                        OnSortCommand="rgReport_SortCommand"
                        OnPreRender="rgReport_PreRender" 
                        AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" AllowCustomPaging="true" ShowFooter="false" Width="100%" PageSize="100">
                        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="InternalResponseId" Width="100%" CommandItemDisplay="Top">
                            <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="10,25,50,100" PageSizeControlType="RadDropDownList" Position="Bottom"/>
                            <NoRecordsTemplate>
                                <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                            </NoRecordsTemplate>
                            <CommandItemTemplate>
                                <div style="margin:5px 5px;">
                                    <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" Visible="true"><i class="fi fi-csv" title="Export To CSV"></i>  Export To CSV</asp:LinkButton>
                                </div>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="SimplesatCreated" DataField="SimplesatCreated" SortExpression="SimplesatCreated" HeaderText="Date" HeaderTooltip="Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                <telerik:GridBoundColumn UniqueName="CustomerName" DataField="CustomerName" SortExpression="CustomerName" HeaderText="Customer Name" HeaderTooltip="Customer Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                <telerik:GridBoundColumn UniqueName="Company" DataField="Company" SortExpression="Company" HeaderText="Company" HeaderTooltip="Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="14%"/>
                                <telerik:GridBoundColumn UniqueName="TeamMembers" DataField="TeamMembers" SortExpression="TeamMembers" HeaderText="Team Member" HeaderTooltip="Team Member" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                <telerik:GridBoundColumn UniqueName="TicketId" DataField="TicketId" SortExpression="TicketId" HeaderText="Ticket #" HeaderTooltip="Ticket #" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                <telerik:GridBoundColumn UniqueName="Choice" DataField="Choice" SortExpression="Choice" HeaderText="Rating" HeaderTooltip="Choice" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                <telerik:GridBoundColumn UniqueName="ChoiceNoFormatting" DataField="Choice" SortExpression="Choice" HeaderText="Rating" HeaderTooltip="Choice" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="FollowUpAnswer" DataField="FollowUpAnswer" SortExpression="FollowUpAnswer" HeaderText="Follow Up Answer" HeaderTooltip="Follow Up Answer" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40%"/>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" />
                    </telerik:RadGrid>
                </div>
            </div>
        </asp:PlaceHolder>
    </asp:Panel>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="ralpReport" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" Runat="Server"></asp:Content>
