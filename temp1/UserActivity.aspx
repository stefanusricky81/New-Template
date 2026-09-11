<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserActivity.aspx.cs" Inherits="cs_forest.UserActivity" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        table{width:300px;font-size:12px}
        table tr th{text-align:left;width:150px}
        table tr th.submit{text-align:right;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="header">User Activity</h1>
    <table>
        <tr>
            <th>Username</th>
            <td>
                <asp:DropDownList ID="DDLUser" runat="server" Style="border:#808080 solid 1px;width:200px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>Date From</th>
            <td>
                <dx:ASPxDateEdit ID="DDLDateFrom" runat="server" Theme="Office2010Black" Width="200"></dx:ASPxDateEdit>
            </td>
        </tr>
        <tr>
            <th>Date To</th>
            <td>
                <dx:ASPxDateEdit ID="DDLDateTo" runat="server" Theme="Office2010Black" Width="200"></dx:ASPxDateEdit>
            </td>
        </tr>
        <tr>
            <th colspan="2" class="submit">
                <dx:ASPxButton ID="BtnSubmit" runat="server" Text="Submit" OnClick="BtnSubmit_Click" Theme="Office2010Black"></dx:ASPxButton>
            </th>
        </tr>
    </table>
    
    <h1 style="font-size:14px;">Activity List</h1>
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" Width="100%" Theme="Office2010Black">
        <Columns>
            <dx:GridViewDataDateColumn FieldName="Username" VisibleIndex="1"></dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="Date Access" VisibleIndex="2">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm:ss" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="Page" VisibleIndex="3"></dx:GridViewDataDateColumn>
        </Columns>
    </dx:ASPxGridView>

</asp:Content>
