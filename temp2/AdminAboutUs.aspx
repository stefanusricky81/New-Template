<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AdminAboutUs.aspx.cs" Inherits="AdminAboutUs" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div>
            <center><h1 style="width:945px; color:#000000; font-size:20px; margin-bottom:10px;">Master About Us</h1></center>
        </div>
        <div class="sign-in">
            <table>
                <tr>
                    <td>For Line 1 </td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtLine1" runat="server" Width="350px" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr>
                    <td>For Line 2 </td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtLine2" runat="server" Width="350px" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr>
                    <td>For Line 3 </td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtLine3" runat="server" Width="350px" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr>
                    <td>For Line 4 </td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtLine4" runat="server" Width="350px" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" BackColor="Black" 
                            ForeColor="White" Width="75" Height="30" onclick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="CANCEL" BackColor="Black" 
                            ForeColor="White" Width="75" Height="30" onclick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <table>
                <tr>          
                    <td>
                        <asp:GridView ID="GV" runat="server" AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="5" HeaderStyle-HorizontalAlign="Left"
                            CellPadding="2" CellSpacing="1" BorderColor="Black" BackColor="White"
                            GridLines="None" OnPageIndexChanging="myGridView_PageIndexChanging"
                            EmptyDataText="[ No Data Available ]" DataKeyNames="ID" >
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
                                <asp:HyperLinkField DataTextField="ID" HeaderText="ID" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="AdminAboutUs.aspx?type=edit&ID={0}" ItemStyle-Width="50px" />
                                <asp:BoundField DataField="Line1" HeaderText="Line 1" ItemStyle-Width="350px" />
                                <asp:BoundField DataField="Line2" HeaderText="Line 2" ItemStyle-Width="35px" />
                                <asp:BoundField DataField="Line3" HeaderText="Line 3" ItemStyle-Width="350px" />
                                <asp:BoundField DataField="Line4" HeaderText="Line 4" ItemStyle-Width="350px" />
                            </Columns>
                            <SelectedRowStyle ForeColor="White" Font-Bold="True" BackColor="#9471DE" />
                            <RowStyle ForeColor="Black" BackColor="#DEDFDE" />
                        </asp:GridView>
                    </td>   
                </tr>
            </table>
</asp:Content>

