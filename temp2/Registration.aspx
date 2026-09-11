<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Registration.aspx.cs" Inherits="Registration" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div>
        <center><h1 style="width:945px; color:#000000; font-size:20px; margin-bottom:10px;">REGISTRATION</h1></center>
    </div>
    <div class="sign-in">
        <center><p>I AM A NEW CUSTOMER</p></center>
        <table>
            <tr>
                <td>Name</td>
                <td></td>
                <td>
                    <asp:TextBox ID="txtName" Width="240" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Address</td>
                <td></td>
                <td>
                    <asp:TextBox ID="txtAddress" Width="240" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Kota</td>
                <td></td>
                <td>
                    <asp:TextBox ID="txtKota" Width="240" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Telephone</td>
                <td></td>
                <td>
                    <asp:TextBox ID="txtTlp" Width="240" runat="server" />
                </td>
            </tr>
            <tr>
                <td>E-mail Address</td>
                <td></td>
                <td>
                    <asp:TextBox ID="txtEmail" Width="240" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Password</td>
                <td></td>
                <td>
                    <asp:TextBox ID="txtPass" TextMode="Password" Width="240" runat="server" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td><asp:Button ID="btnSignIn" OnClick="btnSignIn_Click" runat="server" Text="SIGN UP" BackColor="Black" ForeColor="White" Width="245" Height="30"/></td>
            </tr>
        </table>
        <br /><br />
    </div>

    <div>
        <tr>
            <td class="style1">
                <asp:Label style="color:Red" ID="tStatus" runat="server" />
            </td>
            <td style="width: 75%">
                &nbsp;
            </td>
        </tr>
    </div>       
</asp:Content>

