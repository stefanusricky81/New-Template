<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="QuarterlyClientTicketReport.aspx.cs" Inherits="Reports_QuarterlyClientTicketReport" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketType" Src="~/UserControl/ListBox/MultiTicketType.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
        <style> 
            div.scroll { 
                overflow-x: auto; 
                overflow-y: hidden; 
                white-space: nowrap; 
            } 
        </style> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Ticket Transaction Report</h1>
        </div>
    </div>
    <asp:ValidationSummary ID="vsClientReportTicket" runat="server" CssClass="validationSummary" ValidationGroup="vgClientTicketReport" />
    <asp:Literal ID="litMessage" runat="server" />
    <div class="block">
        <div class="block-title">
            <h2><strong>Search Criteria</strong></h2>
        </div>
        <div class="row">
            <div class="form-group col-sm-6">
                <label>End Month</label>
                <asp:DropDownList ID="ddlMonths" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group col-sm-6">
                <label>Client</label>
                <ddl:Client ID="ddlClient" runat="server" IsRequired="false" DisplayChosenScript="true" />
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-6">
                <label>Number of Months</label>
                <asp:DropDownList ID="ddlNoM" runat="server" CssClass="form-control">
                    <asp:ListItem Text="1" Value="1" />
                    <asp:ListItem Text="2" Value="2" />
                    <asp:ListItem Text="3" Value="3"  Selected="True"/>
                    <asp:ListItem Text="4" Value="4" />
                    <asp:ListItem Text="5" Value="5" />
                    <asp:ListItem Text="6" Value="6" />
                    <asp:ListItem Text="7" Value="7" />
                    <asp:ListItem Text="8" Value="8" />
                    <asp:ListItem Text="9" Value="9" />
                    <asp:ListItem Text="10" Value="10" />
                    <asp:ListItem Text="11" Value="11" />
                    <asp:ListItem Text="12" Value="12" />
                </asp:DropDownList>
            </div>
            <div class="form-group col-sm-6">
                <label>Sort Month</label>
                <asp:DropDownList ID="ddlSort" runat="server" CssClass="form-control" >
                    <asp:ListItem Value="asc" Text="Ascending" Selected="True" />
                    <asp:ListItem Value="desc" Text="Descending" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-6">
                <label>Ticket Type</label>
                <ddl:TicketType ID="ddlTicketType" runat="server" DisplayChosenScript="true" ValidationGroup="vgClientTicketReport" />
            </div>
            <div class="form-group col-sm-6"></div>
        </div>
        <div class="row">
            <div class="form-group form-actions" style="padding-top:20px;">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgClientTicketReport"><i class="hi hi-search"></i> Search</asp:LinkButton>
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
                    <asp:LinkButton ID="lbexport" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbexport_Click"><i class="hi hi-export"></i> Export to .csv</asp:LinkButton>
                </div>
            </div>
            <telerik:RadPivotGrid RenderMode="Lightweight" ID="rpgExport" runat="server" OnNeedDataSource="rpgExport_NeedDataSource" ShowFilterHeaderZone="false" AllowPaging="true" EmptyValue="0" 
                AllowFiltering="false" ShowColumnHeaderZone="false" ShowDataHeaderZone="false" RowTableLayout="Compact" PageSize="100" OnSorting="rpgExport_Sorting"
                TotalsSettings-RowGrandTotalsPosition="None" TotalsSettings-ColumnGrandTotalsPosition="None" TotalsSettings-RowsSubTotalsPosition="None" TotalsSettings-ColumnsSubTotalsPosition="None"
                ColumnGroupsDefaultExpanded="true">
                <Fields>
                    <telerik:PivotGridColumnField DataField="years" UniqueName="years" Caption="Year" ></telerik:PivotGridColumnField>
                    <telerik:PivotGridColumnField DataField="monthno" UniqueName="monthno" Caption="Months" >
                        <CellTemplate>
                            <asp:Label ID="lblMonths" Text='<%# GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "1" ? "January" : 
                                                                GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "2" ? "February" :  
                                                                GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "3" ? "March" :
                                                                GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "4" ? "April" :
                                                                GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "5" ? "May" :
                                                                GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "6" ? "June" :
                                                                GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "7" ? "July" :
                                                                GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "8" ? "August" :
                                                                GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "9" ? "September" :
                                                                GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "10" ? "October" :
                                                                GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "11" ? "November" :
                                                                GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "12" ? "December" :""
                                                               %>'
                                runat="server" />
                        </CellTemplate>
                    </telerik:PivotGridColumnField>
                    <telerik:PivotGridRowField DataField="OrderID1" UniqueName="OrderID1" Caption="Header" >
                        <CellTemplate>
                           <asp:Label ID="lblTotal" Text='<%# GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "1" ? "Total Helpdesk Request" : 
                                                               GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "4" ? "VIP Request" :  
                                                               GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "3" ? "Response Times (minutes)" :
                                                               GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "2" ? "Resolution" :
                                                               GetDataItem().ToString().Split(new string[] { " " }, StringSplitOptions.None)[0] == "5" ? "User Ticket Category" : ""%>'
                                runat="server" />
                        </CellTemplate>
                    </telerik:PivotGridRowField>
                    <telerik:PivotGridRowField DataField="Detail" UniqueName="Detail" Caption="Detail" />
                    <telerik:PivotGridAggregateField DataField="Critical" Aggregate="Sum" UniqueName="Critical" Caption="Critical" >
                         <HeaderCellTemplate>
                            <asp:Label ID="AggregateCell1" Text="Critical" runat="server" />
                        </HeaderCellTemplate>
                    </telerik:PivotGridAggregateField>
                    <telerik:PivotGridAggregateField DataField="High" Aggregate="Sum" Caption="High" UniqueName="High" >
                         <HeaderCellTemplate>
                            <asp:Label ID="AggregateCell1" Text="High" runat="server" />
                        </HeaderCellTemplate>
                    </telerik:PivotGridAggregateField>
                    <telerik:PivotGridAggregateField DataField="Normal" Aggregate="Sum" UniqueName="Normal" >
                        <HeaderCellTemplate>
                            <asp:Label ID="AggregateCell1" Text="Normal" runat="server" />
                        </HeaderCellTemplate>
                    </telerik:PivotGridAggregateField>
                    <telerik:PivotGridAggregateField DataField="Low" Aggregate="Sum" UniqueName="Low">
                        <HeaderCellTemplate>
                            <asp:Label ID="AggregateCell1" Text="Low" runat="server" />
                        </HeaderCellTemplate>
                    </telerik:PivotGridAggregateField>
                </Fields>
            </telerik:RadPivotGrid>
        </div>

    </asp:PlaceHolder>
</asp:Content>

