<%@ Page Title="Bit By Bit Intranet | Monthly Project Analysis" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ProjectMonthly.aspx.cs" Inherits="Reports_ProjectMonthly" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Employee" Src="~/UserControl/DropDownList/Employee.ascx" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<%@ Register TagPrefix="grid" TagName="Timesheet" Src="~/UserControl/Grid/TimesheetResponsive.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        function CloseTimesheetModal() {
            $('#modal-project-hours').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        }
        function ShowimesheetModal() {
            $('.modal-backdrop').remove();
            $('#modal-project-hours').modal('show');
            $('body').addClass('modal-open');
        }
        function OnRequestStart(e, sender) {
            var theRegexp = new RegExp("\.btnExport$", "ig");
            if (sender.EventTarget.match(theRegexp))
                sender.EnableAjax = false;
        }
    </script>
</telerik:RadCodeBlock>
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
    .modal-lg { margin: auto; width: 80%; }
</style>
<asp:PlaceHolder ID="phTabletPagerCss" runat="server" Visible="false">
    <style>
        .RadGrid_3b .rgPagerCell .rgNumPart span { width:25px; }
        .RadGrid_3b .rgPagerCell .rgPagePrev, .rgPageFirst, .rgPageNext, .rgPageLast {display:none; } 
    </style>
</asp:PlaceHolder>
<div class="content-header">
    <div class="header-section">
        <h1>Monthly Project Analysis</h1>
    </div>
