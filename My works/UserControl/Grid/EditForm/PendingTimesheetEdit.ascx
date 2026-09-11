<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PendingTimesheetEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_PendingTimesheetEdit" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<%@ Register TagPrefix="ddl" TagName="ProjectTask" Src="~/UserControl/DropDownList/ProjectTask.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TaskAction" Src="~/UserControl/DropDownList/TaskAction.ascx" %>
<%@ Register TagPrefix="ddl" TagName="SmartClient" Src="~/UserControl/DropDownList/SmartClient.ascx"%>
<style>
    .RadGrid_3b .rgEditForm a { color:#ffffff; }
    .RadGrid_3b .rgEditForm {margin: -1px; padding: 7px; border-bottom: 1px solid #dddddd; background-color:#f2f2f2; }
    .chosen-single span {color:#000 !important;}
</style>
<div class="block" style="min-height:300px;">
    <div class="block-title">
        <h2>
            <strong><asp:Literal ID="litHeader" runat="server" Text="Add Pending Timesheet" /></strong>
        </h2>
    </div>
    <div class="form-horizontal form-bordered">
        <asp:Literal ID="litDebug" runat="server" />
        <asp:ValidationSummary ID="vsTimesheet" runat="server" CssClass="ValidationSummary" ValidationGroup="vgTimesheetUc" />
        <div class="form-group">
            <label class="col-sm-3 control-label">Date</label>
            <div class="col-sm-3 col-md-6">
                <uc:DatePicker ID="ucDate" runat="server" IsRequired="true" ValidationGroup="vgTimesheetUc" PlaceHolderText="Select Date ..." />
            </div>
        </div>
        <asp:PlaceHolder ID="phClient" runat="server" Visible="true">
            <div class="form-group">
                <label class="col-sm-3 control-label">Client</label>
                <div class="col-sm-9 col-md-6">
                    <%--<ddl:Client ID="ddlClient" runat="server" IsRequired="true" ValidationGroup="vgTimesheetUc" DisplayDefaultValue="true" DefaultText="" DefaultValue="" />--%>
                    <ddl:SmartClient ID="ddlSmartClient" style="height=20px" Width="100%" AutoPostback="true" runat="server" />
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="form-group">
            <label class="col-sm-3 control-label">Project</label>
            <div class="col-sm-9 col-md-6">
                <ddl:ProjectTask ID="ddlProjectTask" runat="server" IsRequired="true" ValidationGroup="vgTimesheetUc" DisplayDefaultValue="true" DefaultText="" DefaultValue="" DisplayErrorAsterisk="false" ErrorMessage="Project is required" validatorDisplay="None" ClientMaxLength="10"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-3 control-label">Activity</label>
            <div class="col-sm-9 col-md-6">
                <ddl:TaskAction ID="ddlTaskAction" runat="server" IsRequired="true" ValidationGroup="vgTimesheetUc" DisplayDefaultValue="true" DefaultText="" DefaultValue="" DisplayErrorAsterisk="false" ErrorMessage="Activity is required" validatorDisplay="None"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-3 control-label">Time</label>
            <div class="col-sm-3">
                <asp:DropDownList ID="ddlTimeHour" runat="server" />
                <asp:Literal ID="litChosenJsTimeHour" runat="server" />
            </div>
            <div class="col-sm-3">
                <asp:DropDownList ID="ddlTimeMinute" runat="server" />
                <asp:Literal ID="litChosenJsTimeMinute" runat="server" />
            </div>
             <asp:CustomValidator ID="cvTime" runat="server" ControlToValidate="ddlTimeHour" ValidateEmptyText="true" ValidationGroup="vgTimesheetUc" Display="None" ErrorMessage="Time is required" OnServerValidate="cvTime_ServerValidate" />
        </div>
        <div class="form-group">
            <label class="col-sm-3 control-label">Internal Notes</label>
            <div class="col-sm-9 col-md-6">
                <asp:TextBox runat="server" ID="txtInternalNotes" CssClass="form-control" Placeholder="Enter Internal Notes ..." TabIndex="91" TextMode="MultiLine" Rows="10"/>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-3 control-label">Description <span class="text-danger">*</span></label>
            <div class="col-sm-9 col-md-6">
                <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control" Placeholder="Enter Description ..." TabIndex="92" TextMode="MultiLine" Rows="10"/>
                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" ErrorMessage="Description is required" ValidationGroup="vgTimesheetUc" Display="None" />
            </div>
        </div>
    </div>
</div>
<div class="block">
    <div class="form-group form-actions">
        <asp:LinkButton ID="btnEdit" runat="server" ValidationGroup="vgTimesheetUc" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="Update" TabIndex="94"><i class="hi hi-ok"></i> Edit</asp:LinkButton>
        <asp:LinkButton ID="btnAdd" runat="server" ValidationGroup="vgTimesheetUc" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="PerformInsert" TabIndex="95"><i class="hi hi-plus"></i> Save</asp:LinkButton>
        <asp:LinkButton ID="btnAdd2" runat="server" ValidationGroup="vgTimesheetUc" CausesValidation="true" CssClass="btn btn-sm btn-primary" CommandName="PerformInsert" CommandArgument="SaveAdd" TabIndex="96"><i class="hi hi-plus"></i> Save & Add</asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" CssClass="btn btn-sm btn-warning" CommandName="Cancel"><i class="hi hi-remove" TabIndex="97"></i> Cancel</asp:LinkButton>
    </div>
</div>