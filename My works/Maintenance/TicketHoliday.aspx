<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="TicketHoliday.aspx.cs" Inherits="Maintenance_TicketHoliday" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Bit By Bit Ticket Holiday Table</h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" />
    <div class="block">
        <div class="row">
            <div style="float:left;">
                <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnAdd_Click">Add New</asp:LinkButton>
            </div>
        </div><br />
        <telerik:RadGrid ID="rgHoliday" OnNeedDataSource="rgHoliday_NeedDataSource" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
            AllowPaging="true" ShowFooter="false" PageSize="20" Width="100%" OnItemCommand="rgHoliday_ItemCommand"> 
            <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%">
                <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="10,25,50,100,250,500" PageSizeControlType="RadComboBox" Position="Bottom" />
                <NoRecordsTemplate>
                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                </NoRecordsTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="ButtonColumn" HeaderText="Action" HeaderStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div class="row">
                                <div class="form-group col-sm-4">
                                    <asp:ImageButton ID="ibUpdate" runat="server" ImageUrl="~/Images/rename.png" CommandArgument="UpdateRecord" />
                                </div>
                                <div class="form-group col-sm-4">
                                    <asp:ImageButton ID="ibDelete" runat="server" ImageUrl="~/Images/delete.png" CommandArgument="DeleteRecord" />
                                </div>
                            </div>                                  
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="HolidayDate" HeaderStyle-Width="19%" DataType="System.DateTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Date" SortExpression="HolidayDate" UniqueName="HolidayDate" HeaderTooltip="Holiday Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <telerik:GridBoundColumn DataField="Id" Display="false" HeaderStyle-Width="10%" DataType="System.Int32" ItemStyle-HorizontalAlign="Left" HeaderText="ID" SortExpression="Id" UniqueName="Id" />
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>

<div id="myModalAddEdit" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title" id="myModalLabelAddEdit"><asp:Label ID="lblModalTitleAddEdit" runat="server" /></h4>
            </div>
            <asp:ValidationSummary ID="vsTicketType" runat="server" CssClass="validationSummary" ValidationGroup="vgAddEdit" />
            <div class="modal-body">
                <asp:Panel ID="pnlEdit" runat="server" Visible="false">
                    <div class="row">
                        <div class="form-group">
                            <label class="col-md-2 control-label">Id</label>
                            <div class="col-md-10">
                                <asp:Label ID="lblIdEdit" runat="server" />
                                <asp:HiddenField ID="hfEdit" runat="server" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="row">
                    <div class="form-group">
                        <div class="col-md-12">
                            <div class="row">
                                <label class="col-md-4 control-label">Date *</label>
                                <div class="col-md-8">
                                    <uc:DatePicker ID="dpHolidayDate" runat="server" IsRequired="true" ValidationGroup="vgAddEdit"/>
                                </div>
                            </div>                               
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                <asp:Button ID="btnAddDate" runat="server" Text="Add" class="btn btn-primary" Visible="false" OnCommand="Decision_Command" ValidationGroup="vgAddEdit" CommandArgument="Add" />
                <asp:Button ID="btnEditDate" runat="server" Text="Edit" class="btn btn-primary" Visible="false" OnCommand="Decision_Command" ValidationGroup="vgAddEdit" CommandArgument="Edit" />
            </div>
        </div>
    </div>
</div>

<div id="myModalDelete" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title" id="myModalDeleteLabel"><asp:Label ID="lblModalDeleteTitle" runat="server" /></h4>
            </div>
                <div class="modal-body">
                <h3><asp:Label ID="lblModalDeleteWording" runat="server" /></h3>
                    <asp:HiddenField ID="hfDelete" runat="server" />
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                <asp:Button ID="btnYes" runat="server" Text="Delete" class="btn btn-primary" OnCommand="Decision_Command" CommandArgument="Delete" />
            </div>
        </div>
    </div>
</div>
</asp:Content>