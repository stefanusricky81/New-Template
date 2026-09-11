<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TimePicker.ascx.cs" Inherits="UserControl_DateTime_TimePicker" %>
<div class="input-group bootstrap-timepicker">
    <asp:TextBox ID="txtTime" runat="server" MaxLength="20" CssClass="form-control input-timepicker" />
    <asp:PlaceHolder ID="phButton" runat="server">
        <span class="input-group-btn">
            <a href="javascript:void(0)" class="btn btn-primary"><i class="fa fa-clock-o"></i></a>
        </span>
    </asp:PlaceHolder>
</div>
<asp:RequiredFieldValidator ID="rfvTime" ControlToValidate="txtTime" ErrorMessage="Time is required" ForeColor="red" runat="server" Display="None" />
<asp:Literal ID="litJs" runat="server" />

