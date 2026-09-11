<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketFile.ascx.cs" Inherits="UserControl_Grid_TicketFile" Debug="true"%>

<telerik:radscriptblock id="RadScriptBlock1" runat="server">
<script type="text/javascript">
    function conditionalPostback(e, sender) {
        var theRegexp = new RegExp("\.btnUpdate$|\.btnAdd$|\.InitInsertButton$|\.btnViewFile$", "ig");
        if (sender.EventTarget.match(theRegexp) && sender.get_eventTarget().indexOf("rgTicketFile") > -1) 
            sender.EnableAjax = false;  
    }
</script>
</telerik:radscriptblock>
<asp:Literal ID="litMessage" runat="server" />
<telerik:RadGrid ID="rgTicketFile" runat="server"
    OnNeedDataSource="rgTicketFile_NeedDataSource" 
    OnItemDataBound="rgTicketFile_ItemDataBound"
    OnInsertCommand="rgTicketFile_InsertCommand" 
    OnPreRender="rgTicketFile_PreRender" 
    OnItemCreated="rgTicketFile_ItemCreated" 
    OnItemCommand="rgTicketFile_ItemCommand"
    Skin="BitByBit2" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false"
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="false" 
    MasterTableView-NoMasterRecordsText="No Records found.">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" AllowDragToGroup="false">
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
        
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="auto" Width="100%" 
        InsertItemPageIndexAction="ShowItemOnCurrentPage" CommandItemDisplay="Top" 
        EditMode="EditForms" CommandItemSettings-AddNewRecordText="Upload Files" 
        DataKeyNames="pcscdefectfile">

        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/TicketFileEdit.ascx" EditFormType="WebUserControl">
            <EditColumn UniqueName="EditCommandColumn1"></EditColumn>
        </EditFormSettings>
        
        <Columns>
            <telerik:GridBoundColumn UniqueName="Created" DataField="Created" DataFormatString="{0:MM/dd/yy HH:mm}" SortExpression="Created" HeaderText="Uploaded" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" />
            <telerik:GridBoundColumn UniqueName="UploadedBy" DataField="UploadedBy" SortExpression="UserLast" HeaderText="Uploaded By" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30%" />
            <telerik:GridTemplateColumn DataField="Name" HeaderStyle-Width="30%" HeaderText="File" UniqueName="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="btnViewFile" runat="server" CommandName="ViewFile" />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn UniqueName="EmailFile" HeaderText="Email Attachment" HeaderStyle-Width="20%">
                <ItemTemplate><asp:CheckBox ID="chkEmailFile" runat="server"/></ItemTemplate> 
            </telerik:GridTemplateColumn> 
        </Columns>

    </MasterTableView>
    
</telerik:RadGrid>