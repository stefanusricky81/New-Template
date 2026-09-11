<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketBatchEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_TicketBatchEdit" %>
<%@ Register TagPrefix="uc" TagName="TicketTypeDDL" Src="~/UserControl/DropDownList/TicketType.ascx" %>
<style type="text/css">
    div.RadToolTip_BitByBit
    {
	    position: absolute;
	    cursor: default;
	    margin-left:10px;
    }
</style>
<table border="1" class="gridEditForm" width="100%" style="width:100%">
    <tr>
        <td style="width:6%">
            <telerik:RadToolTip ID="rttTicketDescription" runat="server" TargetControlID="lblTicketNumber" Title="Ticket Description"
                Skin="BitByBit" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" RelativeTo="Element"
                Animation="Slide" ManualClose="false" Width="400px" AutoCloseDelay="5000"/>
            <asp:Label ID="lblTicketNumber" runat="server" />
        </td>
        <td style="width:18%">
            <asp:TextBox ID="tbSummary" runat="server" Width="220" MaxLength="200"/>
            <asp:RequiredFieldValidator ID="rfvSummary" runat="server" ControlToValidate="tbSummary" 
                ErrorMessage="Summary is required" ValidationGroup="vgTicketBatchEdit">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
        </td>
        <td style="width:18%">
            <uc:ClientComboBox ID="ucClient" runat="server" Width="225"
                IsRequired="true" ValidationGroup="vgTicketBatchEdit" 
                ValidationErrorMessage="Client Name is required"/>
        </td>
        <td style="width:14%">
            <uc:TicketDispositionDDL ID="ucTicketDisposition" runat="server" DefaultValue=""
                DefaultText="" CssClass="dropdown" Width="150" ValidationGroup="vgTicketBatchEdit"/>
        </td>
        <td style="width:11%">
            <uc:TicketPriorityDDL ID="ucTicketPriority" runat="server" DefaultValue="-1" DefaultText="" CssClass="dropdown" Width="100" />
        </td>
        <td style="width:11%">
            <uc:EmployeeDDL ID="ucEmployeeAssignedTo" runat="server" DefaultValue="" 
                RequiredErrorMessage="Assigned To is required" CssClass="dropdown"
                ValidationGroup="vgTicketBatchEdit" Width="150" />
        </td>
        <td style="width:11%">
            <uc:ProductDDL ID="ucTicketProduct" runat="server" DefaultText="--Select Product--" DefaultValue=""
                IsRequired="false" ValidationGroup="vgTicketBatchEdit" CssClass="dropdown" Width="150"/>
        </td>
       <td style="width:11%">
            <uc:TicketTypeDDL ID="ucTicketType" runat="server" DefaultText="-- Select Type--" DefaultValue="" DisplayDefaultValue="true" DisplayChosenScript="false"
                IsRequired="false" ValidationGroup="vgTicketBatchEdit" CssClass="dropdown" Width="150"/>
        </td>
    </tr>
</table>