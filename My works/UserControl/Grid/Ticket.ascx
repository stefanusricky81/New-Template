<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Ticket.ascx.cs" Inherits="UserControl_Grid_Ticket" %>
<%@ Register TagPrefix="uc" TagName="TicketRecurring" Src="~/UserControl/DropDownList/TicketRecurring.ascx" %>
<telerik:radscriptblock id="RadScriptBlock1" runat="server">
    <script type="text/javascript">
        //On insert and update buttons click temporarily disables ajax to perform upload actions
        //also disable ajax for export button click
        function conditionalPostback(e, sender) {
            var theRegexp = new RegExp("\.btnUpdate$|\.btnAdd$|\.InitInsertButton$|\.lbMakeActive$", "ig"); 
            var theRegexp2 = new RegExp("\.btnExport$", "ig");
            
            if ((sender.EventTarget.match(theRegexp) && sender.get_eventTarget().indexOf("rgTicketFile") > -1) || (sender.EventTarget.match(theRegexp2)))
            {
                sender.EnableAjax = false;
            }
        }
    </script>
</telerik:radscriptblock>

<telerik:RadNotification ID="rnConfirmation" runat="server" Width="250" Height="100" EnableRoundedCorners="true" VisibleTitlebar="false" Pinned="false" Position="Center" Skin="Black" AnimationDuration="150" Animation="FlyIn"/>

<ajaxToolkit:CollapsiblePanelExtender ID="cpeTicket" 
    runat="Server"
    TargetControlID="pnlContainer"
    ExpandControlID="pnlShowHideTicket"
    CollapseControlID="pnlShowHideTicket" 
    Collapsed="False"
    BehaviorID="cpeBehaviorTicket"
    TextLabelID="lblTicketHeader"
    ImageControlID="imgTicketHeader"    
    ExpandedImage="~/images/collapse_blue.jpg"
    CollapsedImage="~/Images/expand_blue.jpg"
    SuppressPostBack="true" 
/>

<asp:Panel ID="pnlShowHideTicket" runat="server" CssClass="collapsePanelHeaderTicket" Height="25px">
    <div style="cursor: pointer; vertical-align: middle; padding:5px;">
        <div style="float: left;">
            <asp:Label ID="lblTicketHeader" runat="server" />
        </div>
        <div style="float: right; vertical-align: middle;">
            <asp:ImageButton ID="imgTicketHeader" runat="server" ImageUrl="~/images/expand_blue.jpg" />
        </div>
    </div>
</asp:Panel>
    
