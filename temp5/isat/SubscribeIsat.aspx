<%@ Page Title="" Language="C#" MasterPageFile="~/isat/MasterPageIsat.master" AutoEventWireup="true" CodeFile="SubscribeIsat.aspx.cs" Inherits="Subscribe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <!-- Page Content -->
    <div class="container">
        <div class="jumbotron blurred-bg">

            <div class="row">
                <p class="col-xs-12">
                    Anda akah membeli konten Top Model Indonesia seharga Rp.3,300. Ayo! Sedikit lagi anda akan mendapatkan salah satu konten terbaik dari <strong>Top Model Indonesia!</strong>
                    <em style="font-size: 10pt;">*Anda akah menerima notifikasi via SMS setelah anda meng-klik <strong>YA</strong>.</em>
                    <em style="font-size: 10pt;">*Disarankan untuk pengguna SmartPhone.</em>
                </p>
                <br />

                <div class="btn-group col-xs-12">
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default col-xs-6" Text="TIDAK" OnClick="btnCancel_Click" />
                    <asp:Button ID="btnSubscribe" runat="server" CssClass="btn btn-primary col-xs-6" Text="YA" OnClick="btnSubscribe_Click" />
                </div>


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
    </div> <!-- /container -->
    
</asp:Content>