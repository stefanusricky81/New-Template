<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="DepartmentTimesheetReport.aspx.cs" Inherits="Reports_DepartmentTimesheetReport" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Department Timesheet Report</h1>
        </div>
    </div>
    <asp:ValidationSummary ID="vsEmployeeTimeSheet" runat="server" CssClass="validationSummary" ValidationGroup="vgReport" />
    <asp:CustomValidator ID="cvEmployeeTimeSheet" runat="server" ValidateEmptyText="true" ValidationGroup="vgReport" Display="None" 
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
                        <uc:DatePicker ID="ucCreatedStart" runat="server" IsRequired="true" ErrorMessage="Start date is required" ValidationGroup="vgReport" />
                    </div>
                    <div class="form-group col-sm-6">
                        <uc:DatePicker ID="ucCreatedEnd" runat="server" IsRequired="true" ErrorMessage="End date is required" ValidationGroup="vgReport" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-group form-actions" style="padding-top:20px;">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgReport"><i class="hi hi-search"></i> Search</asp:LinkButton>
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
            <telerik:RadGrid ID="rgTimesheetbydepartment" OnNeedDataSource="rgTimesheetbydepartment_NeedDataSource" OnItemDataBound="rgTimesheetbydepartment_ItemDataBound"
                     runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    ShowFooter="true" Width="100%"> 
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%">
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="id" HeaderStyle-Width="5%" DataType="System.Int16" ItemStyle-HorizontalAlign="Left" HeaderText="#" UniqueName="deptid" Display="false" />
                            <telerik:GridBoundColumn DataField="Dept" HeaderStyle-Width="6%" DataType="System.Int16" ItemStyle-HorizontalAlign="Left" HeaderText="Dept" UniqueName="DeptName" />
                            <telerik:GridTemplateColumn UniqueName="ClientHours" HeaderStyle-Width="7%" HeaderText="Client Hours" SortExpression="BILLABLEHRS">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalClientHours" Text='<%# String.Format("{0}",Eval("BILLABLEHRS")) %>' runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="BBBHours" HeaderStyle-Width="7%" HeaderText="BBB Hours" SortExpression="BBBHours">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalBBBHours" Text='<%# String.Format("{0}",Eval("BBBHours")) %>' runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn UniqueName="HOURS" HeaderStyle-Width="7%" HeaderText="Total Hours" SortExpression="HOURS">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalHours" Text='<%# String.Format("{0}",Eval("HOURS")) %>' runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn UniqueName="Clientper" HeaderStyle-Width="7%" HeaderText="Client%" SortExpression="Clientper">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalClientper" Text='<%# String.Format("{0:0} %",Eval("Clientper")) %>' runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>


<%--                            <telerik:GridTemplateColumn UniqueName="BILLABLEperHRS" HeaderStyle-Width="7%" HeaderText="Bill per hours" SortExpression="BILLABLEperHRS">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalBillperHours" Text='<%# String.Format("$ {0:0.00}",Eval("BILLABLEperHRS")) %>' runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn UniqueName="BILLABLEperClientHRS" HeaderStyle-Width="7%" HeaderText="Bill per Client hours" SortExpression="BILLABLEperClientHRS">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalBillperClientHours" Text='<%# String.Format("$ {0:0.00}",Eval("BILLABLEperClientHRS"))%>' runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
        </div>
    </asp:PlaceHolder>
</asp:Content>