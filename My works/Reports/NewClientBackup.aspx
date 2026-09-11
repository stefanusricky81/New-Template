<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="NewClientBackup.aspx.cs" Inherits="Reports_NewClientBackup" %>
<%@ Register TagPrefix="uc" TagName="BackupMonitorStatusTypeDDL" Src="~/UserControl/DropDownList/BackupMonitorStatus.ascx" %>
<%@ Register TagPrefix="uc" TagName="BackupTypeDDL" Src="~/UserControl/DropDownList/BackupType.ascx" %>
<%@ Register TagPrefix="uc" TagName="ClientBackupReportGrid" Src="~/UserControl/Grid/ClientBackupReportResponsive.ascx" %>
<%@ Register TagPrefix="uc" TagName="BackupCoreDDL" Src="~/UserControl/DropDownList/BackupMonitorCore.ascx" %>
<%@ Register TagPrefix="uc" TagName="BackupServerDDL" Src="~/UserControl/DropDownList/BackupMonitorServer.ascx" %>
<%@ Register TagPrefix="ddl" TagName="VeeamBackupType" Src="~/UserControl/DropDownList/VeeamBackupType.ascx" %>
<%@ Register TagPrefix="ddl" TagName="SmartClient" Src="~/UserControl/DropDownList/SmartClient.ascx"%>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
        <script type="text/javascript" src="/js/jquery.simplemodal.js"></script>
    <script type="text/javascript" src="/js/osx.js"></script>

        <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("btnExport") > 0) {
                args.set_enableAjax(false);
            }
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Client Backup Report<asp:Literal ID="litClientName" runat="server" /></h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" Visible="false" />
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <div class="block">
            <div class="block-title">
                <h2 style="display: none;"><strong>Search Criteria</strong></h2>
                <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="validationSummary" ValidationGroup="vgSearch" />
            </div>
            <div class="row">
                <div class="form-group col-sm-6">
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="ddlSmartClient" Text="Client" />
                    <ddl:SmartClient ID="ddlSmartClient" runat="server" Width="100%" />
                </div>
                <div class="form-group col-sm-6">
                    <asp:Label ID="Label2" runat="server" AssociatedControlID="ucBackupMonitorStatusDDL" Text="Status" />
                    <uc:BackupMonitorStatusTypeDDL ID="ucBackupMonitorStatusDDL" runat="server" DisplayChosenScript="true" IsRequired="false" DisplayDefaultValue="true" ValidationGroup="vgSearch" Active="true"/><a href="#" class="osx" style="display:none">Status Key</a>
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-6">
                    
                    <div class="row">
                        <div class="form-group col-sm-6">
                            <asp:Label ID="Label3" AssociatedControlID="rdpStartDate" runat="server" Text="Start" /> 
                            <uc:DatePicker ID="rdpStartDate" runat="server" IsRequired="true" />
                        </div>
                        <div class="form-group col-sm-6">
                            <asp:Label ID="Label9" AssociatedControlID="rdpEndDate" runat="server" Text="End" /> 
                            <uc:DatePicker ID="rdpEndDate" runat="server" IsRequired="true" />
                        </div>
                    </div>
                </div>
                <div class="form-group col-sm-6">
                    <asp:Label ID="Label4" runat="server" AssociatedControlID="ucBackupTypeDDL" Text="Backup Type" />
                    <uc:BackupTypeDDL id="ucBackupTypeDDL" DisplayChosenScript="true" runat="server" DisplayDefaultValue="true" IsRequired="false" />
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-6">
                    <asp:Label ID="Label5" runat="server" AssociatedControlID="txtServer" Text="Protected Machine" />
                    <asp:TextBox ID="txtServer" runat="server" MaxLength="50" CssClass="form-control"/>
                </div>
                <div class="form-group col-sm-6">
                    <asp:Label ID="Label6" runat="server" AssociatedControlID="ddlVeeamBackupType" Text="Veeam Job Type" />
                    <ddl:VeeamBackupType ID="ddlVeeamBackupType" runat="server" DisplayDefaultValue="true" IsRequired="false" DefaultText="ALL" />
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-6">
                    <asp:Label ID="Label7" runat="server" AssociatedControlID="txtBackupServer" Text="Backup Server" />
                    <asp:TextBox ID="txtBackupServer" runat="server" MaxLength="50" CssClass="form-control" />
                </div>
                <div class="form-group col-sm-6">
                    <asp:Label ID="Label8" runat="server" AssociatedControlID="chkIsUnsuccessful" Text="Is Unsuccessful" />
                    <asp:CheckBox ID="chkIsUnsuccessful" runat="server" />
                </div>
            </div>

            <div class="form-group form-actions">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgSearch"><i class="hi hi-search"></i> Search</asp:LinkButton>
                <asp:LinkButton ID="btnViewToday" runat="server" CssClass="btn btn-sm btn-info" OnClick="btnViewToday_Click" CausesValidation="false"><i class="hi hi-eye-open"></i> View Today</asp:LinkButton>
                <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
            </div>
        </div>

        <%--<div class="block">
            <div id="osx-modal-content">
			    <div id="osx-modal-title">Status Key</div>
			    <div class="close"><a href="#" class="simplemodal-close">x</a></div>
			    <div id="osx-modal-data" style="float:left;">
				    <ul class="gridKey">
                        <li><span style="color:#24BF49">Successful</span> -> Backup has completed successfully.</li>
                        <li>TBD -> To Be Determined.</li>
                        <li><span style="color:#DB2929">Unsuccessful - Backup (BU)</span> -> <strong>Amount Data Changed</strong> is 0.</li>
                        <li><span style="color:#DB2929">Unsuccessful - Missing Backup Data (MBD)</span> -> <strong>Backup</strong> file was not found.</li>
                        <li><span style="color:#DB2929">Unsuccessful - Missing Files (MF)</span> -> Backup has not run.</li>
                        <li><span style="color:#DB2929">Unsuccessful - Missing Repository Data (MRD)</span> -> <strong>Repository</strong> file was not found.</li>
                        <li><span style="color:#DB2929">Unsuccessful - Replication (RP)</span> -> Items in <strong>Queue</strong> have exceeded threshold.</li>
                        <li><span style="color:#DB2929">Unsuccessful - Rollup (RU)</span> -> Number of <strong>Rollups</strong> is 0.</li>
                    </ul>
			    </div>
		    </div>
        </div>--%>

        
    </asp:Panel>

    <asp:Panel ID="pnlGrid" runat="server">
        <asp:PlaceHolder ID="phSearchResults" runat="server">
                <asp:Literal ID="litDebug" runat="server" />
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong>Search Results</strong></h2>
                        <asp:PlaceHolder ID="phScrollNotes" runat="server"></asp:PlaceHolder>
                    </div>

                    <div class="table-responsive">
                        <uc:ClientBackupReportGrid ID="ucClientBackupReportGrid" runat="server" Visible="true" />
                    </div>
                </div>
            </asp:PlaceHolder>
    </asp:Panel>
</asp:Content>