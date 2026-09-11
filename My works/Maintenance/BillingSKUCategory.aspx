<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="BillingSKUCategory.aspx.cs" Inherits="Maintenance_BillingSKUCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>SKU Category</h1>
        </div>
    </div>

    <asp:Literal ID="litMessage" runat="server" Visible="false" />
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <div class="block">
            <div class="row">
                <div class="form-group col-sm-2">
                    <label>SKU Category</label>
                </div>
                <div class="form-group col-sm-4">
                    <asp:TextBox ID="txtCategoryName" runat="server" MaxLength="25" CssClass="form-control" PlaceHolder="Enter Category" />
                </div>
                <div class="form-group col-sm-1">
                    <label>Active</label>
                </div>
                <div class="form-group col-sm-5">
                    <asp:CheckBox ID="cbSearchActive" Checked="true" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-6">
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgSKUCategory"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="lbClear" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbClear_Click"><i class="hi hi-remove"></i> Clear</asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="block">
            <div class="row">
                <div style="float:left;">
                    <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnAdd_Click">Add New</asp:LinkButton>
                </div>
            </div><br />
            <div class="row">
                <telerik:RadGrid ID="rgSKUCategory" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%" OnNeedDataSource="rgSKUCategory_NeedDataSource" 
                    OnItemCommand="rgSKUCategory_ItemCommand">
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" AllowSorting="true">
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Name">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditRecord" runat="server"  CommandArgument="UpdateRecord" > <i class="gi gi-pencil" title="Edit"></i></asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Id" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="#" Display="false" SortExpression="Id" UniqueName="Id" />
                            <telerik:GridBoundColumn DataField="CategoryName" HeaderStyle-Width="50%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Name" SortExpression="CategoryName" UniqueName="CategoryName" />
                            <telerik:GridBoundColumn DataField="BillingRollUp" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="Billing Roll Up" SortExpression="BillingRollUp" UniqueName="BillingRollUp" />
                            <telerik:GridBoundColumn DataField="InvoiceRollup" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="Invoice Roll Up" SortExpression="InvoiceRollup" UniqueName="InvoiceRollup" />
                            <telerik:GridBoundColumn DataField="Active" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="Active" SortExpression="Active" UniqueName="Active" />
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" />
                </telerik:RadGrid>
            </div>
        </div>

    </asp:Panel>

    <div id="myModalAddEdit" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabelAddEdit"><asp:Label ID="lblModalTitleAddEdit" runat="server" /></h4>
                </div>
                <asp:ValidationSummary ID="vsTicketCategory" runat="server" CssClass="validationSummary" ValidationGroup="vgAddEdit" />
                <asp:HiddenField ID="hfId" runat="server" />
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-4 control-label">Category Name<asp:RequiredFieldValidator ID="rfvDesc" runat="server" ControlToValidate="txtAddCategoryName" ForeColor="Red" ErrorMessage="Category name is required" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator></label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtAddCategoryName" MaxLength="25" CssClass="form-control" Placeholder="Enter Category Name" />
                                        <asp:HiddenField ID="hfOldCategoryName" runat="server" />
                                    </div>
                                </div>  
                                
                                <div class="row">
                                    <label class="col-md-4 control-label">Active</label>
                                    <div class="col-md-8">
                                        <asp:CheckBox ID="cbActive" runat="server" />
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-md-4 control-label">Billing Roll up</label>
                                    <div class="col-md-8">
                                        <asp:CheckBox ID="cbBillingRollUp" runat="server" />
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-md-4 control-label">Invoice Roll up</label>
                                    <div class="col-md-8">
                                        <asp:CheckBox ID="cbInvoiceRollUp" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddSKUCategory" runat="server" Text="Add" class="btn btn-primary" Visible="false" OnCommand="Decision_Command" ValidationGroup="vgAddEdit" CommandArgument="Add" />
                    <asp:Button ID="btnEditSKUCategory" runat="server" Text="Edit" class="btn btn-primary" Visible="false" OnCommand="Decision_Command" ValidationGroup="vgAddEdit" CommandArgument="Edit" />
                </div>
            </div>
        </div>
    </div>

    <div id="myModalDelete" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalDeleteLabel"><asp:Label ID="lblModalDeleteTitle" runat="server" /></h4>
                </div>
                    <div class="modal-body">
                    <h3><asp:Label ID="lblModalDeleteWording" runat="server" /></h3>
                        <asp:HiddenField ID="hfDelete" runat="server" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnYes" runat="server" Text="Delete" class="btn btn-primary" OnCommand="Decision_Command" CommandArgument="Delete" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>