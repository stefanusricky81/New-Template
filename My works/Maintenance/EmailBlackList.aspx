<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="EmailBlackList.aspx.cs" Inherits="Maintenance_EmailBlackList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
        <div class="content-header">
        <div class="header-section">
            <font style="font-size:30px;font-weight:100">Ticket Email Black List -</font>
            <font style="font-size:large;font-weight:100">BBB ticket system will not process emails received from items in this list</font>
        </div>
    </div>

    <asp:Literal ID="litMessage" runat="server" Visible="false" />
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <div class="block">
            <div class="row">
                <div class="form-group col-sm-6">
                    <label>Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter location name" />
                </div>
                <div class="form-group col-sm-6">
                    <label>Block Status</label>
                    <asp:DropDownList ID="ddlBlock" CssClass="form-control" runat="server">
                        <asp:ListItem Text="" />
                        <asp:ListItem Value="true" Text="Yes" />
                        <asp:ListItem Value="false" Text="No" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div style="float:right;">
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgEmailBlackList"><i class="hi hi-search"></i> Search</asp:LinkButton>
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
                <telerik:RadGrid ID="rgEmailBlackList" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="5" Width="100%" OnNeedDataSource="rgEmailBlackList_NeedDataSource" OnItemCommand="rgEmailBlackList_ItemCommand">
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="id" Width="100%" AllowSorting="true">
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
                            <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="60%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" />
                            <telerik:GridBoundColumn DataField="DisableBlock" Display="false" HeaderStyle-Width="30%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Block Email" SortExpression="DisableBlock" UniqueName="DisableBlock" />
                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" SortExpression="DisableBlock" HeaderStyle-Width="10%" HeaderText="Disable Block" UniqueName="DisableBlock1">
                                <ItemTemplate>
                                    <asp:Label ID="lbBlockEmail" runat="server" Text='<%# Convert.ToBoolean(Eval("DisableBlock")) == true ? "Yes" : "No" %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="id" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="id" SortExpression="id" UniqueName="id" />
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
                <asp:ValidationSummary ID="vsEmailBlackList" runat="server" CssClass="validationSummary" ValidationGroup="vgAddEdit" />
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="row">
                                    <asp:HiddenField ID="hfId" runat="server" />
                                </div>                               
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-3 control-label">Email<asp:RequiredFieldValidator ID="rfvAddEditEmail" runat="server" ControlToValidate="txtAddEditEmail" ForeColor="Red" ErrorMessage="Email is required" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator></label>
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ControlToValidate="txtAddEditEmail" ValidationGroup="vgAddEdit" Display="None" ErrorMessage="Invalid Email Address"/>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtAddEditEmail" MaxLength="50" CssClass="form-control" Placeholder="Enter Email" />
                                    </div>
                                </div>                               
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-3 control-label">Disable Block<asp:RequiredFieldValidator ID="rfvDisableBlock" runat="server" ControlToValidate="ddlAddEditDisableBlock" ForeColor="Red" ErrorMessage="Disable block is required" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator></label>
                                    <div class="col-md-9">
                                        <asp:DropDownList ID="ddlAddEditDisableBlock" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="" />
                                            <asp:ListItem Text="Yes" Value="true" />
                                            <asp:ListItem Text="No" Value="false" />
                                        </asp:DropDownList>
                                    </div>
                                </div>                               
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddTrunk" runat="server" Text="Add" class="btn btn-primary" Visible="false" OnCommand="Decision_Command" ValidationGroup="vgAddEdit" CommandArgument="Add" />
                    <asp:Button ID="btnEditTrunk" runat="server" Text="Edit" class="btn btn-primary" Visible="false" OnCommand="Decision_Command" ValidationGroup="vgAddEdit" CommandArgument="Edit" />
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
