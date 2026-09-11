<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AdminHome.aspx.cs" Inherits="AdminHome" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div>
            <center><h1 style="width:945px; color:#000000; font-size:20px; margin-bottom:10px;">Master Home</h1></center>
        </div>
        <div class="sign-in">
            <table>
                <tr>
                    <td>Name :</td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>First Image :</td>
                    <td></td>
                    <td><asp:FileUpload ID="fileuploadimagesFirst" runat="server" /></td>
                </tr>
                <tr>
                    <td>Second Image :</td>
                    <td></td>
                    <td><asp:FileUpload ID="fileuploadimagesSecond" runat="server" /></td>
                </tr>
                <tr>
                    <td>Third Image :</td>
                    <td></td>
                    <td><asp:FileUpload ID="fileuploadimagesThird" runat="server" /></td>
                </tr>
                <tr>
                    <td>Fourth Image :</td>
                    <td></td>
                    <td><asp:FileUpload ID="fileuploadimagesFourth" runat="server" /></td>
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
        <div class="sign-in">
                <tr>    
                   <td></td>
                    <td></td>      
                    <td>
                        <asp:GridView ID="GV" runat="server" AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="5" HeaderStyle-HorizontalAlign="Left"
                            CellPadding="2" CellSpacing="1" BorderColor="Black" BackColor="White"
                            GridLines="None" OnPageIndexChanging="myGridView_PageIndexChanging"
                            EmptyDataText="[ No Data Available ]" DataKeyNames="ID,First_Image_Home,Second_Image_Home,Third_Image_Home,Fourth_Image_Home" >
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
                                <asp:HyperLinkField DataTextField="HOME_NAME" HeaderText="Product Name" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="AdminHome.aspx?type=edit&ID={0}" ItemStyle-Width="50px" />
                                <asp:TemplateField HeaderText="Image" SortExpression="img">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" Width="100px" Height="100px" ImageUrl='<%# Bind("First_Image_Home")  %>' />
                                        <asp:Image ID="Image2" runat="server" Width="100px" Height="100px" ImageUrl='<%# Bind("Second_Image_Home")  %>' />
                                        <asp:Image ID="Image3" runat="server" Width="100px" Height="100px" ImageUrl='<%# Bind("Third_Image_Home")  %>' />
                                        <asp:Image ID="Image4" runat="server" Width="100px" Height="100px" ImageUrl='<%# Bind("Fourth_Image_Home")  %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle ForeColor="White" Font-Bold="True" BackColor="#9471DE" />
                            <RowStyle ForeColor="Black" BackColor="#DEDFDE" />
                        </asp:GridView>
                    </td>   
                </tr>
        </div>
</asp:Content>

