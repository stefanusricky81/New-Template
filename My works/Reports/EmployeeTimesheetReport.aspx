<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="EmployeeTimesheetReport.aspx.cs" Inherits="Reports_EmployeeTimesheetReport" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Employee Timesheet Report</h1>
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
                <label>Client</label>
                <asp:DropDownList ID="ddlClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClient_SelectedIndexChanged" />
                <asp:Literal ID="litJs" runat="server" Visible="false"/>
            </div>
            <div class="form-group col-sm-6">
                <label>Project</label>
                <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control select-chosen" />
            </div>
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
            <telerik:RadGrid ID="rgTimesheetbyemployee" OnNeedDataSource="rgTimesheetbyemployee_NeedDataSource" OnItemDataBound="rgTimesheetbyemployee_ItemDataBound"
                     runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" MasterTableView-ShowFooter="true"
                    ShowFooter="false" Width="100%"> 
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="FK_EMPLOYEE" Width="100%" AllowCustomSorting="false">
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="FK_EMPLOYEE" HeaderStyle-Width="5%" DataType="System.Int16" ItemStyle-HorizontalAlign="Left" HeaderText="#" UniqueName="EmployeeId" Display="false" />
                            <telerik:GridTemplateColumn UniqueName="EmpName" HeaderText="Name" HeaderStyle-Width="9%" SortExpression="FNAME">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" Text='<%# String.Format("{0} {1}",Eval("FNAME"), Eval("LNAME"))%>' runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Dept" HeaderStyle-Width="7%" DataType="System.Int16" ItemStyle-HorizontalAlign="Left" HeaderText="Dept" SortExpression="Dept" UniqueName="Dept" HeaderTooltip="Department" />
                            <telerik:GridTemplateColumn UniqueName="ClientHours" HeaderStyle-Width="7%" HeaderText="Client Hours" SortExpression="BILLABLEHRS">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalClientHours" Text='<%# String.Format("{0}",Eval("BILLABLEHRS")) %>' runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="BBHours" HeaderStyle-Width="7%" HeaderText="BBB Hours" SortExpression="BBHours">
                                <ItemTemplate>
                                    <asp:Label ID="lblBBBHours" Text='<%# String.Format("{0}",Eval("BBHours")) %>' runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="HOURS" HeaderStyle-Width="7%" HeaderText="Total Hours" SortExpression="HOURS">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalHours" Text='<%# String.Format("{0}",Eval("HOURS")) %>' runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn UniqueName="Clientper" HeaderStyle-Width="7%" HeaderText="Client%" SortExpression="Clientper">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalClientper" Text='<%# String.Format("{0:0} %", Eval("Clientper")) %>' runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>



<%--                            <telerik:GridTemplateColumn UniqueName="BILLABLEperHRS" HeaderStyle-Width="7%" HeaderText="Bill per hours" SortExpression="BILLABLEperHRS">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalBillperHours" Text='<%# String.Format("$ {0:0.00}", (Eval("BILLABLEperHRS"))) %>' runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn UniqueName="BILLABLEperClientHRS" HeaderStyle-Width="7%" HeaderText="Bill per Client hours" SortExpression="BILLABLEperClientHRS">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalBillperClientHours" Text='<%# String.Format("$ {0:0.00}", (Eval("BILLABLEperClientHRS"))) %>' runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>

                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
        </div>
    </asp:PlaceHolder>
</asp:Content>