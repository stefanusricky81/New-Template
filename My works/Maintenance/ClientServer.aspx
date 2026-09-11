<%@ Page Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientServer.aspx.cs" Inherits="Maintenance_ClientServer" Title="Bit By Bit Intranet - Client Servers" %>
<%@ Register TagPrefix="uc" TagName="Client" Src="~/UserControl/Typeahead/Client.ascx" %>
<%@ Register TagPrefix="ddl" TagName="SupportingTable" Src="~/UserControl/DropDownList/SupportingTable.ascx" %>
<%@ Reference VirtualPath="~/UserControl/Grid/EditForm/ClientServerEdit.ascx" %>
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
<telerik:radscriptblock id="RadScriptBlock1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var isMobile = navigator.userAgent.match(/(iPhone|iPod|iPad|Android|BlackBerry)/);
            if (!isMobile) {
                SetChosenActive_<%=ddlActive.ClientID%>();
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenActive_<%=ddlActive.ClientID%>);
                SetChosenLocal_<%=ddlLocal.ClientID%>();
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenLocal_<%=ddlLocal.ClientID%>);
                SetChosenOffsite_<%=ddlOffsite.ClientID%>();
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenOffsite_<%=ddlOffsite.ClientID%>);
                SetChosenReplica_<%=ddlReplica.ClientID%>();
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenReplica_<%=ddlReplica.ClientID%>);
                SetChosenSingleBackup_<%=ddlSingleBackup.ClientID%>();
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenSingleBackup_<%=ddlSingleBackup.ClientID%>);
                SetChosenddlBackupServer_<%=ddlBackupServer.ClientID%>();
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenddlBackupServer_<%=ddlBackupServer.ClientID%>);
                SetChosenddlNoBackup_<%=ddlNoBackup.ClientID%>();
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenddlNoBackup_<%=ddlNoBackup.ClientID%>);
                SetChosenddlType_<%=ddlType.ClientID%>();
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenddlType_<%=ddlType.ClientID%>);
                SetChosenddlImported_<%=ddlImported.ClientID%>();
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenddlImported_<%=ddlImported.ClientID%>);
            }
        });
        function SetChosenActive_<%=ddlActive.ClientID%>(sender, args) {
            $('#<%=ddlActive.ClientID%>').chosen({
                allow_single_deselect: true,
                width: "100%"
            });
        }
        function SetChosenLocal_<%=ddlLocal.ClientID%>(sender, args) {
            $('#<%=ddlLocal.ClientID%>').chosen({
                allow_single_deselect: true,
                width: "100%"
            });
        }
        function SetChosenOffsite_<%=ddlOffsite.ClientID%>(sender, args) {
            $('#<%=ddlOffsite.ClientID%>').chosen({
                allow_single_deselect: true,
                width: "100%"
            });
        }
        function SetChosenReplica_<%=ddlReplica.ClientID%>(sender, args) {
            $('#<%=ddlReplica.ClientID%>').chosen({
                allow_single_deselect: true,
                width: "100%"
            });
        }
        function SetChosenSingleBackup_<%=ddlSingleBackup.ClientID%>(sender, args) {
            $('#<%=ddlSingleBackup.ClientID%>').chosen({
                allow_single_deselect: true,
                width: "100%"
            });
        }
        function SetChosenddlBackupServer_<%=ddlBackupServer.ClientID%>(sender, args) {
            $('#<%=ddlBackupServer.ClientID%>').chosen({
                allow_single_deselect: true,
                width: "100%"
            });
        }
        function SetChosenddlNoBackup_<%=ddlNoBackup.ClientID%>(sender, args) {
            $('#<%=ddlNoBackup.ClientID%>').chosen({
                allow_single_deselect: true,
                width: "100%"
            });
        }
        function SetChosenddlType_<%=ddlType.ClientID%>(sender, args) {
            $('#<%=ddlType.ClientID%>').chosen({
                allow_single_deselect: true,
                width: "100%"
            });
        }
        function SetChosenddlImported_<%=ddlImported.ClientID%>(sender, args) {
            $('#<%=ddlImported.ClientID%>').chosen({
                allow_single_deselect: true,
                width: "100%"
            });
        }
    </script>
