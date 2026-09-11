<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="MyAccount.aspx.cs" Inherits="MyAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
     <div class="center_content">
         <div class="center_title_bar">My List</div>
        <div style="width:500px; height:auto; float:left; padding:10px 10px 10px 11px;">
            <asp:GridView ID="gvAccount" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="id" EmptyDataText="No Item in the Cart" BackColor="White"  
                CellPadding="3" RowStyle-HorizontalAlign="Center">
                <HeaderStyle ForeColor="#E7E7FF" Font-Bold="True" BackColor="GrayText" />
                <AlternatingRowStyle BackColor="#F7F7F7" />
                <SelectedRowStyle ForeColor="White" Font-Bold="True" BackColor="#9471DE" />
                <RowStyle ForeColor="Black" BackColor="#DEDFDE" />
                <Columns>
                    <asp:BoundField DataField="NO_ORDER" HeaderText="No Order" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="TANGGAL" HeaderText="Tanggal Transaction" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="TOTAL_QUANTITY" HeaderText="Total Quantity" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="TOTAL_PRICE" HeaderText="Total Price" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="IMAGE_PAYMENT_APP" HeaderText="Payment Image" ItemStyle-Width="150px" />
                </Columns>
            </asp:GridView>
        </div>
     </div>
</asp:Content>
    

