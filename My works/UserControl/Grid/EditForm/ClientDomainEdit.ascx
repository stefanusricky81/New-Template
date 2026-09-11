<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientDomainEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_ClientDomainEdit" %>
<style>
    .RadGrid_3b .rgEditForm a { color:#ffffff; }
    .RadGrid_3b .rgEditForm {margin: -1px; padding: 7px; border-bottom: 1px solid #dddddd; background-color:#f2f2f2; }
</style>
<div class="block">
    <div class="block-title">
        <h2>
            <strong><asp:Literal ID="litHeader" runat="server" Text="Add Client Domain" /></strong>
            <asp:ValidationSummary ID="vsClientDomain" runat="server" ValidationGroup="vgClientDomain" CssClass="validationSummary" />
        </h2>
    </div>
    <div class="form-horizontal form-bordered">
        <div class="form-group">
            <label class="col-md-3 control-label">Name<span class="text-danger">*</span></label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtName" CssClass="form-control" Placeholder="Enter Name ..." TabIndex="1"/>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required" ValidationGroup="vgClientDomain" Display="None" />
                <asp:CustomValidator ID="cvName" runat="server" ControlToValidate="txtName" ValidateEmptyText="false" Display="None" OnServerValidate="cvName_ServerValidate" 
                    ValidationGroup="vgClientDomain" ErrorMessage="Domain is already in use"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Description</label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtDescription" MaxLength="500" CssClass="form-control" Placeholder="Enter Description ..." TabIndex="2" TextMode="MultiLine" Rows="5"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Active</label>
            <div class="col-md-5">
                <asp:CheckBox ID="chkActive" runat="server" CssClass="form-control form-control-borderless" TabIndex="3"  />
            </div>
        </div>
    </div>
</div>
<div class="block">
    <div class="form-group form-actions">
        <asp:LinkButton ID="btnEdit" runat="server" ValidationGroup="vgClientDomain" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="Update" TabIndex="15"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
        <asp:LinkButton ID="btnAdd" runat="server" ValidationGroup="vgClientDomain" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="PerformInsert" TabIndex="16"><i class="hi hi-plus"></i> Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CssClass="btn btn-sm btn-warning" CommandName="Cancel"><i class="hi hi-remove" TabIndex="16"></i> Cancel</asp:LinkButton>
    </div>
</div>