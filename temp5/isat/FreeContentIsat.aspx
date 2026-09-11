<%@ Page Title="" Language="C#" MasterPageFile="~/isat/MasterPageIsat.master" AutoEventWireup="true" CodeFile="FreeContentIsat.aspx.cs" Inherits="FreeContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Page Content -->
    <div class="container">
        <div class="row tittlePage">
            <h3>KONTEN GRATIS</h3>
        </div>

        <div class="subhead">
            <div class="">
                <h4 class="bordered text-center"><strong>WALLPAPER GRATIS</strong></h4>
            </div>
        </div>
        <div class="blurred-bg gallery">
            <asp:DataList ID="dlFreeWallPaper" runat="server" RepeatColumns="3">
                <ItemTemplate>
                    <div class="col-xs-6 col-sm-12 col-md-12">
                         <%if(Request["msisdn"] != string.Empty && Request["msisdn"] != null ){ %>
                            <a href='ArtistListIsat.aspx?cat=<%# Eval("WallPaperArtist") %>&msisdn=<%# Request["msisdn"] %>' class="thumbnail">
                        <%  } else {%>
                            <a href="#" class="thumbnail wrapper" data-toggle="modal" data-target=".defaultModal">
                        <%} %>
                            <img src='<%# Eval("WallPaperLogo") %>' alt='<%# Eval("WallPaperTitle") %>' class="imgresponsive" oncontextmenu="return false" />

                            <h4 class="ModelTittle text-center"><%# Eval("WallPaperTitle") %></h4>
                        </a>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <p class="text-center container">
                <asp:Button ID="btnFreeWallpaper" CssClass="btn btn-primary" Visible="false" runat="server" Text="SELEBIHNYA" OnClick="btnFreeWallpaper_Click" />
                <a href="ArtistListIsat.aspx" class="btn btn-primary grayBg">SELEBIHNYA&nbsp; <i class="fa fa-arrow-circle-right "></i></a>
            </p>
        </div>
        <br />
        <div class="subhead">
            <div class="">
                <h4 class="bordered text-center"><strong>VIDEO GRATIS</strong></h4>
            </div>
        </div>
        <div class="gallery blurred-bg">
            <asp:DataList ID="dlFreeVideo" runat="server" Width="100%" RepeatColumns="3">
                <ItemTemplate>
                    <div class="col-xs-6 col-sm-12 col-md-12" data-src='<%# Eval("VideoLogo") %>'>
                        <%if(Request["msisdn"] != string.Empty && Request["msisdn"] != null ){ %>
                            <a href='ArtistListIsat.aspx?cat=<%# Eval("VideoArtist") %>&msisdn=<%# Request["msisdn"] %>' class="thumbnail" >
                        <%  } else {%>
                            <a href="#" class="thumbnail wrapper" data-toggle="modal" data-target=".defaultModal">
                        <%} %>
                            <img class="img-responsive" src='<%# Eval("VideoLogo") %>' alt='<%# Eval("VideoTitle") %>' oncontextmenu="return false" />
                            <h4 class="ModelTittle text-center"><%# Eval("VideoTitle") %></h4>
                        </a>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <p class="text-center container">
                <asp:Button ID="btnFreeVideo" Visible="false" CssClass="btn btn-primary" runat="server" Text="SELEBIHNYA" OnClick="btnFreeVideo_Click" />
                <a href="ArtistListIsat.aspx" class="btn btn-primary grayBg">SELEBIHNYA&nbsp; <i class="fa fa-arrow-circle-right "></i></a>
            </p>
        </div>
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

