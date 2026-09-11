<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="EmailCategory.aspx.cs" Inherits="Maintenance_EmailCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Email Category</h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" Visible="false" />
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <div class="block">
            <div class="row">
                <div class="form-group col-sm-2">
                    <label>Email Category</label>
                </div>
                <div class="form-group col-sm-4">
                    <asp:TextBox ID="txtSearchEmailCategory" runat="server" MaxLength="25" CssClass="form-control" PlaceHolder="Enter Email Category" />
                </div>
                <div class="form-group col-sm-6">
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgTicketDesignation"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="lbClear" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbClear_Click"><i class="hi hi-remove"></i> Clear</asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="block">
            <div class="row">
                <div style="float:right;">
                    <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnAdd_Click">Add New</asp:LinkButton>
                </div>
            </div><br />
            <div class="row">
                <telerik:RadGrid ID="rgEmailCategory" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%" OnNeedDataSource="rgEmailCategory_NeedDataSource" 
                    OnItemCommand="rgEmailCategory_ItemCommand">
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="ID" Width="100%" AllowSorting="true">
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Assign">
                                 <ItemTemplate>
                                     <%--<asp:HyperLink ID="btnAssigntoClientContact" runat="server" ToolTip="Assign to Client Contact" Target="_blank" 
                                         Visible='<%# Eval("AvailableFor").ToString().Trim() == "CC" ? true : false %>'
                                         NavigateUrl='<%# Eval("ID","NewClientContactSearch.aspx?ecat={0}") %>'  style="padding-right:5px;">
                                                <i class="fa fa-building" title="Assign Client Contact"></i></asp:HyperLink>--%>
                                     <asp:LinkButton ID="btnAssignCC" Visible='<%# Eval("AvailableFor").ToString().Trim() == "CC" ? true : false %>' runat="server" CommandArgument="AssignCC" > <i class="fa fa-building" title="Assign Client Contact"></i></asp:LinkButton>
                                 </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Name">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditRecord" runat="server"  CommandArgument="UpdateRecord" > <i class="gi gi-pencil" title="Edit"></i></asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="CategoryName" HeaderStyle-Width="50%" Display="false" DataType="System.String" ItemStyle-HorizontalAlign="Left" UniqueName="CategoryNameEdit" />
                            <telerik:GridTemplateColumn SortExpression="CategoryName" HeaderStyle-Width="50%" HeaderText="Category Name" UniqueName="CategoryName">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblDesignation" runat="server" Text='<%# Eval("CategoryName") %>' CommandArgument="UpdateRecord" ><i class="gi gi-pencil" title="Designation"></i></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="AvailableFor" HeaderStyle-Width="50%" Display="false" DataType="System.String" ItemStyle-HorizontalAlign="Left" UniqueName="AvailableForEdit" />
                            <telerik:GridTemplateColumn SortExpression="AvailableFor" HeaderStyle-Width="50%" HeaderText="Available For" UniqueName="AvailableFor">
                                <ItemTemplate>
                                    <asp:Label ID="lblAvailableFor" runat="server" Text='<%# Eval("AvailableFor").ToString().Trim() == "CC" ? "Client Contact" : "Client" %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Active" HeaderStyle-Width="10%" DataType="System.Boolean" ItemStyle-HorizontalAlign="Left" HeaderText="Active" SortExpression="Active" UniqueName="Active" />
                            <telerik:GridBoundColumn DataField="ID" HeaderStyle-Width="10%" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="ID" SortExpression="ID" UniqueName="ID" />
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
                <asp:ValidationSummary ID="vsEmailCategory" runat="server" CssClass="validationSummary" ValidationGroup="vgAddEdit" />
                <div class="modal-body">
                    <asp:HiddenField ID="hfEdit" runat="server" />
                    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
                        <div class="row">
                            <div class="form-group">
                                <label class="col-md-2 control-label">Id</label>
                                <div class="col-md-10">
                                    <asp:Label ID="lblIdEdit" runat="server" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-4 control-label">Email Category<asp:RequiredFieldValidator ID="rfEmailCategory" runat="server" ControlToValidate="txtemailCategory" ForeColor="Red" ErrorMessage="Email Category is required" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator></label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtemailCategory" MaxLength="25" CssClass="form-control" Placeholder="Enter Email Category" />
                                    </div>
                                </div>                               
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-4 control-label">Available For<asp:RequiredFieldValidator ID="rfAvailableFor" runat="server" ControlToValidate="ddlAvailableFor" ForeColor="Red" ErrorMessage="Availabe For is required" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator></label>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="ddlAvailableFor" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Client" Value="C" Selected="True" />
                                            <asp:ListItem Text="Client Contact" Value="CC" />
                                        </asp:DropDownList>
                                    </div>
                                </div>                               
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-4 control-label">Active</label>
                                    <div class="col-md-8">
                                        <asp:CheckBox ID="cbActive" runat="server" Checked="true" />
                                    </div>
                                </div>                               
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddEmailCategory" runat="server" Text="Add" class="btn btn-primary" Visible="false" OnCommand="Decision_Command" ValidationGroup="vgAddEdit" CommandArgument="Add" />
                    <asp:Button ID="btnEditEmailCategory" runat="server" Text="Edit" class="btn btn-primary" Visible="false" OnCommand="Decision_Command" ValidationGroup="vgAddEdit" CommandArgument="Edit" />
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

