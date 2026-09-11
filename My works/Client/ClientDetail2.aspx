<%@ Page Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientDetail2.aspx.cs" Inherits="Client_ClientDetail2" Title="" %>
<%@ Register TagPrefix="ctrl" TagName="ClientDetailTabs" Src="~/UserControl/Client/ClientDetailTabs.ascx" %>
<%@ Register TagPrefix="grid" TagName="ClientLocation" Src="~/UserControl/Grid/ClientLocation.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketCategory" Src="~/UserControl/DropDownList/TicketCategory.ascx"%>
<%--<%@ Register TagPrefix="ddl" TagName="ClientEmailCategory" Src="~/UserControl/DropDownList/ClientEmailCategory.ascx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpHeadBottom" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
<%--<telerik:RadAjaxPanel ID="rapClient" runat="server" LoadingPanelID="ralpClient">--%>
    <asp:Literal ID="litMessage" runat="server" />
    <asp:Panel ID="pnlMain" runat="server">
        <ctrl:ClientDetailTabs ID="ClientTabs" runat="server" SelectedTabIndex="9" />
        
        <div class="block" style="padding-bottom:20px;">
            <div class="block-title">
                <h2><strong>Locations</strong></h2>
            </div>
            <div class="form-horizontal form-bordered">
                <div class="form-group">
                    <label class="col-md-2 col-sm-4 control-label">Active</label>
                    <div class="col-md-4 col-sm-6">
                        <asp:CheckBox ID="chkLocationActive" runat="server" CssClass="form-control form-control-borderless" AutoPostBack="true" OnCheckedChanged="chkLocationActive_CheckedChanged"  />
                    </div>
                </div>
            </div>
            <grid:ClientLocation ID="gridClientLocation" runat="server" />
        </div>
        <div class="block" style="padding-bottom: 20px;">
            <div class="block-title">
                <h2><strong>
                    <asp:Label ID="Label5" Text="Ticket Categories" runat="server"></asp:Label>
                </strong></h2>
            </div>
            <div class="Container">
                <div class="row">
                    <div class="col-md-12">
                        <div class="row topMargin5">
                            <div class="col-md-1"><strong>Category</strong></div>
                            <div class="col-md-3">
                                <%--<ddl:TicketCategory ID="ddlTicketCategory" runat="server" />--%>
                                <asp:DropDownList ID="ddlTicketCategory" runat="server" CssClass="form-control select-chosen" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnAdd" Text="Add" OnClick="btnAdd_Click" runat="server" />
                            </div>
                            <div class="col-md-6">
                                <telerik:RadGrid ID="rgTicketCategories" runat="server" Skin="3b" OnNeedDataSource="rgTicketCategories_NeedDataSource" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                                    AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%" OnItemCommand="rgTicketCategories_ItemCommand">
                                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" AllowSorting="true">
                                        <NoRecordsTemplate>
                                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                        </NoRecordsTemplate>
                                        <Columns>
                                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Action">
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument="DeleteRecord" ImageUrl="~/Images/delete.png" runat="server" Visible='<%# Eval("CategoryDefault").ToString().Trim() =="Y" ? false:true %>' ID="btnDeleteRecord" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="Id" Display="false" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Id" SortExpression="Id" UniqueName="Id" />
                                            <telerik:GridBoundColumn DataField="CategoryName" DataType="System.String" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Assigned Category" SortExpression="CategoryName" UniqueName="CategoryName" />
                                            <telerik:GridBoundColumn DataField="CategoryDefault" DataType="System.String" Display="false" HeaderText="Default" SortExpression="CategoryDefault" UniqueName="CategoryDefault" />
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings EnableRowHoverStyle="true" />
                                </telerik:RadGrid>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="row topMargin5">
                            <div class="col-md-6"><asp:Button ID="btnAddNew" Text="Create New Category" OnClick="btnAddNew_Click" runat="server" /></div>
                            <div class="col-md-6"></div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="row topMargin5">
                            <div class="col-md-12"><asp:CheckBox ID="cbRequireTicketCategory" Text="&nbsp; Make Ticket Category Required" runat="server" AutoPostBack="true" OnCheckedChanged="cbRequireTicketCategory_CheckedChanged" /></div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="row topMargin5">
                            <div class="col-md-12"><asp:CheckBox ID="cbDisplayTicketOrder" Text="&nbsp; Display Projects" runat="server" OnCheckedChanged="cbDisplayTicketOrder_CheckedChanged" AutoPostBack="true"/></div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="row topMargin5">
                            <div class="col-md-12"><asp:CheckBox ID="cbClosedTicketHistoryEmail" Text="&nbsp; Closed Ticket History Email" runat="server" OnCheckedChanged="cbClosedTicketHistoryEmail_CheckedChanged" AutoPostBack="true"/></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="block" style="padding-bottom: 20px;">
            <div class="block-title">
                <h2><strong>
                    <asp:Label ID="Label1" Text="Client Email Categories" runat="server"></asp:Label>
                </strong></h2>
            </div>
            <div class="Container">
                <div class="row">
                    <div class="form-group col-sm-12">
                        <div class="row topMargin5">
                            <div class="col-md-1"><strong>Category</strong></div>
                            <div class="col-md-3">
                                <%--<ddl:ClientEmailCategory ID="ddlClientEmailCategory" runat="server" Active="true" Available="C" IsRequired="true" DisplayChosenScript="true" ValidationGroup="vgEmailCategory" />--%>
                                <asp:DropDownList ID="ddlClientEmailCategory" runat="server" CssClass="form-control select-chosen" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnAddClientEmailCategory" Text="Add Email Category" OnClick="btnAddClientEmailCategory_Click" ValidationGroup="vgEmailCategory" runat="server" />
                            </div>
                            <div class="col-md-6">
                                <telerik:RadGrid ID="rgEmailCategory" runat="server" Skin="3b" OnNeedDataSource="rgEmailCategory_NeedDataSource" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                                    AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%" OnItemCommand="rgTicketCategories_ItemCommand">
                                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" AllowSorting="true">
                                        <NoRecordsTemplate>
                                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                        </NoRecordsTemplate>
                                        <Columns>
                                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Action">
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument="DeleteRecordEmailCategory" ImageUrl="~/Images/delete.png" runat="server" ID="btnDeleteRecord" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="Id" Display="false" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Id" SortExpression="Id" UniqueName="Id" />
                                            <telerik:GridBoundColumn DataField="CategoryName" DataType="System.String" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Assigned Category" SortExpression="CategoryName" UniqueName="CategoryName" />
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings EnableRowHoverStyle="true" />
                                </telerik:RadGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="block" style="padding-bottom:20px;">
            <div class="block-title">
                <h2><strong>Pager Duty</strong></h2>
            </div>
            <div class="Container">
                <div class="row">
                    <div class="form-group col-sm-12">
                        <div class="row topMargin5">
                            <div class="col-md-3"><strong>Pager Duty Email Forwarding</strong></div>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtPagerDutyEmailForwarding" CssClass="form-control" runat="server" />
                            </div>
                        </div>
                        <div class="row topMargin5">
                            <asp:Button id="btnAddPagerDuty" runat="server" Text="Add Pager Duty" ValidationGroup="vgPagerDuty" OnClick="btnAddPagerDuty_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="block" style="padding-bottom: 20px;">
            <div class="block-title">
                <h2><strong>
                    <asp:Label ID="Label2" Text="Portal Dashboard Widgets" runat="server"></asp:Label>
                </strong></h2>
            </div>
            <div class="Container">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <asp:Repeater ID="rptWidget" runat="server" >
                                <ItemTemplate>
                                    <div class="col-md-4"><asp:Label ID="lblWidgetName" Text='<%# Eval("WidgetDisplayName") %>' runat="server" /></div>
                                    <div class="col-md-2">
                                        <label class="switch switch-success">
                                            <asp:CheckBox ID="chkWidget" runat="server" Checked='<%# Eval("Active") %>' /><span></span></label>
                                        <%--<asp:HiddenField ID="hfWidgetID" runat="server" Value='<%# Eval("ClientWigdetID")  %>' />--%>
                                        <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("ID")  %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
