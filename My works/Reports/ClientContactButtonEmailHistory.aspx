<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientContactButtonEmailHistory.aspx.cs" Inherits="Reports_ClientContactButtonEmailHistory" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Client Contact Button Email History</h1>
        </div>
    </div>
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <div class="block">
            <div class="row">
                <div class="col-md-6">
                    <label>Client</label>
                    <ddl:Client ID="ddlClient" runat="server" IsRequired="false" DisplayChosenScript="true" />
                </div>
                <div class="col-md-6">
                    <label>Email</label>
                    <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <label>Date Start</label>
                    <uc:DatePicker ID="ucDateStart" runat="server" IsRequired="false" />
                </div>
                <div class="col-md-6">
                    <label>Date End</label>
                    <uc:DatePicker ID="ucDateEnd" runat="server" IsRequired="false" />
                    <asp:CustomValidator ID="cvDate" runat="server" ValidateEmptyText="true" ValidationGroup="VgClientButtonHistory" 
                            ErrorMessage="Date end must be greater than start Date" Display="None" OnServerValidate="cvDate_ServerValidate" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <label>Action</label>
                    <asp:DropDownList ID="ddlActions" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="row">
                <div class="form-group form-actions" style="margin-left:8px">
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="VgClientButtonHistory"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                </div>
            </div> 
        </div>
        <div class="block">
             <div class="row">
                 <telerik:RadGrid ID="rgButtonHistory" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                        OnNeedDataSource="rgButtonHistory_NeedDataSource" 
                        AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" ShowFooter="false" PageSize="25" Width="100%">
                        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="pclientcontact" Width="100%">
                            <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="10,25,50,100,250" PageSizeControlType="RadComboBox" Position="Bottom" />
                            <NoRecordsTemplate>
                                <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                            </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="UserPriority" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Right" HeaderText="Pr #" SortExpression="UserPriority" UniqueName="UserPriority" />
                                <telerik:GridBoundColumn DataField="company" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Client" SortExpression="company" UniqueName="company" />
                                <telerik:GridBoundColumn DataField="Last" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Last" SortExpression="Last" UniqueName="LastName" />
                                <telerik:GridBoundColumn DataField="First" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="First" SortExpression="First" UniqueName="First" />
                                <telerik:GridBoundColumn DataField="Email" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email" SortExpression="Email" UniqueName="Email" />
                                <telerik:GridBoundColumn DataField="ContactBusPhone" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Phone" SortExpression="ContactBusPhone" UniqueName="ContactBusPhone" />
                                <telerik:GridBoundColumn DataField="ContactCellPhone" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Cell" SortExpression="ContactCellPhone" UniqueName="ContactCellPhone" />
                                <telerik:GridBoundColumn DataField="LocationName" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Location" SortExpression="LocationName" UniqueName="LocationName" Visible="false" />
                                <telerik:GridBoundColumn DataField="ContactActive" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Active" SortExpression="ContactActive" UniqueName="ContactActive" />
                                <telerik:GridBoundColumn DataField="CategoryEmail" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email Category" SortExpression="CategoryEmail" UniqueName="CategoryEmail" />
                                <telerik:GridBoundColumn DataField="buttonName" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Action" SortExpression="buttonName" UniqueName="buttonName" />
                                <telerik:GridBoundColumn DataField="VIP" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="VIP" SortExpression="VIP" UniqueName="VIP" />
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" />
                    </telerik:RadGrid>
             </div>
        </div>
    </asp:Panel>
</asp:Content>