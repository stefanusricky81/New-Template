<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientEmailBroadcastLog.ascx.cs" Inherits="UserControl_Grid_ClientEmailBroadcastLog" %>
<style type="text/css">
    .RadGrid_BitByBit .rgRow td,
    .RadGrid_BitByBit .rgAltRow td
    {
	    white-space: normal;
    }    
    .rgDetailTable
    {
        border: 2px dotted black;
        margin: 10px !important;
    }
</style>
<asp:Panel ID="pnlHeader" runat="server">
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
                    <asp:HyperLink ID="hlShowHide" runat="server" />
                </div>	
            </td>
        </tr>
    </table>
</asp:Panel>
<div ID="divClientEmailBroadcastLog" runat="server" style="border-style:none;">

<telerik:RadGrid ID="rgClientEmailBroadcastReport" runat="server"
    OnNeedDataSource="rgClientEmailBroadcastReport_NeedDataSource" 
    OnPreRender="rgClientEmailBroadcastReport_PreRender"
    OnDetailTableDataBind="rgClientEmailBroadcastReport_DetailTableDataBind" 
    PageSize="50"
    Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false" 
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="true" PagerStyle-Position="Bottom">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" >
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
    </ClientSettings>
    
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="auto" CommandItemSettings-ShowAddNewRecordButton="false" 
        InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="Id" CommandItemDisplay="Top" EditMode="EditForms" Width="100%">

        <CommandItemTemplate>
            <table class="rgCommandRow" style="width:100%">
                <tr>
                    <td style="width:70%" align="left">
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
            </table>
        </CommandItemTemplate> 

        <DetailTables>
            
            <telerik:GridTableView DataKeyNames="Id" Name="Recipients" Width="98%" AllowPaging="true" PageSize="10"  >
                <Columns>    
                    <telerik:GridBoundColumn UniqueName="Created" DataField="Created"  
                        SortExpression="Created" HeaderText="Date" 
                        HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-VerticalAlign="Top" ItemStyle-Width="20%">
                    </telerik:GridBoundColumn> 
                                                    
                    <telerik:GridBoundColumn UniqueName="Recipient" DataField="Recipient"  
                        SortExpression="Recipient" HeaderText="Recipient" 
                        HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-VerticalAlign="Top" ItemStyle-Width="20%">
                    </telerik:GridBoundColumn> 

                    <telerik:GridBoundColumn UniqueName="Company" DataField="Company"  
                        SortExpression="Company" HeaderText="Company" 
                        HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-VerticalAlign="Top" ItemStyle-Width="20%">
                    </telerik:GridBoundColumn>   

                    <telerik:GridBoundColumn UniqueName="First" DataField="First"  
                        SortExpression="First" HeaderText="FirstName" 
                        HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-VerticalAlign="Top" ItemStyle-Width="20%">
                    </telerik:GridBoundColumn>   

                    <telerik:GridBoundColumn UniqueName="Last" DataField="Last"  
                        SortExpression="Last" HeaderText="LastName" 
                        HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-VerticalAlign="Top" ItemStyle-Width="20%">
                    </telerik:GridBoundColumn>   

                </Columns>
            </telerik:GridTableView>        

        </DetailTables>
                                
        <Columns>

            <telerik:GridBoundColumn UniqueName="Created" DataField="Created"  
                SortExpression="Created" HeaderText="Date" 
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%"
                ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>   

            <telerik:GridBoundColumn UniqueName="Subject" DataField="Subject"  
                SortExpression="Subject" HeaderText="Subject" 
                HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="25%"
                ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            
            <telerik:GridBoundColumn UniqueName="Product" DataField="Product"  
                SortExpression="Product" HeaderText="Product" 
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-VerticalAlign="Top" ItemStyle-Width="25%">
            </telerik:GridBoundColumn>   

            <telerik:GridBoundColumn UniqueName="Description" DataField="Description"  
                SortExpression="Description" HeaderText="Description" 
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-VerticalAlign="Top" ItemStyle-Width="30%">
            </telerik:GridBoundColumn>   

            <telerik:GridBoundColumn UniqueName="NumOfEmails" DataField="NumOfEmails"  
                SortExpression="NumOfEmails" HeaderText="# of Recipients" 
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-VerticalAlign="Top" ItemStyle-Width="5%">
            </telerik:GridBoundColumn>   
            
        </Columns>
    </MasterTableView>
    
</telerik:RadGrid>
</div>