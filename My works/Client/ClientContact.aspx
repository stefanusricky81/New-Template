<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Base.master" AutoEventWireup="true" CodeFile="ClientContact.aspx.cs" Inherits="Client_ClientContact" %>
<%@ Register TagPrefix="uc" TagName="ClientContactGrid" Src="~/UserControl/Grid/ClientContact.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
   
    <style type="text/css">
        .auto-style12 {
            height:2px; 
            padding-top: 1px;
            padding-bottom: 100px;  
            padding-left: 1px;
            padding-right: 1px; 	
        
        }
        .auto-style13 {
            height: 4px;
            border-bottom: 1px;
	        border-right: 1px;
	        border-left: 175px;
	        border-top:1px;
            padding-top:1px;	
        }
                       
       
       
        .auto-style22 {
            height: 100px;
            padding-left: 1px;
            padding-right: 300px;
            padding-bottom: 5px;
            width: 293px;
        }

        .searchForm {
            height: 50px;
        }

        .ValidationSummary {}


        .auto-style23 {
            width: 30%;
        }



    </style>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server" Height ="10px">
        <asp:Panel ID="pnlSearch" runat="server" Height="280px">
        <h2>Client Contact
            <asp:Literal ID="litClientName" runat="server" />
        </h2>
        
        <table class="searchForm" border="0">
            <tr>
                <td colspan="4" class="auto-style13">
                    <p style=" padding-top: 5px; padding-bottom: 1px; height: 1px;">
                        <b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SEARCH CRITERIA</b></p>
                    <asp:ValidationSummary ID="vsSearch" runat="server" 
                        CssClass="ValidationSummary" ValidationGroup="vgSearch" Height="1px" />
                </td>
            </tr>
             <tr>
                     <th style="width:15%;"><br id="brClient"/>
                         <br />
                         Client:<br /><br />Last: <br /><br />First:  </th>
                     <td class="auto-style23">
                         <br />
                         <uc:ClientComboBox ID="ucClientComboBox" runat="server" />
                         <!-- &nbsp;Id:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     <asp:TextBox ID="tbId" runat="server" CssClass="textbox" MaxLength="120" Width="266"></asp:TextBox>
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;<br /> -->
                         <br />
                         <br />
                         <!--Last:&nbsp;&nbsp;-->
                         <asp:TextBox ID="tbLast" runat="server" CssClass="textbox" MaxLength="80" Width="273"></asp:TextBox>
                         &nbsp;<br />
                         <br />
                         <!--First:&nbsp;&nbsp;&nbsp;-->
                         <asp:TextBox ID="tbFirst" runat="server" CssClass="textbox" MaxLength="120" Width="273px"></asp:TextBox>
                         <br />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br /><!-- Email:-->

                         
                     </td>
                 <th style="width:2%;"><br />
                     <br />
                     Email:<br />
                     <br />
                     Phone: <br /><br />Status: <br /><br /><!--Client: <br />Vendor: <br /> General: <br />
                     <br />
                     Priority: <br />--></th>
                         
                     <td class="auto-style22">
                         <br />
                         <br />
                         <asp:TextBox ID="tbEmail" runat="server" CssClass="textbox" MaxLength="80" Width="268px"></asp:TextBox>
                         <br />
                         <br />
                         <asp:TextBox ID="tbPhone" runat="server" CssClass="textbox" MaxLength="80" Width="268px"></asp:TextBox>
                         <br />
                         <br />
                        <!-- Status:&nbsp;-->
                         <asp:DropDownList ID="ddlStatus" runat="server" CssClass="dropdown" Width="270px">
                             <asp:ListItem Text="All" Value="-1" />
                             <asp:ListItem Text="Active" Value="1" />
                             <asp:ListItem Text="Inactive" Value="0" />
                             
                         </asp:DropDownList>
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br /> 
                         <br />
                         
                     </td>
                 <td>
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Client:&nbsp;&nbsp;&nbsp; 
                     <asp:CheckBox ID="chkClient" runat="server" />
                         <br />
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Vendor:&nbsp; 
                         <asp:CheckBox ID="chkVendor" runat="server" />
                         <br />
                         <!--General:&nbsp;-->
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; General: 
                         <asp:CheckBox ID="chkGeneral" runat="server" />
                         <br />
                         <br />
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Priority:&nbsp; 
                         <asp:CheckBox ID="chkPriority" runat="server" />
                     </td>
                </tr>
                <tr>
                    <td colspan="4" style="padding-bottom:500px"  class="auto-style12">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button href="#pnl" ID="btnSearch" runat="server" CssClass="actionbutton" OnClick="btnSearch_Click" Text="SEARCH" ValidationGroup="vgSearch" />
                        &nbsp;
                        <asp:Button ID="btnClearSearch" runat="server" CausesValidation="false" CssClass="actionbutton2" OnClick="btnClearSearch_Click" Text="CLEAR" />
                    </td>
                </tr>
          
        </table>
        
        <br /><br />
        
    </asp:Panel>
    <div id="pnl">
        <asp:Panel ID="pnlGrid" runat="server">
        <uc:ClientContactGrid ID="ucClientContactGrid" runat="server" Visible="true" />
            <a name="bottom"></a>
    </asp:Panel>
        </div>
            <telerik:RadAjaxManagerProxy ID="rampClientContact" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="pnlSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="ralpClientContact" />
                    <telerik:AjaxUpdatedControl ControlID="pnlSearch" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="pnlGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="ralpClientContact" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
        <telerik:RadAjaxLoadingPanel ID="ralpClientContact" runat="server" Transparency="25" BackColor="#E0E0E0">
        <img alt="Loading..." src="/Images/loading.gif" style="border: 0px; padding-top:90px;" />
    </telerik:RadAjaxLoadingPanel>
    
</asp:Content>

