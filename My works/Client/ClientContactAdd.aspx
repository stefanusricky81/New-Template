<%@ Page Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientContactAdd.aspx.cs" Inherits="Client_ClientContactAdd" Title="" %>
<%@ Register TagPrefix="rpt" TagName="ClientPortalUserRole" Src="~/UserControl/Repeater/ClientPortalUserRole.ascx" %>
<%@ Register TagPrefix="ctrl" TagName="ClientDetailTabs" Src="~/UserControl/Client/ClientDetailTabs.ascx" %>
<%@ Register TagPrefix="ddl" TagName="ClientLocation" Src="~/UserControl/DropDownList/ClientLocation.ascx" %>
<%@ Register TagPrefix="lb" TagName="ClientLocation" Src="~/UserControl/ListBox/ClientLocation.ascx" %>
<%--<%@ Register TagPrefix="ddl" TagName="ClientEmailCategory" Src="~/UserControl/DropDownList/ClientEmailCategory.ascx" %>--%>
<%@ Register TagPrefix="ddl" TagName="Country" Src="~/UserControl/DropDownList/Country.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">

    <%--<telerik:RadAjaxPanel ID="rapMenu" runat="server" LoadingPanelID="ralpMenu" EnableAJAX="true">--%>
        <asp:Literal ID="litMessage" runat="server" Visible="false" />
        <asp:Literal ID="litDebug" runat="server" />
        <ctrl:ClientDetailTabs ID="ClientTabs" runat="server" SelectedTabIndex="2" />
        <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
            <div class="block" style="padding-bottom:15px;">
                <asp:ValidationSummary ID="vsProfile" runat="server" ValidationGroup="vgContact" CssClass="validationSummary" />
                <div class="row">
                    <div class="col-md-6">
                        <div style="float: left">
                            <asp:HyperLink ID="hlBacktoList" runat="server"><i class="gi gi-list" title="Back to Contact list"></i>  Back to Contact list</asp:HyperLink>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div style="float: right">
                            <asp:HyperLink ID="hlAddNewRecord" runat="server"><i class="gi gi-user_add" title="Add New Contact"></i>  Add New Contact</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
            <div class="block" style="padding-bottom:15px;">
                <div class="block-title">
                    <h2><asp:Literal ID="litHeaderName" Text="Contact Details" runat="server" /></h2>
                </div>
                <div class="row">
                    <div class="form-group col-sm-6">
                        <label>First Name <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" CssClass="form-control" />
                        <%--<asp:RequiredFieldValidator ControlToValidate="txtFirstName" runat="server" ID="rfvFirstName" ValidationGroup="vgContact" Display="None" ErrorMessage="First Name is required"></asp:RequiredFieldValidator>--%>
                        <asp:RequiredFieldValidator ControlToValidate="txtFirstName" runat="server" ID="rfvFirstName" Display="None" ErrorMessage="First Name is required"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-sm-6">
                        <label>Last Name <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" CssClass="form-control" />
                        <%--<asp:RequiredFieldValidator ControlToValidate="txtLastName" runat="server" ID="rfvLastName" ValidationGroup="vgContact" Display="None" ErrorMessage="Last Name is required"></asp:RequiredFieldValidator>--%>
                        <asp:RequiredFieldValidator ControlToValidate="txtLastName" runat="server" ID="rfvLastName" Display="None" ErrorMessage="Last Name is required"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-6">
                        <label>Email <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" CssClass="form-control" />
                        <%--<asp:RequiredFieldValidator ControlToValidate="txtEmail" runat="server" ID="rfvEmail" ValidationGroup="vgContact" Display="None" ErrorMessage="Email is required"></asp:RequiredFieldValidator>--%>
                        <asp:RequiredFieldValidator ControlToValidate="txtEmail" runat="server" ID="rfvEmail" Display="None" ErrorMessage="Email is required"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvEmail" runat="server" ControlToValidate="txtEmail" ValidationGroup="vgContact" Display="None" OnServerValidate="cvEmail_ServerValidate" />
                    </div>
                    <div class="col-sm-6">
                        <div class="row">
                            <div class="form-group col-sm-9">
                                <asp:Label ID="lblPhone" runat="server" AssociatedControlID="txtPhone" Placeholder="Enter Phone ..." Text="Phone" />
                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="17" CssClass="form-control" />
                                <asp:CustomValidator ID="cvPhone" runat="server" ValidationGroup="vgContact" Display="None" OnServerValidate="cvPhone_ServerValidate" />
                                <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone" ValidationGroup="vgContact" Display="None"/>
                            </div>
                            <div class="form-group col-sm-3">
                                <asp:Label ID="lblExt" runat="server" AssociatedControlID="txtExt" Text="Ext" />
                                <asp:TextBox ID="txtExt" runat="server" MaxLength="6" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-6">
                        <asp:Label ID="lblCellPhone" runat="server" AssociatedControlID="txtPhone" Text="Cell Phone" />
                        <asp:TextBox ID="txtCellPhone" runat="server" MaxLength="17" CssClass="form-control" />
                        <asp:RegularExpressionValidator ID="revCellPhone" runat="server" ControlToValidate="txtCellPhone" ValidationGroup="vgContact" Display="None"/>
                    </div>
                    <div class="form-group col-sm-6">
                        <label>Home Phone</label>
                        <asp:TextBox ID="txtHomePhone" runat="server" MaxLength="17" CssClass="form-control" />
                        <asp:RegularExpressionValidator ID="revHomePhone" runat="server" ControlToValidate="txtHomePhone" ValidationGroup="vgContact" Display="None"/>
                    </div>
                </div>
                <div class="row">
                     <div class="form-group col-sm-6">
                        <label>Email 2 </label> <%--email4 in DB--%>
                        <asp:TextBox ID="txtEmail2" runat="server" MaxLength="50" CssClass="form-control" />
                        <asp:CustomValidator ID="cvEmail2" runat="server" ControlToValidate="txtEmail2" ValidationGroup="vgContact" Display="None" OnServerValidate="cvEmail_ServerValidate" />
                    </div>
                    <div class="form-group col-sm-6">
                        <label>Email 3 </label><%--email5 in DB--%>
                        <asp:TextBox ID="txtEmail3" runat="server" MaxLength="50" CssClass="form-control" />
                        <asp:CustomValidator ID="cvEmail3" runat="server" ControlToValidate="txtEmail3" ValidationGroup="vgContact" Display="None" OnServerValidate="cvEmail_ServerValidate" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-6">
                        <label>Email 4 </label><%--email6 in DB--%>
                        <asp:TextBox ID="txtEmail4" runat="server" MaxLength="50" CssClass="form-control" />
                        <asp:CustomValidator ID="cvEmail4" runat="server" ControlToValidate="txtEmail4" ValidationGroup="vgContact" Display="None" OnServerValidate="cvEmail_ServerValidate" />
                    </div>
                     <div class="form-group col-sm-6">
                        <label>Fax</label>
                        <asp:TextBox ID="txtFax" runat="server" MaxLength="10" CssClass="form-control" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-6">
                        <asp:Label ID="lblAddress1" runat="server" AssociatedControlID="txtAddress1" Text="Address1" />
                        <asp:TextBox ID="txtAddress1" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                    <div class="form-group col-sm-6">
                        <asp:Label ID="lblAddress2" runat="server" AssociatedControlID="txtAddress2" Text="Address2" />
                        <asp:TextBox ID="txtAddress2" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-6">
                        <asp:Label ID="lblCity" runat="server" AssociatedControlID="txtCity" Text="City" />
                        <asp:TextBox ID="txtCity" runat="server" MaxLength="50" CssClass="form-control" />
                    </div>
                    <div class="form-group col-sm-6">
                        <asp:Label ID="lblCountry" runat="server" AssociatedControlID="ddlCountry" Text="Country" />
                        <ddl:Country ID="ddlCountry" runat="server" CssClass="form-control select-chosen" DefaultText="United States" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-6">
                        <asp:Label ID="lblZip" runat="server" AssociatedControlID="txtZip" Text="Zip" />
                        <asp:TextBox ID="txtZip" runat="server" MaxLength="10" CssClass="form-control" />
                    </div>
                    <div class="form-group col-sm-6">
                        <asp:Label ID="lblState" runat="server" AssociatedControlID="ddlState" Text="State" />
                        <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control select-chosen"></asp:DropDownList>
                        <asp:TextBox ID="txtState" runat="server" CssClass="form-control" Visible="false" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-6">
                        <label>Priority</label>
                        <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control select-chosen" />
                    </div>
                    <div class="form-group col-sm-6">
                        <label>Title</label>
                        <asp:DropDownList ID="ddldesignation" runat="server" CssClass="form-control select-chosen" />
                    </div>
                    
                </div>
                <div class="row">
                    <div class="form-group col-sm-6">
                        <label>Preferred Method of Contact</label>
                        <asp:DropDownList ID="ddlMethodofContact" runat="server" CssClass="form-control">
                            <asp:ListItem Text="" />
                            <asp:ListItem Text="Email" />
                            <asp:ListItem Text="Phone" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-sm-2">
                        <label>VIP</label>
                        <asp:CheckBox ID="cbkVIP" runat="server" CssClass="form-control form-control-borderless" />
                    </div>
                    <div class="form-group col-sm-4">
                        <label>Active</label>
                        <asp:CheckBox ID="chkPortalActive" runat="server" CssClass="form-control form-control-borderless" />
                    </div>
                    
                </div>
                <div class="row">
                    <asp:PlaceHolder ID="phHelpDesk" runat="server" Visible="false">
                        <div class="form-group col-sm-6">
                            <label>Help Desk User</label>
                            <asp:CheckBox ID="chkHelpDesk" runat="server" CssClass="form-control form-control-borderless" />
                        </div>
                    </asp:PlaceHolder>
                </div>
            </div>
            <div class="block">
                <div class="block-title">
                    <h2>Portal Settings</h2>
                </div>
                <div class="form-horizontal">
                    <rpt:ClientPortalUserRole ID="rptClientPortalUserRole" runat="server" IsRequired="true" ValidationGroup="vgContact" />
                </div>
            </div>
            <div class="block">
                <div class="block-title">
                    <h2>Notification Preferences</h2>
                </div>
                <div class="form-horizontal">
                    <asp:RadioButtonList ID="rblNotificationPreferences" RepeatDirection="Vertical" runat="server">
                        <asp:ListItem Text="&nbsp&nbspTicket Notifications ON" Value="on" Selected="True" />
                        <asp:ListItem Text="&nbsp&nbspTicket Notifications OFF" Value="off" />
                        <asp:ListItem Text="&nbsp&nbspTicket Notifications only on BBB updates" Value="not"/>
                    </asp:RadioButtonList>
                </div>
            </div>

            <div class="block">
                <div class="block-title">
                    <h2>VIP</h2>
                </div>
                
            </div>
            <asp:PlaceHolder ID="phLocations" runat="server" Visible="false">
                <div class="block">
                    <div class="block-title">
                        <h2>Locations</h2>
                    </div>
                    <div class="row">
                        <div class="form-group col-sm-6">
                            <label>Primary <span class="text-danger">*</span></label>
                            <ddl:ClientLocation ID="ddlClientLocation" runat="server" IsRequired='<%# chkPortalActive.Checked == false ? false : true %>' ValidationGroup="vgContact" ErrorMessage="Primary Location is required" />
                        </div>
                        <div class="form-group col-sm-6">
                            <label>Secondary</label>
                            <lb:ClientLocation ID="lbClientLocation" runat="server" />
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phEmailCategory" runat="server">
                <div class="block">
                    <div class="block-title">
                        <h2>Client Email Categories</h2>
                    </div>
                    <div class="row">
                        <div class="form-group col-sm-12">
                            <div class="row topMargin5">
                                <div class="col-md-1"><strong>Category</strong></div>
                                <div class="col-md-3">
                                    <%--<ddl:ClientEmailCategory ID="ddlClientEmailCategory" runat="server" IsRequired="true" Active="true" Available="CC" DisplayChosenScript="true" ValidationGroup="vgContact" />--%>
                                    <asp:DropDownList ID="ddlClientEmailCategory" runat="server" CssClass="form-control select-chosen" />
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnAddClientEmailCategory" Text="Add Email Category" OnClick="btnAddClientEmailCategory_Click" runat="server" />
                                </div>
                                <div class="col-md-6">
                                    <telerik:RadGrid ID="rgEmailCategory" runat="server" Skin="3b" OnNeedDataSource="rgEmailCategory_NeedDataSource" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                                        AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%" OnItemCommand="rgEmailCategory_ItemCommand">
                                        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" AllowSorting="true">
                                            <NoRecordsTemplate>
                                                <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Action">
                                                    <ItemTemplate>
                                                        <asp:ImageButton CommandArgument="DeleteRecordEmailCategory" ImageUrl="~/Images/delete.png" runat="server" ID="btnDeleteRecord" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="Id" Display="false" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Id" SortExpression="Id" UniqueName="Id" />
                                                <telerik:GridBoundColumn DataField="CategoryName" DataType="System.String" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Assigned Category" SortExpression="CategoryName" UniqueName="CategoryName" />
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" />
                                    </telerik:RadGrid>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>
            
            <asp:PlaceHolder ID="phButtons" runat="server">
                <div class="block">
                    <div class="form-group form-actions">
                        <asp:LinkButton ID="btnSubmit" runat="server" ValidationGroup="vgContact" OnClick="btnSubmit_Click" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
                        <asp:LinkButton ID="btnResetPassword" OnClick="btnResetPassword_Click" runat="server" ValidationGroup="vgContact" CausesValidation="true" CssClass="btn btn-sm btn-primary"
                            OnClientClick="if (!confirm('Confirm Reset Password')) {return false;}" Visible="false" style="margin-left:15px;"><i class="gi gi-keys"></i> Reset Password</asp:LinkButton>
                        <asp:LinkButton ID="btnPortalInvite" OnClick="btnPortalInvite_Click" runat="server" ValidationGroup="vgContact" CausesValidation="true" CssClass="btn btn-sm btn-primary"
                            OnClientClick="if (!confirm('Confirm Invite To Portal')) {return false;}" Visible="false" style="margin-left:15px;"><i class="gi gi-message_out"></i> Invite To Portal</asp:LinkButton>
                        <%--<asp:LinkButton ID="btnPortalBilling" OnClick="btnPortalBilling_Click" runat="server" ValidationGroup="vgContact" CausesValidation="true" CssClass="btn btn-sm btn-primary"
                            OnClientClick="if (!confirm('Confirm Send Portal Billing Email')) {return false;}" Visible="false" style="margin-left:15px;"><i class="gi gi-money"></i> Send Portal Billing Email</asp:LinkButton>--%>
                        <asp:LinkButton ID="btnShowPortalBillingLink" OnClick="btnShowPortalBillingLink_Click" runat="server" ValidationGroup="vgContact" CausesValidation="true" CssClass="btn btn-sm btn-primary"
                            Visible="false" style="margin-left:15px;"><i class="gi gi-link"></i> Show Portal Billing Link</asp:LinkButton>
                        <asp:LinkButton ID="btnSetQWSync" runat="server" ValidationGroup="vgContact"  OnClick="btnSetQWSync_Click" CausesValidation="false" CssClass="btn btn-sm btn-primary pull-right">Add contact to the next QuoteWerks Sync</asp:LinkButton>
                    </div>
                    <div class="form-group form-actions">
                        <asp:LinkButton ID="lbSendInvite" OnClick="lbSendInvite_Click" runat="server" ValidationGroup="vgContact" CausesValidation="true" CssClass="btn btn-sm btn-primary"
                            OnClientClick="if (!confirm('Send Invitation')) {return false;}" ><i class="gi gi-message_out"></i> Send Client Setup Screen</asp:LinkButton>
                    </div>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phPortalBillingLink" runat="server" Visible="false">
                <div class="block" style="padding-bottom:15px;">
                    <div class="block-title">
                        <h2>Portal Billing Link</h2>
                    </div>
                    <div class="row">
                        <div class="form-group col-sm-12 form-control-static">
                            <asp:Literal ID="litPortalBillingLink" runat="server" />
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="phButtonSendHistory" runat="server" Visible="true">
                <div class="block">
                    <div class="row">
                        <telerik:RadGrid ID="rgButtonSendHistory" runat="server" Skin="3b" OnNeedDataSource="rgButtonSendHistory_NeedDataSource" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                            AllowPaging="true" ShowFooter="false" PageSize="10" Width="100%">
                            <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" AllowSorting="true">
                                <NoRecordsTemplate>
                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Id" Display="false" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Id" SortExpression="Id" UniqueName="Id" />
                                    <telerik:GridBoundColumn DataField="buttonName" DataType="System.String" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Button Name" SortExpression="buttonName" UniqueName="buttonName" />
                                    <telerik:GridBoundColumn DataField="emailAddress" DataType="System.String" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Email Address" SortExpression="emailAddress" UniqueName="emailAddress" />
                                    <telerik:GridBoundColumn DataField="CreatedDate" DataType="System.String" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Date/Time" SortExpression="CreatedDate" UniqueName="CreatedDate" />
                                    <telerik:GridBoundColumn DataField="CreatedByName" DataType="System.String" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="User who send email" SortExpression="CreatedByName" UniqueName="CreatedByName" />
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" />
                        </telerik:RadGrid>
                    </div>
                </div>
            </asp:PlaceHolder>

        </asp:Panel>
    <%--</telerik:RadAjaxPanel>--%>
    <%--<telerik:RadAjaxLoadingPanel ID="ralpMenu" runat="server" Transparency="70" BackColor="#69b899">
        <i class="fa fa-spinner fa-4x fa-spin"></i>
    </telerik:RadAjaxLoadingPanel>--%>
    <div id="myModalDelete" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalDeleteLabel"><asp:Label ID="lblModalDeleteTitle" runat="server" /></h4>
                </div>
                    <div class="modal-body">
                    <h3><asp:Label ID="lblModalDeleteWording" runat="server" /></h3>
                        <asp:HiddenField ID="hfDelete" runat="server" />
                        <asp:HiddenField ID="hfCategoryname" runat="server" />
                        <asp:HiddenField ID="hfDeleteFor" runat="server" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnYes" runat="server" Text="Delete" class="btn btn-primary" OnCommand="Decision_Command" CommandArgument="Delete" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="content3" ContentPlaceHolderID="cphEnd" runat="server">=
</asp:Content>
