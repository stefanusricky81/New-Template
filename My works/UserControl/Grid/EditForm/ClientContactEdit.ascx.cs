using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class UserControl_Grid_EditForm_ClientContactEdit : AbstractClientContactEdit
{
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

        

    }

    #endregion

    public override void LoadValues(System.Data.DataRowView ClientContactRow)
    {
        btnInsert.Visible = (ClientContactRow == null);
        btnInsertTop.Visible = (ClientContactRow == null);

        btnUpdate.Visible = (ClientContactRow != null);
        btnUpdateTop.Visible = (ClientContactRow != null);

        phExistingFields.Visible = (ClientContactRow != null);

        #region existing record

        if (ClientContactRow != null)
        {
            tbFirst.Text = ClientContactRow["FirstName"].ToString().Trim();
            tbLast.Text = ClientContactRow["LastName"].ToString().Trim();
            tbEmail.Text = ClientContactRow["Email"].ToString().Trim();
            tbEmail2.Text = ClientContactRow["Email2"].ToString().Trim();
            tbEmail3.Text = ClientContactRow["Email3"].ToString().Trim();
            tbPhone.Text = ClientContactRow["Busphone"].ToString().Trim();
            tbExt.Text = ClientContactRow["Busext"].ToString().Trim();
            tbFax.Text = ClientContactRow["Faxphone"].ToString().Trim();
            if (tbFax.Text.Length == 10)
            {
                tbFax.Text = String.Format("{0:(###) ###-####}", Convert.ToInt64(ClientContactRow["Faxphone"].ToString().Trim()));
            }
                
            //tbFax.Text = String.Format("{0:(###) ###-####}", ClientContactRow["Faxphone"].ToString().Trim());
            tbCellPhone.Text = ClientContactRow["Cellphone"].ToString().Trim();
            tbAddress1.Text = ClientContactRow["Addr1"].ToString().Trim();
            tbAddress2.Text = ClientContactRow["Addr2"].ToString().Trim();
            tbCity.Text = ClientContactRow["City"].ToString().Trim();
            ddlState.SelectedValue = ClientContactRow["State"].ToString().Trim();
            tbZip.Text = ClientContactRow["Zip"].ToString().Trim();
            tbPassword.Text = ClientContactRow["Password"].ToString().Trim();
            ddlStatus.SelectedValue = ClientContactRow["Active"].ToString().Trim();
            tbCreated.Text = ClientContactRow["Created"].ToString().Trim();
            tbLastUpdated.Text = ClientContactRow["LastUpdated"].ToString().Trim();
            tbLastUpdatedBy.Text = ClientContactRow["LastUpdatedBy"].ToString().Trim();
            ucClientComboBox.SelectedClientId = Convert.ToInt32(ClientContactRow["FkClient"].ToString().Trim());


            hlClientContactId.Text = ClientContactRow["PclientContact"].ToString().Trim();
            hlClientContactId.NavigateUrl = String.Format("/Client/ClientContactDetail.aspx?Id={0}", int.Parse(ClientContactRow["PclientContact"].ToString()));


            if (_editMode == EditMode.Page)
                btnUpdatePage.Text = "UPDATE";

        }

        #endregion
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        

    }
    public event EventHandler evButtonClick;

    protected void btnUpdatePage_Click(object sender, EventArgs e)
    {
        
        evButtonClick(this, e);
       
    }


    private EditMode _editMode = EditMode.Grid;

    public enum EditMode
    {
        Grid,
        Page
    }

    public override void LoadValues(int ClientContactID)
    {
        LoadValues(null);
    }

    public override int SaveValues(int ClientContactID)
    {
        DesktopShared.EntityClasses.ClientContactEntity ClientContact = null;
        int _employeeId = DesktopShared.User.EmployeeID;


        if (ClientContactID > 0) //edit
        {
            ClientContact = new DesktopShared.EntityClasses.ClientContactEntity(ClientContactID);
            ClientContact.Created = Created;
            //ClientContact.LastUpdated = DateTime.Now.ToString();
        }
        else //add new
        {
            ClientContact = new DesktopShared.EntityClasses.ClientContactEntity();
            ClientContact.Created = DateTime.Now.ToString();
            ClientContact.LastUpdated= DateTime.Now.ToString();
        }

        ClientContact.First = First;
        ClientContact.Last = Last;
        ClientContact.Email = Email;
        ClientContact.Email2 = Email2;
        ClientContact.Email3 = Email3;

        ClientContact.Busphone = Phone; 
        ClientContact.Faxphone = Fax;
        ClientContact.Cellphone = CellPhone;
        ClientContact.Addr1 = Address1;
        ClientContact.Addr2 = Address2;
        ClientContact.City = City;
        ClientContact.State = State;
        ClientContact.Zip = Zip;
        ClientContact.Password = Password;
        ClientContact.Active = Active;
        ClientContact.LastUpdated = DateTime.Now.ToString();
        ClientContact.FkClient = FkClient;
        ClientContact.LastUpdatedby = LastUpdatedBy;
        ClientContact.Save();

        return ClientContact.PclientContact;
    }

    public string First
    {
        get { return tbFirst.Text; }
        set { tbFirst.Text = value; }
    }
    public string Last
    {
        get { return tbLast.Text; }
        set { tbLast.Text = value; }
    }
    public string Email
    {
        get { return tbEmail.Text; }
        set { tbEmail.Text = value; }
    }
    public string Email2
    {
        get { return tbEmail2.Text; }
        set { tbEmail2.Text = value; }
    }
    public string Email3
    {
        get { return tbEmail3.Text; }
        set { tbEmail3.Text = value; }
    }
    public string Phone
    {
        get { return tbPhone.Text; }
        set { tbPhone.Text = value; }
    }
    public string Fax
    {
        get { return tbFax.Text; }
        set { tbFax.Text = value; }
    }
    public string CellPhone
    {
        get { return tbCellPhone.Text; }
        set { tbCellPhone.Text = value; }
    }
    public string Address1
    {
        get { return tbAddress1.Text; }
        set { tbAddress1.Text = value; }
    }
    public string Address2
    {
        get { return tbAddress2.Text; }
        set { tbAddress2.Text = value; }
    }
    public string City
    {
        get { return tbCity.Text; }
        set { tbCity.Text = value; }
    }
    public string State
    {
        get { return ddlState.SelectedValue; }
        set { ddlState.SelectedValue = value; }
    }
    public string Zip
    {
        get { return tbZip.Text; }
        set { tbZip.Text = value; }
    }
    public string Password
    {
        get { return tbPassword.Text; }
        set { tbPassword.Text = value; }
    }

    public string Active
    {
        get { return ddlStatus.SelectedValue; }
        set { ddlStatus.SelectedValue = value; }
    }
    public string Created
    {
        get { return tbCreated.Text; }
        set { tbCreated.Text = value; }
    }
    public string LastUpdated
    {
        get { return tbLastUpdated.Text; }
        set { tbLastUpdated.Text = value; }
    }
    public string LastUpdatedBy
    {
        get { return tbLastUpdatedBy.Text; }
        set { tbLastUpdatedBy.Text = value; }
    }
    public override int FkClient
    {
        get { return ucClientComboBox.SelectedClientId; }
        set { ucClientComboBox.SelectedClientId = value; }
    }
    
}