<%@ Page Title="" Language="C#" MasterPageFile="~/isat/MasterPageIsat.master" AutoEventWireup="true" CodeFile="premiumvideoIsat.aspx.cs" Inherits="premiumvideo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
     <!-- Page Content -->
    <div class="container">
        <div class="row tittlePage" >
            <h3 >PREMIUM VIDEO</h3>
        </div>
        <!-- /.row -->

        <!-- Projects Row -->
        <div class="gallery blurred-bg">
            <asp:DataList ID="dlFreeVideo" runat="server" Width="100%" RepeatColumns="3">
                <ItemTemplate>
                    <div class="col-xs-6 col-sm-12 col-md-12" data-src='<%# Eval("VideoLogo") %>'>
                        <a href='SingleIsat.aspx?cat=<%# Eval("VideoId") %>' class="thumbnail">
                            <img class="img-responsive" src='<%# Eval("VideoLogo") %>' alt='<%# Eval("VideoTitle") %>' />
                            <h4 class="ModelTittle text-center"><%# Eval("VideoTitle") %></h4>
                        </a>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <br />
         <div class="container">
               <div class="col-xs-12 btn-group">
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