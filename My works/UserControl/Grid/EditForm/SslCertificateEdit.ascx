<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SslCertificateEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_SslCertificateEdit" %>
<%@ Register TagPrefix="uc" TagName="SslCertificateTypeDDL" Src="~/UserControl/DropDownList/SslCertificateType.ascx" %>

<div id="divMain" runat="server" style="background-color:#fafbfc; padding:5px 5px 5px 5px; width:99%;">

<table border="0" class="gridEditForm" width="100%" style="width:100%">
     <tr>
        <td colspan="2" style="text-align:center;">
            <asp:MultiView ID="mvButtonTop" runat="server">
            
                <asp:View ID="viewGridTop" runat="server">
                    <asp:button id="btnUpdateTop" text="UPDATE" runat="server" 
                        CssClass="actionbutton" ValidationGroup="vgSslCertificate" CommandName="Update">
                    </asp:button>
                    <asp:button id="btnInsertTop" text="ADD" runat="server" 
                        CssClass="actionbutton" ValidationGroup="vgSslCertificate" CommandName="PerformInsert">
                    </asp:button>
                    &nbsp;
                    <asp:Button ID="btnCloseTop" Text="CLOSE" runat="server" 
                        CssClass="actionbutton2" CommandName="Cancel"/>
                </asp:View>
                
                <asp:View ID="viewPageTop" runat="server">
                </asp:View>
                
            </asp:MultiView>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:ValidationSummary ID="vsSslCertificate" runat="server" 
                CssClass="ValidationSummary" ValidationGroup="vgSslCertificate" />
        </td>
    </tr>
    <asp:PlaceHolder ID="phSslCertificateId" runat="server">
        <tr>
            <th style="width:135px;">Id:</th>
            <td>
                <asp:HyperLink ID="hlSslCertificateId" runat="server" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <th style="width:135px;" class="Required">FQDN:</th>
        <td>
            <asp:TextBox ID="tbFqdn" runat="server" CssClass="textbox" MaxLength="80" Width="400"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvFqDn" runat="server" ControlToValidate="tbFqdn" 
                ErrorMessage="FQDN is required" ValidationGroup="vgSslCertificate">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th style="width:135px;" class="Required">Start Date:</th>
        <td>
             <telerik:RadDatePicker id="rdpStartDate" Runat="server" SharedCalendarID="sharedCalendar" 
                Width="120px" EnableEmbeddedSkins="false" Skin="BitByBit">
            </telerik:RadDatePicker>
            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Start Date is required" 
                ControlToValidate="rdpStartDate" ValidationGroup="vgSslCertificate">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
            <telerik:RadCalendar ID="sharedCalendar" runat="server" EnableMultiSelect="false" EnableEmbeddedSkins="false" Skin="BitByBit" ShowRowHeaders="false">
                <FastNavigationSettings EnableTodayButtonSelection="true" />
            </telerik:RadCalendar>
        </td>
    </tr>
    <tr>
        <th style="width:135px;" class="Required">End Date:</th>
        <td>
             <telerik:RadDatePicker id="rdpEndDate" Runat="server" SharedCalendarID="sharedCalendar" 
                Width="120px" EnableEmbeddedSkins="false" Skin="BitByBit">
            </telerik:RadDatePicker>
            <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="End Date is required" 
                ControlToValidate="rdpEndDate" ValidationGroup="vgSslCertificate">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
            <asp:CompareValidator ID="cvDates" runat="server" ControlToValidate="rdpEndDate" ControlToCompare="rdpStartDate" 
                Operator="GreaterThan" Type="Date" ValidationGroup="vgSslCertificate" ErrorMessage="End Date must be greater than Start Date">
                <span class="error">*</span>
            </asp:CompareValidator>
        </td>
    </tr>
    <tr>
        <th style="width:135px;" class="Required">Certificate Type:</th>
        <td>
            <uc:SslCertificateTypeDDL ID="ucSslCertificateTypeDDL" runat="server" Width="405"
                IsRequired="true" DisplayDefaultValue="true" ValidationGroup="vgSslCertificate" />
        </td>
    </tr>   
    <tr>
        <th style="width:135px;" class="Required">Client:</th>
        <td>
            <uc:ClientComboBox ID="ucClientComboBox" runat="server" 
                ValidationGroup="vgSslCertificate" IsRequired="true" Width="405"/>
        </td>
    </tr>   
    <tr>
        <th style="width:135px; vertical-align:top;">Description:</th>
        <td>
            <asp:TextBox ID="tbDescription" runat="server" TextMode="MultiLine" 
                Rows="10" CssClass="textbox" Width="400"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th style="width:135px; vertical-align:top;">Purchased By Client:</th>
        <td>
            <asp:CheckBox ID="chkPurchasedByClient" runat="server" />
        </td>
    </tr>
    <asp:PlaceHolder ID="phExistingFields" runat="server">
        <tr>
            <th style="width:135px;">Archived:</th>
            <td>
                <asp:CheckBox ID="chkArchived" runat="server" />
            </td>
        </tr>
        <tr>
            <th style="width:135px;">Created:</th>
            <td>
                <asp:Literal ID="litCreated" runat="server" />
            </td>
        </tr>
        <tr>
            <th style="width:135px;">Last Updated:</th>
            <td>
                <asp:Literal ID="litLastUpdated" runat="server" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <td colspan="2" style="text-align:center;">
            <asp:MultiView ID="mvButton" runat="server">
            
                <asp:View ID="viewGrid" runat="server">
                    <asp:button id="btnUpdate" text="UPDATE" runat="server" 
                        CssClass="actionbutton" ValidationGroup="vgSslCertificate" CommandName="Update">
                    </asp:button>
                    <asp:button id="btnInsert" text="ADD" runat="server" 
                        CssClass="actionbutton" ValidationGroup="vgSslCertificate" CommandName="PerformInsert">
                    </asp:button>
                    &nbsp;
                    <asp:Button ID="btnClose" Text="CLOSE" runat="server" 
                        CssClass="actionbutton2" CommandName="Cancel"/>
                </asp:View>
                
                <asp:View ID="viewPage" runat="server">
                    <asp:Button ID="btnUpdatePage" runat="server" Text="UPDATE" 
                        CssClass="actionbutton" ValidationGroup="vgSslCertificate" 
                        OnClick="btnUpdatePage_Click" />
                </asp:View>
                
            </asp:MultiView>
        </td>
    </tr>
    
</table>    

</div>