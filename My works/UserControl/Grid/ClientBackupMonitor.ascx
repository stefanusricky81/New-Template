<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientBackupMonitor.ascx.cs" Inherits="UserControl_Grid_ClientBackupMonitor" %>

<asp:PlaceHolder ID="phCssClip" runat="server" Visible="false">
    <style type="text/css">
        .RadGrid_BitByBit .rgRow td,
        .RadGrid_BitByBit .rgAltRow td
        {
	        white-space: normal;
        }    
    </style>
</asp:PlaceHolder>
<asp:Panel ID="pnlHeader" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="GridHeaderTable">
        <tr>
            <th style="width:33%"><asp:Literal ID="litHeader" runat="server"></asp:Literal></th>
            <td style="width:33%; text-align:center;">&nbsp;</td>
            <td style="width:32%">Records Per Page:&nbsp;<uc:RecordPerPageDDL ID="ucRecordsPerPage" runat="server" /></td>
            <td style="width:2%"><div class="showHide"><asp:HyperLink ID="hlShowHide" runat="server" /></div></td>    
        </tr>
    </table>
</asp:Panel>

<div ID="divClientBackupMonitor" runat="server" style="border-style:none;">

<telerik:RadGrid ID="rgClientBackupMonitor" runat="server" 
    OnNeedDataSource="rgClientBackupMonitor_NeedDataSource" 
    OnItemDataBound="rgClientBackupMonitor_ItemDataBound" 
    OnUpdateCommand="rgClientBackupMonitor_UpdateCommand" 
    OnItemCommand="rgClientBackupMonitor_ItemCommand"
    OnPreRender="rgClientBackupMonitor_PreRender"
    PageSize="50" Skin="BitByBit" EnableEmbeddedSkins="false" 
    ImagesPath="/RadControls/Skin/Grid/BitByBit/" ItemStyle-Wrap="true"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false" AllowMultiRowEdit="true"
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="true" PagerStyle-Position="Bottom">
        
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" >
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
    
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed"
        CommandItemSettings-ShowAddNewRecordButton="false" 
        InsertItemPageIndexAction="ShowItemOnCurrentPage" 
        DataKeyNames="Id" CommandItemDisplay="Top" EditMode="EditForms" Width="100%">

        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/BackupMonitorEdit.ascx" EditFormType="WebUserControl">
            <EditColumn UniqueName="EditCommandColumn1"></EditColumn>
        </EditFormSettings>

        <CommandItemTemplate>
            <table class="rgCommandRow" style="width:100%">
                <asp:MultiView ID="mvCommandTemplate" runat="server" ActiveViewIndex="0">
                    <asp:View ID="viewCommandTemplateDefault" runat="server">
                            <tr>    
                            <td style="width:85%" align="center">
                                <asp:LinkButton ID="lbEditAll" runat="server" CommandName="EditAll">
                                    <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Edit.gif" />Edit All
                                </asp:LinkButton>   
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
                                <asp:LinkButton ID="lbUpdateAll" runat="server" CommandName="UpdateAll" ValidationGroup="vgBackupMonitorBatchEdit" CausesValidation="true">
                                    <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Update.gif" />Update All
                                </asp:LinkButton>
                            </td>
                            <td style="width:10%" align="left"></td>
                            <td style="width:45%" align="left">
                                <asp:LinkButton ID="lbCancelUpdateAll" runat="server" CommandName="CancelUpdateAll">
                                    <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Cancel.gif" />Cancel All
                                </asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table border="0" style="width:100%;background: #D6DFEF;padding-top: 15px;padding-bottom: 15px">
                                    <tr>
                                        <th style="width:85px;">Client</th>
                                        <th style="width:85px;">Server</th>
                                        <th style="width:85px;">BU Set</th>
                                        <th style="width:85px;">BU Server</th>
                                        <th style="width:225px;">Backup Description</th>
                                        <th style="width:140px;">Status</th>
                                        <th>Comment</th>
                                    </tr>  
                                </table>                            
                            </td>
                        </tr>                   
                    </asp:View>
                        
                </asp:MultiView>
                
            </table>
        </CommandItemTemplate> 
                        
    </MasterTableView>
    
</telerik:RadGrid>
</div>