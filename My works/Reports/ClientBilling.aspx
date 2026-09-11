<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientBilling.aspx.cs" Inherits="Report_ClientBilling" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        .success {
            color: green;
        }

        .error {
            color: red;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Search Client</h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" Visible="false" />
    <telerik:RadAjaxPanel ID="rapMenu" runat="server" LoadingPanelID="ralpMenu" EnableAJAX="true">

        <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">

            <div class="block">
                <div class="block-title">
                    <h2 style="display: none;"><strong>Search Criteria</strong></h2>
                    <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="validationSummary" ValidationGroup="vgSearch" />
                </div>
                <div class="row">
                    <div class="form-group col-sm-4">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="txtCompany" Text="Company" />
                        <asp:TextBox ID="txtCompany" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                    <div class="form-group col-sm-4">
                        <asp:Label ID="lblcode" runat="server" AssociatedControlID="txtCode" Text="Code" />
                        <asp:TextBox ID="txtCode" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                    <div class="form-group col-sm-4">
                        <label>Auto Billing</label>
                        <asp:DropDownList ID="ddlAutoBill" runat="server" CssClass="form-control">
                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>
                <div class="form-group form-actions">
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgSearch"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                </div>
            </div>



            <asp:PlaceHolder ID="phSearchResults" runat="server">
                <asp:Literal ID="litDebug" runat="server" />
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong>Search Results</strong></h2>
                        <asp:PlaceHolder ID="phScrollNotes" runat="server"></asp:PlaceHolder>
                    </div>

                    <div class="table-responsive">
                        <telerik:RadGrid ID="rgClient" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                            OnNeedDataSource="rgClient_NeedDataSource" OnItemCommand="rgClient_ItemCommand"
                            AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" ShowFooter="false" PageSize="25" Width="100%">
                            <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" CommandItemDisplay="Top">
                                <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                                <NoRecordsTemplate>
                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                </NoRecordsTemplate>
                                <CommandItemTemplate>
                                    <div style="margin: 2px 5px; display: inline; float: left">
                                        <asp:HyperLink ID="hlAddNewRecord" runat="server" Enabled="false" NavigateUrl="Detail.aspx?ID=0"><i class="gi gi-user_add" title="Add New Pilot Information"></i>  Add New Client</asp:HyperLink>
                                    </div>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn DataField="pclient" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="5%" HeaderText="" UniqueName="ActionColumn">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="btnEditRecord" runat="server" NavigateUrl='<%# Eval("pclient","clientdetail.aspx?ClientId={0}") %>' style="padding-right:8px;"><i class="gi gi-pencil" title="Edit"></i></asp:HyperLink>
                                            <asp:HyperLink ID="btnEditBilling" runat="server" NavigateUrl='<%# Eval("pclient","ClientBilling.aspx?ClientId={0}") %>'><i class="gi gi-money" title="View/Edit Billing"></i></asp:HyperLink>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Code" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Code" SortExpression="Code" UniqueName="Code" />
                                    <%--<telerik:GridBoundColumn DataField="Company" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Company Name" SortExpression="Company" UniqueName="Company" />--%>
                                    <telerik:GridTemplateColumn DataField="pclient" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="15%" HeaderText="Company Name" UniqueName="ActionColumn">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="btnEditRecord" runat="server" ToolTip="View Client" Text='<%# Eval("Company") %>' NavigateUrl='<%# Eval("pclient","clientdetail.aspx?ClientId={0}") %>'></asp:HyperLink>
                                                    &nbsp;
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="addr1" ItemStyle-HorizontalAlign="Left" SortExpression="addr1" HeaderStyle-Width="20%" HeaderText="Address" UniqueName="addr1">
                                        <ItemTemplate>
                                            <%# Eval("addr1")+" " +Eval("addr2") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="City" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="City" SortExpression="City" UniqueName="City" />
                                    <telerik:GridBoundColumn DataField="State" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="State" SortExpression="State" UniqueName="State" />
                                    <telerik:GridBoundColumn DataField="Zip" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Zip" SortExpression="Zip" UniqueName="Zip" />
                                    <telerik:GridBoundColumn DataField="BusPhone" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Phone" SortExpression="BusPhone" UniqueName="BusPhone" />
                                    <telerik:GridBoundColumn DataField="AutoBilling" HeaderStyle-Width="10%" DataType="System.Boolean" ItemStyle-HorizontalAlign="Left" HeaderText="Auto Billing" SortExpression="AutoBilling" UniqueName="AutoBilling" />
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" />
                        </telerik:RadGrid>
                        <br />
                        <br />
                        <br />
                    </div>
                </div>
            </asp:PlaceHolder>

        </asp:Panel>

    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="ralpMenu" runat="server" Transparency="70" BackColor="#69b899">
        <i class="fa fa-spinner fa-4x fa-spin"></i>
    </telerik:RadAjaxLoadingPanel>
</asp:Content>


