<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientSearch.aspx.cs" Inherits="Client_ClientSearch" %>
<%@ Register TagPrefix="ddl" TagName="TicketType" Src="~/UserControl/DropDownList/TicketType.ascx" %>
<%@ Register TagPrefix="lb" TagName="ClientServices" Src="~/UserControl/ListBox/ClientServices.ascx" %>

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
                    <div class="form-group col-sm-3">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="txtCompany" Text="Company" />
                        <asp:TextBox ID="txtCompany" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                    <div class="form-group col-sm-3">
                        <asp:Label ID="lblcode" runat="server" AssociatedControlID="txtCode" Text="Code" />
                        <asp:TextBox ID="txtCode" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                    <div class="form-group col-sm-3">
                        <label>Auto Billing</label>
                        <asp:DropDownList ID="ddlAutoBill" runat="server" CssClass="form-control select-chosen">
                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-3">
                        <label>Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control select-chosen">
                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                            <asp:ListItem Text="Active" Value="Y" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="N"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                     <div class="form-group col-sm-3">
                        <label>Backup Monitor</label>
                        <asp:DropDownList ID="ddlBackupMonitor" runat="server" CssClass="form-control select-chosen">
                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                            <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="N"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-sm-3">
                        <label>Ticket Type</label>
                        <ddl:TicketType ID="ddlTicketType" runat="server" CssClass="form-control select-chosen" IsRequired="false" DefaultValue="" DefaultText="" DisplayDefaultValue="true" SetSize="false"  />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-3">
                        <label>Client Success Manager</label>
                        <asp:DropDownList ID="ddlProjectManager" runat="server" CssClass="form-control select-chosen" />
                    </div>
                    <div class="form-group col-sm-3">
                        <label>Sales Person</label>
                        <asp:ListBox runat="server" ID="ddlSalesPerson" CssClass="form-control select-chosen" Rows="1" SelectionMode="Single"></asp:ListBox>
                    </div>
                    <div class="form-group col-sm-3">
                        <label>Lead Tech</label>
                        <asp:DropDownList ID="ddlTechnicalLead" runat="server" CssClass="form-control select-chosen" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-3">
                        <label>Client Type</label>
                        <asp:DropDownList ID="ddlClientType" runat="server" CssClass="form-control select-chosen" />
                    </div>
                    <div class="form-group col-sm-3">
                        <label>MSP Type</label>
                        <asp:DropDownList ID="ddlMSPType" runat="server" CssClass="form-control select-chosen" />
                    </div>
                     <div class="form-group col-sm-3">
                        <label>TAM</label>
                        <asp:DropDownList ID="ddlTAM" runat="server" CssClass="form-control select-chosen" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-3">
                        <label>BBB Client Services</label>
                        <lb:ClientServices ID="lbClientService" runat="server" DisplayChosenScript="true" />
                    </div>
                    <div class="form-group col-sm-3">
                        <label>Payment method</label>
                        <asp:DropDownList ID="ddlBillingType" runat="server" CssClass="form-control select-chosen">
                            <asp:ListItem Text="" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Credit Card" Value="1" ></asp:ListItem>
                            <asp:ListItem Text="ACH" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Invoice" Value="2"></asp:ListItem>
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
                            OnNeedDataSource="rgClient_NeedDataSource" 
                            OnItemDataBound="rgClient_ItemDataBound"
                            AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" ShowFooter="false" PageSize="25" Width="100%">
                            <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="pclient" Width="100%" CommandItemDisplay="Top">
                                <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                                <NoRecordsTemplate>
                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                </NoRecordsTemplate>
                                <CommandItemTemplate>
                                    <div style="margin: 2px 5px; display: inline; float: left">
                                        <asp:HyperLink ID="hlAddNewRecord" runat="server" Enabled="true" NavigateUrl="ClientDetail.aspx?ClientId=0"><i class="gi gi-user_add" title="Add New Pilot Information"></i>  Add New Client</asp:HyperLink>
                                    </div>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn DataField="pclient" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="5%" HeaderText="" UniqueName="ActionColumn">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="btnEditRecord" runat="server" NavigateUrl='<%# Eval("pclient","clientdetail.aspx?ClientId={0}") %>' Style="padding-right: 8px;"><i class="gi gi-pencil" title="Edit"></i></asp:HyperLink>
                                            <asp:HyperLink ID="btnEditBilling" runat="server" NavigateUrl='<%# Eval("pclient","ClientBilling.aspx?ClientId={0}") %>'><i class="gi gi-money" title="View/Edit Billing"></i></asp:HyperLink>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Code" HeaderStyle-Width="4%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Code" SortExpression="Code" UniqueName="Code" />
                                    <telerik:GridBoundColumn DataField="pclient" HeaderStyle-Width="4%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Id" SortExpression="pclient" UniqueName="pclient" />
                                    <telerik:GridTemplateColumn DataField="pclient" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="15%" HeaderText="Company Name" UniqueName="ActionColumn">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="btnEditRecord" runat="server" ToolTip="View Client" Text='<%# Eval("Company") %>' NavigateUrl='<%# Eval("pclient","clientdetail.aspx?ClientId={0}") %>'></asp:HyperLink>
                                            &nbsp;
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="addr1" ItemStyle-HorizontalAlign="Left" SortExpression="addr1" HeaderStyle-Width="12%" HeaderText="Address" UniqueName="addr1">
                                        <ItemTemplate>
                                            <%# Eval("addr1")+" " +Eval("addr2") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="City" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="City" SortExpression="City" UniqueName="City" />
                                    <telerik:GridBoundColumn DataField="State" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="State" SortExpression="State" UniqueName="State" />
                                    <telerik:GridBoundColumn DataField="Zip" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Zip" SortExpression="Zip" UniqueName="Zip" />
                                    <telerik:GridBoundColumn DataField="BusPhone" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Phone" SortExpression="BusPhone" UniqueName="BusPhone" />
                                    <telerik:GridBoundColumn DataField="ChargeFutureInvoices" HeaderStyle-Width="4%" DataType="System.Boolean" ItemStyle-HorizontalAlign="Left" HeaderText="Auto Billing" SortExpression="ChargeFutureInvoices" UniqueName="ChargeFutureInvoices" />
                                    <telerik:GridBoundColumn DataField="BackupMonitorActive" HeaderStyle-Width="4%" DataType="System.Boolean" ItemStyle-HorizontalAlign="Left" HeaderText="Backup Monitor" SortExpression="BackupMonitorActive" UniqueName="BackupMonitorActive" />
                                    <telerik:GridBoundColumn DataField="Active" HeaderStyle-Width="5%" DataType="System.String" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Active" SortExpression="Active" UniqueName="Active" />
                                    <telerik:GridBoundColumn DataField="ProjectManager" HeaderStyle-Width="5%" DataType="System.String" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="PM" SortExpression="ProjectManager" UniqueName="ProjectManager" />
                                    <telerik:GridBoundColumn DataField="techlead" HeaderStyle-Width="5%" DataType="System.String" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Tech Lead" SortExpression="techlead" UniqueName="techlead" />
                                    <telerik:GridBoundColumn DataField="clienttype" HeaderStyle-Width="5%" DataType="System.String" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Client Type" SortExpression="clienttype" UniqueName="clienttype" />
                                    <telerik:GridBoundColumn DataField="salesperson" HeaderStyle-Width="5%" DataType="System.String" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Sales Person" SortExpression="salespeson" UniqueName="salespeson" />
                                    <telerik:GridBoundColumn DataField="ClientServiceName" HeaderStyle-Width="5%" DataType="System.String" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Client Service" SortExpression="ClientServiceName" UniqueName="ClientServiceName" />

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


