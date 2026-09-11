using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_UploadBillingUsers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        string errmessage = string.Empty;
        string Successmessage = string.Empty;
        string allerrmessage = string.Empty;
        int? projectbillinguserid = null;
        try 
        {
            string excelpath = string.Empty;
            string sheetname = string.Empty;

            #region Datatable for upload
            DataTable dtUpload = dt();
            DataRow drUpload;
            #endregion

            foreach (UploadedFile file in rauFileUpload.UploadedFiles)
            {
                if (file.GetExtension() == ".xlsx")
                {
                    UploadedFileInfo uploadedFileInfo = new UploadedFileInfo(file);
                    string filename = file.GetNameWithoutExtension() + "-" + DesktopShared.User.UserName + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + file.GetExtension();
                    excelpath = Server.MapPath("~/Archieves/Imports/") + filename;
                    file.SaveAs(excelpath);

                    string connectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + excelpath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";
                    OleDbConnection ad = new OleDbConnection(connectionstring);
                    ad.Open();
                    DataTable dtexcel = ad.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    sheetname = dtexcel.Rows[0]["TABLE_NAME"].ToString();//"Billing Extra Users$";//
                    DataSet dsExcel = new DataSet();
                    OleDbCommand command = new OleDbCommand("select * from [" + sheetname + "]", ad);
                    string q = "select * from [" + sheetname + "]";

                    OleDbDataAdapter oda = new OleDbDataAdapter(q, ad);
                    oda.Fill(dsExcel);
                    ad.Close();

                    #region validation
                    foreach (DataRow row in dsExcel.Tables[0].Rows)
                    {
                        if (checkMandatory(row[0].ToString(), ref errmessage) == false)
                        {
                            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Client {0}", errmessage.Trim()), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                            return;
                        }
                        if (checkMandatory(row[1].ToString(), ref errmessage) == false)
                        {
                            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Project ID {0}", errmessage.Trim()), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                            return;
                        }
                        //if (checkMandatory(row[14].ToString(), ref errmessage) == false)
                        //{
                        //    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Contract Term {0}", errmessage.Trim()), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                        //    return;
                        //}

                        DesktopShared.BillingExtraUsers.GetProjectBillingExtraUsers(Convert.ToInt32(row[1].ToString().Trim()), ref projectbillinguserid, ref errmessage);

                        bool chekcprojectexist = DesktopShared.BillingExtraUsers.CheckProjectExist(Convert.ToInt32(row[1].ToString().Trim()), ref errmessage);
                        if (chekcprojectexist == false)
                        {
                            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, errmessage, DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                            return;
                        }

                        if (DesktopShared.Client.GetByCode(row[0].ToString().Trim()) == null)
                        {
                            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Client don't Exist", DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                            return;
                        }

                        DesktopShared.EntityClasses.ClientEntity _client = DesktopShared.Client.GetByCode(row[0].ToString().Trim());
                        bool validclientproject = DesktopShared.BillingExtraUsers.CheckValidClientProject(Convert.ToInt32(row[1].ToString().Trim()), _client.Pclient, ref errmessage);
                        if (chekcprojectexist == false)
                        {
                            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, errmessage, DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                            return;
                        }
                        if (!String.IsNullOrEmpty(row[14].ToString().Trim()))
                        {
                            int exist = DesktopShared.ContractTerm.checkDuplicateTermCode(row[14].ToString().Trim(), ref errmessage);
                            if (exist < 1)
                            {
                                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Term don't Exist", DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                                return;
                            }
                        }
                    }
                    #endregion

                    foreach (DataRow row in dsExcel.Tables[0].Rows)
                    {
                        int projectid = Convert.ToInt32(row[1].ToString().Trim());
                        DataRow _row = DesktopShared.Project.GetRow(Convert.ToInt32(row[1].ToString()));
                        DesktopShared.BillingExtraUsers.GetProjectBillingExtraUsers(Convert.ToInt32(row[1].ToString().Trim()), ref projectbillinguserid, ref errmessage);
                        if (_row.Table.Rows.Count > 0)
                        {
                            DesktopShared.EntityClasses.ClientEntity _client = DesktopShared.Client.GetByCode(row[0].ToString().Trim());

                            drUpload = dtUpload.NewRow();
                            drUpload["Client"] = _client.Company;
                            //drUpload["ProjectID"] = DesktopShared.BillingExtraUsers.GetProjectName(Convert.ToInt32(row[1].ToString().Trim()), ref errmessage);
                            drUpload["ProjectID"] = row[1].ToString().Trim();
                            drUpload["MRR"] = row[2].ToString();
                            drUpload["FeeMonthlyMSP"] = row[3].ToString();
                            drUpload["FeeHowManyUsers"] = row[4].ToString();
                            drUpload["ActualCurrentO365"] = row[6].ToString();
                            drUpload["EachAddlUsers"] = row[7].ToString();
                            drUpload["AddlBillingUserIncrement"] = row[8].ToString();
                            drUpload["AdditionalUserNotes"] = row[9].ToString();
                            drUpload["IncludedDeviceSetUp"] = row[10].ToString();
                            drUpload["AddlDeviceFee"] = row[11].ToString();
                            drUpload["OnsiteDayRates"] = row[12].ToString();
                            drUpload["CappedHours"] = row[13].ToString();
                            drUpload["ContractTerm"] = String.IsNullOrEmpty(row[14].ToString().Trim()) ? "" : DesktopShared.ContractTerm.GetContractTermName(row[14].ToString().Trim());
                            drUpload["BackupDataSizeIncluded"] = row[15].ToString().Trim();
                            drUpload["ContractFound"] = "";
                            drUpload["ContractSignedDate"] = row[16].ToString();
                            drUpload["TerminationNotification"] = row[17].ToString();
                            drUpload["ContractEndDate"] = row[18].ToString();
                            drUpload["UsingClientMSA"] = row[19].ToString();
                            dtUpload.Rows.Add(drUpload);
                        }
                        long? contracttermid = null;

                        if (!String.IsNullOrEmpty(row[14].ToString().Trim()))
                            contracttermid = DesktopShared.ContractTerm.GetContractTermID(row[14].ToString().Trim());

                        if (DesktopShared.BillingExtraUsers.UpdateBillingExtraUsers(projectbillinguserid, projectid, row[2].ToString(), row[3].ToString(), row[4].ToString()
                            , row[6].ToString(), row[7].ToString(), row[8].ToString(), row[9].ToString()
                            , row[10].ToString(), row[11].ToString(), row[12].ToString(), row[13].ToString()
                            , contracttermid, row[15].ToString(), row[16].ToString(), row[17].ToString()
                            , row[18].ToString(), row[19].ToString(), DesktopShared.User.UserID, ref errmessage) == false)
                        {
                            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, errmessage, DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                            return;
                        }
                    }
                }
                else
                {
                    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Upload file must be in .xlsx format.", DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                    return;
                }
            }
            if (File.Exists(excelpath))
                File.Delete(excelpath);

            rgUploadBilling.DataSource = dtUpload;
            rgUploadBilling.Rebind();
            pnlResults.Visible = true;

            litMessage.Text = "";
        }
        catch (Exception ex)
        {
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, ex.Message.Trim(), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
            return;
        }
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dtExport = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetExportBillingUsers();
        //Exporttocsv(dtExport);
        tocsv(dtExport);
        //rgExport.DataSource = dtExport;
        //rgExport.Rebind();

        //rgExport.ExportSettings.FileName = "Billing Users -" + DateTime.Now.ToString();

        //rgExport.ExportSettings.ExportOnlyData = true;
        //rgExport.ExportSettings.OpenInNewWindow = true;
        //rgExport.MasterTableView.ExportToCSV();

    }

    protected void rgUploadBilling_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

    }

    protected bool checkMandatory(string data, ref string err)
    {
        try 
        {
            if (data == string.Empty)
            {
                err = "can't empty";
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            err = ex.Message.ToString();
            return false;
        }
    }

    protected DataTable dt()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Client", System.Type.GetType("System.String"));//0
        dt.Columns.Add("ProjectID", System.Type.GetType("System.String"));//1
        dt.Columns.Add("MRR", System.Type.GetType("System.String"));//2
        dt.Columns.Add("FeeMonthlyMSP", System.Type.GetType("System.String"));//3
        dt.Columns.Add("FeeHowManyUsers", System.Type.GetType("System.String"));//4
        dt.Columns.Add("ActualCurrentO365", System.Type.GetType("System.String"));//5
        dt.Columns.Add("EachAddlUsers", System.Type.GetType("System.String"));//6
        dt.Columns.Add("AddlBillingUserIncrement", System.Type.GetType("System.String"));//7
        dt.Columns.Add("AdditionalUserNotes", System.Type.GetType("System.String"));//8
        dt.Columns.Add("IncludedDeviceSetUp", System.Type.GetType("System.String"));//9
        dt.Columns.Add("AddlDeviceFee", System.Type.GetType("System.String"));//10
        dt.Columns.Add("OnsiteDayRates", System.Type.GetType("System.String"));//11
        dt.Columns.Add("CappedHours", System.Type.GetType("System.String"));//12
        dt.Columns.Add("ContractTerm", System.Type.GetType("System.String"));//13
        dt.Columns.Add("BackupDataSizeIncluded", System.Type.GetType("System.String"));//14
        dt.Columns.Add("ContractFound", System.Type.GetType("System.String"));//15
        dt.Columns.Add("ContractSignedDate", System.Type.GetType("System.String"));//16
        dt.Columns.Add("TerminationNotification", System.Type.GetType("System.String"));//17
        dt.Columns.Add("ContractEndDate", System.Type.GetType("System.String"));//18
        dt.Columns.Add("UsingClientMSA", System.Type.GetType("System.String"));//19

        return dt;
    }

    #region NOT USE
    //protected void rgExport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    //DataTable dtExport = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetExportBillingUsers();
    //    //rgExport.DataSource = dtExport;
    //    ////rgExport.DataBind();
    //}

    public void Exporttocsv(DataTable dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder str = new StringBuilder();
            str.Append("<table  align='center' border='1' bordercolor='#00aeef' width='99%' class='reporttable1' cellspacing='0' cellpadding='0' style='font-size:15;'>");
            str.Append("<tr>");
            #region for header
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                str.Append("<td>");
                str.Append("<b>" + dt.Columns[i].ColumnName + "</b>");
                str.Append("</td>");
            }
            #endregion
            str.Append("</tr>");

            #region for Detail
            for (int j = 0; j <= dt.Rows.Count - 1; j++)
            {
                str.Append("<tr>");
                for (int col = 0; col <= dt.Columns.Count - 1; col++)
                {
                    if (dt.Columns[col].ColumnName.ToString().Trim() != "EndUserName")
                    {
                        str.Append("<td>");

                        if (dt.Columns[col].ColumnName.ToString().Trim() == "ContractSignedDate")
                        {
                            if (dt.Rows[j][dt.Columns[col].ColumnName].ToString().Trim() != string.Empty)
                                str.Append(Convert.ToDateTime(dt.Rows[j][dt.Columns[col].ColumnName].ToString().Trim()).ToString("yyyy-MM-dd"));
                        }
                        else if (dt.Columns[col].ColumnName.ToString().Trim() == "ContractEndDate")
                        {
                            if (dt.Rows[j][dt.Columns[col].ColumnName].ToString().Trim() != string.Empty)
                                str.Append(Convert.ToDateTime(dt.Rows[j][dt.Columns[col].ColumnName].ToString().Trim()).ToString("yyyy-MM-dd"));
                        }
                        else
                            str.Append(dt.Rows[j][dt.Columns[col].ColumnName].ToString().Trim());
                        
                        str.Append("</td>");
                    }
                }
                str.Append("</tr>");
            }

            #endregion
            str.Append("</table>".ToString());

            sb.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
            sb.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
            sb.Append("<DIV style='font-size:12px;'>");
            sb.Append(str.ToString());
            sb.Append("</div></body></html>");
            string strFile = "Billing Extra Users -" + DateTime.Now.ToString() + ".xls";
            string strcontentType = "application/vnd.ms-excel";
            Response.ClearContent();
            Response.ClearHeaders();
            Response.BufferOutput = true;
            Response.ContentType = strcontentType;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
            Response.Write(sb.ToString());
            Response.Flush();
            Response.Close();
            Response.End();
        }
        catch (Exception ex)
        {
            litMessage.Text = ex.Message;
            return;
        }
    }
    #endregion

    public void tocsv(DataTable dtDataTable)
    {
        StringBuilder sb = new StringBuilder();

        string[] columnNames = dtDataTable.Columns.Cast<DataColumn>().
                                          Select(column => column.ColumnName).
                                          ToArray();
        sb.AppendLine(string.Join(",", columnNames));

        foreach (DataRow row in dtDataTable.Rows)
        {
            string[] fields = row.ItemArray.Select(field => field.ToString()).
                                            ToArray();
            if (fields[16].ToString() != string.Empty)
                fields[16] = Convert.ToDateTime(fields[16]).ToString("yyyy-MM-dd");

            if (fields[18].ToString() != string.Empty)
                fields[18] = Convert.ToDateTime(fields[18]).ToString("yyyy-MM-dd");

            sb.AppendLine(string.Join(",", fields));
        }

        //File.WriteAllText("test.csv", sb.ToString());
        Response.Clear();
        Response.ContentType = "application/CSV";
        Response.AddHeader("content-disposition", "attachment; filename=\"" + "Billing Extra Users" + ".csv\"");
        Response.Write(sb.ToString());
        Response.End();
    }
}