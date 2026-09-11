<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="PatchRebootReport.aspx.cs" Inherits="Reports_PatchRebootReport" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Patch/Reboot Report</h1>
        </div>
    </div>
    <%--<telerik:RadAjaxPanel ID="RapReport" runat="server" ClientEvents-OnRequestStart="conditionalPostback" LoadingPanelID="ralpReport" EnableAJAX="true">--%>
        <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
            <div class="block">
                <div class="block-title">
                    <h2><strong>Search Criteria</strong></h2>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="row topMargin5" style="margin-left:-35px">
                            <div class="col-md-4">
                                <div style="float:right">
                                    <label>Client</label>
                                </div>                            
                            </div>
                            <div class="col-md-8">
                                <ddl:Client ID="ddlClient" runat="server" IsRequired="false" DisplayChosenScript="true" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group form-actions" style="margin-left:8px">
                        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgPatchReport"><i class="hi hi-search"></i> Search</asp:LinkButton>
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="false">
            <div class="block" style="padding-bottom: 20px;">
                <ajaxToolkit:CollapsiblePanelExtender ID="cpeReportSummary" 
                    runat="Server"
                    TargetControlID="pnlReportSummary"
                    ExpandControlID="pnlShowHideReportSummary"
                    CollapseControlID="pnlShowHideReportSummary" 
                    Collapsed="false"
                    BehaviorID="cpeReportSummary"
                    TextLabelID="lblReportSummary"
                    ImageControlID="imgReportSummary"    
                    ExpandedImage="~/Images/collapse_blue.jpg"
                    CollapsedImage="~/Images/expand_blue.jpg"
                    SuppressPostBack="true"
                    CollapsedText="Report Summary [click to display]"
                    ExpandedText="Report Summary [click to hide]" />
                <div class="block-title">
                    <asp:Panel ID="pnlShowHideReportSummary" runat="server" CssClass="collapsePanelHeaderTicket" Height="35px">
                        <div style="float: left;">
                            <h2><strong>
                                <asp:Label ID="lblReportSummary" runat="server" />
                                </strong>
                            </h2>                            
                        </div>
                        <div style="float: right; vertical-align: middle; margin:10px 10px 0px 0px">
                            <asp:ImageButton ID="imgReportSummary" runat="server" ImageUrl="~/img/expand_blue.jpg" />
                        </div>
                    </asp:Panel>
                </div>
                <asp:Panel ID="pnlReportSummary" style="overflow:hidden" runat="server">
                    <div class="row">
                        <div class="form-group" style="margin-left:16px">
                            <h4><asp:Literal ID="ltDates" runat="server" /></h4>
                        </div>
                    </div>
                    <div class="block" style="padding-bottom: 20px;">
                        <telerik:RadGrid ID="rgPatchRebootReportSummarry" OnNeedDataSource="rgPatchRebootReportSummarry_NeedDataSource" runat="server" Skin="3b" EnableEmbeddedSkins="false" 
                            AutoGenerateColumns="false" AllowSorting="true" ShowFooter="false" Width="100%" AllowPaging="true" PageSize="20"> 
                            <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" Width="100%" ShowFooter="true">
                                <NoRecordsTemplate>
                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn UniqueName="pclient" DataField="pclient" SortExpression="pclient" HeaderText="pclient" HeaderTooltip="pclient" Display="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                    <telerik:GridBoundColumn UniqueName="Company" DataField="Company" SortExpression="Company" HeaderText="Company" HeaderTooltip="Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                    <telerik:GridBoundColumn UniqueName="NumofComputer" DataField="NumofComputer" SortExpression="NumofComputer" HeaderText="#Computer" HeaderTooltip="Number ofComputer" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                    <telerik:GridBoundColumn UniqueName="NumofWordkStation" DataField="NumofWordkStation" SortExpression="NumofWordkStation" HeaderText="#WorkStation" HeaderTooltip="Number of Work Station" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                    <telerik:GridBoundColumn UniqueName="NumofServer" DataField="NumofServer" SortExpression="NumofServer" HeaderText="#Server" HeaderTooltip="Number of Server" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="7%"/>
                                    <telerik:GridBoundColumn UniqueName="NumOfNotRebooted" DataField="NumOfNotRebooted" SortExpression="NumOfNotRebooted" HeaderText="#Server not Rebooted" HeaderTooltip="Number Of Not Rebooted Server" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                    <telerik:GridBoundColumn UniqueName="NumofNotPatches" DataField="NumofNotPatches" SortExpression="NumofNotPatches" HeaderText="#Server Not Patched" HeaderTooltip="Number of Not Patched Server" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                    <telerik:GridBoundColumn UniqueName="wksNotRebooted" DataField="wksNotRebooted" SortExpression="wksNotRebooted" HeaderText="#Wks not Rebooted" HeaderTooltip="Number Of Workstation Not Rebooted" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                    <telerik:GridBoundColumn UniqueName="wksNotPatched" DataField="wksNotPatched" SortExpression="wksNotPatched" HeaderText="#Wks Not Patched" HeaderTooltip="Number of Workstation Not Patched Server" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </asp:Panel>
            </div>
            <div class="block" style="padding-bottom: 20px;">
                <ajaxToolkit:CollapsiblePanelExtender ID="cpeReportDetail" 
                    runat="Server"
                    TargetControlID="pnlReportDetail"
                    ExpandControlID="pnlShowHideReportDetail"
                    CollapseControlID="pnlShowHideReportDetail" 
                    Collapsed="true"
                    BehaviorID="cpeReportDetail"
                    TextLabelID="lblReportDetail"
                    ImageControlID="imgReportDetail"    
                    ExpandedImage="~/Images/collapse_blue.jpg"
                    CollapsedImage="~/Images/expand_blue.jpg"
                    SuppressPostBack="true"
                    CollapsedText="Report Detail [click to display]"
                    ExpandedText="Report Detail [click to hide]" />
                <div class="block-title">
                    <asp:Panel ID="pnlShowHideReportDetail" runat="server" CssClass="collapsePanelHeaderTicket" Height="35px">
                        <div style="float: left;">
                            <h2><strong>
                                <asp:Label ID="lblReportDetail" runat="server" />
                                </strong>
                            </h2>                            
                        </div>
                        <div style="float: right; vertical-align: middle; margin:10px 10px 0px 0px">
                            <asp:ImageButton ID="imgReportDetail" runat="server" ImageUrl="~/img/expand_blue.jpg" />
                        </div>
                    </asp:Panel>
                
                </div>
                <asp:Panel ID="pnlReportDetail" style="overflow:hidden" runat="server">
                    <div class="row">
                        <div class="form-group" style="margin-left:10px">
                            <asp:LinkButton ID="lbexport" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbexport_Click"><i class="hi hi-export"></i> Export to Excel</asp:LinkButton>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4 control-label">Not Patched or Rebooted</label>
                            <label class="switch switch-success">
                                <asp:CheckBox ID="cbPatchReboot" OnCheckedChanged="cbPatchReboot_CheckedChanged" AutoPostBack="true" runat="server" /><span></span>
                            </label>
                        </div>
                    </div>
                    <div class="block">
                        
                        <telerik:RadGrid ID="rgPatchRebootReportDetail" OnNeedDataSource="rgPatchRebootReportSummarry_NeedDataSource" runat="server" Skin="3b" EnableEmbeddedSkins="false" 
                            AutoGenerateColumns="false" AllowSorting="true" ShowFooter="false" Width="100%"> 
                            <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" Width="100%" ShowFooter="true">
                                <NoRecordsTemplate>
                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn UniqueName="Company" DataField="Company" SortExpression="Company" HeaderText="Client" HeaderTooltip="Client" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                    <telerik:GridBoundColumn UniqueName="Name" DataField="Name" SortExpression="Name" HeaderText="Computer Name" HeaderTooltip="Computer Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                    <telerik:GridBoundColumn UniqueName="LASTLOGGEDINUSER" DataField="LASTLOGGEDINUSER" SortExpression="LASTLOGGEDINUSER" HeaderText="Last Login" HeaderTooltip="Last Login" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                                    <telerik:GridBoundColumn UniqueName="LASTPATCHDATE" DataField="LASTPATCHDATE" SortExpression="LASTPATCHDATE" HeaderText="Last Patch Date" HeaderTooltip="Last patch date" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="7%"/>
                                    <telerik:GridBoundColumn UniqueName="KASEYALASTREBOOT" DataField="KASEYALASTREBOOT" SortExpression="KASEYALASTREBOOT" HeaderText="Last Reboot" HeaderTooltip="Last Reboot" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                    <telerik:GridBoundColumn UniqueName="LastActiveDate" DataField="LastActiveDate" SortExpression="LastActiveDate" HeaderText="Last Active Date" HeaderTooltip="Last Active Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                    <telerik:GridBoundColumn UniqueName="KASEYAOPERATINGSYSTEM" DataField="KASEYAOPERATINGSYSTEM" SortExpression="KASEYAOPERATINGSYSTEM" HeaderText="Operating System" HeaderTooltip="Operating System" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                    <telerik:GridBoundColumn UniqueName="KASEYAOSINFORMATION" DataField="KASEYAOSINFORMATION" SortExpression="KASEYAOSINFORMATION" HeaderText="OS Information" HeaderTooltip="OS Information" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                    <telerik:GridBoundColumn UniqueName="KASEYAMISSINGAPPROVED" Display="false" DataField="KASEYAMISSINGAPPROVED" SortExpression="KASEYAMISSINGAPPROVED" HeaderText="Missing Approved" HeaderTooltip="Missing Approved" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                    <telerik:GridBoundColumn UniqueName="KASEYAKBARTICLE" Display="false" DataField="KASEYAKBARTICLE" SortExpression="KASEYAKBARTICLE" HeaderText="KB Article" HeaderTooltip="KB Article" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                    <telerik:GridBoundColumn UniqueName="KASEYAPATCHSTATUS" Display="false" DataField="KASEYAPATCHSTATUS" SortExpression="KASEYAPATCHSTATUS" HeaderText="Patch Status" HeaderTooltip="Patch Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </asp:Panel>
            </div>
            <div class="block" style="padding-bottom: 20px;">
                <asp:LinkButton ID="lbSendMail" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbSendMail_Click"><i class="gi gi-message_out"></i> Send Email</asp:LinkButton>
            </div>
        </asp:PlaceHolder>
    <%--</telerik:RadAjaxPanel>--%>
