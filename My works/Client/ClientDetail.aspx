<%@ Page Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="ClientDetail.aspx.cs" Inherits="Client_Default" Title="" %>
<%@ Register TagPrefix="ddl" TagName="TicketType" Src="~/UserControl/DropDownList/TicketType.ascx" %>
<%@ Register TagPrefix="ctrl" TagName="ClientDetailTabs" Src="~/UserControl/Client/ClientDetailTabs.ascx" %>
<%@ Register TagPrefix="grid" TagName="ClientDomain" Src="~/UserControl/Grid/ClientDomain.ascx" %>
<%@ Register TagPrefix="ddl" TagName="EndpointPlatform" Src="~/UserControl/DropDownList/EndpointPlatform.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Country" Src="~/UserControl/DropDownList/Country.ascx" %>
<%@ Register TagPrefix="lb" TagName="ClientServices" Src="~/UserControl/ListBox/ClientServices.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        .topMargin5 {
            margin-top: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">

            function OnWindowClose(sender, eventArgs) {
                //window.location.reload();
            }

            function openWindowCRM(sender, args) {
                var wd = window.radopen("/CRM/CRMHistoryPopUp.aspx?Id=" + args.get_commandArgument(), "CRMDialog");
            }
        </script>
    </telerik:RadCodeBlock>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="CRMDialog" runat="server" Title="CRM History"
                Height="670px"
                Width="850px" ReloadOnShow="true" CssClass="modalPopup" ShowContentDuringLoad="false" Behaviors="Close"
                Modal="true" OnClientClose="OnWindowClose">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <asp:Literal ID="litMessage" runat="server" Visible="false" />

    <ctrl:ClientDetailTabs ID="ClientTabs" runat="server" SelectedTabIndex="0" />
    <div class="row">
        <div class="col-md-12">
            <asp:PlaceHolder ID="phSearchResults" runat="server">
                <asp:Literal ID="litDebug" runat="server" />
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong>
                            <asp:Label ID="lblClientName" Visible="false" runat="server"></asp:Label>
                            <asp:Label ID="Label1" Text="Client Details" runat="server"></asp:Label>
                            <asp:ValidationSummary ID="vsClient" runat="server" ValidationGroup="vgClient" CssClass="validationSummary" />
                        </strong></h2>
                        <div class="pull-right" style="padding-right: 15px; padding-top: 10px;">
                            <asp:HyperLink ID="hlBacktoList" NavigateUrl="~/Client/ClientSearch.aspx" runat="server"><i class="gi gi-list" title="Back to Search list"></i>  Back to Search list</asp:HyperLink>
                            &nbsp;&nbsp;<asp:HyperLink ID="hlGoToOldDetail" runat="server"><i class="fa fa-user" title="Go to Old Client Detail"></i>  Go to Old Details</asp:HyperLink>
                        </div>
                        <asp:PlaceHolder ID="phScrollNotes" runat="server"></asp:PlaceHolder>
                    </div>
                    <div class="Container">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row topMargin5">
                                    <div class="col-md-2"></div>
                                    <div class="col-md-10">
                                        <asp:LinkButton ID="btnSetQWSync" runat="server" ValidationGroup="vgMenu" OnClick="btnSetQWSync_Click" CausesValidation="false" CssClass="btn btn-sm btn-success">Add contacts to the next QuoteWerks Sync</asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="btnITGlueSync" runat="server" ValidationGroup="vgMenu" OnClick="btnITGlueSync_Click" CausesValidation="false" CssClass="btn btn-sm btn-success">Add contacts to IT Glue</asp:LinkButton>&nbsp;&nbsp;
                                        <telerik:RadButton ID="rbCRMHistory" runat="server" AutoPostBack="false" OnClientClicked="openWindowCRM" Text="CRM History" ToolTip="Pop up CRM History" ButtonType="LinkButton" CssClass="btn btn-primary" BackColor="#27ae60" ForeColor="White" BorderColor="DarkGreen" Height="30"></telerik:RadButton>
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Company Name <span class="text-danger">*</span></strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="lblCompanyName" MaxLength="50" CssClass="form-control" runat="server"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="lblCompanyName" ErrorMessage="Company is required" Display="None" ValidationGroup="vgClient" />--%>
                                        <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="lblCompanyName" ErrorMessage="Company is required" Display="None" ValidationGroup="vgClient"/>
                                    </div>
                                    <div class="col-md-2"><strong>City</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="lblCity" MaxLength="18" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Client Code <span class="text-danger">*</span></strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="lblClientCode" MaxLength="10" CssClass="form-control" runat="server"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="rfvClientCode" runat="server" ControlToValidate="lblClientCode" ErrorMessage="Client Code is required" Display="None" ValidationGroup="vgClient" />--%>
                                        <%--<asp:RequiredFieldValidator ID="rfvClientCode" runat="server" ControlToValidate="lblClientCode" ErrorMessage="Client Code is required" Display="None" ValidationGroup="vgClient" />--%>
                                        <asp:CustomValidator ID="cvRequiredClientCode" runat="server" ControlToValidate="lblClientCode" ErrorMessage="Client Code is required" Display="None" ValidationGroup="vgClient" />
                                        <asp:CustomValidator ID="cvClientCode" runat="server" ControlToValidate="lblClientCode" ErrorMessage="Client Code is already in use" Display="None" ValidationGroup="vgClient" OnServerValidate="cvClientCode_ServerValidate" />
                                    </div>
                                    <div class="col-md-2"><strong>Country</strong></div>
                                    <div class="col-md-4">
                                        <ddl:Country ID="ddlCountry" runat="server" CssClass="form-control select-chosen" DefaultText="United States" />
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2">
                                        <strong>Address 1</strong>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="lblAddress" MaxLength="30" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"><strong><asp:Label ID="lbState" runat="server" Text="State" /></strong></div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control select-chosen"></asp:DropDownList>
                                        <asp:TextBox ID="txtState" runat="server" CssClass="form-control" Visible="false" />
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2">
                                        <strong>Address 2</strong>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="lblAddress2" MaxLength="30" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"><strong><asp:Label ID="lbZip" runat="server" Text="Zip" /></strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="lblZip" MaxLength="10" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Fax</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="lblFax" CssClass="form-control" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txtFax" CssClass="form-control" runat="server" Visible="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"><strong>Phone</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="lblPhone" CssClass="form-control" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txtPhone" CssClass="form-control" runat="server" Visible="false" />
                                    </div>
                                    
                                </div>
                                <div class="row topMargin5">
                                    <asp:PlaceHolder ID="phLastUpdated" runat="server">
                                        <div class="col-md-2"><strong>Last Updated</strong></div>
                                        <div class="col-sm-4"><p class="form-control-static"><asp:Literal ID="litLastUpdated" runat="server" /></p></div>
                                    </asp:PlaceHolder>
                                    <div class="col-md-2"><strong>URL</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtURL" MaxLength="40" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong>
                            <asp:Label ID="Label3" Text="Client Status" runat="server"></asp:Label>
                        </strong></h2>
                    </div>
                    <div class="Container">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Active</strong></div>
                                    <div class="col-md-4">
                                        <asp:CheckBox ID="chkActive" Checked="true" Text="" runat="server" CssClass="form-control form-control-borderless"/>
                                    </div>
                                     <div class="col-md-2"><strong>Documentation URL</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtDocumentationURL" MaxLength="50" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Priority <span class="text-danger">*</span></strong></div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control select-chosen" />
                                        <%--<asp:RequiredFieldValidator ID="rfvPriority" runat="server" ControlToValidate="ddlPriority" ErrorMessage="Priority is required" Display="None" ValidationGroup="vgClient" />--%>
                                        <%--<asp:RequiredFieldValidator ID="rfvPriority" runat="server" ControlToValidate="ddlPriority" ErrorMessage="Priority is required" Display="None" ValidationGroup="vgClient"  />--%>
                                        <asp:CustomValidator ID="cvRequiredPriority" runat="server" ControlToValidate="ddlPriority" ErrorMessage="Priority Code is required" Display="None" ValidationGroup="vgClient" />
                                    </div>
                                    <div class="col-md-2"><strong>Dark Web</strong></div>
                                    <div class="col-md-4">
                                        <asp:CheckBox ID="chkDarkWeb" Text="" runat="server" CssClass="form-control form-control-borderless"/>
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Accounting Status</strong></div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlAccStatus" runat="server" CssClass="form-control select-chosen">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2"><strong>Help Desk Client</strong></div>
                                    <div class="col-md-4">
                                        <asp:CheckBox ID="chkHelpDesk" Checked="false" Text="" runat="server" CssClass="form-control form-control-borderless"/>
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Accounting Comments</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtAccountingComments" MaxLength="50" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"><strong>Default Ticket Type</strong></div>
                                    <div class="col-md-4">
                                        <ddl:TicketType ID="ddlTicketType" runat="server" IsRequired="false" CssClass="form-control" DisplayChosenScript="true" />
                                    </div>
       <%--                                <div class="col-md-2"><strong>Auto CC on Ticket Update</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtAutoCC" MaxLength="500" CssClass="form-control" runat="server"></asp:TextBox>
                                        <asp:CustomValidator ID="cvAutoCc" runat="server" ValidateEmptyText="false" ErrorMessage="Invalid Auto CC" ValidationGroup="vgClient" OnServerValidate="cvAutoCc_ServerValidate" Display="None" />
                                    </div>--%>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Hourly Rate</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtHourlyRate" MaxLength="10" CssClass="form-control" runat="server"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="revHourlyRate" runat="server" ControlToValidate="txtHourlyRate" ValidationGroup="vgClient" ValidationExpression="\d+(\.\d{1,2})?" Display="Dynamic" ErrorMessage="Hourly Rate must be numeric. Ex. 150 or 150.00"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2"><strong>Backup Monitor</strong></div>
                                    <div class="col-md-4">    
                                        <asp:CheckBox ID="chkBackupMonitor" runat="server" CssClass="form-control form-control-borderless"/>
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Rec ID</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtRecID" MaxLength="50" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"><strong>Endpoint</strong></div>
                                    <div class="col-md-4">
                                        <ddl:EndpointPlatform ID="ddlEndpointPlatform" runat="server" IsRequired="false" CssClass="form-control" DisplayChosenScript="true" ShowDefaultEntry="true" />
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>SentinelOne Name</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtSentinelOneSiteName" MaxLength="100" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"><strong>Splashtop Name</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtSplashtopName" MaxLength="100" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Payment Method <span class="text-danger">*</span></strong></div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlBillingType" runat="server" CssClass="form-control select-chosen">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Credit Card" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="ACH" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Invoice" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvBillingType" runat="server" ControlToValidate="ddlBillingType" ErrorMessage="Billing Type is required" Display="None" ValidationGroup="vgClient" />
                                    </div>
                                    <div class="col-md-2"><strong>Exclude from Reports</strong></div>
                                    <div class="col-md-4">
                                        <asp:CheckBox ID="cbExcludeFromReports" runat="server" />
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Arctic Wolf Name</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtArcticWolfName" MaxLength="100" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"><strong>Patching Report</strong></div>
                                    <div class="col-md-4">
                                        <asp:CheckBox ID="cbPatchingReport" runat="server" />
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Client Job Number</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtClientJobNumber" MaxLength="10" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"><strong>Client Type<span class="text-danger">*</span></strong></div>
                                    <div class="col-md-4">
                                        <asp:ListBox runat="server" ID="lbClientType" CssClass="form-control select-chosen" Rows="10" SelectionMode="Multiple"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="rfvClientType" runat="server" ControlToValidate="lbClientType" ErrorMessage="Client Type is required" Display="None" ValidationGroup="vgClient" />
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <%--<div class="col-md-2"><strong>BBB Client Services</strong></div>
                                    <div class="col-md-4">
                                        <lb:ClientServices ID="lbClientService" runat="server" DisplayChosenScript="true" />
                                    </div>--%>
                                    <div class="col-md-2"><strong>Managed Fax Reports</strong></div>
                                    <div class="col-md-4">
                                         <asp:CheckBox ID="chkPortalManagedFaxReport" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong>
                            <asp:Label ID="Label5" Text="Client Auto CC" runat="server"></asp:Label>
                        </strong></h2>
                    </div>
                    <div class="Container">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Auto CC on Ticket Update</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtAutoCC" MaxLength="500" CssClass="form-control" runat="server"></asp:TextBox>
                                        <label style="color:red">Type in single address per entry</label>
                                        <%--<asp:CustomValidator ID="cvAutoCc" runat="server" ValidateEmptyText="false" ErrorMessage="Invalid Auto CC" ValidationGroup="vgClient" OnServerValidate="cvAutoCc_ServerValidate" Display="None" />--%>
                                    </div>
                                    <div class="col-md-6">
                                        <telerik:RadGrid ID="rgAutoClientCC" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                                            AllowPaging="true" ShowFooter="false" PageSize="5" Width="100%" OnNeedDataSource="rgAutoClientCC_NeedDataSource" OnItemCommand="rgAutoClientCC_ItemCommand">
                                            <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" AllowSorting="true">
                                                <NoRecordsTemplate>
                                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                                </NoRecordsTemplate>
                                                <Columns>
                                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderText="" UniqueName="Name">
                                                        <ItemTemplate>
                                                            <asp:ImageButton CommandArgument="DeleteRecord" ImageUrl="~/Images/delete.png" runat="server" ID="btnDeleteRecord" />
                                                            <%--<asp:LinkButton ID="btnDeleteRecord" runat="server" CommandArgument="DeleteRecord" > <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>--%>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="Id" Display="false" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Id" SortExpression="Id" UniqueName="Id" />
                                                    <telerik:GridBoundColumn DataField="Email" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Email Address" SortExpression="Email" UniqueName="Email" />
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true" />
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2">
                                        <asp:LinkButton ID="lbAutoCC" OnClick="lbAutoCC_Click" runat="server" ValidationGroup="VgAutoCC" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-ok"></i> Add</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong>
                            <asp:Label ID="Label2" Text="Account Reps" runat="server"></asp:Label>
                        </strong></h2>
                    </div>
                    <div class="Container">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="row topMargin5">
                                    <div class="col-md-3"><strong>Lead tech</strong></div>
                                    <div class="col-md-9">
                                        <asp:DropDownList ID="ddlTechnicalLead" runat="server" CssClass="form-control select-chosen" />
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-3"><strong>TAM</strong></div>
                                    <div class="col-md-9">
                                        <asp:DropDownList ID="ddlTAM" runat="server" CssClass="form-control select-chosen" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="row topMargin5">
                                    <div class="col-md-3"><strong>Sales Person <span class="text-danger">*</span></strong></div>
                                    <div class="col-md-9">
                                        <asp:ListBox runat="server" ID="ddlSalesPerson" CssClass="form-control select-chosen" Rows="10" SelectionMode="Multiple"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="rfvSalesPerson" runat="server" ControlToValidate="ddlSalesPerson" ErrorMessage="Sales Person is required" Display="None" ValidationGroup="vgClient" />
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-3"><strong>Client Sucess Manager</strong></div>
                                    <div class="col-md-9">
                                        <asp:DropDownList ID="ddlProjectManager" runat="server" CssClass="form-control select-chosen" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong>
                            <asp:Label ID="Label4" Text="Notes" runat="server"></asp:Label>
                        </strong></h2>
                    </div>
                    <div class="Container">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>Notes</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtNotes" TextMode="MultiLine" CssClass="form-control" runat="server" />
                                    </div>
                                    <div class="col-md-2"><strong>Directions</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtDirections" TextMode="MultiLine" CssClass="form-control" runat="server" />
                                    </div>
                                </div>
                                <div class="row topMargin5">
                                    <div class="col-md-2"><strong>What we do</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtWhatwedo" TextMode="MultiLine" CssClass="form-control" runat="server" />
                                    </div>
                                    <div class="col-md-2"><strong>What we can do</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtWhatwecando" TextMode="MultiLine" CssClass="form-control" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:PlaceHolder ID="phDomains" runat="server">
                    <div class="block" style="padding-bottom:20px;">
                        <div class="block-title">
                            <h2><strong>Domains</strong></h2>
                            <a href="#modal-domains-help" data-toggle="modal"><i class="gi gi-circle_question_mark" title="View Help for Domains" style="padding-bottom:8px;"></i></a>
                        </div>
                        <grid:ClientDomain ID="gridClientDomain" runat="server" />
                    </div>
                    <asp:CustomValidator ID="cvDomains" runat="server" ValidationGroup="vgClient" ErrorMessage="At least 1 active domain must be associated with client" Display="None" OnServerValidate="cvDomains_ServerValidate" />
                </asp:PlaceHolder>
                <div class="block">
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group form-actions">
                                <asp:LinkButton ID="btnSave" OnClick="btnSave_Click" runat="server" ValidationGroup="vgClient" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-ok"></i> Submit</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="modal-domains-help" class="modal" tabindex="-1" role="dialog" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h3 class="modal-title">Domains Help</h3>
                            </div>
                            <div class="modal-body">
                                Tickets emailed from users with these domains will be assigned to this client.
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>
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
<asp:Content ID="content4" ContentPlaceHolderID="cphEnd" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= lblPhone.ClientID %>").mask("(999) 999-9999");
            $("#<%= lblFax.ClientID %>").mask("(999) 999-9999");
        });
    </script>
</asp:Content>
