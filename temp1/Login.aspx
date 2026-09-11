<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="cs_forest.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
   <%-- <meta name="viewport" content="width=device-width, initial-scale=1">--%>
    <link href="Styles/Style.css" rel="stylesheet" />

</head>
<body style="font-family: Helvetica,Arial,sans-serif; width: 100%; vertical-align: top; margin: 0; padding: 0; background-color: #FCFCFC; position:fixed;">

    <form id="form1" runat="server">

        <div class="navbar">
            <img src="Images/telco logo_logo_fin.png" />
        </div>
        <div style="text-align:center;">
            <img style="margin: 20px 0 0 0; vertical-align: bottom;" src="Images/telco logo_login_banner.png" />
            <div class="login-form" style="width:260px;margin:auto;">
                <img src="Images/telco logo_box_fin.png" style="margin: 10px 0 20px 0;" />
                <div style="margin: 0 5px 10px 0;">
                    <img src="images/user-icon.png" style="vertical-align: middle; margin-right: 3px;" />
                    <asp:TextBox ID="txtUsername" runat="server" Width="190" CssClass="input" onblur="if(this.value=='')this.value='Username';" value="Username" onfocus="if(this.value=='Username')this.value='';"></asp:TextBox>
                </div>
                <div style="margin: 0 5px 10px 0;">
                    <img src="images/key-icon.png" style="vertical-align: middle; margin-right: 3px;" />
                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" Width="190" CssClass="input" onblur="if(this.value=='')this.value='Password';" value="Password" onfocus="if(this.value=='Password')this.value='';"></asp:TextBox>
                </div>
                <asp:Button ID="BtnLogin" runat="server" OnClick="BtnLogin_Click" Text="Login" CssClass="login-btn" />
                <asp:Button ID="BtnForgot" runat="server" OnClick="BtnForgot_Click" Text="Forgot Password?" CssClass="grey-btn" />
                <div>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="IndianRed" Font-Bold="true"></asp:Label>
                </div>
            </div>
        </div>
    </form>
<%--    <hr width="50%">
        <footer class="footer-basic-centered">
            <div class="footer-left"><p><h4>ABOUT US</h4>

Established in January 2006, FOREST focuses on providing Application Development and Mobile Related Services to Content Partners and Aggregators operating in the ASEAN Countries.

Our primary aim was broadened to specialise in development of software solutions in the areas of Creative Multimedia, Telecommunications as well as ICT.</p>
            </div>
			<div class="footer-left2">
               <h4>LICENSED</h4>
                <p><img src="Images/skmm-mcmc.png" width="50px" /></p>
           
            <h4>AWARDS</h4>

         
            <p><img src="Images/footer-02.png" width="100px"/></p>

          
            <h4>INDUSTRY STATUS</h4>

           
            <p><img src="Images/footer-01.png" width="100px" /></p>

               
</div>

			<div class="footer-right">
               <h4>INFORMATION</h4>
                <ul>
                    <li><a href="http://www.forest-interactive.com/telcooperator/">Telco/Operator</a></li>
                    <li><a href="http://www.forest-interactive.com/ict-sales/governmentweb-development-services/">Government</a></li>
                    <li><a href="http://www.forest-interactive.com/corporate/">Corporate</a></li>
                    <li><a href="http://www.forest-interactive.com/telcooperator/">Mobile Games</a></li>
                    <li><a href="http://www.forest-interactive.com/telcooperator/">Industry Statistics</a></li>
                    <li><a href="http://www.forest-interactive.com/telcooperator/">Partners</a></li>
                    <li><a href="http://www.forest-interactive.com/telcooperator/">Careers</a></li>
                </ul>
                </div>
                <div class="footer-right2">
               <h4>Media</h4>
                <ul>
                    <li><a href="http://www.forest-interactive.com/telcooperator/">News</a></li>
                    <li><a href="http://www.forest-interactive.com/telcooperator/">Galleries</a></li>
                    <li><a href="http://www.forest-interactive.com/telcooperator/">CSR</a></li>
                    <li><a href="http://www.forest-interactive.com/telcooperator/">Events</a></li>
                </ul>
              </div>
                    
                 <div class="footer-right3">
             <p>
Headquarters Office
<br>
C-5-1 Megan Avenue 2,<br>
12 Jalan Yap Kwan Seng,<br>
50450 Kuala Lumpur, Malaysia.
</p>
                     <a id="contactbtn" href="http://www.forest-interactive.com/contact-us/" target="_self" class="contactbtn" style="margin-left: 25px"><span>Contact Us</span></a>
</div>
            </div>--%>
<%--		</footer>
    <footer class="footer" style="text-align:center;">
            <p style="margin: 0px 0">Copyright © 2014 <a style="text-decoration: none; color: #fff;" href="http://forest-interactive.com/">Forest Interactive Sdn. Bhd.</a> All rights reserved.</p>
        </footer>--%>
    
</body>

</html>