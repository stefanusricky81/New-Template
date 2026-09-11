<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MOMT.aspx.cs" Inherits="cs_forest.MOMT" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="header"><asp:Label ID="lblHeader" runat="server"></asp:Label></h1>
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" Theme="Office2010Black" Width="100%">
        <SettingsPager PageSize="50"></SettingsPager>
    </dx:ASPxGridView>
    <dx:ASPxGridView ID="ASPxGridView2" runat="server" AutoGenerateColumns="False" KeyFieldName="MoId" SettingsPager-PageSize="50" Width="100%">
        <Columns>
            <dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0"></dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="MoId" ReadOnly="True" VisibleIndex="1" Visible="false">
                <EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="DateCreated" VisibleIndex="2">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm:ss"></PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="TransactionId" VisibleIndex="3"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Msisdn" VisibleIndex="4"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Message" VisibleIndex="5"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MoType" VisibleIndex="6"></dx:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True"></Settings>
    </dx:ASPxGridView>
    <dx:ASPxGridView ID="ASPxGridView3" runat="server" AutoGenerateColumns="False" SettingsPager-PageSize="50" Width="100%">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="DateCreated" VisibleIndex="1">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm:ss"></PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="MtId" VisibleIndex="2"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Msisdn" VisibleIndex="3"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Message" VisibleIndex="4"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="KeywordName" VisibleIndex="5"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ContentType" VisibleIndex="6"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Price" VisibleIndex="7"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MtStatus" VisibleIndex="8"></dx:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True"></Settings>
    </dx:ASPxGridView>
    <dx:ASPxGridView ID="ASPxGridView4" runat="server" AutoGenerateColumns="False" SettingsPager-PageSize="50" Width="100%" KeyFieldName="Id">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="0" ReadOnly="True">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MoId" VisibleIndex="1"></dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="DateCreated" VisibleIndex="2">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm:ss"></PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Message" VisibleIndex="4"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Msisdn" VisibleIndex="3"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="KeywordName" VisibleIndex="4"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ContentType" VisibleIndex="5"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Message" VisibleIndex="6"></dx:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True"></Settings>
    </dx:ASPxGridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:XOX101 %>' SelectCommand="SELECT [Id], [MoId], [DateCreated], [Msisdn], [KeywordName], [ContentType], [Message] FROM [MO-ForestDetails]"></asp:SqlDataSource>
</asp:Content>
