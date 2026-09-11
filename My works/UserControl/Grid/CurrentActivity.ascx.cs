using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Telerik.Web.UI;
using System.Data;
using System.Collections;

public partial class UserControl_Grid_CurrentActivity : System.Web.UI.UserControl
{
    private bool _isExport = false;
    
    private string _enteredActionType = "Entered";
    private string _updatedActionType = "Updated";
    private string _highlightColor = "#FEE5AC";

    private string _activityEnteredActionType = "Active Ticket Entered";
    private string _activityUpdatedExternalActionType = "Active Ticket External Updated";
    private string _activityUpdatedInternalActionType = "Active Ticket Interal Updated";

    protected void Page_Load(object sender, EventArgs e)
    {
        //ajax manager, set conditional postback script to cancel ajax on grid export
        Telerik.Web.UI.RadAjaxManager.GetCurrent(this.Page).ClientEvents.OnRequestStart = "conditionalPostback";

        rgCurrentActivity.HierarchySettings.ExpandTooltip= "Click to view Scheduled/Active/Updated Tickets for today.";
        rgCurrentActivity.HierarchySettings.CollapseTooltip = "Click to hide Scheduled/Active/Updated Tickets for today.";

    }

    #region protected methods / events

    #region telerik grid events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgCurrentActivity_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        #region sort

        if (!e.IsFromDetailTable)
        {
            string _fieldName = DesktopShared.User.Activity.Current.GridSort.GetFieldName().Trim(); //sort field name stored in cookie
            if ((_fieldName.Length > 0) && (!this.Page.IsPostBack))
            {
                //sort operator
                Telerik.Web.UI.GridSortOrder _gridSortOrder = GridSortOrder.Ascending;
                string _enum = DesktopShared.User.Activity.Current.GridSort.GetOperator();
                if (_enum.Length > 0)
                {
                    try { _gridSortOrder = (Telerik.Web.UI.GridSortOrder)Enum.Parse(typeof(Telerik.Web.UI.GridSortOrder), _enum, true); }
                    catch { _gridSortOrder = GridSortOrder.Ascending; }
                }

                GridSortExpression sortExperssion = new GridSortExpression();
                sortExperssion.FieldName = _fieldName;
                sortExperssion.SortOrder = _gridSortOrder;
                rgCurrentActivity.MasterTableView.SortExpressions.AddSortExpression(sortExperssion);
            }

            if (rgCurrentActivity.MasterTableView.SortExpressions.Count == 0)
            {
                GridSortExpression sortExperssion = new GridSortExpression();
                sortExperssion.FieldName = "UserFirst";
                sortExperssion.SortOrder = GridSortOrder.Ascending;
                rgCurrentActivity.MasterTableView.SortExpressions.AddSortExpression(sortExperssion);
            }
        }
        #endregion

