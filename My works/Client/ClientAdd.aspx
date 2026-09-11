<%@ Page Language="C#"   AutoEventWireup="true" MasterPageFile="~/Template/Base.master" CodeFile="ClientAdd.aspx.cs" Inherits="Client_ClientAdd" %>
<%@ Register TagPrefix="uc" TagName="Employee" Src="~/UserControl/ComboBox/Employee.ascx" %>
<%@ Register TagPrefix="uc" TagName="ClientSalesPerson" Src="~/UserControl/DropDownList/ClientSalesPerson.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server" >

    <style type="text/css">
        .auto-style3 {
            width: 124px;
        }
        .red {
            color: red;
        }
        .auto-style4 {
            width: 412px;
        }
        .auto-style6 {
            width: 264px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <telerik:RadNotification ID="rnClientAdd" runat="server" Width="250" Height="100" EnableRoundedCorners="true" VisibleTitlebar="false" Pinned="false" Position="Center" Skin="Black" AnimationDuration="2" Animation="FlyIn"/>

<h2>Client Add <asp:Literal ID="litHeaderText" runat="server" /></h2>

<asp:Panel id="pnlGrid" runat="server" style="background-color:#fafbfc; padding:5px 5px 5px 5px; width:99%;" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
    
<table border="0" class="gridEditForm" width="100%" style="width:100%">
				<!--<tr>
					<th>
						<b>General Information:</b>
					</th>
					<td>
					</td>
				</tr> -->
				<tr>
					<th class="Required" >Client Code:<span class="red">*</span></th>
					<td class="auto-style4">
					<asp:TextBox ID="tbClientCode" runat="server" CssClass="textbox" MaxLength="5" Width="260px" TabIndex="1"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvClientCode" runat="server" ControlToValidate="tbClientCode" 
                ErrorMessage="Client Code is required" ValidationGroup="vgClientContact">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
					</td>
                        <th class="auto-style6">Premium Support:</th>
					<td>
                        <asp:CheckBox ID="chkPremiumSupport" runat="server" TabIndex="20" />
					</td>
                    
				</tr>
				<tr>
					<th class="auto-style3">Type:</th>
					<td class="auto-style4">
                       <asp:DropDownList ID="ddlType" runat="server" TabIndex="2">
             <asp:ListItem Text="Client" Value="Y" />
               <asp:ListItem Text="Vendor" Value="N" />
               </asp:DropDownList>
					</td>
                    <th class="auto-style6">Account Status:</th>
					<td>						<asp:DropDownList ID="ddlStatus" runat="server" MaxLength="20" Width="175px" TabIndex="21">

                <asp:ListItem value=""></asp:ListItem>
                  <asp:ListItem value="Approved">Approved</asp:ListItem>
                            <asp:ListItem value="ON" >On Notice</asp:ListItem>
                            <asp:ListItem value="OH">On Hold</asp:ListItem>
                            <asp:ListItem value="RFRP">RightFax Renewal Pending</asp:ListItem>
                            </asp:DropDownList>
					</td>
				</tr>
				<tr>
					<th class="Required" style="width: 124px">Company:<span class="red">*</span></th>
					<td class="auto-style4">
					<asp:TextBox ID="tbCompany" runat="server" CssClass="textbox" MaxLength="20" Width="260px" TabIndex="3"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="tbCompany" 
                ErrorMessage="Company is required" ValidationGroup="vgCompany">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
                        </td>
                    <th class="auto-style6">Account Comments:</th>
					<td>
                        <asp:TextBox ID="tbAcctComments" runat="server" CssClass="textbox" MaxLength="50" Width="260px" TabIndex="22"></asp:TextBox>
					</td>
				</tr>
    <tr>
                    <th class="auto-style3">Address1:</th>
					<td class="auto-style4">
					<asp:TextBox ID="tbAddress1" runat="server" CssClass="textbox" MaxLength="50" Width="260px" TabIndex="4"></asp:TextBox>
					</td>
                    <th class="auto-style6">Password Code:</th>
					<td>
						<asp:TextBox ID="tbPasswordCode" runat="server" CssClass="textbox" MaxLength="50" Width="260px" TabIndex="23"></asp:TextBox>
					</td>
                        </tr>
				<tr>
					<th class="auto-style3">Address2:</th>
					<td class="auto-style4">
					<asp:TextBox ID="tbAddress2" runat="server" CssClass="textbox" MaxLength="50" Width="260px" TabIndex="5"></asp:TextBox>
					</td>
                    <th class="auto-style6">Backup Enabled:</th>
					<td>
                        <asp:CheckBox ID="cbBackupEnabled" runat="server" TabIndex="24"></asp:CheckBox>
					</td>
				</tr>
				<tr>
					<th class="auto-style3">City:</th>
					<td class="auto-style4">
					<asp:TextBox ID="tbCity" runat="server" CssClass="textbox" MaxLength="20" Width="260px" TabIndex="6"></asp:TextBox>
					</td>
                    <th class="auto-style6">Priority Client:</th>
					<td>
                       <asp:CheckBox ID="cbPriorityClient" runat="server" TabIndex="25"></asp:CheckBox>

					</td>
				</tr>
				<tr>
					<th class="auto-style3">State:</th>
					<td class="auto-style4">
						<asp:DropDownList ID="ddlState" runat="server" MaxLength="20" Width="45px" TabIndex="7">
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
					</td>
                     <th class="auto-style6">Auto CC on Ticket Update: </th>
                    <td>
                        <asp:TextBox ID="tbTicketUpdate" runat="server" CssClass="textbox" MaxLength="20" Width="260px" TabIndex="26"></asp:TextBox>
                    </td>
					</tr>
 
				<tr>
					<th class="auto-style3">Zip:</th>
					<td class="auto-style4">
						<asp:TextBox ID="tbZip" runat="server" CssClass="textbox" MaxLength="10" Width="104px" TabIndex="8"></asp:TextBox>
					</td>
                   
                    <th class="Required" style="width: 264px">Vendor Classification:</th>
                    <td></td>
				</tr>
				<tr>
					<th class="auto-style3">Phone:</th>
					<td class="auto-style4">
						<asp:TextBox ID="tbPhone" runat="server" CssClass="textbox" MaxLength="12" Width="260px" TabIndex="9"></asp:TextBox>
					</td>
                    <th class="auto-style6">Lead System Vendor:</th>
                    <td>
                        <asp:CheckBox ID="chLSV" runat="server" TabIndex="27"></asp:CheckBox>
                    </td>
                    
				</tr>
				<tr>
					<th class="auto-style3">Fax:</th>
					<td class="auto-style4">
						<asp:TextBox ID="tbFax" runat="server" CssClass="fax" MaxLength="14" Width="260px" AutoCompleteType="HomeFax" TabIndex="10" ></asp:TextBox>
					</td>
                    
                    <th class="auto-style6">Vendor For Delivery:</th>
                    <td>
                        <asp:CheckBox ID="chVFD" runat="server" TabIndex="28"></asp:CheckBox>
                    </td>
                    
				</tr>
       <tr>
						<th class="auto-style3">Ranking:</th>
                    <td class="auto-style4">
                        <asp:DropDownList Id="ddlRanking" runat="server" TabIndex="18">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                        </asp:DropDownList>
                    </td>


                      <th class="Required" style="width: 264px">CSC Support Options:</th>
                    <td> </td>
				</tr>
                <tr>
                    <th class="auto-style3">Project Manager:</th>
                    <td class="auto-style4">
                        <!--<asp:DropDownList ID="ddlProjectManager" runat="server" Width="150px" TabIndex="12">
                        <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>-->
                         <uc:EmployeeComboBox ID="ucEmployeeComboBoxProjectManager" runat="server" />
                    </td>
                    <th class="auto-style6">BBB CSC:</th>
                    <td><asp:CheckBox ID="chBBBCSC" runat="server" TabIndex="29"></asp:CheckBox></td>
                  </tr>
                    <tr>
                    <th class="auto-style3">Technical Lead:</th>
                    <td class="auto-style4">
                        <!--<asp:DropDownList ID="ddlTechnicalLead" runat="server" Width="150px" TabIndex="13">
                        <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>-->
                        <uc:EmployeeComboBox ID="ucEmployeeComboBoxTechnicalLead" runat="server" />
                    </td>
                        <th class="auto-style6">3B Digital CSC:</th>
                    <td><asp:CheckBox ID="ch3B" runat="server" TabIndex="30"></asp:CheckBox></td>
                  </tr>
				<tr>
					<th class="auto-style3">URL:</th>
					<td class="auto-style4">
						<asp:TextBox ID="tbUrl" runat="server" CssClass="textbox" MaxLength="20" Width="260px" TabIndex="14"></asp:TextBox>
					</td>
                    <th class="auto-style6">&nbsp;</th>
                    <td>&nbsp;</td>
				</tr>
				<tr>
					<th class="auto-style3">RecId</th>
					<td class="auto-style4">
					<asp:TextBox ID="tbRecId" runat="server" CssClass="textbox" MaxLength="20" Width="260px" TabIndex="15"></asp:TextBox>
					</td>
                    <!--<th class="Required" style="width: 264px">Site Housed On Server:</th><td></td>-->
				</tr>
				<tr>
					<th class="auto-style3">Hourly Rate:</th>
					<td class="auto-style4">
						<asp:TextBox ID="tbHourlyRate" runat="server" CssClass="textbox" MaxLength="5" Width="59px" TabIndex="16"></asp:TextBox>
                     
                    </td>
				</tr>
				<tr>
					<th class="auto-style3">Active:</th>
					<td class="auto-style4">
                        <asp:DropDownList ID="ddlActive" runat="server">
                            <asp:ListItem Text=""  />
             <asp:ListItem Text="Active" Value="Y" />
               <asp:ListItem Text="Inactive" Value="N" />
               </asp:DropDownList>
					</td>
                    <!--<th class="auto-style6">NT2WEB1:</th>
                    <td><asp:CheckBox ID="chNT2WEB1" runat="server" TabIndex="33"></asp:CheckBox></td>-->
				</tr>
                <tr>
                    <th class="auto-style3">Saleperson(s):</th>
                    <td class="auto-style4">
				
                       <uc:ClientSalesPerson ID="ucClientSalesPerson" runat="server" Width="175" />
					   
					 </td>
                    
                    <!--<th class="auto-style6">RAQ2WEB1:</th>
                    <td><asp:CheckBox ID="chRAQ2WEB1" runat="server" TabIndex="34"></asp:CheckBox></td>-->
                </tr>
    <tr>
        <th class="auto-style3">Notes:</th>
        <td class="auto-style4">
         <asp:TextBox ID="tbNotes" runat="server" TextMode="MultiLine" 
                Rows="5" CssClass="textbox" Width="400" TabIndex="35"></asp:TextBox>
            </td>
        <th class="auto-style6">Directions:</th>
        <td>
         <asp:TextBox ID="tbDirections" runat="server" TextMode="MultiLine" 
                Rows="5" CssClass="textbox" Width="400" TabIndex="36"></asp:TextBox>
            </td>
    </tr>
		<tr>
        <th class="auto-style3">What We Can Do For Them:</th>
        <td class="auto-style4">
         <asp:TextBox ID="tbWhatWeCanDoForThem" runat="server" TextMode="MultiLine" 
                Rows="5" CssClass="textbox" Width="400" TabIndex="37"></asp:TextBox>
            </td>
        <th class="auto-style6">What We Do For Them:</th>
        <td>
         <asp:TextBox ID="tbWhatWeDoForThem" runat="server" TextMode="MultiLine" 
                Rows="5" CssClass="textbox" Width="400" TabIndex="38"></asp:TextBox>
            </td>
    </tr>		
    <tr>
        <td colspan="4" style="text-align:center;">
                    <asp:button id="btnInsert" text="ADD" runat="server" 
                        CssClass="actionbutton" ValidationGroup="vgClientContact" CommandName="PerformInsert" TabIndex="39" OnClick="btnInsert_Click">
                    </asp:button>

        </td>
    </tr>
        </table>
    </asp:Panel>

   </asp:Content>