<asp:Panel ID="pnlContainer" runat="server">
    
    <asp:Panel ID="pnlHeader" runat="server">
        <asp:Table ID="tblGridHeader" runat="server" CellPadding="0" CellSpacing="0" CssClass="GridHeaderTable">
            <asp:TableRow>
                <asp:TableHeaderCell ID="tblGridHeaderCell1" runat="server" Width="20%">
                    <asp:Literal ID="litLastUpdated" runat="server" />
                </asp:TableHeaderCell>
                <asp:TableCell ID="tblGridHeaderCell2" runat="server" Width="20%">
                    Warning:&nbsp;
                    <asp:DropDownList ID="ddlLastUpdated" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="ddlLastUpdated_SelectedChange" Width="75">
                    </asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell ID="tblGridHeaderCell3" runat="server" Width="16%" Visible="false">
                    Views:&nbsp;
                    <uc:TicketViewDDL ID="ucTicketView" runat="server" ViewType="HomePage" 
                        DisplayDefaultValue="false" IsRequired="false" Width="150" Visible="false" HighlightUserViews="true"/>
                </asp:TableCell>
                <asp:TableCell ID="tblGridHeaderCell4" runat="server" Width="20%">
                    Tags:&nbsp;
                    <uc:UserTicketTagsDDL ID="ucUserTicketTags" runat="server" DisplayDefaultValue="true"
                        DefaultText="" DefaultValue="" IsRequired="false" Width="150"/>
                </asp:TableCell>
                <asp:TableCell ID="tblGridHeaderCell5" runat="server" Width="20%">
                    Recurring:&nbsp;
                    <uc:TicketRecurring ID="ucTicketRecurring" runat="server" IsRequired="false" Width="125" />
                </asp:TableCell>
                <asp:TableCell ID="tblGridHeaderCell6" runat="server" Width="20%">
                    Records:&nbsp;
                    <uc:RecordPerPageDDL ID="ucRecordsPerPage" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
    
    <asp:PlaceHolder ID="phEditors" runat="server" />
    <asp:Literal ID="litDebug" runat="server" />
    <telerik:RadGrid ID="rgTicket" runat="server"
        OnNeedDataSource="rgTicket_NeedDataSource" 
        OnItemDataBound="rgTicket_ItemDataBound" 
        OnUpdateCommand="rgTicket_UpdateCommand" 
        OnInsertCommand="rgTicket_InsertCommand" 
        OnPreRender="rgTicket_PreRender"
        OnItemCreated="rgTicket_ItemCreated"
        OnItemCommand="rgTicket_ItemCommand"
        OnRowDrop="rgTicket_RowDrop"
        OnSortCommand="rgTicket_SortCommand"
        AllowPaging="true"
        Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
        AllowSorting="true" AutoGenerateColumns="false" AllowMultiRowSelection="true" AllowMultiRowEdit="true" PagerStyle-ShowPagerText="true">
        
        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true">
            <Selecting AllowRowSelect="true" />
            <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
            <ClientMessages DragToGroupOrReorder="" DragToResize="" DropHereToReorder="" />    
        </ClientSettings>
        <SortingSettings SortToolTip="" />    
 
        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" InsertItemPageIndexAction="ShowItemOnCurrentPage"
            DataKeyNames="Id,TaggedTicketId,AwaitingResponseUserId,Priority,CompanyName,UseHelpDesk,UserClientId,UserFirstName,AssignedTo,FkDisposition,AssignedToUserEmail,IsRecurring,UserLastName,FkClient,AwaitingResponseInternal" 
            CommandItemDisplay="Top" EditMode="EditForms" Width="100%" CommandItemSettings-AddNewRecordText="Add Ticket" 
            PagerStyle-ShowPagerText="true">
            <PagerStyle Position="Bottom" AlwaysVisible="true" Mode="NumericPages" />
            <CommandItemTemplate>
                <table class="rgCommandRow" style="width:100%">
                    <asp:MultiView ID="mvCommandTemplate" runat="server" ActiveViewIndex="0">
                        <asp:View ID="viewCommandTemplateDefault" runat="server">
                                <tr>    
                                <td style="width:13%">
                                     <asp:LinkButton ID="lbAddTicket" runat="server" CommandName="AddNewTicket">
                                        <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/AddRecord.gif"/>Add Ticket
                                    </asp:LinkButton>
                                </td>
                                <td style="width:20%" align="center">
                                    <asp:LinkButton ID="lbCloseTickets" runat="server" CommandName="CloseSelected"
                                        OnClientClick="javascript:return confirm('Close all selected tickets?')">
                                            <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Cancel.gif" />Close Selected Tickets
                                    </asp:LinkButton>
                                </td>
                                <td style="width:13%" align="center">
                                    <asp:LinkButton ID="lbEditAll" runat="server" CommandName="EditAll">
                                            <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Edit.gif" />Edit All
                                    </asp:LinkButton>   
                                </td>
                                <td style="width:19%" align="center">
                                    <asp:LinkButton ID="lbTicketMerge" runat="server" CommandName="TicketMerge">
                                            <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Update.gif" />Merge Selected Tickets
                                    </asp:LinkButton>   
                                </td>
                                <td style="width:20%" align="center">
                                    <uc:GridExportTypeDDL ID="ucGridExportType" runat="server" />&nbsp;
                                    <asp:LinkButton ID="btnExport" runat="server"
                                        Text="Export" OnClick="btnExport_Click"/>
                                </td>
                                <td style="width:15%" align="right">
                                    <asp:LinkButton ID="lbRefresh" runat="server" CommandName="RefreshGrid">
                                        <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Refresh.gif" />Refresh
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </asp:View>

                        <asp:View ID="viewCommandTemplateBatchEdit" runat="server">
                            <tr>
                                <td style="width:45%" align="right">
                                    <asp:LinkButton ID="lbUpdateAll" runat="server" CommandName="UpdateAll" ValidationGroup="vgTicketBatchEdit" CausesValidation="true">
                                        <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Update.gif" />Update All
                                    </asp:LinkButton>
                                </td>
                                <td style="width:10%" align="left"></td>
                                <td style="width:45%" align="left">
                                    <asp:LinkButton ID="lbCancelUpdateAll" runat="server" CommandName="CancelUpdateAll">
                                        <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Cancel.gif" />Cancel
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </asp:View>

                        
                    </asp:MultiView>
                
                </table>
            </CommandItemTemplate> 
        
            <EditFormSettings UserControlName="/UserControl/Grid/EditForm/TicketEdit.ascx" EditFormType="WebUserControl">
                <EditColumn UniqueName="EditCommandColumn1"></EditColumn>
            </EditFormSettings>
        
        </MasterTableView>
        
    </telerik:RadGrid>

    <telerik:RadWindowManager ID="rwmTicketMerge" runat="server" EnableShadow="true" />

</asp:Panel> 






