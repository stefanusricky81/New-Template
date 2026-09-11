<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientContact.ascx.cs" Inherits="UserControl_Grid_ClientContact" %>
    
<telerik:RadNotification ID="rnContactUpdate" runat="server" Width="250" Height="100" EnableRoundedCorners="true" VisibleTitlebar="false" Pinned="false" Position="Center" Skin="Black" AnimationDuration="2" Animation="FlyIn"/>

<asp:Panel ID="pnlHeader" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="GridHeaderTable">
        <tr>
            <th style="width:33%"><asp:Literal ID="litHeader" runat="server"></asp:Literal></th>
            <td style="text-align:center;" class="auto-style2">
               &nbsp;
            </td>
            <td style="width:32%">
                Records Per Page:&nbsp;
                <uc:RecordPerPageDDL ID="ucRecordsPerPage" runat="server" />
            </td>
            <td style="width:2%">
                <div class="showHide">
                    <asp:HyperLink ID="hlShowHide" runat="server" />
                </div>	
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Literal ID="litDebug" runat="server" />
    <telerik:RadGrid ID="rgClientContact" runat="server" 
        OnItemDataBound="rgClientContact_ItemDataBound"
        OnNeedDataSource="rgClientContact_NeedDataSource"
        OnUpdateCommand="rgClientContact_UpdateCommand"
        OnInsertCommand="rgClientContact_InsertCommand" 
        OnDeleteCommand="rgClientContact_DeleteCommand"
        OnPreRender="rgClientContact_PreRender"
        OnItemCreated="rgClientContact_ItemCreated"
        Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
        AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false" 
        PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="true" PagerStyle-Position="Bottom"
        Visible ="true">

        <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" InsertItemPageIndexAction="ShowItemOnCurrentPage"
        DataKeyNames="Pusers,PclientContact" CommandItemDisplay="Top" EditMode="EditForms" Width="100%" 
        CommandItemSettings-AddNewRecordText="Add Contact">
        
        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/ClientContactEdit.ascx" EditFormType="WebUserControl">
            <EditColumn UniqueName="EditCommandColumn1"></EditColumn>
        </EditFormSettings>
            <Columns>
                <telerik:GridEditCommandColumn UniqueName="Edit" HeaderStyle-Width="5%" EditText="Edit" />
                <telerik:GridBoundColumn UniqueName="company" DataField="company"  
                    SortExpression="company" HeaderText="Company" 
                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%"
                    ItemStyle-VerticalAlign="Top">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="code" DataField="code"  
                    SortExpression="code" HeaderText="Company Code" 
                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%"
                    ItemStyle-VerticalAlign="Top" >
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="FirstName" DataField="FirstName"  
                    SortExpression="FirstName" HeaderText="First Name" 
                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%"
                    ItemStyle-VerticalAlign="Top">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="LastName" DataField="LastName"  
                    SortExpression="LastName" HeaderText="Last Name" 
                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%"
                    ItemStyle-VerticalAlign="Top" HeaderButtonType="LinkButton" >
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="busphone" DataField="busphone"  
                    SortExpression="busphone" HeaderText="Phone" 
                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%"
                    ItemStyle-VerticalAlign="Top">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="Email" DataField="Email"  
                    SortExpression="Email" HeaderText="Email" 
                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%"
                    ItemStyle-VerticalAlign="Top">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="Priority" DataField="Priority"  
                    SortExpression="Priority" HeaderText="Priority" 
                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%"
                    ItemStyle-VerticalAlign="Top">
                </telerik:GridBoundColumn>
                <telerik:GridButtonColumn UniqueName="Delete" CommandName="Delete" 
                    Text="Delete" ConfirmText="Confirm Delete" 
                    ItemStyle-ForeColor="#DB2929" HeaderStyle-Width="5%">
                </telerik:GridButtonColumn>

            </Columns>
            </MasterTableView>

    </telerik:RadGrid>
   