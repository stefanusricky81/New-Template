<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="EventPromo.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="how">
       <div class="how-left">
            <asp:DataList ID="dlProduct" runat="server" RepeatDirection="Horizontal" 
                RepeatColumns="1" BackColor="White" BorderStyle="None" CellPadding="3" GridLines="None" >
                <ItemTemplate>
                    <asp:Image ID="ImgProduct" runat="server" Height="300px" Width="600px" ImageUrl='<%# Eval("IMAGE") %>' />
                </ItemTemplate>
            </asp:DataList>
       </div>
       <div class="how-right" style="vertical-align:top">
            <asp:Image ID="ImgProduct" runat="server" ImageUrl="~/images/event2.jpg" />
       </div>
       
    </div>
</asp:Content>
