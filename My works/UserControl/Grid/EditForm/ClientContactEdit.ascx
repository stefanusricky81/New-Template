<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientContactEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_ClientContactEdit" %>



<style type="text/css">
    .auto-style1 {
        height: 61px;
    }
    .auto-style2 {
        width: 694px;
    }
</style>



<div id="divMain" runat="server" style="background-color:#fafbfc; padding:5px 5px 5px 5px; width:99%;">
     
<table border="0" class="gridEditForm" width="100%" style="width:100%">

     <tr>

        <td colspan="2" style="text-align:center;">

            <asp:MultiView ID="mvButtonTop" runat="server">
                       
                <asp:View ID="viewGridTop" runat="server">

                    <asp:button id="btnUpdateTop" text="UPDATE" runat="server" 
                        CssClass="actionbutton" ValidationGroup="vgClientContact" CommandName="Update">
                                             
                    </asp:button>

                    <asp:button id="btnInsertTop" text="ADD" runat="server" 
                        CssClass="actionbutton" ValidationGroup="vgClientContact" CommandName="PerformInsert">
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
        <td colspan="2" class="auto-style1">
            <asp:ValidationSummary ID="vsClientContact" runat="server" 
                CssClass="ValidationSummary" ValidationGroup="vgSClientContact" />
        </td>
    </tr>
    <td colspan="1" style="text-align:left;" class="auto-style2">
        <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Client:
                    <uc:ClientComboBox ID="ucClientComboBox" runat="server" />
    <br /> <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
     <asp:PlaceHolder ID="phClientContactId" runat="server">
     
            
            Id: 
                <asp:HyperLink ID="hlClientContactId" runat="server" />

    </asp:PlaceHolder>
                    <br /> <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    *First:
            <asp:TextBox ID="tbFirst" runat="server" CssClass="textbox" MaxLength="20" Width="260px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvFirst" runat="server" ControlToValidate="tbFirst" 
                ErrorMessage="First Name is required" ValidationGroup="vgClientContact">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
                    <br /> <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
       *Last:
            <asp:TextBox ID="tbLast" runat="server" CssClass="textbox" MaxLength="20" Width="260px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvLast" runat="server" ControlToValidate="tbLast" 
                ErrorMessage="Last Name is required" ValidationGroup="vgClientContact">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
                    <br /> <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
     *Email:
            <asp:TextBox ID="tbEmail" runat="server" CssClass="textbox" MaxLength="30" Width="260px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="tbEmail" 
                ErrorMessage="Email is required" ValidationGroup="vgClientContact">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
                    <br /> <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
       Email2:
            <asp:TextBox ID="tbEmail2" runat="server" CssClass="textbox" MaxLength="30" Width="260px"></asp:TextBox>
       <br /> <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        Email3:
            <asp:TextBox ID="tbEmail3" runat="server" CssClass="textbox" MaxLength="50" Width="260px"></asp:TextBox>
       <br /> <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
       *Phone:
            <asp:TextBox ID="tbPhone" runat="server" CssClass="textbox" MaxLength="12" Width="260px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="tbPhone" 
                ErrorMessage="Phone number is required" ValidationGroup="vgClientContact">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
            Ext:&nbsp;
            <asp:TextBox ID="tbExt" runat="server" MaxLength="6" Width="70px"></asp:TextBox>
                    <br /> <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
       Fax:
            <asp:TextBox ID="tbFax" runat="server" CssClass="fax" MaxLength="14" Width="260px" AutoCompleteType="HomeFax" ></asp:TextBox>
<br /> <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        Cell Phone:
        
            <asp:TextBox ID="tbCellPhone" runat="server" CssClass="textbox" MaxLength="12" Width="260px"></asp:TextBox>
        <br /> <br />
        <!-- <asp:Literal ID="litLastUpdated" runat="server" /> -->

    <asp:PlaceHolder ID="phExistingFields" runat="server">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        Created:<asp:Label ID="tbCreated" runat="server" CssClass="textbox" MaxLength="20" Width="125px"></asp:Label>
            <br /> <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
Last Updated: <asp:Label ID="tbLastUpdated" runat="server" CssClass="textbox" MaxLength="20" Width="125px"></asp:Label>
            <br /> <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
Last Updated By: <asp:TextBox ID="tbLastUpdatedBy" runat="server" CssClass="textbox" MaxLength="20" Width="104px"></asp:TextBox>
            <br /> <br />
        </asp:PlaceHolder>
        </td>
    <td colspan="1" style="text-align:left;" >
        Address1:
            <asp:TextBox ID="tbAddress1" runat="server" CssClass="textbox" MaxLength="50" Width="260px"></asp:TextBox>
<br /> <br />

Address2
            <asp:TextBox ID="tbAddress2" runat="server" CssClass="textbox" MaxLength="50" Width="260px"></asp:TextBox>
<br /> <br />        City:
            <asp:TextBox ID="tbCity" runat="server" CssClass="textbox" MaxLength="20" Width="260px"></asp:TextBox>
