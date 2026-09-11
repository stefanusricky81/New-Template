<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="UploadInvoiceDetail.aspx.cs" Inherits="Maintenance_UploadInvoiceDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Synnex Billing Import Upload Detail</h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" />
    <asp:PlaceHolder ID="phSearchResults" runat="server">
        <div class="block" style="padding-bottom: 20px;">
            <div class="row">
                <div class="form-group col-sm-12">
                    <asp:Button ID="btnBackUploadInvoice" runat="server" Text="Return to Upload" class="btn btn-primary"  OnClick="btnBackUploadInvoice_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnExport" runat="server" Text="Export Billing" class="btn btn-primary"  OnClick="btnExport_Click" />
                </div>
            </div>
            <div class="table-responsive scroll">
                <telerik:RadGrid ID="rgUploadInvoice" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="10" Width="150%" OnNeedDataSource="rgUploadInvoice_NeedDataSource"
                    ClientSettings-Resizing-AllowColumnResize="true" OnItemDataBound="rgUploadInvoice_ItemDataBound"
                    OnGridExporting="rgUploadInvoice_GridExporting" >
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="150%" AllowSorting="true">
                        <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="10,25,50,100,250" PageSizeControlType="RadComboBox" Position="Bottom" />
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="Id" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="#" Display="false" SortExpression="Id" UniqueName="Id" />
                            <telerik:GridBoundColumn DataField="VendorFullName" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Vendor Name" SortExpression="VendorFullName" UniqueName="VendorFullName" />
                            <telerik:GridBoundColumn DataField="APAccountFullName" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="AP Account Name" SortExpression="APAccountFullName" UniqueName="APAccountFullName" />
                            <telerik:GridBoundColumn DataField="txnDate" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Txn Date" SortExpression="txnDate" UniqueName="txnDate" />
                            <telerik:GridBoundColumn DataField="DueDate" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Due Date" SortExpression="DueDate" UniqueName="DueDate" />
                            <telerik:GridBoundColumn DataField="AmountDue" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Amount Due" SortExpression="AmountDue" UniqueName="AmountDue" />
                            <telerik:GridBoundColumn DataField="CIN" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="RefNum" SortExpression="CIN" UniqueName="CIN" />
                            <telerik:GridBoundColumn DataField="Terms" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="Terms" SortExpression="Terms" UniqueName="Terms" />
                            <telerik:GridBoundColumn DataField="Memo" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="Memo" SortExpression="Memo" UniqueName="Memo" />
                            <telerik:GridBoundColumn DataField="IsPaid" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="IsPaid" SortExpression="IsPaid" UniqueName="IsPaid" />
                            <telerik:GridBoundColumn DataField="OpenAccount" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="Open Account" SortExpression="OpenAccount" UniqueName="OpenAccount" />
                            <telerik:GridBoundColumn DataField="EXPAcocuntFullName" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="EXPAcocuntFullName" SortExpression="EXPAcocuntFullName" UniqueName="EXPAcocuntFullName" />
                            <telerik:GridBoundColumn DataField="Fk_Sku" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="ItemFullName" SortExpression="fk_SKU" UniqueName="fk_SKU" />
                            <telerik:GridBoundColumn DataField="Subscription" DataType="System.String" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Left" HeaderText="Subscription" SortExpression="Subscription" UniqueName="Subscription" />
                            <telerik:GridBoundColumn DataField="Cost" DataType="System.String" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Left" HeaderText="Cost" SortExpression="Cost" UniqueName="Cost" />
                            <telerik:GridBoundColumn DataField="MSRP" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="MSRP" SortExpression="MSRP" UniqueName="MSRP" />
                            <telerik:GridBoundColumn DataField="ItemAmount" DataType="System.String" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Left" HeaderText="ItemAmount" SortExpression="ItemAmount" UniqueName="ItemAmount" />
                            <telerik:GridBoundColumn DataField="ItemCustomerFullName" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Client" SortExpression="ItemCustomerFullName" UniqueName="ItemCustomerFullName" />
                            <telerik:GridBoundColumn DataField="ItemBillableStatus" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="ItemBillableStatus" SortExpression="ItemBillableStatus" UniqueName="ItemBillableStatus" />
                            <telerik:GridBoundColumn DataField="ItemSalesRepRefFullName" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="ItemSalesRepRefFullName" SortExpression="ItemSalesRepRefFullName" UniqueName="ItemSalesRepRefFullName" />                         
                            <telerik:GridBoundColumn DataField="EndUserName" Display="false" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Client" SortExpression="EndUserName" UniqueName="EndUserName" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>