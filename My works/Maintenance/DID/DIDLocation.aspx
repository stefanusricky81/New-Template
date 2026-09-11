<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="DIDLocation.aspx.cs" Inherits="Maintenance_DID_DIDLocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>DID Location</h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" Visible="false" />
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <div class="block">
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>Location name</label>
                    <asp:TextBox ID="txtLocName" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter location name" />
                </div>
                <div class="form-group col-sm-4">
                    <label>City</label>
                    <asp:TextBox ID="txtCity" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter city" />
                </div>
                <div class="form-group col-sm-4">
                    <label>State</label>
                    <asp:DropDownList ID="ddlState" CssClass="form-control" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>Zip</label>
                    <asp:TextBox ID="txtZip" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter zip" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Service Id</label>
                    <asp:TextBox ID="txtServiceId" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter service id" />
                </div>
                <div class="form-group col-sm-4">
                    <label>Area Code</label>
                    <asp:TextBox ID="txtAreCode" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter area code" />
                </div>
            </div>
            <div class="row">
                <div style="float:right;">
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgTrunkGroup"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="lbClear" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbClear_Click"><i class="hi hi-remove"></i> Clear</asp:LinkButton>
                </div>
            </div>
            <br />
        </div>

        <div class="block">
            <div class="row">
                <div style="float:right;">
                    <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnAdd_Click">Add New</asp:LinkButton>
                </div>
            </div><br />
            <div class="row">
                <telerik:RadGrid ID="rgDidLocation" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%" OnNeedDataSource="rgDidLocation_NeedDataSource" OnItemCommand="rgDidLocation_ItemCommand">
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" 
                        DataKeyNames="didlocationid,locationname,addr1,addr2,city,state,zip,serviceid,areacode" 
                        Width="100%" AllowSorting="true">
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Name">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditRecord" runat="server" CommandArgument="UpdateRecord"> <i class="gi gi-pencil" title="Edit"></i></asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="didlocationid" HeaderText="ID" SortExpression="didlocationid" UniqueName="didlocationid" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="locationname" HeaderText="Location Name" SortExpression="locationname" UniqueName="locationname" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="addr1" HeaderText="Address Line 1" SortExpression="addr1" UniqueName="addr1" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="addr2" HeaderText="Address Line 2" SortExpression="addr2" UniqueName="addr2" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="city" HeaderText="City" SortExpression="city" UniqueName="city" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="state" HeaderText="State" SortExpression="state" UniqueName="state" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="zip" HeaderText="Zip" SortExpression="zip" UniqueName="zip" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="serviceid" HeaderText="Service id" SortExpression="serviceid" UniqueName="serviceid" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="areacode" HeaderText="Area code" SortExpression="areacode" UniqueName="areacode" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" />
                </telerik:RadGrid>
            </div>
        </div>
    </asp:Panel>

    <div id="myModalAddEdit" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabelAddEdit"><asp:Label ID="lblModalTitleAddEdit" runat="server" /></h4>
                </div>
                <asp:ValidationSummary ID="vsDIDLocation" runat="server" CssClass="validationSummary" ValidationGroup="vgAddEdit" />
                <div class="modal-body">
                    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
                        <div class="row">
                            <div class="form-group">
                                <label class="col-md-3 control-label">Id</label>
                                <div class="col-md-9">
                                    <asp:Label ID="lblIdEdit" runat="server" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-3 control-label">Location name<asp:RequiredFieldValidator ID="rfvAddLocName" runat="server" ErrorMessage="Location name is required" ControlToValidate="txtAddLocName" ForeColor="Red" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator></label>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtAddLocName" MaxLength="50" CssClass="form-control" Placeholder="Enter Location Name" />
                                    </div>
                                </div>                               
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-3 control-label">Address Lne 1<asp:RequiredFieldValidator ID="rfvAddrLine1" runat="server" ErrorMessage="Address line 1 is required" ControlToValidate="txtAddrLine1" ForeColor="Red" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator></label>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtAddrLine1" MaxLength="50" CssClass="form-control" Placeholder="Enter Address Line 1" />
                                    </div>
                                </div>                               
                            </div>
                             <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-3 control-label">Address Lne 2<%--<asp:RequiredFieldValidator ID="rfvAddrLine2" runat="server" ControlToValidate="txtAddrLine2" ForeColor="Red" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator>--%></label>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtAddrLine2" MaxLength="50" CssClass="form-control" Placeholder="Enter Address Line 2" />
                                    </div>
                                </div>                               
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-3 control-label">City<asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtAddCity" ErrorMessage="City is Required" ForeColor="Red" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator></label>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtAddCity" MaxLength="50" CssClass="form-control" Placeholder="Enter City" />
                                    </div>
                                </div>                               
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-3 control-label">State </label>
                                    <div class="col-md-9">
                                        <asp:DropDownList ID="ddlAddState" CssClass="form-control" runat="server" />
                                    </div>
                                </div>                               
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-3 control-label">Zip<%--<asp:RequiredFieldValidator ID="rfvZip" runat="server" ControlToValidate="txtAddZip" ForeColor="Red" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator>--%></label>
                                    <div class="col-md-4">
                                        <asp:TextBox runat="server" ID="txtAddZip" MaxLength="10" CssClass="form-control" Placeholder="Enter zip" />
                                    </div>
                                    <div class="col-md-5">
                                        <asp:RegularExpressionValidator ID="revAddZip" runat="server" ControlToValidate="txtAddZip" ErrorMessage="Please Enter Only Numbers" ForeColor="Red" ValidationGroup="vgAddEdit" ValidationExpression="^\d+$" />
                                    </div>
                                </div>                               
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-3 control-label">Service Id<asp:RequiredFieldValidator ID="rfvServiceID" runat="server" ErrorMessage="Service ID is required" ControlToValidate="txtAddServiceID" ForeColor="Red" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator></label>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtAddServiceID" MaxLength="25" CssClass="form-control" Placeholder="Enter service id" />
                                    </div>
                                </div>                               
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <label class="col-md-3 control-label">Area code<asp:RequiredFieldValidator ID="rfvAreaCode" runat="server" ErrorMessage="Area code is required" ControlToValidate="txtAreaCode" ForeColor="Red" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator></label>
                                    <div class="col-md-4">
                                        <asp:TextBox runat="server" ID="txtAreaCode" MaxLength="3" CssClass="form-control" Placeholder="Enter area code" />
                                    </div>
                                    <div class="col-md-5">
                                        <asp:RegularExpressionValidator ID="revAreaCode" runat="server" ControlToValidate="txtAreaCode" ErrorMessage="Please Enter Only Numbers" ForeColor="Red" ValidationGroup="vgAddEdit" ValidationExpression="^\d+$" />
                                    </div>
                                </div>                               
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddTrunk" runat="server" Text="Add" class="btn btn-primary" Visible="false" OnCommand="Decision_Command" ValidationGroup="vgAddEdit" CommandArgument="Add" />
                    <asp:Button ID="btnEditTrunk" runat="server" Text="Save" class="btn btn-primary" Visible="false" OnCommand="Decision_Command" ValidationGroup="vgAddEdit" CommandArgument="Edit" />
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
<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" Runat="Server">
</asp:Content>

