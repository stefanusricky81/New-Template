<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="VendorInvoiceSearch.aspx.cs" Inherits="Reports_VendorInvoiceSearch" %>

<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="Server">


    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">       
            function conditionalPostback(e, sender) {
                var theRegexp = new RegExp("\.btnExport$|\.btnViewPdf$", "ig");
                if (sender.EventTarget.match(theRegexp)) {
                    sender.EnableAjax = false;
                }
            }
        </script>
    </telerik:RadScriptBlock>

    <div class="content-header">
        <div class="header-section">
            <h1>Vendor Invoices</h1>
        </div>
    </div>
    <telerik:RadAjaxPanel ID="rapInvoice" runat="server" LoadingPanelID="ralpInvoice" EnableAJAX="true" ClientEvents-OnRequestStart="conditionalPostback">
        <asp:Literal ID="litMessage" runat="server" />
        <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
            <asp:Literal ID="litDebug" runat="server" />
            <div class="block">
                <div class="block-title">
                    <h2><strong>Search Criteria</strong></h2>
                    <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="validationSummary" ValidationGroup="vgSearch" />
                </div>
                <div class="row">
                    <div class="form-group col-sm-4">
                        <label>Vendor</label>
                        <asp:TextBox ID="txtVendor" runat="server" CssClass="form-control" MaxLength="50" PlaceHolder="Enter Vendor ..." />
                        (INGR, SYNN, Advantage, ...)
                    </div>
                    <div class="form-group col-sm-4">
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-4">
                        <label>Start Date</label>
                        <uc:DatePicker ID="ucStartDate" runat="server" IsRequired="false" PlaceHolderText="Enter Start Date ... " />
                    </div>
                    <div class="form-group col-sm-4">
                        <label>End Date</label>
                        <uc:DatePicker ID="ucEndDate" runat="server" IsRequired="false" PlaceHolderText="Enter End Date ... " />
                        <asp:CustomValidator ID="cvDates" runat="server" ControlToValidate="txtVendor" ValidateEmptyText="true" ValidationGroup="vgSearch"
                            ErrorMessage="End Date must be greater than Created Start Date" Display="None" OnServerValidate="cvDates_ServerValidate" />
                    </div>
                </div>
                <div class="form-group form-actions" style="padding-top: 20px;">
                    <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-sm btn-primary" ValidationGroup="vgSearch"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="btnClear" runat="server" OnClick="btnClear_Click" CssClass="btn btn-sm btn-warning" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                </div>
            </div>
            <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="false">
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong>Search Results</strong></h2>
                    </div>
                    <div class="table-responsive" style="margin-left: 10px; margin-right: 10px">
                        <telerik:RadGrid ID="rgInvoice" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                            OnNeedDataSource="rgInvoice_NeedDataSource"
                            OnPreRender="rgInvoice_PreRender"
                            OnItemCommand="rgInvoice_ItemCommand"
                            AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" ShowFooter="false" PageSize="50" Width="100%">
                            <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" CommandItemDisplay="Top">
                                <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                                <NoRecordsTemplate>
                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                </NoRecordsTemplate>
                                <CommandItemTemplate>
                                    <div style="margin: 2px 5px; display: inline; float: right">
                                        <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" CausesValidation="false"><i class="fi fi-csv" title="Export To CSV"></i> Export To CSV</asp:LinkButton>
                                    </div>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Name" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="13%" HeaderText="Vendor Code" SortExpression="Name" UniqueName="Name" />
                                    <%-- <telerik:GridBoundColumn DataField="CompanyName" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%" HeaderText="Vendor Name" SortExpression="CompanyName" UniqueName="CompanyName" />--%>
                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="RefNumber" HeaderStyle-Width="15%" HeaderText="Invoice #" UniqueName="RefNumber">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlTxnID" runat="server" Text='<%# Eval("RefNumber") %>' Target="_blank" NavigateUrl='<%# String.Format("/Reports/VendorInvoiceDetail.aspx?Id={0}", Eval("Id").ToString().Trim()) %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>



                                    <telerik:GridBoundColumn DataField="AmountDue" DataType="System.String" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="14%" HeaderText="Amount" SortExpression="AmountDue" UniqueName="AmountDue" />

                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="IsPaid" HeaderStyle-Width="8%" HeaderText="Paid" UniqueName="Paid">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsPaid" runat="server" Text='<%# (Convert.ToBoolean(Eval("IsPaid"))) ? "Yes" : "No" %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridBoundColumn DataField="TxnDate" DataType="System.DateTime" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="11%" HeaderText="Bill Date" SortExpression="TxnDate" UniqueName="TxnDate" DataFormatString="{0:MM/dd/yyyy}" />
                                    <telerik:GridBoundColumn DataField="DueDate" DataType="System.DateTime" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="11%" HeaderText="Due Date" SortExpression="DueDate" UniqueName="DueDate" DataFormatString="{0:MM/dd/yyyy}" />

                                    <telerik:GridBoundColumn DataField="TermsRef_FullName" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="8%" HeaderText="Terms" SortExpression="TermsRef_FullName" UniqueName="TermsRef_FullName" />
                                    <telerik:GridBoundColumn DataField="Memo" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" HeaderText="Memo" SortExpression="Memo" UniqueName="Memo" />
                                    <telerik:GridBoundColumn DataField="CustomerJobList" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%" HeaderText="Customer:Job" SortExpression="CustomerJobList" UniqueName="CustomerJobList" />
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>
            </asp:PlaceHolder>
        </asp:Panel>

    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="ralpInvoice" runat="server" Transparency="70" BackColor="#69b899">
        <i class="fa fa-spinner fa-4x fa-spin"></i>
    </telerik:RadAjaxLoadingPanel>
</asp:Content>




