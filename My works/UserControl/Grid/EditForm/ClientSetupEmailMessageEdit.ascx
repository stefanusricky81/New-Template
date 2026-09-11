<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientSetupEmailMessageEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_ClientSetupEmailMessageEdit" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>

<style>
    .RadGrid_3b .rgEditForm a { color:#ffffff; }
    .RadGrid_3b .rgEditForm {margin: -1px; padding: 7px; border-bottom: 1px solid #dddddd; background-color:#f2f2f2; }
</style>
<div class="block">
    <div class="block-title">
         <h2>
            <strong><asp:Literal ID="litHeader" runat="server" Text="Edit Client Setup Email" /></strong>
            <asp:ValidationSummary ID="vsSystemEmail" runat="server" ValidationGroup="vgSystemEmail" CssClass="validationSummary" />
        </h2>
    </div>
    <asp:Literal ID="litMessage" runat="server" />
    <div class="form-horizontal form-bordered">
        <div class="form-group">
            <label class="col-md-2 control-label">Body<span class="text-danger">*</span></label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtBody" CssClass="form-control" Placeholder="Enter Email Body ..." TabIndex="5" TextMode="MultiLine" Rows="5"/>
                <asp:RequiredFieldValidator ID="rfvBody" runat="server" ControlToValidate="txtBody" ErrorMessage="Email Body is required" ValidationGroup="vgSystemEmail" Display="None" />
            </div>
        </div>
    </div>
</div>
<div class="block">
    <div class="form-group form-actions" style="margin-left:20px;">
        <asp:LinkButton ID="btnEdit" runat="server" ValidationGroup="vgSystemEmail" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="Update" TabIndex="7"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
        <asp:LinkButton ID="btnAdd" runat="server" ValidationGroup="vgSystemEmail" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="PerformInsert" TabIndex="8"><i class="hi hi-plus"></i> Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CssClass="btn btn-sm btn-warning" CommandName="Cancel"><i class="hi hi-remove" TabIndex="9"></i> Cancel</asp:LinkButton>
    </div>
</div>