<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientServerEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_ClientServerEdit" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<%@ Register TagPrefix="ddl" TagName="SupportingTable" Src="~/UserControl/DropDownList/SupportingTable.ascx" %>
<style>
    .RadGrid_3b .rgEditForm a { color:#ffffff; }
    .RadGrid_3b .rgEditForm {margin: -1px; padding: 7px; border-bottom: 1px solid #dddddd; background-color:#f2f2f2; }
    .chosen-container-single .chosen-single { color:black !important;}
    .disabledchk {padding-left:8px; margin-top:8px;}
</style>
<div class="block">
    <div class="block-title">
        <h2>
            <strong><asp:Literal ID="litHeader" runat="server" Text="Add Edit Client Server" /></strong>
            <asp:ValidationSummary ID="vsClientServer" runat="server" ValidationGroup="vgClientServer" CssClass="validationSummary" />
        </h2>
    </div>
    <asp:Literal ID="litMessage" runat="server" />
    <div class="form-horizontal form-bordered">
        <div class="form-group">
            <label class="col-md-3 control-label">Server Name <span class="text-danger">*</span></label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtName" MaxLength="50" CssClass="form-control" Placeholder="Enter Server Name ..." TabIndex="100"/>
                <asp:RequiredFieldValidator ID="rvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required" ValidationGroup="vgClientServer" Display="None" />
                <asp:CustomValidator ID="cvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name is in use" ValidationGroup="vgClientServer" Display="None" OnServerValidate="cvName_ServerValidate" />
            </div>
        </div>
        <asp:PlaceHolder ID="phClient" runat="server" Visible="false">
            <div class="form-group">
                <label class="col-md-3 control-label">Client <span class="text-danger">*</span></label>
                <div class="col-md-5">
                    <ddl:Client ID="ddlClient" runat="server" IsRequired="true" ValidationGroup="vgClientServer" DisplayDefaultValue="true" TabIndex="101"  />
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="form-group">
            <label class="col-md-3 control-label">Backup Type</label>
            <div class="col-md-5">
                <ddl:SupportingTable ID="ddlBackupType" runat="server" IsRequired="false" TabIndex="102"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">OS</label>
            <div class="col-md-5">
                <ddl:SupportingTable ID="ddlOs" runat="server" IsRequired="false" DisplayDefaultValue="true" TabIndex="103"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">OS Version</label>
            <div class="col-md-5">
                <ddl:SupportingTable ID="ddlOsVersion" runat="server" IsRequired="false" DisplayDefaultValue="true" TabIndex="104"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Server/Workstation</label>
            <div class="col-md-5">
                <asp:DropDownList ID="ddlServerWorkstation" runat="server" CssClass="form-control" TabIndex="106">
                    <asp:ListItem Text="Server" Value="1" />
                    <asp:ListItem Text="Workstation" Value="0" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Alternate Backup Set Server Name</label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtBackupSet" MaxLength="50" CssClass="form-control" Placeholder="Enter Alternate Backup Set Server Name..." TabIndex="107"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Description</label>
            <div class="col-md-5">
                <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control" Placeholder="Enter Description ..." TabIndex="108" TextMode="MultiLine" Rows="5"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">No Backup</label>
            <div class="col-md-5">
                <asp:CheckBox ID="chkNoBackup" runat="server" CssClass="form-control-borderless disabledchk" TabIndex="109" AutoPostBack="true" OnCheckedChanged="chkNoBackup_CheckedChanged" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">No EndPoint</label>
            <div class="col-md-5">
                <asp:CheckBox ID="chkNoEndPoint" runat="server" CssClass="form-control-borderless disabledchk" TabIndex="110" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">No Patch</label>
            <div class="col-md-5">
                <asp:CheckBox ID="chkPatch" runat="server" CssClass="form-control-borderless disabledchk" TabIndex="111" />
            </div>
        </div>
        <asp:PlaceHolder ID="phNoBackupYes" runat="server" Visible="false">
            <div class="form-group">
                <label class="col-md-3 control-label">No Backup Reason<span class="text-danger">*</span></label>
                <div class="col-md-5">
                    <asp:TextBox runat="server" ID="txtNoBackupReason" MaxLength="2000" CssClass="form-control" Placeholder="Enter No Backup Reason ..." TabIndex="112" TextMode="MultiLine" Rows="3"/>
                    <asp:RequiredFieldValidator ID="rfvNoBackupReason" runat="server" ControlToValidate="txtNoBackupReason" ErrorMessage="No Backup Reason is required" ValidationGroup="vgClientServer" Display="None" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-3 control-label">No Backup Approved</label>
                <div class="col-md-5">
                    <asp:MultiView ID="mvNoBackupApproved" runat="server" ActiveViewIndex="0">
                        <asp:View ID="viewNoBackupReadOnly" runat="server">
                            <p class="form-control-static"><asp:Literal ID="litNoBackupApproved" runat="server" /></p>
                        </asp:View>
                        <asp:View ID="viewNoBackupEdit" runat="server">
                            <asp:DropDownList ID="ddlNoBackupApproved" runat="server" TabIndex="113" CssClass="form-control">
                                <asp:ListItem Text="Pending" Value="" />
                                <asp:ListItem Text="Approved" Value="1" />
                                <asp:ListItem Text="Denied" Value="0" />
                            </asp:DropDownList>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="form-group">
            <label class="col-md-3 control-label">Active</label>
            <div class="col-md-5">
                <asp:CheckBox ID="chkActive" runat="server" CssClass="form-control form-control-borderless" TabIndex="114"  />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Single Backup</label>
            <div class="col-md-5">
                <asp:CheckBox ID="chkSingleBackup" runat="server" CssClass="form-control form-control-borderless" TabIndex="115"  />
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-3 control-label">Backup Server</label>
            <div class="col-md-5">
                <asp:CheckBox ID="chkBackupServer" runat="server" CssClass="form-control form-control-borderless" TabIndex="116"  />
            </div>
        </div>
        <asp:PlaceHolder ID="phExistingItem" runat="server">
            <div class="form-group">
                <label class="col-md-3 control-label">Local</label>
                <div class="col-md-5">
                    <asp:CheckBox ID="chkLocal" runat="server" CssClass="form-control-borderless disabledchk" TabIndex="117" Enabled="false" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-3 control-label">Offsite</label>
                <div class="col-md-5">
                    <asp:CheckBox ID="chkOffsite" runat="server" CssClass="form-control-borderless disabledchk" TabIndex="118" Enabled="false" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-3 control-label">Replica</label>
                <div class="col-md-5">
                    <asp:CheckBox ID="chkReplica" runat="server" CssClass="form-control-borderless disabledchk" TabIndex="119" Enabled="false" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-3 control-label">Imported</label>
                <div class="col-md-5">
                    <asp:CheckBox ID="chkImported" runat="server" CssClass="form-control-borderless disabledchk" TabIndex="120" Enabled="false" />
                </div>
            </div>
             <div class="form-group">
            <label class="col-md-3 control-label"><asp:Literal ID="litCreatedHeader" runat="server" Text="Imported Date" /></label>
            <div class="col-md-5 form-control-static">
                <asp:Literal ID="litCreated" runat="server" />
            </div>
        </div>
        </asp:PlaceHolder>
    </div>
</div>

<div class="block">
    <div class="form-group form-actions" style="margin-left:20px;">
        <asp:LinkButton ID="btnEdit" runat="server" ValidationGroup="vgClientServer" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="Update" TabIndex="121"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
        <asp:LinkButton ID="btnAdd" runat="server" ValidationGroup="vgClientServer" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="PerformInsert" TabIndex="122"><i class="hi hi-plus"></i> Submit</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CssClass="btn btn-sm btn-warning" CommandName="Cancel"><i class="hi hi-remove" TabIndex="123"></i> Cancel</asp:LinkButton>
    </div>
</div>