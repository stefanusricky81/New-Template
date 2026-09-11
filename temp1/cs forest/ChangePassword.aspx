<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="cs_forest.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script>

            function goBack() {
            window.history.back();
            }
    </script>
    <style>
       
        .field-help {
            white-space: normal;
            word-wrap: break-word;
            width: 100%;
            max-width: 635px;
            font-size: .75em;
            line-height: 1.84615385em;
            font-weight: 600;
            position: relative;
            color: #147049;
            margin: 0;
            margin-bottom: 18px;
            margin-left:25px;
        }

        .field-wrapper input[type="text"] ~ .field-help, .field-wrapper input[type="number"] ~ .field-help, .field-wrapper input[type="email"] ~ .field-help, .field-wrapper input[type="url"] ~ .field-help, .field-wrapper input[type="tel"] ~ .field-help, .field-wrapper input[type="password"] ~ .field-help, .field-wrapper textarea ~ .field-help {
            -ms-transition: all .25s ease-in-out 0s;
            transition: all .25s ease-in-out 0s;
            display: block;
            opacity: 0;
            max-height: 0;
            overflow: hidden;
        }

        .field-wrapper input[type="text"]:focus ~ .field-help, .field-wrapper input[type="number"]:focus ~ .field-help, .field-wrapper input[type="email"]:focus ~ .field-help, .field-wrapper input[type="url"]:focus ~ .field-help, .field-wrapper input[type="tel"]:focus ~ .field-help, .field-wrapper input[type="password"]:focus ~ .field-help, .field-wrapper textarea:focus ~ .field-help, .field-wrapper input[type="text"]:active ~ .field-help, .field-wrapper input[type="number"]:active ~ .field-help, .field-wrapper input[type="email"]:active ~ .field-help, .field-wrapper input[type="url"]:active ~ .field-help, .field-wrapper input[type="tel"]:active ~ .field-help, .field-wrapper input[type="password"]:active ~ .field-help, .field-wrapper textarea:active ~ .field-help {
            opacity: 1;
            max-height: 200px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script>
            function goBack() {
            window.history.back();
            }
    </script>
    <button onclick="goBack()" class="login-btn">Go Back</button>
     <div style="width: 100%; text-align: center">
        <img src="images/create-user.png" />
    </div>
    <div class="create-user-form">
        <h3>Change Password</h3>
        
        <div style="margin-bottom:15px;width:100%;text-align:center;">
            <asp:Label ID="lblMessage" Font-Size="10" runat="server" Font-Bold="true"></asp:Label>
        </div>
        <div class="field-wrapper" style="margin: 0 0 5px 0;">
            <img src="images/key-icon.png" style="vertical-align: middle; margin-right: 3px;" />
            <asp:TextBox ID="txtCurrentPassword" TextMode="Password" runat="server" CssClass="input" onblur="if(this.value=='')this.value='Password';" value="Password" onfocus="if(this.value=='Password')this.value='';"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCurrentPassword" ErrorMessage="" Style="margin-left:25px;font-weight:600;font-size: .8125em;"></asp:RequiredFieldValidator>
            <div class="field-help">Please enter current password.</div>
        </div>
        <div class="field-wrapper" style="margin: 0 0 10px 0;">
            <img src="images/key-icon.png" style="vertical-align: middle; margin-right: 3px;" />
            <asp:TextBox ID="txtNewPassword" TextMode="Password" MaxLength="15" runat="server" CssClass="input" onblur="if(this.value=='')this.value='Password';" value="Password" onfocus="if(this.value=='Password')this.value='';"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ForeColor="IndianRed" ErrorMessage="Invalid Password" Font-Size="10" ControlToValidate="txtNewPassword" ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$" Style="margin-left:28px;font-weight:600;font-size: .8125em;"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewPassword" ErrorMessage="" Display="None" Style="margin-left:25px;font-weight:600;font-size: .8125em;"></asp:RequiredFieldValidator>
            <div class="field-help">Please enter new password.</div>
        </div>
        <div class="field-wrapper" style="margin: 0 0 10px 0;">
            <img src="images/key-icon.png" style="vertical-align: middle; margin-right: 3px;" />
            <asp:TextBox ID="txtConfirmPassword" TextMode="Password" MaxLength="15" runat="server" CssClass="input" onblur="if(this.value=='')this.value='Password';" value="Password" onfocus="if(this.value=='Password')this.value='';"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="" Style="margin-left:25px;font-weight:600;font-size: .8125em;" Display="None"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="New password not match" ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword" Style="margin-left:25px;font-weight:600;font-size: .8125em;"></asp:CompareValidator>
            <div class="field-help">Please re-enter new password.</div>
        </div>
        <asp:Button ID="BtnSubmit" runat="server" OnClick="BtnSubmit_Click" Text="Submit" CssClass="btn" />
    </div>
</asp:Content>
