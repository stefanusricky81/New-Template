<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ReportSettings.aspx.cs" Inherits="Maintenance_ReportSettings" %>
<%@ Register TagPrefix="lb" TagName="Days" Src="~/UserControl/ListBox/Days.ascx"%>
<%@ Register TagPrefix="ddl" TagName="Time" Src="~/UserControl/DropDownList/Time.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Report Settings</h1>
        </div>
    </div>
    <asp:ValidationSummary ID="vsReportSettings" runat="server" CssClass="validationSummary" ValidationGroup="vgReportSettings" />
    <asp:Literal ID="litMessage" runat="server" />
    <div class="block">
        <div class="block-title">
            <h2><strong>Patch reboot Report</strong></h2>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <label>Schedule Daily : 12 AM</label>
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <label>Settings :</label>
            </div>            
        </div>
        <div class="row">
            <div class="form-group col-sm-1">
                &nbsp;
            </div>
            <div class="form-group col-sm-3">
                <label># of days since last reboot/patch</label>
            </div>
            <div class="form-group col-sm-8">
                <asp:TextBox ID="txtNumberofDays" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvNumberofDays" runat="server" ControlToValidate="txtNumberofDays" Display="None" ErrorMessage="Please Insert # of days first" ValidationGroup="vgReportSettings" />
                <asp:RegularExpressionValidator ID="revNumberofDays" runat="server" ControlToValidate="txtNumberofDays" Display="None" ErrorMessage="Please Enter Only Numbers" ValidationGroup="vgReportSettings" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <asp:LinkButton ID="btnSubmitPatch" runat="server" OnClick="btnSubmitPatch_Click" ValidationGroup="vgReportSettings" CssClass="btn btn-sm btn-primary"><i class="hi hi-save"></i> Save</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <label>Recipients :</label>
            </div>
        </div>
        <div class="row">
            <div style="float:left;">
                <asp:LinkButton ID="btnAddPatchReport" OnClick="btnAddPatchReport_Click" runat="server" CssClass="btn btn-sm btn-primary">Add New</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <telerik:RadGrid ID="rgPatchReports" runat="server" Skin="3b" EnableEmbeddedSkins="false" OnNeedDataSource="rgPatchReports_NeedDataSource" 
                OnItemCommand="rgPatchReports_ItemCommand" AutoGenerateColumns="false" AllowSorting="true" 
                AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%">
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="id" Width="100%" AllowSorting="true">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="id" Display="false" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="ID" SortExpression="id" UniqueName="id" />
                        <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="30%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" />
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" />
            </telerik:RadGrid>
        </div>
    </div>

    <div class="block" style="padding-bottom: 20px;">
        <div class="block-title">
            <h2><strong>Backup Report</strong></h2>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <label>Schedule Daily : 12 AM</label>
            </div>
        </div>
         <div class="row">
            <div class="form-group col-sm-12">
                <label>Recipients :</label>
            </div>
        </div>
        <div class="row">
            <div style="float:left;">
                <asp:LinkButton ID="lbAddnewBackupReports" OnClick="lbAddnewBackupReports_Click" runat="server" CssClass="btn btn-sm btn-primary">Add New</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <telerik:RadGrid ID="rgBackupReports" runat="server" Skin="3b" EnableEmbeddedSkins="false" OnNeedDataSource="rgBackupReports_NeedDataSource" 
                OnItemCommand="rgPatchReports_ItemCommand" AutoGenerateColumns="false" AllowSorting="true" 
                AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%">
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="id" Width="100%" AllowSorting="true">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="id" Display="false" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="ID" SortExpression="id" UniqueName="id" />
                        <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="30%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" />
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" />
            </telerik:RadGrid>
        </div>
    </div>

    <div class="block" style="padding-bottom: 20px;">
         <div class="block-title">
            <h2><strong>AR Report Email</strong></h2>
        </div>
        <div class="row">
            <div class="form-group col-sm-1">
                <label>Day</label>
            </div>
            <div class="form-group col-sm-2">
                <asp:CheckBox ID="cbEveryDaysArReport" runat="server" CssClass="form-control select2-results" Text="&nbsp;Every Day" AutoPostBack="true" OnCheckedChanged="cbEveryDaysArReport_CheckedChanged"  />
            </div>
            <div class="form-group col-sm-3">
                <asp:Panel ID="pnllbARReport" runat="server">
                    <lb:Days ID="lbDaysARReport" runat="server" DisplayChosenScript="true" />
                </asp:Panel>                
            </div>
            <div class="form-group col-sm-1">
                <label>Time</label>
            </div>
            <div class="form-group col-sm-2">
                <ddl:Time ID="ddlTimeARReport" runat="server" FifteenMinuteIncrements="true" DisplayDefaultValue="false"/>
            </div>
            <div class="form-group col-sm-3">
                <asp:CheckBox ID="cbEnabledARReport" runat="server" CssClass="form-control select2-results" Text="&nbsp;Active" />
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <asp:LinkButton ID="lbARReport" runat="server" OnClick="lbARReport_Click" CssClass="btn btn-sm btn-primary"><i class="hi hi-save"></i> Save</asp:LinkButton>
                <asp:LinkButton ID="lbRunARReport" runat="server" OnClick="lbRunARReport_Click" CssClass="btn btn-sm btn-primary"><i class="hi hi-save"></i> Run</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <label>Recipients :</label>
            </div>
        </div>
        <div class="row">
            <div style="float:left;">
                <asp:LinkButton ID="lbAddARReport" CommandArgument="AddARReport" OnCommand="Decision_Command" runat="server" CssClass="btn btn-sm btn-primary">Add New</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <telerik:RadGrid ID="rgARReport" runat="server" Skin="3b" EnableEmbeddedSkins="false" OnNeedDataSource="rgPatchReports_NeedDataSource" 
                OnItemCommand="rgPatchReports_ItemCommand" AutoGenerateColumns="false" AllowSorting="true" 
                AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%">
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="id" Width="100%" AllowSorting="true">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="id" Display="false" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="ID" SortExpression="id" UniqueName="id" />
                        <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="30%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" />
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" />
            </telerik:RadGrid>
        </div>
    </div>

    <asp:Panel ID="pnlHide" runat="server" Visible="false">

        <div class="block" style="padding-bottom: 20px;">
             <div class="block-title">
                <h2><strong>Computer Management Report</strong></h2>
            </div>
            <div class="row">
                <div class="form-group col-sm-1">
                    <label>Day</label>
                </div>
                <div class="form-group col-sm-2">
                    <asp:CheckBox ID="cbComputerManagementReport" runat="server" CssClass="form-control select2-results" Text="&nbsp;Every Day" AutoPostBack="true" OnCheckedChanged="cbComputerManagementReport_CheckedChanged" />
                </div>
                <div class="form-group col-sm-3">
                    <asp:Panel ID="pnllbComputerManagementReport" runat="server">
                        <lb:Days ID="lbDaysComputerManagementReport" runat="server" DisplayChosenScript="true" />
                    </asp:Panel>                
                </div>
                <div class="form-group col-sm-1">
                    <label>Time</label>
                </div>
                <div class="form-group col-sm-2">
                    <ddl:Time ID="ddlTimeComputerManagementReport" runat="server" FifteenMinuteIncrements="true" DisplayDefaultValue="false"/>
                </div>
                <div class="form-group col-sm-3">
                    <asp:CheckBox ID="cbEnabledComputerManagementReport" runat="server" CssClass="form-control select2-results" Text="&nbsp;Active" />
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-12">
                    <asp:LinkButton ID="lbComputerManagementReport" runat="server" OnClick="lbComputerManagementReport_Click" CssClass="btn btn-sm btn-primary"><i class="hi hi-save"></i> Save</asp:LinkButton>
                    <asp:LinkButton ID="lbRunComputerManagementReport" OnClick="lbRunComputerManagementReport_Click" runat="server" CssClass="btn btn-sm btn-primary"><i class="hi hi-save"></i> Run</asp:LinkButton>
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-12">
                    <label>Recipients :</label>
                </div>
            </div>
            <div class="row">
                <div style="float:left;">
                    <asp:LinkButton ID="lbAddComuterManagementReport" CommandArgument="AddComputerManagementReport" OnCommand="Decision_Command" runat="server" CssClass="btn btn-sm btn-primary">Add New</asp:LinkButton>
                </div>
            </div>
            <div class="row">
                <telerik:RadGrid ID="rgComputerManagementReport" runat="server" Skin="3b" EnableEmbeddedSkins="false" OnNeedDataSource="rgPatchReports_NeedDataSource" 
                    OnItemCommand="rgPatchReports_ItemCommand" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%">
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="id" Width="100%" AllowSorting="true">
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="id" Display="false" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="ID" SortExpression="id" UniqueName="id" />
                            <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="30%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" />
                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" />
                </telerik:RadGrid>
            </div>
        </div>

        <div class="block" style="padding-bottom: 20px;">
         <div class="block-title">
            <h2><strong>Backup Monitor Report Alert v2</strong></h2>
        </div>
        <div class="row">
            <div class="form-group col-sm-1">
                <label>Day</label>
            </div>
            <div class="form-group col-sm-2">
                <asp:CheckBox ID="cbBackupReport" runat="server" CssClass="form-control select2-results" Text="&nbsp;Every Day" AutoPostBack="true" OnCheckedChanged="cbBackupReport_CheckedChanged" />
            </div>
            <div class="form-group col-sm-3">
                <asp:Panel ID="pnllbBackupReport" runat="server">
                    <lb:Days ID="lbDaysBackupReport" runat="server" DisplayChosenScript="true" />
                </asp:Panel>                
            </div>
            <div class="form-group col-sm-1">
                <label>Time</label>
            </div>
            <div class="form-group col-sm-2">
                <ddl:Time ID="ddlTimeBackupReport" runat="server" FifteenMinuteIncrements="true" DisplayDefaultValue="false"/>
            </div>
            <div class="form-group col-sm-3">
                <asp:CheckBox ID="cbEnableBackupReport" runat="server" CssClass="form-control select2-results" Text="&nbsp;Active" />
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <asp:LinkButton ID="lbBackupMonitorReport" runat="server" OnClick="lbBackupMonitorReport_Click" CssClass="btn btn-sm btn-primary"><i class="hi hi-save"></i> Save</asp:LinkButton>
                <asp:LinkButton ID="lbRunBackupMonitorReport" OnClick="lbRunBackupMonitorReport_Click" runat="server" CssClass="btn btn-sm btn-primary"><i class="hi hi-save"></i> Run</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <label>Recipients :</label>
            </div>
        </div>
        <div class="row">
            <div style="float:left;">
                <asp:LinkButton ID="lbBackupReport" CommandArgument="AddBackupReport" OnCommand="Decision_Command" runat="server" CssClass="btn btn-sm btn-primary">Add New</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <telerik:RadGrid ID="rgBackupReport" runat="server" Skin="3b" EnableEmbeddedSkins="false" OnNeedDataSource="rgPatchReports_NeedDataSource" 
                OnItemCommand="rgPatchReports_ItemCommand" AutoGenerateColumns="false" AllowSorting="true" 
                AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%">
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="id" Width="100%" AllowSorting="true">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="id" Display="false" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="ID" SortExpression="id" UniqueName="id" />
                        <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="30%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" />
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" />
            </telerik:RadGrid>
        </div>
    </div>
    </asp:Panel>

    <div class="block" style="padding-bottom: 20px;">
         <div class="block-title">
            <h2><strong>Daily Closed Ticket</strong></h2>
        </div>
        <div class="row">
            <div class="form-group col-sm-1">
                <label>Day</label>
            </div>
            <div class="form-group col-sm-2">
                <asp:CheckBox ID="cbClosedTicket" runat="server" CssClass="form-control select2-results" Text="&nbsp;Every Day" AutoPostBack="true" OnCheckedChanged="cbClosedTicket_CheckedChanged" />
            </div>
            <div class="form-group col-sm-3">
                <asp:Panel ID="pnllbClosedTicket" runat="server">
                    <lb:Days ID="lbDaysClosedTicket" runat="server" DisplayChosenScript="true" />
                </asp:Panel>                
            </div>
            <div class="form-group col-sm-1">
                <label>Time</label>
            </div>
            <div class="form-group col-sm-2">
                <ddl:Time ID="ddlTimeCloseTicket" runat="server" FifteenMinuteIncrements="true" DisplayDefaultValue="false"/>
            </div>
            <div class="form-group col-sm-3">
                <asp:CheckBox ID="cbEnableClosedTicket" runat="server" CssClass="form-control select2-results" Text="&nbsp;Active" />
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <asp:LinkButton ID="lbCloseTicket" runat="server" OnClick="lbCloseTicket_Click" CssClass="btn btn-sm btn-primary"><i class="hi hi-save"></i> Save</asp:LinkButton>
                <asp:LinkButton ID="lbRunCloseTicket" OnClick="lbRunCloseTicket_Click" runat="server" CssClass="btn btn-sm btn-primary"><i class="hi hi-save"></i> Run</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <label>Recipients :</label>
            </div>
        </div>
        <div class="row">
            <div style="float:left;">
                <asp:LinkButton ID="lbClosedTicket" CommandArgument="AddClosedTicket" OnCommand="Decision_Command" runat="server" CssClass="btn btn-sm btn-primary">Add New</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <telerik:RadGrid ID="rgClosedTicket" runat="server" Skin="3b" EnableEmbeddedSkins="false" OnNeedDataSource="rgPatchReports_NeedDataSource" 
                OnItemCommand="rgPatchReports_ItemCommand" AutoGenerateColumns="false" AllowSorting="true" 
                AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%">
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="id" Width="100%" AllowSorting="true">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="id" Display="false" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="ID" SortExpression="id" UniqueName="id" />
                        <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="30%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" />
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" />
            </telerik:RadGrid>
        </div>
    </div>

    <div class="block" style="padding-bottom: 20px;">
         <div class="block-title">
            <h2><strong>Daily Ticket Report Email</strong></h2>
        </div>
        <div class="row">
            <div class="form-group col-sm-1">
                <label>Day</label>
            </div>
            <div class="form-group col-sm-2">
                <asp:CheckBox ID="cbTicketReportEmail" runat="server" CssClass="form-control select2-results" Text="&nbsp;Every Day" AutoPostBack="true" OnCheckedChanged="cbTicketReportEmail_CheckedChanged"/>
            </div>
            <div class="form-group col-sm-3">
                <asp:Panel ID="pnllbTicketReportEmail" runat="server">
                    <lb:Days ID="lbDaysTicketReportEmail" runat="server" DisplayChosenScript="true" />
                </asp:Panel>                
            </div>
            <div class="form-group col-sm-1">
                <label>Time</label>
            </div>
            <div class="form-group col-sm-2">
                <ddl:Time ID="ddlTimeTicketReportEmail" runat="server" FifteenMinuteIncrements="true" DisplayDefaultValue="false"/>
            </div>
            <div class="form-group col-sm-2">
                <asp:CheckBox ID="cbEnabledTicketReport" runat="server" CssClass="form-control select2-results" Text="&nbsp;Active" />
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <asp:LinkButton ID="lbDailyTicketReport" runat="server" OnClick="lbDailyTicketReport_Click" CssClass="btn btn-sm btn-primary"><i class="hi hi-save"></i> Save</asp:LinkButton>
                <asp:LinkButton ID="lbRunDailyTicketReport" runat="server" OnClick="lbRunDailyTicketReport_Click" CssClass="btn btn-sm btn-primary"><i class="hi hi-save"></i> Run</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <label>Recipients :</label>
            </div>
        </div>
        <div class="row">
            <div style="float:left;">
                <asp:LinkButton ID="lbTicketReport" CommandArgument="AddTicketReport" OnCommand="Decision_Command" runat="server" CssClass="btn btn-sm btn-primary">Add New</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <telerik:RadGrid ID="rgTicketReport" runat="server" Skin="3b" EnableEmbeddedSkins="false" OnNeedDataSource="rgPatchReports_NeedDataSource" 
                OnItemCommand="rgPatchReports_ItemCommand" AutoGenerateColumns="false" AllowSorting="true" 
                AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%">
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="id" Width="100%" AllowSorting="true">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="id" Display="false" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="ID" SortExpression="id" UniqueName="id" />
                        <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="30%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" />
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" />
            </telerik:RadGrid>
        </div>
    </div>

    <div class="block" style="padding-bottom: 20px;">
        <div class="block-title">
            <h2><strong>VIP Nighty Email</strong></h2>
        </div>
        <div class="row">
            <div class="form-group col-sm-1">
                <label>Day</label>
            </div>
            <div class="form-group col-sm-2">
                <asp:CheckBox ID="cbEveryDayVIPEmail" runat="server" CssClass="form-control select2-results" Text="&nbsp;Every Day" AutoPostBack="true" OnCheckedChanged="cbEveryDayVIPEmail_CheckedChanged"/>
            </div>
            <div class="form-group col-sm-3">
                <asp:Panel ID="pnlVIPEmail" runat="server">
                    <lb:Days ID="lbDaysVIPEmail" runat="server" DisplayChosenScript="true" />
                </asp:Panel>                
            </div>
            <div class="form-group col-sm-1">
                <label>Time</label>
            </div>
            <div class="form-group col-sm-2">
                <ddl:Time ID="ddlTimeVIPEmail" runat="server" FifteenMinuteIncrements="true" DisplayDefaultValue="false"/>
            </div>
            <div class="form-group col-sm-2">
                <asp:CheckBox ID="cbActiveVIPEmail" runat="server" CssClass="form-control select2-results" Text="&nbsp;Active" />
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <asp:LinkButton ID="lbVIPEmail" runat="server" OnClick="lbVIPEmail_Click" CssClass="btn btn-sm btn-primary"><i class="hi hi-save"></i> Save</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-12">
                <label>Recipients :</label>
            </div>
        </div>
        <div class="row">
            <div style="float:left;">
                <asp:LinkButton ID="lbAddNewVIPEmail" CommandArgument="AddVIPEmail" OnCommand="Decision_Command" runat="server" CssClass="btn btn-sm btn-primary">Add New</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <telerik:RadGrid ID="rgVIPEmail" runat="server" Skin="3b" EnableEmbeddedSkins="false" OnNeedDataSource="rgPatchReports_NeedDataSource" 
                OnItemCommand="rgPatchReports_ItemCommand" AutoGenerateColumns="false" AllowSorting="true" 
                AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%">
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="id" Width="100%" AllowSorting="true">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="id" Display="false" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="ID" SortExpression="id" UniqueName="id" />
                        <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="30%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" />
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" />
            </telerik:RadGrid>
        </div>
    </div>

    <div id="myModalAddPatchRecipients" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="width:160%; margin-left:-30%">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabelAddPatchRecipients"><asp:Label ID="lblModalTitleAddPatchRecipients" runat="server" /></h4>
                </div>
                <asp:Literal ID="ltAddPatchRecipients" runat="server" Visible="false" />
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-4 control-label">Email </label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtEmailPatchReports" MaxLength="100" CssClass="form-control" Placeholder="Enter Email address ..." />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddPatchReports" runat="server" Text="Submit" class="btn btn-primary" OnCommand="Decision_Command" ValidationGroup="vgAddPatchReports" CommandArgument="AddPatch" />
                </div>
            </div>
        </div>
    </div>

    <div id="myModalAddBackupRecipients" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="width:160%; margin-left:-30%">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabelAddBackupRecipients"><asp:Label ID="lblModalTitleAddBackupRecipients" runat="server" /></h4>
                </div>
                <asp:Literal ID="ltAddBackupRecipients" runat="server" Visible="false" />
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-md-12">
                                <label class="col-md-4 control-label">Email </label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtEmailBackupReports" MaxLength="100" CssClass="form-control" Placeholder="Enter Email address ..." />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddBackupReports" runat="server" Text="Submit" class="btn btn-primary" OnCommand="Decision_Command" ValidationGroup="vgAddBackupReports" CommandArgument="AddBackup" />
                </div>
            </div>
        </div>
    </div>

    <div id="myModalDelete" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalDeleteLabel"><asp:Label ID="lblModalDeleteTitle" runat="server" /></h4>
                </div>
                    <div class="modal-body">
                    <h3><asp:Label ID="lblModalDeleteWording" runat="server" /></h3>
                        <asp:HiddenField ID="hfDelete" runat="server" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnYes" runat="server" Text="Delete" class="btn btn-primary" OnCommand="Decision_Command" CommandArgument="Delete" />
                </div>
            </div>
        </div>
    </div>

    <div id="myModalAddRecipients" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="width:160%; margin-left:-30%">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabelAddRecipients"><asp:Label ID="lblModalTitleAddRecipients" runat="server" /></h4>
                </div>
                <asp:Literal ID="ltAddRecipients" runat="server" Visible="false" />
                <asp:HiddenField ID="hfFromReport" runat="server" />
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-4 control-label">Email </label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="tbAddEmailRecipients" MaxLength="100" CssClass="form-control" Placeholder="Enter Email address ..." />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddRecipients" runat="server" Text="Submit" class="btn btn-primary" OnCommand="Decision_Command" CommandArgument="AddRecipients" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>

