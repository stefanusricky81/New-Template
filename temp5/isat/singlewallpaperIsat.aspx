<%@ Page Title="" Language="C#" MasterPageFile="~/isat/MasterPageIsat.master" AutoEventWireup="true" CodeFile="singlewallpaperIsat.aspx.cs" Inherits="singlewallpaper" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="container">
        <div class="row">
            <div class="tittlePage">
                <h3>WALLPAPER</h3>
            </div>

            <div class="col-xs-12">
                <asp:Image ID="imgWallPaper" runat="server" CssClass="img-responsive" />
                <p class="text-center wllpprLbl">
                    <asp:Label ID="lblFreeWallPaper" CssClass="ModelTittle2" runat="server"></asp:Label>
                 </p>
            </div>

            <br />
            <div class="btn-group col-xs-12" style="text-align: center">
                <asp:Button ID="btnHome" runat="server" class="btn btn-primary col-xs-6" Text="Home" OnClick="btnHome_Click" />
                <asp:Button ID="btnDownload" runat="server" class="btn btn-primary col-xs-6" Text="Download" OnClick="btnDownload_Click" />
            </div>

            <div class="clearfix"></div>
            <%--   <div class="published">
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
