using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Grid_EditForm_ClientServerEdit : System.Web.UI.UserControl
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();
    }

    #region private methods

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        bool _displayChosenScript = true;
        string _cssClass = "form-control select-chosen";
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            _cssClass = "form-control";
            _displayChosenScript = false;
        }

        ddlClient.CssClass = _cssClass;
        ddlClient.DisplayChosenScript = _displayChosenScript;
        ddlBackupType.CssClass = _cssClass;
        ddlBackupType.DisplayChosenScript = _displayChosenScript;
        ddlOs.CssClass = _cssClass;
        ddlOs.DisplayChosenScript = _displayChosenScript;
        ddlOsVersion.CssClass = _cssClass;
        ddlOsVersion.DisplayChosenScript = _displayChosenScript; 
    }

    /// <summary>
    /// configure for no backup selection
    /// </summary>
    private void ConfigureNoBackup(bool isPageLoad = true)
    {
        phNoBackupYes.Visible = chkNoBackup.Checked;
        if (DesktopShared.Role.Desktop.UserIsIn(DesktopShared.User.UserID, DesktopShared.Role.Desktop.Id.BackupAdministrator))
        {
            mvNoBackupApproved.ActiveViewIndex =1;
            chkNoBackup.Enabled = true;
            txtNoBackupReason.Enabled = true;
        }
        else
        {
            mvNoBackupApproved.ActiveViewIndex = 0;

            if (isPageLoad)
            {
                //read only for check & textbox all users if already checked
                chkNoBackup.Enabled = !chkNoBackup.Checked;
                txtNoBackupReason.Enabled = chkNoBackup.Enabled;

                //read only checkbox if no backup has been denied by admin
                if (litNoBackupApproved.Text == "Denied")
                    chkNoBackup.Enabled = false;
            }
            else
            {
                chkNoBackup.Enabled = true;
                txtNoBackupReason.Enabled = true;
            }

        }
    }

    #endregion

    #region public methods

    /// <summary>
    /// load values
    /// </summary>
    /// <param name="objClientServer"></param>
    public void LoadValues(DesktopShared.EntityClasses.ClientServerEntity objClientServer)
    {
        txtName.Focus();
        btnAdd.Visible = objClientServer == null;
        btnEdit.Visible = !btnAdd.Visible;
        phExistingItem.Visible = !btnAdd.Visible;
        ddlBackupType.TableType = "Client_Server_Backup_Type";
        ddlOs.TableType = "Client_Server_Os";
        ddlOsVersion.TableType = "Client_Server_OsVersion";

        DropDownList _ddlClient = ddlClient.GetDropDownList();
        if (!String.IsNullOrWhiteSpace(_ddlClient.Items[0].Value))
            _ddlClient.Items.Insert(0, new ListItem("", ""));

        if (objClientServer == null)
        {
            ClientServerId = null;
            chkActive.Checked = true;
            litHeader.Text = "Add Client Server";
            ddlBackupType.PopulateDropDownList();
            ddlOs.PopulateDropDownList();
            ddlOsVersion.PopulateDropDownList();
            return;
        }

        
        litHeader.Text = "Edit Client Server";
        ClientServerId = objClientServer.Id;
        txtName.Text = objClientServer.Name.Trim();
        ddlClient.ClientId = objClientServer.ClientId;
        ddlBackupType.SupportingTableId = objClientServer.BackupTypeId;
        ddlOs.SupportingTableId = objClientServer.OsId;
        ddlOsVersion.SupportingTableId = objClientServer.OsVersionId;
        ddlServerWorkstation.SelectedIndex = objClientServer.IsServer ? 0 : 1;
        txtBackupSet.Text = objClientServer.BackupSetServerName.Trim();
        txtDescription.Text = objClientServer.Description.Trim();
        chkActive.Checked = objClientServer.Active;
        chkSingleBackup.Checked = objClientServer.SingleBackup;
        chkBackupServer.Checked = objClientServer.BackupServer;
        chkLocal.Checked= objClientServer.Local;
        chkOffsite.Checked = objClientServer.Offsite;
        chkReplica.Checked = objClientServer.Replica;
        chkImported.Checked = objClientServer.AutomatedImport;
        litCreatedHeader.Text = objClientServer.AutomatedImport ? "Imported Date" : "Created Date";
        litCreated.Text = objClientServer.Created.HasValue ? objClientServer.Created.Value.ToString() : "";

        //no backup
        chkNoBackup.Checked = objClientServer.NoBackup;
        txtNoBackupReason.Text = objClientServer.NoBackupReason.Trim();
        litNoBackupApproved.Text = objClientServer.NoBackupApproved.HasValue ? objClientServer.NoBackupApproved.Value ? "Approved" : "Denied" : "Pending";
        string _approved = objClientServer.NoBackupApproved.HasValue ? objClientServer.NoBackupApproved.Value ? "1" : "0" : "";
        ListItem _li = ddlNoBackupApproved.Items.FindByValue(_approved);
        if (_li != null)
        {
            ddlNoBackupApproved.ClearSelection();
            _li.Selected = true;
        }
        if (_approved == "0") //denied by admin
            chkNoBackup.Enabled = false;

        chkNoEndPoint.Checked = objClientServer.NoEndPoint;
        chkPatch.Checked = objClientServer.NoPatch;
        ConfigureNoBackup();
    }

    /// <summary>
    /// save values
    /// </summary>
    /// <param name="clientDoccumentId"></param>
    public int SaveValues(int? clientDoccumentId)
    {
        DesktopShared.EntityClasses.ClientServerEntity objClientServer = null;
        DateTime _auditDate = DateTime.Now;
        int _auditUserId = DesktopShared.User.UserID;

        if (!clientDoccumentId.HasValue)
        {
            objClientServer = new DesktopShared.EntityClasses.ClientServerEntity();
            objClientServer.AutomatedImport = false;
            objClientServer.Created = _auditDate;
            objClientServer.CreatedByUserId = _auditUserId;
            objClientServer.ClientId = phClient.Visible ? ddlClient.ClientId : ClientId;
        }
        else
        {
            objClientServer = new DesktopShared.EntityClasses.ClientServerEntity(clientDoccumentId.Value);
            if (phClient.Visible)
                objClientServer.ClientId = phClient.Visible ? ddlClient.ClientId : ClientId;
        }

        objClientServer.Name = txtName.Text.Trim();
        objClientServer.BackupTypeId = ddlBackupType.SupportingTableId;
        objClientServer.OsId = ddlOs.SupportingTableId;
        objClientServer.OsVersionId = ddlOsVersion.SupportingTableId;
        objClientServer.IsServer = ddlServerWorkstation.SelectedIndex == 0 ? true : false;
        objClientServer.BackupSetServerName = txtBackupSet.Text.Trim();
        objClientServer.Description = txtDescription.Text.Trim();
        objClientServer.Active = chkActive.Checked;
        objClientServer.SingleBackup = chkSingleBackup.Checked;
        objClientServer.BackupServer = chkBackupServer.Checked;

        //no backup configuration
        bool _createTicket = false;
        if (chkNoBackup.Checked != objClientServer.NoBackup)
        {
            if (!chkNoBackup.Checked)
            {
                objClientServer.NoBackup = false;
                objClientServer.NoBackupReason = null;
                objClientServer.NoBackupApproved = null;
            }
            else
            {
                _createTicket = true;
                objClientServer.NoBackup = true;
                if (txtNoBackupReason.Enabled)
                    objClientServer.NoBackupReason = txtNoBackupReason.Text.Trim();
            }
        }
        else if (mvNoBackupApproved.ActiveViewIndex == 1)
        {
            objClientServer.NoBackupReason = txtNoBackupReason.Text.Trim();
            string _approved = ddlNoBackupApproved.SelectedValue.Trim();
            objClientServer.NoBackupApproved = String.IsNullOrWhiteSpace(_approved) ? (bool?)null : _approved == "1" ? true : false;
            if (_approved == "0")
                objClientServer.NoBackup = false;
            _createTicket = false;
         }
        objClientServer.NoEndPoint = chkNoEndPoint.Checked;
        objClientServer.NoPatch = chkPatch.Checked;

        objClientServer.LastUpdated = _auditDate;
        objClientServer.LastUpdatedByUserId = _auditUserId;
        objClientServer.Save();

        DesktopShared.Client.Server.SyncWithBackupSet(objClientServer);

        #region create ticket

        if (_createTicket)
        {
            System.Text.StringBuilder _sbDescription = new System.Text.StringBuilder();
            _sbDescription.Append(String.Format("Kaseya Server - No Backup Request: {0}", DateTime.Now));
            _sbDescription.Append(String.Format("{0}Server Name: {1}", Environment.NewLine, objClientServer.Name.Trim()));
            _sbDescription.Append(String.Format("{0}Client Name: {1}", Environment.NewLine, objClientServer.Client.Company.Trim()));
            _sbDescription.Append(String.Format("{0}Employee: {1}", Environment.NewLine, DesktopShared.User.UserFullName));
            _sbDescription.Append(String.Format("{0}Reason: {1}", Environment.NewLine, objClientServer.NoBackupReason));
            _sbDescription.Append(String.Format("{0}{0}{1}/Maintenance/ClientServer.aspx?Id={2}", Environment.NewLine, DesktopShared.Email.DesktopFqdn.Trim(), objClientServer.Id));

            string _employeeFirst = "";
            string _employeeLast = "";
            string _employeeEmail = "";
            DesktopShared.EntityClasses.EmployeeEntity objEmployee = DesktopShared.Employee.GetEmployeeEntity(DesktopShared.User.EmployeeID);
            if (objEmployee.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched)
            {
                _employeeFirst = objEmployee.First.Trim();
                _employeeLast = objEmployee.Last.Trim();
                _employeeEmail = objEmployee.Email.Trim();
            }

            string _summary = BitByBit.Configuration.GetConfigString("ClientServerNoBackup_MailSubject", "Kaseya Server - No Backup Request");
            _summary += String.Format(": {0}", objClientServer.Name.Trim());

            //create ticket
            int _ticketId = DesktopShared.Ticket.CreateTicket(
                _summary,//summary
                _sbDescription.ToString(), //description
                ddlClient.ClientId.Value, //client id
                DesktopShared.User.UserID, //user id entered by
                null, //clientContactId 
                BitByBit.Configuration.GetConfigInt("KaseyaServerNoBackupRequest_UserId", 13), //assignedToUserId
                "", //internalNotes 
                null, //ticketHistoryTypeId 
                DesktopShared.User.UserFullName, //addedBy
                null, //tam frequency id
                _employeeFirst, //entered by first
                _employeeLast, //entered by last
                _employeeEmail //entered by email
            ); 
        }

        #endregion

        return objClientServer.Id;
    }

    #endregion

    #region protected events

    /// <summary>
    /// no backup on checked changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkNoBackup_CheckedChanged(object sender, EventArgs e)
    {
        ConfigureNoBackup(false);
        
    }

    #region custom validators

    /// <summary>
    /// name validation
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = DesktopShared.Client.Server.IsUniqueName(txtName.Text.Trim(), ddlClient.ClientId.HasValue ? ddlClient.ClientId.Value : -1, ClientServerId);
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set client document  id
    /// </summary>
    private int? ClientServerId
    {
        get
        {
            object obj = this.ViewState["csid_ed"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["csid_ed"] = value; }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set client id
    /// </summary>
    public int ClientId
    {
        get
        {
            object obj = this.ViewState["cid_ed"];
            return (obj == null) ? -1 : (int)obj;
        }
        set { this.ViewState["cid_ed"] = value; }
    }

    /// <summary>
    /// set display client 
    /// </summary>
    public bool DisplayClient
    {
        set { phClient.Visible = value;  }
    }

    /// <summary>
    /// set is server selection
    /// </summary>
    public bool IsServer
    {
        set { ddlServerWorkstation.SelectedIndex = value ? 0 : 1; }
    }

    #endregion
}
 