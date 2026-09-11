<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientLocationEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_ClientLocationEdit" %>
<%@ Register TagPrefix="ddl" TagName="SmartClient" Src="~/UserControl/DropDownList/SmartClient.ascx"%>
<style>
    .RadGrid_3b .rgEditForm a { color:#ffffff; }
    .RadGrid_3b .rgEditForm {margin: -1px; padding: 7px; border-bottom: 1px solid #dddddd; background-color:#f2f2f2; }
</style>
<div class="block">
    <div class="block-title">
        <h2>
            <strong><asp:Literal ID="litHeader" runat="server" Text="Add Client Location" /></strong>
            <asp:ValidationSummary ID="vsClientLocation" runat="server" ValidationGroup="vgClientLocation" CssClass="validationSummary" />
        </h2>
    </div>
    <div class="form-horizontal form-bordered">
        <div class="form-group">
            <label class="col-md-3 control-label">Name<span class="text-danger">*</span></label>
            <div class="col-md-5">
                <ddl:SmartClient ID="ddlSmartClient" runat="server" Width="100%" />
                <asp:TextBox runat="server" ID="txtName" MaxLength="50" CssClass="form-control" Placeholder="Enter Name ..." TabIndex="1"/>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required" ValidationGroup="vgClientLocation" Display="None" />
                <asp:CustomValidator ID="cvName" runat="server" ControlToValidate="txtName" ValidateEmptyText="false" Display="None" OnServerValidate="cvName_ServerValidate" 
                    ValidationGroup="vgClientLocation" ErrorMessage="Location Name is already in use"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Address</label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtAddress" MaxLength="30" CssClass="form-control" Placeholder="Enter Address ..." TabIndex="2"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">City</label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtxCity" MaxLength="18" CssClass="form-control" Placeholder="Enter City ..." TabIndex="3"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">State</label>
            <div class="col-md-5">
                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control" Placeholder="Enter State ..." TabIndex="4" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Zip</label>
            <div class="col-md-5">
                <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" Placeholder="Enter Zip ..." TabIndex="5" />
                <asp:CustomValidator ID="cvZip" runat="server" ControlToValidate="txtZip" ValidateEmptyText="false" Display="None" OnServerValidate="cvZip_ServerValidate" 
                    ValidationGroup="vgClientLocation" ErrorMessage="Only for numeric"/>
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
        <asp:LinkButton ID="btnEdit" runat="server" ValidationGroup="vgClientLocation" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="Update" TabIndex="15"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
        <asp:LinkButton ID="btnAdd" runat="server" ValidationGroup="vgClientLocation" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="PerformInsert" TabIndex="16"><i class="hi hi-plus"></i> Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CssClass="btn btn-sm btn-warning" CommandName="Cancel"><i class="hi hi-remove" TabIndex="16"></i> Cancel</asp:LinkButton>
    </div>
</div>