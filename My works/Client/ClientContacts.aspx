<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientContacts.aspx.cs" Inherits="Client_ClientContacts" %>

<%@ Register TagPrefix="ctrl" TagName="ClientDetailTabs" Src="~/UserControl/Client/ClientDetailTabs.ascx" %>
<%@ Register TagPrefix="ddl" TagName="ClientEmailCategory" Src="~/UserControl/DropDownList/ClientEmailCategory.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
        <script type="text/javascript">
            function CheckAll(id) {
                var masterTable = $find("<%= rgClient.ClientID %>").get_masterTableView();
                var row = masterTable.get_dataItems();
                if (id.checked == true) {
                    for (var i = 0; i < row.length; i++) {
                        masterTable.get_dataItems()[i].findElement("cboxSelect").checked = true; // for checking the checkboxes
                    }
                }
                else {
                    for (var i = 0; i < row.length; i++) {
                        masterTable.get_dataItems()[i].findElement("cboxSelect").checked = false; // for unchecking the checkboxes
                    }
                }
            }
            function unCheckHeader(id) {
                var masterTable = $find("<%= rgClient.ClientID %>").get_masterTableView();
                        //accessing header checkbox
                        var chkBox = $('input[id$="checkAll"]');
                        chkBox[0].checked = false;
                    }
        </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
<style>
    .RadGrid td.rgPagerCell { padding: 10px; }
    .RadGrid_3b .rgPagerCell .rgPagerLabel { padding: 7px 0 7px;}
    .RadDropDownList .rddlInner {height:25px; margin-top:6px;}
    .RadGrid_3b .RadComboBox .rcbInput {width:50px !important;}
 
    /*.RadUpload .ruBrowse
    {
    display:none !important;
    }*/

</style>
<script>
    function validateUpload(sender, args) {
        var upload = $find("<%=rauFileUpload.ClientID%>");
        args.IsValid = upload.getUploadedFiles().length != 0;
    }
</script>
<asp:PlaceHolder ID="phTabletPagerCss" runat="server" Visible="false">
    <style>
        .RadGrid_3b .rgPagerCell .rgNumPart span { width:25px; }
        .RadGrid_3b .rgPagerCell .rgPagePrev, .rgPageFirst, .rgPageNext, .rgPageLast {display:none; } 
    </style>
