using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;

public partial class UserControl_Grid_TicketTags : System.Web.UI.UserControl
{
    private string _defaultTextAddTag = "--select--";

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region protected events

    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTicketTags_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (TicketId > 0)
        {
            //dipslay grid
            rgTicketTags.Visible = true;

            //get typed list
            DesktopShared.TypedListClasses.TicketTagsTypedList ticketTags =
                DesktopShared.Ticket.Tagged.GetTagsForTicket(TicketId, DesktopShared.User.UserID);

            //set data source
            rgTicketTags.DataSource = ticketTags;

            //populate user tags for drop down list used for add
            PopulateUserTags(ticketTags);
        }
        else
            rgTicketTags.Visible = false;
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketTags_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DesktopShared.TypedListClasses.TicketTagsRow _tag =
                (DesktopShared.TypedListClasses.TicketTagsRow)
                ((DataRowView)e.Item.DataItem).Row;

            #region tag name

            TableCell tcName = item["Name"];

            string publicClient = "";
            if (_tag.Public)
                publicClient = " (P";

            if (_tag.ClientPublic)
            {
                if (publicClient.Length > 0)
                    publicClient += "C)";
                else
                    publicClient = " (C)";
            }
            else
            {
                if (publicClient.Length > 0)
                    publicClient += ")";
            }

            tcName.Text = _tag.Name.Trim() + publicClient;

