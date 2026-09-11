<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="CategoryBulkUpdate.aspx.cs" Inherits="Maintenance_CategoryBulkUpdate" %>
<%@ Register TagPrefix="ddl" TagName="TicketStatus" Src="~/UserControl/DropDownList/TicketStatusEnumResponsive.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketDisposition" Src="~/UserControl/DropDownList/TicketDispositionResponsive.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketDesignation" Src="~/UserControl/DropDownList/NewTicketDesignation.ascx"%>
<%@ Register TagPrefix="ddl" TagName="Employee" Src="~/UserControl/DropDownList/Employee.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketType" Src="~/UserControl/DropDownList/TicketType.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketCategory" Src="~/UserControl/DropDownList/TicketCategory.ascx"%>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script type="text/javascript">
    function CheckAll(id) {
        var masterTable = $find("<%= rgTicket.ClientID %>").get_masterTableView();
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
        var masterTable = $find("<%= rgTicket.ClientID %>").get_masterTableView();
        //accessing header checkbox
        var chkBox = $('input[id$="checkAll"]');
        chkBox[0].checked = false;
    }
</script>

    <style>
        .RadGrid_3b .RadComboBox .rcbInput {width:50px !important;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Category Bulk Update</h1>
        </div>
    </div>
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <asp:ValidationSummary ID="vsBulkCategory" runat="server" CssClass="validationSummary" ValidationGroup="vgBulkCategory" />
        <asp:CustomValidator ID="cvCriteriaEntered" runat="server" ControlToValidate="txtTicketNumber" ValidateEmptyText="true" ValidationGroup="vgBulkCategory" Display="None" 
            ErrorMessage="Minimum of 1 search field is required" OnServerValidate="cvCriteriaEntered_ServerValidate" />
        <div class="block">
            <div class="block-title">
                <h2><strong>Search Criteria</strong></h2>
            </div>
            <div class="row">
                <div class="form-group col-sm-6">
                    <label>Client</label>
                    <ddl:Client ID="ddlClient" runat="server" IsRequired="false" DisplayChosenScript="true" />
                </div>
                <div class="form-group col-sm-6">
                    <label>Status</label>
                    <ddl:TicketStatus ID="ddlTicketStatus" runat="server" IsRequired="false" DisplayChosenScript="true" />
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-6">
                    <label>Created : Date range</label>
                    <div class="row">
                        <div class="form-group col-sm-6">
                            <uc:DatePicker ID="ucCreatedStart" runat="server" IsRequired="false" />
                        </div>
                        <div class="form-group col-sm-6">
                            <uc:DatePicker ID="ucCreatedEnd" runat="server" IsRequired="false" />
                        </div>
                    </div>
                    <asp:CustomValidator ID="cvCreatedDates" runat="server" ControlToValidate="txtTicketNumber" ValidateEmptyText="true" ValidationGroup="vgBulkCategory" 
                        ErrorMessage="Created End Date must be greater than Created Start Date" Display="None" OnServerValidate="cvCreatedDates_ServerValidate" />
                </div>
                <div class="form-group col-sm-6">
                    <label>Disposition</label>
                    <ddl:TicketDisposition ID="ddlTicketDisposition" runat="server" IsRequired="false" DisplayChosenScript="true" />
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-6">
                    <label>Ticket #</label>
                    <asp:TextBox ID="txtTicketNumber" runat="server" MaxLength="10" CssClass="form-control" PlaceHolder="Enter Ticket #" />
                    <asp:RangeValidator ID="rvTicketNumber" runat="server" ControlToValidate="txtTicketNumber" Type="Integer" MinimumValue="1" MaximumValue="1000000" ErrorMessage="Invalid Ticket #" ValidationGroup="vgBulkCategory" Display="None" />
                </div>
                <div class="form-group col-sm-6">
                    <label>Ticket Designation</label>
                    <ddl:TicketDesignation ID="ddlTicketDesignationDDL" runat="server" />
                 </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-6">
                    <label>Description</label>
                    <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter Description" />
                </div>
                <div class="form-group col-sm-6">
                    <label>Type</label>
                    <ddl:TicketType ID="ddlTicketType" runat="server" IsRequired="false" DisplayChosenScript="true" DefaultValue="" DefaultText="" DisplayDefaultValue="true" SetSize="false" />
                </div>
            </div>
            <div class="row">
                 <div class="form-group col-sm-6">
                    <label>Assigned To</label>
                    <ddl:Employee ID="ddlEmployeeAssignedTo" runat="server" DisplayChosenScript="true" DefaultValue="" SetSize="false" />
                 </div>
                <div class="form-group col-sm-6">
                    <label>Category</label>
                    <ddl:TicketCategory ID="ddlTicketCategory" runat="server" DisplayChosenScript="true" />
                 </div>
            </div>
            <div class="row">
                 <div class="form-group col-sm-6">
                    <label>Uncategorized Only</label>
                    <asp:CheckBox ID="cbUncategories" runat="server" />
                 </div>
            </div>
            <div class="row">
                <div class="form-group form-actions" style="padding-top:20px;">
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgBulkCategory"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="false">
            <asp:Literal ID="litDebug" runat="server" />
            <asp:Literal ID="litMessage" runat="server" />
            <div class="block" style="padding-bottom: 20px;">
                <div class="block-title">
                    <h2><strong>Search Results</strong></h2>
                    <asp:PlaceHolder ID="phScrollNotes" runat="server"></asp:PlaceHolder>
                </div>
                <div class="row">
                    <div class="form-group col-sm-11">
                            Select checkboxes for each ticket and assign ticket category and ticket type from dropdowns below:
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-11">
                        <div class="row">
                            <div class="form-group col-sm-3">
                                <ddl:TicketCategory ID="ddlTicketCategoryUpdateTop" runat="server" DisplayChosenScript="true" />
                            </div>
                            <div class="form-group col-sm-3">
                                <ddl:TicketType runat="server" DisplayChosenScript="true"  ID="ddlTicketTypeTop"/>
                            </div>
                            <div class="form-group col-sm-5">
                                <asp:button id="btnUpdateAssignedTop" text="Update Category" runat="server" OnCommand="Decision_Command" CssClass="btn btn-sm btn-primary" ValidationGroup="vgKB" CommandName="Assign" />&nbsp;&nbsp;
                                 <asp:button id="btnAssignTicketTypeTop" text="Update Type" runat="server" OnCommand="Decision_Command" CssClass="btn btn-sm btn-primary" ValidationGroup="vgKB" CommandName="AssignType" />&nbsp;&nbsp;
                                <asp:button id="btnAssignBothTop" text="Update Category and Type" runat="server" CssClass="btn btn-sm btn-primary" OnCommand="Decision_Command" ValidationGroup="vgKB" CommandName="AssignBoth" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-11">
                        <div class="row">
                            <div class="form-group col-sm-8">
                                Update category dropdown for individual tickets from dropdown in the list
                            </div>
                            <div class="form-group col-sm-3">
                                 <asp:button id="btnUpdateTop" text="Bulk Update All Categories" runat="server" CssClass="btn btn-sm btn-primary" OnCommand="Decision_Command" ValidationGroup="vgKB" CommandName="Update" />
                            </div>  
                        </div>
                    </div>
                </div>
            </div>

            <telerik:RadGrid ID="rgTicket" OnNeedDataSource="rgTicket_NeedDataSource" OnItemDataBound="rgTicket_ItemDataBound"
                     runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="20" Width="100%"> 
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="TicketID" Width="100%">
                        <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="10,25,50,100,250,500" PageSizeControlType="RadComboBox" Position="Bottom" />
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
                            <telerik:GridBoundColumn DataField="TicketID" HeaderStyle-Width="6%" DataType="System.Int16" ItemStyle-HorizontalAlign="Left" HeaderText="#" UniqueName="Id" />
                            <telerik:GridBoundColumn DataField="Summary" HeaderStyle-Width="19%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Summary" SortExpression="Summary" UniqueName="Summary" HeaderTooltip="Ticket Summary" />
                            <telerik:GridBoundColumn DataField="Description" HeaderStyle-Width="30%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Description" SortExpression="Description" UniqueName="Description" HeaderTooltip="Description" />
                            <telerik:GridBoundColumn DataField="CategoryName" Display="false" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Category" SortExpression="Category" UniqueName="Category" HeaderTooltip="Category" />
                            <telerik:GridBoundColumn DataField="CategoryID" Display="false" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Category Id" SortExpression="CategoryID" UniqueName="CategoryID" HeaderTooltip="Category Id" />
                            <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="12%" ItemStyle-Height="10px" DataType="System.String" ItemStyle-HorizontalAlign="Left" >
                                <EditItemTemplate>
                                    <ddl:TicketCategory ID="ddlCategoryUpdateGrid" runat="server" DisplayChosenScript="true" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlCategoryUpdate" runat="server" />
                                    <%--<asp:Label ID="lblCategory" runat="server" Text='<%# Eval("CategoryName") %>' />--%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="TICKETTYPEID" Display="false" DataType="System.String" ItemStyle-HorizontalAlign="Left" UniqueName="TICKETTYPEID"/>
                            <telerik:GridTemplateColumn HeaderText="Ticket Type" HeaderStyle-Width="12%" ItemStyle-Height="10px" DataType="System.String" ItemStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTicketTypeUpdate" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Status" HeaderStyle-Width="6%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Status" SortExpression="StatusName" UniqueName="StatusName" HeaderTooltip="Ticket Status" />
                            <telerik:GridBoundColumn DataField="TicketTypeName" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Type" SortExpression="TicketTypeName" UniqueName="TicketTypeName" HeaderTooltip="Ticket Type" />
                            <telerik:GridBoundColumn DataField="DateEntered" HeaderStyle-Width="8%" DataType="System.DateTime" ItemStyle-HorizontalAlign="Left" HeaderText="Entered" SortExpression="DateEntered" UniqueName="DateEntered" HeaderTooltip="Date Entered" DataFormatString="{0:MM/dd/yyyy}" />
                            <telerik:GridBoundColumn DataField="AssignedToUser" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Assigned" SortExpression="AssignedToUser" UniqueName="AssignedToUserName" HeaderTooltip="Assigned To" />
                            <telerik:GridBoundColumn DataField="AssignedToUser2" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Assigned" SortExpression="AssignedToUserName2" UniqueName="AssignedToUserName2" HeaderTooltip="Assigned To" Visible="false" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            <br />
            <div class="row">
                    <div class="form-group col-sm-9"></div>
                    <div class="form-group col-sm-3">
                        <asp:button id="btnUpdateBottom" text="Bulk Update All Categories" runat="server" CssClass="btn btn-sm btn-primary" OnCommand="Decision_Command" ValidationGroup="vgKB" CommandName="Update" />
                    </div>
                </div>
        </asp:PlaceHolder>
    </asp:Panel>
</asp:Content>

