<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="Accessories.aspx.cs" Inherits="LssCulture" %>
<%@ MasterType VirtualPath="~/Site2.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="kirir" Runat="Server">
    <p style="font-size:15px"><a href="LssCouture.aspx">LSS COUTURE</a></p>
    <p style="font-size:15px"><a href="Lss.aspx">LITTLE SUPER STAR</a></p>
    <p style="font-size:15px"><a href="Moms.aspx">MOMS</a></p>
    <p style="font-size:15px"><a href="Accessories.aspx">ACCESSORIES</a></p>
    <ul class="kiri-1">
       <li>GIRLS</li>
       <ul>
             <li><a href="Accessories.aspx?f=GT">Tops</a></li>
             <li><a href="Accessories.aspx?f=GB">Bottoms</a></li>
             <li><a href="Accessories.aspx?f=GD">Dresses</a></li>
             <li><a href="Accessories.aspx?f=GJ">Baby</a></li>
       </ul>
       <li>BOYS</li>
       <ul>
             <li><a href="Accessories.aspx?f=BT">Tops</a></li>
             <li><a href="Accessories.aspx?f=BB">Bottoms</a></li>
             <li><a href="Accessories.aspx?f=BD">Dresses</a></li>
             <li><a href="Accessories.aspx?f=BJ">Baby</a></li>
       </ul>
    </ul>
    <p style="font-size:15px"><a href="Sale.aspx">SALE</a></p>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="kananr" Runat="Server">
    <div class="products">
        <asp:DataList ID="dlProduct" runat="server" RepeatDirection="Horizontal" 
                RepeatColumns="3" RepeatLayout="Table" BackColor="White" BorderStyle="None" CellPadding="3" >
                
                <ItemTemplate>
                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>' Visible="false" />
                    <asp:Image ID="ImgProduct" runat="server" Height="300px" Width="220px" ImageUrl='<%# Eval("image") %>' />
                    <p>
                       <asp:HyperLink ID="hpCart" runat="server" NavigateUrl='<%# ((int)Eval("product_status")) == 2 ? "LSSDetail.aspx?ID=" + Eval("id") +"&d=N&OOS=Y":"LSSDetail.aspx?ID=" + Eval("id") +"&d=N&OOS=N" %>'>
                            <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("productName") %>' />
                        </asp:HyperLink>
                    </p>
                    <div style="float:right; background-color:Black; color:#ffffff; padding:1px; ">
                        <asp:Label ID="lblTag" runat="server" Text='<%# ProcessMyDataItem(Eval("product_status")) %>' />
                    </div>
                    <p><b>IDR &nbsp<asp:Label ID="lblPrice" runat="server" Text='<%# Eval("price","{0:0,0}") %>' /></b></p>
                </ItemTemplate>
                <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
        </asp:DataList>
    </div>
    <table width="30%" align="center">
        <tr>
            <td>
                <asp:Button ID="btnfirst" runat="server" Font-Bold="true" Text="<<" Height="31px" Width="43px" onclick="btnfirst_Click" />
            </td>
            <td>
                <asp:Button ID="btnprevious" runat="server" Font-Bold="true" Text="<" Height="31px" Width="43px" onclick="btnprevious_Click" />
            </td>
            <td>
                <asp:Button ID="btnnext" runat="server" Font-Bold="true" Text=">" Height="31px" Width="43px" onclick="btnnext_Click"/>
            </td>
            <td>
                <asp:Button ID="btnlast" runat="server" Font-Bold="true" Text=">>" Height="31px" Width="43px" onclick="btnlast_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
