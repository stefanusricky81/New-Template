using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_DID_DIDAssignment : System.Web.UI.Page
{
    private string _errorheader = "File not uploaded. Please review required fields and formats.</br>";
    private string _error = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindddlTrunkGroup();
            bindddlLocation();
            SetUpPage();
        }
    }

    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    private void SetUpPage()
    {
        txtDIDNumber.Focus();

        #region tab index
        short _tabIndex = 0;
        txtDIDNumber.TabIndex = ++_tabIndex;
        ddlTrunkGroup.TabIndex = ++_tabIndex;
        ddlAssigned.TabIndex = ++_tabIndex;
        ddlActive.TabIndex = ++_tabIndex;
        ddlClient.TabIndex = ++_tabIndex;
        btnSearch.TabIndex = ++_tabIndex;
        lbClear.TabIndex = ++_tabIndex;
        lbMultiAdd.TabIndex = ++_tabIndex;
        lbSingleAdd.TabIndex = ++_tabIndex;
        #endregion
    }
    #region Button
    protected void lbMultipleAdd_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleMultiAdd.Text = "Multi - Add";

            txtMultiAddRingtoNumber.Text = string.Empty;
            txtMultiAddDeviceName.Text = string.Empty;
            txtMultiAddDIDNumber.Text = string.Empty;
            ddlMultipleAddLocation.SelectedValue = string.Empty;
            ddlMultipleAddTrunk.SelectedValue = string.Empty;
            ddlMultiAddClient.ClientId = null;
            rblMultiAddAssigned.SelectedValue = string.Empty;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalMultiAdd", "$('#myModalMultiAdd').modal('show');", true);
        }
    }

    protected void lbSingleAdd_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleSingleAddEdit.Text = "Add/Edit DID Assignment";
            pnlEdit.Visible = false;
            lblSingleIdEdit.Text = string.Empty;
            btnAddSingleDIDAssignment.Visible = true;
            btnEditSingleDIDAssignment.Visible = false;

            txtAddSingleDIDNumber.Text = string.Empty;
            txtAddSingleRingToNumber.Text = string.Empty;
            txtAddSingleDeviceName.Text = string.Empty;

            ddlAddSingleLocation.SelectedValue = string.Empty;
            ddlAddSingleTrunk.SelectedValue = string.Empty;
            ddlAddSingleClient.ClientId = null;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalSingleAddEdit", "$('#myModalSingleAddEdit').modal('show');", true);
        }
    }

    protected void lbBulkAssign_Click(object sender, EventArgs e)
    {
        string _idnumber = string.Empty;
        int _check = 0;

        foreach (GridDataItem item in rgDIDAssignment.MasterTableView.Items)
        {
            CheckBox chkbox = item.FindControl("cbBulk") as CheckBox;
            if (chkbox != null && chkbox.Checked)
            {
                if (_idnumber == string.Empty)
                    _idnumber += item["didnumberid"].Text;
                else
                    _idnumber += "," + item["didnumberid"].Text;
                _check ++;
            }
        }
        if (_check > 0)
        {
            if (true)
            {
                lblModalBulkAssign.Text = "Bulk Assign";

                ddlClientBulkAssign.ClientId = null;
                BindBulkGrid(_idnumber);
                rgBulkAssign.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalBulkAssign", "$('#myModalBulkAssign').modal('show');", true);
            }
        }
        else
            DisplayMessage("You need check at least one to bulk assign", Bootstrap.Alert.AlertType.Warning);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgDIDAssignment.DataBind();
    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        txtDIDNumber.Text = string.Empty;
        ddlTrunkGroup.SelectedValue = string.Empty;
        ddlAssigned.SelectedValue = string.Empty;
        ddlActive.SelectedValue = string.Empty;
        ddlClient.ClientId = null;

        BindGrid();
        rgDIDAssignment.DataBind();
    }
    #endregion

    #region protected

    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        int _usercode= DesktopShared.User.UserID;
        string str = e.CommandArgument.ToString().Trim();
        string clientcode = string.Empty;

        switch (str)
        {
            case "AddSingle":
                clientcode = DesktopShared.DID.getclientcode(Convert.ToInt32(ddlAddSingleClient.ClientId));
                if (DesktopShared.DidAssignment.checkDIDNumber(txtAddSingleDIDNumber.Text.Trim().Replace("-","")) > 0)
                    DisplayMessage("DID Number already exist", Bootstrap.Alert.AlertType.Warning);
                else
                {
                    if (DesktopShared.DidAssignment.AddDIDAssignment(0, txtAddSingleDIDNumber.Text.Trim().Replace("-",""), txtAddSingleRingToNumber.Text.Trim()
                        , ddlAddSingleLocation.SelectedValue, clientcode, ddlAddSingleTrunk.SelectedValue, txtAddSingleDeviceName.Text.Trim()
                        , rblAddSingleAssigned.SelectedValue, string.Empty, cbAddSingleActive.Checked == true ? "Y" : "N", Convert.ToInt32(ddlAddSingleClient.ClientId)) == true)
                    {
                        DesktopShared.DidAudiTrail.AddDIDAuditTrail(txtAddSingleDIDNumber.Text, ddlAddSingleTrunk.SelectedItem.Text, rblAddSingleAssigned.SelectedValue, ddlAddSingleClient.ClientId.ToString(), _usercode, "SingleAdd");
                        DisplayMessage(String.Format("DID Assignment has been {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                        BindGrid();
                        rgDIDAssignment.DataBind();
                    }
                    else
                        DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                }
                break;
            case "AddMultiple":
                string[] rowlines = txtMultiAddDIDNumber.Text.Split(new Char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                //int number = rowlines.Length;
                string _faildinsert = string.Empty;
                string _exist = string.Empty;
                int j = 0;
                int existcount = 0;
                clientcode = DesktopShared.DID.getclientcode(Convert.ToInt32(ddlMultiAddClient.ClientId));
                for (int x = 0; x <= rowlines.Length - 1; x++)
                {
                    if (rowlines[x].Replace("\r", "").Length > 12)
                    {
                        DisplayMessage(String.Format("DID numbers is allowed a maximum of 12 characters"), Bootstrap.Alert.AlertType.Warning);
                        return;
                    }
                }
                for (int i = 0; i <= rowlines.Length - 1; i++)
                {
                    string _DIDnumbers = rowlines[i].ToString().Trim();
                    if (DesktopShared.DidAssignment.checkDIDNumber(_DIDnumbers.Replace("-","")) > 0)
                    {
                        existcount++;
                        j++;
                        if (_exist == string.Empty)
                            _exist += _DIDnumbers;
                        else
                            _exist += "," + _DIDnumbers;
                    }
                    else
                    {
                        if (DesktopShared.DidAssignment.AddDIDAssignment(0, _DIDnumbers.Replace("-",""), txtMultiAddRingtoNumber.Text.Trim()
                        , ddlMultipleAddLocation.SelectedValue, clientcode, ddlMultipleAddTrunk.SelectedValue, txtMultiAddDeviceName.Text.Trim()
                        , rblMultiAddAssigned.SelectedValue, string.Empty, cbMultiAddActive.Checked == true ? "Y" : "N", Convert.ToInt32(ddlMultiAddClient.ClientId)) == false)
                        {
                            j++;
                            if (_faildinsert == string.Empty)
                                _faildinsert += _DIDnumbers;
                            else
                                _faildinsert += "," + _DIDnumbers;
                        }
                        else
                        {
                            DesktopShared.DidAudiTrail.AddDIDAuditTrail(_DIDnumbers, ddlMultipleAddTrunk.SelectedItem.Text, rblMultiAddAssigned.SelectedValue, ddlMultiAddClient.ClientId.ToString(), _usercode, "Multi-Add");
                        }
                    }
                }
                if (j == 0)
                {
                    if (existcount == 0)
                    {
                        DisplayMessage(String.Format("All DID Assignment has been {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                        BindGrid();
                        rgDIDAssignment.DataBind();
                    }
                    //else
                    //{
                    //    DisplayMessage(String.Format("DID Assignment has been {0} , but some DID numbers already exist. Here are the existind DID {1} - {2}", "Added", _exist, DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    //    BindGrid();
                    //    rgDIDAssignment.DataBind();
                    //}
                }
                else
                {
                    if (existcount == 0)
                    {
                        DisplayMessage(String.Format("Some DID Assignment has been {0}, here are some data that failed to add {1} - {2}", "Added", _faildinsert, DateTime.Now), Bootstrap.Alert.AlertType.Warning);
                        BindGrid();
                        rgDIDAssignment.DataBind();
                    }
                    else
                    {
                        if (rowlines.Length==existcount)
                        {
                            DisplayMessage(String.Format("All DID numbers already exist. The following data already exists: {0} - {1}", _exist, DateTime.Now), Bootstrap.Alert.AlertType.Danger);
                            BindGrid();
                            rgDIDAssignment.DataBind();
                        }
                        else
                        {
                            DisplayMessage(String.Format("DID Assignment has been {0} , but some DID numbers already exist. Here are the existind DID {1} - {2}", "Added", _exist, DateTime.Now), Bootstrap.Alert.AlertType.Success);
                            BindGrid();
                            rgDIDAssignment.DataBind();
                        }
                    }
                }
                break;
            case "Edit":
                clientcode = DesktopShared.DID.getclientcode(Convert.ToInt32(ddlAddSingleClient.ClientId));
                if (hfEditOldDidnumber.Value.Trim() != txtAddSingleDIDNumber.Text.Trim())
                {
                    if (DesktopShared.DidAssignment.checkDIDNumber(txtAddSingleDIDNumber.Text.Trim().Replace("-","")) > 0)
                        DisplayMessage("DID Number already exist", Bootstrap.Alert.AlertType.Warning);
                    else
                    {
                        if (DesktopShared.DidAssignment.AddDIDAssignment(Convert.ToInt32(lblSingleIdEdit.Text), txtAddSingleDIDNumber.Text.Trim().Replace("-",""), txtAddSingleRingToNumber.Text.Trim()
                        , ddlAddSingleLocation.SelectedValue, clientcode, ddlAddSingleTrunk.SelectedValue, txtAddSingleDeviceName.Text.Trim()
                        , rblAddSingleAssigned.SelectedValue, string.Empty, cbAddSingleActive.Checked == true ? "Y" : "N", Convert.ToInt32(ddlAddSingleClient.ClientId)) == true)
                        {
                            DesktopShared.DidAudiTrail.AddDIDAuditTrail(txtAddSingleDIDNumber.Text, ddlAddSingleTrunk.SelectedItem.Text, rblAddSingleAssigned.SelectedValue, ddlAddSingleClient.ClientId.ToString(), _usercode, "Updated");
                            DisplayMessage(String.Format("DID Assignment has been {0} - {1}", "Edited", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                            BindGrid();
                            rgDIDAssignment.DataBind();
                        }
                        else
                            DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                    }
                }
                else
                {
                    if (DesktopShared.DidAssignment.AddDIDAssignment(Convert.ToInt32(lblSingleIdEdit.Text), txtAddSingleDIDNumber.Text.Trim().Replace("-",""), txtAddSingleRingToNumber.Text.Trim()
                        , ddlAddSingleLocation.SelectedValue, clientcode, ddlAddSingleTrunk.SelectedValue, txtAddSingleDeviceName.Text.Trim()
                        , rblAddSingleAssigned.SelectedValue, string.Empty, cbAddSingleActive.Checked == true ? "Y" : "N", Convert.ToInt32(ddlAddSingleClient.ClientId)) == true)
                    {
                        DesktopShared.DidAudiTrail.AddDIDAuditTrail(txtAddSingleDIDNumber.Text, ddlAddSingleTrunk.SelectedItem.Text, rblAddSingleAssigned.SelectedValue, ddlAddSingleClient.ClientId.ToString(), _usercode, "Updated");
                        DisplayMessage(String.Format("DID Assignment has been {0} - {1}", "Edited", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                        BindGrid();
                        rgDIDAssignment.DataBind();
                    }
                    else
                        DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                }
                break;
            case "Delete":
                if (DesktopShared.DidAssignment.DeleteDidAssignment(Convert.ToInt32(hfDelete.Value)))
                {
                    DesktopShared.DidAudiTrail.AddDIDAuditTrail(hfDeleteDIDNumber.Value, hfDeleteTrunkGroup.Value,hfAssigned.Value, hfDeleteClient.Value, _usercode,"Deleted");
                    
                    DisplayMessage(String.Format("DID Assignment has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgDIDAssignment.DataBind();
                }
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
            case "BulkAssign":
                //int _row = rgBulkAssign.MasterTableView.Items.Count;
                int failed = 0;
                string _faileddata = string.Empty;
                clientcode = DesktopShared.DID.getclientcode(Convert.ToInt32(ddlClientBulkAssign.ClientId));
                foreach (GridDataItem item in rgBulkAssign.MasterTableView.Items)
                {
                    int _id = Convert.ToInt32(item["didnumberid"].Text);
                    string _didNumber = item["didnumber"].Text;
                    string _trunk = item["trunkgroupdescr"].Text;
                    if (DesktopShared.DidAssignment.UpdateDIDBulkAssign(_id, cbActiveBulkAssign.Checked == true ? "Y" : "N", rblAssignedBulkAssign.SelectedValue, clientcode, Convert.ToInt32(ddlClientBulkAssign.ClientId)) == false)
                    {
                        failed++;
                        if (_faileddata == string.Empty)
                            _faileddata += _didNumber;
                        else
                            _faileddata += "," + _didNumber;
                    }
                    else
                    {
                        DesktopShared.DidAudiTrail.AddDIDAuditTrail(_didNumber, _trunk, rblAssignedBulkAssign.SelectedValue, ddlClientBulkAssign.ClientId.ToString(), _usercode, "Bulk Assign");
                    }
                }
                if (failed == 0)
                {
                    DisplayMessage(String.Format("Bulk Assign has been {0} - {1}", "Update", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgDIDAssignment.DataBind();
                }
                else
                {
                    DisplayMessage(String.Format("Some Bulk Assign has been {0}, here are some data that failed to update {1} - {2}", "Updated", _faileddata, DateTime.Now), Bootstrap.Alert.AlertType.Warning);
                    BindGrid();
                    rgDIDAssignment.DataBind();
                }
                
                break;
        }
    }

    protected void lbImport_Click(object sender, EventArgs e)
    {
        string _loc = string.Empty;
        if (true)
        {
            lblModalTitleImport.Text = "Import DID numbers";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalImport", "$('#myModalImport').modal('show');", true);
        }
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        int _success = 0;
        int totalallrecord = 0;
        string sheetname = string.Empty;
        string Conn = string.Empty;
        string excelpath = string.Empty;
        List<int> _selectedIds = new List<int>();
        bool _failed = false;
        string _rowerror = string.Empty;
        try
        {
            foreach (UploadedFile file in rauFileUpload.UploadedFiles)
            {
                UploadedFileInfo uploadedFileInfo = new UploadedFileInfo(file);
                excelpath = Server.MapPath("~/Archieves/") + file.GetNameWithoutExtension() + "-" + DesktopShared.User.UserName + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + file.GetExtension();
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
                if (totalallrecord > 0)
                {
                    //Check all mandatory and client name
                    #region all checking
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        _error = string.Empty;
                        _failed = false;
                        int _index = ds.Tables[0].Rows.IndexOf(row);
                        #region Check Mandatory
                        if (checkMandatory(row[0].ToString(), " &emsp; Required Trunk ID") == false)
                            _failed = true;
                        if (checkMandatory(row[2].ToString(), " &emsp; Required DID number") == false)
                            _failed = true;
                        #endregion

                        #region Check Length
                        if (row[2].ToString().Length > 12)
                        {
                            _error += string.Format(" &emsp; DID numbers is allowed a maximum of 12 characters : {0}", row[2].ToString()) + "</br>";
                            _failed = true;
                        }
                        #endregion

                        #region Other checking
                        if (row[0].ToString() != string.Empty)
                        {
                            int _id = 0;
                            bool result = int.TryParse(row[0].ToString().Trim(), out _id);
                            if (result == false)
                            {
                                _error += string.Format(" &emsp; TrunkID not numeric = {0}", row[0].ToString()) + "</br>";
                                _failed = true;
                            }
                            else if (checkingTrunkID(row[0].ToString().Trim()) == 0)
                            {
                                _error += string.Format(" &emsp; Invalid TrunkId = {0}", row[0].ToString()) + "</br>";
                                _failed = true;
                            }
                        }
                        //if (checkingClientName(row[4].ToString().Trim()) == 0)
                        //{
                        //    _error += string.Format(" &emsp; Invalid client name = {0}", row[4].ToString()) + "</br>";
                        //    _failed = true;
                        //}
                        #endregion
                        if (_failed == true)
                            _rowerror += string.Format("Row {0}: </br> {1}", _index + 1, _error);
                    }
                    #endregion
                    //end of checking
                    //if (_failed == true)
                    if(_rowerror != string.Empty)
                    {
                        DisplayMessage(string.Format("{0}{1}", _errorheader, _rowerror), Bootstrap.Alert.AlertType.Danger);
                        return;
                    }
                    //Processing into database
                    #region Database
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        int _trunkid = int.Parse(row[0].ToString().Trim());
                        string _trunkdesc = row[1].ToString().Trim();
                        string _didnumber = row[2].ToString().Trim().Replace("-", "");
                        string _ringtonumber = row[3].ToString().Trim();
                        int _didnumberid = getDIDNumberID(_didnumber);

                        string _type = _didnumberid == 0 ? "ImportAdded" : "ImportUpdated";

                        if (ImportDID(_didnumberid, _didnumber, _ringtonumber, _trunkid) == true)
                        {
                            _success++;
                            DesktopShared.DidAudiTrail.AddDIDAuditTrail(_didnumber, _trunkdesc, "N", string.Empty, DesktopShared.User.UserID, _type);
                        }

                    }
                    #endregion
                    //end of processing
                }
                else
                {
                    DisplayMessage("Empty File", Bootstrap.Alert.AlertType.Danger);
                    return;
                }
            }
            if (totalallrecord == _success)
                DisplayMessage(string.Format("Success added all {0} records", _success), Bootstrap.Alert.AlertType.Success);
            else
                DisplayMessage(string.Format("Success added {0} records and failed {1} records", _success, (totalallrecord - _success)), Bootstrap.Alert.AlertType.Success);
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message.ToString(), Bootstrap.Alert.AlertType.Danger);
        }
        finally
        {
            File.Delete(excelpath);
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        var coverFolderPath1 = Server.MapPath(@"~/Archieves/DID");
        var directory = new DirectoryInfo(coverFolderPath1);

        // or...
        FileInfo myFile = directory.GetFiles()
                        .OrderByDescending(f => f.LastWriteTime)
                        .First();

        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + myFile.Name + "\"");
        Response.TransmitFile(myFile.FullName);
        Response.End();
    }
    #endregion

    #region binddropdown
    private void bindddlTrunkGroup()
    {
        ddlTrunkGroup.DataSource = DesktopShared.DID.GetDDLTrunkGroup();
        ddlTrunkGroup.DataTextField = "trunkgroupdescr";
        ddlTrunkGroup.DataValueField = "Trunkgroupid";
        ddlTrunkGroup.DataBind();
        ddlTrunkGroup.Items.Insert(0, new ListItem("Select", string.Empty));

        ddlAddSingleTrunk.DataSource = DesktopShared.DID.GetDDLTrunkGroup();
        ddlAddSingleTrunk.DataTextField = "trunkgroupdescr";
        ddlAddSingleTrunk.DataValueField = "Trunkgroupid";
        ddlAddSingleTrunk.DataBind();
        ddlAddSingleTrunk.Items.Insert(0, new ListItem("Select", string.Empty));

        ddlMultipleAddTrunk.DataSource = DesktopShared.DID.GetDDLTrunkGroup();
        ddlMultipleAddTrunk.DataTextField = "trunkgroupdescr";
        ddlMultipleAddTrunk.DataValueField = "Trunkgroupid";
        ddlMultipleAddTrunk.DataBind();
        ddlMultipleAddTrunk.Items.Insert(0, new ListItem("Select", string.Empty));
    }

    private void bindddlLocation()
    {
        ddlAddSingleLocation.DataSource = DesktopShared.DID.GetDDLDIDLocation();
        ddlAddSingleLocation.DataTextField = "locationname";
        ddlAddSingleLocation.DataValueField = "didlocationid";
        ddlAddSingleLocation.DataBind();
        ddlAddSingleLocation.Items.Insert(0, new ListItem("Select", string.Empty));

        ddlMultipleAddLocation.DataSource = DesktopShared.DID.GetDDLDIDLocation();
        ddlMultipleAddLocation.DataTextField = "locationname";
        ddlMultipleAddLocation.DataValueField = "didlocationid";
        ddlMultipleAddLocation.DataBind();
        ddlMultipleAddLocation.Items.Insert(0, new ListItem("Select", string.Empty));
    }

    #endregion

    #region Private
    private bool checkMandatory(string _field, string message)
    {
        if (_field == string.Empty)
        {
            _error += message + "</br>";
            return false;
        }
        return true;
    }
    #endregion

    #region Grid
    private void BindGrid()
    {
        rgDIDAssignment.DataSource = DesktopShared.DidAssignment.getDIDAssignment(txtDIDNumber.Text.Trim(), ddlTrunkGroup.SelectedValue, ddlAssigned.SelectedValue, ddlActive.SelectedValue, ddlClient.ClientId);
    }

    protected void rgDIDAssignment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgDIDAssignment_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();

        switch (command)
        {
            case "UpdateRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    bindddlTrunkGroup();
                    bindddlLocation();
                    pnlEdit.Visible = true;
                    lblModalTitleSingleAddEdit.Text = "Add/Edit DID Assignment";
                    btnAddSingleDIDAssignment.Visible = false;
                    btnEditSingleDIDAssignment.Visible = true;
                    hfEditOldDidnumber.Value= item["didnumber"].Text == "&nbsp;" ? string.Empty : item["didnumber"].Text.Trim();
                    lblSingleIdEdit.Text = item["didnumberid"].Text;

                    txtAddSingleDIDNumber.Text = item["didnumber"].Text == "&nbsp;" ? string.Empty : item["didnumber"].Text.Trim();
                    txtAddSingleRingToNumber.Text = item["ringtodidnumber"].Text == "&nbsp;" ? string.Empty : item["ringtodidnumber"].Text.Trim();
                    txtAddSingleDeviceName.Text = item["diddevicename"].Text == "&nbsp;" ? string.Empty : item["diddevicename"].Text.Trim();
                    
                    try { ddlAddSingleLocation.SelectedValue = item["didlocationid"].Text == "&nbsp;" ? string.Empty : item["didlocationid"].Text.Trim(); }
                    catch { ddlAddSingleLocation.SelectedValue = string.Empty; }

                    try { ddlAddSingleTrunk.SelectedValue = item["trunkgroupid"].Text == "&nbsp;" ? string.Empty : item["trunkgroupid"].Text.Trim(); }
                    catch { ddlAddSingleTrunk.SelectedValue = string.Empty; }

                    if (item["pclient"].Text == "&nbsp;")
                        ddlAddSingleClient.ClientId = null;
                    else
                        ddlAddSingleClient.ClientId = Convert.ToInt32(item["pclient"].Text);

                    rblAddSingleAssigned.SelectedValue= item["assigned"].Text.Trim();
                    cbAddSingleActive.Checked = item["active"].Text.Trim() == "Y" ? true : false;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalSingleAddEdit", "$('#myModalSingleAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete DID Assignment";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["didnumber"].Text + " ? ";
                    hfDelete.Value = item["didnumberid"].Text;
                    hfDeleteDIDNumber.Value = item["didnumber"].Text;
                    hfDeleteTrunkGroup.Value= item["trunkgroupdescr"].Text;
                    hfDeleteClient.Value = item["clientcode"].Text;
                    hfAssigned.Value = item["assigned"].Text;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
            case "UpdateData":
                if (true)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalSingleAddEdit", "$('#myModalSingleAddEdit').modal('show');", true);
                }
                break;
        }
    }

    private void BindBulkGrid(string idnumber)
    {
        rgBulkAssign.DataSource = DesktopShared.DidAssignment.getBulkAssign(idnumber);
    }

    #endregion

    #region for importing into database
    //private static int checkingClientName(String _clientname)
    //{
    //    DesktopShared.CollectionClasses.ClientCollection client = new DesktopShared.CollectionClasses.ClientCollection();

    //    SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression _orFilter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression();
    //    _orFilter.AddWithAnd(DesktopShared.HelperClasses.ClientFields.Company == _clientname);

    //    //fetch
    //    client.GetMulti(_orFilter, 0, null);

    //    //return
    //    return client.Count;
    //}

    //private static int getPClient(String _clientname)
    //{
    //    DesktopShared.CollectionClasses.ClientCollection client = new DesktopShared.CollectionClasses.ClientCollection();

    //    SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression _orFilter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression();
    //    _orFilter.AddWithAnd(DesktopShared.HelperClasses.ClientFields.Company == _clientname);

    //    //fetch
    //    client.GetMulti(_orFilter, 0, null);

    //    //return
    //    return client.Count > 0 ? client[0].Pclient : 0;
    //}
    private static int checkingTrunkID(String _trunkid)
    {
        DesktopShared.CollectionClasses.TrunkgroupCollection _tg = new DesktopShared.CollectionClasses.TrunkgroupCollection();

        SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression _orFilter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression();
        _orFilter.AddWithAnd(DesktopShared.HelperClasses.TrunkgroupFields.Trunkgroupid == _trunkid);

        //fetch
        _tg.GetMulti(_orFilter, 0, null);

        //return
        return _tg.Count;
    }
    private static int getDIDNumberID(String _didnumbers)
    {
        DesktopShared.CollectionClasses.DidnumberCollection _did = new DesktopShared.CollectionClasses.DidnumberCollection();

        SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression _orFilter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression();
        _orFilter.AddWithAnd(DesktopShared.HelperClasses.DidnumberFields.Didnumber == _didnumbers);

        //fetch
        _did.GetMulti(_orFilter, 0, null);

        //return
        return _did.Count > 0 ? _did[0].Didnumberid : 0;
    }
    public static bool ImportDID(int pid, string didnumber, string ringtonumber, int trunkid)
    {
        try
        {
            DesktopShared.EntityClasses.DidnumberEntity didAssign = pid > 0 ? new DesktopShared.EntityClasses.DidnumberEntity(pid) : new DesktopShared.EntityClasses.DidnumberEntity();

            didAssign.Didnumber = didnumber;
            didAssign.Ringtodidnumber = ringtonumber;
            didAssign.Trunkgroupid = trunkid;
            //didAssign.Pclient = pclient;

            didAssign.Save();

            didAssign.Refetch();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    #endregion
}