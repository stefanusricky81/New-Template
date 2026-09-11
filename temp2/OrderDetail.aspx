<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="OrderDetail.aspx.cs" Inherits="OrderDetail" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div>
            <center><h1 style="width:945px; color:#000000; font-size:20px; margin-bottom:10px;">Order Detail</h1></center>
        </div>
        
        <div class="sign-in">
            <table>
                <tr>
                    <td>No Order</td>
                    <td></td>
                    <td><asp:Label ID="txtNoOrder" runat="server" Width="240" /></td>
                </tr>
                <tr>
                    <td>Name</td>
                    <td></td>
                    <td><asp:Label ID="txtName" runat="server" Width="240" /></td>
                </tr>
                <tr>
                    <td>Address</td>
                    <td></td>
                    <td><asp:Label ID="txtAddress" runat="server" Width="240" /></td>
                </tr>
                <tr>
                    <td>City</td>
                    <td></td>
                    <td><asp:Label ID="txtCity" runat="server" Width="240" /></td>
                </tr>
                <tr>
                    <td>Telephone</td>
                    <td></td>
                    <td><asp:Label ID="txtTlp" runat="server" Width="240" /></td>
                </tr>
                <tr>
                    <td>Total Price</td>
                    <td></td>
                    <td><asp:Label ID="txtTotal" runat="server" Width="240" /></td>
                    <td><asp:HiddenField ID="hfFlagPaid" runat="server" /></td>
                    <td><asp:HiddenField ID="hfFlagSend" runat="server" /></td>
                </tr>
                <tr>
                    <td><asp:Button ID="btnSend" Text="Send" runat="server" onclick="btnSend_Click" />
                    </td>
                </tr>
            </table>
        </div>
        
        <table align="center">
            <tr>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>      
                <td>
                    <asp:GridView ID="GV" runat="server" AutoGenerateColumns="False"
                        AllowPaging="true" PageSize="5" HeaderStyle-HorizontalAlign="Left"
                        CellPadding="2" CellSpacing="1" BorderColor="Black" BackColor="White"
                        GridLines="None" OnPageIndexChanging="myGridView_PageIndexChanging"
                        EmptyDataText="[ No Data Available ]" Width="620px" >
                        <PagerStyle ForeColor="Black" HorizontalAlign="Center" BackColor="#C6C3C6" />
                        <HeaderStyle ForeColor="#E7E7FF" Font-Bold="True" BackColor="#4A3C8C" />
                        <Columns>
                            <asp:BoundField DataField="ProductName" HeaderText="Product Name" ItemStyle-Width="100px" />
                            <asp:TemplateField HeaderText="Image" SortExpression="img">
                                <ItemTemplate>
                                    <asp:Image ID="Image1" runat="server" Width="100px" Height="100px" ImageUrl='<%# Bind("Image")  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Qty" HeaderText="Quantity" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="Price" DataFormatString="{0:N0}" HeaderText="Price" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="Total_Price" DataFormatString="{0:N0}" HeaderText="Total Price" />
                        </Columns>
                        <SelectedRowStyle ForeColor="White" Font-Bold="True" BackColor="#9471DE" />
                        <RowStyle ForeColor="Black" BackColor="#DEDFDE" />
                    </asp:GridView>
                </td>   
            </tr>
        </table>
</asp:Content>

