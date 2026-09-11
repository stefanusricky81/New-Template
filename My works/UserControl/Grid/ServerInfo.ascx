<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ServerInfo.ascx.cs" Inherits="UserControl_Grid_ServerInfo" %>


<asp:Panel ID="pnlHeader" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="GridHeaderTable">
        <tr>
            <th style="width:33%">Hosts</th>
            <td style="width:33%; text-align:center;">
               &nbsp;
            </td>
            <td style="width:32%">
                Records Per Page:&nbsp;
               <uc:RecordPerPageDDL ID="ucRecordsPerPage" runat="server" />
            </td>
            <td style="width:2%">
                <div class="showHide">
                    <a href="#" class="hide" onclick="showHideInfo(this, '<%=this.ClientID %>'); return false;"></a>
                </div>	
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Literal ID="litDivBegin" runat="server"></asp:Literal>

<telerik:RadGrid ID="rgServerInfo" runat="server"
    OnNeedDataSource="rgServerInfo_NeedDataSource" 
    OnItemDataBound="rgServerInfo_ItemDataBound"
    Skin="BitByBit" EnableEmbeddedSkins="false" Width="100%" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false"
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="false" 
    MasterTableView-NoMasterRecordsText="No Records found.">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="false">
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
        
    <MasterTableView ShowHeadersWhenNoRecords="true" Width="100%" TableLayout="fixed"
        DataKeyNames="pserver" EditMode="EditForms">
        <Columns>
            <telerik:GridEditCommandColumn UniqueName="ViewLink" HeaderStyle-Width="6%" EditText="View">
            </telerik:GridEditCommandColumn>
            <telerik:GridBoundColumn UniqueName="Created" DataField="Created" DataFormatString="{0:MM/dd/yy HH:mm}"
                SortExpression="Created" HeaderText="Date" HeaderStyle-Width="20%">
            </telerik:GridBoundColumn>
             <telerik:GridBoundColumn UniqueName="Host Name" DataField="machinename" 
                SortExpression="machinename" HeaderText="Host Name" HeaderStyle-Width="16%">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="Primary IP" DataField="primaryipaddress"  
                SortExpression="primaryipaddress" HeaderText="Primary IP" HeaderStyle-Width="18%">
            </telerik:GridBoundColumn>
            
        </Columns>
        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/ServerInfoEdit.ascx" EditFormType="WebUserControl" >
            <EditColumn UniqueName="EditCommandColumn1"></EditColumn>
        </EditFormSettings>

    </MasterTableView>
    
</telerik:RadGrid>

<asp:Literal ID="litDivEnd" runat="server" Text="</div>"></asp:Literal>