<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientSetupEmailMessage.aspx.cs" Inherits="Maintenance_ClientSetupEmailMessage" %>
<%@ Reference VirtualPath="~/UserControl/Grid/EditForm/ClientSetupEmailMessageEdit.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Client Setup Email Maintenance</h1>
        </div>
    </div>
<%--    <telerik:RadAjaxPanel ID="rapClientSetupEmail" runat="server" LoadingPanelID="ralpClientSetupEmail">--%>
        <asp:Literal ID="litMessage" runat="server" Visible="false" /> 
        <div class="block" style="padding-bottom:20px;">
            <div class="row">
                <div style="float:left;">
                    <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnAdd_Click" >Add New</asp:LinkButton>
                </div>
            </div><br />
            <div class="table-responsive">
                <telerik:RadGrid ID="rgClientSetupEmail" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                    OnNeedDataSource="rgClientSetupEmail_NeedDataSource"
                    OnUpdateCommand="rgClientSetupEmail_UpdateCommand"
                    OnItemDataBound="rgClientSetupEmail_ItemDataBound" 
                    OnPreRender="rgClientSetupEmail_PreRender" 
                    AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="true" ShowFooter="false" PageSize="25" Width="100%" >
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" CommandItemDisplay="None">
                        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/ClientSetupEmailMessageEdit.ascx" EditFormType="WebUserControl" />
                        <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                             <telerik:GridTemplateColumn DataField="Id" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%" HeaderText="" UniqueName="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditSystemEmail" runat="server" CommandName="Edit"><i class="gi gi-edit" title="Edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="fkClient" DataType="System.String" Display="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="15%" HeaderText="Client" SortExpression="fkClient" UniqueName="fkClient" />
                            <telerik:GridBoundColumn DataField="EmailMessage" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="25%" HeaderText="Email Message" SortExpression="EmailMessage" UniqueName="EmailMessage" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
        <div id="myModalAdd" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myModalLabelAdd"><asp:Label ID="lblModalTitleAdd" runat="server" /></h4>
                    </div>
                    <asp:ValidationSummary ID="vsClientSetupEmail" runat="server" CssClass="validationSummary" ValidationGroup="vgClientSetupEmail" />
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group">
                                <label class="col-md-4 control-label">Message<span class="text-danger">*</span></label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtBody" CssClass="form-control" Placeholder="Enter Email Body ..." TabIndex="5" TextMode="MultiLine" Rows="5"/>
                                    <asp:RequiredFieldValidator ID="rfvBody" runat="server" ControlToValidate="txtBody" ErrorMessage="Email Body is required" ValidationGroup="vgClientSetupEmail" Display="None" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                        <asp:Button ID="btnAddEmail" runat="server" Text="Add" class="btn btn-primary" OnCommand="Decision_Command" ValidationGroup="vgClientSetupEmail" CommandArgument="Add" />
                    </div>
                </div>
            </div>
        </div>
    <%--</telerik:RadAjaxPanel>--%>
    
<%--    <telerik:RadAjaxLoadingPanel ID="ralpClientSetupEmail" runat="server" Transparency="70" BackColor="#69b899">
        <i class="fa fa-spinner fa-4x fa-spin"></i>
    </telerik:RadAjaxLoadingPanel>--%>
</asp:Content>

