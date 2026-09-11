<%@ Page Title="" Language="C#" MasterPageFile="~/isat/MasterPageIsat.master" AutoEventWireup="true" CodeFile="NotSubscribeIsat.aspx.cs" Inherits="NotSubscribe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="jumbotron blurred-bg">
            <div class="row">
                <div class="col-xs-12">
                    <p>
                        Maaf, anda harus berlangganan terlebih dahulu untuk dapat mengakses konten-konten <strong>Top Model Indonesia,</strong> Silahkan klik tombol Berlangganan di halaman beranda untuk berlangganan.!<br />
                    </p>

                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary col-xs-12" Text="Home" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

