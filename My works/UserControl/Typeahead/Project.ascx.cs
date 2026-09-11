using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Typeahead_Project : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region public methods

    /// <summary>
    /// clear all items from combo box
    /// </summary>
    public void ClearItems()
    {
        txtProject.Value = "";
    }

    #endregion

    #region public properties

    /// <summary>
    /// set selectd client to be used for query
    /// </summary>
    public int? SelectedClientId
    {
        set { hdnClientId.Value = value.HasValue ? value.Value.ToString() : ""; }
    }

    /// <summary>
    /// get selected project id
    /// </summary>
    public int? ProjectId
    {
        get
        {
            if (!String.IsNullOrEmpty(hdnProjectId.Value))
            {
                int _id = -1;
                if (int.TryParse(hdnProjectId.Value, out _id))
                    return _id;
                else
                    return null;
            }
            else
                return null;
        }
        set
        {
            txtProject.Value = "";
            hdnProjectId.Value = "";
            if (!value.HasValue)
                return;

            var objProject = DesktopShared.Project.GetProjectRow(value.Value);
            if (objProject == null)
                return;

            txtProject.Value = String.Format("{0}: {1} - {2}", objProject.Pproject, objProject.Company.Trim(), objProject.Descr.Trim());
            hdnProjectId.Value = value.Value.ToString();
        }
    }

    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { txtProject.Attributes.Add("tabindex", value.ToString()); }
    }

    /// <summary>
    /// set is required
    /// </summary>
    public bool IsRequired
    {
        set { cvProject.Visible = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ValidationErrorMessage
    {
        set { cvProject.ErrorMessage = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { cvProject.ValidationGroup = value; }
    }


    /// <summary>
    /// get html text box
    /// </summary>
    public System.Web.UI.HtmlControls.HtmlInputText HtmlTextBox
    {
        get { return txtProject; }
    }

    #endregion

    #region custom validators

    /// <summary>
    /// validate client selected
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvProject_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = ProjectId.HasValue;
    }

    #endregion
}