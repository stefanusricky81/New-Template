<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Contact.ascx.cs" Inherits="UserControl_Grid_Contact" %>
<telerik:RadGrid ID="rgContact" AllowSorting="true" AutoGenerateColumns="false"
    Skin="BitByBit" runat="server" AllowPaging="true" PageSize="50"
    OnNeedDataSource="rgContact_NeedDataSource" 
    OnItemDataBound="rgContact_ItemDataBound" 
    OnUpdateCommand="rgContact_UpdateCommand" 
    OnInsertCommand="rgContact_InsertCommand" 
    OnDeleteCommand="rgContact_DeleteCommand"
    AllowMultiRowSelection="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    ShowFooter="false" PagerStyle-Mode="NumericPages"
    PagerStyle-AlwaysVisible="true" EnableEmbeddedSkins="false" 
    MasterTableView-NoMasterRecordsText="No Records found.">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="false">
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="false" EnableRealTimeResize="false" />
    </ClientSettings>
        
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="fixed" Width="100%"
        DataKeyNames="ContactPclientContact" CommandItemDisplay="None">
        <Columns>
            <telerik:GridEditCommandColumn UniqueName="EditCommandColumn"
                EditText="Details" Visible="false">
            </telerik:GridEditCommandColumn>
            <telerik:GridBoundColumn HeaderText="Last Name" DataField="ContactLast" SortExpression="ContactLast"
                HeaderStyle-Width="15%" UniqueName="ContactLast">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn HeaderText="First Name" DataField="ContactFirst" SortExpression="ContactFirst"
                HeaderStyle-Width="15%" UniqueName="ContactFirst">
            </telerik:GridBoundColumn>
            <telerik:GridHyperLinkColumn HeaderText="Email" DataNavigateUrlFields="ContactEmail" 
                SortExpression="ContactEmail" DataTextField="ContactEmail"
                DataNavigateUrlFormatString="mailto:{0}" HeaderStyle-Width="20%">
            </telerik:GridHyperLinkColumn>
            <telerik:GridBoundColumn HeaderText="Main Phone" DataField="Busphone" SortExpression="Busphone"
                HeaderStyle-Width="15%" UniqueName="Busphone">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn HeaderText="Contact Phone" DataField="ContactBusphone" SortExpression="ContactBusphone"
                HeaderStyle-Width="15%" UniqueName="ContactBusphone">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn HeaderText="Company" DataField="Company" SortExpression="Company"
                HeaderStyle-Width="20%" UniqueName="Company">
            </telerik:GridBoundColumn>
            <telerik:GridButtonColumn UniqueName="DeleteColumn" Text="Delete" Visible="false"
                CommandName="Delete" ConfirmText="Confirm Delete"/>

        </Columns>
        
        <EditFormSettings UserControlName="/UserControl/Grid/EditForm/ContactEdit.ascx" EditFormType="WebUserControl" >
            <EditColumn UniqueName="EditCommandColumn1">
            </EditColumn>
        </EditFormSettings>


    </MasterTableView>
    
</telerik:RadGrid>

<telerik:RadAjaxManagerProxy ID="rampContact" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="rgContact">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rgContact" LoadingPanelID="ralpContact"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
       
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>

<telerik:RadAjaxLoadingPanel ID="ralpContact" runat="server" Transparency="25" BackColor="#E0E0E0">
    <img alt="Loading..." 
            src="/Images/loading.gif"
            style="border: 0px; padding-top:90px;" />
</telerik:RadAjaxLoadingPanel>