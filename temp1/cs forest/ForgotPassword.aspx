<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="cs_forest.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Forgot Password</title>
    <link href="Styles/Style.css" rel="stylesheet" />
</head>
<body style="font-family: Helvetica,Arial,sans-serif; position: fixed; width: 100%; vertical-align: top; margin: 0; padding: 0; background-color: #FCFCFC;">

    <form id="form1" runat="server">

        <div class="navbar">
            <img src="images/forest-interactive-logo.png" />
        </div>
        <div style="text-align:center;">
            <img style="margin: 20px 0 0 0; vertical-align: bottom;" src="images/login-banner.png" />
            <div class="login-form" style="width:260px;margin:auto;">
                <%--<img src="images/forest-interactive-login.png" style="margin: 10px 0 20px 0;" />--%>
                <div style="margin: 0 5px 10px 0;">
                    <img src="images/user-icon.png" style="vertical-align: middle; margin-right: 3px;" />
                    <asp:TextBox ID="txtEmail" runat="server" Width="190" CssClass="input" onblur="if(this.value=='')this.value='Email';" value="Email" onfocus="if(this.value=='Email')this.value='';"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic" ForeColor="IndianRed" ErrorMessage="Invalid Email Address" Font-Size="10" ControlToValidate="txtEmail" ValidationExpression=".*@.*\..*" Style="margin-left:28px;font-weight:600;font-size: .8125em;"></asp:RegularExpressionValidator>
                </div>
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="login-btn" CausesValidation="false" />
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Send Password" CssClass="login-btn" />
                <div>
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Font-Size="10"></asp:Label>
                </div>
            </div>
            
        </div>
    </form>
</body>
</html>