<%--    <telerik:RadAjaxLoadingPanel ID="ralpTicket" runat="server" Transparency="70" BackColor="#69b899">
        <i class="fa fa-spinner fa-4x fa-spin"></i>
    </telerik:RadAjaxLoadingPanel>--%>
    <div style="display:none">
        <telerik:RadGrid ID="rgExportDetailGrid" OnNeedDataSource="rgPatchRebootReportSummarry_NeedDataSource" runat="server" Skin="3b" EnableEmbeddedSkins="false" 
                AutoGenerateColumns="false" AllowSorting="true" ShowFooter="false" Width="100%"> 
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" Width="100%" ShowFooter="true">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn UniqueName="Company" DataField="Company" SortExpression="Company" HeaderText="Client" HeaderTooltip="Client" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                        <telerik:GridBoundColumn UniqueName="Name" DataField="Name" SortExpression="Name" HeaderText="Computer Name" HeaderTooltip="Computer Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                        <telerik:GridBoundColumn UniqueName="LASTLOGGEDINUSER" DataField="LASTLOGGEDINUSER" SortExpression="LASTLOGGEDINUSER" HeaderText="Last Login" HeaderTooltip="Last Login" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"/>
                        <telerik:GridBoundColumn UniqueName="LASTPATCHDATE" DataField="LASTPATCHDATE" SortExpression="LASTPATCHDATE" HeaderText="Last Patch Date" HeaderTooltip="Last patch date" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="7%"/>
                        <telerik:GridBoundColumn UniqueName="KASEYALASTREBOOT" DataField="KASEYALASTREBOOT" SortExpression="KASEYALASTREBOOT" HeaderText="Last Reboot" HeaderTooltip="Last Reboot" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                        <telerik:GridBoundColumn UniqueName="LastActiveDate" DataField="LastActiveDate" SortExpression="LastActiveDate" HeaderText="Last Active Date" HeaderTooltip="Last Active Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                        <telerik:GridBoundColumn UniqueName="KASEYAOPERATINGSYSTEM" DataField="KASEYAOPERATINGSYSTEM" SortExpression="KASEYAOPERATINGSYSTEM" HeaderText="Operating System" HeaderTooltip="Operating System" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                        <telerik:GridBoundColumn UniqueName="KASEYAOSINFORMATION" DataField="KASEYAOSINFORMATION" SortExpression="KASEYAOSINFORMATION" HeaderText="OS Information" HeaderTooltip="OS Information" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                        <telerik:GridBoundColumn UniqueName="KASEYAMISSINGAPPROVED" Display="false" DataField="KASEYAMISSINGAPPROVED" SortExpression="KASEYAMISSINGAPPROVED" HeaderText="Missing Approved" HeaderTooltip="Missing Approved" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                        <telerik:GridBoundColumn UniqueName="KASEYAKBARTICLE" Display="false" DataField="KASEYAKBARTICLE" SortExpression="KASEYAKBARTICLE" HeaderText="KB Article" HeaderTooltip="KB Article" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                        <telerik:GridBoundColumn UniqueName="KASEYAPATCHSTATUS" Display="false" DataField="KASEYAPATCHSTATUS" SortExpression="KASEYAPATCHSTATUS" HeaderText="Patch Status" HeaderTooltip="Patch Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%"/>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
    </div>
</asp:Content>