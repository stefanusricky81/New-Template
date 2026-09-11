<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ContactUs.aspx.cs" Inherits="contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<br /><br /><br />
      <div class="prod_box_big">
        <div class="center_prod_box_big">
            <img src="images/doll.jpg" class="doll" />
            <ul>
                <li>
                    <img src="images/clock.jpg" />
                    <h1>OFFICE HOUR</h1>
                    <p>
                        <asp:Label ID ="lblOfficeDay" runat="server" />&nbsp&nbsp
                        <asp:Label ID ="lblOfficeHour" runat="server" />
                    </p>
                </li>
            </ul>
             <ul>
                <li>
                    <img src="images/phone.jpg" />
                    <h1>CALL US</h1>
                    <p>
                        <asp:Label ID ="lblPhone1" runat="server" />&nbsp&nbsp/&nbsp&nbsp
                        <asp:Label ID ="lblPhone2" runat="server" />
                    </p>
                </li>
            </ul>
             <ul>
                <li>
                    <img src="images/sms.jpg" />
                    <h1>SMS US</h1>
                    <p>
                        <asp:Label ID ="lblSms" runat="server" />
                    </p>
                </li>
            </ul>
             <ul>
                <li>
                    <img src="images/email.jpg" />
                    <h1>EMAIL US</h1>
                    <p>
                        <asp:Label ID ="lblEmail" runat="server" />
                    </p>
                </li>
            </ul>
          
        </div>
      </div>
      <br /><br /><br /><br /><br /><br /><br />
</asp:Content>

