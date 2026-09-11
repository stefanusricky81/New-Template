<%@ Page Title="" Language="C#" MasterPageFile="~/Site3.master" AutoEventWireup="true" CodeFile="CCart.aspx.cs" Inherits="CCart" %>
<%@ MasterType VirtualPath="~/Site3.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="kirir" Runat="Server">
    <p style="font-size:15px">LSS COUTURE</p>
    <p style="font-size:15px">LITTLE SUPER STAR</p>
    <p style="font-size:15px">MOMS</p>
    <p style="font-size:15px">ACCESSORIES</p>
    <p style="font-size:15px">SALE</p>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="kananr" Runat="Server">
    <div class="products">
        <h3>My Bag Summary</h3>
        <div style="width:500px; height:auto; float:left; padding:10px 10px 10px 11px;">
            <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="id" EmptyDataText="No Item in the Cart" BackColor="White"  
                CellPadding="3" RowStyle-HorizontalAlign="Center" BorderStyle="None" OnRowDataBound="GV_RowDataBound" OnRowDeleting="gvPurchasing_RowDeleting">
                <HeaderStyle ForeColor="Black"  Font-Bold="True" BackColor="White"/>
                <AlternatingRowStyle BackColor="#F7F7F7" />
                <SelectedRowStyle ForeColor="White" Font-Bold="True" BackColor="#9471DE" />
                <RowStyle ForeColor="Black" BackColor="#DEDFDE" />
                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="PRODUCT" ItemStyle-Width="100px" SortExpression="pName" />
                    <asp:BoundField DataField="Size" HeaderText="SIZE" ItemStyle-Width="150px" SortExpression="quantity" />
                    <asp:BoundField DataField="Weight" HeaderText="WEIGHT PER ITEM" ItemStyle-Width="150px" SortExpression="quantity" />
                    <asp:BoundField DataField="Price" DataFormatString="{0:N0}" HeaderText="PRICE" ItemStyle-Width="150px" SortExpression="price" />
                    <asp:BoundField DataField="Qty" HeaderText="QUANTITY" ItemStyle-Width="150px" SortExpression="quantity" />
                    <asp:CommandField HeaderText="REMOVE" ButtonType="Image" ShowDeleteButton="true" DeleteImageUrl="~/images/DeleteButton2.jpg" />
                    <asp:BoundField HeaderText="TOTAL" DataFormatString="{0:N0}" ItemStyle-Width="150px" />                    
                </Columns>
            </asp:GridView>
            <div align="right">
                <asp:Label ID="lblQty" runat="server" Text="SubTotal : IDR " style=" color:Black; font-weight:bold" />
                <asp:label ID="txtQuantity" runat="server" />
            </div>
            <hr />
            <div align="right">
                <asp:Label ID="lblPrice" runat="server" Text="Total : IDR " style=" color:Black; font-weight:bold"/>
                <asp:label ID="txtPrice" runat="server" />
            </div>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnCheckOut" runat="server" Text="Continue Shopping" BackColor="Gray" ForeColor="Black" Width="175" Height="30" Visible="false" onclick="btnCheckOut_Click"/>
                    </td>
                    <td>
                        <asp:Button ID="btnShopping" runat="server" Text="Continue Shopping" 
                            BackColor="Gray" ForeColor="Black" Width="175" Height="30" 
                            onclick="btnShopping_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnNext" runat="server" Text="Next" BackColor="Gray" ForeColor="Black" Width="100" Height="30" onclick="btnNext_Click"/>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>