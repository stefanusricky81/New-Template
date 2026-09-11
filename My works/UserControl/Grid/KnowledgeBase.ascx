<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KnowledgeBase.ascx.cs" Inherits="UserControl_Grid_KnowledgeBase" %>

<asp:Panel ID="pnlHeader" runat="server" Visible="false">
    <table border="0" cellpadding="0" cellspacing="0" class="GridHeaderTable">
        <tr>
            <th style="width:33%"><asp:Literal ID="litHeader" runat="server"></asp:Literal></th>
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

<telerik:RadGrid ID="rgKnowledgeBase" runat="server" 
    OnNeedDataSource="rgKnowledgeBase_NeedDataSource" 
    OnItemDataBound="rgKnowledgeBase_ItemDataBound" 
    OnUpdateCommand="rgKnowledgeBase_UpdateCommand" 
    OnInsertCommand="rgKnowledgeBase_InsertCommand" 
    OnDeleteCommand="rgKnowledgeBase_DeleteCommand" 
    OnPreRender="rgKnowledgeBase_PreRender"
    Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false"
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="false" 
    MasterTableView-NoMasterRecordsText="No Records found.">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="false">
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
        
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Auto" InsertItemPageIndexAction="ShowItemOnCurrentPage"
        DataKeyNames="Pknowledge,FkClient" CommandItemDisplay="Top" EditMode="EditForms" Width="100%">

        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/KnowledgeBaseEdit.ascx" EditFormType="WebUserControl" >
            <EditColumn UniqueName="EditCommandColumn1"></EditColumn>
        </EditFormSettings>
        
        <Columns>
            <telerik:GridHyperLinkColumn UniqueName="View" Text="View"
                DataNavigateUrlFields="PKnowledge"
                DataNavigateUrlFormatString="/KnowledgeBase/View.aspx?ID={0}"
                HeaderStyle-Width="5%" ItemStyle-VerticalAlign="Top">
            </telerik:GridHyperLinkColumn>
            <telerik:GridEditCommandColumn UniqueName="Edit" HeaderStyle-Width="5%" EditText="Edit">
            </telerik:GridEditCommandColumn>
            <telerik:GridBoundColumn UniqueName="Pknowledge" DataField="Pknowledge" 
                SortExpression="Pknowledge" HeaderText="KB #" HeaderStyle-Width="6%">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="Product" DataField="Product" 
                SortExpression="Product" HeaderText="Product / Category" 
                HeaderStyle-Width="14%">
            </telerik:GridBoundColumn>
            <telerik:GridHyperLinkColumn UniqueName="Subject" DataTextField="Subject" 
                SortExpression="Subject" HeaderText="Subject" 
                DataNavigateUrlFields="PKnowledge"
                DataNavigateUrlFormatString="/KnowledgeBase/Detail.aspx?ID={0}"
                HeaderStyle-Width="31%" ItemStyle-VerticalAlign="Top">
            </telerik:GridHyperLinkColumn>
            <telerik:GridBoundColumn UniqueName="FkClient" DataField="FkClient" 
                SortExpression="FkClient" HeaderText="Client" HeaderStyle-Width="8%">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="FkEmployee" DataField="FkEmployee" 
                SortExpression="FkEmployee" HeaderText="Created By" HeaderStyle-Width="8%">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="Created" DataField="Created" 
                SortExpression="Created" HeaderText="Created"  DataFormatString="{0:MM/dd/yy HH:mm}" 
                HeaderStyle-Width="9%">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="LastUpdated" DataField="LastUpdated" 
                SortExpression="LastUpdated" HeaderText="Updated"  DataFormatString="{0:MM/dd/yy HH:mm}" 
                HeaderStyle-Width="9%">
            </telerik:GridBoundColumn>
            <telerik:GridButtonColumn UniqueName="Delete" CommandName="Delete" 
                Text="Delete" ConfirmText="Confirm Delete" 
                ItemStyle-ForeColor="#DB2929" HeaderStyle-Width="5%">
            </telerik:GridButtonColumn>
     
        </Columns>

        

    </MasterTableView>
    
</telerik:RadGrid>

<asp:Literal ID="litDivEnd" runat="server" Text="</div>"></asp:Literal>
