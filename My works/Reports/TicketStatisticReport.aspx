<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="TicketStatisticReport.aspx.cs" Inherits="Reports_TicketStatisticReport" %>



<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <script type="text/javascript">
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
    <asp:Literal ID="litMessage" runat="server" Visible="false" />
    <asp:Panel ID="pnlMain" runat="server">

        <div class="block">
            <div class="block-title">
                <h2><strong>Ticket Statistic Report</strong></h2>
                <asp:ValidationSummary ID="vsBilling" runat="server" ValidationGroup="vgBilling" CssClass="validationSummary" />
            </div>
            <div class="block">
                <div class="row">
                    <div class="col-md-6">
                        <label>Start Date</label>
                        <uc:DatePicker ID="ucDateStart" runat="server" IsRequired="false" />
                    </div>
                    <div class="col-md-6">
                        <label>End Date</label>
                        <uc:DatePicker ID="ucDateEnd" runat="server" IsRequired="false" />
                        <asp:CustomValidator ID="cvTicketdate" runat="server" ValidateEmptyText="true" ValidationGroup="VgTicket"
                            ErrorMessage="Date end must be greater than Start Date" Display="None" OnServerValidate="cvTicketdate_ServerValidate" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group form-actions" style="margin-left: 8px">
                        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="VgTicket"><i class="hi hi-search"></i> Search</asp:LinkButton>
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="block" style="padding-bottom: 20px;">
                <div class="block-title">
                    <h2><strong></strong></h2>
                </div>
                
                <telerik:RadGrid ID="rgTicket" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true"
                    AllowPaging="true" ShowFooter="false" PageSize="25" Width="100%" OnNeedDataSource="rgTicket_NeedDataSource">
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" Width="100%" AllowSorting="true">
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="TicketDate" DataType="System.DateTime" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%" HeaderText="Ticket Date" SortExpression="TicketDate" UniqueName="TicketDate" />
                            <telerik:GridBoundColumn DataField="ActionType" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%" HeaderStyle-Wrap="false" HeaderText="Action Type" SortExpression="ActionType" UniqueName="ActionType" />
                            <telerik:GridBoundColumn DataField="Total" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%" HeaderStyle-Wrap="false" HeaderText="Total" SortExpression="Total" UniqueName="Total" />


                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" />
                </telerik:RadGrid>
            </div>
        </div>
    </asp:Panel>

    
</asp:Content>

