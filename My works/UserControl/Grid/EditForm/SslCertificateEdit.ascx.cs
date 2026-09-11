using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControl_Grid_EditForm_SslCertificateEdit : AbstractSslCertificateEdit
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
        if (_editMode == EditMode.Grid)
        {
            mvButton.SetActiveView(viewGrid);
            mvButtonTop.SetActiveView(viewGridTop);
        }
        else
        {
            mvButton.SetActiveView(viewPage);
            mvButtonTop.SetActiveView(viewPageTop);

            divMain.Style.Add("border", "1px solid #3b5a82");
        }

        chkArchived.InputAttributes.Add("class", "checkbox");
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

    #endregion

    #region public override methods

    /// <summary>
    /// load ssl certificate values
    /// </summary>
    /// <param name="sslCertificateID"></param>
    public override void LoadValues(int sslCertificateID)
    {
        if (sslCertificateID > 0)
        {
            DesktopShared.TypedListClasses.SslCertificateRow sslCertificateTypedListRow = 
                DesktopShared.SslCertificate.GetTypedListRow(sslCertificateID);

            LoadValues(sslCertificateTypedListRow);
        }
        else
            LoadValues(null);
    }

    /// <summary>
    /// load ssl certificate values
    /// </summary>
    /// <param name="sslCertificateTypedListRow"></param>
    public override void LoadValues(DesktopShared.TypedListClasses.SslCertificateRow sslCertificateTypedListRow)
    {
        btnInsert.Visible = (sslCertificateTypedListRow == null);
        btnInsertTop.Visible = (sslCertificateTypedListRow == null);

        btnUpdate.Visible = (sslCertificateTypedListRow != null);
        btnUpdateTop.Visible = (sslCertificateTypedListRow != null);

        phExistingFields.Visible = (sslCertificateTypedListRow != null);        
        
        #region existing record

        if (sslCertificateTypedListRow != null)
        {
            //link to detail apge
            hlSslCertificateId.Text = sslCertificateTypedListRow.Id.ToString();
            hlSslCertificateId.NavigateUrl = String.Format("/SslCertificate/Detail.aspx?Id={0}", sslCertificateTypedListRow.Id);

            Fqdn = sslCertificateTypedListRow.Fqdn.Trim();
            StartDate = sslCertificateTypedListRow.StartDate;
            EndDate = sslCertificateTypedListRow.EndDate;
            SslCertificateTypeId = sslCertificateTypedListRow.SslCertificateTypeId;
            SelectedClientId = sslCertificateTypedListRow.ClientId;
            Description = sslCertificateTypedListRow.Description.Trim();
            PurchasedByClient = sslCertificateTypedListRow.PurchasedByClient;
            Archived = sslCertificateTypedListRow.Archive;
            Created = sslCertificateTypedListRow.Created;
            LastUpdated = sslCertificateTypedListRow.LastUpdated;

            if (_editMode == EditMode.Page)
                btnUpdatePage.Text = "UPDATE";
        }

        #endregion

        #region new record

        else
        {
            if (SelectedClientId > 0) //default selected client id
                ucClientComboBox.SelectedClientId = SelectedClientId;

            ucSslCertificateTypeDDL.Populate(); //populate cert type drop down list

            phSslCertificateId.Visible = false; //hide id link

            if (_editMode == EditMode.Page)
                btnUpdatePage.Text = "ADD";
        }

        #endregion

    }

    /// <summary>
    /// save ssl certificate values
    /// </summary>
    /// <param name="authenticationInfoId"></param>
    /// <returns></returns>
    public override int SaveValues(int sslCertificateID)
    {
        DesktopShared.EntityClasses.SslCertificateEntity sslCertificate = null;
        DateTime _now = DateTime.Now;
        int _employeeId = DesktopShared.User.EmployeeID;

        if (sslCertificateID > 0) //existing record
            sslCertificate = new DesktopShared.EntityClasses.SslCertificateEntity(sslCertificateID);
        else //new record
        {
            sslCertificate = new DesktopShared.EntityClasses.SslCertificateEntity();
            sslCertificate.Created = _now;
            sslCertificate.CreatedByEmployeeId = _employeeId;
        }

        sslCertificate.Fqdn = Fqdn.Trim();
        sslCertificate.StartDate = StartDate;
        sslCertificate.EndDate = EndDate;
        sslCertificate.SslCertificateTypeId = SslCertificateTypeId;
        sslCertificate.ClientId = SelectedClientId;
        sslCertificate.Description = Description.Trim();
        sslCertificate.PurchasedByClient = PurchasedByClient;
        sslCertificate.Archive = Archived;
        sslCertificate.LastUpdated = _now;
        sslCertificate.LastUpdatedByEmployeeId = _employeeId;

        sslCertificate.Save();

        return sslCertificate.Id;
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set FQDN
    /// </summary>
    public string Fqdn
    {
        get { return tbFqdn.Text; }
        set { tbFqdn.Text = value; }
    }

    /// <summary>
    /// get/set description
    /// </summary>
    public string Description
    {
        get { return tbDescription.Text; }
        set { tbDescription.Text = value; }
    }

    /// <summary>
    /// get/set start date
    /// </summary>
    public DateTime StartDate
    {
        get { return rdpStartDate.SelectedDate.Value; }
        set { rdpStartDate.SelectedDate = value; }
    }

    /// <summary>
    /// get/set end date
    /// </summary>
    public DateTime EndDate
    {
        get { return rdpEndDate.SelectedDate.Value; }
        set { rdpEndDate.SelectedDate = value; }
    }

    /// <summary>
    /// get/set ssl certificate type id
    /// </summary>
    public int SslCertificateTypeId
    {
        get { return ucSslCertificateTypeDDL.SslCertificateTypeId; }
        set { ucSslCertificateTypeDDL.SslCertificateTypeId = value; }
    }

    /// <summary>
    /// set/set client id
    /// </summary>
    public override int SelectedClientId
    {
        get { return ucClientComboBox.SelectedClientId; }
        set { ucClientComboBox.SelectedClientId = value; }
    }

    /// <summary>
    /// get/set purchsed by client
    /// </summary>
    public bool PurchasedByClient
    {
        get { return chkPurchasedByClient.Checked; }
        set { chkPurchasedByClient.Checked = value; }
    }

    /// <summary>
    /// get/set archived
    /// </summary>
    public bool Archived
    {
        get { return chkArchived.Checked; }
        set { chkArchived.Checked = value; }
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
