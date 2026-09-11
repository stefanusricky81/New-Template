<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KnowledgeBaseLog.ascx.cs" Inherits="UserControl_Grid_KnowledgeBaseLog" %>

<asp:Panel ID="pnlHeader" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="GridHeaderTable">
        <tr>
            <th style="width:33%; text-align:left; font-weight:bold;">&nbsp;</th>
            <td style="width:33%; text-align:center;">
               &nbsp;
            </td>
            <td style="width:32%; text-align:right;">
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

<telerik:RadGrid ID="rgKnowledgeBaseLog" runat="server"
    OnNeedDataSource="rgKnowledgeBaseLog_NeedDataSource" 
    OnItemDataBound="rgKnowledgeBaseLog_ItemDataBound"
    Skin="BitByBit2" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false"
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="false" 
    MasterTableView-NoMasterRecordsText="No Records found.">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="false">
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
        
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="fixed" Width="100%"
        DataKeyNames="ID">
        
        <Columns>
            
            <telerik:GridBoundColumn UniqueName="Created" DataField="Created" DataFormatString="{0:MM/dd/yy HH:mm}"
                SortExpression="Created" HeaderText="Date" HeaderStyle-Width="30%">
            </telerik:GridBoundColumn>
             <telerik:GridBoundColumn UniqueName="Description" DataField="Description" 
                SortExpression="Description" HeaderText="Description" HeaderStyle-Width="40%">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="User" DataField="User"  
                SortExpression="User" HeaderText="User" HeaderStyle-Width="30%">
            </telerik:GridBoundColumn>
            
        </Columns>

    </MasterTableView>
    
</telerik:RadGrid>

<asp:Literal ID="litDivEnd" runat="server" Text="</div>"></asp:Literal>