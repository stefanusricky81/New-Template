<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <script type="text/javascript">
            function show_alert() {
                alert("Hello! I am an alert box!");
            }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="myCarousel" class="carousel slide" data-ride="carousel">
        <ol class="carousel-indicators">
            <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
            <li data-target="#myCarousel" data-slide-to="1"></li>
            <li data-target="#myCarousel" data-slide-to="2"></li>
        </ol>
                <div class="carousel-inner">
            <asp:Repeater ID="rptBanner" runat="server" >
                <ItemTemplate>
                    <div class="item SliderItem">
                        <img src='<%# Eval("BannerAddress") %>' style="width: 100%" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>          
        </div>
        <a class="left carousel-control" href="#myCarousel" data-slide="prev"><span class="glyphicon glyphicon-chevron-left"></span></a><a class="right carousel-control" href="#myCarousel" data-slide="next"><span class="glyphicon glyphicon-chevron-right"></span></a>
    </div>
    <div class="container" style="text-align: center;">
        <div class="row page-header blurred-bg">
            <div class="col-xs-12 ">
                <h2 class="no-margin">DOWNLOAD SEPUASNYA
                </h2>
            </div>
        </div>
        <br />

        <div class="subhead">
            <div class="">
                <h3 class="bordered text-center" style="letter-spacing:2px;"><strong>KONTEN TERBARU</strong><span></span></h3>
            </div>
        </div>
        <div class="blurred-bg gallery border0">
            <h4 class="cstmhometitle">WALLPAPER</h4>
            <asp:DataList ID="dlFreeWallPaper" runat="server" Width="100%" RepeatColumns="3" RepeatLayout="Table" CellPadding="3">
                <ItemTemplate>
                    <div class="col-xs-6 col-sm-12 col-md-12 col-lg-12" data-src='<%# Eval("WallPaperLogo") %>'>
                        <%if(Request["msisdn"] != string.Empty && Request["msisdn"] != null ){ %>
                            <a href='LatestWallpaper.aspx?cat=<%# Eval("WallPaperArtist") %>&msisdn=<%# Request["msisdn"] %>' class="thumbnail wrapper">
                        <%  } else {%>
                            <a href="#" class="thumbnail wrapper" data-toggle="modal" data-target=".defaultModal">
                        <%} %>
                       
                            <div class='description'>
                                <p class='description_content ellipsis-preview '>PREVIEW</p>
                            </div>
                            <img src='<%# Eval("WallPaperLogo") %>' alt='<%# Eval("WallPaperTitle") %>' class="img-responsive" oncontextmenu="return false" />
                            <h4 class="ModelTittle text-center"><%# Eval("WallPaperTitle") %></h4>
                        </a>
                    </div>

                </ItemTemplate>
            </asp:DataList>
            <p class="text-center container">
                <asp:Button ID="btnFreeWallpaper" CssClass="btn btn-primary" Visible="false" runat="server" Text="SELEBIHNYA" OnClick="btnFreeWallpaper_Click" />
                <a href="LatestWallpaper.aspx" class="btn btn-primary grayBg">SELEBIHNYA&nbsp; <i class="fa fa-arrow-circle-right "></i></a>
            </p>
        </div>
        <div class="gallery blurred-bg">
            <h4 class="cstmhometitle">VIDEOS</h4>
            <asp:DataList ID="dlFreeVideo" runat="server" Width="100%" RepeatColumns="3">
                <ItemTemplate>
                    <div class="col-xs-6 col-sm-12 col-md-12 col-lg-12" data-src='<%# Eval("VideoLogo") %>'>
                        <%if(Request["msisdn"] != string.Empty && Request["msisdn"] != null ){ %>
                            <a href='LatestVideo.aspx?cat=<%# Eval("VideoArtist") %>&msisdn=<%# Request["msisdn"] %>' class="thumbnail wrapper">
                        <%  } else {%>
                            <a href="#" class="thumbnail wrapper" data-toggle="modal" data-target=".defaultModal">
                        <%} %>
                            <img src='<%# Eval("VideoLogo") %>' alt='<%# Eval("VideoTitle") %>' class="img-responsive" oncontextmenu="return false" />
                            <h4 class="ModelTittle text-center"><%# Eval("VideoTitle") %></h4>
                        </a>
                    </div>

                </ItemTemplate>
            </asp:DataList>
            <p class="text-center container">
                <asp:Button ID="btnFreeVideo" Visible="false" CssClass="btn btn-primary" runat="server" Text="SELEBIHNYA" OnClick="btnFreeVideo_Click" />
                <a href="LatestVideo.aspx" class="btn btn-primary grayBg">SELEBIHNYA&nbsp; <i class="fa fa-arrow-circle-right "></i></a>
            </p>
        </div>


        <div class="subhead">
            <div class="">
                <h4 class="bordered text-center"><strong>PREMIUM VIDEO</strong></h4>
            </div>
        </div>
        <div class="blurred-bg gallery">
            <asp:DataList ID="dlPremiumVideo" runat="server" Width="100%" RepeatColumns="3">
                <ItemTemplate>
                    <div class="col-xs-6 col-sm-12 col-md-12 col-lg-12" data-src='<%# Eval("VideoLogo") %>'>
                        <a href='premiumvideo.aspx?cat=<%# Eval("VideoArtist") %>&msisdn=<%# Request["msisdn"] %>'  class="thumbnail wrapper">
                            <img src='<%# Eval("VideoLogo") %>' alt='<%# Eval("VideoTitle") %>' class="img-responsive" oncontextmenu="return false" />
                            <h4 class="ModelTittle text-center"><%# Eval("VideoTitle") %></h4>
                        </a>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <p class="text-center container">
                <asp:Button ID="btnPremiumVideo" Visible="false" CssClass="btn btn-primary" runat="server" Text="SELEBIHNYA" OnClick="btnPremiumVideo_Click" />
                <a href="premiumvideo.aspx" class="btn btn-primary grayBg">SELEBIHNYA&nbsp; <i class="fa fa-arrow-circle-right "></i></a>
            </p>
        </div>

        <br />
        <div class="crossselling">
            <div class="row">
                <div class="col-xs-12">
                    <h3 class="text-center"><strong>Servis lainnya</strong></h3>
                    <div id="myCarousel22" class="carousel slide" data-ride="carousel">
                        <ol class="carousel-indicators">
                            <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
                            <li data-target="#myCarousel" data-slide-to="1"></li>
                            <li data-target="#myCarousel" data-slide-to="2"></li>
                        </ol>
                        <div class="carousel-inner">
                            <div class="item active">
                                <img src="img/banner1.jpg" style="width: 100%" alt="">
                            </div>
                            <div class="item">
                                <img src="img/banner2.gif" style="width: 100%" data-src="" alt="">
                                <div class="container">
                                </div>
                            </div>
                            <div class="item">
                                <img src="img/banner3.jpg" style="width: 100%" data-src="" alt="">
                            </div>
                        </div>
                        <a class="left carousel-control" href="#myCarousel22" data-slide="prev"><span class="glyphicon glyphicon-chevron-left"></span></a><a class="right carousel-control" href="#myCarousel22" data-slide="next"><span class="glyphicon glyphicon-chevron-right"></span></a>
                    </div>
                </div>
            </div>
        </div>
        <br />
    </div>
</asp:Content>