            #endregion

        }
    }

    /// <summary>
    /// grid on item created
    /// </summary>
    /// <param name="sneder"></param>
    /// <param name="e"></param>
    protected void rgTicketTags_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
         #region Item or AlternatingItem

        //action link buttons needs to be added for both ItemCreated & ItemDataBound to raise postback events
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem item = e.Item as GridDataItem;

            #region action column

            TableCell tcAction = item["Action"];
            tcAction.Controls.Clear();

            #region delete tag link

            LinkButton lbDelete = new LinkButton();
            lbDelete.Text = "Delete";
            lbDelete.ID = "lbDelete";
            lbDelete.CommandName = "Delete";
            lbDelete.Style.Add("color", "red");

            tcAction.Controls.Add(lbDelete);

            #endregion

            #endregion
        }

        #endregion
    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketTags_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        #region delete

        if (e.CommandName == "Delete")
        {
            int taggedTicketValueId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues
                 [e.Item.ItemIndex]["TaggedTicketValueId"].ToString());

            DesktopShared.Ticket.Tagged.DeleteTagValue(taggedTicketValueId);
        }

        #endregion
    }

    #endregion

    /// <summary>
    /// add tag on button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddTag_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //user id (public tags are not assigned to user)
            int? userId = TagIsPublic ? (int?)null: DesktopShared.User.UserID;

            //strip out (P) or (PC)
            string _tagValue = TagValue.Replace("(P)", "").Replace("(PC)", "").Trim();

            //add tag
            DesktopShared.Ticket.Tagged.AddTagValue(TicketId, _tagValue, userId, TagIsPublic, TagIsClientPublic);

            //clear selection
            tbTagText.Text = "";
            chkTagPublic.Checked = false;
            chkTagClientPublic.Checked = false;
            ucUserTicketTagsDDL.TagName = "";

            //rebind grd
            rgTicketTags.DataSource = null;
            rgTicketTags.Rebind();
        }
    }

    #region custom validators

    /// <summary>
    /// server validation for add new tag
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cvAddTag_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        cvAddTag.ErrorMessage = "Tag already exists for ticket";

        //client public must also be public
        if (chkTagClientPublic.Checked && !chkTagPublic.Checked)
        {
            cvAddTag.ErrorMessage = "Client Public tags must also be flagged as Public";
            args.IsValid = false;
            return;
        }

        //characters "|", "(P)", "(PC)" not allowed
        string _tagText = tbTagText.Text.Trim();
        if (_tagText.Length > 0)
        {
            if (_tagText.Contains("|") || _tagText.Contains("(P)") || _tagText.Contains("(PC)"))
            {
                cvAddTag.ErrorMessage = "Tags may not contain \"|\", \"(P)\", or \"(PC)\"";
                args.IsValid = false;
                return;
            }
        }

        //check tag is unique
        int? userId = chkTagPublic.Checked ? (int?) null: DesktopShared.User.UserID;
        args.IsValid = !DesktopShared.Ticket.Tagged.TagExistsForTicket(TicketId, TagValue, userId);
    }

    #endregion

    #endregion

    #region private methods

    /// <summary>
    /// populate user tags drop down list used for add tag
    /// hide 
    /// </summary>
    /// <param name="ticketTags"></param>
    private void PopulateUserTags(DesktopShared.TypedListClasses.TicketTagsTypedList ticketTags)
    {
        ucUserTicketTagsDDL.UserId = DesktopShared.User.UserID;
        ucUserTicketTagsDDL.DefaultText = _defaultTextAddTag;
        ucUserTicketTagsDDL.Populate();

        //get drop down list user control
        DropDownList ddlUserTicketTags = ucUserTicketTagsDDL.GetDropDownList();

        //iterate through typed list and remove item from drop down list if item is already tagged
        foreach (DesktopShared.TypedListClasses.TicketTagsRow row in ticketTags)
        {
            #region append public / client public for drop down list value

            //drop down value = [name]|public|clientpublic
            //"Desktop|1|0"

            //name
            string _ddlValue = row.Name.Trim() + "|";

            //public
            if (row.Public)
                _ddlValue += "1|";
            else
                _ddlValue += "0|";

            //client public
            if (row.ClientPublic)
                _ddlValue += "1";
            else
                _ddlValue += "0";

            #endregion

            ListItem li = ddlUserTicketTags.Items.FindByValue(_ddlValue);
            if (li != null)
                ddlUserTicketTags.Items.Remove(li);
        }

        //hide drop down list if no options available
        ucUserTicketTagsDDL.Visible = ddlUserTicketTags.Items.Count > 1; //default entry would be item # 1
        phOrText.Visible = ddlUserTicketTags.Visible;

        //css class for check boxes
        chkTagPublic.InputAttributes.Add("class", "checkbox");
        chkTagClientPublic.InputAttributes.Add("class", "checkbox");
    }

    #endregion

    #region private properties

    /// <summary>
    /// get tag value from either text box or drop down list
    /// </summary>
    private string TagValue
    {
        get
        {
            //text box
            string tagValue = tbTagText.Text.Trim();

            if (tagValue.Trim().Length == 0)
            {
                //drop down list
                if (ucUserTicketTagsDDL.TagName != _defaultTextAddTag)
                    tagValue = ucUserTicketTagsDDL.TagName.Trim();
            }

            //default text if no entry in text box or ddl
            if (tagValue.Trim().Length == 0)
                tagValue = DesktopShared.Ticket.Tagged.DefaultTagText;

            return tagValue;
        }

    }

    /// <summary>
    /// get/set tag is public 
    /// </summary>
    public bool TagIsPublic
    {
        get
        {
            if (tbTagText.Text.Trim().Length > 0) //new value via text box
                return chkTagPublic.Checked;
            else //value via drop down list
            {
                if (ucUserTicketTagsDDL.TagName != _defaultTextAddTag)
                    return ucUserTicketTagsDDL.IsPublic;
                else
                    return false;
            }
        }
        set { chkTagPublic.Checked = value; }
    }

    /// <summary>
    /// get/set tag is client public
    /// </summary>
    public bool TagIsClientPublic
    {
        get
        {
            if (tbTagText.Text.Trim().Length > 0) //new value via text box
                return chkTagClientPublic.Checked;
            else //value via drop down list
            {
                if (ucUserTicketTagsDDL.TagName != _defaultTextAddTag)
                    return ucUserTicketTagsDDL.IsClientPublic;
                else
                    return false;
            }
        
        }
        set { chkTagClientPublic.Checked = value; }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set ticket
    /// set rebinds grid
    /// </summary>
    public int TicketId
    {
        get
        {
            object obj = this.ViewState["TicketId"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["TicketId"] = value;
            rgTicketTags.DataSource = null;
            rgTicketTags.Rebind();
        }
    }

    #endregion
}