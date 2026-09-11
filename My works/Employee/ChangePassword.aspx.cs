using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_ChangePassword : BasePage
{
    /// <summary>
    /// page pre init
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /*protected void Page_PreInit(object sender, EventArgs e)
    {
        this.CheckPassword = false;
    }*/

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        txtCurrentPassword.Attributes.Add("value", txtCurrentPassword.Text.Trim());
        txtNewPassword.Attributes.Add("value", txtNewPassword.Text.Trim());
        txtNew2Password.Attributes.Add("value", txtNew2Password.Text.Trim());

        if (!this.IsPostBack)
        {
            SetUpPage();
        }
    }

    #region private methods

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
        txtCurrentPassword.Focus();

        //tab index
        short _tabIndex = 0;
        txtCurrentPassword.TabIndex = ++_tabIndex;
        txtNewPassword.TabIndex = ++_tabIndex;
        txtNew2Password.TabIndex = ++_tabIndex;
        btnSubmit.TabIndex = ++_tabIndex;
    }

    #endregion

    #region protected events

    /// <summary>
    /// submit button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            var objEmployee = DesktopShared.Employee.GetEmployeeEntity(DesktopShared.User.EmployeeID);
            string _saltedPassword = "";
            string _salt = "";
            DesktopShared.Utility.PasswordHelper.HashPassword(txtNewPassword.Text.Trim(), ref _salt, ref _saltedPassword);

            if (objEmployee.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched)
            {
                objEmployee.Salt = _salt;
                objEmployee.SaltedPassword = _saltedPassword;
                objEmployee.Save();
            }

            //confirmation
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Password has been updated.", DesktopShared.Bootstrap.Alert.AlertType.Success);
            phMainForm.Visible = false;
        }
    }

    #region custom validators

    /// <summary>
    /// validate current password
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvCurrentPassword_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = DesktopShared.Employee.Password.Validate(txtCurrentPassword.Text.Trim(), DesktopShared.User.EmployeeID);
    }

    /// <summary>
    /// validate new password
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvNewPassword_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string _errorMessage = "";
        args.IsValid = DesktopShared.Employee.Password.IsValidNew(DesktopShared.User.EmployeeID, txtNewPassword.Text.Trim(), ref _errorMessage);
        cvNewPassword.ErrorMessage = _errorMessage;
    }

    #endregion

    #endregion


}