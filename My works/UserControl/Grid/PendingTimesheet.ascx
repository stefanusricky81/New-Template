<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PendingTimesheet.ascx.cs" Inherits="UserControl_Grid_PendingTimesheet" %>
<%@ Reference VirtualPath="~/UserControl/Grid/EditForm/PendingTimesheetEdit.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<%@ Register TagPrefix="ddl" TagName="ProjectTask" Src="~/UserControl/DropDownList/ProjectTask.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TaskAction" Src="~/UserControl/DropDownList/TaskAction.ascx" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<%@ Register TagPrefix="ddl" TagName="SmartClient" Src="~/UserControl/DropDownList/SmartClient.ascx"%>
<telerik:RadCodeBlock ID="rcbMain" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            SetUpControls();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            SetUpControls();
        });

        function SetUpControls() {
             setTimeout(function() { $('[id$=ddlProjectTask]').focus() }, 1500);
        }
    </script>
</telerik:RadCodeBlock>
<style>
    .RadGrid_3b .rgEditForm a { color:#ffffff; }
    .RadGrid_3b .rgEditForm {margin: -1px; padding: 7px; border-bottom: 1px solid #dddddd; background-color:#f2f2f2; }
    .chosen-single span {color:#000 !important;}
    .btn-primary {color:#fff !important; }
    .no-height {}
</style>
<asp:Literal ID="litMessage" runat="server" Visible="false" />
<asp:ValidationSummary ID="vsTimesheet" runat="server" CssClass="ValidationSummary" ValidationGroup="vgTimesheet" />
<telerik:RadGrid ID="rgTimesheet" runat="server" Skin="3b" EnableEmbeddedSkins="false"
    OnNeedDataSource="rgTimesheet_NeedDataSource" 
    OnItemDataBound="rgTimesheet_ItemDataBound" 
    OnUpdateCommand="rgTimesheet_UpdateCommand" 
    OnInsertCommand="rgTimesheet_InsertCommand"
    OnDeleteCommand="rgTimesheet_DeleteCommand" 
    OnPreRender="rgTimesheet_PreRender" 
    OnItemCreated="rgTimesheet_ItemCreated" 
    OnItemCommand="rgTimesheet_ItemCommand" 
    AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="true" ShowFooter="true" PageSize="25" Width="100%" >
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" CommandItemDisplay="Top" EditMode="InPlace" InsertItemDisplay="Bottom">
        <CommandItemTemplate>        
            <div style="padding:10px;">
                <asp:LinkButton ID="btnAdd" runat="server" CommandName="AddViaUserControl"><i class="gi gi-circle_plus" title="Add Pending Timesheet"></i>  Add Pending Timesheet</asp:LinkButton>
            </div>
        </CommandItemTemplate> 
        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/PendingTimesheetEdit.ascx" EditFormType="WebUserControl">
            <EditColumn UniqueName="EditCommandColumn1"></EditColumn>
        </EditFormSettings>
        <NoRecordsTemplate>
            <div style="padding:10px;">No records to display.</div>
        </NoRecordsTemplate>
        <Columns>
            <telerik:GridTemplateColumn UniqueName="Checked">
                <ItemTemplate>
                    <asp:CheckBox ID="chkSubmit" runat="server" Checked="true"/>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn DataField="Id" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="6%" HeaderText="" UniqueName="ActionColumn">
                <ItemTemplate>
                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit"><i class="gi gi-search" title="Edit" style="padding-right:10px;"></i></asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="if (!confirm('Are you sure you want to delete this entry?')) return false;" 
                        CommandArgument='<%# Eval("Id") %>' CausesValidation="false"> <i class="gi gi-circle_remove" title="Delete"></i></asp:LinkButton>
                    <asp:LinkButton ID="btnSubmitEdit" runat="server" CausesValidation="true" ValidationGroup="vgTimesheet" CommandName="Update" Visible="false"><i class="hi hi-ok" title="Submit" style="padding-right:10px;"></i></asp:LinkButton>
                    <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CommandName="Cancel" Visible="false"><i class="hi hi-remove" title="Cancel Edit"></i></asp:LinkButton>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn UniqueName="Date" DataField="Date" SortExpression="Date" DataType="System.DateTime" HeaderText="Date" HeaderTooltip="Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Left">
                <EditItemTemplate>
                    <uc:DatePicker ID="ucDate" runat="server" IsRequired="true" ValidationGroup="vgTimesheet" PlaceHolderText="Select Date ..." TabIndex="0" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <uc:DatePicker ID="ucDate" runat="server" IsRequired="true" ValidationGroup="vgTimesheet" PlaceHolderText="Select Date ..." TabIndex="0" />
                </InsertItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn UniqueName="ClientName" DataField="ClientName" SortExpression="ClientName" DataType="System.String" HeaderText="Client" HeaderTooltip="Client" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Left">
                <InsertItemTemplate>
                    <ddl:SmartClient ID="ddlSmartClient" AutoPostback="true" Width="100%" runat="server" />
                </InsertItemTemplate>
                <EditItemTemplate>
                    <ddl:SmartClient ID="ddlSmartClient" AutoPostback="true" Width="100%" runat="server" />
                </EditItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn UniqueName="ProjectName" DataField="ProjectName" SortExpression="ProjectName" DataType="System.String" HeaderText="Project" HeaderTooltip="Project" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="14%" ItemStyle-HorizontalAlign="Left">
                <EditItemTemplate>
                    <ddl:ProjectTask ID="ddlProjectTask" runat="server" IsRequired="true" ValidationGroup="vgTimesheet" DisplayDefaultValue="true" DefaultText="" DefaultValue="" DisplayErrorAsterisk="false" ErrorMessage="Project is required" 
                        validatorDisplay="None" CssClass="form-control" TabIndex="1" ClientMaxLength="10" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <ddl:ProjectTask ID="ddlProjectTask" runat="server" IsRequired="true" ValidationGroup="vgTimesheet" DisplayDefaultValue="true" DefaultText="Select Project" DefaultValue="" DisplayErrorAsterisk="false" ErrorMessage="Project is required" 
                        validatorDisplay="None" CssClass="form-control" TabIndex="1" ClientMaxLength="10" />
                </InsertItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn UniqueName="TaskActionName" DataField="TaskActionName" SortExpression="TaskActionName" DataType="System.String" HeaderText="Activity" HeaderTooltip="Activity" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Left">
                <EditItemTemplate>
                    <ddl:TaskAction ID="ddlTaskAction" runat="server" IsRequired="true" ValidationGroup="vgTimesheet" DisplayDefaultValue="true" DefaultText="" DefaultValue="" DisplayErrorAsterisk="false" ErrorMessage="Activity is required" 
                        validatorDisplay="None" CssClass="form-control" TabIndex="2"/>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <ddl:TaskAction ID="ddlTaskAction" runat="server" IsRequired="true" ValidationGroup="vgTimesheet" DisplayDefaultValue="true" DefaultText="" DefaultValue="" DisplayErrorAsterisk="false" ErrorMessage="Activity is required" 
                        validatorDisplay="None" CssClass="form-control" TabIndex="2"/>
                </InsertItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn UniqueName="TimeSpentMinutes" DataField="TimeSpentMinutes" SortExpression="TimeSpentMinutes" DataType="System.Int32" HeaderText="Time" HeaderTooltip="Time" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Left">
                <EditItemTemplate>
                    <asp:TextBox ID="txtTimeSpent" runat="server" CssClass="form-control" MaxLength="3" TabIndex="3" />
                    <asp:RequiredFieldValidator ID="rfvTimeSpent" runat="server" ControlToValidate="txtTimeSpent" ValidationGroup="vgTimesheet" ErrorMessage="Time In Minutes is required" Display="None"/>
                    <asp:RangeValidator ID="rvTimeSpent" runat="server" ControlToValidate="txtTimeSpent" ValidationGroup="vgTimesheet" ErrorMessage="Invalid Time In Minutes" Display="None" Type="Integer" MaximumValue="1000" MinimumValue="5" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="txtTimeSpent" runat="server" CssClass="form-control" MaxLength="3" PlaceHolder="Enter Time ..." TabIndex="3" />
                    <asp:RequiredFieldValidator ID="rfvTimeSpent" runat="server" ControlToValidate="txtTimeSpent" ValidationGroup="vgTimesheet" ErrorMessage="Time In Minutes is required" Display="None"/>
                    <asp:RangeValidator ID="rvTimeSpent" runat="server" ControlToValidate="txtTimeSpent" ValidationGroup="vgTimesheet" ErrorMessage="Invalid Time In Minutes" Display="None" Type="Integer" MaximumValue="1000" MinimumValue="5" />
                </InsertItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn UniqueName="InternalNotes" DataField="InternalNotes" SortExpression="InternalNotes" DataType="System.String" HeaderText="Internal Notes" HeaderTooltip="Internal Notes" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="18%" ItemStyle-HorizontalAlign="Left">
                <EditItemTemplate>
                    <asp:TextBox ID="txtInternalNotes" runat="server" CssClass="form-control" PlaceHolder="Enter Internal Notes ..." TabIndex="4" TextMode="MultiLine" Rows="1" Height="34px" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="txtInternalNotes" runat="server" CssClass="form-control" PlaceHolder="Enter Internal Notes ..." TabIndex="4" TextMode="MultiLine" Rows="1" Height="34px" />
                </InsertItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn UniqueName="Description" DataField="Description" SortExpression="Description" DataType="System.String" HeaderText="Description" HeaderTooltip="Description" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="18%" ItemStyle-HorizontalAlign="Left">
                <EditItemTemplate>
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtDescriptionEdit_TextChanged" TabIndex="5" TextMode="MultiLine"  Rows="1" Height="34px" />
                    <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" ValidationGroup="vgTimesheet" ErrorMessage="Description is required" Display="None" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtDescription_TextChanged" PlaceHolder="Enter Description ..." TabIndex="5" TextMode="MultiLine" Rows="1" Height="34px" />
                    <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" ValidationGroup="vgTimesheet" ErrorMessage="Description is required" Display="None" />
                </InsertItemTemplate>
            </telerik:GridTemplateColumn>
        </Columns>
    </MasterTableView>
    <ClientSettings EnableRowHoverStyle="true" AllowKeyboardNavigation="true">
        <Selecting AllowRowSelect="true" />
    </ClientSettings>
</telerik:RadGrid>
<asp:Literal ID="litDebug" runat="server" />