<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="Lss.aspx.cs" Inherits="Lss" %>
<%@ MasterType VirtualPath="~/Site2.master" %>



<asp:Content ID="Content1" ContentPlaceHolderID="kirir" Runat="Server">
    <p style="font-size:15px"><a href="LssCouture.aspx">LSS COUTURE</a></p>
    <p style="font-size:15px"><a href="Lss.aspx">LITTLE SUPER STAR</a></p>
    <ul class="kiri-1">
       <li>GIRLS</li>
       <ul>
             <li><a href="Lss.aspx?f=GT">Tops</a></li>
             <li><a href="Lss.aspx?f=GB">Bottoms</a></li>
             <li><a href="Lss.aspx?f=GD">Dresses</a></li>
             <li><a href="Lss.aspx?f=GJ">Baby</a></li>
       </ul>
       <li>BOYS</li>
       <ul>
             <li><a href="Lss.aspx?f=BT">Tops</a></li>
             <li><a href="Lss.aspx?f=BB">Bottoms</a></li>
             <li><a href="Lss.aspx?f=BD">Dresses</a></li>
             <li><a href="Lss.aspx?f=BJ">Baby</a></li>
       </ul>
    </ul>
    <p style="font-size:15px"><a href="Moms.aspx">MOMS</a></p>
    <p style="font-size:15px"><a href="Accessories.aspx">ACCESSORIES</a></p>
    <p style="font-size:15px"><a href="Sale.aspx">SALE</a></p>
    

    
    <script type="text/javascript">
    function fnZoomImage(imageUrl) 
    {
     var imgDiv = document.getElementById("divImage");
     var imgZoom = document.getElementById("imgZoom");
     imgZoom.src = imageUrl.replace("Thumbnail", "Full");
     var width = document.body.clientWidth;
     imgDiv.style.left = (width - 650) / 2 + "px";
     imgDiv.style.display = "block";
     imgZoom.style.display = "block";
     return false;
    }

    function HideImage() 
    {
        var imgDiv = document.getElementById("divImage");
        var imgZoom = document.getElementById("imgZoom");
        imgDiv.style.display = "none";
        imgZoom.style.display = "none";
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="kananr" Runat="Server">
    <div class="products">
        <asp:DataList ID="dlProduct" runat="server" RepeatDirection="Horizontal" 
                RepeatColumns="3" RepeatLayout="Table" BackColor="White" BorderStyle="None" CellPadding="3" >
                
                <ItemTemplate>
                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>' Visible="false" />
                    <asp:Image ID="ImgProduct" runat="server" Height="300px" Width="220px" ImageUrl='<%# Eval("image") %>' onmouseover = "fnZoomImage(this.src);"
                            onmouseout = "HideImage();" />
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

    <div id="divImage" style=" display:none; position:fixed; top:20px; left:0px; background-color:#FFFFFF; height:500px; width:600px; padding:3px; border:solid 1px #525252;">
        <table style="height: 100%; width: 100%">
            <tr>
                <td valign="middle" align="center">             
                    <img id="imgZoom" runat="server" style="display: none;"/>
                </td>
            </tr>      
        </table>
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

