<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SystemEmailEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_SystemEmailEdit" %>
<style>
    .RadGrid_3b .rgEditForm a { color:#ffffff; }
    .RadGrid_3b .rgEditForm {margin: -1px; padding: 7px; border-bottom: 1px solid #dddddd; background-color:#f2f2f2; }
</style>
<div class="block">
    <div class="block-title">
        <h2>
            <strong><asp:Literal ID="litHeader" runat="server" Text="Add Edit System Email" /></strong>
            <asp:ValidationSummary ID="vsSystemEmail" runat="server" ValidationGroup="vgSystemEmail" CssClass="validationSummary" />
        </h2>
    </div>
    <asp:Literal ID="litMessage" runat="server" />
    <div class="form-horizontal form-bordered">
        <div class="form-group">
            <label class="col-md-2 control-label">Name</label>
            <div class="col-md-5">
                <p class="form-control-static"><asp:Literal ID="litName" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">From<span class="text-danger">*</span></label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtFrom" MaxLength="50" CssClass="form-control" Placeholder="Enter Email From ..." TabIndex="1"/>
                <asp:RequiredFieldValidator ID="rfvFrom" runat="server" ControlToValidate="txtFrom" ErrorMessage="Email From is required" ValidationGroup="vgSystemEmail" Display="None" />
                <asp:CustomValidator ID="cvFrom" runat="server" ControlToValidate="txtFrom" ValidateEmptyText="false" Display="None" OnServerValidate="cvEmail_ServerValidate"
                    ValidationGroup="vgSystemEmail" ErrorMessage="Invalid Email From"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">Subject<span class="text-danger">*</span></label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtSubject" MaxLength="200" CssClass="form-control" Placeholder="Enter Email Subject ..." TabIndex="2"/>
                <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" ErrorMessage="Email Subject is required" ValidationGroup="vgSystemEmail" Display="None" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">Cc</label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtCc" MaxLength="200" CssClass="form-control" Placeholder="Enter Email Cc ..." TabIndex="3"/>
                <asp:CustomValidator ID="cvCc" runat="server" ControlToValidate="txtCc" ValidateEmptyText="false" Display="None" OnServerValidate="cvEmail_ServerValidate"
                    ValidationGroup="vgSystemEmail" ErrorMessage="Invalid Email Cc"/>
            </div>
        </div>
         <div class="form-group">
            <label class="col-md-2 control-label">Bcc</label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtBcc" MaxLength="200" CssClass="form-control" Placeholder="Enter Email Bcc ..." TabIndex="4"/>
                <asp:CustomValidator ID="cvBcc" runat="server" ControlToValidate="txtBcc" ValidateEmptyText="false" Display="None" OnServerValidate="cvEmail_ServerValidate"
                    ValidationGroup="vgSystemEmail" ErrorMessage="Invalid Email Bcc"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">Body<span class="text-danger">*</span></label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtBody" CssClass="form-control" Placeholder="Enter Email Body ..." TabIndex="5" TextMode="MultiLine" Rows="5"/>
                <asp:RequiredFieldValidator ID="rfvBody" runat="server" ControlToValidate="txtBody" ErrorMessage="Email Body is required" ValidationGroup="vgSystemEmail" Display="None" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">Description</label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control" Placeholder="Enter Description ..." TabIndex="6" TextMode="MultiLine" Rows="5"/>
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