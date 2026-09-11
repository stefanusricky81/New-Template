<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="DIDAssignment.aspx.cs" Inherits="Maintenance_DID_DIDAssignment" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script type="text/javascript">
        $(function () {
            //Normal Configuration
            $("[id*=txtMultiAddDIDNumber]").MaxLength({ MaxLength: 10 });
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>DID Assignment</h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" Visible="false" />
     <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSearch">
         <div class="block">
            <div class="row">
                <div class="form-group col-sm-4">
                    <label>DID number</label>
                    <asp:TextBox ID="txtDIDNumber" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter DID number" />
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
                    <label>Active</label>
                    <asp:DropDownList ID="ddlActive" CssClass="form-control" runat="server" >
                        <asp:ListItem Text="Select" Value="" />
                        <asp:ListItem Text="Yes" Value="Y" />
                        <asp:ListItem Text="No" Value="N" />
                    </asp:DropDownList>
                </div>
                <div class="form-group col-sm-4">
                    <label>Client</label>
                    <ddl:Client ID="ddlClient" runat="server" IsRequired="false" DisplayChosenScript="true" />
                </div>
            </div>
            <div class="row">
                <div style="float:right;">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSearch_Click" ValidationGroup="vgTrunkGroup"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="lbClear" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbClear_Click" ValidationGroup="vgTrunkGroup"><i class="hi hi-remove"></i> Clear</asp:LinkButton>
                </div>
            </div>
             <br />
         </div>
         
         <div class="block">
            <div class="row">
                <div style="float:right;">
                    <asp:LinkButton ID="lbMultiAdd" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbMultipleAdd_Click">Multi-Add</asp:LinkButton>
                    <asp:LinkButton ID="lbSingleAdd" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbSingleAdd_Click">Add New</asp:LinkButton>
                    <asp:LinkButton ID="lbBulkAssign" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbBulkAssign_Click">Bulk Assign</asp:LinkButton>
                </div>
            </div><br />
            <div class="row">
                <telerik:RadGrid ID="rgDIDAssignment" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="50" Width="100%" OnNeedDataSource="rgDIDAssignment_NeedDataSource" OnItemCommand="rgDIDAssignment_ItemCommand">
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" 
                        DataKeyNames="didnumberid,didnumber,didlocationid,trunkgroupid,assigned,active,ringtodidnumber,clientcode,diddevicename,trunkgroupdescr"
                        Width="100%" AllowSorting="true" CommandItemDisplay="Top">
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <CommandItemTemplate>
                            <div style="margin:10px;">
                                <asp:LinkButton ID="lbImport" runat="server" OnClick="lbImport_Click"><asp:Image ID="imgbulk" runat="server" ImageUrl="~/Images/bulkupload.jpg" />  Import</asp:LinkButton>  
                            </div>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" HeaderText="Bulk" UniqueName="Bulk">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbBulk" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" HeaderText="" UniqueName="Name">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditRecord" runat="server" CommandArgument="UpdateRecord"> <i class="gi gi-pencil" title="Edit"></i></asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="trunkgroupid" HeaderText="ID" Display="false" SortExpression="trunkgroupid" UniqueName="trunkgroupid" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="didlocationid" HeaderText="ID" Display="false" SortExpression="didlocationid" UniqueName="didlocationid" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            
                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" SortExpression="didnumber" HeaderStyle-Width="10%" HeaderText="DID Number" UniqueName="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDIDNumber" runat="server" Text='<%# Eval("didnumber") %>' CommandArgument="UpdateRecord" > <i class="gi gi-pencil" title="DID Number"></i></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="didnumber" Display="false" HeaderText="DID number" SortExpression="didnumber" UniqueName="didnumber" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />

                            <telerik:GridBoundColumn DataField="locationname" HeaderText="DID Location" SortExpression="locationname" UniqueName="locationname" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="trunkgroupdescr" HeaderText="Trunk group" SortExpression="trunkgroupdescr" UniqueName="trunkgroupdescr" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="assigned" HeaderText="Assigned" SortExpression="assigned" UniqueName="assigned" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="active" HeaderText="Active" SortExpression="active" UniqueName="active" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="ringtodidnumber" HeaderText="Ring to number" SortExpression="ringtodidnumber" UniqueName="ringtodidnumber" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="clientcode" Display="false" HeaderText="Client" SortExpression="clientcode" UniqueName="clientcode" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="pclient" Display="false" HeaderText="Client id" SortExpression="pclient" UniqueName="pclient" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="company" HeaderText="Client" SortExpression="company" UniqueName="company" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="diddevicename" HeaderText="Device name" SortExpression="diddevicename" UniqueName="diddevicename" HeaderStyle-Width="8%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn DataField="didnumberid" HeaderText="ID" SortExpression="didnumberid" UniqueName="didnumberid" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" />
                </telerik:RadGrid>
            </div>
         </div>
     </asp:Panel>

    <div id="myModalSingleAddEdit" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content" style="width:120%">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabelSingleAddEdit"><asp:Label ID="lblModalTitleSingleAddEdit" runat="server" /></h4>
                </div>
                <asp:ValidationSummary ID="vsDIDSingleAddEdit" runat="server" CssClass="validationSummary" ValidationGroup="vgAddEdit" />
                <asp:HiddenField ID="hfEditOldDidnumber" runat="server" />
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-sm-6">
                                <asp:Panel ID="pnlEdit" runat="server" Visible="false">
                                <div class="row">
                                    <label class="col-md-6 control-label">Id</label>
                                </div>
                                    <div class="form-group">
                                        <asp:Label ID="lblSingleIdEdit" runat="server" />
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="col-sm-6">
                                <div class="row">
                                    <label class="col-lg-6 control-label">Active </label>
                                </div>
                                <div class="form-group">             
                                    <asp:CheckBox ID="cbAddSingleActive" Checked="true" runat="server" />
                                </div>                               
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-6">
                                <div class="row">
                                    <label class="col-md-6 control-label">DID number </label>
                                    <asp:RequiredFieldValidator ID="rfvAddSingleDIDNumber" runat="server" ControlToValidate="txtAddSingleDIDNumber" ErrorMessage="DID number is required" ForeColor="Red" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">                                    
                                    <asp:TextBox runat="server" ID="txtAddSingleDIDNumber" MaxLength="12" CssClass="form-control" Placeholder="Enter DID number" />
                                </div>                               
                            </div>
                            <div class="col-sm-6">
                                <div class="row">
                                    <label class="col-md-6 control-label">Ring to number </label>
                                    <%--<asp:RequiredFieldValidator ID="rfvAddSingleRingToNumber" runat="server" ControlToValidate="txtAddSingleDIDNumber" ErrorMessage="Ring to number is required" ForeColor="Red" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator>--%>
                                </div>
                                <div class="form-group">   
                                    <asp:TextBox runat="server" ID="txtAddSingleRingToNumber" MaxLength="12" CssClass="form-control" Placeholder="Enter DID number" />
                                </div>                               
                            </div>
                            
                        </div>
                        <div class="form-group">
                            <div class="col-sm-6">
                                <div class="row">
                                    <label class="col-lg-6 control-label">Location </label>
                                    <asp:RequiredFieldValidator ID="rfvSingleAddLocation" runat="server" ControlToValidate="ddlAddSingleLocation" ErrorMessage="Location is required" ForeColor="Red" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">                                    
                                    <asp:DropDownList ID="ddlAddSingleLocation" runat="server" CssClass="form-control" />
                                </div>                               
                            </div>
                            <div class="col-sm-6">
                                <div class="row">
                                    <label class="col-lg-6 control-label">Client </label>
                                </div>
                                <div class="form-group">     
                                    <ddl:Client ID="ddlAddSingleClient" runat="server" IsRequired="true" DisplayChosenScript="true" ValidationGroup="vgAddEdit" />
                                </div>                               
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-6">
                                <div class="row">
                                    <label class="col-lg-6 control-label">Trunk group </label>
                                    <asp:RequiredFieldValidator ID="rfvSingleAddTrunk" runat="server" ControlToValidate="ddlAddSingleTrunk" ErrorMessage="Trunk is required" ForeColor="Red" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">                                    
                                    <asp:DropDownList ID="ddlAddSingleTrunk" runat="server" CssClass="form-control" />
                                </div>                               
                            </div>
                            <div class="col-sm-6">
                                <div class="row">
                                    <label class="col-lg-6 control-label">Device Name </label>
                                    <%--<asp:RequiredFieldValidator ID="rfvAddSingleDeviceName" runat="server" ControlToValidate="txtAddSingleDeviceName" ErrorMessage="Device name is required" ForeColor="Red" ValidationGroup="vgAddEdit">*</asp:RequiredFieldValidator>--%>
                                </div>
                                <div class="form-group">                                    
                                    <asp:TextBox runat="server" ID="txtAddSingleDeviceName" MaxLength="50" CssClass="form-control" Placeholder="Enter device name" />
                                </div>                               
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-6">
                                <div class="row">
                                    <label class="col-lg-6 control-label">Assigned </label>
                                </div>
                                <div class="form-group">                                    
                                    <asp:RadioButtonList ID="rblAddSingleAssigned" RepeatDirection="Horizontal" runat="server">
                                        <asp:ListItem Text="Yes" Value="Y" Selected="True" />
                                        <asp:ListItem Text="No" Value="N" />
                                    </asp:RadioButtonList>
                                </div>                               
                            </div>
                            <div class="col-sm-6">
                                <div class="row">
                                    <label class="col-lg-6 control-label">International </label>
                                </div>
                                <div class="form-group">             
                                    <asp:CheckBox ID="cbAddSingleInternational" runat="server" />
                                </div>                               
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddSingleDIDAssignment" runat="server" Text="Add" class="btn btn-primary" Visible="false" OnCommand="Decision_Command" ValidationGroup="vgAddEdit" CommandArgument="AddSingle" />
                    <asp:Button ID="btnEditSingleDIDAssignment" runat="server" Text="Save" class="btn btn-primary" Visible="false" OnCommand="Decision_Command" ValidationGroup="vgAddEdit" CommandArgument="Edit" />
                </div>
            </div>
        </div>
    </div>

    <div id="myModalMultiAdd" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabelMultiAdd"><asp:Label ID="lblModalTitleMultiAdd" runat="server" /></h4>
                </div>
                <asp:ValidationSummary ID="vsMultipleAddDID" runat="server" CssClass="validationSummary" ValidationGroup="vgAddMultiple" />
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-sm-6">
                                <div class="form-group">        
                                    <label class="control-label">Location </label><asp:RequiredFieldValidator ID="rfvMultipleAddLocation" runat="server" ControlToValidate="ddlMultipleAddLocation" ForeColor="Red" ErrorMessage="Location is required" ValidationGroup="vgAddMultiple">*</asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="ddlMultipleAddLocation" runat="server" CssClass="form-control" />
                                </div>
                                <div class="form-group"> 
                                    <label class="control-label">Trunk group </label><asp:RequiredFieldValidator ID="rfvMultipleAddTrunk" runat="server" ControlToValidate="ddlMultipleAddTrunk" ForeColor="Red" ErrorMessage="Trunk is required" ValidationGroup="vgAddMultiple">*</asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="ddlMultipleAddTrunk" runat="server" CssClass="form-control" />
                                </div>
                                <div class="form-group"> 
                                    <label class="control-label">Assigned </label>
                                    <asp:RadioButtonList ID="rblMultiAddAssigned" RepeatDirection="Horizontal" runat="server">
                                        <asp:ListItem Text="Yes" Value="Y" Selected="True" />
                                        <asp:ListItem Text="No" Value="N" />
                                    </asp:RadioButtonList>
                                </div>
                                <div class="form-group">  
                                    <label class="control-label">Active </label>
                                    <asp:CheckBox ID="cbMultiAddActive" Checked="true" runat="server" />
                                </div>
                                <div class="form-group" style="display:none">  
                                    <label class="control-label">Ring to number </label>
                                    <asp:TextBox runat="server" ID="txtMultiAddRingtoNumber" MaxLength="12" CssClass="form-control" Placeholder="Enter DID number" />
                                </div>
                                <div class="form-group">
                                    <label class="control-label">Client </label>
                                    <ddl:Client ID="ddlMultiAddClient" runat="server" IsRequired="true" DisplayChosenScript="true" ValidationGroup="vgAddMultiple" />

                                </div>
                                <div class="form-group" style="display:none">  
                                    <label class="control-label">Device Name </label>
                                    <asp:TextBox runat="server" ID="txtMultiAddDeviceName" MaxLength="50" CssClass="form-control" Placeholder="Enter device name" />
                                </div>
                                <div class="form-group">
                                    <label class="control-label">International </label>
                                    <asp:CheckBox ID="cbMultiAddInternational" runat="server" />
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label">DIDs-one DID per line</label><asp:RequiredFieldValidator ID="rfvMultiAddDIDNumber" runat="server" ControlToValidate="txtMultiAddDIDNumber" ForeColor="Red" ErrorMessage="DID Number is required" ValidationGroup="vgAddMultiple">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtMultiAddDIDNumber"  runat="server" Rows="22" TextMode="MultiLine" CssClass="form-control" />
                                    <%--<input name="txtMultiAddDIDNumber" runat="server" Rows="22" TextMode="MultiLine" CssClass="form-control" />
                                    <textarea id="txtMultiAddDIDNumber" runat="server" rows="22" " />--%>
                                </div>                      
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnAddMultipleDIDAssignment" runat="server" Text="Add" class="btn btn-primary" OnCommand="Decision_Command" ValidationGroup="vgAddMultiple" CommandArgument="AddMultiple" />
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
                            <asp:HiddenField ID="hfDeleteDIDNumber" runat="server" />
                            <asp:HiddenField ID="hfDeleteTrunkGroup" runat="server" />
                            <asp:HiddenField ID="hfDeleteClient" runat="server" />
                            <asp:HiddenField ID="hfAssigned" runat="server" />
                    </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnYes" runat="server" Text="Delete" class="btn btn-primary" OnCommand="Decision_Command" CommandArgument="Delete" />
                </div>
            </div>
        </div>
    </div>

    <div id="myModalBulkAssign" class="modal fade">
        <div class="modal-dialog" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabelBulkAssign"><asp:Label ID="lblModalBulkAssign" runat="server" /></h4>
                </div>
                <asp:ValidationSummary ID="vsBulkAssign" runat="server" CssClass="validationSummary" ValidationGroup="vgBulkAssign" />
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-sm-8">
                                <div class="form-group">        
                                    <label class="control-label">DID Numbers Selected : </label>
                                </div>
                                <div class="form-group"> 
                                    <telerik:RadGrid ID="rgBulkAssign" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                                        AllowPaging="true" ShowFooter="false" PageSize="50" Width="100%">
                                        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" 
                                            DataKeyNames="didnumber,assigned,active,clientcode"
                                            Width="100%" AllowSorting="true">
                                            <NoRecordsTemplate>
                                                <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="didnumberid" Display="false" HeaderText="id" SortExpression="didnumberid" UniqueName="didnumberid" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                                                <telerik:GridBoundColumn DataField="didnumber" HeaderText="DID number" SortExpression="didnumber" UniqueName="didnumber" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                                                <telerik:GridBoundColumn DataField="assigned" HeaderText="Assigned" SortExpression="assigned" UniqueName="assigned" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                                                <telerik:GridBoundColumn DataField="active" HeaderText="Active" SortExpression="active" UniqueName="active" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                                                <telerik:GridBoundColumn DataField="clientcode" HeaderText="Client" SortExpression="clientcode" UniqueName="clientcode" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                                                <telerik:GridBoundColumn DataField="trunkgroupdescr" Display="false" HeaderText="TrunkGroup" SortExpression="trunkgroupdescr" UniqueName="trunkgroupdescr" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" />
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" />
                                    </telerik:RadGrid>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">  
                                    <label class="control-label">Active </label>
                                    <asp:CheckBox ID="cbActiveBulkAssign" Checked="true" runat="server" />
                                </div> 
                                <div class="form-group"> 
                                    <label class="control-label">Assigned </label>
                                    <asp:RadioButtonList ID="rblAssignedBulkAssign" RepeatDirection="Horizontal" runat="server">
                                        <asp:ListItem Text="Yes" Value="Y" Selected="True" />
                                        <asp:ListItem Text="No" Value="N" />
                                    </asp:RadioButtonList>
                                </div>
                                <div class="form-group">
                                    <label class="control-label">Client </label>
                                    <ddl:Client ID="ddlClientBulkAssign" runat="server" IsRequired="true" DisplayChosenScript="true" ValidationGroup="vgBulkAssign"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnBulkAssign" runat="server" Text="Update" class="btn btn-primary" OnCommand="Decision_Command" ValidationGroup="vgBulkAssign" CommandArgument="BulkAssign" />
                </div>
            </div>
        </div>
    </div>

    <div id="myModalImport" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title" id="myModalLabelImport"><asp:Label ID="lblModalTitleImport" runat="server" /></h4>
            </div>
            <asp:ValidationSummary ID="vsImport" runat="server" CssClass="validationSummary" ValidationGroup="vgImport" />
            <div class="modal-body">
                <div class="row">
                    <div class="form-group">
                        <div class="col-md-12">
                            <div class="row">
                                <label class="col-md-12 control-label">Template File - <asp:LinkButton ID="btnDownload" runat="server" Text="Download here" OnClick="btnDownload_Click" /></label>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row">
                                <label class="col-md-4 control-label">File</label>
                                <div class="col-md-8">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="rauFileUpload" MaxFileInputsCount="1" />
                                            <asp:CustomValidator runat="server" ID="cvUpload" ValidationGroup="vgImport" Display="None" ClientValidationFunction="validateUpload" ErrorMessage="Please Select File To Upload" />
                                        </div>
                                        <div class="col-md-1">&nbsp;</div>
                                        <div class="col-md-1">
                                            <asp:Button ID="btnImport" runat="server" Text="Import" class="btn btn-primary" ValidationGroup="vgImport" OnClick="btnImport_Click" />
                                        </div>
                                    </div>                                  
                                </div>
                            </div>
                        </div>
                </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
            </div>
        </div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" Runat="Server">
</asp:Content>