<%--                        <div class="form-group">
                            <label class="col-md-4 control-label">Client Ticket</label>
                            <div class="col-md-8">
                                <label class="switch switch-success">
                                    <asp:CheckBox ID="chkClientTicket" runat="server" /><span></span></label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4 control-label">Client Backups</label>
                            <div class="col-md-8">
                                <label class="switch switch-success">
                                    <asp:CheckBox ID="chkClientBackups" runat="server" /><span></span></label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4 control-label">Client Alert</label>
                            <div class="col-md-8">
                                <label class="switch switch-success">
                                    <asp:CheckBox ID="chkClientAlerts" runat="server" /><span></span></label>
                            </div>
                        </div>--%>          </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <asp:Button ID="btnSubmitWidget" Text="Submit" OnClick="btnSubmitWidget_Click" runat="server" />
                            <asp:Button ID="btnAddwidget" Text="Add Widget" Visible="false" OnClick="btnAddwidget_Click" runat="server" />
                        </div>
                    </div>
                    <%--<div class="col-md-6">
                        <div class="form-group">
                            <label class="col-md-4 control-label">Client Renewal</label>
                            <div class="col-md-8">
                                <label class="switch switch-success">
                                    <asp:CheckBox ID="chkClientRenewal" runat="server" /><span></span></label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4 control-label">Client Server</label>
                            <div class="col-md-8">
                                <label class="switch switch-success">
                                    <asp:CheckBox ID="chkClientServer" runat="server" /><span></span></label>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>

        <div class="block" style="padding-bottom: 20px;">
            <div class="block-title">
                <h2><strong>
                    <asp:Label ID="Label3" Text="Jamf Api" runat="server"></asp:Label>
                </strong></h2>
            </div>
            <div class="Container">
                <div class="row">
                    <div class="form-group col-sm-12">
                        <div class="row topMargin5">
                            <div class="col-md-3"><strong>Jamf Portal URL</strong></div>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtJamfPortalURL" CssClass="form-control" runat="server" />
                            </div>
                        </div>
                        <div class="row topMargin5">
                            <div class="col-md-3"><strong>Client Id</strong></div>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtClientID" CssClass="form-control" runat="server" />
                            </div>
                        </div>
                        <div class="row topMargin5">
                            <div class="col-md-3"><strong>Client Secret</strong></div>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtClientSecret" CssClass="form-control" runat="server" />
                            </div>
                        </div>
                         <div class="row topMargin5">
                            <div class="col-md-3"><strong>User Name</strong></div>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtUserName" CssClass="form-control" runat="server" />
                            </div>
                        </div>
                         <div class="row topMargin5">
                            <div class="col-md-3"><strong>Password</strong></div>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtPassword" CssClass="form-control" runat="server" />
                            </div>
                        </div>
                        <div class="row topMargin5">
                            <asp:Button id="btnAddJamf" runat="server" Text="Submit"  OnClick="btnAddJamf_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <asp:Literal ID="litDebug" runat="server" />
    </asp:Panel>
