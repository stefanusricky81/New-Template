<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="TicketType.aspx.cs" Inherits="Maintenance_TicketType" %>
<%@ Register TagPrefix="ddl" TagName="TicketDesignation" Src="~/UserControl/DropDownList/NewTicketDesignation.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Ticket Type</h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" Visible="false" />
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <div class="block">
            <div class="row">
                <div class="form-group col-sm-2">
                    <label>Ticket Type</label>
                </div>
                <div class="form-group col-sm-2">
                    <asp:TextBox ID="txtDescription" runat="server" MaxLength="25" CssClass="form-control" PlaceHolder="Enter Type" />
                </div>
                 <div class="form-group col-sm-2">
                    <label>Ticket Designation</label>
                </div>
                <div class="form-group col-sm-2">
                    <ddl:TicketDesignation id="ddlTicketDesignation" runat="server" />
                </div>
                <div class="form-group col-sm-4">
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgTicketType"><i class="hi hi-search"></i> Search</asp:LinkButton>
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
                <telerik:RadGrid ID="rgTicketType" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%" OnNeedDataSource="rgTicketType_NeedDataSource" OnItemCommand="rgTicketType_ItemCommand">
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="id" Width="100%" AllowSorting="true">
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditRecord" runat="server"  CommandArgument="UpdateRecord" > <i class="gi gi-pencil" title="Edit"></i></asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn SortExpression="Name" HeaderStyle-Width="30%" HeaderText="Ticket Type" UniqueName="Name2">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblName" runat="server" Text='<%# Eval("Name") %>' CommandArgument="UpdateRecord" ><i class="gi gi-pencil" title="Name"></i></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Name" Display="false" HeaderStyle-Width="30%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Name" SortExpression="Name" UniqueName="Name" />
                            <telerik:GridBoundColumn DataField="Designation" HeaderStyle-Width="30%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Designation" SortExpression="Designation" UniqueName="Designation" />
                            <telerik:GridBoundColumn DataField="id" HeaderStyle-Width="10%" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="ID" SortExpression="id" UniqueName="id" />
                            <telerik:GridBoundColumn DataField="fk_DesignationId" HeaderStyle-Width="10%" Display="false" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="ID" SortExpression="fk_DesignationId" UniqueName="fk_DesignationId" />
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
                <asp:ValidationSummary ID="vsTicketType" runat="server" CssClass="validationSummary" ValidationGroup="vgAddEdit" />
                <div class="modal-body">
                    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
                        <div class="row">
                            <div class="form-group">
                                <label class="col-md-2 control-label">Id</label>
                                <div class="col-md-10">
                                    <asp:Label ID="lblIdEdit" runat="server" />
                                    <asp:HiddenField ID="hfEdit" runat="server" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-4 control-label">Ticket Type<asp:RequiredFieldValidator ID="rfvDesc" runat="server" ControlToValidate="txtDesc" ForeColor="Red" ErrorMessage="Type is required" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator></label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtDesc" MaxLength="25" CssClass="form-control" Placeholder="Enter Type" />
                                    </div>
                                </div>                               
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-4 control-label">Ticket Designation<%--<asp:RequiredFieldValidator ID="rfvDesignation" runat="server" ControlToValidate="ddlAddDesignation" ForeColor="Red" ErrorMessage="Designation is required" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator>--%></label>
                                    <div class="col-md-8">
                                        <ddl:TicketDesignation ID="ddlAddDesignation" runat="server" ValidationGroup="vgAddEdit" />
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

