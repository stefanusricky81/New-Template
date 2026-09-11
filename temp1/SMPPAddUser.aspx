<%@ Page Title="" Language="C#" MasterPageFile="~/SiteSMPP.Master" AutoEventWireup="true" CodeBehind="SMPPAddUser.aspx.cs" Inherits="cs_forest.SMPPAddUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 100%; text-align: center">
        <img src="Images/globe-smpp-management.png" />
    </div>

    <div class="create-user-form" style="min-height:300px;width:390px">
        <div style="margin:auto;text-align:center;width:100%;margin-bottom:10px">
            <asp:Label ID="lblMessage" Font-Size="10" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
        </div>

        <div class="field-wrapper" style="margin: 0 0 5px 0;">
            <asp:TextBox ID="txtSystemId" runat="server" CssClass="input" Style="width: 93%;" MaxLength="20" onblur="if(this.value=='')this.value='System Id';" value="System Id" onfocus="if(this.value=='System Id')this.value='';"></asp:TextBox>
            <div class="field-help">Please enter system id.</div>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtSystemId" ErrorMessage="System Id is require!" Style="float: right; color: #9a0707; font-weight:500; font-size:14px;margin-bottom:10px;"></asp:RequiredFieldValidator>
        </div>
        <div class="field-wrapper" style="margin: 0 0 5px 0;">
            <asp:TextBox ID="txtPassword" runat="server" CssClass="input" Style="width: 93%;" MaxLength="10" onblur="if(this.value=='')this.value='Password';" value="Password" onfocus="if(this.value=='Password')this.value='';"></asp:TextBox>
            <div class="field-help">Please enter password for this System Id
                <%--<div style="float:left;margin:0">
                    <ul style="margin:0;padding:0 0 0 10px;">
                        <li>One lowercase character</li>
                        <li>One uppercase character</li>
                        <li>One number</li>
                    </ul>
                </div>
                <div style="float:left;margin:0 0 0 15px;">
                    <ul style="margin:0;padding:0 0 0 10px;">
                        <li>One special character</li>
                        <li>Five characters minimum</li>
                        <li>Ten characters maximum</li>
                    </ul>
                </div>--%>
            </div>
            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ForeColor="IndianRed" ErrorMessage="Invalid Password" Font-Size="10" ControlToValidate="txtPassword" ValidationExpression="^.*(?=.{5,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$" Style="float: right; color: #9a0707; font-weight:500; font-size:14px;margin-bottom:10px;"></asp:RegularExpressionValidator>--%>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPassword" ErrorMessage="Username is required!" Style="float: right; color: #9a0707; font-weight:500; font-size:14px;margin-bottom:10px"></asp:RequiredFieldValidator>
        </div>
        <div class="field-wrapper" style="margin: 0 0 5px 0;">
            <asp:TextBox ID="txtSystemType" runat="server" CssClass="input" Style="width: 93%;" MaxLength="20" onblur="if(this.value=='')this.value='System Type';" value="System Type" onfocus="if(this.value=='System Type')this.value='';"></asp:TextBox>
            <div class="field-help">Please enter system type.</div>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSystemType" ErrorMessage="Password is required!" Style="float: right; color: #9a0707; font-weight:500; font-size:14px;margin-bottom:10px;"></asp:RequiredFieldValidator>
            
        </div>
        <div style="margin: 0 0 5px 0;">
            <asp:TextBox ID="txtAllowedId" runat="server" Text="*.*.*.*" CssClass="input" Style="width: 93%;"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAllowedId" ErrorMessage="Ip Address is required!" Style="float: right; color: #9a0707; font-weight:500; font-size:14px;margin-bottom:10px;"></asp:RequiredFieldValidator>
            <div class="field-help">
            <ul style="margin:0;padding:0 0 0 10px;">
                <li>Please enter Ip Address without http://</li>
                <li>Default value is *.*.*.*</li>
                <li>Able to whitelist 3 ip address separate by semicolumn(;) e.g 1.1.1.1;2.2.2.2</li>
            </ul>
            </div>
        </div>
        <div class="form-group">
            <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" CssClass="btn btn-primary" />
        </div>
    </div>
</asp:Content>
