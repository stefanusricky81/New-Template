<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GlobeSMPP.aspx.cs" Inherits="cs_forest.GlobeSMPP" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" Width="100%" Theme="Office2010Black" EnableTheming="True" KeyFieldName="id" SettingsPager-PageSize="50" Settings-UseFixedTableLayout="false">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" VisibleIndex="1" Visible="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="smpp_user" ReadOnly="True" VisibleIndex="2" Caption="smpp_user" Visible="true"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="sender_number" ReadOnly="True" VisibleIndex="3" Caption="sender_number"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="receiver_number" ReadOnly="True" VisibleIndex="4" Caption="receiver_number"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="message_id" ReadOnly="True" VisibleIndex="5" Caption="message_id"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="http_response" ReadOnly="True" VisibleIndex="6" Caption="http_response" Visible="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="dn_status" ReadOnly="True" VisibleIndex="7" Caption="dn_status" Visible="true"></dx:GridViewDataTextColumn>
            <%--<dx:GridViewDataTextColumn FieldName="sms_sent_date" ReadOnly="True" VisibleIndex="7" Caption="sms_sent_date"></dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn FieldName="sms_sent_time" ReadOnly="True" VisibleIndex="8" Caption="sms_sent_time">
                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy HH:mm:ss"></PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="dn_received_time" ReadOnly="True" VisibleIndex="9" Caption="dn_received_time" Visible="true">
                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy HH:mm:ss"></PropertiesTextEdit>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="http_api_id" ReadOnly="True" VisibleIndex="10" Caption="http_api_id" Visible="true"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="sms_coding" ReadOnly="True" VisibleIndex="11" Caption="sms_coding" Visible="true"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="udh" ReadOnly="True" VisibleIndex="12" Caption="udh" Visible="true"></dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="message" ReadOnly="True" VisibleIndex="13" Caption="message"></dx:GridViewDataTextColumn>
        </Columns>
        <SettingsDataSecurity AllowEdit="False" AllowInsert="False" AllowDelete="False"></SettingsDataSecurity>
    </dx:ASPxGridView>
</asp:Content>
