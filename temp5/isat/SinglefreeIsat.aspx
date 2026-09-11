<%@ Page Title="" Language="C#" MasterPageFile="~/isat/MasterPageIsat.master" AutoEventWireup="true" CodeFile="SinglefreeIsat.aspx.cs" Inherits="Singlefree" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="container">
        <div class="row">
            <div class="col-xs-12">
                    <div class="row tittlePage">
                        <h3>VIDEO</h3>
                    </div>

                    <div class="video-grid">
                        <video controls>
                            <source src="<%= Session["urlFree"]%>" type="<%= Session["vtypeFree"]%>" />
                            Your browser does not support HTML5 video.
                        </video>
                        <asp:Label ID="lblFreeWallPaper" CssClass="ModelTittle2 whiteClr col-xs-12" runat="server"></asp:Label>
                    </div>
                <div class="clearfix"></div>
<%--                <div class="published">
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

                </div>--%>

            </div>
        </div>
        <div class="col-xs-12 ">
            <h4 class="whiteClr text-right">Up Next</h4>
            <div class="">
                <asp:DataList ID="dlUpnext" runat="server" Width="100%" RepeatColumns="1" RepeatLayout="Table" CellPadding="3">
                    <ItemTemplate>
                        <div class="gallery blurred-bg">
                            <div class="col-xs-6">
                                <a href='SinglefreeIsat.aspx?cat=<%# Eval("VideoId") %>&msisdn=<%# Request["msisdn"] %>'>
                                    <img src='<%# Eval("VideoLogo") %>' class="img-responsive" /></a>
                            </div>
                            <div class="col-xs-6 SingleDsc">
                                <a href='SinglefreeIsat.aspx?cat=<%# Eval("VideoId") %>&msisdn=<%# Request["msisdn"] %>' class="title"><%# Eval("VideoTitle") %></a>
                                <p class="author"><a href="#" class="author"><%# Eval("VideoArtist") %></a></p>
                                <p class="views"><%# Eval("VideoCounter") %> views</p>
                            </div>
                            <div class="clearfix"></div>

                        </div>
                        <br />
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </div>
        <div class="clearfix"></div>
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
</asp:Content>
