<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientContactEmailHistory.aspx.cs" Inherits="Reports_ClientContactEmailHistory" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script type="text/javascript">
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
            <h1>Client Contact Email History</h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" />
    <div class="block">
        <div class="block-title">
            <h2><strong>Email History</strong></h2>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="row topMargin5" style="margin-left:-35px">
                    <div class="col-md-4">
                        <div style="float:right">
                            <label>Email Subject</label>
                        </div>
                    </div>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtEmailSubject" runat="server" CssClass="form-control" PlaceHolder="Enter Subject Email" />
                    </div>
                </div>
                <div class="row topMargin5" style="margin-left:-35px">
                    <div class="col-md-4">
                        <div style="float:right">
                            <label>Send Date Start</label>
                        </div>                            
                    </div>
                    <div class="col-md-8">
                        <uc:DatePicker ID="ucSendDateStart" runat="server" IsRequired="false" />
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="row topMargin5" style="margin-left:-35px">
                    <div class="col-md-4">
                        <div style="float:right">
                            <label>Email Body</label>
                        </div>
                    </div>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtEmailBody" runat="server" CssClass="form-control" PlaceHolder="Enter Body Email" />
                    </div>
                </div>
                <div class="row topMargin5" style="margin-left:-35px">
                    <div class="col-md-4">
                        <div style="float:right">
                            <label>Send Date End</label>
                        </div>                            
                    </div>
                    <div class="col-md-8">
                        <uc:DatePicker ID="ucSendDateEnd" runat="server" IsRequired="false" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-group form-actions" style="margin-left:8px">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click"><i class="hi hi-search"></i> Search</asp:LinkButton>
                <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
            </div>
        </div>
    </div>
    <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="true">
        <div class="block" style="padding-bottom: 20px;">
            <div class="block-title">
                <h2><strong>Search Results</strong></h2>
                <asp:PlaceHolder ID="phScrollNotes" runat="server"></asp:PlaceHolder>
            </div>
            <telerik:RadGrid ID="rgEmailHistory" OnNeedDataSource="rgEmailHistory_NeedDataSource" OnItemCommand="rgEmailHistory_ItemCommand"
                 OnItemDataBound="rgEmailHistory_ItemDataBound"
                     runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="20" Width="100%"> 
                <MasterTableView ShowHeadersWhenNoRecords="true" DataKeyNames="ID" TableLayout="Fixed" Width="100%">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" HeaderText="" UniqueName="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnLookUp" runat="server"  CommandArgument="LookUp" > <i class="gi gi-circle_info" title="Look Up"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="Subject" HeaderStyle-Width="9%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Subject" SortExpression="Subject" UniqueName="Subject" HeaderTooltip="Email Subject" />
                        <%--<telerik:GridBoundColumn DataField="EmailBody" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Body" SortExpression="EmailBody" UniqueName="EmailBody" HeaderTooltip="Email Body" />--%>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" HeaderText="" UniqueName="EmailBody">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEmailBody" runat="server" Text='<%# Eval("Body").ToString().Trim() %>' CommandArgument="BodyEmail" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="SendDate" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Send Date" SortExpression="SendDate" UniqueName="SendDate" HeaderTooltip="SendDate" />
                        <telerik:GridBoundColumn DataField="ID" Display="false" HeaderStyle-Width="10%" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="ID" SortExpression="ID" UniqueName="ID" />
                        <telerik:GridBoundColumn DataField="Body" Display="false" HeaderStyle-Width="10%" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="Body" SortExpression="Body" UniqueName="Body" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        
    </asp:PlaceHolder>

    <div id="myModalEmailContact" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabelEmailContact"><asp:Label ID="lblModalTitleEmailContact" runat="server" /></h4>
                    <asp:HiddenField ID="hfID" runat="server" />
                </div>
                <div class="modal-body">
                    <div class="row">
                        <telerik:RadGrid ID="rgEmailContact" OnNeedDataSource="rgEmailContact_NeedDataSource" runat="server" Skin="3b" 
                            EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" ShowFooter="false" 
                            PageSize="20" Width="100%"> 
                            <MasterTableView ShowHeadersWhenNoRecords="true" DataKeyNames="Email" TableLayout="Fixed" Width="100%">
                                <NoRecordsTemplate>
                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="9%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" HeaderTooltip="Email" />
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Back</button>
                </div>
            </div>
        </div>
    </div>

    <div id="myBodyEmail" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="width:115%">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabelBodyEmail"><asp:Label ID="lblModalBodyEmail" runat="server" /></h4>
                    <asp:HiddenField ID="hfBodyEmail" runat="server" />
                </div>
                <div class="modal-body">
                    <div class="row">
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
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Back</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>