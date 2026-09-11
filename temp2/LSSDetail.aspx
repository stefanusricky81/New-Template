<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="LSSDetail.aspx.cs" Inherits="contact" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<script type="text/javascript">
    $(function () {
        $('#demo > a:first').fancyzoom({ Speed: 400, showoverlay: false });
        $('#demo > a:last').fancyzoom({ Speed: 400, showoverlay: false });
        $('#nooverlay').fancyzoom({ Speed: 400, showoverlay: false });
        $('img.fancyzoom').fancyzoom();
    });
</script>

<style type="text/css">
    A IMG {border:0;}
    A{text-decoration:none;color:#000;}
    #pageWrapper{
    margin:0 auto;
    width:1000px;
    border:1px solid #000;
    background:#FFF;
    padding: 0px 20px 40px 20px;	
    }
    h1{text-align:right;font-size:24px;}
    h2{font-size:16px;border-bottom:1px solid #CCC;margin-top:40px;}
    h3{font-size:14px;border-bottom:1px solid #CCC;margin-left:40px;}
    #demo A {
    display:block;
    float:left;
    width:400px;
    text-align:left;
    text-decoration:none;
    color:#000;
    font-size:11px;
    }
    #demo{
    padding-left:200px;
    }
    #demo  ul {text-align:left;color:#000;}
    p.code{
    margin-left:60px;
    }
    pre{
    margin-left:60px;
    background:#CCC;
    padding:6px;
    }

</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<br /><br /><br />
      <div class="prod_box_big">
        <div class="center">
            <p>Home/Little Superstar</p>
            <div class="left_dt">
                <asp:Image ID="ImgProduct" runat="server" />
                <center>
                <asp:DataList ID="dlDetailProductPic" runat="server" RepeatDirection="Horizontal" 
                                RepeatColumns="4"  BackColor="White" BorderStyle="None" CellPadding="3" HorizontalAlign="Left" >
                    <ItemTemplate>
                        <asp:Image ID="ImgDetailProduct" runat="server" ImageUrl='<%# Eval("IMAGE_DETAIL") %>' style=" width:70px; height:100px; margin-right:3px;" class="fancyzoom" />
                    </ItemTemplate>
                </asp:DataList>
            </div>
            
            <div class="right_dt">
                 <p>
                    <b><asp:Label ID="lblBrand" runat="server" /></b>
                    <br />
                    IDR <asp:Label ID="lblPrice" runat="server" />
                 </p>
                 <p>
                    Description :
                    <br />                   
                    <asp:TextBox ID="lblDesc" ReadOnly="true" BorderStyle="None" style="font-size:15px; font-family:Times New Roman"
                         TextMode="MultiLine" Rows="2" runat="server" Height="59px" Width="400px" />
                 </p>
                 <p>
                    Material :
                    <br />
                    <asp:Label ID="lblMaterial" runat="server" />
                 </p>
                 <p>
                    Care :
                    <br />
                    <asp:Label ID="lblCare" runat="server" />
                 </p>
                <div class="det-brg">
                    <div style=" height:200px; width:240px; background-color:#cccccc;">
                        <p style="font-size:18px; color:gray;" >CHOOSE COLOR</p>
                        <asp:DropDownList ID="ddlColor" runat="server" style="font-size:medium; font-weight:bold" />
                        <p style="font-size:18px; color:gray;" >CHOOSE SIZE</p>
                        <asp:DropDownList ID="ddlSize" runat="server" style="font-size:medium; font-weight:bold" />
                        <p style="font-size:18px; color:gray;" >QUANTITY</p>
                        <asp:TextBox ID="txtQty" runat="server" style="font-size:medium; font-weight:bold" />
                    </div>
                    <div style="height:200px; width:220px; padding:60px 0px 15px 0px;clear:both;">
                        <asp:Button ID="btnAddToBag" Text="ADD TO BAG" Width="200" runat="server" Height="40" 
                            BackColor="Black" ForeColor="White" onclick="btnAddToBag_Click" />
                        <asp:Button ID="btnCheckOut" Text="CHECKOUT" Width="200" runat="server" 
                            Height="40" BackColor="Gray" ForeColor="White" onclick="btnCheckOut_Click" />
                    </div>
                </div>
            </div>
            <br /><br />
           
        </div>
      </div>
</asp:Content>

