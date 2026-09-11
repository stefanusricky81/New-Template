<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="Email.aspx.cs" Inherits="Maintenance_CRM_Email" %>



<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        .success {
            color: green;
        }

        .error {
            color: red;
        }
    </style>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Email Search</h1>
        </div>
    </div>
   
    <telerik:RadAjaxPanel ID="rapMenu" runat="server" LoadingPanelID="ralpMenu" EnableAJAX="true">
         <asp:Literal ID="litMessage" runat="server" Visible="false" />
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">

                function OnWindowClose(sender, eventArgs) {
                    window.location.reload();
                }

            </script>
        </telerik:RadCodeBlock>

      
        <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">

            <div class="block">
                <div class="block-title">                    
                    <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="validationSummary" ValidationGroup="vgSearch" />
                </div>

                <div class="form-horizontal form-bordered">

                    <div class="form-group">
                        <label class="col-md-2 control-label">Email Display Name</label>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="txtEmailName" MaxLength="200" CssClass="form-control" />
                        </div>
                    </div>

                  <%--  <div class="form-group">
                        <label class="col-md-2 control-label">Include Inactive</label>
                        <div class="col-md-4">
                            <asp:CheckBox ID="chkActive" runat="server" CssClass="form-control form-control-borderless" />                            
                        </div>
                    </div>--%>
                </div>
                <br />
                <div class="form-group form-actions">
                    <label class="col-md-2 control-label"></label>

                    <span style="padding-left: 5px"></span>
                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgSearch"><i class="hi hi-search"></i> Search</asp:LinkButton>

                </div>
            </div>

            <asp:PlaceHolder ID="phSearchResults" runat="server">
                <asp:Literal ID="litDebug" runat="server" />
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong>Search Results</strong></h2>
                        <asp:PlaceHolder ID="phScrollNotes" runat="server"></asp:PlaceHolder>
                    </div>
                    <div class="table-responsive">
                        <telerik:RadGrid ID="rgGrid" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                            OnNeedDataSource="rgGrid_NeedDataSource"
                            OnItemDataBound="rgGrid_ItemDataBound"
                            OnItemCreated="rgGrid_ItemCreated"
                            OnDataBound="rgGrid_DataBound"
                            OnItemCommand="rgGrid_OnRowCommand"
                            AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" ShowFooter="false" PageSize="25" Width="100%">
                            <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="Id" Width="100%" CommandItemDisplay="Top">
                                <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                                <NoRecordsTemplate>
                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                </NoRecordsTemplate>
                                <CommandItemTemplate>
                                    <div style="margin: 2px 5px; display: inline; float: left">
                                        <asp:HyperLink ID="hlAddNewRecord" runat="server" NavigateUrl="~/Maintenance/CRM/EmailDetail.aspx?ID=0"><i class="gi gi-plus" title="Add New Email"></i>  Add New Email</asp:HyperLink>
                                    </div>
                                </CommandItemTemplate>
                                <Columns>

                                    <telerik:GridTemplateColumn DataField="Id" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="8%" HeaderText="" UniqueName="ActionColumn">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="btnEdit" runat="server" NavigateUrl='<%# Eval("Id","/Maintenance/CRM/EmailDetail.aspx?Id={0}") %>'><i class="gi gi-pencil" title="Edit"></i></asp:HyperLink>

                                            <asp:LinkButton ID="btnDeleteRecord" runat="server" CommandName="Delete"
                                                OnClientClick="javascript:return confirm('Are you sure want to delete this record?');"
                                                CommandArgument='<%# Eval("Id") %>'> <i class="gi gi-delete" title="Delete"></i></asp:LinkButton>

                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridBoundColumn DataField="EmailName" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40%" HeaderText="Display Name" SortExpression="EmailName" UniqueName="EmailName" />
                                    <telerik:GridBoundColumn DataField="ProductName" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50%" HeaderText="Lead Product" SortExpression="ProductName" UniqueName="ProductName" />
                              
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" />
                        </telerik:RadGrid>
                    </div>
                </div>
            </asp:PlaceHolder>


        </asp:Panel>


    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="ralpMenu" runat="server" Transparency="70" BackColor="#69b899">
        <i class="fa fa-spinner fa-4x fa-spin"></i>
    </telerik:RadAjaxLoadingPanel>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" runat="Server">
</asp:Content>