<br /> <br />
 State:
            <asp:DropDownList ID="ddlState" runat="server" MaxLength="20" Width="45px">
                <asp:ListItem value=""></asp:ListItem>
                  <asp:ListItem value="AK">AK</asp:ListItem>
                  <asp:ListItem Value="AL">AL</asp:ListItem>
                  <asp:ListItem value="AR">AR</asp:ListItem>
                  <asp:ListItem value="AZ">AZ</asp:ListItem>
                  <asp:ListItem value="CA">CA</asp:ListItem>
                  <asp:ListItem value="CO">CO</asp:ListItem>
                  <asp:ListItem value="CT">CT</asp:ListItem>
                  <asp:ListItem value="DC">DC</asp:ListItem>
                  <asp:ListItem value="DE">DE</asp:ListItem>
                  <asp:ListItem value="FL">FL</asp:ListItem>
                  <asp:ListItem value="GA">GA</asp:ListItem>
                  <asp:ListItem value="HI">HI</asp:ListItem>
                  <asp:ListItem value="IA">IA</asp:ListItem>
                  <asp:ListItem value="ID">ID</asp:ListItem>
                  <asp:ListItem value="IL">IL</asp:ListItem>
                  <asp:ListItem value="IN">IN</asp:ListItem>
                  <asp:ListItem value="KS">KS</asp:ListItem>
                  <asp:ListItem value="KY">KY</asp:ListItem>
                  <asp:ListItem value="LA">LA</asp:ListItem>
                  <asp:ListItem value="MA">MA</asp:ListItem>
                  <asp:ListItem value="MD">MD</asp:ListItem>
                  <asp:ListItem value="ME">ME</asp:ListItem>
                  <asp:ListItem value="MI">MI</asp:ListItem>
                  <asp:ListItem value="MN">MN</asp:ListItem>
                  <asp:ListItem value="MO">MO</asp:ListItem>
                  <asp:ListItem value="MS">MS</asp:ListItem>
                  <asp:ListItem value="MT">MT</asp:ListItem>
                  <asp:ListItem value="NC">NC</asp:ListItem>
                  <asp:ListItem value="ND">ND</asp:ListItem>
                  <asp:ListItem value="NE">NE</asp:ListItem>
                  <asp:ListItem value="NH">NH</asp:ListItem>
                  <asp:ListItem value="NJ">NJ</asp:ListItem>
                  <asp:ListItem value="NM">NM</asp:ListItem>
                  <asp:ListItem value="NV">NV</asp:ListItem>
                  <asp:ListItem value="NY">NY</asp:ListItem>
                  <asp:ListItem value="OH">OH</asp:ListItem>
                  <asp:ListItem value="OK">OK</asp:ListItem>
                  <asp:ListItem value="OR">OR</asp:ListItem>
                  <asp:ListItem value="PA">PA</asp:ListItem>
                  <asp:ListItem value="RI">RI</asp:ListItem>
                  <asp:ListItem value="SC">SC</asp:ListItem>
                  <asp:ListItem value="SD">SD</asp:ListItem>
                  <asp:ListItem value="TN">TN</asp:ListItem>
                  <asp:ListItem value="TX">TX</asp:ListItem>
                  <asp:ListItem value="UT">UT</asp:ListItem>
                  <asp:ListItem value="VA">VA</asp:ListItem>
                  <asp:ListItem value="VT">VT</asp:ListItem>
                  <asp:ListItem value="WA">WA</asp:ListItem>
                  <asp:ListItem value="WI">WI</asp:ListItem>
                  <asp:ListItem value="WV">WV</asp:ListItem>
                  <asp:ListItem Value="WY">WY</asp:ListItem>
            </asp:DropDownList>
<br /> <br />
Zip:
            <asp:TextBox ID="tbZip" runat="server" CssClass="textbox" MaxLength="10" Width="104px"></asp:TextBox>
<br /> <br />
 Password:
            <asp:TextBox ID="tbPassword" runat="server" CssClass="textbox" MaxLength="20" Width="104px"></asp:TextBox>
<br /> <br />
Status:
          <asp:DropDownList ID="ddlStatus" runat="server">
             <asp:ListItem Text="Active" Value="Y" />
               <asp:ListItem Text="Inactive" Value="N" />
               </asp:DropDownList>

    </td>
     
    <tr>
        <td colspan="2" style="text-align:center;">
            <asp:MultiView ID="mvButton" runat="server">
                <asp:View ID="viewGrid" runat="server"> 
                     
                    
                    <asp:button id="btnUpdate" text="UPDATE" runat="server" 
                        CssClass="actionbutton" ValidationGroup="vgClientContact" CommandName="Update" OnClick="btnUpdate_Click" > 
                    </asp:button>
                    <asp:button id="btnInsert" text="ADD" runat="server" 
                        CssClass="actionbutton" ValidationGroup="vgClientContact" CommandName="PerformInsert">
                    </asp:button>
                    &nbsp;
                    <asp:Button ID="btnClose" Text="CLOSE" runat="server" 
                        CssClass="actionbutton2" CommandName="Cancel"/>
                </asp:View>

                <asp:View ID="viewPage" runat="server">
                    
                    
                    <asp:Button ID="btnUpdatePage" runat="server" Text="UPDATE" 
                        CssClass="actionbutton" ValidationGroup="vgClientContact" 
                        OnClick="btnUpdatePage_Click"  />
                </asp:View>

            </asp:MultiView>
        </td>
    </tr>
</table>    
</div>