<%--</telerik:RadAjaxPanel>--%>
<%--<telerik:RadAjaxLoadingPanel ID="ralpClient" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>--%>

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
                                <label class="col-md-4 control-label">Category Name<asp:RequiredFieldValidator ID="rfvDesc" runat="server" ControlToValidate="txtCategoryName" ForeColor="Red" ErrorMessage="Category name is required" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator></label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtCategoryName" MaxLength="25" CssClass="form-control" Placeholder="Enter Category Name" />
                                </div>
                            </div>  
                            <div class="row">
                                <label class="col-md-4 control-label">Available to all clients</label>
                                <div class="col-md-8">
                                    <asp:CheckBox ID="cbDefault" runat="server" />
                                </div>
                            </div>
                            <asp:Panel runat="server" Visible="false">
                                <div class="row">
                                    <label class="col-md-4 control-label">Active</label>
                                    <div class="col-md-8">
                                        <asp:CheckBox ID="cbActive" runat="server" />
                                    </div>
                                </div>
                            </asp:Panel>
                            
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                <asp:Button ID="btnAddClientTicketCategory" runat="server" Text="Add" class="btn btn-primary" Visible="false" OnCommand="Decision_Command" ValidationGroup="vgAddEdit" CommandArgument="Add" />
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
                    <asp:HiddenField ID="hfDeleteFor" runat="server" />
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                <asp:Button ID="btnYes" runat="server" Text="Delete" class="btn btn-primary" OnCommand="Decision_Command" CommandArgument="Delete" />
            </div>
        </div>
    </div>
</div>

<div id="myModalSetWidget" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title" id="myModalSetWidgetLabel"><asp:Label ID="lblModalSetWidget" runat="server" /></h4>
            </div>
            <div class="modal-body">
                <div class="Container">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group col-sm-6">
                                <telerik:RadListBox ID="lstSrcWidget" runat="server" RenderMode="Lightweight" EmptyMessage="No items added" AllowTransfer="true"
                                   AllowTransferOnDoubleClick="true" TransferToID="lstDestWidget">
                                </telerik:RadListBox>
                            </div>
                            <div class="form-group col-sm-6">
                                <telerik:RadListBox ID="lstDestWidget" runat="server" RenderMode="Lightweight" EmptyMessage="No items added"></telerik:RadListBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                <asp:Button ID="btnAddNewWidget" runat="server" Text="Add New Widget" class="btn btn-primary" OnCommand="Decision_Command" CommandArgument="Widget" />
            </div>
        </div>
    </div>
</div>
</asp:Content>


