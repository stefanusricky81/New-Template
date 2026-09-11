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

public partial class UserControl_DropDownList_GridExportType : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region public methods

    /// <summary>
    /// populate grid
    /// </summary>
    public void Populate()
    {
        ddlExportType.DataSource = Enum.GetNames(typeof(ExportMode));
        ddlExportType.DataBind();
    }

    #endregion

    #region public properties

    /// <summary>
    /// set width for drop down list
    /// </summary>
    public Unit Width
    {
        set { ddlExportType.Width = value; }
    }

    /// <summary>
    /// get drop down list control
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlExportType;
    }

    /// <summary>
    /// get/set mode
    /// </summary>
    public ExportMode Mode
    {
        get
        {
            return (ExportMode)Enum.Parse
                (typeof(ExportMode), ddlExportType.SelectedItem.Value, true);
        }
        set
        {
            ListItem li = ddlExportType.Items.FindByValue(value.ToString());
            if (li != null)
            {
                ddlExportType.ClearSelection();
                li.Selected = true;
            }
        }
    }

    #endregion

    #region public enum

    /// <summary>
    /// export mode enum
    /// </summary>
    public enum ExportMode
    {
        Csv,
        Excel,
        Pdf,
        Word
    }

    #endregion
}
