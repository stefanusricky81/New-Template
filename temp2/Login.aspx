<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script type="text/javascript">
    function textFocus() {
        document.getElementById('txtEmail').value = '';
    }
    function textBlur() {
        document.getElementById('txtEmail').value = 'Youre email here';
    }
    function doClick(buttonName, e) {
        var key;

        if (window.event)
            key = window.event.keyCode;     //IE
        else
            key = e.which;     //firefox

        if (key == 13) {
            //Get the button the user wants to have clicked
            var btn = document.getElementById(buttonName);
            if (btn != null) { //If we find the button click it f79407
                btn.click();
                event.keyCode = 0
            }
        }
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div>
        <asp:Label ID="tStatus" runat="server" Font-Names="Tahoma" ForeColor="Red"/>
    </div>
    <div class="center_title_bar">Log In Form</div>
    <div class="prod_box_big">
        <div class="center_prod_box_big">
          <div class="contact_form">
            <div class="form_row">
              <label class="contact"><strong>Email :</strong></label>
              <asp:TextBox ID="txtEmail" runat="server" />
            </div>
            <div class="form_row">
              <label class="contact"><strong>Password:</strong></label>
              <asp:TextBox ID="txtPass" TextMode="Password" runat="server" />
            </div>
            <div class="form_row">
                <asp:Button class="button" ID="btnSignIn" Text="Sign In" runat="server" 
                    onclick="btnSignIn_Click" />
                <asp:Button class="button" ID="btnCancel" Text="Cancel" runat="server" 
                    onclick="btnCancel_Click" />
            </div>
          </div>
        </div>
      </div>  
</asp:Content>

