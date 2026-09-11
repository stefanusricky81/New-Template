<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DatePicker.ascx.cs" Inherits="UserControl_DateTime_DatePicker" %>
<asp:Literal ID="litDivStart" runat="server" Text='<div class="input-group date">' />
    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" MaxLength="10" />
    <asp:PlaceHolder ID="phButton" runat="server">
        <span class="input-group-btn">
            <a href="javascript:void(0)" class="btn btn-primary"><i class="fa fa-calendar-plus-o"></i></a>
        </span>
    </asp:PlaceHolder>
<asp:Literal ID="litDivEnd" runat="server" Text="</div>" />
<asp:RequiredFieldValidator ID="rfvDate" ControlToValidate="txtDate" ErrorMessage="Date is required" ForeColor="red" runat="server" Display="Dynamic">
    <asp:Literal ID="litError" runat="server" Text="*" Visible="false" />
</asp:RequiredFieldValidator>
<asp:RangeValidator ID="rvDate" ControlToValidate="txtDate" ErrorMessage="Date is required" runat="server" Display="None" MinimumValue="01/01/1900" MaximumValue="01/01/2200" Type="Date"/>
<asp:CustomValidator ID="cvDate" ControlToValidate="txtDate" ErrorMessage="Date is required" runat="server" Display="None" OnServerValidate="cvDate_ServerValidate" ValidateEmptyText="true"/>
<asp:Literal ID="litJs" runat="server" />

    