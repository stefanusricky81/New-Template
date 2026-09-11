<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefaultNew.aspx.cs" Inherits="Employee_DefaultNew" MasterPageFile="~/Template/Responsive.master" %>

<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Search Employee</h1>
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
                        <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" Text="First Name" />
                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                    <div class="form-group col-sm-3">
                        <asp:Label ID="lblLastName" runat="server" AssociatedControlID="txtLastName" Text="Last Name" />
                        <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                    <div class="form-group col-sm-2">
                        <asp:Label ID="lblTeam" runat="server" AssociatedControlID="ddlTeam" Text="Team" />
                        <asp:DropDownList ID="ddlTeam" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group col-sm-2">
                        <asp:Label ID="lblTiers" runat="server" AssociatedControlID="ddlTier" Text="Tier" />
                        <asp:DropDownList ID="ddlTier" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group col-sm-2">
                        <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" Text="Status" />
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                            <asp:ListItem Selected="True" Text="Active" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
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
                        <telerik:RadGrid ID="rgEmployee" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                            OnNeedDataSource="rgEmployee_NeedDataSource" 
                            OnItemDataBound="rgEmployee_ItemDataBound"
                            AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" ShowFooter="false" PageSize="25" Width="100%">
                            <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="EmployeeId" Width="100%" CommandItemDisplay="Top">
                                <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                                <NoRecordsTemplate>
                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                </NoRecordsTemplate>
                                <CommandItemTemplate>
                                    <div style="margin: 2px 5px;">
                                        <asp:HyperLink ID="hlAddNewRecord" runat="server" NavigateUrl="DetailNew.aspx?Id=0"><i class="gi gi-user_add" title="Add New Employee Information"></i>  Add New Employee</asp:HyperLink>
                                    </div>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn DataField="pemployee" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%" HeaderText="" UniqueName="ActionColumn">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="btnEditRecord" runat="server" NavigateUrl='<%# Eval("EmployeeId","DetailNew.aspx?Id={0}") %>'><i class="gi gi-pencil" title="Edit"></i></asp:HyperLink>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Code" HeaderStyle-Width="8%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Code" SortExpression="Code" UniqueName="Code" />
                                    <telerik:GridBoundColumn DataField="First" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="First Name" SortExpression="First" UniqueName="First" />
                                    <telerik:GridBoundColumn DataField="Last" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Last Name" SortExpression="Last" UniqueName="Last" />
                                    <telerik:GridBoundColumn DataField="BusExt" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ext" SortExpression="BusExt" UniqueName="BusExt" />
                                    <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" />
                                    <telerik:GridBoundColumn DataField="TierName" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Tier" SortExpression="TierName" UniqueName="TierName" />
                                    <telerik:GridBoundColumn DataField="TeamName" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Team" SortExpression="TeamName" UniqueName="TeamName" />
                                    <telerik:GridBoundColumn DataField="Empltype" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Type" SortExpression="Empltype" UniqueName="Empltype" />
                                    <telerik:GridBoundColumn DataField="TeamPhone" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Teams Phone" SortExpression="TeamPhone" UniqueName="TeamPhone" />
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>
            </asp:PlaceHolder>
        </asp:Panel>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="ralpMenu" runat="server" Transparency="70" BackColor="#69b899">
        <i class="fa fa-spinner fa-4x fa-spin"></i>
    </telerik:RadAjaxLoadingPanel>
</asp:Content>

<asp:Content ID="cphEnd" ContentPlaceHolderID="cphEnd" runat="Server">
</asp:Content>

