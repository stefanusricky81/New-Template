<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="NewClientContactSearch.aspx.cs" Inherits="Maintenance_NewClientContactSearch" %>
<%@ Register TagPrefix="ddl" TagName="ClientEmailCategory" Src="~/UserControl/DropDownList/ClientEmailCategory.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style>
        .RadGrid_3b .rgRow td,.RadGrid_3b .rgAltRow td,.RadGrid_3b .rgEditRow td,.RadGrid_3b .rgFooter td,.RadGrid_3b .rgFilterRow td,.RadGrid_3b .rgResizeCol,.RadGrid_3b .rgGroupHeader td
        {
	        padding:4px 5px !important;
	        white-space: nowrap;
            overflow:hidden;
        }

    </style>
    <script type="text/javascript">
        function CheckAll(id) {
            var masterTable = $find("<%= rgClientContact.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == true) {
                for (var i = 0; i < row.length; i++) {
                    masterTable.get_dataItems()[i].findElement("cboxSelect").checked = true; // for checking the checkboxes
                }
            }
            else {
                for (var i = 0; i < row.length; i++) {
                    masterTable.get_dataItems()[i].findElement("cboxSelect").checked = false; // for unchecking the checkboxes
                }
            }
        }
        function unCheckHeader(id) {
            var masterTable = $find("<%= rgClientContact.ClientID %>").get_masterTableView();
            //accessing header checkbox
            var chkBox = $('input[id$="checkAll"]');
            chkBox[0].checked = false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Search Client Contacts : Assign Email Category<asp:Label ID="lbltitle" Visible="false" runat="server" /></h1>
        </div>
    </div>
    <telerik:RadAjaxPanel ID="rapClientContact" runat="server" LoadingPanelID="ralpClientContact" EnableAJAX="true">
        <asp:Literal ID="litMessage" runat="server" />
        <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
            <div class="block">
                <div class="block-title">
                    <h2 style="display: none;"><strong>Search Client</strong></h2>
                    <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="validationSummary" ValidationGroup="vgSearch" />
                </div>
                <div class="row">
                    <div class="form-group col-sm-4">
                        <label>Company</label>
                        <asp:TextBox ID="txtCompany" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                    <div class="form-group col-sm-4">
                        <label>First Name</label>
                        <asp:TextBox ID="txtFirst" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                    <div class="form-group col-sm-4">
                        <label>Last Name</label>
                        <asp:TextBox ID="txtLast" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                </div>
                <div class="row">
                     <div class="form-group col-sm-4">
                        <label>Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                    <div class="form-group col-sm-4">
                        <label>Company Code</label>
                        <asp:TextBox ID="txtCode" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                     <div class="form-group col-sm-4">
                        <label>Contact Priority</label>
                        <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control select-chosen" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-4">
                        <label>Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control select-chosen">
                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                            <asp:ListItem Text="Active" Value="1" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-sm-4">
                        <label>category name</label>
                        <ddl:ClientEmailCategory ID="ddlClientEmailCategory" runat="server" Active="true" Available="CC" DisplayChosenScript="true" />
                    </div>
                </div>
                <div class="form-group form-actions" style="padding-top:20px;">
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgSearch"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                </div>
            </div>
            <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="false">
                <asp:Literal ID="litDebug" runat="server" />
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong>Search Results</strong></h2>
                        <asp:PlaceHolder ID="phScrollNotes" runat="server"></asp:PlaceHolder>
                    </div>
                    <div class="row">
                        <div class="form-group form-actions" style="padding-top:20px;">
                            <asp:LinkButton ID="lbAssign" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbAssign_Click"><i class="hi hi-plus-sign"></i> Assign Email Category </asp:LinkButton>&nbsp;&nbsp;<asp:Label ID="lblAssign" runat="server" /> 
                        </div>
                    </div>
                    <div class="table-responsive">
                        <telerik:RadGrid ID="rgClientContact" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                            OnNeedDataSource="rgClientContact_NeedDataSource" 
                            OnPreRender="rgClientContact_PreRender" 
                            OnItemDataBound="rgClientContact_ItemDataBound" 
                            OnSortCommand="rgClientContact_SortCommand"
                            AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" AllowCustomPaging="true" ShowFooter="false" PageSize="25" Width="100%">
                            <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id,ClientId" Width="100%" CommandItemDisplay="None">
                                <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                                <NoRecordsTemplate>
                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cboxSelect" runat="server" onclick="unCheckHeader(this)" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="ClientId" Display="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%" HeaderText="" UniqueName="ActionColumn">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="btnViewClient" runat="server" ToolTip="View Client" NavigateUrl='<%# Eval("ClientId","clientdetail.aspx?ClientId={0}") %>' style="padding-right:5px;">
                                                <i class="fa fa-building" title="View Client"></i></asp:HyperLink>
                                            <asp:HyperLink ID="btnViewContact" runat="server" ToolTip="View Contact" NavigateUrl='<%# String.Format("ClientContactAdd.aspx?ClientContactID={0}&ClientId={1}",Eval("Id"),Eval("ClientId")) %>'>
                                                <i class="fa fa-user" title="View Contact"></i></asp:HyperLink>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="ClientId" Display="false" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="ClientId" SortExpression="ClientId" UniqueName="ClientId" />
                                    <telerik:GridBoundColumn DataField="UserRanking" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="PR" SortExpression="UserRanking" UniqueName="UserRanking" HeaderTooltip="User Priority Ranking" />
                                    <telerik:GridBoundColumn DataField="Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%" HeaderText="Company Name" SortExpression="Company" UniqueName="Company"  HeaderTooltip="Company Name"/>
                                    <telerik:GridBoundColumn DataField="First" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="First" SortExpression="First" UniqueName="First"  HeaderTooltip="Contact First Name" />
                                    <telerik:GridBoundColumn DataField="Last" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Last" SortExpression="Last" UniqueName="Last" HeaderTooltip="Contact Last Name"/>
                                    <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" HeaderTooltip="Contact Email"/>
                                    <telerik:GridBoundColumn DataField="BusPhone" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Phone" SortExpression="BusPhone" UniqueName="BusPhone" HeaderTooltip="Contact Phone" />
                                    <telerik:GridBoundColumn DataField="Cellphone" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Cell" SortExpression="Cellphone" UniqueName="Cellphone" HeaderTooltip="Contact Cell Phone" />
                                    <telerik:GridBoundColumn DataField="Active" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Active" SortExpression="Active" UniqueName="Active" HeaderTooltip="Contact Active"/>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" />
                        </telerik:RadGrid>
                    </div>
                </div>
            </asp:PlaceHolder>
        </asp:Panel>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="ralpClientContact" runat="server" Transparency="70" BackColor="#69b899">
        <i class="fa fa-spinner fa-4x fa-spin"></i>
    </telerik:RadAjaxLoadingPanel>
</asp:Content>