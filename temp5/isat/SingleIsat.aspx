<%@ Page Title="" Language="C#" MasterPageFile="~/isat/MasterPageIsat.master" AutoEventWireup="true" CodeFile="SingleIsat.aspx.cs" Inherits="Single" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="container">
    <div class="col-sm-9 col-sm-offset-3 col-md-10 col-md-offset-2 main">
        <div class="show-top-grids">
            <div class="col-sm-8 single-left">
                <div class="">
                    <div class="">
                        <h3>Premium Video</h3>
                    </div>
                    <div class="">
                        <img src='<%= Session["VideoLogo"] %>' alt='<%= Session["VideoLogo"] %>' class="img-responsive" />
                    </div>
                    <asp:Label ID="lblFreeWallPaper" CssClass="ModelTittle2 col-xs-12" runat="server"></asp:Label>
                    <asp:Label ID="lblMSISDN" CssClass="ModelTittle2 col-xs-12" runat="server"></asp:Label>
                </div>
                <div style="text-align: center">
                    <asp:Button ID="btnBuy" runat="server" class="btn btn-primary col-xs-12" Text="Beli" OnClick="btnBuy_Click" />
                </div>
                <div class="clearfix"></div>
                <div class="published">
                    <script src="jquery.min.js"></script>
                    <script>
                        $(document).ready(function () {
                            size_li = $("#myList li").size();
                            x = 1;
                            $('#myList li:lt(' + x + ')').show();
                            $('#loadMore').click(function () {
                                x = (x + 1 <= size_li) ? x + 1 : size_li;
                                $('#myList li:lt(' + x + ')').show();
                            });
                            $('#showLess').click(function () {
                                x = (x - 1 < 0) ? 1 : x - 1;
                                $('#myList li').not(':lt(' + x + ')').hide();
                            });
                        });
                    </script>

                </div>

            </div>
        </div>
        <div class="col-md-4 single-right">
            <h3>Up Next</h3>
            <div class="single-grid-right">
                <div class="single-right-grids">
                    <div class="col-md-4 single-right-grid-left">
                        <a href="SingleIsat.aspx">
                            <img src="img/r1.jpg" alt="" /></a>
                    </div>
                    <div class="col-md-8 single-right-grid-right">
                        <a href="SingleIsat.aspx" class="title">Nullam interdum metus</a>
                        <p class="author"><a href="#" class="author">John Maniya</a></p>
                        <p class="views">2,114,200 views</p>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div class="single-right-grids">
                    <div class="col-md-4 single-right-grid-left">
                        <a href="SingleIsat.aspx">
                            <img src="img/r2.jpg" alt="" /></a>
                    </div>
                    <div class="col-md-8 single-right-grid-right">
                        <a href="single.html" class="title">Nullam interdum metus</a>
                        <p class="author"><a href="#" class="author">John Maniya</a></p>
                        <p class="views">2,114,200 views </p>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div class="single-right-grids">
                    <div class="col-md-4 single-right-grid-left">
                        <a href="SingleIsat.aspx">
                            <img src="img/r3.jpg" alt="" /></a>
                    </div>
                    <div class="col-md-8 single-right-grid-right">
                        <a href="SingleIsat.aspx" class="title">Nullam interdum metus</a>
                        <p class="author"><a href="#" class="author">John Maniya</a></p>
                        <p class="views">2,114,200 views</p>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div class="single-right-grids">
                    <div class="col-md-4 single-right-grid-left">
                        <a href="SingleIsatIsat.aspx">
                            <img src="img/r4.jpg" alt="" /></a>
                    </div>
                    <div class="col-md-8 single-right-grid-right">
                        <a href="SingleIsat.aspx" class="title">Nullam interdum metus</a>
                        <p class="author"><a href="#" class="author">John Maniya</a></p>
                        <p class="views">2,114,200 views</p>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div class="single-right-grids">
                    <div class="col-md-4 single-right-grid-left">
                        <a href="SingleIsat.aspx">
                            <img src="img/r1.jpg" alt="" /></a>
                    </div>
                    <div class="col-md-8 single-right-grid-right">
                        <a href="SingleIsat.aspx" class="title">Nullam interdum metus</a>
                        <p class="author"><a href="#" class="author">John Maniya</a></p>
                        <p class="views">2,114,200 views</p>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div class="single-right-grids">
                    <div class="col-md-4 single-right-grid-left">
                        <a href="SingleIsat.aspx">
                            <img src="img/r2.jpg" alt="" /></a>
                    </div>
                    <div class="col-md-8 single-right-grid-right">
                        <a href="SingleISat.aspx" class="title">Nullam interdum metus</a>
                        <p class="author">By <a href="#" class="author">John Maniya</a></p>
                        <p class="views">2,114,200 views</p>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div class="single-right-grids">
                    <div class="col-md-4 single-right-grid-left">
                        <a href="SingleIsat.aspx">
                            <img src="img/r3.jpg" alt="" /></a>
                    </div>
                    <div class="col-md-8 single-right-grid-right">
                        <a href="SingleIsat.aspx" class="title">Nullam interdum metus</a>
                        <p class="author"><a href="#" class="author">John Maniya</a></p>
                        <p class="views">2,114,200 views</p>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div class="single-right-grids">
                    <div class="col-md-4 single-right-grid-left">
                        <a href="SingleIsat.aspx">
                            <img src="img/r4.jpg" alt="" /></a>
                    </div>
                    <div class="col-md-8 single-right-grid-right">
                        <a href="SingleIsat.aspx" class="title">Nullam interdum metus</a>
                        <p class="author"><a href="#" class="author">John Maniya</a></p>
                        <p class="views">2,114,200 views</p>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div class="single-right-grids">
                    <div class="col-md-4 single-right-grid-left">
                        <a href="SingleIsat.aspx">
                            <img src="img/r1.jpg" alt="" /></a>
                    </div>
                    <div class="col-md-8 single-right-grid-right">
                        <a href="SingleIsat.aspx" class="title">Nullam interdum metus</a>
                        <p class="author"><a href="#" class="author">John Maniya</a></p>
                        <p class="views">2,114,200 views</p>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div class="single-right-grids">
                    <div class="col-md-4 single-right-grid-left">
                        <a href="SingleIsat.aspx">
                            <img src="img/r2.jpg" alt="" /></a>
                    </div>
                    <div class="col-md-8 single-right-grid-right">
                        <a href="SingleIsat.aspx" class="title">Nullam interdum metus</a>
                        <p class="author"><a href="#" class="author">John Maniya</a></p>
                        <p class="views">2,114,200 views</p>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div class="single-right-grids">
                    <div class="col-md-4 single-right-grid-left">
                        <a href="SingleIsat.aspx">
                            <img src="img/r3.jpg" alt="" /></a>
                    </div>
                    <div class="col-md-8 single-right-grid-right">
                        <a href="SingleIsat.aspx" class="title">Nullam interdum metus</a>
                        <p class="author"><a href="#" class="author">John Maniya</a></p>
                        <p class="views">2,114,200 views</p>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div class="single-right-grids">
                    <div class="col-md-4 single-right-grid-left">
                        <a href="SingleIsat.aspx">
                            <img src="img/r4.jpg" alt="" /></a>
                    </div>
                    <div class="col-md-8 single-right-grid-right">
                        <a href="SingleIsat.aspx" class="title">Nullam interdum metus</a>
                        <p class="author"><a href="#" class="author">John Maniya</a></p>
                        <p class="views">2,114,200 views</p>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
    </div>

    </div>
		<div class="clearfix"></div>




    <br />
    <div class="crossselling">
        <div class="container">
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
