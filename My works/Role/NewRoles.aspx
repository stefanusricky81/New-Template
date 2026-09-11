<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" Async="true" AutoEventWireup="true" CodeFile="NewRoles.aspx.cs" Inherits="Role_NewRoles" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<%@ Register TagPrefix="ddl" tagName="TimeDDL" src="~/UserControl/DropDownList/Time.ascx"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style>
        .RadGrid_3b .RadComboBox .rcbInput {width:50px !important;}
        .RadGrid table.rgMasterTable tr.rgRow td {overflow: visible !important;}
        .RadGrid .rgClipCells .rgAltRow td {overflow:visible !important;}
/*        body { font-family: Arial; margin: 20px; }
        h2 { color: #2c3e50; }
        table { border-collapse: collapse; width: 70%; margin-top: 20px; }
        th, td { border: 1px solid #ccc; padding: 8px; text-align: left; }
        th { background: #f4f4f4; }
        .available { color: green; font-weight: bold; }
        .busy { color: red; font-weight: bold; }
        .form-section { margin-bottom: 15px; }*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Roles</h1>
        </div>
    </div>

    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <asp:ValidationSummary ID="vsTicket" runat="server" CssClass="validationSummary" ValidationGroup="vgRoles" />
        <asp:Literal ID="litMessage" runat="server" />
        <div class="block">
            <div class="block-title">
                <h2><strong>Search Criteria</strong></h2>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Start Date</label>
                            </div>                            
                        </div>
                        <div class="col-md-8">
                            <uc:DatePicker ID="ucCreatedStart" runat="server" IsRequired="false" />
                            <asp:TextBox ID="txt1" runat="server" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="form-group form-actions" style="margin-left:8px">
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgRoles"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                </div>
            </div>  
        </div>
    </asp:Panel>

    <div class="block">
        <div class="row">
            <div class="col-md-6">
                <label>24x7 Primary</label>
                <asp:ListBox runat="server" ID="lb24x7Primary" CssClass="form-control select-chosen" Rows="10" SelectionMode="Multiple"></asp:ListBox>
            </div>
            <div class="col-md-6">
                <label>24x7 Secondary</label>
                <asp:ListBox runat="server" ID="lb24x7Secondary" CssClass="form-control select-chosen" Rows="10" SelectionMode="Multiple"></asp:ListBox>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6">
                <label>Scheduling</label>
                <asp:ListBox runat="server" ID="lbScheduling" CssClass="form-control select-chosen" Rows="10" SelectionMode="Multiple"></asp:ListBox>
            </div>
            <div class="col-md-6">
                <label>Ticket Dispatch</label>
                <asp:ListBox runat="server" ID="lbTicketDispatch" CssClass="form-control select-chosen" Rows="10" SelectionMode="Multiple"></asp:ListBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <label>Help Desk</label>
                <asp:ListBox runat="server" ID="lbHelpDesk" CssClass="form-control select-chosen" Rows="10" SelectionMode="Multiple"></asp:ListBox>
            </div>
            <div class="col-md-6">
                <label>Onsite</label>
                <asp:ListBox runat="server" ID="lbOnsite" CssClass="form-control select-chosen" Rows="10" SelectionMode="Multiple"></asp:ListBox>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="form-group form-actions" style="margin-left:8px">
                <asp:LinkButton ID="lbSave" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbSave_Click"><i class="hi hi-ok"></i> Save</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <br />
        </div>
    </div>

    <asp:Panel ID="pnlContainer3" runat="server" Visible="false">
        <%--<ajaxToolkit:CollapsiblePanelExtender ID="cpeSendEmail" 
                    runat="Server"
                    TargetControlID="pnlSearchResultForEmail"
                    ExpandControlID="pnlShowHideSendEmail"
                    CollapseControlID="pnlShowHideSendEmail" 
                    BehaviorID="cpeSendEmail"
                    TextLabelID="lblSendEmailHeader"
                    ImageControlID="imgSendEmail"    
                    ExpandedImage="~/Images/collapse_blue.jpg"
                    CollapsedImage="~/Images/expand_blue.jpg"
                    SuppressPostBack="true"
                    CollapsedText="Send Email [click to display]"
                    ExpandedText="Send Email [click to hide]"
                    Collapsed="true"/>
        <div class="block-title">
            <asp:Panel ID="pnlShowHideSendEmail" runat="server" CssClass="collapsePanelHeaderTicket" Height="35px">
                <div style="float: left; padding-right:60%">
                    <h2><strong>
                        <asp:Label ID="lblSendEmailHeader" runat="server" />
                        </strong>
                    </h2>                            
                </div>
                <div style="float: right; vertical-align: middle; margin:10px 10px 0px 0px">
                    <asp:ImageButton ID="imgSendEmail" runat="server" ImageUrl="~/Images/expand_blue.jpg" />
                </div>
            </asp:Panel>
        </div>--%>
    
        <asp:Panel ID="pnlSearchResultForEmail" style="overflow:visible; height:auto;" runat="server">
            <div class="block" style="padding-bottom: 100px">
                <div class="block-title">
                    <h2><strong>Send Email</strong></h2>
                    <asp:PlaceHolder ID="phScrollNotes" runat="server"></asp:PlaceHolder>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="row topMargin5" style="margin-left:-35px">
                            <div class="col-md-4">
                                <div style="float:right">
                                    <label>Employee</label>
                                </div>                            
                            </div>
                            <div class="col-md-8">
                                <asp:ListBox runat="server" ID="ddlEmailEmployee" CssClass="form-control select-chosen" Rows="10" SelectionMode="Multiple"></asp:ListBox>
<%--                                <asp:DropDownList ID="ddlEmailEmployee" runat="server" />
                                <asp:Literal ID="litJs" runat="server" Visible="true"/>--%>
                            </div>
                        </div>                    
                    </div>
                    <div class="col-md-6">
                        <div class="row topMargin5" style="margin-left:-35px">
                                <div class="col-md-12">
                                    <asp:LinkButton ID="lbSendMail" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbSendMail_Click"><i class="hi hi-search"></i> Send Mail</asp:LinkButton>
                                </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
            
    </asp:Panel>

    <asp:Panel ID="pnlContainer2" runat="server" Visible="false">
        <ajaxToolkit:CollapsiblePanelExtender ID="cpeEmployeeScheduling" 
            runat="Server"
            TargetControlID="phSearchResults"
            ExpandControlID="pnlShowHideEmployeeScheduling"
            CollapseControlID="pnlShowHideEmployeeScheduling" 
            BehaviorID="cpeEmployeeScheduling"
            TextLabelID="lblEmployeeSchedulingHeader"
            ImageControlID="imgEmployeeScheduling"    
            ExpandedImage="~/Images/collapse_blue.jpg"
            CollapsedImage="~/Images/expand_blue.jpg"
            SuppressPostBack="true"
            CollapsedText="Employee Schedule [click to display]"
            ExpandedText="Employee Schedule [click to hide]"
            Collapsed="true"/>
        <div class="block-title">
            <asp:Panel ID="pnlShowHideEmployeeScheduling" runat="server" CssClass="collapsePanelHeaderTicket" Height="35px">
                <div style="float: left; padding-right:60%">
                    <h2><strong>
                        <asp:Label ID="lblEmployeeSchedulingHeader" runat="server" />
                        </strong>
                    </h2>                            
                </div>
                <div style="float: right; vertical-align: middle; margin:10px 10px 0px 0px">
                    <asp:ImageButton ID="imgEmployeeScheduling" runat="server" ImageUrl="~/Images/expand_blue.jpg" />
                </div>
            </asp:Panel>
        </div>
    
        <asp:Panel ID="phSearchResults" runat="server">
            <div class="block" style="padding-bottom: 20px; overflow-x:scroll; overflow:visible">
                <asp:Repeater ID="rptEmployeeScheduling" OnItemDataBound="rptEmployeeScheduling_ItemDataBound" runat="server">
                    <ItemTemplate>
                        <h3><asp:Label ID="lblName" runat="server" /></h3>
                        <asp:HiddenField id="hfEmpId" runat="server" />
                        <asp:HiddenField id="hfEmpEmail" runat="server" />
                        <asp:Repeater ID="rptSchedule" OnItemDataBound="rptSchedule_ItemDataBound" runat="server">
                            <ItemTemplate>
                                <asp:Label ID="lblSchedule" runat="server" /><br />
                            </ItemTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </asp:Panel>
    </asp:Panel>
    

</asp:Content>

