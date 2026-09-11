<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="OrderHeader.aspx.cs" Inherits="OrderHeader" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div>
            <center><h1 style="width:945px; color:#000000; font-size:20px; margin-bottom:10px;">Order Header</h1></center>
        </div>
        <table>
            <tr>
                <td></td>
                <td></td>      
                <td>
                    <asp:GridView ID="GV" runat="server" AutoGenerateColumns="False"
                        AllowPaging="true" PageSize="5" HeaderStyle-HorizontalAlign="Left"
                        CellPadding="2" CellSpacing="1" BorderColor="Black" BackColor="White"
                        GridLines="None" OnPageIndexChanging="myGridView_PageIndexChanging"
                        EmptyDataText="[ No Data Available ]" DataKeyNames="No_Order" OnRowDataBound="GV_RowDataBound" >
                        <PagerStyle ForeColor="Black" HorizontalAlign="Center" BackColor="#C6C3C6" />
                        <HeaderStyle ForeColor="#E7E7FF" Font-Bold="True" BackColor="#4A3C8C" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                    <HeaderTemplate>
                                    <asp:CheckBox ID="chkBxHeader" OnClick="javascript:select_deselectAll(this.checked,this.id);" runat="server" />
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                    <ItemTemplate>
                                    <asp:CheckBox ID="chkBxSelect" runat="server"  />
                                    </ItemTemplate>
                            </asp:TemplateField>
                            <asp:HyperLinkField DataTextField="NO_ORDER" HeaderText="Order No" DataNavigateUrlFields="No_Order" DataNavigateUrlFormatString="OrderDetail.aspx?type=edit&NoOrder={0}" ItemStyle-Width="150px" Target="_blank" />
                            <asp:BoundField DataField="EMAIL_ADD" HeaderText="User" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="TOTAL_QUANTITY" HeaderText="Quantity" ItemStyle-Width="150px" />
                            <asp:BoundField DataField="TOTAL_PRICE" HeaderText="Total Harga" ItemStyle-Width="100px" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="NAMA_EKSPEDISI" HeaderText="Ekpedisi" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="BANK_ACCOUNT" HeaderText="No Rek Lss" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="BANK_NAME" HeaderText="Nama Bank Lss" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="ACCOUNT_NAME" HeaderText="Nama Rek Lss" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="ACCOUNT_NO" HeaderText="No Rek User" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="ACCOUNT_CUSTOMER" HeaderText="Nama Rek User" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="MESSAGE_PAYMENT" HeaderText="Description" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="FLAG_PAID" HeaderText="Paid" />
                            <asp:TemplateField HeaderText="Image" SortExpression="img">
                                <ItemTemplate>
                                    <asp:Image ID="Image1" runat="server" Width="100px" Height="100px" ImageUrl='<%# Bind("Image_Payment_App")  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PAYMENT_DATE" HeaderText="Payment Date" DataFormatString="{0:dd-MMM-yyyy}" />
                            <asp:BoundField DataField="FLAG_SEND" HeaderText="Send" />
                            <asp:BoundField DataField="SEND_DATE" HeaderText="Send Date" DataFormatString="{0:dd-MMM-yyyy}"/>
                        </Columns>
                        <SelectedRowStyle ForeColor="White" Font-Bold="True" BackColor="#9471DE" />
                        <RowStyle ForeColor="Black" BackColor="#DEDFDE" />
                    </asp:GridView>
                </td>   
            </tr>
        </table>
</asp:Content>

