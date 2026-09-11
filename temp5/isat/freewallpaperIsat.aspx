<%@ Page Title="" Language="C#" MasterPageFile="~/isat/MasterPageIsat.master" AutoEventWireup="true" CodeFile="freewallpaperIsat.aspx.cs" Inherits="freewallpaper" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Page Content -->
    <div class="container">
        <div class="row tittlePage">
            <h3>WALLPAPER</h3>
        </div>
        <div class="blurred-bg gallery"> 
            <asp:DataList ID="dlFreeWallPaper" runat="server" RepeatColumns="3">
                <ItemTemplate>
                    <div class="col-xs-6 col-sm-12 col-md-12">
                        <a href="singlewallpaperIsat.aspx?cat=<%# Eval("WallPaperID") %>&msisdn=<%# Request["msisdn"] %>" class="thumbnail">
                            <img src='<%# Eval("WallPaperLogo") %>' alt='<%# Eval("WallPaperTitle") %>' class="imgresponsive" oncontextmenu="return false" />

                            <h4 class="ModelTittle text-center"><%# Eval("WallPaperTitle") %></h4>
                        </a>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <br />
        <div class="container">
            <div class="btn-group col-xs-12">
                <asp:Button ID="btnfirst" runat="server" Font-Bold="true" Text="First" CssClass="btn btn-primary btn-md col-xs-3" OnClick="btnfirst_Click" />
                <asp:Button ID="btnprevious" runat="server" Font-Bold="true" Text="Prev" CssClass="btn btn-primary btn-md col-xs-3" OnClick="btnprevious_Click" />
                <asp:Button ID="btnnext" runat="server" Font-Bold="true" Text="Next" CssClass="btn btn-primary btn-md col-xs-3" OnClick="btnnext_Click" />
                <asp:Button ID="btnlast" runat="server" Font-Bold="true" Text="Last" CssClass="btn btn-primary btn-md col-xs-3" OnClick="btnlast_Click" />
            </div>
        </div>

        <br />
        <br />
        <div class="crossselling">
            <div class="row">
                <div class="col-xs-12">
                    <h3 class="text-center"><strong>Servis lainnya</strong></h3>
                    <a href="http://www.gamespro.co.id" target="_blank">
                        <img class="img-responsive gamespro" src="img/gamespro.png" /></a>
                </div>
            </div>
        </div>

        <br />


    </div>

</asp:Content>
