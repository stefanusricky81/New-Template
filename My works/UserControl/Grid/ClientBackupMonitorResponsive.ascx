<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientBackupMonitorResponsive.ascx.cs" Inherits="UserControl_Grid_ClientBackupMonitorResponsive" %>

<asp:PlaceHolder ID="phCssClip" runat="server" Visible="false">
    <style type="text/css">
        .RadGrid_BitByBit .rgRow td,
        .RadGrid_BitByBit .rgAltRow td
        {
	        white-space: normal;
        }    
    </style>
</asp:PlaceHolder>

<div ID="divClientBackupMonitor" runat="server" style="border-style:none;">

 <div class="table-responsive">
     <telerik:RadGrid ID="rgClientBackupMonitor" runat="server" 
    OnNeedDataSource="rgClientBackupMonitor_NeedDataSource" 
    OnItemDataBound="rgClientBackupMonitor_ItemDataBound" 
    OnUpdateCommand="rgClientBackupMonitor_UpdateCommand" 
    OnItemCommand="rgClientBackupMonitor_ItemCommand"
    OnPreRender="rgClientBackupMonitor_PreRender"
    Skin="3b" EnableEmbeddedSkins="false" 
    ItemStyle-Wrap="true"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false" AllowMultiRowEdit="true"
    ShowFooter="true" >
        
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" >
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
    
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed"
        CommandItemSettings-ShowAddNewRecordButton="false" 
        InsertItemPageIndexAction="ShowItemOnCurrentPage" 
        DataKeyNames="Id" CommandItemDisplay="Top" EditMode="EditForms" Width="100%">
        <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="10,25,50,100,250" PageSizeControlType="RadComboBox" Position="Bottom" />
        <NoRecordsTemplate>
            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
        </NoRecordsTemplate>
        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/BackupMonitorEdit.ascx" EditFormType="WebUserControl">
            <EditColumn UniqueName="EditCommandColumn1"></EditColumn>
        </EditFormSettings>

        <CommandItemTemplate>
            <asp:MultiView ID="mvCommandTemplate" runat="server" ActiveViewIndex="0">
                <asp:View ID="viewCommandTemplateDefault" runat="server">
                    <div style="margin:5px 5px;">
                        <asp:LinkButton ID="lbEditAll" runat="server" CommandName="EditAll" style="padding-right:10px;"><i class="gi gi-pencil" title="Edit"></i> Edit All</asp:LinkButton>
                        <asp:LinkButton ID="lbRefresh" runat="server" CommandName="RebindGrid" CssClass="pull-right"><i class="gi gi-refresh" title="Refresh"></i>  Refresh</asp:LinkButton>
                    </div>
                </asp:View>

                <asp:View ID="viewCommandTemplateBatchEdit" runat="server">
                    <div style="margin:5px 5px;">
                        <asp:LinkButton ID="lbUpdateAll" runat="server" CommandName="UpdateAll" ValidationGroup="vgBackupMonitorBatchEdit" CausesValidation="true" style="padding-right:10px;"><i class="gi gi-pencil" title="Edit"></i> Update All</asp:LinkButton>
                        <asp:LinkButton ID="lbCancelUpdateAll" runat="server" CommandName="CancelUpdateAll" CssClass="pull-right"><i class="gi gi-cross" title="Cancel"></i>  Cancel All</asp:LinkButton>
                    </div>
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
        </CommandItemTemplate> 
                        
    </MasterTableView>
    
</telerik:RadGrid>
 </div>

</div>