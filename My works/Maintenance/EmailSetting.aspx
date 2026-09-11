<%@ Page Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="EmailSetting.aspx.cs" Inherits="Email_Default" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server" />

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">

    <div class="content-header">
        <div class="header-section">
            <h1>Email Setting</h1>
        </div>
    </div>

    <telerik:RadAjaxPanel ID="rapUser" runat="server" LoadingPanelID="ralpUser" ClientEvents-OnRequestStart="conditionalPostback">
        <asp:Literal ID="litMessage" runat="server" Visible="false" />

        <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">

            <asp:PlaceHolder ID="phSearchResults" runat="server">
                <asp:Literal ID="litDebug" runat="server" />
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong>Email Setting</strong></h2>
                        <asp:PlaceHolder ID="phScrollNotes" runat="server"></asp:PlaceHolder>
                    </div>

                    <div class="table-responsive" style="width: 40%">
                        <telerik:RadGrid ID="rgEmail" runat="server" Skin="3b" EnableEmbeddedSkins="false"
                            OnNeedDataSource="rgEmail_NeedDataSource"
                            OnItemDataBound="rgEmail_ItemDataBound"
                            AutoGenerateColumns="false" AllowSorting="true" AllowPaging="false" ShowFooter="false"  Width="100%">
                            <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" Width="100%" CommandItemDisplay="None">
                                <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
                                <NoRecordsTemplate>
                                    <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                                </NoRecordsTemplate>

                                <Columns>
                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="15%" HeaderText="Email" UniqueName="ActionColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" Text="Email: " runat="server"></asp:Label>

                                            <asp:Label ID="lblEmail" Text='<%# Eval("EmailNumber") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="35%" HeaderText="Days Before Contract Expiration" UniqueName="ActionColumn">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("ID") %>' />
                                             <asp:HiddenField ID="hfDays" runat="server" Value='<%# Eval("NumberOfDays") %>' />
                                            <asp:TextBox ID="txtDays" runat="server" CssClass="form-control" TextMode="Number" min="0" max="100" step="1" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>


                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" />
                        </telerik:RadGrid>
                        <br />
                    </div>
                    <div class="form-group form-actions">
                        <br />

                        <asp:LinkButton ID="btnSubmit" runat="server" ValidationGroup="vgMenu" OnClick="btnSubmit_Click" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-ok"></i> Save</asp:LinkButton>
                    </div>
                </div>

            </asp:PlaceHolder>
        </asp:Panel>
    </telerik:RadAjaxPanel>

</asp:Content>

