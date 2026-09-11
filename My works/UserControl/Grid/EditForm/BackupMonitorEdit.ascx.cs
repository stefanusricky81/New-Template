using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControl_Grid_EditForm_BackupMonitorEdit : AbstractBackupMonitorEdit
{
    private EditMode _editMode = EditMode.Grid;

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        SetUpControl();
    }

    #region private methods

    /// <summary>
    /// set up control for display
    /// </summary>
    private void SetUpControl()
    {
        // when status is TBD, the comment field becomes required.
        RadioButtonList rblBackupMonitorStatus = ucBackupMonitorStatusRBL.GetRadioButtonList();
        rblBackupMonitorStatus.SelectedIndexChanged += new EventHandler(rblBackupMonitorStatus_SelectedChange);

        if (_editMode == EditMode.Grid)
        {
            mvButton.SetActiveView(viewGrid);
            //mvButtonTop.SetActiveView(viewGridTop);
        }
        else
        {
            mvButton.SetActiveView(viewPage);
            //mvButtonTop.SetActiveView(viewPageTop);

            divMain.Style.Add("border", "1px solid #3b5a82");
        }
    }

    #endregion

    #region protected methods / events

    /// <summary>
    /// update page button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdatePage_Click(object sender, EventArgs e)
    {
        evButtonClick(this, e);
    }

    /// <summary>
    /// backup monitor status drop list on selected index changed -> Unsuccessful - comment field is required
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rblBackupMonitorStatus_SelectedChange(object sender, EventArgs e)
    {
        rfvComment.Enabled = DesktopShared.Backup.Monitor.Status.IsUnsuccessful(BackupMonitorStatusId);
    }

    #endregion

    #region public override methods

    /// <summary>
    /// load backup monitor values
    /// </summary>
    /// <param name="backupMonitorID"></param>
    public override void LoadValues(int backupMonitorID)
    {
        if (backupMonitorID > 0)
        {
            DesktopShared.TypedListClasses.BackupMonitorRow backupMonitorRow =
                DesktopShared.Backup.Monitor.GetTypedListRow(backupMonitorID);

            LoadValues(backupMonitorRow);
        }
        else
            LoadValues(null);
    }

    /// <summary>
    /// load backup monitor values
    /// </summary>
    /// <param name="backupSetTypedListRow"></param>
    public override void LoadValues(DesktopShared.TypedListClasses.BackupMonitorRow backupMonitorRow)
    {
        btnUpdate.Visible = (backupMonitorRow != null);

        #region existing record

        if (backupMonitorRow != null)
        {
            BackupMonitorStatusId = backupMonitorRow.BackupMonitorStatusId;
            Comment = backupMonitorRow.Comment.Trim();
            ClientName = backupMonitorRow.ClientCompany.Trim();
            ServerName = backupMonitorRow.Server.Trim();
            BackupSetName = backupMonitorRow.BackupSetName.Trim();
            BackupSetBackupServer = backupMonitorRow.BackupSetBackupServer.Trim();
            BackupSetDescription = backupMonitorRow.BackupSetDescription.Trim();

            //rfvComment.Enabled = DesktopShared.Backup.Monitor.Status.IsUnsuccessful(BackupMonitorStatusId);

            Created = backupMonitorRow.Created;
            LastUpdated = backupMonitorRow.LastUpdated;

            if (_editMode == EditMode.Page)
                btnUpdatePage.Text = "UPDATE";
        }

        #endregion

        #region new record

        else
        {
            ucBackupMonitorStatusRBL.Populate();

            if (_editMode == EditMode.Page)
                btnUpdatePage.Text = "ADD";
        }

        #endregion

    }

    /// <summary>
    /// save backup monitor values
    /// </summary>
    /// <param name="authenticationInfoId"></param>
    /// <returns></returns>
    public override int SaveValues(int backupMonitorID)
    {
        DesktopShared.EntityClasses.BackupMonitorEntity backupMonitor = null;
        DateTime _now = DateTime.Now;
        int _employeeId = DesktopShared.User.EmployeeID;

        if (backupMonitorID > 0) //existing record
        {
            backupMonitor = new DesktopShared.EntityClasses.BackupMonitorEntity(backupMonitorID);
        }
        else //new record
        {
            backupMonitor = new DesktopShared.EntityClasses.BackupMonitorEntity();
            backupMonitor.Created = _now;
            backupMonitor.CreatedBy = _employeeId;
        }

        backupMonitor.BackupMonitorStatusId = BackupMonitorStatusId;
        backupMonitor.Comment = Comment.Trim();  
        backupMonitor.LastUpdated = _now;
        backupMonitor.LastUpdatedBy = _employeeId;

        backupMonitor.Save();

        return backupMonitor.Id;
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set comment
    /// </summary>
    public string Comment
    {
        get { return tbComment.Text; }
        set { tbComment.Text = value; }
    }

    /// <summary>
    /// set client
    /// </summary>
    public string ClientName
    {
        set { lblClient.Text = value; }
    }

    /// <summary>
    /// set server
    /// </summary>
    public string ServerName
    {
        set { lblServer.Text = value; }
    }

    /// <summary>
    /// set Backup Set Name
    /// </summary>
    public string BackupSetName
    {
        set { lblBackupSet.Text = value; }
    }

    /// <summary>
    /// set Backup Set Description
    /// </summary>
    public string BackupSetDescription
    {
        set { lblBackupDescription.Text = value.Trim().Replace("\n", "<br />"); }
    }

    public string BackupSetBackupServer
    {
        set { lblBackupSetBackupServer.Text = value;  }
    }

    /// <summary>
    /// set created date
    /// </summary>
    public DateTime Created
    {
        set { litCreated.Text = value.ToString(); }
    }

    /// <summary>
    /// set last updated date
    /// </summary>
    public DateTime LastUpdated
    {
        set { litLastUpdated.Text = value.ToString(); }
    }

    /// <summary>
    /// set control edit mode
    /// </summary>
    public EditMode FormEditMode
    {
        set { _editMode = value; }
    }

    /// <summary>
    /// get/set backup monitor status type id
    /// </summary>
    public int BackupMonitorStatusId
    {
        get { return ucBackupMonitorStatusRBL.StatusId; }
        set { ucBackupMonitorStatusRBL.StatusId = value; }
    }


    #endregion

    #region public event

    /// <summary>
    /// event handler for parent page to use on update/add button click
    /// </summary>
    public event EventHandler evButtonClick;

    #endregion

    #region public enum

    /// <summary>
    /// enum type for edit mode
    /// </summary>
    public enum EditMode
    {
        Grid,
        Page
    }

    #endregion
}