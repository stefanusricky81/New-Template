<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" Async="true" AutoEventWireup="true" CodeFile="Calendar.aspx.cs" Inherits="Activity_Calendar" %>
<%@ Register TagPrefix="ddl" TagName="Employee" Src="~/UserControl/DropDownList/Employee.ascx" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<%@ Register TagPrefix="ddl" TagName="ProjectTask" src="~/UserControl/DropDownList/ProjectTask.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TaskAction" src="~/UserControl/DropDownList/TaskAction.ascx"  %>
<%@ Register TagPrefix="ddl" tagName="TimeDDL" src="~/UserControl/DropDownList/Time.ascx"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script type="text/javascript">
        function CheckAll(id) {
            var masterTable = $find("<%= rgCalendarEvents.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == true) {
                for (var i = 0; i < row.length; i++) {
                    masterTable.get_dataItems()[i].findElement("cboxSelect").checked = true; // for checking the checkboxes
                }
            }
            else {
                for (var i = 0; i < row.length; i++) {
                    masterTable.get_dataItems()[i].findElement("cboxSelect").checked = false; // for unchecking the checkboxes
                }
            }
        }
        function unCheckHeader(id) {
            var masterTable = $find("<%= rgCalendarEvents.ClientID %>").get_masterTableView();
            //accessing header checkbox
            var chkBox = $('input[id$="checkAll"]');
            chkBox[0].checked = false;
        }
        //function fixDropdownOverflow() {
        //    $('.select-chosen').each(function () {
        //        $(this).css({
        //            'overflow': 'visible',
        //            'position': 'relative',
        //            'z-index': '1000'
        //        });

        //        // If using Chosen plugin
        //        if ($(this).next('.chosen-container').length) {
        //            $(this).next('.chosen-container').css('overflow', 'visible');
        //            $(this).next('.chosen-container').find('.chosen-drop').css({
        //                'max-height': 'none',
        //                'overflow-y': 'visible'
        //            });
        //        }
        //    });
        //}

        //// Call this after grid loads
        //Sys.Application.add_load(function () {
        //    fixDropdownOverflow();
        //});
    </script>

    <style>
        .RadGrid_3b .RadComboBox .rcbInput {width:50px !important;}
        .RadGrid table.rgMasterTable tr.rgRow td {overflow: visible !important;}
        .RadGrid .rgClipCells .rgAltRow td {overflow:visible !important;}
