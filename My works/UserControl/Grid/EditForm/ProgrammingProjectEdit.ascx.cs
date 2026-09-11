using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Grid_EditForm_ProgrammingProjectEdit : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SetUpForm();
    }

    #region public methods

    /// <summary>
    /// load project values
    /// </summary>
    /// <param name="programmingProjectId"></param>
    public void LoadValues(int programmingProjectId)
    {
        if (programmingProjectId > 0)
        {
            DesktopShared.EntityClasses.ProgrammingProjectEntity objProject = new DesktopShared.EntityClasses.ProgrammingProjectEntity(programmingProjectId);
            LoadValues(objProject);
        }
        else
            LoadValues(null);
    }

    /// <summary>
    /// load project values
    /// </summary>
    /// <param name="objProject"></param>
    public void LoadValues(DesktopShared.EntityClasses.ProgrammingProjectEntity objProject)
    {
        #region existing record

        if (objProject != null)
        {
            SelectedClientId = objProject.ClientId.HasValue ? objProject.ClientId.Value : -1;

            #region production

            ProductionFqdn = objProject.ProductionFqdn.Trim();
            ProductionAlternateFqdn = objProject.ProductionAlternateFqdn.Trim();
            ProductionWebServerName = objProject.ProductionServerName.Trim();
            ProductionDbServerName = objProject.ProductionDbServerName.Trim();
            ProductionServerBbb = objProject.ProductionServerBbb;

            //non bbb server
            ProductionServerNonBbbLocation = objProject.ProductionServerLocation.Trim();
            ProductionServerNonBbbDescription = objProject.ProductionServerDescription.Trim();
            ProductionServerNonBbbVpnRequired = objProject.ProductionServerVpnRequired;
            ProductionServerNonBbbVpnSoftware = objProject.ProductionServerVpnSoftware.Trim();
            ProductionServerNonBbbCredsInAuthInfo = objProject.ProductionServerCredsAuthInfo;

            #endregion

            #region staging

            StagingFqdn = objProject.StagingFqdn.Trim();
            StagingAlternateFqdn = objProject.StagingAlternateFqdn.Trim();
            StagingWebServerName = objProject.StagingServerName.Trim();
            StagingDbServerName = objProject.StagingDbServerName.Trim();
            StagingServerBbb = objProject.StagingServerBbb;

            //non bbb server
            StagingServerNonBbbLocation = objProject.StagingServerLocation.Trim();
            StagingServerNonBbbDescription = objProject.StagingServerDescription.Trim();
            StagingServerNonBbbVpnRequired = objProject.StagingServerVpnRequired;
            StagingServerNonBbbVpnSoftware = objProject.StagingServerVpnSoftware.Trim();
            StagingServerNonBbbCredsInAuthInfo = objProject.StagingServerCredsAuthInfo;

            #endregion

            #region server platform

            ServerOsId = objProject.ProgrammingProjectServerOsId;
            ServerWebPlatformId = objProject.ProgrammingProjectServerWebPlatformId;
            ServerDatabaseId = objProject.ProgrammingProjectServerDatabaseId;
            ServerDbCredsAuthInfo = objProject.ProductionServerDbCredsAuthInfo;
            
            #endregion

            #region development

            DevelopmentLanguageId = objProject.ProgrammingProjectDevelopmentLanguageId;
            DevelopmentPlatformId = objProject.ProgrammingProjectDevelopmentPlatformId;
            CvsProjectName = objProject.CvsProjectName.Trim();
            LlblGen = objProject.LlblGen;
            DevelopmentLlblGenVersionId = objProject.ProgrammingProjectDevelopmentLlblGenVersionId;

            #endregion

            #region documentation

            RequirementsInSharepoint = objProject.RequirementsSharepoint;
            ThirdPartyText = objProject.ThirdPartyLibrary.Trim();
            AdditionalGeneralText = objProject.GeneralComment.Trim();
            AdditionalTechnicalText = objProject.TechincalInformation.Trim();

            #endregion

        }

        #endregion

        #region add new record - set default values

        else
        {

        }

        #endregion

    }

    
    /// <summary>
    /// save project values
    /// </summary>
    /// <param name="programmingProjectId"></param>
    public int SaveValues(int programmingProjectId)
    {
        if (Page.IsValid)
        {
            DesktopShared.EntityClasses.ProgrammingProjectEntity objProject = null;
            DateTime _now = DateTime.Now;
            int _userId = DesktopShared.User.UserID;

            if (programmingProjectId > 0) //existing record
                objProject = new DesktopShared.EntityClasses.ProgrammingProjectEntity(programmingProjectId);
            else //new record
            {
                objProject = new DesktopShared.EntityClasses.ProgrammingProjectEntity();
                objProject.Created = _now;
                objProject.CreatedByUserId = _userId;
            }

            objProject.ClientId = SelectedClientId;

            #region production

            objProject.ProductionFqdn = ProductionFqdn.Trim();
            objProject.ProductionAlternateFqdn = ProductionAlternateFqdn.Trim();
            objProject.ProductionServerName = ProductionWebServerName.Trim();
            objProject.ProductionDbServerName = ProductionDbServerName.Trim();
            objProject.ProductionServerBbb = ProductionServerBbb;

            //non bbb server
            objProject.ProductionServerLocation = ProductionServerBbb ? "" : ProductionServerNonBbbLocation.Trim();
            objProject.ProductionServerDescription = ProductionServerBbb ? "" : ProductionServerNonBbbDescription.Trim();
            objProject.ProductionServerVpnRequired = ProductionServerBbb ? false : ProductionServerNonBbbVpnRequired;
            objProject.ProductionServerVpnSoftware = ProductionServerBbb ? "" : ProductionServerNonBbbVpnSoftware.Trim();
            objProject.ProductionServerCredsAuthInfo = ProductionServerBbb ? false : ProductionServerNonBbbCredsInAuthInfo;

            #endregion

            #region staging

            objProject.StagingFqdn = StagingFqdn.Trim();
            objProject.StagingAlternateFqdn = StagingAlternateFqdn.Trim();
            objProject.StagingServerName = StagingWebServerName.Trim();
            objProject.StagingDbServerName = StagingDbServerName.Trim();
            objProject.StagingServerBbb = StagingServerBbb;

            //non bbb server
            objProject.StagingServerLocation = StagingServerBbb ? "" : StagingServerNonBbbLocation.Trim();
            objProject.StagingServerDescription = StagingServerBbb ? "" : StagingServerNonBbbDescription.Trim();
            objProject.StagingServerVpnRequired = StagingServerBbb ? false : StagingServerNonBbbVpnRequired;
            objProject.StagingServerVpnSoftware = StagingServerBbb ? "" : StagingServerNonBbbVpnSoftware.Trim();
            objProject.StagingServerCredsAuthInfo = StagingServerBbb ? false : StagingServerNonBbbCredsInAuthInfo;

            #endregion

            #region server platform

            objProject.ProgrammingProjectServerOsId = ServerOsId;
            objProject.ProgrammingProjectServerWebPlatformId = ServerWebPlatformId;
            objProject.ProgrammingProjectServerDatabaseId = ServerDatabaseId;
            objProject.ProductionServerDbCredsAuthInfo = ServerDbCredsAuthInfo;

            #endregion

            #region development

            objProject.ProgrammingProjectDevelopmentLanguageId = DevelopmentLanguageId;
            objProject.ProgrammingProjectDevelopmentPlatformId = DevelopmentPlatformId;
            objProject.CvsProjectName = CvsProjectName.Trim();
            objProject.LlblGen = LlblGen;
            objProject.ProgrammingProjectDevelopmentLlblGenVersionId = LlblGen ? DevelopmentLlblGenVersionId : (int?)null;

            #endregion

            #region documentation

            objProject.RequirementsSharepoint = RequirementsInSharepoint;
            objProject.ThirdPartyLibrary = ThirdPartyText.Trim();
            objProject.GeneralComment = AdditionalGeneralText.Trim();
            objProject.TechincalInformation = AdditionalTechnicalText.Trim();

            #endregion

            objProject.LastUpdated = _now;
            objProject.LastUpdatedByUserId = _userId;
            objProject.Save();

            return objProject.Id;

        }

        return -1;
    }


    #endregion

    #region private methods

    /// <summary>
    /// set up form for display
    /// </summary>
    private void SetUpForm()
    {
        //production
        chkProductionServerVpnRequired.InputAttributes.Add("class", "checkbox");
        chkProductionServerCredsInAuthInfo.InputAttributes.Add("class", "checkbox");

        //staging
        chkStagingServerVpnRequired.InputAttributes.Add("class", "checkbox");
        chkStagingServerCredsInAuthInfo.InputAttributes.Add("class", "checkbox");

        //server platform
        chkServerDbCredsAuthInfo.InputAttributes.Add("class", "checkbox");

        //development
        chkLlblgen.InputAttributes.Add("class", "checkbox");

        //documentation
        chkRequirementsSharepoint.InputAttributes.Add("class", "checkbox");
    }

    #region production server

    /// <summary>
    /// set display of fields for production server located at bbb
    /// </summary>
    private void ProductionServerBbbDisplay()
    {
        phProductionServerLocationData.Visible = !ProductionServerBbb;
    }

    /// <summary>
    /// set display of fields for production server vpn required
    /// </summary>
    private void ProductionServerVpnRequiredDisplay()
    {
        phProductionSeverVpnSoftware.Visible = ProductionServerNonBbbVpnRequired;
    }

    #endregion

    #region staging server

    /// <summary>
    /// set display of fields for staging server located at bbb
    /// </summary>
    private void StagingServerBbbDisplay()
    {
        phStagingServerLocationData.Visible = !StagingServerBbb;
    }

    /// <summary>
    /// set display of fields for staging server vpn required
    /// </summary>
    private void StagingServerVpnRequiredDisplay()
    {
        phStagingSeverVpnSoftware.Visible = StagingServerNonBbbVpnRequired;
    }

    #endregion

    #region development

    /// <summary>
    /// set display of fields for llblgen
    /// </summary>
    private void LlblGenDisplay()
    {
        phLlblgen.Visible = LlblGen;
    }

    #endregion

    #endregion

    #region protected events

    #region production server

    /// <summary>
    /// production server location drop down list on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProductionServerLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProductionServerBbbDisplay();
    }

    /// <summary>
    /// production server (non bbb) vpn required on checked changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkProductionServerVpnRequired_CheckedChanged(object sender, EventArgs e)
    {
        ProductionServerVpnRequiredDisplay();
    }

    #endregion

    #region staging server

    /// <summary>
    /// staging server location drop down list on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStagingServerLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        StagingServerBbbDisplay();
    }

    /// <summary>
    /// staging server (non bbb) vpn required on checked changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkStagingServerVpnRequired_CheckedChanged(object sender, EventArgs e)
    {
        StagingServerVpnRequiredDisplay();
    }

    #endregion

    #region development

    /// <summary>
    /// llblgen check box on checked changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkLlblgen_CheckedChanged(object sender, EventArgs e)
    {
        LlblGenDisplay();
    }

    #endregion

    #endregion

    #region public properties

    /// <summary>
    /// get/set selected client id
    /// </summary>
    public int SelectedClientId
    {
        get { return ucClientComboBox.SelectedClientId; }
        set { ucClientComboBox.SelectedClientId = value; }
    }

    #region production

    /// <summary>
    /// get/set production fqdn
    /// </summary>
    public string ProductionFqdn
    {
        get { return tbProductionFqdn.Text; }
        set { tbProductionFqdn.Text= value; }
    }

    /// <summary>
    /// get/set production alternate fqdn
    /// </summary>
    public string ProductionAlternateFqdn
    {
        get { return tbProductionAlternateFqdn.Text; }
        set { tbProductionAlternateFqdn.Text = value; }
    }

    /// <summary>
    /// get/set production web server name
    /// </summary>
    public string ProductionWebServerName
    {
        get { return tbProductionServerName.Text; }
        set { tbProductionServerName.Text = value; }
    }

    /// <summary>
    /// get/set production db server name
    /// </summary>
    public string ProductionDbServerName
    {
        get { return tbProductionDbServerName.Text; }
        set { tbProductionDbServerName.Text = value; }
    }

    /// <summary>
    /// get/set production server is bbb
    /// </summary>
    public bool ProductionServerBbb
    {
        get { return ddlProductionServerLocation.SelectedValue == "1"; }
        set
        {
            string _selectedValue = value ? "1" : "0";
            ListItem li = ddlProductionServerLocation.Items.FindByValue(_selectedValue);
            if (li!=null)
                li.Selected = true;

            ProductionServerBbbDisplay();
        }
    }

    /// <summary>
    /// get/set production server non bbb location 
    /// </summary>
    public string ProductionServerNonBbbLocation
    {
        get { return tbProductionServerNonBbbLocation.Text; }
        set { tbProductionServerNonBbbLocation.Text = value; }
    }

    /// <summary>
    /// get/set production server non bbb description
    /// </summary>
    public string ProductionServerNonBbbDescription
    {
        get { return tbProductionServerNonBbbDescription.Text; }
        set { tbProductionServerNonBbbDescription.Text = value; }
    }

    /// <summary>
    /// get/set production sever non bbb vpn required
    /// </summary>
    public bool ProductionServerNonBbbVpnRequired
    {
        get { return chkProductionServerVpnRequired.Checked; }
        set
        {
            chkProductionServerVpnRequired.Checked = value;
            ProductionServerVpnRequiredDisplay();
        }
    }

    /// <summary>
    /// get/set production server non bbb vpn software
    /// </summary>
    public string ProductionServerNonBbbVpnSoftware
    {
        get { return tbProductionSeverVpnSoftware.Text; }
        set { tbProductionSeverVpnSoftware.Text = value; }
    }

    /// <summary>
    /// get/set production server non bbb credentials stored in authentiation info library
    /// </summary>
    public bool ProductionServerNonBbbCredsInAuthInfo
    {
        get { return chkProductionServerCredsInAuthInfo.Checked; }
        set { chkProductionServerCredsInAuthInfo.Checked = value; }
    }

    #endregion

    #region staging

    /// <summary>
    /// get/set staging fqdn
    /// </summary>
    public string StagingFqdn
    {
        get { return tbStagingFqdn.Text; }
        set { tbStagingFqdn.Text = value; }
    }

    /// <summary>
    /// get/set staging alternate fqdn
    /// </summary>
    public string StagingAlternateFqdn
    {
        get { return tbStagingAlternateFqdn.Text; }
        set { tbStagingAlternateFqdn.Text = value; }
    }

    /// <summary>
    /// get/set staging web server name
    /// </summary>
    public string StagingWebServerName
    {
        get { return tbStagingServerName.Text; }
        set { tbStagingServerName.Text = value; }
    }

    /// <summary>
    /// get/set staging db server name
    /// </summary>
    public string StagingDbServerName
    {
        get { return tbStagingDbServerName.Text; }
        set { tbStagingDbServerName.Text = value; }
    }

    /// <summary>
    /// get/set staging server is bbb
    /// </summary>
    public bool StagingServerBbb
    {
        get { return ddlStagingServerLocation.SelectedValue == "1"; }
        set
        {
            string _selectedValue = value ? "1" : "0";
            ListItem li = ddlStagingServerLocation.Items.FindByValue(_selectedValue);
            if (li != null)
                li.Selected = true;

            StagingServerBbbDisplay();
        }
    }

    /// <summary>
    /// get/set staging server non bbb location 
    /// </summary>
    public string StagingServerNonBbbLocation
    {
        get { return tbStagingServerNonBbbLocation.Text; }
        set { tbStagingServerNonBbbLocation.Text = value; }
    }

    /// <summary>
    /// get/set staging server non bbb description
    /// </summary>
    public string StagingServerNonBbbDescription
    {
        get { return tbStagingServerNonBbbDescription.Text; }
        set { tbStagingServerNonBbbDescription.Text = value; }
    }

    /// <summary>
    /// get/set staging sever non bbb vpn required
    /// </summary>
    public bool StagingServerNonBbbVpnRequired
    {
        get { return chkStagingServerVpnRequired.Checked; }
        set
        {
            chkStagingServerVpnRequired.Checked = value;
            StagingServerVpnRequiredDisplay();
        }
    }

    /// <summary>
    /// get/set staging server non bbb vpn software
    /// </summary>
    public string StagingServerNonBbbVpnSoftware
    {
        get { return tbStagingSeverVpnSoftware.Text; }
        set { tbStagingSeverVpnSoftware.Text = value; }
    }

    /// <summary>
    /// get/set staging server non bbb credentials stored in authentiation info library
    /// </summary>
    public bool StagingServerNonBbbCredsInAuthInfo
    {
        get { return chkStagingServerCredsInAuthInfo.Checked; }
        set { chkStagingServerCredsInAuthInfo.Checked = value; }
    }

    #endregion

    #region server platform

    /// <summary>
    /// get/set server os id
    /// </summary>
    public int? ServerOsId
    {
        get { return ddlProgrammingProjectDataOs.SelectedId; }
        set { ddlProgrammingProjectDataOs.SelectedId = value; }
    }

    /// <summary>
    /// get/set server web platform id
    /// </summary>
    public int? ServerWebPlatformId
    {
        get { return ddlProgrammingProjectDataWebPlatform.SelectedId; }
        set { ddlProgrammingProjectDataWebPlatform.SelectedId = value; }
    }

    /// <summary>
    /// get/set server database id
    /// </summary>
    public int? ServerDatabaseId
    {
        get { return ddlProgrammingProjectDataServerDatabase.SelectedId; }
        set { ddlProgrammingProjectDataServerDatabase.SelectedId = value; }
    }

    /// <summary>
    /// get/set data base creds in auth info
    /// </summary>
    public bool ServerDbCredsAuthInfo
    {
        get { return chkServerDbCredsAuthInfo.Checked; }
        set { chkServerDbCredsAuthInfo.Checked = value; }
    }

    #endregion

    #region development

    /// <summary>
    /// get/set development langauge id
    /// </summary>
    public int? DevelopmentLanguageId
    {
        get { return ddlProgrammingProjectDataDevelopmentLanguage.SelectedId; }
        set { ddlProgrammingProjectDataDevelopmentLanguage.SelectedId = value; }
    }

    /// <summary>
    /// get/set development platform / environment id
    /// </summary>
    public int? DevelopmentPlatformId
    {
        get { return ddlProgrammingProjectDataDevelopmentPlatform.SelectedId; }
        set { ddlProgrammingProjectDataDevelopmentPlatform.SelectedId = value; }
    }

    /// <summary>
    /// get/set cvs project name
    /// </summary>
    public string CvsProjectName
    {
        get { return tbCvsName.Text; }
        set { tbCvsName.Text = value; }
    }

    /// <summary>
    /// get/set llblgen
    /// </summary>
    public bool LlblGen
    {
        get { return chkLlblgen.Checked; }
        set
        { 
            chkLlblgen.Checked = value;
            LlblGenDisplay();
        }
    }

    /// <summary>
    /// get/set development llblgen version id
    /// </summary>
    public int? DevelopmentLlblGenVersionId
    {
        get { return ddlProgrammingProjectDataDevelopmentLlblGenVersion.SelectedId; }
        set { ddlProgrammingProjectDataDevelopmentLlblGenVersion.SelectedId = value; }
    }

    #endregion

    #region documentation

    /// <summary>
    /// get/set requirements in sharepoint
    /// </summary>
    public bool RequirementsInSharepoint
    {
        get { return chkRequirementsSharepoint.Checked; }
        set { chkRequirementsSharepoint.Checked = value; }
    }

    /// <summary>
    /// get/set 3rd pary library text
    /// </summary>
    public string ThirdPartyText
    {
        get { return ucEditorThirdParty.Content; }
        set { ucEditorThirdParty.Content = value; }
    }

    /// <summary>
    /// get/set additional general comments text
    /// </summary>
    public string AdditionalGeneralText
    {
        get { return ucEditorGeneral.Content; }
        set { ucEditorGeneral.Content = value; }
    }

    /// <summary>
    /// get/set additional technical information
    /// </summary>
    public string AdditionalTechnicalText
    {
        get { return ucEditorTechnical.Content; }
        set { ucEditorTechnical.Content = value; }
    }

    #endregion

    #endregion
}