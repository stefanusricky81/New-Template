<%@ Page Title="" Language="C#" MasterPageFile="~/SiteSMPP.Master" AutoEventWireup="true" CodeBehind="SMPPAddHttpRoute.aspx.cs" Inherits="cs_forest.SMPPAddHttpRoute" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 100%; text-align: center">
        <img src="Images/globe-smpp-management.png" />
    </div>

    <div class="create-user-form" style="min-height:300px;width:390px;">
        <div style="margin:auto;text-align:center;width:100%;margin-bottom:10px">
            <asp:Label ID="lblMessage" Font-Size="10" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
        </div>

        <div class="field-wrapper" style="margin: 0 0 15px 0;">
            <asp:TextBox ID="txtRoute" runat="server" CssClass="input" Style="width: 92%;" MaxLength="20" onblur="if(this.value=='')this.value='Route Name';" value="Route Name" onfocus="if(this.value=='Route Name')this.value='';"></asp:TextBox>
            <div class="field-help">Please enter route name.</div>
        </div>
        <div class="field-wrapper" style="margin: 0 0 15px 0;">
            <asp:DropDownList ID="DDLSystemId" CssClass="input"  runat="server" Style="width: 100%;">
            </asp:DropDownList>
            <div class="field-help">Please select System Id</div>
        </div>
        <div class="field-wrapper" style="margin: 0 0 15px 0;">
            <asp:DropDownList ID="DDLHTTPApi" CssClass="input"  runat="server" Style="width: 100%;">
                <asp:ListItem>BEEPSEND</asp:ListItem>
                <asp:ListItem>GSM</asp:ListItem>
                <asp:ListItem>INFOBIP</asp:ListItem>
                <asp:ListItem>XOX</asp:ListItem>
                <asp:ListItem>RED-IDR </asp:ListItem>
                <asp:ListItem>RED-RM</asp:ListItem>
                <asp:ListItem>SILVER-EUR</asp:ListItem>
                <asp:ListItem>SILVER-RM</asp:ListItem>
                <asp:ListItem>TUNE-TALK</asp:ListItem>
            </asp:DropDownList>
            <div class="field-help">Please select HTTP Api Id.</div>
        </div>
        <div class="field-wrapper" style="margin: 0 0 15px 0;">
            <asp:TextBox ID="txtPrefix" runat="server" CssClass="input" Style="width: 92%;"  MaxLength="20" onblur="if(this.value=='')this.value='Prefix';" value="Prefix" onfocus="if(this.value=='Prefix')this.value='';"></asp:TextBox>
            <div class="field-help">Please enter prefix number e.g 6012</div>
        </div>
        <div class="field-wrapper" style="margin: 0 0 15px 0;">
            <asp:TextBox ID="txtOperator" runat="server" CssClass="input" Style="width: 92%;" MaxLength="20" onblur="if(this.value=='')this.value='Operator';" value="Operator" onfocus="if(this.value=='Operator')this.value='';"></asp:TextBox>
            <div class="field-help">Please enter operator.</div>
        </div>
        <div class="field-wrapper" style="margin: 0 0 15px 0;">
            <asp:TextBox ID="txtCountry" runat="server" CssClass="input" Style="width: 92%;" MaxLength="20" onblur="if(this.value=='')this.value='Country';" value="Country" onfocus="if(this.value=='Country')this.value='';"></asp:TextBox>
            <div class="field-help">Please enter country name.</div>
        </div>
        <div>
            <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" CssClass="btn btn-primary" Text="Create Account" />
        </div>
    </div>
</asp:Content>
