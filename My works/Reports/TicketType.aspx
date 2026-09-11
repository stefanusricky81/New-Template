<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TicketType.aspx.cs" Inherits="Reports_TicketType" MasterPageFile="~/Template/Responsive.master" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        function OnRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("btnExport") > 0) {
                args.set_enableAjax(false);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Ticket Type Analysis</h1>
        </div>
    </div>
    <telerik:RadAjaxPanel ID="rapReport" runat="server" LoadingPanelID="ralpReport" EnableAJAX="true" ClientEvents-OnRequestStart="OnRequestStart">
        <div class="block">
            <div class="block-title">
                <h2><strong>Search Criteria</strong></h2>
                <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="validationSummary" ValidationGroup="vgSearch" />
            </div>
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>Start Date</label>
                    <uc:DatePicker ID="dtStartDate" runat="server" IsRequired="true" ValidationGroup="vgSearch" ErrorMessage="Start Date is required" />
                </div>
                <div class="form-group col-sm-4">
                    <label>End Date</label>
                    <uc:DatePicker ID="dtEndDate" runat="server" IsRequired="true" ValidationGroup="vgSearch" ErrorMessage="End Date is required" />
                    <div style="display:none;"><asp:TextBox ID="txtValidator" runat="server" Width="1" /></div>
                    <asp:CustomValidator ID="cvDates" runat="server" ControlToValidate="txtValidator" ValidateEmptyText="true" ValidationGroup="vgSearch" OnServerValidate="cvDates_ServerValidate" Display="None"/>
                </div>
                <div class="form-group col-sm-4">
                    <label>Company</label>
                    <ddl:Client ID="ddlClient" runat="server" IsRequired="false" DisplayChosenScript="true" />
                </div>
            </div>
            <div class="row" style="padding-top:10px; padding-bottom:10px;">
                <div class="col-sm-12">
                    <div class="form-group form-actions" style="margin-top: 15px">
                        <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSearch_Click" ValidationGroup="vgSearch"><i class="hi hi-search"></i>Search</asp:LinkButton>
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="phGrid" runat="server" Visible="false">
            <div class="block" style="padding-bottom:20px;">
                <div class="table-responsive" style="padding-top: 10px; padding-bottom: 20px;">
                    <telerik:RadGrid ID="rgTicketType" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                        OnNeedDataSource="rgTicketType_NeedDataSource"
                        OnItemDataBound="rgTicketType_ItemDataBound"
                        OnColumnCreated="rgTicketType_ColumnCreated" 
                        OnGridExporting="rgTicketType_GridExporting" 
                        OnDetailTableDataBind="rgTicketType_DetailTableDataBind"
                        AutoGenerateColumns="true" AllowSorting="true" AllowPaging="true" ShowFooter="true" PageSize="500" Width="100%">
                        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Name" Width="100%" CommandItemDisplay="None">
                            <DetailTables>
                                <telerik:GridTableView DataKeyNames="Name" Name="Week" Width="100%">
                                    <DetailTables>
                                        <telerik:GridTableView DataKeyNames="Name" Name="Day" Width="100%" />
                                    </DetailTables>
                                </telerik:GridTableView>
                            </DetailTables>
                            <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                            <NoRecordsTemplate>
                                <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                            </NoRecordsTemplate>
                            <CommandItemTemplate>
                                <div style="margin:2px 5px;">
                                    <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click"><i class="fi fi-csv" title="Export To CSV"></i>  Export To CSV</asp:LinkButton>
                                </div>
                            </CommandItemTemplate>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" />
                        <ExportSettings ExportOnlyData="true" FileName="TicketType" IgnorePaging="true" OpenInNewWindow="true">
                            <Csv ColumnDelimiter="Comma" FileExtension="csv" RowDelimiter="NewLine" EncloseDataWithQuotes="true"/>
                        </ExportSettings>
                    </telerik:RadGrid>
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:Literal ID="litDebug" runat="server" />
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="ralpReport" runat="server" Transparency="70" BackColor="#69b899">
        <i class="fa fa-spinner fa-4x fa-spin"></i>
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
