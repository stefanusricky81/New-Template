<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="GlobalDesktopSettings.aspx.cs" Inherits="Maintenance_GlobalDesktopSettings" %>

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
            <h1>Global Desktop Settings</h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" />
    <asp:Panel ID="pnlContainer" runat="server">
        <ajaxToolkit:CollapsiblePanelExtender ID="cpeAutoClose" 
                    runat="Server"
                    TargetControlID="pnlAutoClose"
                    ExpandControlID="pnlShowHideAutoClose"
                    CollapseControlID="pnlShowHideAutoClose" 
                    BehaviorID="cpeAutoClose"
                    TextLabelID="lblTAutoCloseHeader"
                    ImageControlID="imgAutoCloseHeader"    
                    ExpandedImage="~/Images/collapse_blue.jpg"
                    CollapsedImage="~/Images/expand_blue.jpg"
                    SuppressPostBack="true"
                    CollapsedText="Auto Close [click to display]"
                    ExpandedText="Auto Close [click to hide]"
                     Collapsed="true"/>
        <div class="block-title">
            <asp:Panel ID="pnlShowHideAutoClose" runat="server" CssClass="collapsePanelHeaderTicket" Height="35px">
                <div style="float: left;">
                    <h2><strong>
                        <asp:Label ID="lblTAutoCloseHeader" runat="server" />
                        </strong>
                    </h2>                            
                </div>
                <div style="float: right; vertical-align: middle; margin:10px 10px 0px 0px">
                    <asp:ImageButton ID="imgAutoCloseHeader" runat="server" ImageUrl="~/Images/expand_blue.jpg" />
                </div>
            </asp:Panel>
        </div>
        <asp:Panel ID="pnlAutoClose" style="overflow:hidden" runat="server">
            <div class="block" >
                <div class="form-horizontal form-bordered">
                    <div class="form-group">
                        <label class="col-md-3 col-sm-4 control-label"># of hours after previous notification.</label>
                        <div class="col-md-3 col-sm-6">
                            <asp:TextBox ID="txtFirst" CssClass="form-control" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-md-3 col-sm-4 control-label">First Notification Email</label>
                        <div class="col-md-3 col-sm-6">
                            <telerik:RadEditor runat="server" ID="txtFirstNotification" OnClientLoad="OnClientLoad" Skin="Telerik">
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

                    <div class="form-group">
                        <label class="col-md-3 col-sm-4 control-label"># of hours after previous notification.</label>
                        <div class="col-md-3 col-sm-6">
                            <asp:TextBox ID="txtSecond" CssClass="form-control" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-md-3 col-sm-4 control-label">Second Notification Email</label>
                        <div class="col-md-3 col-sm-6">
                            <telerik:RadEditor runat="server" ID="txtSecondNotification" OnClientLoad="OnClientLoad" Skin="Telerik">
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

                    <div class="form-group">
                        <label class="col-md-3 col-sm-4 control-label"># of hours after previous notification.</label>
                        <div class="col-md-3 col-sm-6">
                            <asp:TextBox ID="txtAuto" CssClass="form-control" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-md-3 col-sm-4 control-label">Auto close Notification Email</label>
                        <div class="col-md-3 col-sm-6">
                            <telerik:RadEditor runat="server" ID="txtAutoNotification" OnClientLoad="OnClientLoad" Skin="Telerik">
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

                    <div class="form-group">
                        <asp:LinkButton ID="lbSave" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbSave_Click" ValidationGroup="vgSendEmail"><i class="hi hi-export"></i> Submit</asp:LinkButton>
                    </div>
                </div>
            </div>            
        </asp:Panel>


    </asp:Panel>

    <asp:Panel ID="pnlContainer2" runat="server">
        <ajaxToolkit:CollapsiblePanelExtender ID="cpeClientCreditCardFees" 
                    runat="Server"
                    TargetControlID="pnlClientCreditCardFees"
                    ExpandControlID="pnlShowHideClientCreditCardFees"
                    CollapseControlID="pnlShowHideClientCreditCardFees" 
                    BehaviorID="cpeClientCreditCardFees"
                    TextLabelID="lblClientCreditCardFeesHeader"
                    ImageControlID="imgClientCreditCardFees"    
                    ExpandedImage="~/Images/collapse_blue.jpg"
                    CollapsedImage="~/Images/expand_blue.jpg"
                    SuppressPostBack="true"
                    CollapsedText="Client Credit Card Fees [click to display]"
                    ExpandedText="Client Credit Card Fees [click to hide]"
                    Collapsed="true"/>
        <div class="block-title">
            <asp:Panel ID="pnlShowHideClientCreditCardFees" runat="server" CssClass="collapsePanelHeaderTicket" Height="35px">
                <div style="float: left; padding-right:60%">
                    <h2><strong>
                        <asp:Label ID="lblClientCreditCardFeesHeader" runat="server" />
                        </strong>
                    </h2>                            
                </div>
                <div style="float: right; vertical-align: middle; margin:10px 10px 0px 0px">
                    <asp:ImageButton ID="imgClientCreditCardFees" runat="server" ImageUrl="~/Images/expand_blue.jpg" />
                </div>
            </asp:Panel>
        </div>
        <asp:Panel ID="pnlClientCreditCardFees" style="overflow:hidden" runat="server">
            <div class="block" >
                <div class="form-horizontal form-bordered">
                    <div class="form-group">
                        <label class="col-md-3 col-sm-4 control-label">The Fee</label>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtFee" CssClass="form-control" runat="server" /> 
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblPercent" runat="server" Text="%" Font-Bold="true" />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:LinkButton ID="lbSaveCreditCardFees" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbSaveCreditCardFees_Click" ValidationGroup="vgCreditCardFees"><i class="hi hi-export"></i> Submit</asp:LinkButton>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="pnlContainer3" runat="server">
        <ajaxToolkit:CollapsiblePanelExtender ID="cpeEmailSurvey" 
                    runat="Server"
                    TargetControlID="pnlSurveyEmail"
                    ExpandControlID="pnlShowHideSurveyEmail"
                    CollapseControlID="pnlShowHideSurveyEmail" 
                    BehaviorID="cpeEmailSurvey"
                    TextLabelID="lblSurveyEmail"
                    ImageControlID="imgSurveyEmail"    
                    ExpandedImage="~/Images/collapse_blue.jpg"
                    CollapsedImage="~/Images/expand_blue.jpg"
                    SuppressPostBack="true"
                    CollapsedText="Email Survey [click to display]"
                    ExpandedText="Email Survey [click to hide]"
                    Collapsed="true"/>
        <div class="block-title">
            <asp:Panel ID="pnlShowHideSurveyEmail" runat="server" CssClass="collapsePanelHeaderTicket" Height="35px">
                <div style="float: left; padding-right:60%">
                    <h2><strong>
                        <asp:Label ID="lblSurveyEmail" runat="server" />
                        </strong>
                    </h2>                            
                </div>
                <div style="float: right; vertical-align: middle; margin:10px 10px 0px 0px">
                    <asp:ImageButton ID="imgSurveyEmail" runat="server" ImageUrl="~/Images/expand_blue.jpg" />
                </div>
            </asp:Panel>
        </div>
        <asp:Panel ID="pnlSurveyEmail" style="overflow:hidden" runat="server">
            <div class="block">
                <div class="form-horizontal form-bordered">
                    <div class="form-group">
                        <label class="col-md-3 col-sm-4 control-label">Survey Id</label>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtSurveyId" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvSurveyId" runat="server" ControlToValidate="txtSurveyId" Display="None" ErrorMessage="Please Insert Survey Id" ValidationGroup="vgSurveyId" />
                        </div>
                        <div class="col-md-6">
                            <label>Send Email</label>
                            <label class="switch switch-success">
                                <asp:CheckBox ID="chkSendMailSurveyID" runat="server" /><span></span>
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:LinkButton ID="lbSurveyId" runat="server" OnClick="lbSurveyId_Click" ValidationGroup="vgSurveyId" CssClass="btn btn-sm btn-primary"><i class="hi hi-save"></i> Save</asp:LinkButton>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>

</asp:Content>