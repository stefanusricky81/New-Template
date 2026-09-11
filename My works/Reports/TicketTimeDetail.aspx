<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="TicketTimeDetail.aspx.cs" Inherits="Reports_TicketTimeDetail" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketType" Src="~/UserControl/ListBox/MultiTicketType.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
       <style> 
            div.scroll { 
                overflow-x: auto; 
                overflow-y: hidden; 
                white-space: nowrap; 
            } 

            .RadGrid_3b .RadComboBox .rcbInput {width:50px !important;}
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Ticket Time Detail Report</h1>
        </div>
    </div>
    <asp:ValidationSummary ID="vsClientReportTicket" runat="server" CssClass="validationSummary" ValidationGroup="vgClientTicketReport" />
    <asp:Literal ID="litMessage" runat="server" />
    <div class="block">
        <div class="block-title">
            <h2><strong>Search Criteria</strong></h2>
        </div>
        <div class="row">
            <div class="form-group col-sm-6">
                <label>Client</label>
                <ddl:Client ID="ddlClient" runat="server" IsRequired="true" DisplayChosenScript="true" ValidationGroup="vgClientTicketReport" />
            </div>
            <div class="form-group col-sm-6">
                <label>Ticket Type</label>
                <ddl:TicketType ID="ddlTicketType" runat="server" DisplayChosenScript="true" ValidationGroup="vgClientTicketReport" />
            </div>
        </div>
        <div class="row">
            <div class="form-group col-sm-6">
                <label>Created Date Start</label>
                <uc:DatePicker ID="ucCreatedStart" runat="server" IsRequired="false" />
            </div>
            <div class="form-group col-sm-6">
                <label>Created Date End</label>
                <uc:DatePicker ID="ucCreatedEnd" runat="server" IsRequired="false" />
            </div>
            <asp:CustomValidator ID="cvCreatedDates" runat="server" ValidateEmptyText="true" ValidationGroup="vgClientTicketReport" 
                ErrorMessage="Created End Date must be greater than Created Start Date" Display="None" OnServerValidate="cvCreatedDates_ServerValidate" />
        </div>
        <div class="row">
            <div class="form-group form-actions" style="padding-top:20px;">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgClientTicketReport"><i class="hi hi-search"></i> Search</asp:LinkButton>
                <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
            </div>
        </div>
    </div>
    <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="false">
        <div class="block" style="padding-bottom: 20px;">
            <div class="block-title">
                <h2><strong>Search Results</strong></h2>
                <asp:PlaceHolder ID="phScrollNotes" runat="server"></asp:PlaceHolder>
            </div>
            <div class="row">
                <div class="form-group form-actions" style="padding-top:20px;">
                    <asp:LinkButton ID="lbexport" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lbexport_Click"><i class="hi hi-export"></i> Export to Excel</asp:LinkButton>
                </div>
            </div>
            <br />
             <div>
                <strong>Received Via</strong>
            </div>
            <telerik:RadGrid ID="rgReceiveVia" OnNeedDataSource="rgClientTicketReport_NeedDataSource" OnItemDataBound="rgReceiveVia_ItemDataBound"
                OnItemCommand="rgReceiveVia_ItemCommand" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                 ShowFooter="false" Width="100%"> 
                <MasterTableView ShowHeadersWhenNoRecords="true" DataKeyNames="Detail" TableLayout="Fixed" Width="100%" ShowFooter="true">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="RECEIVEDMETHOD" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" SortExpression="RECEIVEDMETHOD" UniqueName="RECEIVEDMETHOD" HeaderTooltip="Received Via" />
                        <telerik:GridBoundColumn DataField="Detail" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" UniqueName="Detail" Display="false"/>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Critical" HeaderStyle-Width="7%" HeaderText="Critical" UniqueName="Critical">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbCritical" runat="server" Text='<%# Eval("Critical") %>' CommandArgument="CriticalReceivedVia" > <i class="gi gi-pencil" title="Critical"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="High" HeaderStyle-Width="7%" HeaderText="High" UniqueName="High">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbHigh" runat="server" Text='<%# Eval("High") %>' CommandArgument="HighReceivedVia" > <i class="gi gi-pencil" title="High"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Normal" HeaderStyle-Width="7%" HeaderText="Normal" UniqueName="Normal">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbNormal" runat="server" Text='<%# Eval("Normal") %>' CommandArgument="NormalReceivedVia" > <i class="gi gi-pencil" title="Normal"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Low" HeaderStyle-Width="7%" HeaderText="Low" UniqueName="Low">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbLow" runat="server" Text='<%# Eval("Low") %>' CommandArgument="LowReceivedVia" > <i class="gi gi-pencil" title="Low"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <br />
            <div>
                <strong>Ticket Category</strong>
            </div>
            <telerik:RadGrid ID="rgTicketCategory" OnNeedDataSource="rgClientTicketReport_NeedDataSource" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                ShowFooter="false" OnItemCommand="rgReceiveVia_ItemCommand" Width="100%"> 
                <MasterTableView ShowHeadersWhenNoRecords="true" DataKeyNames="Detail" TableLayout="Fixed" Width="100%">
                    <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="10,25,50,100,250,500" PageSizeControlType="RadComboBox" Position="Bottom" />
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="TICKETCATEGORY" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" SortExpression="TICKETCATEGORY" UniqueName="TICKETCATEGORY" HeaderTooltip="Ticket Category" />
                        <telerik:GridBoundColumn DataField="Detail" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" UniqueName="Detail" Display="false"/>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Critical" HeaderStyle-Width="7%" HeaderText="Critical" UniqueName="Critical">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbCritical" runat="server" Text='<%# Eval("Critical") %>' CommandArgument="CriticalTicketCategory" > <i class="gi gi-pencil" title="Critical"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="High" HeaderStyle-Width="7%" HeaderText="High" UniqueName="High">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbHigh" runat="server" Text='<%# Eval("High") %>' CommandArgument="HighTicketCategory" > <i class="gi gi-pencil" title="High"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Normal" HeaderStyle-Width="7%" HeaderText="Normal" UniqueName="Normal">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbNormal" runat="server" Text='<%# Eval("Normal") %>' CommandArgument="NormalTicketCategory" > <i class="gi gi-pencil" title="Normal"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Low" HeaderStyle-Width="7%" HeaderText="Low" UniqueName="Low">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbLow" runat="server" Text='<%# Eval("Low") %>' CommandArgument="LowTicketCategory" > <i class="gi gi-pencil" title="Low"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <br />
            <div>
                <strong>Ticket Status</strong>
            </div>
            <telerik:RadGrid ID="rgTicketStatus" OnNeedDataSource="rgClientTicketReport_NeedDataSource" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                AllowPaging="true" OnItemCommand="rgReceiveVia_ItemCommand" ShowFooter="false" Width="100%"> 
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" Width="100%">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="TICKETSTATUS" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" SortExpression="TICKETSTATUS" UniqueName="TICKETSTATUS" HeaderTooltip="Ticket Status" />
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Critical" HeaderStyle-Width="7%" HeaderText="Critical" UniqueName="Critical">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbCritical" runat="server" Text='<%# Eval("Critical") %>' CommandArgument="CriticalTicketStatus" > <i class="gi gi-pencil" title="Critical"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="High" HeaderStyle-Width="7%" HeaderText="High" UniqueName="High">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbHigh" runat="server" Text='<%# Eval("High") %>' CommandArgument="HighTicketStatus" > <i class="gi gi-pencil" title="High"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Normal" HeaderStyle-Width="7%" HeaderText="Normal" UniqueName="Normal">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbNormal" runat="server" Text='<%# Eval("Normal") %>' CommandArgument="NormalTicketStatus" > <i class="gi gi-pencil" title="Normal"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Low" HeaderStyle-Width="7%" HeaderText="Low" UniqueName="Low">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbLow" runat="server" Text='<%# Eval("Low") %>' CommandArgument="LowTicketStatus" > <i class="gi gi-pencil" title="Low"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <br />
            <div>
                <strong>Response Time</strong>
            </div>
            <telerik:RadGrid ID="rgResponseTime" OnNeedDataSource="rgClientTicketReport_NeedDataSource" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                AllowPaging="true" OnItemCommand="rgReceiveVia_ItemCommand" ShowFooter="false" Width="100%"> 
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" Width="100%">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="AVGresponsetime" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" SortExpression="AVGresponsetime" UniqueName="AVGresponsetime" HeaderTooltip="Average response time" />
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Critical" HeaderStyle-Width="7%" HeaderText="Critical" UniqueName="Critical">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbCritical" runat="server" Text='<%# Eval("Critical") %>' CommandArgument="CriticalResponseTime" > <i class="gi gi-pencil" title="Critical"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="High" HeaderStyle-Width="7%" HeaderText="High" UniqueName="High">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbHigh" runat="server" Text='<%# Eval("High") %>' CommandArgument="HighResponseTime" > <i class="gi gi-pencil" title="High"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Normal" HeaderStyle-Width="7%" HeaderText="Normal" UniqueName="Normal">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbNormal" runat="server" Text='<%# Eval("Normal") %>' CommandArgument="NormalResponseTime" > <i class="gi gi-pencil" title="Normal"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Low" HeaderStyle-Width="7%" HeaderText="Low" UniqueName="Low">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbLow" runat="server" Text='<%# Eval("Low") %>' CommandArgument="LowResponseTime" > <i class="gi gi-pencil" title="Low"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <br />
            <div>
                <strong>VIP</strong>
            </div>
            <telerik:RadGrid ID="rgVIP" OnItemCommand="rgReceiveVia_ItemCommand" OnNeedDataSource="rgClientTicketReport_NeedDataSource" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                AllowPaging="true" ShowFooter="false" Width="100%"> 
                <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" Width="100%">
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Title" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" SortExpression="AVGresponsetime" UniqueName="AVGresponsetime" HeaderTooltip="Average response time" />
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Critical" HeaderStyle-Width="7%" HeaderText="Critical" UniqueName="Critical">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbCritical" runat="server" Text='<%# Eval("Critical") %>' CommandArgument="CriticalVIP" > <i class="gi gi-pencil" title="Critical"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="High" HeaderStyle-Width="7%" HeaderText="High" UniqueName="High">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbHigh" runat="server" Text='<%# Eval("High") %>' CommandArgument="HighVIP" > <i class="gi gi-pencil" title="High"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Normal" HeaderStyle-Width="7%" HeaderText="Normal" UniqueName="Normal">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbNormal" runat="server" Text='<%# Eval("Normal") %>' CommandArgument="NormalVIP" > <i class="gi gi-pencil" title="Normal"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" SortExpression="Low" HeaderStyle-Width="7%" HeaderText="Low" UniqueName="Low">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbLow" runat="server" Text='<%# Eval("Low") %>' CommandArgument="LowVIP" > <i class="gi gi-pencil" title="Low"></i></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <br />
            <div class="table-responsive scroll">
                <telerik:RadGrid ID="rgClientTicketReport" Skin="3b" EnableEmbeddedSkins="false" OnNeedDataSource="rgClientTicketReport_NeedDataSource"
                    runat="server" AutoGenerateColumns="false" AllowSorting="true" OnItemDataBound="rgClientTicketReport_ItemDataBound"
                AllowPaging="true" ShowFooter="false" Width="150%" ClientSettings-Resizing-AllowColumnResize="true"> 
                <MasterTableView ShowHeadersWhenNoRecords="true" DataKeyNames="ID" TableLayout="Fixed" Width="150%">
                    <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" PageSizes="10,25,50,100,250,500" PageSizeControlType="RadComboBox" Position="Bottom" />
                    <NoRecordsTemplate>
                        <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" HeaderStyle-Width="3%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket #" SortExpression="ID" UniqueName="ID" HeaderTooltip="Ticket #" />
                        <telerik:GridBoundColumn DataField="TICKETRECEIVED" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Received" SortExpression="TICKETRECEIVED" UniqueName="TICKETRECEIVED" HeaderTooltip="Ticket Received" />
                        <telerik:GridBoundColumn DataField="DESCR" HeaderStyle-Width="15%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Description" SortExpression="DESCR" UniqueName="CompDESCRany" HeaderTooltip="Ticket Description" />
                        <telerik:GridBoundColumn DataField="REQUESTOR" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Requestor" SortExpression="REQUESTOR" UniqueName="REQUESTOR" HeaderTooltip="Requestor" />
                        <telerik:GridBoundColumn DataField="CONTACTNAME" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Client contact" SortExpression="CONTACTNAME" UniqueName="CONTACTNAME" HeaderTooltip="Client contact name" />
                        <telerik:GridBoundColumn DataField="TICKETPRIORITY" HeaderStyle-Width="3%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Priority" SortExpression="TICKETPRIORITY" UniqueName="TICKETPRIORITY" HeaderTooltip="Ticket Priority" />
                        <telerik:GridBoundColumn DataField="TICKETSUMMARY" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Summary" SortExpression="TICKETSUMMARY" UniqueName="TICKETSUMMARY" HeaderTooltip="Ticket Summary" />
                        <telerik:GridBoundColumn DataField="RESPONSETIMENEW" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Response Time" SortExpression="RESPONSETIMENEW" UniqueName="RESPONSETIMENEW" HeaderTooltip="Response Time" />
                        <telerik:GridBoundColumn DataField="TICKETCLOSED" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Closed" SortExpression="TICKETCLOSED" UniqueName="TICKETCLOSED" HeaderTooltip="Ticket Closed" />
                        <telerik:GridBoundColumn DataField="TECHNICIAN" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Technician" SortExpression="TECHNICIAN" UniqueName="TECHNICIAN" HeaderTooltip="Technician" />
                        <telerik:GridBoundColumn DataField="TICKETSTATUS" HeaderStyle-Width="3%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Status" SortExpression="TICKETSTATUS" UniqueName="TICKETSTATUS" HeaderTooltip="Ticket Status" />
                        <telerik:GridBoundColumn DataField="RECEIVEDMETHOD" HeaderStyle-Width="3%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Receive Via" SortExpression="RECEIVEDMETHOD" UniqueName="RECEIVEDMETHOD" HeaderTooltip="Receive Via" />
                        <telerik:GridBoundColumn DataField="TICKETCATEGORY" HeaderStyle-Width="5%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Ticket Category" SortExpression="TICKETCATEGORY" UniqueName="TICKETCATEGORY" HeaderTooltip="Ticket Category" />
                        <telerik:GridBoundColumn DataField="AllUpdateNotes" HeaderStyle-Width="75%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="All Update Note" SortExpression="AllUpdateNotes" UniqueName="AllUpdateNotes" HeaderTooltip="All Update Note" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            </div>
            
         </div>
            
    </asp:PlaceHolder>
</asp:Content>