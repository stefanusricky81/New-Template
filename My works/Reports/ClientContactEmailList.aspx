<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientContactEmailList.aspx.cs" Inherits="Reports_ClientContactEmailList" %>
<%@ Register TagPrefix="ddl" TagName="ClientEmailCategory" Src="~/UserControl/DropDownList/ClientEmailCategory.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
     <script type="text/javascript">
         function CheckAll(id) {
             var masterTable = $find("<%= rgClientEmailCategory.ClientID %>").get_masterTableView();
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
        var masterTable = $find("<%= rgClientEmailCategory.ClientID %>").get_masterTableView();
        //accessing header checkbox
        var chkBox = $('input[id$="checkAll"]');
        chkBox[0].checked = false;
         }

         function OnClientLoad(editor, args) {
             var style = editor.get_contentArea().style;
             style.backgroundImage = "none";
             style.backgroundColor = "white";
             style.color = "black";
             style.fontFamily = "Arial";
             style.fontSize = 15 + "px";
         }
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Client Contact Email List</h1>
        </div>
    </div>
    <asp:ValidationSummary ID="vsClientContactEmailList" runat="server" CssClass="validationSummary" ValidationGroup="vgClientContactEmailList" />
    <asp:ValidationSummary ID="vsSendEmail" runat="server" CssClass="validationSummary" ValidationGroup="vgSendEmail" />
    <asp:Literal ID="litMessage" runat="server" />
    <div class="block">
        <div class="block-title">
            <h2><strong>Search Criteria</strong></h2>
        </div>
        <div class="row">
            <div class="form-group col-sm-6">
                <label>Client </label>
                <ddl:Client ID="ddlClient" runat="server" DisplayChosenScript="true" ValidationGroup="vgClientContactEmailList" />
            </div>
            <div class="form-group col-sm-6">
                <label>Client Category</label>
                <ddl:ClientEmailCategory ID="ddlClientCategory" runat="server" IsRequired="true" Active="true" Available="C" DisplayChosenScript="true" ValidationGroup="vgClientContactEmailList" />
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-6">
                <label>Client Contact Category</label>
                <ddl:ClientEmailCategory ID="ddlClientContactCategory" runat="server" IsRequired="true" Active="true" Available="CC" DisplayChosenScript="true" ValidationGroup="vgClientContactEmailList" />
            </div>
        </div>
        <div class="row">
            <div class="form-group form-actions" style="padding-top:20px;">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgClientContactEmailList"><i class="hi hi-search"></i> Search</asp:LinkButton>
                <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
            </div>
        </div>
    </div>
    <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="false">
        <div class="block" style="padding-bottom: 20px;">
            <div class="block-title">
                <h2><strong>Search Results</strong></h2>
                <asp:PlaceHolder ID="phScrollNotes" runat="server"></asp:PlaceHolder>
            </div>
            <div class="row">
                <div class="form-group form-actions" style="padding-top:20px;">
                    <asp:LinkButton ID="lbexport" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbexport_Click"><i class="hi hi-export"></i> Export to Excel</asp:LinkButton>
                </div>
            </div>
            <telerik:RadGrid ID="rgClientEmailCategory" OnNeedDataSource="rgClientEmailCategory_NeedDataSource"
                     runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="20" Width="100%"> 
                    <MasterTableView ShowHeadersWhenNoRecords="true" DataKeyNames="Email, pclient_contact" TableLayout="Fixed" Width="100%">
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cboxSelect" runat="server" onclick="unCheckHeader(this)" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="FName" HeaderStyle-Width="9%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="First" SortExpression="FName" UniqueName="FName" HeaderTooltip="First Name" />
                            <telerik:GridBoundColumn DataField="LName" HeaderStyle-Width="9%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Last" SortExpression="LName" UniqueName="LName" HeaderTooltip="Last Name" />
                            <telerik:GridBoundColumn DataField="Company" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Client" SortExpression="Company" UniqueName="Company" HeaderTooltip="Client" />
                            <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" HeaderTooltip="Email" />
                            <telerik:GridBoundColumn DataField="Phone" HeaderStyle-Width="6%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Phone" SortExpression="Phone" UniqueName="Phone" HeaderTooltip="Phone" />
                            <telerik:GridBoundColumn DataField="ClientCat" HeaderStyle-Width="6%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Client Category" SortExpression="ClientCat" UniqueName="ClientCat" HeaderTooltip="Client Category" />
                            <telerik:GridBoundColumn DataField="CCCat" HeaderStyle-Width="6%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Client Contact Category" SortExpression="CCCat" UniqueName="CCCat" HeaderTooltip="Client Contact Category" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            <div class="row">
                <div class="form-group form-actions" style="padding-top:20px;">
                    <asp:LinkButton ID="lbSendEmail" runat="server" CssClass="btn btn-sm btn-primary" ValidationGroup="vgSendEmail" OnClick="lbSendEmail_Click"><i class="hi hi-export"></i> Send Email</asp:LinkButton>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
    <div class="block" style="padding-bottom: 20px;">
        <div class="block-title">
            <h2><strong>Compose the Email</strong></h2>
        </div>
        <div class="form-horizontal form-bordered" style="padding-bottom:15px;">
            <div class="form-group">
                <label class="col-md-2 control-label">Subject</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" ForeColor="Red" ErrorMessage="Email Subject is required" ValidationGroup="vgSendEmail" Display="None">*</asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-2 control-label">Body</label>
                <div class="col-sm-10">
                    <%--<asp:TextBox ID="txtBody" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" />--%>
                    <telerik:RadEditor runat="server" ID="txtBody" OnClientLoad="OnClientLoad" Skin="Telerik">
                        <Tools>
                            <telerik:EditorToolGroup>
                                <telerik:EditorTool Name="FormatBlock" />
                                <telerik:EditorTool Name="FontName" />
                                <telerik:EditorTool Name="FontSize" />
                            </telerik:EditorToolGroup>
                            <telerik:EditorToolGroup>
                                <telerik:EditorTool Name="AbsolutePosition" />
                                <telerik:EditorTool Name="Bold" />
                                <telerik:EditorTool Name="Italic" />
                                <telerik:EditorTool Name="Underline" />
                                <telerik:EditorTool Name="StrikeThrough" />
                                <telerik:EditorTool Name="JustifyLeft" />
                                <telerik:EditorTool Name="JustifyCenter" />
                                <telerik:EditorTool Name="JustifyRight" />
                                <telerik:EditorTool Name="JustifyFull" />
                                <telerik:EditorTool Name="JustifyNone" />
                                <telerik:EditorTool Name="Indent" />
                                <telerik:EditorTool Name="Outdent" />
                                <telerik:EditorTool Name="InsertOrderedList" />
                                <telerik:EditorTool Name="InsertUnorderedList" />
                                <telerik:EditorTool Name="ToggleTableBorder" />
                                <telerik:EditorTool Name="XhtmlValidator" />
                            </telerik:EditorToolGroup>
                        </Tools>
                    </telerik:RadEditor>
                    <asp:RequiredFieldValidator ID="rfvEmailBody" runat="server" ControlToValidate="txtBody" ForeColor="Red" ErrorMessage="Email Body is required" ValidationGroup="vgSendEmail" Display="None">*</asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-2 control-label">Test email</label>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtSendTest" runat="server" CssClass="form-control" />
                </div>
                <div class="col-sm-2">
                    <asp:LinkButton ID="lbSendTest" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbSendTest_Click" ValidationGroup="vgSendEmail"><i class="hi hi-export"></i> Send Test</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