        //bind
        rgCurrentActivity.MasterTableView.HierarchyLoadMode = GridChildLoadMode.ServerBind;
        rgCurrentActivity.DataSource = DesktopShared.Activity.GetCurrent(SearchUserStatusIds, SearchTeamId, SearchMsp);
    }

    /// <summary>
    /// grid on item created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgCurrentActivity_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region Item or AlternatingItem


        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            int _ticketId = 0;

            #region parent table for users

            if (e.Item.OwnerTableView.Name == "CurrentActivity")
            {
                #region data keys

                _ticketId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"], -1);

                string _activeTicketStart = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserActiveTicketStart"].ToString().Trim();

                string _userStartTimeString = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserStartTime"].ToString().Trim();
                string _userLastCheckInStartTimeString = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserLastCheckInStartTime"].ToString().Trim();
                string _userEndTimeString = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserEndTime"].ToString().Trim();

                string _enteredDate = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketDateEntered"].ToString().Trim();
                string _enteredDescription = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketDescription"].ToString().Trim();
                string _enteredByFirst = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketEnteredByFirstName"].ToString().Trim();
                string _enteredByLast = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketEnteredByLastName"].ToString().Trim();

                string _updatedExternalDate = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketHistoryCreated"].ToString().Trim();
                string _updatedExternalDescription = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketHistoryNotes"].ToString().Trim();
                string _updatedExternalByFirst = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketUpdatedByFirstName"].ToString().Trim();
                string _updatedExternalByLast = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketUpdatedByLastName"].ToString().Trim();

                string _updatedInternalDate = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["InternalTicketHistoryCreated"].ToString().Trim();
                string _updatedInternalDescription = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["InternalTicketHistoryNotes"].ToString().Trim();
                string _updatedInternalByFirst = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["InternalTicketUpdatedByFirstName"].ToString().Trim();
                string _updatedInternalByLast = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["InternalTicketUpdatedByLastName"].ToString().Trim();

                #endregion

                string _timeAgo = "";

                //user start time
                DateTime _userStartTime = DateTime.MinValue;
                DateTime.TryParse(_userStartTimeString, out _userStartTime);
                if (_userStartTime != DateTime.MinValue)
                {
                    _timeAgo = DesktopShared.Utility.Date.GetTimeDisplaySinceLastEvent(_userStartTime);
                    TelerikHelper.AddLabelToCell(
                        String.Format("{0} ({1} ago)", _userStartTime.ToString("HH:mm"), _timeAgo),
                        item["UserStartTime"]);
                }

                //user last check in
                DateTime _userLastCheckInStartTime = DateTime.MinValue;
                DateTime.TryParse(_userLastCheckInStartTimeString, out _userLastCheckInStartTime);
                if (_userLastCheckInStartTime != DateTime.MinValue)
                {
                    _timeAgo = DesktopShared.Utility.Date.GetTimeDisplaySinceLastEvent(_userLastCheckInStartTime);
                    TelerikHelper.AddLabelToCell(
                        String.Format("{0} ({1} ago)", _userLastCheckInStartTime.ToString("HH:mm"), _timeAgo),
                        item["UserLastCheckInStartTime"]);
                }

                //user end time
                DateTime _userEndTime = DateTime.MinValue;
                DateTime.TryParse(_userEndTimeString, out _userEndTime);
                if (_userEndTime != DateTime.MinValue)
                {
                    _timeAgo = DesktopShared.Utility.Date.GetTimeDisplaySinceLastEvent(_userEndTime);
                    TelerikHelper.AddLabelToCell(
                        String.Format("{0} ({1} ago)", _userEndTime.ToString("HH:mm"), _timeAgo),
                        item["UserEndTime"]);
                }

                //active ticket start
                DateTime _ticketStart = DateTime.MinValue;
                DateTime.TryParse(_activeTicketStart, out _ticketStart);
                if (_ticketStart != DateTime.MinValue)
                {
                    _timeAgo = DesktopShared.Utility.Date.GetTimeDisplaySinceLastEvent(_ticketStart);
                    TelerikHelper.AddLabelToCell(
                        String.Format("{0} ({1} ago)", _ticketStart.ToString("HH:mm"), _timeAgo),
                        item["UserActiveTicketStart"]);

                }

                #region ticket entered

                DateTime _actionDate = DateTime.MinValue;
                DateTime.TryParse(_enteredDate, out _actionDate);
                if (_actionDate != DateTime.MinValue)
                {
                    TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                        _ticketId, //ticket id
                        _actionDate, //ticket entered date
                        _enteredDescription, //ticket entered description
                        _enteredByFirst, //entered by first name
                        _enteredByLast, //entered by last name
                        _enteredActionType, //action type
                        _activityEnteredActionType, //label name
                        true, //display time ago
                        item["TicketDateEntered"] //telerik cell
                    );
                }
                else
                    item["TicketDateEntered"].Text = "";

                #endregion

                #region ticket last updated external

                DateTime _externalUpdate = DateTime.MinValue;
                DateTime.TryParse(_updatedExternalDate, out _externalUpdate);
                if (_externalUpdate != DateTime.MinValue)
                {
                    TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                    _ticketId, //ticket id
                    _externalUpdate, //ticket updated date
                    _updatedExternalDescription, //ticket updated description
                    _updatedExternalByFirst,  //updated by first name
                    _updatedExternalByLast, //updated by last name
                    _updatedActionType, //action type
                    _activityUpdatedExternalActionType, //label name
                    true, //display time ago
                    item["TicketHistoryCreated"] //telerik cell
                    );
                }
                else
                    item["TicketHistoryCreated"].Text = "";

                #endregion

                #region ticket last updated internal

                DateTime _internalUpdate = DateTime.MinValue;
                DateTime.TryParse(_updatedInternalDate, out _internalUpdate);
                if (_internalUpdate != DateTime.MinValue)
                {
                    TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                    _ticketId, //ticket id
                    _internalUpdate, //ticket updated date
                    _updatedInternalDescription, //ticket updated description
                    _updatedInternalByFirst,  //updated by first name
                    _updatedInternalByLast, //updated by last name
                    _updatedActionType, //action type
                    _activityUpdatedInternalActionType, //label name
                    true, //display time ago
                    item["InternalTicketHistoryCreated"] //telerik cell
                    );
                }
                else
                    item["InternalTicketHistoryCreated"].Text = "";

                #endregion

                #region highlight active ticket start time / ticket updated cells based on last updated warning

                if ((UpdatedWarningsHours.HasValue) && (_ticketId > 0))
                {
                    DateTime _now = DateTime.Now;
                    int _minutesWarning = UpdatedWarningsHours.Value * 60;

                    TimeSpan tsActiveTicketStart = _now.Subtract(_ticketStart);
                    int _ticketStartMinutesAgo = (int)tsActiveTicketStart.TotalMinutes;

                    TimeSpan tsExternalUpdate = _now.Subtract(_externalUpdate);
                    int _externalUpdateMinutes = (int)tsExternalUpdate.TotalMinutes;

                    TimeSpan tsInternalUpdate = _now.Subtract(_internalUpdate);
                    int _internalUpdateMinutes = (int)tsInternalUpdate.TotalMinutes;

                    if ((_ticketStartMinutesAgo > _minutesWarning) && (_externalUpdateMinutes > _minutesWarning) && (_internalUpdateMinutes > _minutesWarning))
                    {
                        item["UserActiveTicketStart"].Attributes.CssStyle.Add("background-color", _highlightColor);
                        item["TicketHistoryCreated"].Attributes.CssStyle.Add("background-color", _highlightColor);
                        item["InternalTicketHistoryCreated"].Attributes.CssStyle.Add("background-color", _highlightColor);
                    }
                }

                #endregion
            }

            #endregion

            #region scheduled tickets detail table / active tickets detail table / updated tickets detail table

            else if ((e.Item.OwnerTableView.Name == "ScheduledTicket") || (e.Item.OwnerTableView.Name == "ActiveTicket") || (e.Item.OwnerTableView.Name == "UpdatedTicket"))
            {
                #region data keys

                _ticketId = -1;

                string _summary = "";

                string _scheduledDateString = "";
                string _timeSpentString = "";

                string _enteredDate = "";
                string _enteredDescription = "";
                string _enteredByFirst = "";
                string _enteredByLast = "";

                string _updatedDate = "";
                string _updatedDescription = "";
                string _updatedByFirst = "";
                string _updatedByLast = "";

                string _activeTicketStartTime = "";
                string _activeTicketEndTime = "";

                if (e.Item.OwnerTableView.Name == "ScheduledTicket")
                {
                    _ticketId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"], -1);

                    _summary = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Summary"].ToString().Trim();

                    _scheduledDateString = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ScheduledDate"].ToString().Trim();
                    _timeSpentString = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TimeSpent"].ToString().Trim();

                    _enteredDate = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DateEntered"].ToString().Trim();
                    _enteredDescription = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Description"].ToString().Trim();
                    _enteredByFirst = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EnteredByFirstName"].ToString().Trim();
                    _enteredByLast = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EnteredByLastName"].ToString().Trim();

                    _updatedDate = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["LastUpdated"].ToString().Trim();
                    _updatedDescription = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["LastUpdatedNotes"].ToString().Trim();
                    _updatedByFirst = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UpdatedByFirstName"].ToString().Trim();
                    _updatedByLast = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UpdatedByLastName"].ToString().Trim();
                }
                else if ((e.Item.OwnerTableView.Name == "ActiveTicket") || (e.Item.OwnerTableView.Name == "UpdatedTicket"))
                {
                    _ticketId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketId"], -1);

                    _summary = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketSummary"].ToString().Trim();

                    _scheduledDateString = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketScheduledDate"].ToString().Trim();
                    _timeSpentString = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TimeSpent"].ToString().Trim();

                    _enteredDate = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketDateEntered"].ToString().Trim();
                    _enteredDescription = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketDescription"].ToString().Trim();
                    _enteredByFirst = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketEnteredByFirstName"].ToString().Trim();
                    _enteredByLast = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketEnteredByLastName"].ToString().Trim();

                    if (e.Item.OwnerTableView.Name == "UpdatedTicket")
                    {
                        _updatedDate = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HistoryCreated"].ToString().Trim();
                        _updatedDescription = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HistoryNotes"].ToString().Trim();
                        _updatedByFirst = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketUpdatedByFirstName"].ToString().Trim();
                        _updatedByLast = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TicketUpdatedByLastName"].ToString().Trim();
                    }
                    else if (e.Item.OwnerTableView.Name == "ActiveTicket")
                    {
                        //active ticket start
                        DateTime _ticketStart = DateTime.MinValue;
                        DateTime.TryParse((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ActiveTicketStartTime"].ToString().Trim(), out _ticketStart);
                        if (_ticketStart != DateTime.MinValue)
                            _activeTicketStartTime = _ticketStart.ToString("HH:mm");

                        //active ticket end
                        DateTime _ticketEnd = DateTime.MinValue;
                        DateTime.TryParse((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ActiveTicketEndTime"].ToString().Trim(), out _ticketEnd);
                        if (_ticketEnd != DateTime.MinValue)
                            _activeTicketEndTime = _ticketEnd.ToString("HH:mm");
                    }
                }

                #endregion

                //ticket summary link
                TelerikHelper.AddHyperLinkToCell(
                    _summary,
                    String.Format("/Ticket/Detail2.aspx?Id={0}", _ticketId),
                    item["Summary"],
                    "_blank");

                //scheduled time
                DateTime _scheduledDate = DateTime.MinValue;
                DateTime.TryParse(_scheduledDateString, out _scheduledDate);
                if (_scheduledDate != DateTime.MinValue)
                {
                    string _timeAgo = DesktopShared.Utility.Date.GetTimeDisplaySinceLastEvent(_scheduledDate);
                    string _timeAgoDescription = _scheduledDate <= DateTime.Now ? "ago" : "from now";
                    _timeAgo = _timeAgo.Replace("-", "");

                    TelerikHelper.AddLabelToCell(
                        String.Format("{0} ({1} {2})", _scheduledDate.ToString("HH:mm"), _timeAgo, _timeAgoDescription),
                        item["ScheduledDate"]);
                }

                //time spent on ticket
                double _ticketTimeSpent = 0;
                double.TryParse(_timeSpentString, out _ticketTimeSpent);
                if (_ticketTimeSpent == 0) //label
                    TelerikHelper.AddLabelToCell(_timeSpentString, item["TimeSpent"]);
                else //hyperlink
                {
                    TelerikHelper.AddHyperLinkToCell(
                        _timeSpentString,
                        String.Format("/Timesheet/Default.aspx?TicketId={0}", _ticketId),
                         item["TimeSpent"],
                        "_blank");
                }

                #region entered

                DateTime _actionDate = DateTime.MinValue;
                DateTime.TryParse(_enteredDate, out _actionDate);
                if (_actionDate != DateTime.MinValue)
                {
                    TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                        _ticketId, //ticket id
                        _actionDate, //ticket entered date
                        _enteredDescription, //ticket entered description
                        _enteredByFirst, //entered by first name
                        _enteredByLast, //entered by last name
                        _enteredActionType, //action type
                        true, //display time ago
                        item["DateEntered"]); //telerik cell
                }
                else
                    item["DateEntered"].Text = "";

                #endregion

                #region updated

                if ((e.Item.OwnerTableView.Name == "ScheduledTicket") || (e.Item.OwnerTableView.Name == "UpdatedTicket"))
                {
                    _actionDate = DateTime.MinValue;
                    DateTime.TryParse(_updatedDate, out _actionDate);
                    if (_actionDate != DateTime.MinValue)
                    {
                        TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                            _ticketId, //ticket id
                            _actionDate, //ticket updated date
                            _updatedDescription, //ticket updated description
                            _updatedByFirst, //updated by first name
                            _updatedByLast, //updated by last name
                            _updatedActionType, //action type
                            true, //display time ago
                            item["LastUpdated"]); //telerik cell

                    }
                    else
                        item["LastUpdated"].Text = "";
                }

                #endregion

                #region start time - end time

                else if (e.Item.OwnerTableView.Name == "ActiveTicket")
                {
                    if ((_activeTicketStartTime.Trim().Length > 0) || (_activeTicketEndTime.Trim().Length > 0))
                    {
                        System.Text.StringBuilder sbTime = new System.Text.StringBuilder();
                        if (_activeTicketStartTime.Trim().Length > 0)
                            sbTime.Append(_activeTicketStartTime);

                        if (_activeTicketEndTime.Trim().Length > 0)
                            sbTime.Append(String.Format("{0}{1}", _activeTicketStartTime.Trim().Length > 0 ? " - " : "", _activeTicketEndTime));

                        TelerikHelper.AddLabelToCell(sbTime.ToString(), item["StartTime"]);
                    }
                    else
                        item["StartTime"].Text = "";
                }

                #endregion

            }

            #endregion

        }

        #endregion

    }

    /// <summary>
    /// grid on item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgCurrentActivity_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            int _ticketId = 0;
            string _timeAgo = "";

            #region parent table for users

            if (e.Item.OwnerTableView.Name == "CurrentActivity")
            {
                //ticket id
                int.TryParse(drv["TicketId"].ToString(), out _ticketId);

                //employee name 
                string _employeeFullName = String.Format("{0} {1}", drv["UserFirst"].ToString().Trim(), drv["UserLast"].ToString().Trim());
                TelerikHelper.AddLabelToCell(_employeeFullName, item["UserFirst"]);

                //user start time
                DateTime _userStartTime = DateTime.MinValue;
                DateTime.TryParse(drv["UserStartTime"].ToString(), out _userStartTime);
                if (_userStartTime != DateTime.MinValue)
                {
                    _timeAgo = DesktopShared.Utility.Date.GetTimeDisplaySinceLastEvent(_userStartTime);
                    TelerikHelper.AddLabelToCell(
                        String.Format("{0} ({1} ago)", _userStartTime.ToString("HH:mm"), _timeAgo),
                        item["UserStartTime"]);
                }

                //user last check in
                DateTime _userLastCheckInStartTime = DateTime.MinValue;
                DateTime.TryParse(drv["UserLastCheckInStartTime"].ToString(), out _userLastCheckInStartTime);
                if (_userLastCheckInStartTime != DateTime.MinValue)
                {
                    _timeAgo = DesktopShared.Utility.Date.GetTimeDisplaySinceLastEvent(_userLastCheckInStartTime);
                    TelerikHelper.AddLabelToCell(
                        String.Format("{0} ({1} ago)", _userLastCheckInStartTime.ToString("HH:mm"), _timeAgo),
                        item["UserLastCheckInStartTime"]);
                }

                //user end time
                DateTime _userEndTime = DateTime.MinValue;
                DateTime.TryParse(drv["UserEndTime"].ToString(), out _userEndTime);
                if (_userEndTime != DateTime.MinValue)
                {
                    _timeAgo = DesktopShared.Utility.Date.GetTimeDisplaySinceLastEvent(_userEndTime);
                    TelerikHelper.AddLabelToCell(
                        String.Format("{0} ({1} ago)", _userEndTime.ToString("HH:mm"), _timeAgo),
                        item["UserEndTime"]);
                }

                #region employee notes

                string _employeeNotes = drv["UserActivityNotes"].ToString().Trim();

                //table cell
                TableCell tcUserActivityNotes = item["UserActivityNotes"];
                tcUserActivityNotes.Controls.Clear();

                //label for notes and tooltip
                Label lblEmployeeNotes = new Label();
                lblEmployeeNotes.Text = _employeeNotes;
                lblEmployeeNotes.ID = "lblEmployeeNotes_" + _ticketId;
                tcUserActivityNotes.Controls.Add(lblEmployeeNotes);

                //activity notes tooltip
                string _userActivityNotes = drv["UserActivityNotes"].ToString().Trim();
                Telerik.Web.UI.RadToolTip _toolTipUserActivityNotes = new RadToolTip();
                _toolTipUserActivityNotes.Text = _userActivityNotes.Replace("\n", "<br />");
                _toolTipUserActivityNotes.TargetControlID = lblEmployeeNotes.ID.ToString();
                _toolTipUserActivityNotes.AutoCloseDelay = 5000;
                _toolTipUserActivityNotes.Width = Unit.Pixel(400);
                _toolTipUserActivityNotes.Skin = "BitByBit";
                _toolTipUserActivityNotes.EnableEmbeddedSkins = false;
                _toolTipUserActivityNotes.EnableEmbeddedBaseStylesheet = false;
                _toolTipUserActivityNotes.Title = _employeeFullName + " Notes";
                tcUserActivityNotes.Controls.Add(_toolTipUserActivityNotes);

                #endregion

                #region employee roles

                string _employeeRoles = "";

                string _roleOne = drv["UserRoleOneName"].ToString().Trim();
                if (_roleOne.Trim().Length > 0)
                    _employeeRoles += String.Format("{0}", _roleOne.Trim());

                string _roleTwo = drv["UserRoleTwoName"].ToString().Trim();
                if (_roleTwo.Trim().Length > 0)
                {
                    _employeeRoles += String.Format("{0}{1}",
                        _employeeRoles.Trim().Length > 0 ? ", " : "",
                        _roleTwo.Trim()
                        );
                }

                string _roleThree = drv["UserRoleThreeName"].ToString().Trim();
                if (_roleThree.Trim().Length > 0)
                {
                    _employeeRoles += String.Format("{0}{1}",
                        _employeeRoles.Trim().Length > 0 ? ", " : "",
                        _roleThree.Trim()
                        );
                }

                TelerikHelper.AddLabelToCell(_employeeRoles, item["UserRoleOneName"]);

                #endregion

                //employee location
                string _employeeLocation = String.Format("{0}{1}{2}",
                    drv["UserLocationName"].ToString().Trim(),
                    drv["UserLocationText"].ToString().Trim().Length > 0 ? " - " : "",
                    drv["UserLocationText"].ToString().Trim()
                    );
                TelerikHelper.AddLabelToCell(_employeeLocation, item["UserLocationName"]);

                //employee status
                string _employeeStatus = String.Format("{0}{1}{2}",
                    drv["UserStatusName"].ToString().Trim(),
                    drv["UserStatusText"].ToString().Trim().Length > 0 ? " - " : "",
                    drv["UserStatusText"].ToString().Trim()
                    );
                TelerikHelper.AddLabelToCell(_employeeStatus, item["UserStatusName"]);

                //scheduled ticket count
                TelerikHelper.AddLabelToCell(drv["TotalScheduledTickets"].ToString(), item["TotalScheduledTickets"]);

                //active ticket count
                TelerikHelper.AddLabelToCell(drv["TotalActiveTickets"].ToString(), item["TotalActiveTickets"]);

                //update ticket count
                TelerikHelper.AddLabelToCell(drv["TotalUpdatedTickets"].ToString(), item["TotalUpdatedTickets"]);

                //ticket summary 
                if (_ticketId > 0)
                {
                    TelerikHelper.AddHyperLinkToCell(
                        String.Format("{0} - {1}", _ticketId, drv["TicketSummary"].ToString().Trim()),
                        String.Format("/Ticket/Detail2.aspx?Id={0}", _ticketId),
                        item["TicketSummary"]);
                }

                //active ticket start
                DateTime _ticketStart = DateTime.MinValue;
                DateTime.TryParse(drv["UserActiveTicketStart"].ToString(), out _ticketStart);
                if (_ticketStart != DateTime.MinValue)
                {
                    _timeAgo = DesktopShared.Utility.Date.GetTimeDisplaySinceLastEvent(_ticketStart);
                    TelerikHelper.AddLabelToCell(
                        String.Format("{0} ({1} ago)", _ticketStart.ToString("HH:mm"), _timeAgo), 
                        item["UserActiveTicketStart"]);
                }

                //ticket estimate
                TelerikHelper.AddLabelToCell(drv["TicketEstimate"].ToString(), item["TicketEstimate"]);

                //ticket client name
                TelerikHelper.AddLabelToCell(drv["ClientCompany"].ToString().Trim(), item["ClientCompany"]);

                #region ticket entered

                DateTime _enteredDate = DateTime.MinValue;
                DateTime.TryParse(drv["TicketDateEntered"].ToString(), out _enteredDate);
                if (_enteredDate != DateTime.MinValue)
                {
                    TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                    _ticketId, //ticket id
                    _enteredDate, //ticket entered date
                    drv["TicketDescription"].ToString().Trim(), //ticket entered description
                    drv["TicketEnteredByFirstName"].ToString().Trim(),  //entered by first name
                    drv["TicketEnteredByLastName"].ToString().Trim(), //entered by last name
                    _enteredActionType, //action type
                    _activityEnteredActionType,  //label name
                    true, //display time ago
                    item["TicketDateEntered"] //telerik cell
                    );
                }
                else
                    item["TicketDateEntered"].Text = "";

                #endregion

                #region ticket last updated external

                DateTime _externalUpdate = DateTime.MinValue;
                DateTime.TryParse(drv["TicketHistoryCreated"].ToString(), out _externalUpdate);
                if (_externalUpdate != DateTime.MinValue)
                {
                    TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                    _ticketId, //ticket id
                    _externalUpdate, //ticket updated date
                    drv["TicketHistoryNotes"].ToString().Trim(), //ticket updated description
                    drv["TicketUpdatedByFirstName"].ToString().Trim(),  //updated by first name
                    drv["TicketUpdatedByLastName"].ToString().Trim(), //updated by last name
                    _updatedActionType, //action type
                    _activityUpdatedExternalActionType, //label name
                    true, //display time ago
                    item["TicketHistoryCreated"] //telerik cell
                    );
                }
                else
                    item["TicketHistoryCreated"].Text = "";

                #endregion

                #region ticket last updated internal

                DateTime _internalUpdate = DateTime.MinValue;
                DateTime.TryParse(drv["InternalTicketHistoryCreated"].ToString(), out _internalUpdate);
                if (_internalUpdate != DateTime.MinValue)
                {
                    TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                    _ticketId, //ticket id
                    _internalUpdate, //ticket updated date
                    drv["InternalTicketHistoryNotes"].ToString().Trim(), //ticket updated description
                    drv["InternalTicketUpdatedByFirstName"].ToString().Trim(),  //updated by first name
                    drv["InternalTicketUpdatedByLastName"].ToString().Trim(), //updated by last name
                    _updatedActionType, //action type
                    _activityUpdatedInternalActionType, //label name
                    true, //display time ago
                    item["InternalTicketHistoryCreated"] //telerik cell
                    );
                }
                else
                    item["InternalTicketHistoryCreated"].Text = "";

                #endregion

                #region highlight active ticket start time / ticket updated cells based on last updated warning

                if ((UpdatedWarningsHours.HasValue) && (_ticketId > 0) && (_ticketStart != DateTime.MinValue))
                {
                    DateTime _now = DateTime.Now;
                    int _minutesWarning = UpdatedWarningsHours.Value * 60;

                    TimeSpan tsActiveTicketStart = _now.Subtract(_ticketStart);
                    int _ticketStartMinutesAgo = (int)tsActiveTicketStart.TotalMinutes;

                    TimeSpan tsExternalUpdate = _now.Subtract(_externalUpdate);
                    int _externalUpdateMinutes = (int)tsExternalUpdate.TotalMinutes;

                    TimeSpan tsInternalUpdate = _now.Subtract(_internalUpdate);
                    int _internalUpdateMinutes = (int)tsInternalUpdate.TotalMinutes;

                    if ((_ticketStartMinutesAgo > _minutesWarning) && (_externalUpdateMinutes > _minutesWarning) && (_internalUpdateMinutes > _minutesWarning))
                    {
                        item["UserActiveTicketStart"].Attributes.CssStyle.Add("background-color", _highlightColor);
                        item["TicketHistoryCreated"].Attributes.CssStyle.Add("background-color", _highlightColor);
                        item["InternalTicketHistoryCreated"].Attributes.CssStyle.Add("background-color", _highlightColor);

                    }
                }

                #endregion

            }

            #endregion

            #region scheduled tickets detail table / active tickets detail table / updated tickets detail table

            else if ((e.Item.OwnerTableView.Name == "ScheduledTicket") || (e.Item.OwnerTableView.Name == "ActiveTicket") || (e.Item.OwnerTableView.Name == "UpdatedTicket"))
            {
                string _summary = "";

                string _companyName = drv["CompanyName"].ToString();
                string _dispositionName = drv["DispositionName"].ToString();
                string _priorityName = drv["PriorityName"].ToString();
                string _estimate = "";

                string _scheduledDateString = "";
                string _timeSpentString = drv["TimeSpent"].ToString();

                string _enteredDate = "";
                string _enteredDescription = "";
                string _enteredByFirst = "";
                string _enteredByLast = "";

                string _updatedDate = "";
                string _updatedDescription = "";
                string _updatedByFirst = "";
                string _updatedByLast = "";

                string _activeTicketStartTime = "";
                string _activeTicketEndTime = "";

                if (e.Item.OwnerTableView.Name == "ScheduledTicket")
                {
                    _ticketId = Int32.Parse(drv["Id"].ToString());
                    _summary = drv["Summary"].ToString();

                    _scheduledDateString = drv["ScheduledDate"].ToString();
                    _estimate = drv["HoursToFix"].ToString();

                    _enteredDate = drv["DateEntered"].ToString();
                    _enteredDescription = drv["Description"].ToString().Trim();
                    _enteredByFirst = drv["EnteredByFirstName"].ToString().Trim();
                    _enteredByLast = drv["EnteredByLastName"].ToString().Trim();

                    _updatedDate = drv["LastUpdated"].ToString();
                    _updatedDescription = drv["LastUpdatedNotes"].ToString().Trim();
                    _updatedByFirst = drv["UpdatedByFirstName"].ToString().Trim();
                    _updatedByLast = drv["UpdatedByLastName"].ToString().Trim();
                }
                else if ((e.Item.OwnerTableView.Name == "ActiveTicket") || (e.Item.OwnerTableView.Name == "UpdatedTicket"))
                {
                    _ticketId = Int32.Parse(drv["TicketId"].ToString());
                    _summary = drv["TicketSummary"].ToString();

                    _scheduledDateString = drv["TicketScheduledDate"].ToString();
                    _estimate = drv["TicketHoursToFix"].ToString();

                    _enteredDate = drv["TicketDateEntered"].ToString();
                    _enteredDescription = drv["TicketDescription"].ToString().Trim();
                    _enteredByFirst = drv["TicketEnteredByFirstName"].ToString().Trim();
                    _enteredByLast = drv["TicketEnteredByLastName"].ToString().Trim();

                    if (e.Item.OwnerTableView.Name == "UpdatedTicket")
                    {
                        _updatedDate = drv["HistoryCreated"].ToString();
                        _updatedDescription = drv["HistoryNotes"].ToString().Trim();
                        _updatedByFirst = drv["TicketUpdatedByFirstName"].ToString().Trim();
                        _updatedByLast = drv["TicketUpdatedByLastName"].ToString().Trim();
                    }
                    else if (e.Item.OwnerTableView.Name == "ActiveTicket")
                    {
                        //active ticket start
                        DateTime _ticketStart = DateTime.MinValue;
                        DateTime.TryParse(drv["ActiveTicketStartTime"].ToString(), out _ticketStart);
                        if (_ticketStart != DateTime.MinValue)
                            _activeTicketStartTime = _ticketStart.ToString("HH:mm");

                        //active ticket end
                        DateTime _ticketEnd = DateTime.MinValue;
                        DateTime.TryParse(drv["ActiveTicketEndTime"].ToString(), out _ticketEnd);
                        if (_ticketEnd != DateTime.MinValue)
                            _activeTicketEndTime = _ticketEnd.ToString("HH:mm");
                        
                    }
                }

                //ticket number
                TelerikHelper.AddLabelToCell(_ticketId.ToString(), item["Id"]);

                //ticket summary link
                TelerikHelper.AddHyperLinkToCell(
                    _summary,
                    String.Format("/Ticket/Detail2.aspx?Id={0}", _ticketId),
                     item["Summary"],
                    "_blank");

                //company name
                TelerikHelper.AddLabelToCell(_companyName, item["CompanyName"]);

                //status
                TelerikHelper.AddLabelToCell(_dispositionName, item["DispositionName"]);

                //scheduled
                DateTime _scheduledDate = DateTime.MinValue;
                DateTime.TryParse(_scheduledDateString, out _scheduledDate);
                if (_scheduledDate != DateTime.MinValue)
                {
                    _timeAgo = DesktopShared.Utility.Date.GetTimeDisplaySinceLastEvent(_scheduledDate);
                    TelerikHelper.AddLabelToCell(
                        String.Format("{0} ({1} ago)", _scheduledDate.ToString("HH:mm"), _timeAgo),
                        item["ScheduledDate"]);
                }
                
                //time spent on ticket
                double _ticketTimeSpent = 0;
                double.TryParse(_timeSpentString, out _ticketTimeSpent);
                if (_ticketTimeSpent == 0) //label
                    TelerikHelper.AddLabelToCell(_timeSpentString, item["TimeSpent"]);
                else //hyperlink
                {
                    TelerikHelper.AddHyperLinkToCell(
                        _ticketTimeSpent.ToString(),
                        String.Format("/Timesheet/Default.aspx?TicketId={0}", _ticketId),
                         item["TimeSpent"],
                        "_blank");
                }

                //priority
                TelerikHelper.AddLabelToCell(_priorityName, item["PriorityName"]);

                #region entered

                DateTime _actionDate = DateTime.MinValue;
                DateTime.TryParse(_enteredDate, out _actionDate);
                if (_actionDate != DateTime.MinValue)
                {
                    TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                    _ticketId, //ticket id
                    _actionDate, //ticket entered date
                    _enteredDescription, //ticket entered description
                    _enteredByFirst,  //entered by first name
                    _enteredByLast, //entered by last name
                    _enteredActionType, //action type
                    true, //display time ago
                    item["DateEntered"] //telerik cell
                    ); 
                }
                else
                    item["DateEntered"].Text = "";

                #endregion

                #region updated

                if ((e.Item.OwnerTableView.Name == "ScheduledTicket") || (e.Item.OwnerTableView.Name == "UpdatedTicket"))
                {
                    _actionDate = DateTime.MinValue;
                    DateTime.TryParse(_updatedDate, out _actionDate);
                    if (_actionDate != DateTime.MinValue)
                    {
                        TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                        _ticketId, //ticket id
                        _actionDate, //ticket updated date
                        _updatedDescription, //ticket updated description
                        _updatedByFirst,  //updated by first name
                        _updatedByLast, //updated by last name
                        _updatedActionType, //action type
                        true, //display time ago
                        item["LastUpdated"] //telerik cell
                        );
                    }
                    else
                        item["LastUpdated"].Text = "";
                }

                #endregion

                #region start time - end time

                else if (e.Item.OwnerTableView.Name == "ActiveTicket")
                {
                    if ((_activeTicketStartTime.Trim().Length > 0) || (_activeTicketEndTime.Trim().Length > 0))
                    {
                        System.Text.StringBuilder sbTime = new System.Text.StringBuilder();
                        if (_activeTicketStartTime.Trim().Length > 0)
                            sbTime.Append(_activeTicketStartTime);

                        if (_activeTicketEndTime.Trim().Length > 0)
                            sbTime.Append(String.Format("{0}{1}", _activeTicketStartTime.Trim().Length > 0 ? " - " : "", _activeTicketEndTime));

                        TelerikHelper.AddLabelToCell(sbTime.ToString(), item["StartTime"]);
                    }
                    else
                        item["StartTime"].Text = "";
                }

                #endregion

                //estimate
                TelerikHelper.AddLabelToCell(_estimate, item["HoursToFix"]);
            }

            #endregion

        }

        #endregion

    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgCurrentActivity_PreRender(object sender, EventArgs e)
    {
        #region default - show header, footer, pager.  set no records text

        rgCurrentActivity.ShowHeader = true;
        rgCurrentActivity.ShowFooter = false;

        rgCurrentActivity.PagerStyle.AlwaysVisible = true;
        rgCurrentActivity.PagerStyle.Visible = true;

        rgCurrentActivity.MasterTableView.NoMasterRecordsText = "No records found.";

        #endregion

        #region command item template -> populate export to type drop down list

        if (rgCurrentActivity.MasterTableView.GetItems(GridItemType.CommandItem).Length > 0)
        {
            UserControl_DropDownList_GridExportType ucGridExportType =
                rgCurrentActivity.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
                as UserControl_DropDownList_GridExportType;

            ucGridExportType.Populate();
        }

        #endregion

        #region hide expand icon if no detail records

        foreach (GridItem item in rgCurrentActivity.MasterTableView.Controls[0].Controls) 
        {
            if (item is GridNestedViewItem)
            {
                GridNestedViewItem nestedViewItem = (GridNestedViewItem)item;

                //hide expand column if no active tickets, no updated tickets, no scheduled tickets
                if ((nestedViewItem.NestedTableViews[0].Items.Count == 0) && (nestedViewItem.NestedTableViews[1].Items.Count == 0) && (nestedViewItem.NestedTableViews[2].Items.Count == 0))
                {
                    TableCell cell = nestedViewItem.NestedTableViews[0].ParentItem["ExpandColumn"];
                    cell.Controls[0].Visible = false;
                    cell.Text = " ";
                    nestedViewItem.Visible = false;
                }
            }
        }

        #endregion

        #region width of column if stored in cookie

        foreach (GridColumn col in rgCurrentActivity.MasterTableView.RenderColumns)
        {
            //user name
            if ((col.UniqueName == "UserFirst") && (DesktopShared.User.Activity.Current.ColumnWidth.UserFirst.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.UserFirst.Value);

            //user start time
            if ((col.UniqueName == "UserStartTime") && (DesktopShared.User.Activity.Current.ColumnWidth.UserStartTime.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.UserStartTime.Value);

            //user last check in
            if ((col.UniqueName == "UserLastCheckInStartTime") && (DesktopShared.User.Activity.Current.ColumnWidth.UserLastCheckInStartTime.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.UserLastCheckInStartTime.Value);

            //user end time
            if ((col.UniqueName == "UserEndTime") && (DesktopShared.User.Activity.Current.ColumnWidth.UserEndTime.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.UserEndTime.Value);

            //user activity notes
            if ((col.UniqueName == "UserActivityNotes") && (DesktopShared.User.Activity.Current.ColumnWidth.UserActivityNotes.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.UserActivityNotes.Value);

            //user roles
            if ((col.UniqueName == "UserRoleOneName") && (DesktopShared.User.Activity.Current.ColumnWidth.UserRoleOneName.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.UserRoleOneName.Value);

            //user location
            if ((col.UniqueName == "UserLocationName") && (DesktopShared.User.Activity.Current.ColumnWidth.UserLocationName.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.UserLocationName.Value);

            //user status
            if ((col.UniqueName == "UserStatusName") && (DesktopShared.User.Activity.Current.ColumnWidth.UserStatusName.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.UserStatusName.Value);

            //scheduled ticket count
            if ((col.UniqueName == "TotalScheduledTickets") && (DesktopShared.User.Activity.Current.ColumnWidth.TotalScheduledTickets.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.TotalScheduledTickets.Value);

            //active ticket count
            if ((col.UniqueName == "TotalActiveTickets") && (DesktopShared.User.Activity.Current.ColumnWidth.TotalActiveTickets.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.TotalActiveTickets.Value);

            //update ticket count
            if ((col.UniqueName == "TotalUpdatedTickets") && (DesktopShared.User.Activity.Current.ColumnWidth.TotalUpdatedTickets.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.TotalUpdatedTickets.Value);

            //ticket summary
            if ((col.UniqueName == "TicketSummary") && (DesktopShared.User.Activity.Current.ColumnWidth.TicketSummary.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.TicketSummary.Value);

            //active ticket start
            if ((col.UniqueName == "UserActiveTicketStart") && (DesktopShared.User.Activity.Current.ColumnWidth.UserActiveTicketStart.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.UserActiveTicketStart.Value);

            //ticket estimate
            if ((col.UniqueName == "TicketEstimate") && (DesktopShared.User.Activity.Current.ColumnWidth.TicketEstimate.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.TicketEstimate.Value);

            //ticket client name
            if ((col.UniqueName == "ClientCompany") && (DesktopShared.User.Activity.Current.ColumnWidth.ClientCompany.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.ClientCompany.Value);

            //ticketed entered
            if ((col.UniqueName == "TicketDateEntered") && (DesktopShared.User.Activity.Current.ColumnWidth.TicketDateEntered.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.TicketDateEntered.Value);

            //ticket updated external
            if ((col.UniqueName == "TicketHistoryCreated") && (DesktopShared.User.Activity.Current.ColumnWidth.TicketHistoryCreated.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.TicketHistoryCreated.Value);

            //ticket updated internal
            if ((col.UniqueName == "InternalTicketHistoryCreated") && (DesktopShared.User.Activity.Current.ColumnWidth.InternalTicketHistoryCreated.HasValue))
                col.HeaderStyle.Width = Unit.Pixel(DesktopShared.User.Activity.Current.ColumnWidth.InternalTicketHistoryCreated.Value);
            
        }

        #endregion
    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgCurrentActivity_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        bool doRebind = false;
        RadGrid grid = (sender as RadGrid);

        #region refresh grid click

        if (e.CommandName == "RefreshGrid")
            doRebind = true;

        #endregion

        #region do rebind

        if (doRebind)
            ResetGrid();

        #endregion
    }

    /// <summary>
    /// grid on sort command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgCurrentActivity_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        if (e.Item.OwnerTableView.Name == "CurrentActivity")
        {
            GridSortExpression sortExpression = new GridSortExpression();
            DesktopShared.User.Activity.Current.GridSort.SetFieldName(e.SortExpression);

            //these columns will sort descending on first click
            var _nonAscList = new List<string> { "TicketDateEntered", "TicketHistoryCreated", "InternalTicketHistoryCreated", "TotalScheduledTickets", "TotalActiveTickets", "TotalUpdatedTickets" };
            bool _sortAscending = !_nonAscList.Contains(e.SortExpression.Trim());

            switch (e.OldSortOrder)
            {
                case GridSortOrder.None:
                    sortExpression.FieldName = e.SortExpression;
                    sortExpression.SortOrder = _sortAscending ? GridSortOrder.Ascending : GridSortOrder.Descending;
                    e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);

                    DesktopShared.User.Activity.Current.GridSort.SetOperator(_sortAscending ? GridSortOrder.Ascending.ToString() : GridSortOrder.Descending.ToString());

                    break;
                case GridSortOrder.Ascending:

                    sortExpression.FieldName = e.SortExpression;
                    sortExpression.SortOrder = _sortAscending ? GridSortOrder.Descending : GridSortOrder.None;
                    e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);

                    DesktopShared.User.Activity.Current.GridSort.SetOperator(_sortAscending ? GridSortOrder.Descending.ToString() : GridSortOrder.None.ToString());

                    break;
                case GridSortOrder.Descending:

                    sortExpression.FieldName = e.SortExpression;
                    sortExpression.SortOrder = _sortAscending ? GridSortOrder.None : GridSortOrder.Ascending;
                    e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);

                    DesktopShared.User.Activity.Current.GridSort.SetOperator(_sortAscending ? GridSortOrder.None.ToString() : GridSortOrder.Ascending.ToString());

                    break;
            }

            e.Canceled = true;
            rgCurrentActivity.CurrentPageIndex = 0;
            rgCurrentActivity.Rebind();

            if (!e.Item.OwnerTableView.SortExpressions.ContainsExpression(e.SortExpression))
            {
                sortExpression = new GridSortExpression();
                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = GridSortOrder.Ascending;

                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
            }
        }
    }

    /// <summary>
    /// grid on detail table data bind
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgCurrentActivity_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
    {
        GridDataItem parentDataItem = (GridDataItem)e.DetailTableView.ParentItem;

        #region scheduled tickets

        if (e.DetailTableView.Name.Trim() == "ScheduledTicket")
        {
            //user id
            int userId = Convert.ToInt32(parentDataItem.GetDataKeyValue("UserId").ToString());

            //get scheduled tickets
            DataTable dtScheduledTickets = DesktopShared.Ticket.GetScheduledTicketsForUser(userId, DateTime.Now);
            
            //add column & get value for time spent on ticket
            dtScheduledTickets.Columns.Add("TimeSpent", Type.GetType("System.Double")); 
            foreach (DataRow _row in dtScheduledTickets.Rows)
            {
                int _ticketId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(_row["Id"], -1);
                _row["TimeSpent"] = DesktopShared.Timesheet.GetTimeSpentOnTicket(_ticketId);
            }
            dtScheduledTickets.AcceptChanges();

            //bind data table view
            e.DetailTableView.DataSource = dtScheduledTickets;


            //check records exists -> if not hide detail grid
            if (dtScheduledTickets.Rows.Count == 0)
                e.DetailTableView.Visible = false;
        }

        #endregion

        #region active tickets

        else if (e.DetailTableView.Name.Trim() == "ActiveTicket")
        {
            //user id
            int userId = Convert.ToInt32(parentDataItem.GetDataKeyValue("UserId").ToString());

            //get active tickets
            DataTable dtActiveTickets = DesktopShared.Activity.GetForUser(userId, null, DateTime.Now, DateTime.Now);

            //add column & get value for time spent on ticket
            dtActiveTickets.Columns.Add("TimeSpent", Type.GetType("System.Double"));
            foreach (DataRow _row in dtActiveTickets.Rows)
            {
                int _ticketId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(_row["TicketId"], -1);
                _row["TimeSpent"] = DesktopShared.Timesheet.GetTimeSpentOnTicket(_ticketId);
            }
            dtActiveTickets.AcceptChanges();

            //bind data table view
            e.DetailTableView.DataSource = dtActiveTickets;

            //check records exists -> if not hide detail grid
            if (dtActiveTickets.Rows.Count == 0)
                e.DetailTableView.Visible = false;
        }

        #endregion

        #region updated tickets 

        else if (e.DetailTableView.Name.Trim() == "UpdatedTicket")
        {   
            //user id
            int userId = Convert.ToInt32(parentDataItem.GetDataKeyValue("UserId").ToString());

            //get updated tickets
            DataTable dtUpdatedTickets = DesktopShared.Ticket.History.GetForUser(userId, null, DateTime.Now, DateTime.Now, true);

            //add column & get value for time spent on ticket
            dtUpdatedTickets.Columns.Add("TimeSpent", Type.GetType("System.Double"));
            foreach (DataRow _row in dtUpdatedTickets.Rows)
            {
                int _ticketId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(_row["TicketId"], -1);
                _row["TimeSpent"] = DesktopShared.Timesheet.GetTimeSpentOnTicket(_ticketId);
            }
            dtUpdatedTickets.AcceptChanges();

            //bind data table view
            e.DetailTableView.DataSource = dtUpdatedTickets;

            //check records exists -> if not hide detail grid
            if (dtUpdatedTickets.Rows.Count == 0)
                e.DetailTableView.Visible = false;
        }

        #endregion
    }

    #endregion

    /// <summary>
    /// export button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, System.EventArgs e)
    {
        ConfigureExport();

        UserControl_DropDownList_GridExportType ucGridExportType =
           rgCurrentActivity.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
           as UserControl_DropDownList_GridExportType;

        if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Csv)
            rgCurrentActivity.MasterTableView.ExportToCSV();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Excel)
            rgCurrentActivity.MasterTableView.ExportToExcel();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Word)
            rgCurrentActivity.MasterTableView.ExportToWord();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Pdf)
            rgCurrentActivity.MasterTableView.ExportToPdf();
    }

    #endregion

    #region private methods

    /// <summary>
    /// configure grid for export
    /// </summary>
    private void ConfigureExport()
    {
        rgCurrentActivity.ExportSettings.IgnorePaging = true;
        rgCurrentActivity.ExportSettings.OpenInNewWindow = true;
        rgCurrentActivity.ExportSettings.ExportOnlyData = true;
        rgCurrentActivity.ExportSettings.HideStructureColumns = true;

        rgCurrentActivity.ExportSettings.FileName = "CurrentActivity_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        //hide command item template
        foreach (GridItem commandItem in this.rgCurrentActivity.MasterTableView.GetItems(GridItemType.CommandItem))
        {
            commandItem.Visible = false;
            commandItem.Display = false;
        }

        _isExport = true;
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set updated warning hours
    /// </summary>
    public int? UpdatedWarningsHours
    {
        get
        {
            object obj = this.ViewState["UpdatedWarningsHoursForCA"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["UpdatedWarningsHoursForCA"] = value; }
    }

    #region search criteria

    /// <summary>
    /// get/set search team id
    /// </summary>
    public int? SearchTeamId
    {
        get
        {
            object obj = this.ViewState["SearchTeamIdForCA"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchTeamIdForCA"] = value; }
    }

    /// <summary>
    /// get/set search mps
    /// </summary>
    public bool SearchMsp
    {
        get
        {
            object obj = this.ViewState["SearchMspForCA"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["SearchMspForCA"] = value; }
    }

    /// <summary>
    /// set/set array list containing user status id to search for
    /// </summary>
    public ArrayList SearchUserStatusIds
    {
        get
        {
            object obj = this.ViewState["SearchUserStatusIdsCA"];
            return (obj == null) ? new ArrayList() : (ArrayList)obj;
        }
        set { this.ViewState["SearchUserStatusIdsCA"] = value; }
    }

    #endregion

    #endregion

    #region public methods

    /// <summary>
    /// reset grid
    /// </summary>
    public void ResetGrid()
    {
        rgCurrentActivity.Visible = true;
        //pnlHeader.Visible = true;

        rgCurrentActivity.EditIndexes.Clear();
        rgCurrentActivity.DataSource = null;
        rgCurrentActivity.Rebind();
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        rgCurrentActivity.CurrentPageIndex = 0;
        rgCurrentActivity.EditIndexes.Clear();
        rgCurrentActivity.Visible = false;
        //pnlHeader.Visible = false;
    }

    #endregion
  
}