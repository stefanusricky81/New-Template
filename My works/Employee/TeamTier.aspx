<%@ Page Title="Bit By Bit Intranet - Team and Tier Organization" Language="C#" MasterPageFile="~/Template/Base.master" AutoEventWireup="true" CodeFile="TeamTier.aspx.cs" Inherits="Employee_TeamTier" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
<h2>Team & Tier Organization</h2>

<div style="float:left; width:350px;">
    <h3>Teams</h3>
    <asp:Literal ID="litTeams" runat="server" />
</div>
<div style="float:left; width:350px;">
    <h3>Tiers</h3>
    <asp:Literal ID="litTiers" runat="server" />
</div>
</asp:Content>

