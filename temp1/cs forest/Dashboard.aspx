<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="cs_forest.Dashboard" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 46 || charCode > 57))
                return false;
            else
                return true;

            function goBack() {
                window.history.back();
            }
        }
    </script>
    <style>
        .Absolute-Center {
            margin: auto;
            position: absolute;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <button onclick="goBack()" class="login-btn">Go Back</button>
            <div style="text-align: center" runat="server" id="panel_default">
                <img style="margin-top: 20px;" src="images/dashboard-banner.png" /><br />
                <div class="dashboard-form" runat="server" id="panelMT" style="margin: 0 5px 20px 5px; display: inline-block;">
                    <div>
                        <h3>MT</h3>
                        <img src="images/phone-icon.png" style="vertical-align: middle; margin-right: 3px;" />
                        <asp:TextBox ID="txtMsisdnMT" runat="server" Width="178" MaxLength="15" CssClass="input" onkeypress="return isNumber(event)" onblur="if(this.value=='')this.value='MSISDN';" value="MSISDN" onfocus="if(this.value=='MSISDN')this.value='';"></asp:TextBox>
                        <asp:Label runat="server" ID="lblMTExample" Font-Size="Smaller"></asp:Label>
                    </div>
                    <br />
                    <asp:Button ID="btnSearchMT" runat="server" OnClick="btnSearchMT_Click" Text="Search" CssClass="btn" />
                </div>
                <div class="dashboard-form" runat="server" id="panelMO" style="margin: 0 5px 20px 5px; display: inline-block;">
                    <div>
                        <h3>MO</h3>
                        <img src="images/phone-icon.png" style="vertical-align: middle; margin-right: 3px;" />
                        <asp:TextBox ID="txtMsisdnMO" runat="server" Width="178" MaxLength="15" CssClass="input" onkeypress="return isNumber(event)" onblur="if(this.value=='')this.value='MSISDN';" value="MSISDN" onfocus="if(this.value=='MSISDN')this.value='';"></asp:TextBox>
                        <asp:Label runat="server" ID="lblMOExample" Font-Size="Smaller"></asp:Label>
                    </div>
                    <br />
                    <asp:Button ID="btnSearchMO" runat="server" OnClick="btnSearchMO_Click" Text="Search" CssClass="btn" />
                </div>
                <div id="panel_subscriber" runat="server" class="dashboard-form" style="margin: 0 5px 20px 5px; display: inline-block; text-align: center;">
                    <div>
                        <h3>Subscriber</h3>
                        <img src="images/phone-icon.png" style="vertical-align: middle; margin-right: 3px;" />
                        <asp:TextBox ID="txtMsisdnSubs" runat="server" Width="178" MaxLength="15" CssClass="input" onkeypress="return isNumber(event)" onblur="if(this.value=='')this.value='MSISDN';" value="MSISDN" onfocus="if(this.value=='MSISDN')this.value='';"></asp:TextBox>
                        <asp:Label runat="server" ID="lblSubExample" Font-Size="Smaller"></asp:Label>

                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="MSISDN is not valid!" Font-Size="Small" ControlToValidate="txtMsisdnSubs" ValidationExpression="^\d+$" ForeColor="IndianRed" Display="Static"></asp:RegularExpressionValidator>--%>
                    </div>
                    <br />
                    <asp:Button ID="btnSearchSubs" runat="server" OnClick="btnSearchSubs_Click" Text="Search" CssClass="btn" />
                </div>
                <br />
                <div id="panel_blacklist" runat="server" class="dashboard-form" style="margin: 0 5px 20px 5px; display: inline-block; text-align: center;">
                    <h3>Blacklist</h3>
                    <%--<span style="color: #333333; font-size: 12px">Function </span>--%>
                    <asp:RadioButton ID="RBSearchBlack" GroupName="type" Font-Size="10" ForeColor="#333333" runat="server" Text="Search Blacklist" AutoPostBack="true" OnCheckedChanged="RBSearchBlack_CheckedChanged" />
                    <asp:RadioButton ID="RBAddBlack" GroupName="type" Font-Size="10" ForeColor="#333333" runat="server" Text="Add Blacklist" AutoPostBack="true" OnCheckedChanged="RBAddBlack_CheckedChanged" /><br />
                    <div style="margin-top: 15px">
                        <img src="images/phone-icon.png" style="vertical-align: middle; margin-right: 3px;" />
                        <asp:TextBox ID="txtBlacklist" runat="server" Width="178" MaxLength="15" CssClass="input" onkeypress="return isNumber(event)" onblur="if(this.value=='')this.value='MSISDN';" value="MSISDN" onfocus="if(this.value=='MSISDN')this.value='';"></asp:TextBox>
                        <asp:Label runat="server" ID="lblBlackListExample" Font-Size="Smaller"></asp:Label>
                        <asp:Label ID="lblMsgBlack" runat="server" Text="" ForeColor="#009933" Font-Bold="true" Font-Size="Small"></asp:Label>
                    </div>
                    <asp:Button ID="btnBlacklist" OnClick="btnBlacklist_Click" runat="server" Text="Search" CssClass="btn" />

                </div>
                </div>
                <asp:Panel runat="server" ID="pnlDescription" Visible="false">
                   
                        <h3>GUIDELINES:</h3>
                        
                            <asp:BulletedList ID="BulletedList1" runat="server" Font-Size="10pt" ForeColor="#333333">
                                <asp:ListItem>MT SEARCH: Key in the country code followed by the remaining msisdn to search for the message sent  to that specific msisdn.   Example:  63 xxxxxxxxx</asp:ListItem>
                                <asp:ListItem>MO SEARCH:  Key in the country code followed by the remaining msisdn to search for the message received to that specific msisdn.  Example:  63 xxxxxxxxx</asp:ListItem>
                                <asp:ListItem>SEARCH SUBSCRIBER:  Key in the country code followed by the remaining msisdn to search and view whether  that specific msisdn is subscribed. “No Data to display” will prompt if msisdn is unsubscribed/not in the system.  Example:  63 xxxxxxxxx</asp:ListItem>
                                <asp:ListItem>SEARCH BLACKLIST:  Key in in the country code followed by the remaining msisdn to search and view the blacklisted msisdn in the system. Blacklisted msisdn in the system shows its timestamp. Example:  63xxxxxxxxx</asp:ListItem>
                                <asp:ListItem>ADD BLACKLIST:  Key in in the country code followed by the remaining msisdn to add the msisdn to the blacklisted list in the system. Blacklisted msisdn will automatically unsubscribe to the service and will no longer receive any messages nor be charged in the future by Game world.  Example:  63 xxxxxxxxx
</asp:ListItem>

                            </asp:BulletedList>
                       <%-- <p>
                            <asp:Label ID="lblMTDesc" runat="server" Text="MT Search: User can key in msisdn with the country code to search for the message send to that phone number(msisdn) for example 63912345678" Font-Size="Small"></asp:Label>
                        </p>
                        <p>
                            <asp:Label ID="Label1" runat="server" Text="MO Search: User can key in msisdn with the country code to search for the message received from that phone number(msisdn) for example 63912345678" Font-Size="Small"></asp:Label>
                        </p>
                        <p>
                            <asp:Label ID="Label2" runat="server" Text="Subscriber Search: User can key in msisdn with the country code to search for the user if it's subscribe to the service, for example 63912345678" Font-Size="Small"></asp:Label>
                        </p>
                        <p>
                            <asp:Label ID="Label3" runat="server" Text="Blacklist Search: User can key in msisdn with the country code to search for the user if it's in the black list system, for example 63912345678" Font-Size="Small"></asp:Label>
                        </p>
                        <p>
                            <asp:Label ID="Label4" runat="server" Text="Blacklist Ad: User can key in msisdn with the country code to add the user into black list system, this user will automatically unsubscribe from the system and will not get any message in future for example 63912345678" Font-Size="Small"></asp:Label>
                        </p>--%>
                   
                </asp:Panel>
            
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="updProgress"
        AssociatedUpdatePanelID="UpdatePanel1"
        runat="server">
        <ProgressTemplate>
            <img alt="progress" src="Images/ajax-loader.gif" class="Absolute-Center" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
