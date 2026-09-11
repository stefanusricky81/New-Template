<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AdminEventAndPromotion.aspx.cs" Inherits="AdminEventAndPromotion" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div>
            <center><h1 style="width:945px; color:#000000; font-size:20px; margin-bottom:10px;">Event And Promotion</h1></center>
        </div>
        <div class="sign-in">
            <table>
                <tr>
                    <td>Event Name :</td>
                    <td></td>
                    <td><asp:TextBox ID="txtEventName" runat="server" /></td>
                </tr>
                <tr>
                    <td>Product Image :</td>
                    <td></td>
                    <td><asp:FileUpload ID="fileuploadimages" runat="server" /></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Button class="button" ID="btnAdd" Text="Submit" runat="server" 
                            BackColor="Black" ForeColor="White" Width="75" Height="30" 
                            onclick="btnAdd_Click" />
                        <asp:Button class="button" ID="btnCancel" Text="Cancel" runat="server" 
                            BackColor="Black" ForeColor="White" Width="75" Height="30" 
                            onclick="btnCancel_Click"  />
                    </td>
                </tr>
            </table>
        </div>
        <div class="sign-in">
                <tr>    
                   <td></td>
                    <td></td>      
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
                                <asp:HyperLinkField DataTextField="EVENT_NAME" HeaderText="Product Name" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="AdminEventAndPromotion.aspx?type=edit&ID={0}" ItemStyle-Width="150px" />
                                <asp:TemplateField HeaderText="Image" SortExpression="img">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" Width="100px" Height="100px" ImageUrl='<%# Bind("IMAGE")  %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle ForeColor="White" Font-Bold="True" BackColor="#9471DE" />
                            <RowStyle ForeColor="Black" BackColor="#DEDFDE" />
                        </asp:GridView>
                    </td>   
                </tr>
                <tr>
                    <td>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>
                    <td>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>
                    <td>
                        <asp:Button ID="bDelete" Text="Delete" runat="server" BackColor="Black" 
                            ForeColor="White" Width="75" Height="30" />
                    </td>
                </tr>
        </div>
</asp:Content>