</div>
<telerik:RadAjaxPanel ID="rapReport" runat="server" LoadingPanelID="ralpReport" EnableAJAX="true" ClientEvents-OnRequestStart="OnRequestStart">
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <asp:Literal ID="litDebug" runat="server" />
        <div class="block">
            <div class="block-title">
                <h2><strong>Search Criteria</strong></h2>
                <asp:ValidationSummary ID="vsReport" runat="server" CssClass="validationSummary" ValidationGroup="vgReport" />
            </div>
            <div class="row">
                  <div class="form-group col-sm-4">
                    <label>Client</label>
                    <ddl:Client ID="ddlClient" runat="server" IsRequired="false" DisplayChosenScript="true" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Salesperson</label>
                    <ddl:Employee ID="ddlEmployee" runat="server" IsRequired="false" DisplayChosenScript="true" EmployeeTypeToDisplay="Sales" UseEmployeeIdAsDataValue="true" SetSize="false" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Project Status</label>
                    <asp:DropDownList ID="ddlProjectStatus" runat="server">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Open" Value="O" Selected="True" />
                        <asp:ListItem Text="Closed" Value="C" />
                    </asp:DropDownList>
                </div>
            </div>
             <div class="row">
                <div class="form-group col-sm-4">
                    <label>Year</label>
                    <asp:DropDownList ID="ddlYear" runat="server" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Month</label>
                    <asp:DropDownList ID="ddlMonth" runat="server" />
                </div>
                 <div class="form-group col-sm-4">
                    <label>Client Charge Status</label>
                    <asp:DropDownList ID="ddlClientStatus" runat="server">
                        <asp:ListItem Text="All Clients" Value="" Selected="True" />
                        <asp:ListItem Text="Additional Charge Clients Only" Value="A"/>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group form-actions" style="padding-top:20px;">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" CausesValidation="true" ValidationGroup="vgReport"><i class="hi hi-search"></i> Search</asp:LinkButton>
                <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
            </div>
            <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="false">
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong>Search Results</strong></h2>
                    </div>
                    <div class="table-responsive">
                        <telerik:RadGrid ID="rgReport" runat="server"  Skin="3b" EnableEmbeddedSkins="false"
                            OnNeedDataSource="rgReport_NeedDataSource" 
                            OnItemDataBound="rgReport_ItemDataBound" 
                            OnPreRender="rgReport_PreRender"  
                            OnItemCommand="rgReport_ItemCommand"
                            AllowSorting="true" AllowPaging="false" AutoGenerateColumns="false" AllowMultiRowSelection="false"
                            AutoGenerateDeleteColumn="false" AutoGenerateEditColumn="false" ShowFooter="false" PageSize="50" Width="100%" GridLines="None"> 
                           <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="pproject,ProjectDescr" Width="100%" CommandItemDisplay="Top">
                                <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="25,50,100,250" PageSizeControlType="RadDropDownList" Position="Bottom"/>
                                <NoRecordsTemplate>
                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                </NoRecordsTemplate>
                                <CommandItemTemplate>
                                    <div style="margin:5px 5px;">
                                        <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" Visible="true"><i class="fi fi-csv" title="Export To CSV"></i> Export To CSV</asp:LinkButton>
                                        <asp:LinkButton ID="btnRefresh" runat="server" CommandName="RebindGrid" CssClass="pull-right"><i class="gi gi-refresh" title="Refresh"></i>  Refresh</asp:LinkButton>
                                    </div>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn UniqueName="ClientCompany" DataField="ClientCompany" SortExpression="ClientCompany" HeaderText="Client" HeaderTooltip="Client Name" HeaderStyle-Width="18%" />
                                    <telerik:GridBoundColumn UniqueName="ProjectDescr" DataField="ProjectDescr" SortExpression="ProjectDescr" HeaderText="Project" HeaderTooltip="Project" HeaderStyle-Width="18%" />
                                    <telerik:GridBoundColumn UniqueName="Sales" DataField="Sales" SortExpression="Sales" HeaderText="Sales" HeaderTooltip="Sales" HeaderStyle-Width="10%" />
                                    <telerik:GridBoundColumn UniqueName="MonthlyCharge" DataField="laborprice" SortExpression="laborprice" HeaderText="Monthly Charge" HeaderTooltip="Monthly Charge" HeaderStyle-Width="10%" />
                                    <telerik:GridBoundColumn UniqueName="MonthlyHours" DataField="mainthoursavail" SortExpression="mainthoursavail" HeaderText="Monthly Hours" HeaderTooltip="Monthly Hours" HeaderStyle-Width="10%" />
                                    <telerik:GridTemplateColumn DataField="projhrs" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" HeaderText="Hours Logged" UniqueName="HoursLogged" SortExpression="projhrs">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnViewHours" runat="server" CommandName="ViewHours" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn UniqueName="HoursLoggedForExport" DataField="projhrs" HeaderText="Hours Logged" Visible="false" />
                                    <telerik:GridBoundColumn UniqueName="ExtraTime" DataField="ExtraTime" SortExpression="ExtraTime" HeaderText="Extra Time" HeaderTooltip="Extra Time" HeaderStyle-Width="10%" />
                                    <telerik:GridBoundColumn UniqueName="Rate" DataField="bilrate" SortExpression="bilrate" HeaderText="Rate" HeaderTooltip="Rate" HeaderStyle-Width="7%" />
                                    <telerik:GridBoundColumn UniqueName="AdditionalCharge" DataField="AdditionalCharge" SortExpression="AdditionalCharge" HeaderText="Addl Charge" HeaderTooltip="Additional Charge" HeaderStyle-Width="7%" />
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                    <asp:PlaceHolder ID="phGrandTotal" runat="server">
                        <br /><p style="font-weight:bold;">Total Additional Charge: <asp:Literal ID="litGrandTotal" runat="server" /></p>
                    </asp:PlaceHolder>
                </div>
            </asp:PlaceHolder>
        </div>
    </asp:Panel>
    <div id="modal-project-hours" class="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title"><asp:Literal ID="litModalHeader" runat="server" /></h3>
                </div>
                <div class="modal-body">
                    <grid:Timesheet ID="gridTimesheet" runat="server" Visible="false" />
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCloseTimesheetModal" runat="server" class="btn btn-sm btn-primary" OnClick="btnCloseTimesheetModal_Click" CausesValidation="false"><i class="hi hi-remove"></i> Close</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="ralpReport" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" Runat="Server"></asp:Content>

