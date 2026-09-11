<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="UploadBillingUsers.aspx.cs" Inherits="Maintenance_UploadBillingUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <style type="text/css">
        /*.RadUpload .ruBrowse
        {
         display:none !important;
        }*/

        .upload1 
        {
        background-position: 0 -46px !important;
        width: 120px !important;
        height:20px !important;
        } 
    </style>
    <script>
        function validateUpload(sender, args) {
            var upload = $find("<%=rauFileUpload.ClientID%>");
            args.IsValid = upload.getUploadedFiles().length != 0;
        }
    </script>

    <div class="content-header">
        <div class="header-section">
            <h1>Client Project Billing Users Import/Export</h1>
        </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" />
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnImport">
        <div class="block">
            <div class="row">
                <div class="form-group col-sm-2">
                    <label>Upload File</label>
                </div>
                <div class="form-group col-sm-10">
                    <telerik:RadAsyncUpload RenderMode="Lightweight" EnableInlineProgress="true" runat="server" CssClass="upload1" ID="rauFileUpload" MaxFileInputsCount="1" />
                    <asp:CustomValidator runat="server" ID="cvUpload" ValidationGroup="vgUpload" Display="None" ClientValidationFunction="validateUpload" ErrorMessage="Please Select File To Upload" />
                    
                </div>
                
            </div>
            <div class="row">
                <div class="form-group col-sm-2"></div>
                 <div class="form-group col-sm-10">
                     <span class="text-danger">* Upload file must be in .xlsx format.</span>
                 </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <asp:Button ID="btnImport" runat="server" Text="Import" class="btn btn-primary" ValidationGroup="vgImport" OnClick="btnImport_Click"/>
                    <asp:Button ID="btnExport" runat="server" Text="Export to Excel (project type MSP only)" class="btn btn-primary" OnClick="btnExport_Click"/>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlResults" runat="server" Visible="false">
        <div class="block" style="padding-bottom: 20px;">
            <div class="table-responsive scroll">
                <telerik:RadGrid ID="rgUploadBilling" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="10" Width="150%" OnNeedDataSource="rgUploadBilling_NeedDataSource"
                    ClientSettings-Resizing-AllowColumnResize="true"
                     >
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="ProjectID" Width="150%" AllowSorting="true">
                        
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="Client" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Client" SortExpression="Client" UniqueName="Client" />
                            <telerik:GridBoundColumn DataField="ProjectID" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="ProjectID" SortExpression="ProjectID" UniqueName="ProjectID" />
                            <telerik:GridBoundColumn DataField="MRR" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="MRR" SortExpression="MRR" UniqueName="MRR" />
                            <telerik:GridBoundColumn DataField="FeeMonthlyMSP" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="FeeMonthlyMSP" SortExpression="FeeMonthlyMSP" UniqueName="FeeMonthlyMSP" />
                            <telerik:GridBoundColumn DataField="FeeHowManyUsers" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="FeeHowManyUsers" SortExpression="FeeHowManyUsers" UniqueName="FeeHowManyUsers" />
                            <telerik:GridBoundColumn DataField="ActualCurrentO365" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="ActualCurrentO365" SortExpression="ActualCurrentO365" UniqueName="ActualCurrentO365" />
                            <telerik:GridBoundColumn DataField="EachAddlUsers" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="EachAddlUsers" SortExpression="EachAddlUsers" UniqueName="EachAddlUsers" />
                            <telerik:GridBoundColumn DataField="AddlBillingUserIncrement" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="AddlBillingUserIncrement" SortExpression="AddlBillingUserIncrement" UniqueName="AddlBillingUserIncrement" />
                            <telerik:GridBoundColumn DataField="AdditionalUserNotes" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="AdditionalUserNotes" SortExpression="AdditionalUserNotes" UniqueName="AdditionalUserNotes" />
                            <telerik:GridBoundColumn DataField="IncludedDeviceSetUp" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="IncludedDeviceSetUp" SortExpression="IncludedDeviceSetUp" UniqueName="IncludedDeviceSetUp" />
                            <telerik:GridBoundColumn DataField="AddlDeviceFee" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="AddlDeviceFee" SortExpression="AddlDeviceFee" UniqueName="AddlDeviceFee" />
                            <telerik:GridBoundColumn DataField="OnsiteDayRates" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="OnsiteDayRates" SortExpression="OnsiteDayRates" UniqueName="OnsiteDayRates" />
                            <telerik:GridBoundColumn DataField="CappedHours" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="CappedHours" SortExpression="CappedHours" UniqueName="CappedHours" />
                            <telerik:GridBoundColumn DataField="ContractTerm" DataType="System.String" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Left" HeaderText="ContractTerm" SortExpression="ContractTerm" UniqueName="ContractTerm" />
                            <telerik:GridBoundColumn DataField="BackupDataSizeIncluded" DataType="System.String" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Left" HeaderText="BackupDataSizeIncluded" SortExpression="BackupDataSizeIncluded" UniqueName="BackupDataSizeIncluded" />
                            <telerik:GridBoundColumn DataField="ContractFound" DataType="System.String" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Left" HeaderText="ContractFound" SortExpression="ContractFound" UniqueName="ContractFound" />
                            <telerik:GridBoundColumn DataField="ContractSignedDate" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="ContractSignedDate" SortExpression="ContractSignedDate" UniqueName="ContractSignedDate" />
                            <telerik:GridBoundColumn DataField="TerminationNotification" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="TerminationNotification" SortExpression="TerminationNotification" UniqueName="TerminationNotification" />
                            <telerik:GridBoundColumn DataField="ContractEndDate" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="ContractEndDate" SortExpression="ContractEndDate" UniqueName="ContractEndDate" />                         
                            <telerik:GridBoundColumn DataField="UsingClientMSA" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="UsingClientMSA" SortExpression="UsingClientMSA" UniqueName="UsingClientMSA" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </asp:Panel>

    <%--<asp:Panel ID="pnlExport" runat="server" Visible="true">--%>
        <%--<div class="block" style="padding-bottom: 20px;">
            <div class="table-responsive scroll">
                <telerik:RadGrid ID="rgExport" Visible="false" runat="server" Skin="3b" EnableEmbeddedSkins="false" AutoGenerateColumns="false" AllowSorting="true" 
                    AllowPaging="true" ShowFooter="false" PageSize="10" Width="150%" OnNeedDataSource="rgExport_NeedDataSource"
                    ClientSettings-Resizing-AllowColumnResize="true"
                     >
                    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="Fixed" DataKeyNames="ProjectID" Width="150%" AllowSorting="true">
                        
                        <NoRecordsTemplate>
                            <div style="padding: 8px 0 8px 5px;">No Records found.</div>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="Client" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Client" SortExpression="Client" UniqueName="Client" />
                            <telerik:GridBoundColumn DataField="ProjectID" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="ProjectID" SortExpression="ProjectID" UniqueName="ProjectID" />
                            <telerik:GridBoundColumn DataField="MRR" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="MRR" SortExpression="MRR" UniqueName="MRR" />
                            <telerik:GridBoundColumn DataField="FeeforjustmonthlyMSP" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="FeeforjustmonthlyMSP" SortExpression="FeeforjustmonthlyMSP" UniqueName="FeeforjustmonthlyMSP" />
                            <telerik:GridBoundColumn DataField="Feeincludesuptohowmanyusers" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Feeincludesuptohowmanyusers" SortExpression="Feeincludesuptohowmanyusers" UniqueName="Feeincludesuptohowmanyusers" />
                            <telerik:GridBoundColumn DataField="NumberOfO365Licenses" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="NumberOfO365Licenses" SortExpression="NumberOfO365Licenses" UniqueName="NumberOfO365Licenses" />
                            <telerik:GridBoundColumn DataField="ActualCurrentO365Users" HeaderStyle-Width="7%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="ActualCurrentO365Users" SortExpression="ActualCurrentO365Users" UniqueName="ActualCurrentO365Users" />
                            <telerik:GridBoundColumn DataField="HowMuchForEachAddlUser" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="HowMuchForEachAddlUser" SortExpression="HowMuchForEachAddlUser" UniqueName="HowMuchForEachAddlUser" />
                            <telerik:GridBoundColumn DataField="AddlBillingUserIncrement" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="AddlBillingUserIncrement" SortExpression="AddlBillingUserIncrement" UniqueName="AddlBillingUserIncrement" />
                            <telerik:GridBoundColumn DataField="AdditionalUserNotes" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="AdditionalUserNotes" SortExpression="AdditionalUserNotes" UniqueName="AdditionalUserNotes" />
                            <telerik:GridBoundColumn DataField="IncludedDeviceSetUp" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="IncludedDeviceSetUp" SortExpression="IncludedDeviceSetUp" UniqueName="IncludedDeviceSetUp" />
                            <telerik:GridBoundColumn DataField="AddlDeviceFee" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="AddlDeviceFee" SortExpression="AddlDeviceFee" UniqueName="AddlDeviceFee" />
                            <telerik:GridBoundColumn DataField="OnSitesDaysRates" DataType="System.String" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="OnSitesDaysRates" SortExpression="OnSitesDaysRates" UniqueName="OnSitesDaysRates" />
                            <telerik:GridBoundColumn DataField="CappedHours" DataType="System.String" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Left" HeaderText="CappedHours" SortExpression="CappedHours" UniqueName="CappedHours" />
                            <telerik:GridBoundColumn DataField="ContractTerm" DataType="System.String" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Left" HeaderText="ContractTerm" SortExpression="ContractTerm" UniqueName="ContractTerm" />
                            <telerik:GridBoundColumn DataField="BackupDataSizeIncluded" DataType="System.String" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Left" HeaderText="BackupDataSizeIncluded" SortExpression="BackupDataSizeIncluded" UniqueName="BackupDataSizeIncluded" />
                            <telerik:GridBoundColumn DataField="ContractSignedDate" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="ContractSignedDate" SortExpression="ContractSignedDate" UniqueName="ContractSignedDate" />
                            <telerik:GridBoundColumn DataField="TerminationNotification" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="TerminationNotification" SortExpression="TerminationNotification" UniqueName="TerminationNotification" />
                            <telerik:GridBoundColumn DataField="ContractEndDate" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="ContractEndDate" SortExpression="ContractEndDate" UniqueName="ContractEndDate" />                         
                            <telerik:GridBoundColumn DataField="UsingClientMSA" HeaderStyle-Width="20%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="UsingClientMSA" SortExpression="UsingClientMSA" UniqueName="UsingClientMSA" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>--%>
    <%--</asp:Panel>--%>
</asp:Content>