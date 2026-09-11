<%@ Page Title="" Language="C#" MasterPageFile="~/isat/MasterPageIsat.master" AutoEventWireup="true" CodeFile="ArtistListIsat.aspx.cs" Inherits="ArtistList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="myCarousel" class="carousel slide" data-ride="carousel">
        <ol class="carousel-indicators">
            <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
            <li data-target="#myCarousel" data-slide-to="1"></li>
            <li data-target="#myCarousel" data-slide-to="2"></li>
        </ol>
        <div class="carousel-inner">
            <div class="item active">
                <img src="img/BANNER 1-02.jpg" style="width: 100%" alt="First slide">
            </div>
            <div class="item">
                <img src="img/BANNER 2-02.jpg" style="width: 100%" data-src="" alt="Second    slide">
                <div class="container">
                </div>
            </div>
            <div class="item">
                <img src="img/BANNER 3-02.jpg" style="width: 100%" data-src="" alt="Third slide">
            </div>
        </div>
        <a class="left carousel-control" href="#myCarousel" data-slide="prev"><span class="glyphicon glyphicon-chevron-left"></span></a><a class="right carousel-control" href="#myCarousel" data-slide="next"><span class="glyphicon glyphicon-chevron-right"></span></a>
    </div>
    <!-- Page Content -->
    <div class="container">
        <div class="subhead">
            <div class="">
                <h4 class="bordered text-center"><strong>WALLPAPER</strong></h4>
            </div>
        </div>
        <div class="blurred-bg gallery">
            <asp:DataList ID="dlFreeWallPaper" runat="server" RepeatColumns="3">
                <ItemTemplate>
                    <div class="col-xs-6 col-sm-12 col-md-12">
                        <%if(Request["msisdn"] != string.Empty && Request["msisdn"] != null ){ %>
                            <a href='freewallpaperIsat.aspx?cat=<%# Eval("WallPaperArtist") %>&msisdn=<%# Request["msisdn"] %>' class="thumbnail">
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
                <a href="freewallpaperIsat.aspx" class="btn btn-primary grayBg">SELEBIHNYA&nbsp; <i class="fa fa-arrow-circle-right "></i></a>
            </p>
        </div>
        <br />
        <div class="subhead">
            <div class="">
                <h4 class="bordered text-center"><strong>VIDEO</strong></h4>
            </div>
        </div>
        <div class="gallery blurred-bg">
            <asp:DataList ID="dlFreeVideo" runat="server" Width="100%" RepeatColumns="3">
                <ItemTemplate>
                    <div class="col-xs-6 col-sm-12 col-md-12" data-src='<%# Eval("VideoLogo") %>'>
                        <%if(Request["msisdn"] != string.Empty && Request["msisdn"] != null ){ %>
                            <a href='freevideo.aspx?cat=<%# Eval("VideoArtist") %>&msisdn=<%# Request["msisdn"] %>' class="thumbnail">
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
                <a href="freevideoIsat.aspx" class="btn btn-primary grayBg">SELEBIHNYA&nbsp; <i class="fa fa-arrow-circle-right "></i></a>
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
        <div class="row">
            <h3 class="arstSearch">Pencarian Lainnya</h3>
            <div class="container">
                <div id="PopularItem" class="owl-carousel">
                    <asp:Repeater ID="rptPopular" runat="server">
                        <ItemTemplate>
                            <div class="item">
                                 <%if(Request["msisdn"] != string.Empty && Request["msisdn"] != null ){ %>
                                    <a href='ArtistListIsat.aspx?cat=<%# Eval("WallPaperArtist") %>&msisdn=<%# Request["msisdn"] %>' class="thumbnail">
                                <%  } else {%>
                                    <a href="#" class="thumbnail wrapper" data-toggle="modal" data-target=".defaultModal">
                                <%} %>
                                    <img class="img-responsive" src='<%# Eval("WallPaperLogo") %>' alt='<%# Eval("WallPaperTitle") %>' oncontextmenu="return false">
                                    <h4 class="ModelTittle text-center"><%# Eval("WallPaperTitle") %></h4>
                                </a>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
   
</asp:Content>

