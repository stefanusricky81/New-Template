<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AdminUploadProduct.aspx.cs" Inherits="AdminUploadProduct" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div>
            <center><h1 style="width:945px; color:#000000; font-size:20px; margin-bottom:10px;">Product</h1></center>
        </div>
        <div class="sign-in">
            <table>
                <tr>
                    <td>Product Name</td>
                    <td></td>
                    <td><asp:TextBox ID="txtProductName" runat="server" Width="240" /></td>
                </tr>
                <tr>
                    <td>Brand</td>
                    <td></td>
                    <td><asp:TextBox ID="txtBrand" runat="server" Width="240" /></td>
                </tr>
                <tr>
                    <td>Price :</td>
                    <td></td>
                    <td><asp:TextBox ID="txtPrice" runat="server" Width="240" /></td>
                </tr>
                <tr>
                    <td>Description</td>
                    <td></td>
                    <td><asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" Width="240" /></td>
                </tr>
                <tr>
                    <td>Material</td>
                    <td></td>
                    <td><asp:TextBox ID="txtMaterial" runat="server" Width="240" /></td>
                </tr>
                <tr>
                    <td>Care</td>
                    <td></td>
                    <td><asp:TextBox ID="txtCare" runat="server" Width="240" /></td>
                </tr>
                <tr>
                    <td>Product Type :</td>
                    <td></td>
                    <td><asp:DropDownList ID="ddlCategory" runat="server" /></td>
                </tr>
                <tr>
                    <td>Product Category :</td>
                    <td></td>
                    <td><asp:DropDownList ID="ddlProductCategory" runat="server" /></td>
                </tr>
                <tr>
                    <td>Product Gender :</td>
                    <td></td>
                    <td><asp:DropDownList ID="ddlProductGender" runat="server" /></td>
                </tr>
                <tr>
                    <td>Product Status :</td>
                    <td></td>
                    <td><asp:DropDownList ID="ddlProductStatus" runat="server" /></td>
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
                        <asp:Button class="button" ID="btnAdd" Text="Add Product" runat="server" 
                            BackColor="Black" ForeColor="White" Width="75" Height="30" 
                            onclick="btnAdd_Click" />
                        <asp:Button class="button" ID="btnCancel" Text="Cancel" runat="server" 
                            BackColor="Black" ForeColor="White" Width="75" Height="30" 
                            onclick="btnCancel_Click" />
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
                            EmptyDataText="[ No Data Available ]" DataKeyNames="ID" OnRowDataBound="GV_RowDataBound" >
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
                                <asp:HyperLinkField DataTextField="PRODUCTNAME" HeaderText="Product Name" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="AdminUploadProduct.aspx?type=edit&ID={0}" ItemStyle-Width="50px" />
                                <asp:BoundField DataField="BRAND" HeaderText="Brand" ItemStyle-Width="200px" />
                                <asp:BoundField DataField="DESCRIP" HeaderText="Description" HtmlEncode="true" ItemStyle-Width="150px" />
                                <asp:BoundField DataField="MATERIAL" HeaderText="Material" ItemStyle-Width="150px" />
                                <asp:BoundField DataField="CARE" HeaderText="Care" ItemStyle-Width="150px" />
                                <asp:TemplateField HeaderText="Image" SortExpression="img">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" Width="100px" Height="100px" ImageUrl='<%# Bind("IMAGE")  %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Product Type" ItemStyle-Width="200px" />
                                <asp:BoundField DataField="PRODUCT_CATEGORY" HeaderText="Product Category" ItemStyle-Width="200px" />
                                <asp:BoundField DataField="GENDER" HeaderText="Product Gender" ItemStyle-Width="200px" />
                                <asp:BoundField DataField="STATUS" HeaderText="Product Status" ItemStyle-Width="200px" />
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
                            ForeColor="White" Width="75" Height="30" onclick="bDelete_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hfImage" runat="server" />
                    </td>
                </tr>
        </div>
</asp:Content>

