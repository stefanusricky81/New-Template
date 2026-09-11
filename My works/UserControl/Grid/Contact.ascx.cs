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
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_Grid_Contact : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void rgContact_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DesktopShared.TypedListClasses.ContactClientTypedList contacts = 
            new DesktopShared.TypedListClasses.ContactClientTypedList();

        IPredicateExpression contactsFilter = new PredicateExpression();
        if (ClientId > 0)
        {
            contactsFilter.Add(DesktopShared.HelperClasses.ClientContactFields.FkClient == ClientId);
        }

        ISortExpression contactsSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        contactsSort.Add(DesktopShared.HelperClasses.ClientContactFields.Last | SortOperator.Ascending);

        contacts.Fill(0, contactsSort, false, contactsFilter);
        rgContact.DataSource = contacts;

        //string.contacts[0].Busext
        /*DesktopShared.CollectionClasses.AuthenticationInfoCollection authInfo =
            new DesktopShared.CollectionClasses.AuthenticationInfoCollection();

        IPredicateExpression authInfoFilter = new PredicateExpression();
        authInfoFilter.Add(DesktopShared.HelperClasses.AuthenticationInfoFields.FkClient == ClientId);

        ISortExpression authInfoSort = new SortExpression();
        authInfoSort.Add(DesktopShared.HelperClasses.AuthenticationInfoFields.PAuthenticationInfo | SortOperator.Descending);

        authInfo.GetMulti(authInfoFilter, 0, authInfoSort);
        rgContact.DataSource = authInfo;*/

    }

    protected void rgContact_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            #region contact phone

            string extension = drv["ContactBusext"].ToString().Trim();
            if (extension.Length > 0)
            {
                string phone = drv["ContactBusphone"].ToString().Trim();

                TableCell tcContactBusphone = item["ContactBusphone"];
                tcContactBusphone.Text = phone + " x" + extension;
            }

            #endregion

            #region main phone

            string mainExtension = drv["Busext"].ToString().Trim();
            if (mainExtension.Length > 0)
            {
                string phone = drv["Busphone"].ToString().Trim();

                TableCell tcBusphone = item["Busphone"];
                tcBusphone.Text = phone + " x" + mainExtension;
            }

            #endregion

            /*TableCell tcPassword = item["Password"];
            DesktopShared.EntityClasses.AuthenticationInfoEntity authInfo =
                (DesktopShared.EntityClasses.AuthenticationInfoEntity)e.Item.DataItem;

            #region decrypt password

            string userName = authInfo.UserName.Trim();
            string description = authInfo.Description.Trim();
            string encryptedPwd = authInfo.Password.Trim();
            string clientID = authInfo.FkClient.ToString();

            BBBCRYPTLib.SecurityClass bbbSecurity = new BBBCRYPTLib.SecurityClass();
            string decryptedPwd = bbbSecurity.Decrypt(encryptedPwd,
                userName + "__" + clientID + description);

            if (!String.IsNullOrEmpty(decryptedPwd))
                tcPassword.Text = decryptedPwd;
            else
                tcPassword.Text = "&nbsp;";

            #endregion
             * */

        }

        #endregion

        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            UserControl userControl = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl;

            //if (!e.Item.OwnerTableView.IsItemInserted)
            //{
                GridDataItem parentItem = (e.Item as GridEditFormItem).ParentItem;
                GridEditableItem item = e.Item as GridEditableItem;
                /*TextBox tbPassword = (TextBox)userControl.FindControl("tbPassword");

                DesktopShared.EntityClasses.AuthenticationInfoEntity authInfo =
                    (DesktopShared.EntityClasses.AuthenticationInfoEntity)e.Item.DataItem;

                #region decrypt password

                string userName = authInfo.UserName.Trim();
                string description = authInfo.Description.Trim();
                string encryptedPwd = authInfo.Password.Trim();
                string clientID = authInfo.FkClient.ToString();

                BBBCRYPTLib.SecurityClass bbbSecurity = new BBBCRYPTLib.SecurityClass();
                string decryptedPwd = bbbSecurity.Decrypt(encryptedPwd,
                    userName + "__" + clientID + description);

                tbPassword.Text = decryptedPwd;

                #endregion

                userControl.FindControl("pnlHistory").Visible = true;
            }
            else
                userControl.FindControl("pnlHistory").Visible = false;*/

        }

        #endregion
    }

    protected void rgContact_UpdateCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
            HandleCommand(e, false);
    }

    protected void rgContact_InsertCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
            HandleCommand(e, true);
    }

    private void HandleCommand(GridCommandEventArgs e, bool isNew)
    {
        GridEditableItem editedItem = e.Item as GridEditableItem;
        

        /*
        string description = (userControl.FindControl("tbDescription") as TextBox).Text.Trim();
        string userName = (userControl.FindControl("tbUserName") as TextBox).Text.Trim();
        string password = (userControl.FindControl("tbPassword") as TextBox).Text.Trim();
        string existingComments = (userControl.FindControl("litComments") as Literal).Text.Trim();
        string newComments = (userControl.FindControl("tbNotes") as TextBox).Text.Trim();

        string comments = existingComments + "<br /><b>" + DateTime.Now.ToString();
        comments += "-" + DesktopShared.User.UserName + "</b>-EDIT<br />" + newComments;

        string fkClient = "";

        DesktopShared.EntityClasses.AuthenticationInfoEntity authInfo;

        DesktopShared.EntityClasses.AuthenticationInfoLogEntity authInfoLog =
            new DesktopShared.EntityClasses.AuthenticationInfoLogEntity();

        if (!isNew)
        {
            int Id = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues
                [editedItem.ItemIndex]["PAuthenticationInfo"]);
            fkClient = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["FkClient"].ToString();
            authInfo = new DesktopShared.EntityClasses.AuthenticationInfoEntity(Id);

            authInfoLog.Description = "UPDATED";
        }
        else
        {
            fkClient = ClientId.ToString();
            authInfo = new DesktopShared.EntityClasses.AuthenticationInfoEntity();

            authInfoLog.Description = "INSERT";
        }

        #region encrypt password

        BBBCRYPTLib.SecurityClass bbbSecurity = new BBBCRYPTLib.SecurityClass();
        string encryptedPassword = bbbSecurity.Encrypt(password,
            userName + "__" + fkClient + description);
        #endregion

        authInfo.Description = description;
        authInfo.UserName = userName;
        authInfo.Password = encryptedPassword;
        authInfo.FkClient = Convert.ToInt32(fkClient);
        authInfo.Comments = comments;

        authInfo.Save();

        authInfoLog.User = DesktopShared.User.UserName;
        authInfoLog.AuthenticationInfoId = authInfo.PAuthenticationInfo;
        authInfoLog.Save();*/

    }

    protected void rgContact_DeleteCommand(object source, GridCommandEventArgs e)
    {
        /*int Id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues
            [e.Item.ItemIndex]["PAuthenticationInfo"]);

        DesktopShared.EntityClasses.AuthenticationInfoEntity authInfo =
            new DesktopShared.EntityClasses.AuthenticationInfoEntity(Id);

        authInfo.Delete();

        DesktopShared.EntityClasses.AuthenticationInfoLogEntity authInfoLog =
            new DesktopShared.EntityClasses.AuthenticationInfoLogEntity();

        authInfoLog.Description = "DELETED";
        authInfoLog.User = DesktopShared.User.UserName;
        authInfoLog.AuthenticationInfoId = Id;
        authInfoLog.Save();*/

    }

    #region private variables held in Session state

    public int ClientId
    {
        get
        {
            object obj = this.ViewState["ClientIdForContact"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["ClientIdForContact"] = value;

            rgContact.DataSource = null;
            rgContact.Rebind();
        }
    }

    #endregion
}