</telerik:radscriptblock>

    <div class="content-header">
    <div class="header-section">
        <h1>Client Servers</h1>
    </div>
</div>
<telerik:RadAjaxPanel ID="rapTimesheet" runat="server" LoadingPanelID="ralpServer" EnableAJAX="true">
    <asp:Literal ID="litMessage" runat="server" />
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <asp:Literal ID="litDebug" runat="server" />
        <div class="block">
             <div class="block-title">
                <h2><strong>Search Criteria</strong></h2>
                <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="validationSummary" ValidationGroup="vgSearch" />
            </div>
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>Client</label>
                    <div class="input-group">
                        <uc:Client ID="ucClient" runat="server" IsRequired="false" />
                        <span class="input-group-btn"><asp:LinkButton ID="btnClearClient" runat="server" OnClick="btnClearClient_Click" CssClass="btn btn-warning"><i class="fa fa-repeat" title="Clear"></i> </asp:LinkButton></span>
                    </div>
                </div>
                <div class="form-group col-sm-4">
                    <label>Server Name</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="200" PlaceHolder="Enter Server Name ..." />
                </div>
                <div class="form-group col-sm-4">
                    <label>Active</label>
                    <asp:DropDownList ID="ddlActive" runat="server">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>Local</label>
                    <asp:DropDownList ID="ddlLocal" runat="server">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                </div>
                <div class="form-group col-sm-4">
                    <label>Offsite</label>
                    <asp:DropDownList ID="ddlOffsite" runat="server">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                </div>
                <div class="form-group col-sm-4">
                    <label>Replica</label>
                    <asp:DropDownList ID="ddlReplica" runat="server">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>Single Backup</label>
                    <asp:DropDownList ID="ddlSingleBackup" runat="server">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                </div>
                <div class="form-group col-sm-4">
                    <label>Backup Server</label>
                    <asp:DropDownList ID="ddlBackupServer" runat="server">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" Selected="True" />
                    </asp:DropDownList>
                </div>
                <div class="form-group col-sm-4">
                    <label>No Backup</label>
                    <asp:DropDownList ID="ddlNoBackup" runat="server">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" Selected="True" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>Backup Type</label>
                    <ddl:SupportingTable ID="ddlBackupType" runat="server" IsRequired="false" DisplayDefaultValue="true" ChosenDisplayName="Backup Type" DefaultText="All" />
                </div>
                <div class="form-group col-sm-4">
                    <label>OS</label>
                    <ddl:SupportingTable ID="ddlOs" runat="server" IsRequired="false" DisplayDefaultValue="true" ChosenDisplayName="OS" DefaultText="All" />
                </div>
                <div class="form-group col-sm-4">
                    <label>OS Version</label>
                    <ddl:SupportingTable ID="ddlOsVersion" runat="server" IsRequired="false" DisplayDefaultValue="true" ChosenDisplayName="OS Version" DefaultText="All" />
                </div>
            </div>
             <div class="row">
                <div class="form-group col-sm-4">
                    <label>Server/Workstation</label>
                    <asp:DropDownList ID="ddlType" runat="server">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Server" Value="1" />
                        <asp:ListItem Text="Workstation" Value="0" />
                    </asp:DropDownList>
                </div>
                 <div class="form-group col-sm-4">
                    <label>Imported</label>
                    <asp:DropDownList ID="ddlImported" runat="server">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group form-actions" style="padding-top:20px;">
                <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-sm btn-primary" ValidationGroup="vgSearch"><i class="hi hi-search"></i> Search</asp:LinkButton>
                <asp:LinkButton ID="btnClear" runat="server" OnClick="btnClear_Click" CssClass="btn btn-sm btn-warning" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
            </div>
        </div>
        <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="false">
            <div class="block" style="padding-bottom: 20px;">
                <div class="block-title">
                    <h2><strong>Search Results</strong></h2>
                </div>
                 <div class="table-responsive">
                <telerik:RadGrid ID="rgServer" runat="server"  Skin="3b" EnableEmbeddedSkins="false" 
                    OnNeedDataSource="rgServer_NeedDataSource"
                    OnItemDataBound="rgServer_ItemDataBound" 
                    OnItemCommand="rgServer_ItemCommand" 
                    OnPreRender="rgServer_PreRender" 
                    OnUpdateCommand="rgServer_UpdateCommand" 
                    OnInsertCommand="rgServer_InsertCommand" 
                    OnSortCommand="rgServer_SortCommand"
                    AllowSorting="true" AllowPaging="true" AllowCustomPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false"
                    AutoGenerateDeleteColumn="false" AutoGenerateEditColumn="false" ShowFooter="false" Width="100%" GridLines="None" PageSize="100">
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" CommandItemDisplay="Top">
                        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/ClientServerEdit.ascx" EditFormType="WebUserControl" />
                        <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="25,50,100,250" PageSizeControlType="RadDropDownList" Position="Bottom"/>
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <CommandItemTemplate>
                            <div style="margin:2px 5px;">
                                <asp:LinkButton ID="btnAddNewRecord" runat="server" CommandName="InitInsert"><i class="gi gi-circle_plus"></i></i>  Add New Server</asp:LinkButton>
                            </div>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn DataField="Id" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="4%" HeaderText="" UniqueName="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" style="padding-right:5px;"><i class="gi gi-edit" title="Edit"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete"><i class="gi gi-circle_remove" title="Delete"></i></asp:LinkButton>                                    
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Name" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" HeaderText="Server Name" SortExpression="Name" UniqueName="Name" />
                            <telerik:GridBoundColumn DataField="ClientCompany" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" HeaderText="Client" SortExpression="ClientCompany" UniqueName="ClientCompany" />
                            <telerik:GridBoundColumn DataField="Description" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%" HeaderText="Description" SortExpression="Description" UniqueName="Description" />
                            <telerik:GridBoundColumn DataField="Active" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%" HeaderText="Active" SortExpression="Active" UniqueName="Active" />
                            <telerik:GridBoundColumn DataField="Local" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%" HeaderText="Local" SortExpression="Local" UniqueName="Local" />
                            <telerik:GridBoundColumn DataField="Offsite" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="6%" HeaderText="Offsite" SortExpression="Offsite" UniqueName="Offsite" />
                            <telerik:GridBoundColumn DataField="Replica" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="6%" HeaderText="Replica" SortExpression="Replica" UniqueName="Replica" />
                            <telerik:GridBoundColumn DataField="SingleBackup" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="6%" HeaderText="Single BU" SortExpression="SingleBackup" UniqueName="SingleBackup" HeaderTooltip="Single Backup" />
                            <telerik:GridBoundColumn DataField="BackupServer" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="6%" HeaderText="BU Server" SortExpression="BackupServer" UniqueName="BackupServer" HeaderTooltip="Backup Server" />
                            <telerik:GridBoundColumn DataField="NoBackupDisplay" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%" HeaderText="No BU" SortExpression="NoBackupDisplay" UniqueName="NoBackupDisplay" HeaderTooltip="No Backup" />
                            <telerik:GridBoundColumn DataField="BackupTypeName" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="6%" HeaderText="BU Type" SortExpression="BackupTypeName" UniqueName="BackupTypeName" HeaderTooltip="No BackupTypeName" />
                            <telerik:GridBoundColumn DataField="OsName" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%" HeaderText="OS" HeaderTooltip="Operation System" SortExpression="OsName" UniqueName="OsName" />
                            <telerik:GridBoundColumn DataField="OsVersionName" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%" HeaderText="OSV" HeaderTooltip="Operation System Version" SortExpression="OsVersionName" UniqueName="OsVersionName" />
                            <telerik:GridBoundColumn DataField="AutomatedImport" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%" HeaderText="Imp" HeaderTooltip="Imported" SortExpression="AutomatedImport" UniqueName="AutomatedImport" />
                            <telerik:GridBoundColumn DataField="IsServer" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="7%" HeaderText="Type" HeaderTooltip="Server Or Workstation" SortExpression="IsServer" UniqueName="IsServer" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
            </div>
        </asp:PlaceHolder>
    </asp:Panel>

</telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="ralpServer" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>
</asp:Content>

