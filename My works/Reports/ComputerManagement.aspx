<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ComputerManagement.aspx.cs" Inherits="Reports_ComputerManagement" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<%@ Register TagPrefix="ddl" TagName="SupportingTable" Src="~/UserControl/DropDownList/SupportingTable.ascx" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <telerik:radscriptblock id="RadScriptBlock1" runat="server">
    <script type="text/javascript">
        function OnRequestStart(e, sender) {
            var theRegexp = new RegExp("\.btnExport$|\.btnExport2$", "ig");
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
        <h1>Computer Management Report</h1>
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
                    <label>Client</label>
                    <ddl:Client ID="ddlClient" runat="server" IsRequired="false" DisplayChosenScript="true" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Server Name</label>
                    <asp:TextBox ID="txtComputerName" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter Computer Name" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Managed By</label>
                    <asp:ListBox ID="lbManagedBy" runat="server" SelectionMode="Multiple">
                        <asp:ListItem Text="Jamf" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Kaseya" Value="2"></asp:ListItem>
                        <asp:ListItem Text="SentinelOne" Value="3"></asp:ListItem>
                        <asp:ListItem Text="SplashTop" Value="4"></asp:ListItem>
                    </asp:ListBox>
                    <asp:Literal ID="litManagedByJs" runat="server" Visible="false"/>
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>Active</label>
                    <asp:DropDownList ID="ddlActive" runat="server" CssClass="form-control">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                    <asp:Literal ID="litActiveJs" runat="server" Visible="false"/>
                </div>
                <div class="form-group col-sm-4">
                    <label>Machine Type</label>
                    <asp:DropDownList ID="ddlServerWorkstation" runat="server" CssClass="form-control">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Server" Value="1" />
                        <asp:ListItem Text="Workstation" Value="0" />
                    </asp:DropDownList>
                    <asp:Literal ID="litServerWorkstationJs" runat="server" Visible="false"/>
                </div>
                <div class="form-group col-sm-4">
                    <label>Last Backup From</label>
                    <uc:DatePicker ID="ucLastBackupStartDate" runat="server" IsRequired="false" PlaceHolderText="Enter Last Backup From ... " />    
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>Last Backup To</label>
                    <uc:DatePicker ID="ucLastBackupEndDate" runat="server" IsRequired="false" PlaceHolderText="Enter Last Backup To ... " />
                    <asp:CustomValidator ID="cvLastBackupDates" runat="server" ControlToValidate="txtComputerName" ValidateEmptyText="true" ValidationGroup="vgSearch" 
                        ErrorMessage="Last Backup To Date must be greater than Last Backup From Date" Display="None" OnServerValidate="cvLastBackupDates_ServerValidate" />
                </div>
                 <div class="form-group col-sm-4">
                    <label>Last Patch From</label>
                    <uc:DatePicker ID="ucLastPatchStartDate" runat="server" IsRequired="false" PlaceHolderText="Enter Last Patch From ... " />    
                </div>
                <div class="form-group col-sm-4">
                    <label>Last Patch To</label>
                    <uc:DatePicker ID="ucLastPatchEndDate" runat="server" IsRequired="false" PlaceHolderText="Enter Last Patch To ... " />
                    <asp:CustomValidator ID="cvLastPatchDates" runat="server" ControlToValidate="txtComputerName" ValidateEmptyText="true" ValidationGroup="vgSearch" 
                        ErrorMessage="Last Patch To Date must be greater than Last Patch From Date" Display="None" OnServerValidate="cvLastPatchDates_ServerValidate" />
                </div>
            </div>
            <div class="row">
                 <div class="form-group col-sm-4">
                    <label>Last Checkin From</label>
                    <uc:DatePicker ID="ucLastActiveStartDate" runat="server" IsRequired="false" PlaceHolderText="Enter Last Checkin From ... " />    
                </div>
                <div class="form-group col-sm-4">
                    <label>Last Checkin To</label>
                    <uc:DatePicker ID="ucLastActiveEndDate" runat="server" IsRequired="false" PlaceHolderText="Enter Last Checkin To ... " />
                    <asp:CustomValidator ID="cvLastActiveDates" runat="server" ControlToValidate="txtComputerName" ValidateEmptyText="true" ValidationGroup="vgSearch" 
                        ErrorMessage="Last Checkin To Date must be greater than Last Checkin From Date" Display="None" OnServerValidate="cvLastActiveDates_ServerValidate" />
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
                        OnItemCommand="rgReport_ItemCommand"
                        AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" AllowCustomPaging="true" ShowFooter="false" Width="100%" PageSize="25">
                        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" CommandItemDisplay="Top">
                            <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="10,25,50,100" PageSizeControlType="RadDropDownList" Position="Bottom"/>
                            <NoRecordsTemplate>
                                <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                            </NoRecordsTemplate>
                            <CommandItemTemplate>
                                <div style="margin:5px 5px;">
                                    <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" Visible="true"><i class="fi fi-csv" title="Export To CSV"></i>  Export To CSV</asp:LinkButton>
                                    <asp:LinkButton ID="btnExport2" runat="server" OnClick="btnExport2_Click" Visible="true" style="padding-left:10px;"><i class="fi fi-csv" title="Export Client Report To CSV"></i> Export Client Report To CSV</asp:LinkButton>
                                </div>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridTemplateColumn DataField="Id" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="4%" HeaderText="" UniqueName="Action">
                                    <ItemTemplate>        
                                        <asp:LinkButton ID="btnActiveInactive" runat="server" CausesValidation="false" CommandName="MakeActiveInactive"></asp:LinkButton>                                    
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn UniqueName="Company" DataField="Company" SortExpression="Company" HeaderText="Client" HeaderTooltip="Client" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                <telerik:GridBoundColumn UniqueName="Name" DataField="Name" SortExpression="Name" HeaderText="Computer Name" HeaderTooltip="Computer Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                <telerik:GridBoundColumn UniqueName="Os" DataField="Os" SortExpression="Os" HeaderText="OS" HeaderTooltip="OS" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                <telerik:GridBoundColumn UniqueName="OsVersion" DataField="OsVersion" SortExpression="OsVersion" HeaderText="OS Version" HeaderTooltip="Os Version" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                <telerik:GridBoundColumn UniqueName="KASEYAOSINFORMATION" DataField="KASEYAOSINFORMATION" SortExpression="KASEYAOSINFORMATION" HeaderText="OS Information" HeaderTooltip="OS Information" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                <telerik:GridBoundColumn UniqueName="Endpoint" DataField="Endpoint" SortExpression="ManagedBySentinelOne" HeaderText="Endpoint" HeaderTooltip="Endpoint*" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="7%"/>
                                <telerik:GridBoundColumn UniqueName="LastBackupDate" DataField="LastBackupDate" SortExpression="LastBackupDate" HeaderText="Last Backup" HeaderTooltip="Last Backup Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                <telerik:GridBoundColumn UniqueName="LastPatchDate" DataField="LastPatchDate" SortExpression="LastPatchDate" HeaderText="Last Patch*" HeaderTooltip="Last Patch Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                <telerik:GridBoundColumn UniqueName="LastActiveDate" DataField="LastActiveDate" SortExpression="LastActiveDate" HeaderText="Last Checkin*" HeaderTooltip="Last Checkin Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                <telerik:GridBoundColumn UniqueName="KaseyaLastReboot" DataField="KaseyaLastReboot" SortExpression="KaseyaLastReboot" HeaderText="Last Reboot Date" HeaderTooltip="Last Reboot Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                <telerik:GridBoundColumn UniqueName="LastLoggedInUser" DataField="LastLoggedInUser" SortExpression="LastLoggedInUser" HeaderText="Last Login*" HeaderTooltip="Last Logged In User" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                <telerik:GridBoundColumn UniqueName="ManagedBy" DataField="ManagedBy" HeaderText="Managed By" HeaderTooltip="Managed By*" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="9%" AllowSorting="false"/>
                                <telerik:GridBoundColumn UniqueName="Type" DataField="Type" SortExpression="Type" HeaderText="Type" HeaderTooltip="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                <telerik:GridBoundColumn UniqueName="Active" DataField="Active" SortExpression="Active" HeaderText="Active" HeaderTooltip="Active" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="6%"/>

                                <telerik:GridBoundColumn UniqueName="MachineId" DataField="MachineId" HeaderText="Machine Id" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="CurrentUser" DataField="CurrentUser" HeaderText="Current User" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="DomainWorkgroup" DataField="DomainWorkgroup" HeaderText="Domain / Workgroup" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="IpAddress" DataField="IpAddress" HeaderText="Ip Address" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="DefaultGateway" DataField="DefaultGateway" HeaderText="Default Gateway" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="ConnectionGateway" DataField="ConnectionGateway" HeaderText="Connection Gateway" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="Manufacturer" DataField="Manufacturer" HeaderText="Manufacturer" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="SystemSerialNumber" DataField="SystemSerialNumber" HeaderText="System Serial Number" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="RamSize" DataField="RamSize" HeaderText="Ram Size" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="CpuType" DataField="CpuType" HeaderText="Cpu Type" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="SystemPurchaseDate" DataField="SystemPurchaseDate" HeaderText="System Purchase Date" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="SystemWarrantyExpireDate" DataField="SystemWarrantyExpireDate" HeaderText="System Warranty Expire Date" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="MacAddress" DataField="MacAddress" HeaderText="Mac Address" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="EndpointStatus" DataField="EndpointStatus" HeaderText="Endpoint Status" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="BitlockerRecoveryKey" DataField="BitlockerRecoveryKey" HeaderText="Bitlocker Recovery Key" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="TpmVersion" DataField="TpmVersion" HeaderText="Tpm Version" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="TpmEnabled" DataField="TpmEnabled" HeaderText="Tpm Enabled" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                                <telerik:GridBoundColumn UniqueName="TpmOwnership" DataField="TpmOwnership" HeaderText="Tpm Ownership" ItemStyle-HorizontalAlign="Left" Visible="false"/>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" />
                    </telerik:RadGrid>
                </div>
                <p>*Updated Daily</p>
            </div>
        </asp:PlaceHolder>
    </asp:Panel>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="ralpReport" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" Runat="Server"></asp:Content>
