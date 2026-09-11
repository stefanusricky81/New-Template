<%@ Page Title="" Language="C#" MasterPageFile="~/SiteSMPP.Master" AutoEventWireup="true" CodeBehind="SMPPListRoute.aspx.cs" Inherits="cs_forest.SMPPListRoute" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <%--<h1 class="header">Route's List</h1>--%>

    <div style="width: 100%; text-align: left; margin: auto auto 5px auto;">
        <dx:ASPxButton ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" Width="100">
            <ClientSideEvents Click="function(s,e) { e.processOnServer = confirm('Are you sure to delete the selected route?'); }" />
        </dx:ASPxButton>
    </div>
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="false" KeyFieldName="id" Width="100%" SettingsPager-PageSize="30" Theme="Office2010Black">
        <Columns>
            <dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0" SelectAllCheckboxMode="Page" ShowSelectCheckbox="True"></dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" VisibleIndex="1" Visible="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="route_name" ReadOnly="True" VisibleIndex="2" Caption="route_name"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="smpp_user_id" ReadOnly="True" VisibleIndex="3" Caption="smpp_user_id"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="http_api_id" ReadOnly="True" VisibleIndex="4" Caption="http_api_id"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="prefix" ReadOnly="True" VisibleIndex="5" Caption="prefix"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="operator_name" ReadOnly="True" VisibleIndex="6" Caption="operator_name"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="country_name" ReadOnly="True" VisibleIndex="7" Caption="country_name"></dx:GridViewDataTextColumn>
            <dx:GridViewDataColumn Caption="Detail" VisibleIndex="8" Width="50">
                    <DataItemTemplate>
                        <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Text="Edit" Theme="Office2010Black" NavigateUrl='<%# "smppaddroute.aspx?type=update&id=" + DataBinder.Eval(Container.DataItem, "id") + "&route=" + DataBinder.Eval(Container.DataItem, "route_name") + "&userid=" + DataBinder.Eval(Container.DataItem, "smpp_user_id") + "&api=" + DataBinder.Eval(Container.DataItem, "http_api_id") + "&prefix=" + DataBinder.Eval(Container.DataItem, "prefix") + "&oprator=" + DataBinder.Eval(Container.DataItem, "operator_name") + "&country=" + DataBinder.Eval(Container.DataItem, "country_name") %>'>
                    </dx:ASPxHyperLink>
                    </DataItemTemplate>
            </dx:GridViewDataColumn>
        </Columns>
        <Settings ShowGroupPanel="True" ShowFilterRow="True"></Settings>
    </dx:ASPxGridView>
</asp:Content>