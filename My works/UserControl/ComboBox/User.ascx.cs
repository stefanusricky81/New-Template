using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_ComboBox_User : System.Web.UI.UserControl
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
        rcbUser.Text = "";
        rcbUser.Items.Clear();
        rcbUser.Items.Add(new RadComboBoxItem());
    }

    #endregion

    #region public properties

    /// <summary>
    /// set/set selected user id
    /// </summary>
    public int? UserId
    {
        get
        {
            if (!String.IsNullOrEmpty(rcbUser.SelectedValue))
            {
                try { return Convert.ToInt32(rcbUser.SelectedValue); }
                catch { return null; }
            }
            else
                return null;
        }
        set
        {
            rcbUser.Text = "";
            rcbUser.Items.Clear();

            DesktopShared.EntityClasses.UsersEntity objUser = new DesktopShared.EntityClasses.UsersEntity(value.Value);
            if (objUser.Fields.State == EntityState.Fetched)
            {
                rcbUser.Items.Insert(0, new RadComboBoxItem(String.Format("{0} {1}", objUser.First.Trim(), objUser.Last.Trim()), objUser.Pusers.ToString()));
                rcbUser.SelectedIndex = 0;
            }
        }
    }

    /// <summary>
    /// get selected user name
    /// </summary>
    public string UserName
    {
        get
        {
            if (!String.IsNullOrEmpty(rcbUser.SelectedValue))
                return rcbUser.Text;
            else
                return "";
        }
    }

    /// <summary>
    /// set is required
    /// </summary>
    public bool IsRequired
    {
        set { cvUser.Visible = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ValidationErrorMessage
    {
        set { cvUser.ErrorMessage = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { cvUser.ValidationGroup = value; }
    }

    /// <summary>
    /// set combo box control width
    /// </summary>
    public Unit Width
    {
        set { rcbUser.Width = value; }
    }

    #endregion

    #region custom validators

    /// <summary>
    /// validate user selected from combo box
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvUser_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (!UserId.HasValue)
            ClearItems();

        args.IsValid = UserId.HasValue;
    }

    #endregion
}