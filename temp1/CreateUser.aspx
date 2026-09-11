<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs" Inherits="cs_forest.CreateUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 100%; text-align: center">
        <img src="images/create-user.png" />
    </div>
    <div class="create-user-form">
        <h3><asp:Label ID="lblHeader" runat="server"></asp:Label></h3>
        <div>
            <asp:Label ID="lblMessage" Font-Size="10" runat="server" ForeColor="IndianRed" Font-Bold="true"></asp:Label>
        </div>
        <div class="field-wrapper" style="margin: 0 0 5px 0;">
            <img src="images/gear-icon.png" style="vertical-align: middle; margin-right: 3px;" />
            <asp:DropDownList ID="DDLUser" runat="server" OnSelectedIndexChanged="DDLUser_SelectedIndexChanged" AutoPostBack="true" CssClass="input" Style="width: 95%;">
                <asp:ListItem Value="">Select Role</asp:ListItem>
                <asp:ListItem>Admin</asp:ListItem>
                <asp:ListItem>Telco</asp:ListItem>
                <asp:ListItem>OCS</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="DDLUser" runat="server" Display="Dynamic" ErrorMessage="Please Select Role" ForeColor="IndianRed" Font-Size="10" Style="margin-left:25px;font-weight:600;font-size: .8125em;"></asp:RequiredFieldValidator>
            <div class="field-help" style="margin:0;">
                <ul>
                    <li>Telco - Able to select one module.</li>
                    <li>Admin - Able to access all modules.</li>
                    <li>OCS - Able to selected multiple modules.</li>
                </ul>
            </div>
        </div>
        <div class="field-wrapper" style="margin: 0 0 5px 0;">
            <img src="images/icon_email.png" style="vertical-align: middle; margin-right: 3px;" />
            <asp:TextBox ID="txtEmail" runat="server" CssClass="input" onblur="if(this.value=='')this.value='Username';" value="Username" onfocus="if(this.value=='Username')this.value='';"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic" ForeColor="IndianRed" ErrorMessage="Invalid Email Address" Font-Size="10" ControlToValidate="txtEmail" ValidationExpression=".*@.*\..*" Style="margin-left:28px;font-weight:600;font-size: .8125em;"></asp:RegularExpressionValidator>
            <div class="field-help">Please use email address for the username.</div>
        </div>
        <div class="field-wrapper" style="margin: 0 0 10px 0;">
            <img src="images/key-icon.png" style="vertical-align: middle; margin-right: 3px;" />
            <asp:TextBox ID="txtPassword" TextMode="Password" MaxLength="15" runat="server" CssClass="input" onblur="if(this.value=='')this.value='Password';" value="Password" onfocus="if(this.value=='Password')this.value='';"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ForeColor="IndianRed" ErrorMessage="Invalid Password" Font-Size="10" ControlToValidate="txtPassword" ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$" Style="margin-left:28px;font-weight:600;font-size: .8125em;"></asp:RegularExpressionValidator>
            <div class="field-help" style="margin:0;">
                <div style="float:left;margin:0">
                    <ul>
                        <li>One lowercase character</li>
                        <li>One uppercase character</li>
                        <li>One number</li>
                    </ul>
                </div>
                <div style="float:left;margin:0">
                    <ul>
                        <li>One special character</li>
                        <li>Eight characters minimum</li>
                        <li>15 characters maximum</li>
                    </ul>
                </div>
            </div>
        </div>
        <div style="margin: 0; padding-left: 20px; padding-top: 10px; text-align: left">
            <span style="font-size: 14px">Modules :</span>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:CheckBoxList ID="CHKModule" runat="server" Font-Size="11" RepeatColumns="4" AutoPostBack="true" OnSelectedIndexChanged="CHKModule_SelectedIndexChanged">
                    </asp:CheckBoxList>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="CHKModule" />
                    <asp:AsyncPostBackTrigger ControlID="DDLUser" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <asp:Button ID="BtnSubmit" runat="server" OnClick="BtnSubmit_Click" Text="Submit" CssClass="btn" />
    </div>
</asp:Content>