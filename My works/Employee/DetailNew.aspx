<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DetailNew.aspx.cs" Inherits="Employee_DetailNew" MasterPageFile="~/Template/Responsive.master" %>
<%@ Register TagPrefix="ddl" TagName="TimesheetTime" Src="~/UserControl/DropDownList/TimesheetTime.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketType" Src="~/UserControl/DropDownList/TicketType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpHeadBottom" runat="Server">
    <script src="../js/jquery.maskedinput.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Employee Information<asp:Literal ID="litHeaderName" runat="server" /></h1>
        </div>
    </div>
    <asp:PlaceHolder ID="phBreadcrumbs" runat="server">
        <div class="breadcrumb breadcrumb-top">
            <asp:HyperLink ID="hlBack" runat="server" Text="Back To List" NavigateUrl="DefaultNew.aspx" />&nbsp;|
            <a href="/Projectgroups/Default.aspx">Project Groups</a>
            
        </div>
    </asp:PlaceHolder>
    <telerik:RadAjaxPanel ID="rapVendor" runat="server" LoadingPanelID="ralpVendor" EnableAJAX="true">
        <asp:Literal ID="litMessage" runat="server" Visible="false" />
        <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
            <asp:ValidationSummary ID="vsProfile" runat="server" ValidationGroup="vgEmployee" CssClass="validationSummary" />

            <div class="row">
                <div class="col-md-6">
                    <div class="block">
                        <div class="block-title">
                            <h2><strong>Employee Information</strong></h2>
                            <asp:LinkButton ID="lbUpperSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vgEmployee" style="margin:5px 20px 0 0;" CausesValidation="true" CssClass="btn btn-sm btn-primary pull-right"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
                        </div>
                        <div class="form-horizontal form-bordered">
                            <div class="form-group">
                                <label class="col-md-4 control-label">Code<span class="text-danger">*</span></label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtCode" MaxLength="20" CssClass="form-control" Placeholder="Enter Code ..." />
                                    <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtCode" ErrorMessage="Code is required" ValidationGroup="vgEmployee" Display="None" />
                                    <asp:CustomValidator ID="cvCode" runat="server" ValidateEmptyText="false" ErrorMessage="Code is in use" ValidationGroup="vgEmployee" OnServerValidate="cvCode_ServerValidate" Display="None" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">First Name<span class="text-danger">*</span></label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtFirstName" MaxLength="12" CssClass="form-control" Placeholder="Enter First Name ..." />
                                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" ForeColor="Red" ErrorMessage="First Name is required" ValidationGroup="vgEmployee" Display="None">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Last Name<span class="text-danger">*</span></label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtLastName" MaxLength="18" CssClass="form-control" Placeholder="Enter Last Name ..." />
                                    <asp:RequiredFieldValidator ID="rgvLastName" runat="server" ControlToValidate="txtLastName" ForeColor="Red" ErrorMessage="Last Name is required" ValidationGroup="vgEmployee" Display="None">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Allow Contract Renewal</label>
                                <div class="col-md-8">
                                    <label class="switch switch-success">
                                        <asp:CheckBox ID="chkAllowContractRenewal" runat="server" /><span></span></label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Ext</label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtExt" MaxLength="6" CssClass="form-control" Placeholder="Enter Ext ..." />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Email<span class="text-danger">*</span></label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtEmail" MaxLength="75" CssClass="form-control" Placeholder="Enter Email Address ..." />
                                    <asp:RequiredFieldValidator ID="reqtxtEmail" runat="server" ControlToValidate="txtEmail" ForeColor="Red" ErrorMessage="Email is required" ValidationGroup="vgEmployee" Display="None">*</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regtxtEmail" runat="server" ControlToValidate="txtEmail" ForeColor="Red" ErrorMessage="Invalid Email Address" ValidationGroup="vgEmployee" Display="None" ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$">*</asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Cell<span class="text-danger">*</span></label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtCell" CssClass="form-control" Placeholder="Enter Cell ..." />
                                    <asp:RequiredFieldValidator ID="rfvCell" runat="server" ControlToValidate="txtCell" ForeColor="Red" ErrorMessage="Cell is required" ValidationGroup="vgEmployee" Display="None">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Team Phone</label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtTeamPhone" MaxLength="17" CssClass="form-control" Placeholder="Enter Team Phone ..." />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Default Activity</label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlActivity" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Type<span class="text-danger">*</span></label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvEmployeeType" runat="server" ControlToValidate="ddlType" ForeColor="Red" ErrorMessage="Employee type is required" ValidationGroup="vgEmployee" Display="None" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Ticket Type<span class="text-danger">*</span></label>
                                <div class="col-md-8">
                                    <ddl:TicketType ID="ddlTicketType" CssClass="form-control select-chosen" IsRequired="true" runat="server" DisplayChosenScript="true" />
                                </div>
                            </div>
                              <div class="form-group">
                                <label class="col-md-4 control-label">Tier<span class="text-danger">*</span></label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlTiers" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rvTier" runat="server" ControlToValidate="ddlTiers" ForeColor="Red" ErrorMessage="Tier is required" ValidationGroup="vgEmployee" Display="None" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Team<span class="text-danger">*</span></label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlTeam" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvTeam" runat="server" ControlToValidate="ddlTeam" ForeColor="Red" ErrorMessage="Team is required" ValidationGroup="vgEmployee" Display="None" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Address 1</label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtAddress1" MaxLength="30" CssClass="form-control" Placeholder="Enter Address Line 1 ..." />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Address 2</label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtAddress2" MaxLength="30" CssClass="form-control" Placeholder="Enter Address Line 1 ..." />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">City</label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtCity" MaxLength="18" CssClass="form-control" Placeholder="Enter City ..." />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">State</label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtState" MaxLength="2" CssClass="form-control" Placeholder="Enter State ..." />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Zip</label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtZip" MaxLength="10" CssClass="form-control" Placeholder="Enter Zip ..." />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Phone</label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtPhone" CssClass="form-control" Placeholder="Enter Phone ..." />
                                </div>
                            </div>
                            <div class="form-group">
                                 <label class="col-md-4 control-label">Birthday</label>
                                 <div class="col-md-8">
                                     <div class="form-group">
                                        <label class="col-md-2 control-label">Date</label>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlDate" runat="server" CssClass="form-control" />
                                        </div>
                                        <label class="col-md-2 control-label">Month</label>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control" />
                                        </div>
                                     </div>                                  
                                 </div>
                             </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Sales Person Code</label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtSalesPersonCode" MaxLength="17" CssClass="form-control" Placeholder="Enter Sales person code ..." />
                                </div>
                            </div>
                             <div class="form-group">
                                <label class="col-md-4 control-label">Ticket/Timesheet - Time Of Day</label>
                                <div class="col-md-8">
                                    <label class="switch switch-success">
                                        <asp:CheckBox ID="chkTicketTimeOfDay" runat="server" /><span></span>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Timesheet Project Grp.</label>
                                <div class="col-md-8">
                                    <asp:ListBox runat="server" ID="ddlProjectGroup" Enabled="false" CssClass="form-control select-chosen" SelectionMode="Multiple"></asp:ListBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Sales Force Email</label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtSFEmail" MaxLength="75" CssClass="form-control" Placeholder="Enter Sales Force Email Address ..." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSFEmail" ForeColor="Red" ErrorMessage="Invalid Sales Force Email Address" ValidationGroup="vgEmployee" Display="None" ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$">*</asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Start Time</label>
                                <div class="col-md-8">
                                    <ddl:TimesheetTime ID="ddlStartTime" runat="server" IsRequired="false" DisplayChosenScript="true" DisplayDefaultValue="true" DisplayHeaders="false" />
                                    <asp:CustomValidator ID="cvStartEndTime" runat="server" ValidationGroup="vgEmployee" Display="None" ControlToValidate="txtPhone" ValidateEmptyText="true" OnServerValidate="cvStartEndTime_ServerValidate" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">End Time</label>
                                <div class="col-md-8">
                                    <ddl:TimesheetTime ID="ddlEndTime" runat="server" IsRequired="false" DisplayChosenScript="true" DisplayDefaultValue="true" DisplayHeaders="false" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Description</label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtDescription" MaxLength="75" CssClass="form-control" Placeholder="Enter Description ..." />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Active</label>
                                <div class="col-md-8">
                                    <label class="switch switch-success">
                                        <asp:CheckBox ID="chkActive" runat="server" /><span></span>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Assign to Tickets</label>
                                <div class="col-md-8">
                                    <label class="switch switch-success">
                                        <asp:CheckBox ID="cbAssignToTickets" runat="server" /><span></span>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Available for Scheduling</label>
                                <div class="col-md-8">
                                    <label class="switch switch-success">
                                        <asp:CheckBox ID="cbAvailableFosScheduling" runat="server" /><span></span>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4 control-label">Track Activity / Check-In</label>
                                <div class="col-md-8">
                                    <label class="switch switch-success">
                                        <asp:CheckBox ID="chkTrackActivity" runat="server" /><span></span>
                                    </label>
                                </div>
                            </div>
                            <asp:PlaceHolder ID="phBillingRate" runat="server" Visible="false">
                                 <div class="form-group">
                                <label class="col-md-4 control-label">Billing Rate<span class="text-danger">*</span></label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtBillingRate" MaxLength="7" CssClass="form-control" Placeholder="Enter Billing Rate ..." />
                                    <asp:RequiredFieldValidator ID="rfvBillingRate" runat="server" ControlToValidate="txtBillingRate" ForeColor="Red" ErrorMessage="Billing Rate is required" ValidationGroup="vgEmployee" Display="None" />
                                    <asp:RangeValidator ID="rvBillingRate" runat="server" ControlToValidate="txtBillingRate" ForeColor="Red" ErrorMessage="Invalid Billing Rate" ValidationGroup="vgEmployee" Display="None" Type="Currency" MinimumValue="0" MaximumValue="1000" />
                                </div>
                            </div>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="phChangePassword" runat="server" Visible="false">
                                <asp:MultiView ID="mvPassword" runat="server" ActiveViewIndex="0">
                                <asp:View ID="vewPasswordReadOnly" runat="server">
                                    <div class="form-group">
                                        <label class="col-md-4 control-label">Password</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtPasswordReadOnly" runat="server" MaxLength="50" CssClass="form-control" Enabled="false" TextMode="Password" Text="XXXXXXXXXXXX" />
                                        </div>
                                        <div class="col-md-2" style="padding-top:10px;">
                                            <asp:LinkButton ID="btnChangePassword" runat="server" Text="Change" OnClick="btnChangePassword_Click" CommandArgument="1" />
                                        </div>
                                    </div>
                                </asp:View>
                                <asp:View ID="viewPasswordEdit" runat="server">
                                    <div class="form-group">
                                        <label class="col-md-4 control-label">Password<span class="text-danger">*</span></label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" CssClass="form-control" Placeholder="Enter Password ..." />
                                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ForeColor="Red" ErrorMessage="Password is required" ValidationGroup="vgEmployee" Display="None" />
                                        </div>
                                        <div class="col-md-2" style="padding-top:10px;">
                                            <asp:LinkButton ID="btnChangePasswordCancel" runat="server" Text="Cancel" OnClick="btnChangePassword_Click" CommandArgument="0" />
                                        </div>
                                    </div>
                                </asp:View>
                            </asp:MultiView>
                                
                            </asp:PlaceHolder>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="block" id="divHeader" runat="server">
                        <div class="block-title">
                            <h2>
                                <strong>Projects</strong> <asp:Label ID="lblPrjCount" runat="server"></asp:Label>
                            </h2>
                            <a href="/Projectgroups/Default.aspx">Edit</a>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive" style="padding-bottom:20px;">
                                    <telerik:RadGrid ID="rgProjects" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                                        AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" ShowFooter="false" PageSize="25" Width="100%">
                                        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" Width="100%" CommandItemDisplay="None">
                                            <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                                            <NoRecordsTemplate>
                                                <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="Client" HeaderStyle-Width="50%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Client" SortExpression="Client" UniqueName="Client" />
                                                <telerik:GridBoundColumn DataField="Project" HeaderStyle-Width="50%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Project" SortExpression="Project" UniqueName="Project" />
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" />
                                    </telerik:RadGrid>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="block">
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group form-actions">
                            <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vgEmployee" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
                            <asp:LinkButton ID="btnSaveAndAddProjects" OnClick="btnSubmit_Click" CommandName="SaveAndAddProject" runat="server" ValidationGroup="vgEmployee" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-ok"></i> Save & Add Projects</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
            <div id="modal-new-employee" class="modal" tabindex="-1" role="dialog" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h3 class="modal-title">New Employee</h3>
                        </div>
                        <div class="modal-body">
                           Employee has been added. Please add employee to Timesheet Project Group.
                        </div>
                        <div class="modal-footer">
                            <a href="/Projectgroups/Default.aspx" class="btn btn-sm btn-primary">OK</a>
                            <button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>  
        </asp:Panel>
    </telerik:RadAjaxPanel>

    <telerik:RadAjaxLoadingPanel ID="ralpVendor" runat="server" Transparency="70" BackColor="#69b899">
        <i class="fa fa-spinner fa-4x fa-spin"></i>
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
<asp:Content ID="cphEnd" ContentPlaceHolderID="cphEnd" runat="server">
    <script>
        $(document).ready(function () {
            <%--$("#<%= txtPhone.ClientID %>").mask("(999) 999-9999");--%>
           <%-- $("#<%= txtCell.ClientID %>").mask("(999) 999-9999");--%>
        });
    </script>
</asp:Content>
