<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="GetJobNumberResult.aspx.cs" Inherits="Client_GetJobNumberResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Get Job Number</h1>
        </div>
    </div>

    <telerik:RadAjaxPanel ID="rapUser" runat="server" LoadingPanelID="ralpUser" ClientEvents-OnRequestStart="conditionalPostback">
        <asp:Literal ID="litMessage" runat="server" Visible="false" />
        <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
            <div class="block">
                <div class="row">
                    <h4>Here is your job number detail :</h4>
                </div><br />
                <div class="row">
                    <telerik:RadGrid ID="rgJobNumber" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                        AllowPaging="true" ShowFooter="false" PageSize="5" Width="100%" OnNeedDataSource="rgJobNumber_NeedDataSource" >
                        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" Width="100%" AllowSorting="true">
                            <NoRecordsTemplate>
                                <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                            </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="pbbbjob" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Job #" SortExpression="pbbbjob" UniqueName="pbbbjob" />
                                <telerik:GridBoundColumn DataField="company" HeaderStyle-Width="60%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Client name" SortExpression="company" UniqueName="company" />
                                <telerik:GridBoundColumn DataField="empl_code" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Employee code" SortExpression="empl_code" UniqueName="empl_code" />
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" />
                    </telerik:RadGrid>
                </div><br />
                <div class="form-group form-actions">
                    <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-sm btn-primary" ValidationGroup="vgSearch"><i class="hi hi-search"></i> Get Another Job Number</asp:LinkButton>
                </div>
            </div>

        </asp:Panel>
    </telerik:RadAjaxPanel>
</asp:Content>


