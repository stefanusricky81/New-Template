<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Subscriber.aspx.cs" Inherits="cs_forest.Subscriber" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      function goBack()
         {
            window.history.back();
         }
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="header">Subscriber Details</h1>

    <div id="section1" runat="server" visible="false">
          <button onclick="goBack()" class="login-btn">Go Back</button>
        <div id="dvStopAll" runat="server" style="float: left; margin-bottom: 10px; width: 100%;">
            <asp:Button ID="btnStopALL" runat="server" OnClick="btnStopALL_Click" Text="Stop All Services" CssClass="btn" />
            <asp:Button ID="btnBlacklist" runat="server" OnClick="btnBlacklist_Click" Text="Stop All and Add MSISDN Into Blacklist" CssClass="btn" />
            <asp:Label ID="lblMsg" runat="server" Style="float: right; margin-right: 20px; margin-top: 10px;" ForeColor="Green"></asp:Label>
        </div>
        <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" KeyFieldName="userid" OnCustomButtonCallback="ASPxGridView1_CustomButtonCallback" OnCustomButtonInitialize="ASPxGridView1_CustomButtonInitialize" OnDataBound="ASPxGridView1_DataBound" Width="100%">
            <ClientSideEvents CustomButtonClick="function(s, e) {
                                    if (e.buttonID == 'btnUnsub')
                                    e.processOnServer = confirm('Are you sure to unsubscribe the service?'); }" />
            <Columns>
                <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Button" Caption="Action">
                    <CustomButtons>
                        <dx:GridViewCommandColumnCustomButton ID="btnUnsub" Text="Unsubscribe">
                        </dx:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn FieldName="userid" Caption="User Id" ReadOnly="True" VisibleIndex="1">
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="msisdn" Caption="Msisdn" VisibleIndex="2"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="shortcode" Caption="Short Code" VisibleIndex="3"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="keyword" Caption="Keyword" VisibleIndex="4"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="mo_id" Caption="Mo Id" VisibleIndex="5"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="status" Caption="Status" VisibleIndex="6"></dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="date_reg" Caption="Date Reg" VisibleIndex="7">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm:ss"></PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataDateColumn FieldName="date_unsubs" Caption="Date Unsub" VisibleIndex="8">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm:ss"></PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <%--<dx:GridViewDataTextColumn FieldName="unsubby" ReadOnly="True" Caption="Unsub By" VisibleIndex="14"></dx:GridViewDataTextColumn>--%>
                <dx:GridViewDataTextColumn FieldName="Gateway" VisibleIndex="10"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="regtype" Caption="Reg Type" VisibleIndex="11"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="UnsubType" Caption="Unsub Type" VisibleIndex="12"></dx:GridViewDataTextColumn>
                <%--<dx:GridViewDataTextColumn FieldName="mo" ReadOnly="True" Caption="Total MO" VisibleIndex="12"></dx:GridViewDataTextColumn>--%>
                <%--<dx:GridViewDataTextColumn FieldName="mt" ReadOnly="True" Caption="Total MT" VisibleIndex="13"></dx:GridViewDataTextColumn>--%>
                <dx:GridViewDataTextColumn FieldName="revenue" ReadOnly="True" Caption="Revenue (RM)" VisibleIndex="15"></dx:GridViewDataTextColumn>
            </Columns>
        </dx:ASPxGridView>
    </div>

    <div id="section2" runat="server" visible="false">
        <dx:ASPxGridView ID="ASPxGridView2" runat="server" AutoGenerateColumns="False" KeyFieldName="SubId" Width="100%"
            OnCustomButtonCallback="ASPxGridView2_CustomButtonCallback" OnCustomButtonInitialize="ASPxGridView2_CustomButtonInitialize">
             <ClientSideEvents CustomButtonClick="function(s, e) {
                                    if (e.buttonID == 'btnUnsub2')
                                    e.processOnServer = confirm('Are you sure to unsubscribe the service?'); }" />
            
            <Columns>
                <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Button" Caption="Action">
                    <CustomButtons>
                        <dx:GridViewCommandColumnCustomButton ID="btnUnsub2" Text="Unsubscribe">
                        </dx:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn FieldName="SubId" ReadOnly="True" VisibleIndex="0"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="KeywordName" VisibleIndex="1"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Msisdn" VisibleIndex="2"></dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="Status" VisibleIndex="3"></dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="MoId" VisibleIndex="4"></dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="DateRegister" VisibleIndex="5">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm:ss"></PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataDateColumn FieldName="DateUnsubscribe" VisibleIndex="6">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm:ss"></PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
            </Columns>
        </dx:ASPxGridView>
    </div>

    <div id="section3" runat="server" visible="false">
        <dx:ASPxGridView ID="ASPxGridView3" runat="server" SettingsPager-PageSize="25" AutoGenerateColumns="False" KeyFieldName="SubsId" Width="100%"
             OnCustomButtonCallback="ASPxGridView3_CustomButtonCallback" OnCustomButtonInitialize="ASPxGridView3_CustomButtonInitialize">
            <ClientSideEvents CustomButtonClick="function(s, e) {
                                    if (e.buttonID == 'btnUnsub3')
                                    e.processOnServer = confirm('Are you sure to unsubscribe the service?'); }" />
            <Columns>
                <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Button" Caption="Action">
                    <CustomButtons>
                        <dx:GridViewCommandColumnCustomButton ID="btnUnsub3" Text="Unsubscribe">
                        </dx:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn FieldName="SubsId" ReadOnly="True" VisibleIndex="0">
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="DateCreated" VisibleIndex="1">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm:ss"></PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="Msisdn" VisibleIndex="2"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="3"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="TotalDownload" VisibleIndex="4"></dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="RenewDate" VisibleIndex="5"></dx:GridViewDataDateColumn>
                <dx:GridViewDataDateColumn FieldName="LastRenewal" VisibleIndex="6"></dx:GridViewDataDateColumn>
            </Columns>
            <Settings ShowFilterRow="false"></Settings>
        </dx:ASPxGridView>
    </div>
</asp:Content>