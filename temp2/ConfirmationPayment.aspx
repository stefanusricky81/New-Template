<%@ Page Title="" Language="C#" MasterPageFile="~/Site3.master" AutoEventWireup="true" CodeFile="ConfirmationPayment.aspx.cs" Inherits="LssCulture" %>
<%@ MasterType VirtualPath="~/Site3.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="kirir" Runat="Server">
    <p style="font-size:15px">LSS COUTURE</p>
    <p style="font-size:15px">LITTLE SUPER STAR</p>
    <p style="font-size:15px">MOMS</p>
    <p style="font-size:15px">ACCESSORIES</p>
    <p style="font-size:15px">SALE</p>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="kananr" Runat="Server">
    <div>
        <asp:Label ID="lblStatus" runat="server" />
    </div>
    <div class="products">
        <h3>CONFIRMATION PAYMENT</h3>
        <table class="cpayment">
            <tr>
                <td>Order ID</td>
                <td></td>
                <td><asp:TextBox ID="txtOrderID" runat="server" Width="280" ontextchanged="txtOrderID_TextChanged" AutoPostBack="true" /></td>
            </tr>
            <tr>
                <td>Name</td>
                <td></td>
                <td><asp:TextBox ID="txtName" ReadOnly="true" runat="server" Width="280"/></td>
            </tr>
            <tr>
                <td>Amount</td>
                <td></td>
                <td><asp:TextBox ID="txtAmount" ReadOnly="true" runat="server" Width="280"/></td>
            </tr>
            <tr>
                <td>Payment Via</td>
                <td></td>
                <td><asp:DropDownList ID="ddlBank" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Customer Account Name</td>
                <td></td>
                <td><asp:TextBox ID="txtAccountName" runat="server" Width="280" /></td>
            </tr>
            <tr>
                <td>Customer Account Number</td>
                <td></td>
                <td><asp:TextBox ID="txtAccountNumber" runat="server" Width="280" /></td>
            </tr>
            <tr>
                <td>Message</td>
                <td></td>
                <td><asp:TextBox ID="txtMessage" runat="server" Rows="10" TextMode="MultiLine" Width="280" /></td>
            </tr>
            <tr>
                <td>Image Receipt</td>
                <td></td>
                <td><asp:FileUpload ID="fileuploadimages" runat="server" /></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td style="text-align:right;">
                    <asp:Button ID="btnSubmit" runat="server" Rows="10" 
                        Text="Submit" onclick="btnSubmit_Click" /></td>
            </tr>
        </table>

    </div>
</asp:Content>