/*        .RadGrid .select-chosen {
    z-index: 10000 !important;
}*/      
        /*.select-chosen {
    overflow: visible !important;
}*/
/* For the dropdown list */
/*.select-chosen option {
    white-space: normal !important;
}*/
/* If using Chosen plugin */
/*.select-chosen + .chosen-container .chosen-drop {
    max-height: none !important;
    overflow-y: visible !important;
}*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <asp:ValidationSummary ID="vsTicket" runat="server" CssClass="validationSummary" ValidationGroup="vgCalendar" />
            <asp:Literal ID="litMessage" runat="server" />
        <div class="block">
            <div class="block-title">
                <h2><strong>Search Criteria</strong></h2>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Start Date</label>
                            </div>                            
                        </div>
                        <div class="col-md-8">
                            <uc:DatePicker ID="ucCreatedStart" runat="server" IsRequired="false" />
                            <asp:TextBox ID="txt1" runat="server" Visible="false" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Employee</label>
                            </div>
                        </div>
                        <div class="col-md-8">
                            <div class="disable-dropdown">
                                <ddl:Employee ID="ddlEmployee" runat="server" Enabled="false" DisplayChosenScript="true" SetSize="false" />
                            </div>
                        </div>
                    </div>
                    
                </div>
                <div class="col-md-6">
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>End Date</label>
                            </div>                            
                        </div>
                        <div class="col-md-8">
                            <uc:DatePicker ID="ucEndDate" runat="server" IsRequired="false" />
                            <asp:CustomValidator ID="cvCreatedDates" runat="server" ControlToValidate="txt1" ValidateEmptyText="true" ValidationGroup="vgCalendar" 
                            ErrorMessage="Created End Date must be greater than Created Start Date" Display="None" OnServerValidate="cvCreatedDates_ServerValidate" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="form-group form-actions" style="margin-left:8px">
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgCalendar"><i class="hi hi-search"></i> Search</asp:LinkButton>
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                </div>
            </div>  
        </div>
        <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="false">

            <div class="block" style="padding-bottom: 20px; overflow-x:scroll; overflow:visible">
                <div class="block-title">
                    <h2><strong>Search Results</strong></h2>
                    <asp:PlaceHolder ID="phScrollNotes" runat="server"></asp:PlaceHolder>
                </div>
                <div class="row">
                    <asp:button id="btnAddTimeSheet" text="Add Timesheet" runat="server" ValidationGroup="vgAddTimesheet" OnCommand="Decision_Command" CssClass="btn btn-sm btn-primary" CommandName="Add" />
                </div>
                <br />
                <asp:PlaceHolder ID="phTopTimeSpentTotal" runat="server">
                    <p style="font-weight:bold;">Total Time spent: <asp:Literal ID="ltTopTimeSpent" runat="server" /></p>
                    <asp:LinkButton ID="lbRefresh" runat="server" CssClass="btn btn-sm btn-warning" OnClick="lbRefresh_Click" CausesValidation="false"><i class="hi hi-refresh"></i> Refresh</asp:LinkButton>
                </asp:PlaceHolder>
                <br />
                <br />
                <div class="table-responsive" style="overflow:visible !important">
                        <telerik:RadGrid ID="rgCalendarEvents" OnNeedDataSource="rgCalendarEvents_NeedDataSource" runat="server" Skin="3b" 
                            OnItemDataBound="rgCalendarEvents_ItemDataBound" OnPageIndexChanged="rgCalendarEvents_PageIndexChanged" OnPageSizeChanged="rgCalendarEvents_PageSizeChanged"
                            EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" OnSortCommand="rgCalendarEvents_SortCommand"
                            ShowFooter="false" Width="100%"> 
                            <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="ID" Width="100%">
                                <NoRecordsTemplate>
                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="checkAll" runat="server" Checked="true" onclick="CheckAll(this)" OnCheckedChanged="cboxSelect_CheckedChanged" AutoPostBack="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cboxSelect" runat="server" Checked="true" onclick="unCheckHeader(this)" OnCheckedChanged="cboxSelect_CheckedChanged" AutoPostBack="false"/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="ID" Display="false" HeaderStyle-Width="3%" DataType="System.Int16" ItemStyle-HorizontalAlign="Left" HeaderText="#" UniqueName="ID" />
                                    <telerik:GridBoundColumn DataField="Email" Display="false" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Organizer" SortExpression="Email" UniqueName="Email" HeaderTooltip="Email" />
                                    <telerik:GridBoundColumn DataField="Subject" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Subject" SortExpression="Subject" UniqueName="Subject" HeaderTooltip="Subject" />
                                    <telerik:GridBoundColumn DataField="StartDate" HeaderStyle-Width="9%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Date" SortExpression="StartDate" UniqueName="AllDate" HeaderTooltip="AllDate" />
                                    <telerik:GridBoundColumn DataField="StartDate" Display="false" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Start Date" SortExpression="StartDate" UniqueName="StartDate" HeaderTooltip="StartDate" />
                                    <telerik:GridBoundColumn DataField="EndDate" Display="false" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="End Date" SortExpression="EndDate" UniqueName="EndDate" HeaderTooltip="EndDate" />
                                    <telerik:GridTemplateColumn HeaderText="Duration" HeaderStyle-Width="9%" ItemStyle-Height="10px" DataType="System.String" ItemStyle-HorizontalAlign="Left" UniqueName="Duration" >
                                        <ItemTemplate>
                                            <ddl:TimeDDL ID="ddlTimeSpent" runat="server" StartHourAtZero="true" FifteenMinuteIncrements="true" DisplayDefaultValue="false" />
                                            <%--<asp:TextBox ID="txtDuration" ReadOnly="true" runat="server" CssClass="form-control" />--%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Client Project" HeaderStyle-Width="20%" ItemStyle-Height="10px" DataType="System.String" ItemStyle-HorizontalAlign="Left" UniqueName="ClientProject" >
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlProjectTask"  CssClass="form-control select-chosen" runat="server" />
                                            <asp:Literal ID="litJs" runat="server" Visible="false"/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Activity" HeaderStyle-Width="12%" ItemStyle-Height="10px" DataType="System.String" ItemStyle-HorizontalAlign="Left" >
                                        <ItemTemplate>
                                            <ddl:TaskAction ID="ddlTaskAction" CssClass="form-control" DefaultValue="" DefaultText="" runat="server" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Notes" HeaderStyle-Width="25%" ItemStyle-Height="10px" DataType="System.String" ItemStyle-HorizontalAlign="Left" UniqueName="Notes" >
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNotes"  CssClass="form-control" runat="server" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                <asp:PlaceHolder ID="phBottomTimeSpentTotal" runat="server">
                    <br /><p style="font-weight:bold;">Total Time spent: <asp:Literal ID="ltBottomTimeSpent" runat="server" /></p>
                </asp:PlaceHolder>
            </div>
        </asp:PlaceHolder>
    </asp:Panel>
</asp:Content>
