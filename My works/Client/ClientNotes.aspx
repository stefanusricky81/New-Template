<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientNotes.aspx.cs" Inherits="Client_ClientNotes" %>
<%@ Register TagPrefix="ctrl" TagName="ClientDetailTabs" Src="~/UserControl/Client/ClientDetailTabs.ascx" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <asp:Literal ID="litMessage" runat="server" />
    <asp:Panel ID="pnlMain" runat="server">
        <ctrl:ClientDetailTabs ID="ClientTabs" runat="server" SelectedTabIndex="20" />

        <asp:PlaceHolder ID="phBillingInformation" runat="server">
            <div class="block">
                <div class="block-title">
                    <h2><strong>Client Notes</strong></h2>
                    <asp:ValidationSummary ID="vsBilling" runat="server" ValidationGroup="vgClientNotes" CssClass="validationSummary" />
                </div>
                <div class="Container">
                    <telerik:RadEditor runat="server" ID="txtNotes" OnClientLoad="OnClientLoad" Skin="Telerik">
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
            <div class="block">
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group form-actions">
                            <asp:LinkButton ID="btnSave" OnClick="btnSave_Click" runat="server" ValidationGroup="vgClientNotes" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>

        
    </asp:Panel>
        
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" Runat="Server">
</asp:Content>

