<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ActionDetail.aspx.cs" Inherits="Maintenance_CRM_ActionDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Action<asp:Literal ID="litHeaderName" runat="server" /></h1>
        </div>
    </div>
    <asp:PlaceHolder ID="phBreadcrumbs" runat="server">
        <div class="breadcrumb breadcrumb-top">
            <asp:HyperLink ID="hlBack" runat="server" Text="Back To Actions" NavigateUrl="/Maintenance/CRM/Action.aspx" />
            <asp:PlaceHolder ID="phBreadcrumbSpacer" runat="server" Visible="false"><span style="padding: 0 10px">|</span></asp:PlaceHolder>
        </div>
    </asp:PlaceHolder>

    <telerik:RadAjaxPanel ID="rap" runat="server" LoadingPanelID="ralp" EnableAJAX="true">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
            </script>
        </telerik:RadCodeBlock>
       
        <asp:Literal ID="litMessage" runat="server" Visible="false" />
        <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
            <asp:ValidationSummary ID="vsProfile" runat="server" ValidationGroup="vgEdit" CssClass="validationSummary" />

            <div class="row">
                <div class="col-md-12">
                    <div class="block">
                        <div class="form-horizontal form-bordered">

                            <div class="row">
                                <div class="col">
                                    <label class="col-sm-2 control-label">Action:<span class="text-danger">*</span></label>
                                </div>
                                <div class="form-group col-sm-3">
                                    <asp:TextBox ID="txtAction" runat="server" MaxLength="50" Width="230" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvAction" runat="server" ControlToValidate="txtAction" ErrorMessage="Action is required." Display="None" ValidationGroup="vgEdit"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvAction" runat="server" ControlToValidate="txtAction" OnServerValidate="cvAction_ServerValidate" ErrorMessage="Action already exists."
                                        ForeColor="Red" ValidationGroup="vgEdit" Display="None">*</asp:CustomValidator>
                                </div>

                            </div>

                            <%--<div class="row">
                                <div class="col">
                                    <asp:Label ID="lblCheckActive" class="col-sm-2 control-label" runat="server" AssociatedControlID="chkActive" Text="Active:" />
                                </div>
                                <div class="form-group col-sm-8">
                                    <label class="switch switch-success">
                                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                        <span></span>
                                    </label>
                                </div>
                            </div>--%>

                            <div class="row">
                                <div class="col">
                                    <asp:Label ID="Label1" class="col-sm-2 control-label" runat="server" Text="" />
                                </div>
                                <div class="col">
                                    <div class="form-group form-actions">
                                        <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vgEdit" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </asp:Panel>
    </telerik:RadAjaxPanel>

    <telerik:RadAjaxLoadingPanel ID="ralp" runat="server" Transparency="70" BackColor="#69b899">
        <i class="fa fa-spinner fa-4x fa-spin"></i>
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" runat="Server">
</asp:Content>



