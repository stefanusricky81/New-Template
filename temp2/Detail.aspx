<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Detail.aspx.cs" Inherits="Detail" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="center_content">
        <div class="center_title_bar">Products</div>
        <div class="prod_box">
            
            <asp:DataList ID="dlProduct" runat="server" RepeatDirection="Vertical" 
              RepeatColumns="1" BackColor="White" BorderColor="#E7E7FF" 
              BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal">
                <AlternatingItemStyle BackColor="#F7F7F7" />
                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                <ItemStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                <ItemTemplate>
                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>' Visible="false" />

                    <div class="product_img">
                        <asp:Image ID="ImgProduct" runat="server" ImageUrl='<%# Eval("image") %>' Height="350px" Width="375px" />
                    </div>                    
                    <div class="product_title">
                        <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("productName") %>' />
                    </div>
                    <div class="prod_price">
                        <span class="reduce">350$</span>
                    </div>


                </ItemTemplate>
            </asp:DataList>
            <div class="product_title">Quantity
                <asp:TextBox ID="txtQty" runat="server" Columns="5" MaxLength="5" />
            </div>
            <div class="prod_details_tab">
                <asp:ImageButton class="prod_buy" ID="btnAddCart" ToolTip="Add To Cart" ImageUrl="~/images/add-to-cart-button-23443652.jpg" runat="server" onclick="btnAddCart_Click" />
            </div>   
        </div>
    </div>
</asp:Content>

