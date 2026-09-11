<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="Employee_ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server">
    <script type="text/javascript" src="/js/pwstrength-bootstrap-1.2.10.min.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
    <div class="header-section">
        <h1>Reset Password<asp:Literal ID="litHeaderName" runat="server" /></h1>
    </div>
</div>
<telerik:RadAjaxPanel ID="rapUser" runat="server" LoadingPanelID="ralpUser" EnableAJAX="true">
    <asp:Literal ID="litMessage" runat="server" />
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <asp:PlaceHolder ID="phMainForm" runat="server">
            <div class="block">
                <p>Password must be at least 8 characters and contain 1 upper case character, 1 lower case character, and 1 digit.</p>
                <asp:ValidationSummary ID="vsUser" runat="server" ValidationGroup="vgUser" CssClass="validationSummary" />
                <asp:PlaceHolder ID="phResetText" runat="server" Visible="false">
                    <p><strong><asp:Literal ID="litPasswordReset" runat="server"/></strong></p>
                </asp:PlaceHolder>
                <div class="form-bordered">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-sm-4" style="padding-top:10px; padding-bottom:10px;">
                                <label>Current Password<span class="text-danger">*</span></label>
                                <asp:TextBox runat="server" ID="txtCurrentPassword" MaxLength="50" CssClass="form-control" Placeholder="Enter Current Password ..." TextMode="Password" />
                                <asp:RequiredFieldValidator ID="rfvCurrentPassword" runat="server" ControlToValidate="txtCurrentPassword" ErrorMessage="Current Password is required" ValidationGroup="vgUser" Display="None" />
                                <asp:CustomValidator ID="cvCurrentPassword" runat="server" ControlToValidate="txtCurrentPassword" ErrorMessage="Invalid Current Password" ValidationGroup="vgUser" Display="None" OnServerValidate="cvCurrentPassword_ServerValidate" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <div class="col-sm-4" style="padding-top:10px; padding-bottom:0;">
                                <label>New Password<span class="text-danger">*</span></label>
                                <asp:TextBox runat="server" ID="txtNewPassword" MaxLength="50" CssClass="form-control" Placeholder="Enter New Password ..." TextMode="Password" />
                                <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword" ErrorMessage="New Password is required" ValidationGroup="vgUser" Display="None" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <div class="col-sm-4" style="padding-top:10px; padding-bottom:10px;">
                                <label>Repeat New Password<span class="text-danger">*</span></label>
                                <asp:TextBox runat="server" ID="txtNew2Password" MaxLength="50" CssClass="form-control" Placeholder="Enter Repeat Password ..." TextMode="Password" />
                                <asp:RequiredFieldValidator ID="rfvNew2Password" runat="server" ControlToValidate="txtNew2Password" ErrorMessage="Repeat Password is required" ValidationGroup="vgUser" Display="None" />
                                <asp:CompareValidator ID="compvNewPassword" runat="server" ControlToValidate="txtNewPassword" ControlToCompare="txtNew2Password" ErrorMessage="New Passwords do not match" ValidationGroup="vgUser" Display="None"  />
                                <asp:CustomValidator ID="cvNewPassword" runat="server" ControlToValidate="txtNewPassword" ValidationGroup="vgUser" Display="None" OnServerValidate="cvNewPassword_ServerValidate" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="block">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group form-actions">
                            <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vgUser" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>
    </asp:Panel>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="ralpUser" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphEnd" Runat="Server">
      <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            $('#<%= txtNewPassword.ClientID%>').pwstrength({
                ui: {
                    showVerdictsInsideProgressBar: true,
                    showPopover: true
                }
            });
        </script>
    </telerik:RadCodeBlock>
</asp:Content>

