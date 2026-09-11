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
using BitByBit;
using SD.LLBLGen.Pro.ORMSupportClasses;
using DesktopShared;
using System.Text;
using System.Collections.Generic;

public partial class Maintenance_ClientServerImport : BasePage
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Import is no longer available.", DesktopShared.Bootstrap.Alert.AlertType.Warning, false);
    }

    #region protected events

    /// <summary>
    /// import button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImport_Click(object sender, EventArgs e)
    {
        List<string> _importedServers = new List<string>();
        DesktopShared.KaseyaHelper.Agent.ImportServersToClientServer(ref _importedServers);
        _importedServers.Sort();
        int _count = _importedServers.Count;

        System.Text.StringBuilder _sb = new StringBuilder();
        _sb.Append(String.Format("{0} Server{1} been imported- {2}.", _count, _count == 1 ? " has" : "s have", DateTime.Now));
        if (_count > 0)
        {
            _sb.Append("<br /><br />");
            _sb.Append(String.Join("<br />", _importedServers.ToArray()));
        }

        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, _sb.ToString(), DesktopShared.Bootstrap.Alert.AlertType.Success, false);
    }

    #endregion

}

