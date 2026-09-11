<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProgrammingProjectEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_ProgrammingProjectEdit" %>
<%@ Register TagPrefix="ddl" TagName="ProgrammingProjectData" Src="~/UserControl/DropDownList/ProgrammingProject.ascx" %>

<telerik:RadCodeBlock ID="RadCodeBlock" runat="server">
    <script language="javascript">
        function textboxMultilineMaxNumber(txt, maxLen) {
            try {
                if (txt.value.length > (maxLen - 1)) return false;
            } catch (e) {
            }
        }
    </script>
</telerik:RadCodeBlock>


<asp:ValidationSummary ID="vsProgrammingProject" runat="server" CssClass="ValidationSummary" ValidationGroup="vgProgrammingProject" />

<table border="0" cellpadding="0" cellspacing="0" style="width:1000px;"> 
     <tr>
        <td style="width:50%;">
            <table border="0" class="gridEditForm" width="100%" style="width:500px;"> 
                <tr>
                    <th style="width:180px;" class="Required">Client:</th>
                    <td style="width:320px;"><uc:ClientComboBox ID="ucClientComboBox" runat="server" IsRequired="true" Width="300" ValidationGroup="vgProgrammingProject" /></td>
                </tr>
            </table>
        </td>
        <td style="width:50%">&nbsp;</td>
    </tr>
    <tr>
        <td style="width:50%; vertical-align:top;">
            <table border="0" class="gridEditForm" width="100%" style="width:500px; margin:10px 0 0 0;"> 
                <tr>
                    <th colspan="2" style="text-align:center;" class="Required"><div style="width:468px; background-color:#d9d9d9; display:inline-block;"><em>PRODUCTION</em></div></th>
                </tr>
                <tr>
                    <th style="width:180px;" class="Required">FQDN:</th>
                    <td style="width:320px;">
                        <asp:TextBox ID="tbProductionFqdn" runat="server" CssClass="textbox" MaxLength="128" Width="295"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvProductionFqdn" runat="server" ControlToValidate="tbProductionFqdn" ErrorMessage="Production FQDN is required" ValidationGroup="vgProgrammingProject">
                            <span class="error">*</span>
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th style="width:180px;">Alternate FQDN:</th>
                    <td style="width:320px;">
                        <asp:TextBox ID="tbProductionAlternateFqdn" runat="server" CssClass="textbox" Width="295" TextMode="MultiLine" Rows="4" onkeypress="return textboxMultilineMaxNumber(this,1000);"/>
                    </td>
                </tr>
                <tr>
                    <th style="width:180px;" class="Required">Web Server Name:</th>
                    <td style="width:320px;">
                        <asp:TextBox ID="tbProductionServerName" runat="server" CssClass="textbox" MaxLength="128" Width="295"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvProductionServerName" runat="server" ControlToValidate="tbProductionServerName" ErrorMessage="Production Web Server Name is required" ValidationGroup="vgProgrammingProject">
                            <span class="error">*</span>
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th style="width:180px;">DB Server Name:</th>
                    <td style="width:320px;">
                        <asp:TextBox ID="tbProductionDbServerName" runat="server" CssClass="textbox" MaxLength="128" Width="295"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th style="width:180px;" class="Required">Server Location:</th>
                    <td style="width:320px;">
                        <asp:DropDownList ID="ddlProductionServerLocation" runat="server" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlProductionServerLocation_SelectedIndexChanged">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="BBB" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Non BBB" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvProductionServerLocation" runat="server" ControlToValidate="ddlProductionServerLocation" ErrorMessage="Production Server Location is required" ValidationGroup="vgProgrammingProject">
                            <span class="error">*</span>
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <asp:PlaceHolder ID="phProductionServerLocationData" runat="server" Visible="false">
                    <tr>
                        <th style="width:180px;" class="Required">Location (Non BBB):</th>
                        <td style="width:320px;">
                            <asp:TextBox ID="tbProductionServerNonBbbLocation" runat="server" CssClass="textbox" Width="295" MaxLength="128" />
                            <asp:RequiredFieldValidator ID="rfvProductionServerNonBbbLocation" runat="server" ControlToValidate="tbProductionServerNonBbbLocation" ErrorMessage="Production Server (Non BBB) Location is required" ValidationGroup="vgProgrammingProject">
                                <span class="error">*</span>
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th style="width:180px;">Description (Non BBB):</th>
                        <td style="width:320px;">
                            <asp:TextBox ID="tbProductionServerNonBbbDescription" runat="server" CssClass="textbox" Width="295" MaxLength="2000" />
                        </td>
                    </tr>
                    <tr>
                        <th style="width:180px;">VPN Required (Non BBB):</th>
                        <td style="width:320px;">
                            <asp:CheckBox ID="chkProductionServerVpnRequired" runat="server" AutoPostBack="true" OnCheckedChanged="chkProductionServerVpnRequired_CheckedChanged" />
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phProductionSeverVpnSoftware" runat="server" Visible="false">
                        <tr>
                            <th style="width:180px;" class="Required">VPN Software (Non BBB):</th>
                            <td style="width:320px;">
                                <asp:TextBox ID="tbProductionSeverVpnSoftware" runat="server" CssClass="textbox" Width="295" MaxLength="2000" />
                                <asp:RequiredFieldValidator ID="rfvProductionSeverVpnSoftware" runat="server" ControlToValidate="tbProductionSeverVpnSoftware" ErrorMessage="Production Server (Non BBB) VPN Software is required" ValidationGroup="vgProgrammingProject">
                                    <span class="error">*</span>
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <th style="width:180px;">Creds - Auth Info (Non BBB):</th>
                        <td style="width:320px;">
                            <asp:CheckBox ID="chkProductionServerCredsInAuthInfo" runat="server" />
                        </td>
                    </tr>
                </asp:PlaceHolder>
            </table>
        </td>
        <td style="width:50%; vertical-align:top;">
            <table border="0" class="gridEditForm" width="100%" style="width:500px; margin: 10px 0 0 0;"> 
                <tr>
                    <th colspan="2" style="text-align:center;" class="Required"><div style="width:468px; background-color:#d9d9d9; display:inline-block;"><em>STAGING</em></div></th>
                </tr>
                <tr>
                    <th style="width:180px;" class="Required">FQDN:</th>
                    <td style="width:320px;">
                        <asp:TextBox ID="tbStagingFqdn" runat="server" CssClass="textbox" MaxLength="128" Width="295"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvStagingFqdn" runat="server" ControlToValidate="tbStagingFqdn" ErrorMessage="Staging FQDN is required" ValidationGroup="vgProgrammingProject">
                            <span class="error">*</span>
                        </asp:RequiredFieldValidator>
                        </td>
                </tr>
                <tr>
                    <th style="width:180px;">Alternate FQDN:</th>
                    <td style="width:320px;">
                        <asp:TextBox ID="tbStagingAlternateFqdn" runat="server" CssClass="textbox" Width="295" TextMode="MultiLine" Rows="4" onkeypress="return textboxMultilineMaxNumber(this,1000);"/>
                    </td>
                </tr>
                <tr>
                    <th style="width:180px;" class="Required">Web Server Name:</th>
                    <td style="width:320px;">
                        <asp:TextBox ID="tbStagingServerName" runat="server" CssClass="textbox" MaxLength="128" Width="295"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvStagingServerName" runat="server" ControlToValidate="tbStagingServerName" ErrorMessage="Staging Web Server Name is required" ValidationGroup="vgProgrammingProject">
                            <span class="error">*</span>
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th style="width:180px;">DB Server Name:</th>
                    <td style="width:320px;">
                        <asp:TextBox ID="tbStagingDbServerName" runat="server" CssClass="textbox" MaxLength="128" Width="295"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th style="width:180px;" class="Required">Server Location:</th>
                    <td style="width:320px;">
                        <asp:DropDownList ID="ddlStagingServerLocation" runat="server" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlStagingServerLocation_SelectedIndexChanged">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="BBB" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Non BBB" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvStagingServerLocation" runat="server" ControlToValidate="ddlStagingServerLocation" ErrorMessage="Staging Server Location is required" ValidationGroup="vgProgrammingProject">
                            <span class="error">*</span>
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <asp:PlaceHolder ID="phStagingServerLocationData" runat="server" Visible="false">
                    <tr>
                        <th style="width:180px;" class="Required">Location (Non BBB):</th>
                        <td style="width:320px;">
                            <asp:TextBox ID="tbStagingServerNonBbbLocation" runat="server" CssClass="textbox" Width="295" MaxLength="128" />
                            <asp:RequiredFieldValidator ID="rfvStagingServerNonBbbLocation" runat="server" ControlToValidate="tbStagingServerNonBbbLocation" ErrorMessage="Staging Server (Non BBB) Location is required" ValidationGroup="vgProgrammingProject">
                                <span class="error">*</span>
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th style="width:180px;">Description (Non BBB):</th>
                        <td style="width:320px;">
                            <asp:TextBox ID="tbStagingServerNonBbbDescription" runat="server" CssClass="textbox" Width="295" MaxLength="2000" />
                        </td>
                    </tr>
                    <tr>
                        <th style="width:180px;">VPN Required (Non BBB):</th>
                        <td style="width:320px;">
                            <asp:CheckBox ID="chkStagingServerVpnRequired" runat="server" AutoPostBack="true" OnCheckedChanged="chkStagingServerVpnRequired_CheckedChanged" />
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phStagingSeverVpnSoftware" runat="server" Visible="false">
                        <tr>
                            <th style="width:180px;" class="Required">VPN Software (Non BBB):</th>
                            <td style="width:320px;">
                                <asp:TextBox ID="tbStagingSeverVpnSoftware" runat="server" CssClass="textbox" Width="295" MaxLength="2000" />
                                <asp:RequiredFieldValidator ID="rfvStagingSeverVpnSoftware" runat="server" ControlToValidate="tbStagingSeverVpnSoftware" ErrorMessage="Staging Server (Non BBB) VPN Software is required" ValidationGroup="vgProgrammingProject">
                                    <span class="error">*</span>
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <th style="width:180px;">Creds - Auth Info (Non BBB):</th>
                        <td style="width:320px;">
                            <asp:CheckBox ID="chkStagingServerCredsInAuthInfo" runat="server" />
                        </td>
                    </tr>
                </asp:PlaceHolder>
            </table>
        </td>
    </tr>
    <tr><td colspan="2">&nbsp;</td></tr>
    <tr>
        <td style="width:50%; vertical-align:top;">
            <table border="0" class="gridEditForm" width="100%" style="width:500px; margin:10px 0 0 0;"> 
                <tr>
                    <th colspan="2" style="text-align:center;" class="Required"><div style="width:468px; background-color:#d9d9d9; display:inline-block;"><em>SERVER PLATFORM</em></div></th>
                </tr>
                <tr>
                    <th style="width:180px;" class="Required">Operarting System:</th>
                    <td style="width:320px;">
                        <ddl:ProgrammingProjectData ID="ddlProgrammingProjectDataOs" runat="server" TypeOfData="ServerOs" IsRequired="true" DefaultText="" DefaultValue="" RequiredErrorMessage="Operating System is required" ValidationGroup="vgProgrammingProject" Width="300"/>
                    </td>
                </tr>
                <tr>
                    <th style="width:180px;" class="Required">Web Platform:</th>
                    <td style="width:320px;">
                        <ddl:ProgrammingProjectData ID="ddlProgrammingProjectDataWebPlatform" runat="server" TypeOfData="ServerWebPlatform" IsRequired="true" DefaultText="" DefaultValue="" RequiredErrorMessage="Web Platform is required" ValidationGroup="vgProgrammingProject" Width="300"/>
                    </td>
                </tr>
                <tr>
                    <th style="width:180px;">Database:</th>
                    <td style="width:320px;">
                        <ddl:ProgrammingProjectData ID="ddlProgrammingProjectDataServerDatabase" runat="server" TypeOfData="ServerDatabase" IsRequired="false" DefaultText="" DefaultValue="" ValidationGroup="vgProgrammingProject" Width="300"/>
                    </td>
                </tr>
                <tr>
                    <th style="width:180px;">Creds - Auth Info:</th>
                    <td style="width:320px;">
                        <asp:CheckBox ID="chkServerDbCredsAuthInfo" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
        <td style="width:50%; vertical-align:top;">
            <table border="0" class="gridEditForm" width="100%" style="width:500px; margin:10px 0 0 0;"> 
                <tr>
                    <th colspan="2" style="text-align:center;" class="Required"><div style="width:468px; background-color:#d9d9d9; display:inline-block;"><em>DEVELOPMENT</em></div></th>
                </tr>
                <tr>
                    <th style="width:180px;" class="Required">Language:</th>
                    <td style="width:320px;">
                        <ddl:ProgrammingProjectData ID="ddlProgrammingProjectDataDevelopmentLanguage" runat="server" TypeOfData="DevelopmentLanguage" IsRequired="true" DefaultText="" DefaultValue="" RequiredErrorMessage="Langauge is required" ValidationGroup="vgProgrammingProject" Width="300"/>
                    </td>
                </tr>
                <tr>
                    <th style="width:180px;" class="Required">Environment:</th>
                    <td style="width:320px;">
                        <ddl:ProgrammingProjectData ID="ddlProgrammingProjectDataDevelopmentPlatform" runat="server" TypeOfData="DevelopmentPlatform" IsRequired="true" DefaultText="" DefaultValue="" RequiredErrorMessage="Environment is required" ValidationGroup="vgProgrammingProject" Width="300"/>
                    </td>
                </tr>
                <tr>
                    <th style="width:180px;" class="Required">TFS Project Name:</th>
                    <td style="width:320px;">
                        <asp:TextBox ID="tbCvsName" runat="server" CssClass="textbox" MaxLength="128" Width="295"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfCvsName" runat="server" ControlToValidate="tbCvsName" ErrorMessage="CVS Project Name is required" ValidationGroup="vgProgrammingProject">
                            <span class="error">*</span>
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th style="width:180px;">LLBLGen:</th>
                    <td style="width:320px;">
                        <asp:CheckBox ID="chkLlblgen" runat="server" AutoPostBack="true" OnCheckedChanged="chkLlblgen_CheckedChanged"/>
                    </td>
                </tr>
                <asp:PlaceHolder ID="phLlblgen" runat="server" Visible="false">
                     <tr>
                    <th style="width:180px;" class="Required">LLBLGen Version:</th>
                    <td style="width:320px;">
                        <ddl:ProgrammingProjectData ID="ddlProgrammingProjectDataDevelopmentLlblGenVersion" runat="server" TypeOfData="DevelopmentLlblGenVersion" IsRequired="true" DefaultText="" DefaultValue="" RequiredErrorMessage="LLBLGen Version is required" ValidationGroup="vgProgrammingProject" Width="300"/>
                    </td>
                </tr>
                </asp:PlaceHolder>
            </table>
        </td>
    </tr>
    <tr><td colspan="2">&nbsp;</td></tr>
    <tr>
        <td style="width:100%; vertical-align:top;" colspan="2">
            <table border="0" class="gridEditForm" width="100%" style="width:1000px; margin:10px 0 0 0;"> 
                <tr>
                    <th colspan="2" style="text-align:center;" class="Required"><div style="width:966px; background-color:#d9d9d9; display:inline-block;"><em>DOCUMENTATION</em></div></th>
                </tr>
                <tr>
                    <th style="width:174px;">Requirements in Sharepoint:</th>
                    <td style="width:826px;">
                        <asp:CheckBox ID="chkRequirementsSharepoint" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th style="width:174px;">3rd Party Library:</th>
                    <td style="width:826px;">
                        <uc:Editor ID="ucEditorThirdParty" runat="server" Width="800" Height="260" />
                    </td>
                </tr>
                <tr>
                    <th style="width:174px;">General Comments:</th>
                    <td style="width:826px;">
                        <uc:Editor ID="ucEditorGeneral" runat="server" Width="800" Height="260" />
                    </td>
                </tr>
                <tr>
                    <th style="width:174px;">Technical Information:</th>
                    <td style="width:826px;">
                        <uc:Editor ID="ucEditorTechnical" runat="server" Width="800" Height="260" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>