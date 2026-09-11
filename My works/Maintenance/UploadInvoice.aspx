<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="UploadInvoice.aspx.cs" Inherits="Maintenance_UploadInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
<style>
  .upload1 .ruBrowse .ruFakeInput
    {
        background-position: 0 -46px !important;
        width: 120px !important;
        height:100px !important;
    } 

</style>
<script>
function validateUpload(sender, args) {
    var upload = $find("<%=rauFileUpload.ClientID%>");
    args.IsValid = upload.getUploadedFiles().length != 0;
}
</script>

    <div class="content-header">
        <div class="header-section">
            <h1>Synnex Billing Import Upload</h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" />
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnImport">
        <asp:ValidationSummary ID="vsUpload" runat="server" CssClass="validationSummary" ValidationGroup="vgUpload" />
        <div class="block">
            <div class="row">
                <div class="form-group col-sm-1">
                    <label>File ID<span class="text-danger">*</span></label>
                </div>
                <div class="form-group col-sm-5">
                    <asp:TextBox ID="txtFileName" runat="server" MaxLength="25" CssClass="form-control" PlaceHolder="Enter File name" />
                    <asp:RequiredFieldValidator ID="rfvFileID" runat="server" ValidationGroup="vgUpload" Display="None" ControlToValidate="txtFileName" ErrorMessage="File Id can't be empty"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-sm-6">
                    <telerik:RadAsyncUpload RenderMode="Lightweight" EnableInlineProgress="true" runat="server" ID="rauFileUpload" MaxFileInputsCount="1" CssClass="upload1" />
                    <asp:CustomValidator runat="server" ID="cvUpload" ValidationGroup="vgUpload" Display="None" ClientValidationFunction="validateUpload" ErrorMessage="Please Select File To Upload" />
                </div>
            </div>
            <div class="row"></div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnImport" runat="server" Text="Upload" class="btn btn-primary" ValidationGroup="vgUpload" OnClick="btnImport_Click"/>
                    <%--<asp:Button ID="btnLastImport" runat="server" Text="Display Last Import" class="btn btn-primary" OnClick="btnLastImport_Click"/>--%>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlResults" runat="server">
        <div class="block" style="padding-bottom:20px;">
            <div class="table-responsive">
                <telerik:RadGrid ID="rgUploadInvoice" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%" OnNeedDataSource="rgUploadInvoice_NeedDataSource"
                    ClientSettings-Resizing-AllowColumnResize="true"
                    OnGridExporting="rgUploadInvoice_GridExporting" >
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="pUploadInoviceId" Width="100%" AllowSorting="true">
                        <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="10,25,50,100,250" PageSizeControlType="RadComboBox" Position="Bottom" />
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="pUploadInoviceId" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="#" Display="false" SortExpression="Id" UniqueName="Id" />
                            <telerik:GridBoundColumn DataField="fileName" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="File ID" SortExpression="fileName" UniqueName="fileName" />
                            <telerik:GridBoundColumn DataField="totaldata" HeaderStyle-Width="3%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Total data" SortExpression="totaldata" UniqueName="totaldata" />
                            <telerik:GridBoundColumn DataField="nameofUploadedFile" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Uploaded File" SortExpression="nameofUploadedFile" UniqueName="nameofUploadedFile" />
                            <telerik:GridBoundColumn DataField="createdDate" HeaderStyle-Width="6%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Uploaded Date" SortExpression="createdDate" UniqueName="createdDate" />
                             <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="4%" HeaderText="" UniqueName="Assign">
                                 <ItemTemplate>
                                     <asp:HyperLink ID="btnUploadInvoiceDetail" runat="server" ToolTip="Go to detail" Target="_blank" 
                                         NavigateUrl='<%# Eval("pUploadInoviceId","UploadInvoiceDetail.aspx?UploadId={0}") %>' style="padding-right:5px;">
                                                <i class="fa fa-arrow-right" title="Go to detail">Go to Detail</i></asp:HyperLink>
                                 </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </asp:Panel>
 
</asp:Content>