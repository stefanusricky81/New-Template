<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="Backup.aspx.cs" Inherits="Reports_Backup" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
<div class="content-header">
    <div class="header-section">
        <h1>Client Backup Summary Report</h1>
    </div>
</div>
<telerik:RadAjaxPanel ID="rapTicket" runat="server" LoadingPanelID="ralpTicket" EnableAJAX="true">
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <div class="block">
            <div class="block-title">
                <h2><strong>Search Criteria</strong></h2>
            </div>
            <div class="row">
                  <div class="form-group col-sm-4">
                    <label>Client</label>
                    <ddl:Client ID="ddlClient" runat="server" IsRequired="false" DisplayChosenScript="true" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Protected Machine</label>
                    <asp:TextBox ID="txtProtectedMachine" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter Protected Machine" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Backup Server</label>
                    <asp:TextBox ID="txtBackupServer" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter Backup Server" />
                </div>
              
            </div>
            <div class="row">
                 <div class="form-group col-sm-4">
                    <label>Backup Set</label>
                    <asp:TextBox ID="txtBackupSet" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter Backup Set" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Status</label>
                     <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control select-chosen">
                        <asp:ListItem Text="All" Value=""></asp:ListItem>
                        <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Literal ID="litStatusJs" runat="server" />
                </div>
            </div>
            <div class="form-group form-actions" style="padding-top:20px;">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click"><i class="hi hi-search"></i> Search</asp:LinkButton>
                <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
            </div>
        </div>
        <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="false">
            <asp:Literal ID="litDebug" runat="server" />
            <asp:Literal ID="litMessage" runat="server" />
            <div class="block" style="padding-bottom: 20px;">
                <div class="block-title">
                    <h2><strong>Search Results</strong></h2>
                </div>
                <div class="table-responsive" style="padding-bottom: 10px">
                    <telerik:RadGrid ID="rgBackup" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                        OnNeedDataSource="rgBackup_NeedDataSource" 
                        OnItemDataBound="rgBackup_ItemDataBound" 
                        OnPreRender="rgBackup_PreRender"
                        AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" ShowFooter="false" Width="100%" PageSize="500">
                        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" CommandItemDisplay="None">
                            <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                            <NoRecordsTemplate>
                                <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                            </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="ClientName" DataField="ClientName" SortExpression="ClientName" HeaderText="Client" HeaderTooltip="Client Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="15%"/>
                                <telerik:GridBoundColumn UniqueName="ProtectedMachine" DataField="ProtectedMachine" SortExpression="ProtectedMachine" HeaderText="Protected Machine" HeaderTooltip="Protected Machine" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="21%"/>
                                <telerik:GridBoundColumn UniqueName="BackupServer" DataField="BackupServer" SortExpression="BackupServer" HeaderText="Backup Server" HeaderTooltip="Backup Server" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%"/>
                                <telerik:GridBoundColumn UniqueName="BackupSetName" DataField="BackupSetName" SortExpression="BackupSetName" HeaderText="Backup Set Name" HeaderTooltip="Backup Set Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%"/>
                                <telerik:GridBoundColumn UniqueName="LastBackupDate" DataField="LastBackupDate" SortExpression="LastBackupDate" HeaderText="Last Attempt" HeaderTooltip="Last Attempt" DataFormatString="{0:MM/dd/yy}" HeaderStyle-Width="8%"/>
                                <telerik:GridBoundColumn UniqueName="LastSuccessfulBackupDate" DataField="LastSuccessfulBackupDate" SortExpression="LastSuccessfulBackupDate" HeaderText="Last Success" HeaderTooltip="Last Success" DataFormatString="{0:MM/dd/yy}" HeaderStyle-Width="8%"/>
                                <telerik:GridBoundColumn UniqueName="Active" DataField="Active" SortExpression="Active" HeaderText="Active" HeaderTooltip="Active" HeaderStyle-Width="8%"/>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" />
                    </telerik:RadGrid>
                </div>
                
            </div>
        </asp:PlaceHolder>
    </asp:Panel>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="ralpTicket" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" Runat="Server"></asp:Content>
