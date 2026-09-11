<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="GetJobNumber.aspx.cs" Inherits="Client_GetJobNumber" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Get Job Number</h1>
        </div>
    </div>

    <telerik:RadAjaxPanel ID="rapUser" runat="server" LoadingPanelID="ralpUser" ClientEvents-OnRequestStart="conditionalPostback">
        <asp:Literal ID="litMessage" runat="server" Visible="false" />
        <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
            <div class="row">
                <div class="col-md-12">
                    <div class="block">
                        <div class="form-horizontal form-bordered">
                            <div class="form-group">
                                <label class="col-md-2">Client</label>
                                <div class="col-md-10">
                                    <ddl:Client ID="ddlClient" runat="server" IsRequired="false" DisplayChosenScript="true" />
                                </div>
                            </div>
                             <div class="form-group">
                                 <div class="col-md-12">
                                     <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-sm btn-primary" ValidationGroup="vgSearch"><i class="hi hi-search"></i> Job Number</asp:LinkButton>
                                 </div>
                             </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </telerik:RadAjaxPanel>
</asp:Content>

