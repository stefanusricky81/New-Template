<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="EmployeeTimesheetMonthlyReport.aspx.cs" Inherits="Reports_EmployeeTimesheetMonthlyReport" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Employee Timesheet Monthly Report</h1>
        </div>
    </div>
    <asp:ValidationSummary ID="vsEmployeeTimeSheetMonthly" runat="server" CssClass="validationSummary" ValidationGroup="vgEmployeeTimeSheetMonthly" />
    <asp:CustomValidator ID="cvEmployeeTimeSheetMonthly" runat="server" ValidateEmptyText="true" ValidationGroup="vgEmployeeTimeSheetMonthly" Display="None" 
            ErrorMessage="Created End Date must be greater than Created Start Date" />
    <asp:Literal ID="litMessage" runat="server" />
    <div class="block">
        <div class="block-title">
            <h2><strong>Search Criteria</strong></h2>
        </div>
        <div class="row">
            <div class="form-group col-sm-6">
                <label>Date range</label>
                <div class="row">
                    <div class="form-group col-sm-6">
                        <uc:DatePicker ID="ucCreatedStart" runat="server" IsRequired="true" ErrorMessage="Start date is required" ValidationGroup="vgEmployeeTimeSheetMonthly" />
                    </div>
                    <div class="form-group col-sm-6">
                        <uc:DatePicker ID="ucCreatedEnd" runat="server" IsRequired="true" ErrorMessage="End date is required" ValidationGroup="vgEmployeeTimeSheetMonthly" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-group form-actions" style="padding-top:20px;">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgEmployeeTimeSheetMonthly"><i class="hi hi-search"></i> Search</asp:LinkButton>
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
                <telerik:RadPivotGrid RenderMode="Classic" AllowFiltering="false" AllowPaging="true" EnableEmbeddedSkins="true" AutoGenerateColumns="false"
                    ShowFilterHeaderZone="false" ShowDataHeaderZone="false" ShowColumnHeaderZone="false"
                    ID="rpgTimesheetMonthly" runat="server" EmptyValue="0" OnNeedDataSource="rpgTimesheetMonthly_NeedDataSource" ShowFooter="false" PageSize="20" Width="100%"> 
                   
                    <Fields>
                        <telerik:PivotGridRowField DataField="EmpNAME" Caption="Name" UniqueName="EmpNAME" ZoneIndex="1" CellStyle-BorderStyle="Solid" CellStyle-Width="7%">
                        </telerik:PivotGridRowField>
                        <telerik:PivotGridRowField DataField="dept" Caption="Dept" UniqueName="dept" ZoneIndex="0" CellStyle-BorderStyle="Solid" CellStyle-Width="7%">
                        </telerik:PivotGridRowField>
                        <telerik:PivotGridColumnField DataField="timesheetdate" UniqueName="timesheetdate" CellStyle-BorderStyle="Solid" Caption="Date" DataFormatString="{0:MM/dd/yyyy}">
                        </telerik:PivotGridColumnField>
                        <telerik:PivotGridAggregateField DataField="timesheethours" UniqueName="timesheethours" CellStyle-BorderStyle="Solid" Caption="Hours" Aggregate="Sum">
                        </telerik:PivotGridAggregateField>
                    </Fields>
                </telerik:RadPivotGrid>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>

