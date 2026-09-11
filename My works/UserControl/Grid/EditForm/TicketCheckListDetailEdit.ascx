<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketCheckListDetailEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_TicketCheckListDetailEdit" %>
<style>
    .RadGrid_3b .rgEditForm a { color:#ffffff; }
    .RadGrid_3b .rgEditForm {margin: -1px; padding: 7px; border-bottom: 1px solid #dddddd; background-color:#f2f2f2; }
</style>
<div class="block">
    <div class="block-title">
        <h2>
            <strong><asp:Literal ID="litHeader" runat="server" Text="Add Task" /></strong>
            <asp:ValidationSummary ID="vsTicketCheckListDetail" runat="server" ValidationGroup="vgTicketCheckListDetail" CssClass="validationSummary" />
        </h2>
    </div>
    <div class="form-horizontal form-bordered">
        <div class="form-group">
            <label class="col-md-3 control-label">Task<span class="text-danger">*</span></label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtTask" MaxLength="50" CssClass="form-control" Placeholder="Enter Task ..." TabIndex="91" TextMode="MultiLine" Rows="6"/>
                <asp:CustomValidator ID="cvTask" runat="server" ControlToValidate="txtTask" ValidateEmptyText="true" Display="None" OnServerValidate="cvTask_ServerValidate"
                    ValidationGroup="vgTicketCheckListDetail" ErrorMessage="Task required" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Order<span class="text-danger">*</span></label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtOrder" MaxLength="5" CssClass="form-control" Placeholder="Enter Order ..." TabIndex="92"/>
                <asp:RequiredFieldValidator ID="rfvOrder" runat="server" ControlToValidate="txtOrder" ErrorMessage="Order is required" ValidationGroup="vgTicketCheckListDetail" Display="None" />
                <asp:RangeValidator ID="rvOrder" runat="server" ControlToValidate="txtOrder" ErrorMessage="Invalid Order - Min of 0 and Max of 1000" Type="Integer" MinimumValue="0" MaximumValue="1000" ValidationGroup="vgTicketCheckListDetail" Display="None" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Active</label>
            <div class="col-md-5">
                <asp:CheckBox ID="chkActive" runat="server" TabIndex="93" CssClass="form-control form-control-borderless" />
            </div>
        </div>
    </div>
</div>
<div class="block">
    <div class="form-group form-actions">
        <asp:LinkButton ID="btnEdit" runat="server" ValidationGroup="vgTicketCheckListDetail" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="Update" TabIndex="94"><i class="hi hi-ok"></i> Edit Task</asp:LinkButton>
        <asp:LinkButton ID="btnAdd" runat="server" ValidationGroup="vgTicketCheckListDetail" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="PerformInsert" TabIndex="95"><i class="hi hi-plus"></i> Add Task</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CssClass="btn btn-sm btn-warning" CommandName="Cancel"><i class="hi hi-remove" TabIndex="96"></i> Cancel</asp:LinkButton>
    </div>
</div>
<asp:Literal ID="litDebug" runat="server" />