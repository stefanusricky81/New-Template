<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="About.aspx.cs" Inherits="About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="about-us">
       <div class="about-left">
            <h1>ABOUT US</h1>
                <p>
                    <asp:Label ID="lblLine1" runat="server" TextMode="MultiLine"  />
                </p>
                <br /><br />
                <p>
                    <asp:Label ID="lblLine2" runat="server" TextMode="MultiLine" />
                </p>
                <br /><br />
                <p>
                    <asp:Label ID="lblLine3" runat="server" TextMode="MultiLine" />
                </p>
                <br /><br />
                <p>
                    <asp:Label ID="lblLine4" runat="server" TextMode="MultiLine" />
                </p>
       </div>
       <div class="about-right">
            <img src="images/about1.jpg" />
       </div>
    </div>
</asp:Content>
