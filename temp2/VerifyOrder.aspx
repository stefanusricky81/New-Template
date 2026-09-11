<%@ Page Title="" Language="C#" MasterPageFile="~/Site3.master" AutoEventWireup="true" CodeFile="VerifyOrder.aspx.cs" Inherits="VerifyOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="kirir" Runat="Server">
    <asp:Label ID="lblStatus" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="kananr" Runat="Server">
    <div class="products">
        <h3>Verify Your Order</h3>
        <div style="width:500px; height:auto; float:left; padding:10px 10px 10px 11px;">
            <table>
                <tr>
                    <td>Name</td>
                    <td></td>
                    <td>
                        <asp:Label ID="lblName" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>ID Customer</td>
                    <td></td>
                    <td>
                        <asp:Label ID="lblIdCust" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Address</td>
                    <td></td>
                    <td>
                        <asp:Label ID="lblAddress" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>City</td>
                    <td></td>
                    <td>
                        <asp:Label ID="lblCity" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Phone</td>
                    <td></td>
                    <td>
                        <asp:Label ID="lblPhone" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Email</td>
                    <td></td>
                    <td>
                        <asp:Label ID="lblEmail" runat="server" />
                    </td>
                </tr
                <tr>
                    <td>Date Order</td>
                    <td></td>
                    <td>
                        <asp:Label ID="lblDateOrder" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Courier</td>
                    <td></td>
                    <td>
                        <asp:DropDownList ID="ddlEkspedisi" runat="server" onselectedindexchanged="ddlEkspedisi_SelectedIndexChanged" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>Account Name</td>
                    <td></td>
                    <td>
                        <asp:DropDownList ID="ddlBankAcc" runat="server" onselectedindexchanged="ddlBankAcc_SelectedIndexChanged" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>Account Number</td>
                    <td></td>
                    <td>
                        <asp:Label ID="lblAccNo" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="width:500px; height:auto; float:left; padding:10px 10px 10px 11px;">
            <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="id" EmptyDataText="No Item in the Cart" BackColor="White"  
                BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="1px" GridLines="Both"
                CellPadding="3" RowStyle-HorizontalAlign="Center" OnRowDataBound="GV_RowDataBound" OnRowDeleting="gvPurchasing_RowDeleting">
                    <HeaderStyle ForeColor="#E7E7FF" Font-Bold="True" BorderStyle="Solid" BackColor="GrayText" />
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                    <SelectedRowStyle ForeColor="White" Font-Bold="True" BackColor="#9471DE" />
                    <RowStyle ForeColor="Black" BackColor="#DEDFDE" />
                    <Columns>
                        <asp:BoundField DataField="ProductName" HeaderText="PRODUCT" ItemStyle-Width="100px" SortExpression="pName" />
                        <asp:BoundField DataField="Size" HeaderText="SIZE" ItemStyle-Width="150px" SortExpression="quantity" />
                        <asp:BoundField DataField="Weight" HeaderText="WEIGHT PER ITEM" ItemStyle-Width="150px" SortExpression="quantity" />
                        <asp:BoundField DataField="Price" DataFormatString="{0:N0}" HeaderText="PRICE" ItemStyle-Width="150px" SortExpression="price"  />
                        <asp:BoundField DataField="Qty" HeaderText="QUANTITY" ItemStyle-Width="150px" SortExpression="quantity" />
                        <asp:CommandField HeaderText="REMOVE" ButtonType="Image" ShowDeleteButton="true" DeleteImageUrl="~/images/DeleteButton2.jpg" />
                        <asp:BoundField HeaderText="Total Price" DataFormatString="{0:N0}" ItemStyle-Width="150px" />  
                    </Columns>
            </asp:GridView>
            <div align="right">
                <asp:Label ID="lblTotalOfOrder" runat="server" Text="Total Of Order : IDR " style=" color:Black; font-weight:bold; " />
                <asp:label ID="txtTotalOfOrder" runat="server" />    
            </div>
            <div align="right">
                <asp:Label ID="lblTotalOfShipping" runat="server" Text="Total Of Shipping : IDR " style=" color:Black; font-weight:bold; " />
                <asp:label ID="txtTotalOfShipping" runat="server" Text="0"  />
            </div>
            <hr />
            <div align="right">
                <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total : IDR " style=" color:Black; font-weight:bold"/>
                <asp:label ID="txtGrandTotal" runat="server" />
                <asp:HiddenField ID="hfAccountName" runat="server" />
            </div>
             <table>
                <tr>
                    <td>
                        <asp:Button ID="btnConfirmOrder" runat="server" Text="Confirm Order" 
                            BackColor="Gray" ForeColor="Black" Width="175" Height="30" 
                            onclick="btnConfirmOrder_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

