using DesktopShared.CollectionClasses;
using DesktopShared.EntityClasses;
using DesktopShared.FactoryClasses;
using DesktopShared.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;

using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Client_ClientContacts : System.Web.UI.Page
{
    private string _errorheader = "File not uploaded. Please review required fields and formats.</br>";
    private string _error = string.Empty;

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();
        if (!IsPostBack)
        {
            int _id = 0;
            if (int.TryParse(BitByBit.Web.Request.GetString("ClientId").Trim(), out _id))
            {
                ClientId = _id;
                var objClient = new DesktopShared.EntityClasses.ClientEntity(ClientId.Value);
                if (objClient.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched)
                {
                    DisplayLocation = objClient.UseTicketLocation;
                    txtFirstName.Focus();
                    return;
                }
            }
            pnlMain.Visible = false;
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Unable to locate Client.", DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
        }
    }

    #region private methods

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            rgClient.RenderMode = RenderMode.Lightweight;
            ddlStatus.CssClass = "form-control";
            phTabletPagerCss.Visible = true;
        }
    }

    /// <summary>
    /// rebind documet grid
    /// </summary>
    private void RebindGrid()
    {
        rgClient.Visible = true;
        rgClient.EditIndexes.Clear();
        rgClient.DataSource = null;
        rgClient.Rebind();
    }

    #endregion

    #region protected events

    /// <summary>
    /// add new contact click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void hlAddNewRecord_Click(object sender, EventArgs e)
    {
        Response.Redirect("ClientContactAdd.aspx?ClientId=" + Convert.ToString(ClientId) + "&ClientContactID=0");
    }

    /// <summary>
    /// submit button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        RebindGrid();
    }

    /// <summary>
    /// clear button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtFirstName.Text = "";
        txtLastName.Text = "";
        ddlStatus.SelectedIndex = 1;
        RebindGrid();
    }


    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClient_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        string clientemailcategory = string.Empty;
        if (ddlClientEmailCategory.ClientEmailCategoryId != null)
            clientemailcategory = ddlClientEmailCategory.ClientEmailCategoryName.Trim().ToString();

        var _dtContacts = DesktopShared.Client.Search("", "", "","", txtLastName.Text.Trim(), txtFirstName.Text.Trim(), ClientId.ToString(), "", ddlStatus.SelectedValue.Trim(), false, -1, clientemailcategory,txtPhone.Text.Trim(), cbVIP.Checked);
        _dtContacts.DefaultView.Sort = "Last ASC";
        rgClient.DataSource = _dtContacts;
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClient_ItemDataBound(object sender, GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = e.Item as GridDataItem;
            DataRowView _drv = (DataRowView)e.Item.DataItem;

            string _ext = _drv["BusExt"].ToString();
            string _phone = String.Format("{0}{1}", _drv["ContactBusPhone"].ToString(), String.IsNullOrWhiteSpace(_ext) ? "" : String.Format(" x {0}", _ext));

            TelerikHelper.AddLabelToCell(_drv["UserPriority"].ToString(), _gdi["UserPriority"]);
            //TelerikHelper.AddLabelToCell(_drv["Last"].ToString(), _gdi["Last"]);
            TelerikHelper.AddLabelToCell(_drv["First"].ToString(), _gdi["First"]);
            TelerikHelper.AddLabelToCell(_drv["Email"].ToString(), _gdi["Email"]);

            _gdi["ContactBusPhone"].Text = PhoneFormat.CellPhone.cellformat(_drv["ContactBusPhone"].ToString());
            _gdi["ContactCellPhone"].Text = PhoneFormat.CellPhone.cellformat(_drv["ContactCellPhone"].ToString());
            //TelerikHelper.AddLabelToCell(_phone, _gdi["ContactBusPhone"]);
            //TelerikHelper.AddLabelToCell(_drv["ContactCellPhone"].ToString(), _gdi["ContactCellPhone"]);
            TelerikHelper.AddLabelToCell(_drv["LocationName"].ToString(), _gdi["LocationName"]); 
            TelerikHelper.AddLabelToCell(_drv["ContactActive"].ToString(), _gdi["ContactActive"]);

        }

        #endregion
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClient_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;
            rgClient.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgClient.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgClient.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgClient.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgClient.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(250);
            rgClient.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgClient.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgClient.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgClient.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
        }

        if (DisplayLocation)
        {
            rgClient.MasterTableView.Columns.FindByUniqueName("LocationName").Visible = true;
            rgClient.MasterTableView.Columns.FindByUniqueName("Email").HeaderStyle.Width = Unit.Percentage(15);
            rgClient.MasterTableView.Columns.FindByUniqueName("ContactBusPhone").HeaderStyle.Width = Unit.Percentage(10);
            rgClient.MasterTableView.Columns.FindByUniqueName("ContactCellPhone").HeaderStyle.Width = Unit.Percentage(10);
            rgClient.Rebind();
        }
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set client id
    /// </summary>
    private int? ClientId
    {
        get
        {
            object obj = this.ViewState["ClientId_cc"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["ClientId_cc"] = value; }
    }

    /// <summary>
    /// get/set display location
    /// </summary>
    private bool DisplayLocation
    {
        get
        {
            object obj = this.ViewState["dl_cc"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["dl_cc"] = value; }
    }

    #endregion

    #region add by Ricky
    protected void hlBulkAdd_Click(object sender, EventArgs e)
    {
        string _loc = string.Empty;
        if (true)
        {
            lblModalTitleBulkAdd.Text = "Bulk Upload";
            _loc= location(ClientId);
            if (_loc == string.Empty)
                lblLocation.Text = "Client Has No Locations. Leave Primary Location Field Empty";
            else
                lblLocation.Text = location(ClientId);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalBulkAdd", "$('#myModalBulkAdd').modal('show');", true);
        }
    }
    private string location(int? clientid)
    {
        string _loc = string.Empty;
        try {
            DataTable dt = new DataTable();

            ClientLocationCollection _client =DesktopShared.Client.Location.Get(clientid.Value);
            for (int i = 0; i <= _client.Count - 1; i++)
            {
                if (_loc == string.Empty)
                    _loc = _client[i].Name.Trim().ToString();
                else
                    _loc += ", " + _client[i].Name.Trim().ToString();
            }
            return _loc;
        }
        catch {
            return string.Empty;
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        var coverFolderPath1 = Server.MapPath(@"~/Archieves");
        var directory = new DirectoryInfo(coverFolderPath1);

        // or...
        FileInfo myFile = directory.GetFiles()
                        .OrderByDescending(f => f.LastWriteTime)
                        .First();

        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + myFile.Name + "\"");
        Response.TransmitFile(myFile.FullName);
        Response.End();
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        int _success = 0;
        int totalallrecord = 0;
        string sheetname = string.Empty;
        string Conn = string.Empty;
        string excelpath = string.Empty;
        int _primaryId = 0;
        List<int> _selectedIds= new List<int>();
        bool _failed = false;
        int UserId = 0;
        string _rowerror = string.Empty;
        try
        {
            foreach (UploadedFile file in rauFileUpload.UploadedFiles)
            {
                UploadedFileInfo uploadedFileInfo = new UploadedFileInfo(file);
                excelpath = Server.MapPath("~/Archieves/Imports/") + file.GetNameWithoutExtension() + "-" + DesktopShared.User.UserName + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + file.GetExtension();
                file.SaveAs(excelpath);

                string connectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + excelpath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";
                OleDbConnection ad = new OleDbConnection(connectionstring);
                ad.Open();
                DataTable dt = ad.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                sheetname = dt.Rows[0]["TABLE_NAME"].ToString();
                DataSet ds = new DataSet();
                OleDbCommand command = new OleDbCommand("select * from [" + sheetname + "]", ad);
                string q = "select * from [" + sheetname + "]";

                OleDbDataAdapter oda = new OleDbDataAdapter(q, ad);
                oda.Fill(ds);
                ad.Close();
                totalallrecord = ds.Tables[0].Rows.Count;
                //Check all mandatory and primary location
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    _error = string.Empty;
                    int _index = ds.Tables[0].Rows.IndexOf(row);
                    //_error += string.Format("Row {0}: </br>", _index + 1);

                    #region Check for length
                    
                    if (checkLength(row[0].ToString(), string.Format(" &emsp; Maximum Length for first name {0}", 12), 12) == false)
                        _failed = true;
                    if (checkLength(row[1].ToString(), string.Format(" &emsp; Maximum Length for last name {0}", 18), 18) == false)
                        _failed = true;
                    if (checkLength(row[2].ToString(), string.Format(" &emsp; Maximum Length for email {0}", 75), 75) == false)
                        _failed = true;
                    if (row[13].ToString().ToUpper() == "US")
                    {
                        if (checkLength(row[3].ToString(), string.Format(" &emsp; Maximum Length for work phone {0}", 30), 30) == false)
                            _failed = true;
                        if (checkLength(row[5].ToString(), string.Format(" &emsp; Maximum Length for cell phone {0}", 17), 17) == false)
                            _failed = true;
                        if (checkLength(row[6].ToString(), string.Format(" &emsp; Maximum Length for fax {0}", 17), 17) == false)
                            _failed = true;
                        if (checkLength(row[11].ToString(), string.Format(" &emsp; Invalid state {0} maximum length for state {1}", row[10].ToString(), 2), 2) == false)
                            _failed = true;
                    }
                    if (checkLength(row[4].ToString(), string.Format(" &emsp; Maximum Length for ext {0}", 6), 6) == false)
                        _failed = true;
                    if (checkLength(row[8].ToString(), string.Format(" &emsp; Maximum Length for addr1 {0}", 30), 30) == false)
                        _failed = true;
                    if (checkLength(row[9].ToString(), string.Format(" &emsp; Maximum Length for addr2 {0}", 30), 30) == false)
                        _failed = true;
                    if (checkLength(row[10].ToString(), string.Format(" &emsp; Maximum Length for city {0}", 18), 18) == false)
                        _failed = true;
                    if (checkLength(row[12].ToString(), string.Format(" &emsp; Maximum Length for zip code {0}", 10), 10) == false)
                        _failed = true;
                    if (checkLength(row[13].ToString(), string.Format(" &emsp; Maximum Length for country {0}", 2), 2) == false)
                        _failed = true;
                    #endregion

                    #region Check Mandatory
                    if (checkMandatory(row[0].ToString(), " &emsp; Required First Name") == false)
                        _failed = true;
                    if (checkMandatory(row[1].ToString(), " &emsp; Required Last Name") == false)
                        _failed = true;
                    if (checkMandatory(row[2].ToString(), " &emsp; Required Email") == false)
                        _failed = true;
                    if (checkMandatory(row[3].ToString(), " &emsp; Required Work Phone") == false)
                        _failed = true;
                    //if (checkMandatory(row[13].ToString(), " &emsp; Required Country") == false)
                    //    _failed = true;
                    #endregion

                    #region Check Format
                    if (row[2].ToString()!=string.Empty && DesktopShared.Utility.IsValidEmailAddress(row[2].ToString()) == false)
                    {
                        _error += string.Format(" &emsp; Invalid Email Address = {0}", row[2].ToString()) + "</br>";
                        _failed = true;
                    }
                    if (row[13].ToString().ToUpper() == "US")
                    {
                        if (row[3].ToString() != string.Empty && checkFormatPhone(row[3].ToString(), " &emsp; Invalid Work Phone #. Must be ###-###-#### / (###) ###-#### format.") == false)
                            _failed = true;
                        if (row[5].ToString() != string.Empty && checkFormatPhone(row[5].ToString(), " &emsp; Invalid Cell Phone #. Must be ###-###-#### / (###) ###-#### format.") == false)
                            _failed = true;
                    }
                    if (row[7].ToString() != string.Empty)
                    {
                        if (location(ClientId) != string.Empty)
                        {
                            _primaryId = GetClientLocID(row[7].ToString(), ClientId);
                            if (_primaryId == 0)
                            {
                                _error += string.Format(" &emsp; Invalid Primary Location= {0}", row[7].ToString()) + "</br>";
                                _failed = true;
                            }
                        }
                    }
                    //state
                    if (row[13].ToString().ToUpper() == "US")
                    {
                        if (row[11].ToString() != string.Empty)
                        {
                            if (checkstate(row[11].ToString().Trim()) == false)
                            {
                                _error += string.Format(" &emsp; Invalid State = {0}", row[11].ToString()) + "</br>";
                                _failed = true;
                            }

                        }
                    }
                    //Zipcode
                    //if (row[12].ToString() != string.Empty && row[13].ToString().ToUpper() == "US")
                    //{
                    //    if (DesktopShared.Utility.IsValidZipCode(row[12].ToString().Trim()) == false)
                    //    {
                    //        _error += string.Format(" &emsp; Invalid Zip Code = {0}", row[12].ToString()) + "</br>";
                    //        _failed = true;
                    //    }
                    //}
                    #endregion
                    if(_failed == true)
                        _rowerror += string.Format("Row {0}: </br> {1}", _index + 1, _error);
                }

                if (_failed == true)
                {
                    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, string.Format("{0}{1}", _errorheader, _rowerror), DesktopShared.Bootstrap.Alert.AlertType.Danger);
                    return;
                }
                //processing import all data
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string state = string.Empty;
                    int _pclientcontact = 0;
                    string fname = row[0].ToString();
                    string lname = row[1].ToString();
                    string email = row[2].ToString();
                    string wphone = row[3].ToString();
                    string ext = row[4].ToString();
                    string cphone = row[5].ToString();
                    string fax = row[6].ToString();
                    int primloc = GetClientLocID(row[7].ToString(), ClientId);
                    string addr1 = row[8].ToString();
                    string addr2 = row[9].ToString();
                    string city = row[10].ToString();
                    if (row[13].ToString().Trim() == string.Empty)
                    {
                        if (row[11].ToString().Trim().Length == 2)
                            state = row[11].ToString();
                        else
                            state = "NY";
                    }
                    else
                        state = row[11].ToString();

                    string zip = row[12].ToString();
                    int country = row[13].ToString().Trim() == string.Empty ? 1 : GetCountryID(row[13].ToString());

                    _pclientcontact = existingUser(email);
                    ClientContactEntity objClientsContact = _pclientcontact > 0 ? new ClientContactEntity(_pclientcontact) : new ClientContactEntity();
                    objClientsContact.First = fname;
                    objClientsContact.Last = lname;
                    objClientsContact.Email = email;
                    objClientsContact.Busphone = wphone;
                    objClientsContact.Cellphone = cphone;
                    objClientsContact.Faxphone = fax;
                    objClientsContact.FkClient = ClientId;
                    objClientsContact.Addr1 = addr1;
                    objClientsContact.Addr2 = addr2;
                    objClientsContact.City = city;
                    if (country == 1)
                        objClientsContact.State = state;
                    else
                        objClientsContact.StateProvenceRegion = state;
                    objClientsContact.Zip = zip;
                    objClientsContact.Created = DateTime.Now.ToString();
                    objClientsContact.LastUpdatedby = DesktopShared.User.UserName;
                    objClientsContact.Active = "Y";
                    objClientsContact.Country = country;
                    objClientsContact.Save();

                    var objUser = DesktopShared.User.GetForClientContact(objClientsContact.PclientContact);
                    if ((objUser != null) && (objUser.Fields.State == EntityState.Fetched))
                    {
                        objUser.RankingId = null;

                        objUser.ClientPortalActive = true;
                        objUser.Save();
                        UserId = objUser.Pusers;
                    }

                    if (primloc != 0)
                    {
                        _selectedIds.Add(primloc);
                        DesktopShared.User.Client.Location.Set(UserId, _selectedIds, primloc, DesktopShared.User.UserID);
                    }
                    _success++;
                }

            }
            if (totalallrecord == _success)
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, string.Format("Success added all {0} records", _success), DesktopShared.Bootstrap.Alert.AlertType.Success);
            else
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, string.Format("Success added {0} records and failed {1} records", _success, (totalallrecord - _success)), DesktopShared.Bootstrap.Alert.AlertType.Success);

            RebindGrid();
        }
        catch (Exception ex)
        {
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, ex.Message.ToString(), DesktopShared.Bootstrap.Alert.AlertType.Danger);
        }
        finally
        {
            File.Delete(excelpath);
        }
    }
    public static int GetCountryID(string Countrycode)
    {
        CountryCollection _country = new CountryCollection();

        IPredicateExpression countryFilter = new PredicateExpression();
        countryFilter.Add(CountryFields.IsoAlpha2 == Countrycode.ToString().Trim().ToUpper());

        _country.GetMulti(countryFilter);

        return _country.Count > 0 ? _country[0].Id : 0;
    }
    public static int GetClientLocID(string locname, int? clientid)
    {
        ClientLocationCollection _clientLoc = new ClientLocationCollection();

        IPredicateExpression clientslocFilter = new PredicateExpression();
        clientslocFilter.Add(ClientLocationFields.Name == locname.Trim());
        clientslocFilter.Add(ClientLocationFields.ClientId == clientid);

        _clientLoc.GetMulti(clientslocFilter);

        return _clientLoc.Count > 0 ? _clientLoc[0].Id : 0;
    }

    private bool checkMandatory(string _field, string message)
    {
        if (_field == string.Empty)
        {
            _error += message + "</br>";
            return false;
        }
        return true;
    }

    private bool checkLength(string _str, string message, int length)
    {
        if (_str.Length > length)
        {
            _error += message + "</br>";
            return false;
        }
        return true;
    }

    private bool checkFormatPhone(string phonenumber, string message)
    {
        //string pattern = @"^[\d\(\)\-+.\s]+$";
        string pattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        if (Regex.IsMatch(phonenumber, pattern)== false)
        {
            _error += message + "</br>";
            return false;
        }
        return true;
    }

    private bool checkstate(string _state)
    {
        StateCollection state = new StateCollection();
        IPredicateExpression stateFilter = new PredicateExpression();
        stateFilter.Add(StateFields.Pstate == _state.Trim());

        state.GetMulti(stateFilter);

        return state.Count > 0 ? true : false;
    }

    private int existingUser(string email)
    {
        ClientContactCollection _clientContact = new ClientContactCollection();
        IPredicateExpression _orFilter = new PredicateExpression();
        _orFilter.AddWithAnd(ClientContactFields.Email == email);

        //fetch
        _clientContact.GetMulti(_orFilter, 0, null);

        //return
        return _clientContact.Count > 0 ? _clientContact[0].PclientContact : 0;
    }
    #endregion

    protected void lbDeactivate_Click(object sender, EventArgs e)
    {
        try
        {
            int _count = 0;
            foreach (GridEditableItem item in rgClient.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("cboxSelect");
                if (chk.Checked == true)
                {
                    int ccid = (int)item.GetDataKeyValue("pclientcontact");

                    ClientContactEntity cc = new ClientContactEntity(ccid);

                    cc.Active = "N";
                    cc.LastUpdated = DateTime.Now.ToString();
                    cc.LastUpdatedby = DesktopShared.User.UserID.ToString();
                    cc.Save();
                    _count++;
                }
            }

            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("{0} Client {1} have been deactivated", _count, _count > 1 ? "Contacts" : "Contact"), DesktopShared.Bootstrap.Alert.AlertType.Success, false);
        }
        catch (Exception ex)
        {
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, ex.Message.ToString(), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
        }
    }

    protected void lbexport_Click(object sender, EventArgs e)
    {
        rgClient.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;

        rgClient.MasterTableView.GetColumn("CheckBoxTemplateColumn").Visible = false;
        rgClient.MasterTableView.GetColumn("ActionColumn").Visible = false;
        rgClient.MasterTableView.GetColumn("Last").Visible = false;
        rgClient.MasterTableView.GetColumn("LastName").Display = true;

        rgClient.ExportSettings.IgnorePaging = true;
        rgClient.ExportSettings.ExportOnlyData = true;
        rgClient.ExportSettings.HideStructureColumns = true;

        rgClient.GridLines = GridLines.Both;
        rgClient.BorderStyle = BorderStyle.Solid;

        rgClient.ExportSettings.FileName = "Client Contact :" + ClientId;

        rgClient.MasterTableView.ExportToExcel();
    }

    protected void rgClient_ItemCommand(object sender, GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();
        string err = string.Empty;
        switch (command)
        {
            case "Deactivate":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    int ccid = (int)item.GetDataKeyValue("pclientcontact");
                    string fname = item["First"].Text.Trim();
                    string lname = item["LastName"].Text.Trim();
                    ClientContactEntity cc = new ClientContactEntity(ccid);

                    cc.Active = "N";
                    cc.LastUpdated = DateTime.Now.ToString();
                    cc.LastUpdatedby = DesktopShared.User.UserID.ToString();
                    cc.Save();

                    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("{0} {1} have been deactivated", fname,lname), DesktopShared.Bootstrap.Alert.AlertType.Success, false);
                }
                break;
        }
    }
}