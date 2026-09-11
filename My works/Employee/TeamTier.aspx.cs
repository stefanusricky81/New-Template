using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_TeamTier : BasePage
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //active teams
            DesktopShared.CollectionClasses.TeamCollection _activeTeams = DesktopShared.Team.GetActiveTeams();
            foreach (DesktopShared.EntityClasses.TeamEntity objTeam in _activeTeams)
                Display(objTeam, null);

            //active tiers
            DesktopShared.CollectionClasses.TierCollection _activeTiers = DesktopShared.Tier.GetActiveTiers();
            foreach (DesktopShared.EntityClasses.TierEntity objTier in _activeTiers)
                Display(null, objTier);
        }
    }

    #region private methods

    /// <summary>
    /// display name of team/tier and list users
    /// </summary>
    /// <param name="objTeam"></param>
    /// <param name="objTier"></param>
    private void Display(DesktopShared.EntityClasses.TeamEntity objTeam, DesktopShared.EntityClasses.TierEntity objTier)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        
        if (objTeam != null)
        {
            sb.Append(String.Format("<span style=\"font-weight:bold; text-decoration:underline;\">{0}</span><br />", objTeam.Name.Trim()));
            DesktopShared.CollectionClasses.UsersCollection _users = _users = DesktopShared.Team.GetActiveUsers(objTeam.Id);

            if (_users != null)
            {
                foreach (DesktopShared.EntityClasses.UsersEntity objUser in _users)
                    sb.Append(String.Format("{0} {1}<br />", objUser.First.Trim(), objUser.Last.Trim()));
            }

        }
        else if (objTier != null)
        {
            sb.Append(String.Format("<span style=\"font-weight:bold; text-decoration:underline;\">{0}</span><br />", objTier.Name.Trim()));
            var _employees = DesktopShared.Employee.GetDataTable("", "", true, null, objTier.Id);
            foreach (DataRow objEmployee in _employees.Rows)
                sb.Append(String.Format("{0} {1}<br />", objEmployee["First"].ToString().Trim(), objEmployee["Last"].ToString().Trim()));
        }

        
        if (objTeam != null)
            litTeams.Text += sb.ToString() + "<br />";
        else if (objTier != null)
            litTiers.Text += sb.ToString() + "<br />";
    }

    #endregion
}