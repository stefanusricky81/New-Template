using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControl_Grid_EmployeeScheduling : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void RebindGrid()
    {
        rgAvailability.Visible = true;
        rgAvailability.EditIndexes.Clear();
        rgAvailability.DataSource = null;
        rgAvailability.Rebind();
    }

    protected void rgAvailability_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            DateTime date = Convert.ToDateTime(item["Date"].Text);
            DateTime estdate = TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            item["Date"].Text = estdate.ToString("yyyy-MM-dd HH:mm");

            DateTime start = Convert.ToDateTime(item["Start"].Text);
            DateTime eststart = TimeZoneInfo.ConvertTimeFromUtc(start, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            item["Start"].Text = eststart.ToString("yyyy-MM-dd HH:mm");

            DateTime end = Convert.ToDateTime(item["End"].Text);
            DateTime estend = TimeZoneInfo.ConvertTimeFromUtc(end, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            item["End"].Text = estend.ToString("yyyy-MM-dd HH:mm");
        }
    }

    public DataTable employee
    {
        get
        {
            object obj = this.ViewState["dt_employee"];
            return (obj == null) ? null : (DataTable)obj;
        }
        set { this.ViewState["dt_employee"] = value; }
    }

    protected void rgAvailability_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        rgAvailability.DataSource = employee;
    }
}