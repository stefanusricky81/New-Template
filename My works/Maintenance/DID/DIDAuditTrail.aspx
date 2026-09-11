<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="DIDAuditTrail.aspx.cs" Inherits="Maintenance_DID_DIDAuditTrail" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>DID Audit Trails</h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" Visible="false" />
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSearch">
         <div class="block">
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>DID number</label>
                    <asp:TextBox ID="txtDIDNumber" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter location name" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Trunk group</label>
                    <asp:DropDownList ID="ddlTrunkGroup" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Assigned</label>
                    <asp:DropDownList ID="ddlAssigned" CssClass="form-control" runat="server">
                        <asp:ListItem Text="Select" Value="" />
                        <asp:ListItem Text="Yes" Value="Y" />
                        <asp:ListItem Text="No" Value="N" />
                    </asp:DropDownList>
                </div>
            </div>
           <div class="row">
                <div class="form-group col-sm-4">
                    <label>Client</label>
                    <ddl:Client ID="ddlClient" runat="server" IsRequired="false" DisplayChosenScript="true" />
                </div>
                <div class="form-group col-sm-4">
                    <label>User</label>
                    <asp:DropDownList ID="ddlUser" CssClass="form-control" runat="server" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Audit Type</label>
                    <asp:DropDownList ID="ddlAuditType" CssClass="form-control" runat="server" />
                </div>
            </div>
             <div class="row">
                <div class="form-group col-sm-4">
                    <label>Modified Date Start</label>
                    <uc:DatePicker ID="ucModifStart" runat="server" IsRequired="false" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Modified Date End</label>
                    <uc:DatePicker ID="ucModifEnd" runat="server" IsRequired="false" />
                    <asp:CustomValidator ID="cvModifDates" runat="server" ControlToValidate="txtDIDNumber" ValidateEmptyText="true" ValidationGroup="VgAudit" 
                        ErrorMessage="Modif Date End Date must be greater than Created Start Date" Display="None" />
                </div>
            </div>
            <div class="row">
                <div style="float:right;">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSearch_Click" ValidationGroup="VgAudit"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="lbClear" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbClear_Click" ValidationGroup="VgAudit"><i class="hi hi-remove"></i> Clear</asp:LinkButton>
                </div>
            </div>
             <br />
         </div>
         
         <div class="block">
            <div class="row">
                <telerik:RadGrid ID="rgDIDAuditTrail" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="50" Width="100%" OnNeedDataSource="rgDIDAuditTrail_NeedDataSource">
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" 
                        Width="100%" AllowSorting="true">
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="DIDAuditTrailID" HeaderText="ID" SortExpression="DIDAuditTrailID" UniqueName="DIDAuditTrailID" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="DIDNumber" HeaderText="DID Number" SortExpression="DIDNumber" UniqueName="DIDNumber" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="TrunkGroup" HeaderText="Trunk Group" SortExpression="TrunkGroup" UniqueName="TrunkGroup" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="Assigned" HeaderText="Assigned" SortExpression="Assigned" UniqueName="Assigned" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="ClientCode" Display="false" HeaderText="Client" SortExpression="ClientCode" UniqueName="ClientCode" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="Company" HeaderText="Client" SortExpression="Company" UniqueName="Company" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="Name" HeaderText="Modified By" SortExpression="Name" UniqueName="Name" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="AuditType" HeaderText="Action" SortExpression="AuditType" UniqueName="AuditType" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="ModifiedDate" HeaderText="Modified Date" SortExpression="ModifiedDate" UniqueName="ModifiedDate" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" />
                </telerik:RadGrid>
            </div>
         </div>
     </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" Runat="Server">
</asp:Content>

