<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="EmailDetail.aspx.cs" Inherits="Maintenance_CRM_EmailDetail" %>

<%@ Register TagPrefix="crm" TagName="LeadProduct" Src="~/UserControl/CRM/LeadProduct.ascx" %>
<%@ Register TagPrefix="crm" TagName="SenderEmail" Src="~/UserControl/CRM/SalesEmail.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Email<asp:Literal ID="litHeaderName" runat="server" /></h1>
        </div>
    </div>
    <asp:PlaceHolder ID="phBreadcrumbs" runat="server">
        <div class="breadcrumb breadcrumb-top">
            <asp:HyperLink ID="hlBack" runat="server" Text="Back To Emails" NavigateUrl="/Maintenance/CRM/Email.aspx" />
            <asp:PlaceHolder ID="phBreadcrumbSpacer" runat="server" Visible="false"><span style="padding: 0 10px">|</span></asp:PlaceHolder>
        </div>
    </asp:PlaceHolder>

    <telerik:RadAjaxPanel ID="rap" runat="server" LoadingPanelID="ralp" EnableAJAX="true">

        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                function OnClientLoad(editor) {
                    var editorframe = editor.get_contentAreaElement();
                    editorframe.allowTransparency = true;
                    editorframe.contentWindow.document.body.style.background = "transparent";
                }
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
                                    <label class="col-sm-2 control-label">Email Display Name:<span class="text-danger">*</span></label>
                                </div>
                                <div class="form-group col-sm-6">
                                    <asp:TextBox ID="txtDisplayName" runat="server" MaxLength="200" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvDisplayName" runat="server" ControlToValidate="txtDisplayName" ErrorMessage="Email Display Name is required." Display="None" ValidationGroup="vgEdit"></asp:RequiredFieldValidator>
                                    <%--<asp:CustomValidator ID="cvDisplayName" runat="server" ControlToValidate="txtDisplayName" OnServerValidate="cvDisplayName_ServerValidate" ErrorMessage="Email already exists."
                                        ForeColor="Red" ValidationGroup="vgEdit" Display="None">*</asp:CustomValidator>--%>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col">
                                    <label class="col-sm-2 control-label">Lead Product:<span class="text-danger">*</span></label>
                                </div>
                                <div class="form-group col-sm-4">
                                    <crm:LeadProduct ID="ddlLeadProduct" runat="server" CssClass="form-control select-chosen" IsRequired="true" ShowDefaultEntry="true" DefaultText="Select an Option" DefaultValue="" ValidationGroup="vgEdit" ErrorMessage="Lead Product is required." />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col">
                                    <label class="col-sm-2 control-label">From Email:<span class="text-danger">*</span></label>
                                </div>
                                <div class="form-group col-sm-4">
                                    <crm:SenderEmail ID="ddlSenderEmail" runat="server" CssClass="form-control select-chosen" ShowDefaultEntry="true" DefaultText="Select an Option" DefaultValue="" IsRequired="true" ValidationGroup="vgEidt" ErrorMessage="Lead Product is required." />
                                </div>

                            </div>

                            <div class="row">
                                <div class="col">
                                    <label class="col-sm-2 control-label">Email Subject:<span class="text-danger">*</span></label>
                                </div>
                                <div class="form-group col-sm-6">
                                    <asp:TextBox ID="txtEmailSubject" runat="server" MaxLength="200" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvEmailSubject" runat="server" ControlToValidate="txtEmailSubject" ErrorMessage="Email Subject is required." Display="None" ValidationGroup="vgEdit"></asp:RequiredFieldValidator>
                                </div>
                            </div>


                            <div class="row">
                                <div class="col">
                                    <label class="col-sm-2 control-label">Email Content:<span class="text-danger">*</span></label>
                                </div>
                                <div class="form-group col-sm-8">
                                    <%-- <telerik:RadEditor ID="txtEmailContent" runat="server" CssClass="form-control" OnClientLoad="OnClientLoad">                                      
                                    </telerik:RadEditor>--%>
                                    <telerik:RadEditor runat="server" ID="txtEmailContent" Height="400px" CssClass="form-control"  OnClientLoad="OnClientLoad"
                                        Skin="Default" ContentAreaCssFile="~/Css/Editor.css" 
                                        ToolsFile="~/UserControl/Editor/BasicTools.xml"
                                        DialogHandlerUrl="Telerik.Web.UI.DialogHandler.axd">
                                    </telerik:RadEditor>
                                    <asp:RequiredFieldValidator ID="rfvEmailContent" runat="server" ControlToValidate="txtEmailContent" ErrorMessage="Email Content is required." Display="None" ValidationGroup="vgEdit"></asp:RequiredFieldValidator>
                                </div>
                            </div>

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



