<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientBackup2.aspx.cs" Inherits="Reports_ClientBackup2" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Status" Src="~/UserControl/DropDownList/BackupMonitorStatus.ascx" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
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
        <h1>Backup Set Audit</h1>
    </div>
</div>
<telerik:RadAjaxPanel ID="rapTicket" runat="server" LoadingPanelID="ralpTicket" EnableAJAX="true">
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <div class="block">
            <div class="block-title">
                <h2><strong>Search Criteria</strong></h2>
                <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="validationSummary" ValidationGroup="vgSearch" />
            </div>
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>Server Name</label>
                    <asp:TextBox ID="txtServer" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter Server Name" />
                </div>
                  <div class="form-group col-sm-4">
                    <label>Client</label>
                    <ddl:Client ID="ddlClient" runat="server" IsRequired="false" DisplayChosenScript="true" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Type</label>
                    <asp:DropDownList ID="ddlType" runat="server">
                        <asp:ListItem Text="All" Value=""></asp:ListItem>
                        <asp:ListItem Text="Local" Value="l"></asp:ListItem>
                        <asp:ListItem Text="Offsite" Value="o"></asp:ListItem>
                        <asp:ListItem Text="Replication" Value="r"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Literal ID="litTypeJs" runat="server"/>
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>Status</label>
                    <ddl:Status ID="ddlStatus" runat="server" IsRequired="false" DisplayDefaultValue="true" SetSize="false" DefaultText="All" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Start Date</label>
                    <uc:DatePicker ID="ucStartDate" runat="server" IsRequired="false" PlaceHolderText="Enter Start Date ... " />
                </div>
                <div class="form-group col-sm-4">
                    <label>End Date</label>
                    <uc:DatePicker ID="ucEndDate" runat="server" IsRequired="false" PlaceHolderText="Enter End Date ... " />
                    <asp:CustomValidator ID="cvDates" runat="server" ControlToValidate="txtServer" ValidateEmptyText="true" ValidationGroup="vgSearch" 
                        ErrorMessage="End Date must be greater than Created Start Date" Display="None" OnServerValidate="cvDates_ServerValidate" />
                </div>
            </div>
            <div class="form-group form-actions" style="padding-top:20px;">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click"><i class="hi hi-search"></i> Search</asp:LinkButton>
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
                    <telerik:RadGrid ID="rgClientBackupReport" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                        OnNeedDataSource="rgClientBackupReport_NeedDataSource" 
                        OnItemDataBound="rgClientBackupReport_ItemDataBound" 
                        OnSortCommand="rgClientBackupReport_SortCommand"
                        OnPreRender="rgClientBackupReport_PreRender"
                        AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" AllowCustomPaging="true" ShowFooter="false" Width="100%" PageSize="50">
                        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" CommandItemDisplay="None">
                            <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="25,50,100,250" PageSizeControlType="RadDropDownList" Position="Bottom"/>
                            <NoRecordsTemplate>
                                <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                            </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="Server" DataField="Server" SortExpression="Server" HeaderText="Server Name" HeaderTooltip="Server Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="15%"/>
                                <telerik:GridBoundColumn UniqueName="Company" DataField="Company" SortExpression="Company" HeaderText="Client" HeaderTooltip="Client" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="15%"/>
                                <telerik:GridBoundColumn UniqueName="JobTypeName" DataField="JobTypeName" SortExpression="JobTypeName" HeaderText="Type" HeaderTooltip="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="15%"/>
                                <telerik:GridBoundColumn UniqueName="Status" DataField="Status" SortExpression="Status" HeaderText="Status" HeaderTooltip="Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="15%"/>
                                <telerik:GridBoundColumn UniqueName="BackupDate" DataField="BackupDate" SortExpression="BackupDate" HeaderText="Date" HeaderTooltip="Backup Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                <telerik:GridBoundColumn UniqueName="LastSuccessBackupDate_Local" DataField="LastSuccessBackupDate_Local" SortExpression="LastSuccessBackupDate_Local" HeaderText="Local" HeaderTooltip="Last Successful Local Backup" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                <telerik:GridBoundColumn UniqueName="LastSuccessBackupDate_Offsite" DataField="LastSuccessBackupDate_Offsite" SortExpression="LastSuccessBackupDate_Offsite" HeaderText="Offsite" HeaderTooltip="Last Successful Offsite Backup" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                <telerik:GridBoundColumn UniqueName="LastSuccessBackupDate_Replication" DataField="LastSuccessBackupDate_Replication" SortExpression="LastSuccessBackupDate_Replication" HeaderText="Replication" HeaderTooltip="Last Successful Replication Backup" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" />
                    </telerik:RadGrid>
                </div>
                
            </div>
        </asp:PlaceHolder>
    </asp:Panel>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="ralpTicket" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" Runat="Server"></asp:Content>
