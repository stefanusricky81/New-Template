<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="TimeofDayTicketReport.aspx.cs" Inherits="Reports_TimeofDayTicketReport" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketType" Src="~/UserControl/DropDownList/TicketType.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Time of Day Ticket Report</h1>
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
                <label>Client</label>
                <ddl:Client ID="ddlClient" runat="server" IsRequired="true" DisplayChosenScript="true" ValidationGroup="vgClientTicketReport" />
            </div>
            <div class="form-group col-sm-6">
                <label>Ticket Type</label>
                <ddl:TicketType ID="ddlTicketType" runat="server" DisplayChosenScript="true" />
           </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-6">
                <label>Date Start</label>
                <uc:DatePicker ID="ucCreatedStart" runat="server" IsRequired="false" />
            </div>
            <div class="form-group col-sm-6">
                <label>Date End</label>
                <uc:DatePicker ID="ucCreatedEnd" runat="server" IsRequired="false" />
            </div>
            <asp:CustomValidator ID="cvCreatedDates" runat="server" ValidateEmptyText="true" ValidationGroup="vgClientTicketReport" 
                ErrorMessage="Created End Date must be greater than Created Start Date" Display="None" OnServerValidate="cvCreatedDates_ServerValidate" />
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
                    <asp:LinkButton ID="lbexport" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbexport_Click"><i class="hi hi-export"></i> Export to Excel</asp:LinkButton>
                </div>
            </div>
            <telerik:RadGrid ID="rgTimeofDay" OnNeedDataSource="rgTimeofDay_NeedDataSource" runat="server" Skin="3b" EnableEmbeddedSkins="false" 
                AutoGenerateColumns="false" OnItemDataBound="rgTimeofDay_ItemDataBound" AllowSorting="true" ShowFooter="false" Width="100%" AllowPaging="true" AllowCustomPaging="true"> 
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" Width="100%" ShowFooter="true">
                    <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="10,25,50,100,250" PageSizeControlType="RadComboBox" Position="Bottom" />
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Created date" SortExpression="CREATEDDATE" HeaderStyle-Width="5%" UniqueName="CREATEDDATE" HeaderTooltip="Created Date">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblDate" Text='<% #string.Format("{0:dd-MMM-yyyy}", Eval("CREATEDDATE")) %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label runat="Server" ID="lblTotal" Text="Total :" Font-Bold="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="12AM-8:59AM" SortExpression="12AM"  HeaderStyle-Width="5%" UniqueName="12AM" HeaderTooltip="Ticket created between 12AM-8:59AM">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbl12AM" Text='<% #Eval("12AM") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label runat="Server" ID="lblSum12AM" Font-Bold="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="9AM-4:59PM" SortExpression="9AM"  HeaderStyle-Width="5%" UniqueName="9AM" HeaderTooltip="Ticket created between 9AM-4:59PM">
                            <ItemTemplate>
                              <asp:Label runat="server" ID="lbl9AM" Text='<% #Eval("9AM") %>'></asp:Label>
                            </ItemTemplate>
                             <FooterTemplate>
                                <asp:Label runat="Server" ID="lblSum9AM" Font-Bold="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="5PM-11:59PM" SortExpression="5PM"  HeaderStyle-Width="5%" UniqueName="5PM" HeaderTooltip="Ticket created between 5PM-11:59PM">
                            <ItemTemplate>
                              <asp:Label runat="server" ID="lbl5PM" Text='<% #Eval("5PM") %>'></asp:Label>
                            </ItemTemplate>
                             <FooterTemplate>
                                <asp:Label runat="Server" ID="lblSum5PM" Font-Bold="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </asp:PlaceHolder>
</asp:Content>

