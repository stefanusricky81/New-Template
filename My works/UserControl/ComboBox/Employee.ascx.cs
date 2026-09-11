using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;

public partial class UserControl_ComboBox_Employee : System.Web.UI.UserControl
{
    //todo: once employee add/ edit page is created, remove cache upon insert/update on those pages

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void rcbEmployee_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        BindComboBox(e.Text.Trim());
    }

    private void BindComboBox(string employeeName)
    {
        BindComboBox(employeeName, -1);
    }

    private void BindComboBox(int employeeId)
    {
        BindComboBox("", employeeId);
    }

    private void BindComboBox(string employeeName, int employeeId)
    {
        #region get cached datatable of employees

        DataTable dtEmployee = null;
        if (Cache["dtEmployee"] == null)
        {
            dtEmployee = DesktopShared.Employee.GetActiveEmployees();

            Cache.Insert("dtEmployee", dtEmployee, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                TimeSpan.FromMinutes(120));
        }
        else
            dtEmployee = (DataTable)Cache["dtEmployee"];

        #endregion

        bool doBind = false;
        string filter = "";

        if (!String.IsNullOrEmpty(employeeName))
        {
            filter = String.Format("FullName like '%{0}%'", employeeName.Trim());
            doBind = true;
        }
        if (employeeId> 0)
        {
            filter = "Pusers = " + employeeId.ToString();
            doBind = true;
        }

        if (doBind)
        {
            dtEmployee.DefaultView.RowFilter = filter;
            dtEmployee.DefaultView.Sort = "FullName ASC";

            rcbEmployee.ClearSelection();
            rcbEmployee.DataSource = dtEmployee;
            rcbEmployee.DataTextField = "FullName";
            rcbEmployee.DataValueField = "Pusers";
            rcbEmployee.DataBind();
        }
        else
        {
            rcbEmployee.DataSource = null;
            rcbEmployee.DataBind();

        }
    }

    public void ClearItems()
    {
        rcbEmployee.Text = "";
        rcbEmployee.Items.Clear();
        rcbEmployee.Items.Add(new RadComboBoxItem());
    }

    public int EmployeeId
    {
        get
        {
            if (!String.IsNullOrEmpty(rcbEmployee.SelectedValue))
            {
                try { return Convert.ToInt32(rcbEmployee.SelectedValue); }
                catch { return -1; }
            }
            else
                return -1;
        }

        set
        {
            ClearItems();
            BindComboBox(value);
        }
    }

    public bool IsRequired
    {
        set { cvEmployee.Visible = value; }
    }

    public string ValidationErrorMessage
    {
        set { cvEmployee.ErrorMessage = value; }
    }

    public string ValidationGroup
    {
        set { cvEmployee.ValidationGroup = value; }
    }

    public Unit Width
    {
        set { rcbEmployee.Width = value; }
    }

    protected void cvEmployee_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (EmployeeId < 1)
            ClearItems();

        args.IsValid = EmployeeId > 0;
    }
}
