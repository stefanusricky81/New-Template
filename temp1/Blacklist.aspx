<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Blacklist.aspx.cs" Inherits="cs_forest.Blacklist" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script>
         function goBack()
         {
            window.history.back();
         }
        
    </script>
      <button onclick="goBack()" class="login-btn">Back</button>
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" KeyFieldName="id" OnCustomButtonInitialize="ASPxGridView1_CustomButtonInitialize" OnCustomButtonCallback="ASPxGridView1_CustomButtonCallback" Width="100%">
        <Columns>
            <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Button" Caption="Action" Width="50">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnBlock" Text="Block">
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="btnUnBlock" Text="Unblock">
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="msisdn" Caption="Msisdn" VisibleIndex="1"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="telco" Caption="Telco" VisibleIndex="2"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="datecreated" Caption="Date" VisibleIndex="3"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="unblockedBy" Caption="Unblocked By" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="blockedBy" Caption="Blocked By" VisibleIndex="5" Visible="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="status" Caption="Status" VisibleIndex="6"></dx:GridViewDataDateColumn>
        </Columns>
    </dx:ASPxGridView>
    <dx:ASPxGridView ID="ASPxGridView2" runat="server" AutoGenerateColumns="False">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="MSISDN" Caption="MSISDN" VisibleIndex="1"></dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="DateCreated" Caption="Date" VisibleIndex="0">
                <PropertiesDateEdit DisplayFormatString=""></PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
        </Columns>
    </dx:ASPxGridView>
    
</asp:Content>
