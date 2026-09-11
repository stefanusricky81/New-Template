<%@ Page Language="C#" MasterPageFile="~/Template/Base.master" AutoEventWireup="true" CodeFile="ClientContactDetail.aspx.cs" Inherits="Client_ClientContactDetail" Title="Bit By Bit Intranet - Client Contact Detail Link" %>
<%@ Register TagPrefix="uc" TagName="EditForm" Src="~/UserControl/Grid/EditForm/ClientContactEdit.ascx"%>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server" >

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <h2>Client Contact <asp:Literal ID="litHeaderText" runat="server" Text="Detail Link"/></h2>
    
    <asp:Panel ID="pnlGrid" runat="server">
        <p style="text-align:center;"><asp:Literal ID="litMessage" runat="server" /></p>
        <uc:EditForm ID="ucEditForm" runat="server" FormEditMode="Page" TableCssClass="detailLinkForm" />
    </asp:Panel>

    <asp:Panel ID="pnlNotFound" runat="server" Visible="false">
        <p>
            <span class="error">
            Error: unable to fetch entity (ID = <asp:Literal ID="litErrorId" runat="server" />).
            </span>
        </p>
    </asp:Panel>
    
     <telerik:RadAjaxManagerProxy ID="rampClientContact" runat="server"> 
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="pnlGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="ralpClientContact"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <telerik:RadAjaxLoadingPanel ID="ralpClientContact" runat="server" Transparency="25" BackColor="#E0E0E0">
     <img alt="Loading..." 
            src="/Images/loading.gif"
            style="border: 0px; padding-top:90px;" />
    </telerik:RadAjaxLoadingPanel>

</asp:Content>
