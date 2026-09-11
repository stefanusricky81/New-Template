<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientDocumentEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_ClientDocumentEdit" %>
<%@ Register TagPrefix="ddl" TagName="ClientDocumentType" Src="~/UserControl/DropDownList/ClientDocumentType.ascx" %>
<style>
    .RadGrid_3b .rgEditForm a { color:#ffffff; }
    .RadGrid_3b .rgEditForm {margin: -1px; padding: 7px; border-bottom: 1px solid #dddddd; background-color:#f2f2f2; }
</style>
<div class="block">
    <div class="block-title">
        <h2>
            <strong><asp:Literal ID="litHeader" runat="server" Text="Add Edit Client Document Type" /></strong>
            <asp:ValidationSummary ID="vsClientDocument" runat="server" ValidationGroup="vgClientDocument" CssClass="validationSummary" />
        </h2>
    </div>
    <asp:Literal ID="litMessage" runat="server" />
    <div class="form-horizontal form-bordered">
        <div class="form-group">
            <label class="col-md-2 control-label">Name<span class="text-danger">*</span></label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtName" MaxLength="50" CssClass="form-control" Placeholder="Enter Text ..." TabIndex="100"/>
                <asp:RequiredFieldValidator ID="rvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required" ValidationGroup="vgClientDocument" Display="None" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">Document Type <span class="text-danger">*</span></label>
            <div class="col-md-5">
                <ddl:ClientDocumentType ID="ddlClientDocumentType" runat="server" CssClass="form-control" DisplayChosenScript="false" ShowDefaultEntry="true" IsRequired="true" ValidationGroup="vgClientDocument" TabIndex="101" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">Description</label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control" Placeholder="Enter Description ..." TabIndex="102" TextMode="MultiLine" Rows="5"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">Active</label>
            <div class="col-md-5">
                <asp:CheckBox ID="chkActive" runat="server" CssClass="form-control form-control-borderless" TabIndex="103"  />
            </div>
        </div>
    </div>
</div>

<div class="block">
    <div class="form-group form-actions" style="margin-left:20px;">
        <asp:LinkButton ID="btnEdit" runat="server" ValidationGroup="vgClientDocument" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="Update" TabIndex="104"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CssClass="btn btn-sm btn-warning" CommandName="Cancel"><i class="hi hi-remove" TabIndex="105"></i> Cancel</asp:LinkButton>
    </div>
</div>