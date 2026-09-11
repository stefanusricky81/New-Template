<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageUser.aspx.cs" Inherits="cs_forest.ManageUser" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <dx:ASPxButton ID="btnCreateUser" runat="server" Text="Create User" OnClick="btnCreateUser_Click" Style="margin-bottom: 5px;" Theme="Office2010Black"></dx:ASPxButton>
    <dx:ASPxButton ID="btnUserActivity" runat="server" Text="User Activity" OnClick="btnUserActivity_Click" Style="margin-bottom: 5px;" Theme="Office2010Black"></dx:ASPxButton>


    <h1 class="header">User Listing</h1>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:CustomerService %>' 
        SelectCommand="SELECT [Username], [Password], [Roles], [Email], CASE WHEN [Locked] = 'True' THEN 'Lock' ELSE 'Active' END AS Locked, [NewLoginDate], [UserId] FROM [UserLogin]">
    </asp:SqlDataSource>
    
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" KeyFieldName="UserId" DataSourceID="SqlDataSource1" Width="100%" Theme="Office2010Black" OnCustomButtonInitialize="ASPxGridView1_CustomButtonInitialize" OnCustomButtonCallback="ASPxGridView1_CustomButtonCallback" >
        <ClientSideEvents CustomButtonClick="function(s, e) {
            if (e.buttonID == 'btnLock')
            e.processOnServer = confirm('Are you sure to lock this account?');
            else if (e.buttonID == 'btnUnlock')
            e.processOnServer = confirm('Are you sure to unlock this account?'); }" />
        <Columns>
            <dx:GridViewDataTextColumn FieldName="Username" ReadOnly="True" VisibleIndex="0"></dx:GridViewDataTextColumn>
            <%--<dx:GridViewDataTextColumn FieldName="Password" ReadOnly="True" VisibleIndex="1"></dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn FieldName="Roles" ReadOnly="True" VisibleIndex="2"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Email" ReadOnly="True" VisibleIndex="3"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Locked" Caption="Status" ReadOnly="True" VisibleIndex="4"></dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="NewLoginDate" Caption="Last Login" ReadOnly="True" VisibleIndex="6">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy hh:mm:ss tt"></PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="UserId" ReadOnly="True" VisibleIndex="7" Visible="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataColumn Caption="Modules" VisibleIndex="8" Width="100">
                <DataItemTemplate>
                    <a href='<%# "createuser.aspx?type=update&username=" + DataBinder.Eval(Container.DataItem, "Username") + "&password=" + DataBinder.Eval(Container.DataItem, "Password") + "&role=" + DataBinder.Eval(Container.DataItem, "Roles") + "&email=" + DataBinder.Eval(Container.DataItem, "Email") + "&userid=" + DataBinder.Eval(Container.DataItem, "UserId")%>'>Details</a>
                </DataItemTemplate>
            </dx:GridViewDataColumn>
            <dx:GridViewCommandColumn VisibleIndex="10" ButtonType="Button" Caption="Action">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnLock" Text="Lock"/>
                    <dx:GridViewCommandColumnCustomButton ID="btnUnlock" Text="Unlock"/>
                </CustomButtons>
            </dx:GridViewCommandColumn>
        </Columns>
    </dx:ASPxGridView>
    
    
</asp:Content>
