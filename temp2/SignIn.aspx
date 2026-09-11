<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SignIn.aspx.cs" Inherits="SignIn" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <script type="text/javascript">
            function textFocus() {
                document.getElementById('txtEmail').value = '';
            }
            function textBlur() {
                document.getElementById('txtEmail').value = 'Youre email here';
            }
            function doClick(buttonName, e) {
                var key;

                if (window.event)
                    key = window.event.keyCode;     //IE
                else
                    key = e.which;     //firefox

                if (key == 13) {
                    //Get the button the user wants to have clicked
                    var btn = document.getElementById(buttonName);
                    if (btn != null) { //If we find the button click it f79407
                        btn.click();
                        event.keyCode = 0
                    }
                }
            }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div>
        <center><h1 style="width:945px; color:#000000; font-size:20px; margin-bottom:10px;">SIGN IN</h1></center>
    </div>
    <div class="sign-in">
        <center><p>I AM A REGISTERED CUSTOMER</p></center>
        <table>
            <tr>
                <td>E-mail Address</td>
                <td></td>
                <td><asp:TextBox ID="txtEmail" runat="server" Width="240"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Password</td>
                <td></td>
                <td><asp:TextBox ID="txtPass" Width="240" TextMode="Password" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td><asp:Button ID="btnSignIn" runat="server" onclick="btnSignIn_Click" Text="SIGN IN" BackColor="Black" ForeColor="White" Width="245" Height="30"/></td>
            </tr>
        </table>

        <br /><br /><br /><br /><br /><br />
        <center><p>I AM A NEW CUSTOMER</p></center>
        <table>
            <tr>
                <td>E-mail Address</td>
                <td></td>
                <td><asp:TextBox ID="txtEmailNew" Width="240" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td><asp:Button ID="btnCreate" runat="server" Text="CREATE ACCOUNT" 
                        BackColor="Black" ForeColor="White" Width="245" Height="30" 
                        onclick="btnCreate_Click"/></td>
            </tr>
        </table>
        <br /><br />
    </div>
    <br /><br /><br />
</asp:Content>