</asp:PlaceHolder>
<%--<telerik:RadAjaxPanel ID="rapClient" runat="server" LoadingPanelID="ralpClient">--%>
<asp:Literal ID="litMessage" runat="server" />
<asp:Panel ID="pnlMain" runat="server">
    <ctrl:ClientDetailTabs ID="ClientTabs" runat="server" SelectedTabIndex="2" />
    <div class="row">
        <div class="col-md-12">
            <div class="block">
                <div class="row" style="margin-bottom:15px;">
                    <div class="col-sm-6">
                        <label>First Name</label>
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" MaxLength="30" PlaceHolder="First Name ...." TabIndex="2" />
                    </div>
                    <div class="col-sm-6">
                        <label>Last Name</label>
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" MaxLength="30" PlaceHolder="Last Name ...." TabIndex="3" />
                    </div>
                </div>
                <div class="row" style="margin-bottom:15px;">
                    <div class="col-sm-6">
                        <label>Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control select-chosen" TabIndex="1">
                            <asp:ListItem Text="All Contacts" Value=""></asp:ListItem>
                            <asp:ListItem Text="Active Contacts" Value="Y" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Inactive Contacts" Value="N"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-6">
                        <label>Email Category</label>
                        <ddl:ClientEmailCategory ID="ddlClientEmailCategory" runat="server" IsRequired="true" Active="true" Available="CC" DisplayChosenScript="true" ValidationGroup="vgContact" />
                    </div>
                </div>
                <div class="row" style="margin-bottom:15px;">
                    <div class="col-sm-6">
                        <label>Phone</label>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" PlaceHolder="Phone...." TabIndex="4" />
                    </div>
                    <div class="col-sm-6">
                        <label>VIP</label><br />
                        <asp:CheckBox ID="cbVIP" runat="server" CssClass="form-control-borderless" TabIndex="5" />
                    </div>
                </div>
                <div class="form-group form-actions" style="padding-top:20px;">
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgSearch"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                </div>
            </div>
            <div class="block">
                <div class="row">
                    <div class="form-group form-actions" style="padding-top:20px;">
                        <asp:LinkButton ID="lbexport" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbexport_Click"><i class="hi hi-export"></i> Export to Excel</asp:LinkButton>
                    </div>
                </div>
                <div class="table-responsive" style="padding-bottom: 10px">
                    <telerik:RadGrid ID="rgClient" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                        OnNeedDataSource="rgClient_NeedDataSource"  
                        OnItemDataBound="rgClient_ItemDataBound" 
                        OnPreRender="rgClient_PreRender" OnItemCommand="rgClient_ItemCommand"
                        AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" ShowFooter="false" PageSize="25" Width="100%">
                        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="pclientcontact" Width="100%" CommandItemDisplay="Top">
                            <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="10,25,50,100,250" PageSizeControlType="RadComboBox" Position="Bottom" />
                            <NoRecordsTemplate>
                                <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                            </NoRecordsTemplate>
                            <CommandItemTemplate>
                                <div style="margin:10px;">
                                    <asp:LinkButton ID="hlAddNewRecord" runat="server" OnClick="hlAddNewRecord_Click"><i class="gi gi-user_add" title="Add New Contact"></i>  Add New Contact</asp:LinkButton>  &nbsp;&nbsp;
                                    <asp:LinkButton ID="hlBulkAdd" runat="server" OnClick="hlBulkAdd_Click"><asp:Image ID="imgbulk" runat="server" ImageUrl="~/Images/bulkupload.jpg" />  Bulk Upload</asp:LinkButton>  
                                    <asp:LinkButton ID="lbDeactivate" runat="server" OnClick="lbDeactivate_Click"><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/deactivate.png" />  Deactivate</asp:LinkButton>  
                                </div>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cboxSelect" runat="server" onclick="unCheckHeader(this)" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="pclient" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="7%" HeaderText="" UniqueName="ActionColumn">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="btnEditRecord" runat="server" NavigateUrl='<%# String.Format("ClientContactAdd.aspx?ClientContactID={0}&ClientId={1}",Eval("pclientcontact"),Eval("pClient")) %>'><i class="gi gi-pencil" title="Edit"></i></asp:HyperLink>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="btnDeactivate" runat="server" CommandArgument="Deactivate" OnClientClick="javascript:return confirm('Make Deactive?');" > <i class="gi gi-ban" title="Deactivate"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                                        
                                <telerik:GridBoundColumn DataField="UserPriority" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Right" HeaderText="Pr #" SortExpression="UserPriority" UniqueName="UserPriority" />

                                <telerik:GridBoundColumn DataField="Last" HeaderStyle-Width="15%" Display="false" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Last" SortExpression="Last" UniqueName="LastName" />
                                
                                <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="15%" HeaderText="Last" UniqueName="Last">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="btnGo" runat="server" Text='<%# Eval("Last") %>' NavigateUrl='<%# String.Format("ClientContactAdd.aspx?ClientContactID={0}&ClientId={1}",Eval("pclientcontact"),Eval("pClient")) %>'/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridBoundColumn DataField="First" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="First" SortExpression="First" UniqueName="First" />
                                <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" />
                                <telerik:GridBoundColumn DataField="ContactBusPhone" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Phone" SortExpression="ContactBusPhone" UniqueName="ContactBusPhone" />
                                <telerik:GridBoundColumn DataField="ContactCellPhone" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Cell" SortExpression="ContactCellPhone" UniqueName="ContactCellPhone" />
                                <telerik:GridBoundColumn DataField="LocationName" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Location" SortExpression="LocationName" UniqueName="LocationName" Visible="false" />
                                <telerik:GridBoundColumn DataField="ContactActive" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Active" SortExpression="ContactActive" UniqueName="ContactActive" />
                                <telerik:GridBoundColumn DataField="CategoryEmail" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email Category" SortExpression="CategoryEmail" UniqueName="CategoryEmail" />
                                <telerik:GridBoundColumn DataField="VIP" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="VIP" SortExpression="VIP" UniqueName="VIP" />
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" />
                    </telerik:RadGrid>
                </div>
        </div>
        </div>
    </div>
</asp:Panel>
<%--</telerik:RadAjaxPanel>--%>
<telerik:RadAjaxLoadingPanel ID="ralpClient" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>

<div id="myModalBulkAdd" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title" id="myModalLabelBulkAdd"><asp:Label ID="lblModalTitleBulkAdd" runat="server" /></h4>
            </div>
            <asp:ValidationSummary ID="vsBulkAdd" runat="server" CssClass="validationSummary" ValidationGroup="vgBulkAdd" />
            <div class="modal-body">
                <div class="row">
                    <div class="form-group">
                        <div class="col-md-12">
                            <div class="row">
                                <label class="col-md-4 control-label">Available Location</label>
                                <div class="col-md-8 control-label"> 
                                    <asp:Label ID="lblLocation" runat="server" />
                                </div>
                            </div>                               
                        </div>
                        <div class="col-md-12">
                            <div class="row">
                                <label class="col-md-12 control-label">Template File - <asp:LinkButton ID="btnDownload" runat="server" Text="Download here" OnClick="btnDownload_Click" /></label>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row">
                                <label class="col-md-4 control-label">File</label>
                                <div class="col-md-8">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="rauFileUpload" MaxFileInputsCount="1" />
                                            <asp:CustomValidator runat="server" ID="cvUpload" ValidationGroup="vgBulkAdd" Display="None" ClientValidationFunction="validateUpload" ErrorMessage="Please Select File To Upload" />
                                        </div>
                                        <div class="col-md-1">&nbsp;</div>
                                        <div class="col-md-1">
                                            <asp:Button ID="btnImport" runat="server" Text="Upload" class="btn btn-primary" ValidationGroup="vgBulkAdd" OnClick="btnImport_Click" />
                                        </div>
                                    </div>                                  
                                </div>
                            </div>
                        </div>
                </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
            </div>
        </div>
    </div>
</div>
</asp:Content>
