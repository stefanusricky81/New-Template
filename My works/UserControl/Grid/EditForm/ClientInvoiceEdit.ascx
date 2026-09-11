<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientInvoiceEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_ClientInvoiceEdit" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<style>
    .RadGrid_3b .rgEditForm a { color:#ffffff; }
    .RadGrid_3b .rgEditForm {margin: -1px; padding: 7px; border-bottom: 1px solid #dddddd; background-color:#f2f2f2; }
</style>
<div class="block">
    <div class="block-title">
        <h2>
            <strong><asp:Literal ID="litHeader" runat="server" Text="Add Client Invoice" /></strong>
            <asp:ValidationSummary ID="vsClientInvoice" runat="server" ValidationGroup="vgClientInvoice" CssClass="validationSummary" />
        </h2>
    </div>
    <asp:Literal ID="litMessage" runat="server" />
    <div class="form-horizontal form-bordered">
        <div class="form-group">
            <label class="col-md-3 control-label">Invoice Number<asp:Literal ID="litNumberRequired" runat="server" Text="<span class='text-danger'>*</span>" /></label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtNumber" MaxLength="50" CssClass="form-control" Placeholder="Enter Number ..." TabIndex="100"/>
                <asp:RequiredFieldValidator ID="rfvNumber" runat="server" ControlToValidate="txtNumber" ErrorMessage="Invoice Number is required" ValidationGroup="vgClientInvoice" Display="None" />
                <asp:CustomValidator ID="cvNumber" runat="server" ControlToValidate="txtNumber" ValidateEmptyText="false" Display="None" OnServerValidate="cvNumber_ServerValidate" 
                    ValidationGroup="vgClientInvoice" ErrorMessage="Invoice Number is already in use"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Invoice Date<asp:Literal ID="litDateRequired" runat="server" Text="<span class='text-danger'>*</span>" /></label>
            <div class="col-md-5">
                <uc:DatePicker ID="ucInvoiceDate" runat="server" PlaceHolderText="Enter Invoice Date ..." IsRequired="true" ValidationGroup="vgClientInvoice" ErrorMessage="Invoice Date ia required" TabIndex="101" />
                <asp:Literal ID="litInvoiceDate" runat="server" Visible="false" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Amount<asp:Literal ID="litAmountRequired" runat="server" Text="<span class='text-danger'>*</span>" /></label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtAmount" MaxLength="50" CssClass="form-control" Placeholder="Enter Invoice Amount ..." TabIndex="102"/>
                <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount" ErrorMessage="Invoice Amount is required" ValidationGroup="vgClientInvoice" Display="None" />
                <asp:RangeValidator ID="rvAmount" runat="server" ControlToValidate="txtAmount" MinimumValue="1" MaximumValue="99999" Type="Double" ErrorMessage="Invalid Invoice Amount" Display="None" ValidationGroup="vgClientInvoice" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Email To<asp:Literal ID="litEmailRequired" runat="server" Text="<span class='text-danger'>*</span>" /></label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtEmail" MaxLength="50" CssClass="form-control" Placeholder="Enter Email To Address ..." TabIndex="103"/>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" ValidationGroup="vgClientInvoice" Display="None" />
                <asp:CustomValidator ID="cvEmail" runat="server"  ControlToValidate="txtEmail" ErrorMessage="Invalid Email" ValidationGroup="vgClientInvoice" Display="None" OnServerValidate="cvEmail_ServerValidate" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Description<asp:Literal ID="litDescriptionRequired" runat="server" Text="<span class='text-danger'>*</span>" /></label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtDescription" MaxLength="500" CssClass="form-control" Placeholder="Enter Description ..." TabIndex="104" TextMode="MultiLine" Rows="5"/>
                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" ErrorMessage="Invoice Description is required" ValidationGroup="vgClientInvoice" Display="None" />
            </div>
        </div>
        <asp:MultiView ID="mvAttachment" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewAttachmentUpload" runat="server">
                <div class="form-group">
                    <label class="col-md-3 control-label">Upload Invoice File (PDF)</label>
                    <div class="col-md-5">
                        <asp:FileUpload ID="fuOne" runat="server" />
                        <asp:CustomValidator ID="cvFile" runat="server" ControlToValidate="txtValidator" ValidationGroup="vgClientInvoice" Display="None"
                            ValidateEmptyText="true" OnServerValidate="cvFile_ServerValidate" />
                        <div style="display:none"><asp:TextBox ID="txtValidator" runat="server" Width="0" /></div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="viewAttachmentView" runat="server">
                <div class="form-group">
                    <label class="col-md-3 control-label">Invoice File (PDF)</label>
                    <div class="col-md-5">
                        <asp:LinkButton ID="btnDownloadAttachment" runat="server" CausesValidation="false" CssClass="btn btn-sm btn-primary" TabIndex="109" OnClick="btnDownloadAttachment_Click"><i class="hi hi-cloud_download"></i> Download</asp:LinkButton>
                        <asp:LinkButton ID="btnDeleteAttachment" runat="server" CausesValidation="false" CssClass="btn btn-sm btn-danger" TabIndex="110" OnClick="btnDeleteAttachment_Click" 
                            OnClientClick="javascript:if(!confirm('Confirm Deletion?')){return false;}" style="margin-left:10px;"><i class="hi hi-remove-circle"></i> Delete</asp:LinkButton>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="viewAttachmentNone" runat="server"></asp:View>
        </asp:MultiView>
        <asp:PlaceHolder ID="phExisting" runat="server">
            <div class="form-group">
                <label class="col-md-3 control-label">Paid</label>
                <div class="col-md-5">
                    <p class="form-control-static"><asp:Literal ID="litPaid" runat="server" /></p>
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-3 control-label">Link</label>
                <div class="col-md-9">
                    <p class="form-control-static"><asp:Literal ID="litLink" runat="server" /></p>
                </div>
            </div>
        </asp:PlaceHolder>
    </div>
</div>
<div class="block">
    <div class="form-group form-actions">
        <asp:LinkButton ID="btnEdit" runat="server" ValidationGroup="vgClientInvoice" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="Update" TabIndex="115"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
        <asp:LinkButton ID="btnAdd" runat="server" ValidationGroup="vgClientInvoice" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="PerformInsert" TabIndex="116"><i class="hi hi-plus"></i> Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CssClass="btn btn-sm btn-warning" CommandName="Cancel"><i class="hi hi-remove" TabIndex="117"></i> Cancel</asp:LinkButton>
    </div>
</div>