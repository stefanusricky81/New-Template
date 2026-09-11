using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Services;
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;

/// <summary>
/// Summary description for Clients
/// </summary>
[System.Web.Script.Services.ScriptService]
public class Clients : System.Web.Services.WebService
{

    public Clients()
    {
    }

    #region telerik

    [WebMethod]
    public RadComboBoxItemData[] GetClients(object context)
    {
        //IDictionary<string, object> contextDictionary = (IDictionary<string, object>)context;
        //string filterString = Uri.UnescapeDataString(((string)contextDictionary["FilterString"]).Trim());
        string filterString = "";
        Dictionary<string, object> dict = context as Dictionary<string, object>;
        if (dict != null && dict.ContainsKey("FilterString"))
        {
            filterString = dict["FilterString"].ToString();
        }
        else if (dict != null && dict.ContainsKey("Text"))
        {
            filterString = dict["Text"].ToString();
        }

        string companyLike = "%" + filterString + "%";

        DesktopShared.CollectionClasses.ClientCollection clients = DesktopShared.Client.GetAllActive();

        #region get cached collection of clients [removed]

        /*DesktopShared.CollectionClasses.ClientCollection clients = null;
        if (HttpContext.Current.Cache["ClientCollection"] == null)
        {
            clients = DesktopShared.Client.GetAllActive();
            HttpContext.Current.Cache.Insert("ClientCollection", clients, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                TimeSpan.FromMinutes(120));
        }
        else
            clients = (DesktopShared.CollectionClasses.ClientCollection)HttpContext.Current.Cache["ClientCollection"];*/

        #endregion

        EntityView<DesktopShared.EntityClasses.ClientEntity> clientView =
            new EntityView<DesktopShared.EntityClasses.ClientEntity>(clients);

        IPredicateExpression clientsFilter = new PredicateExpression();

        IPredicateExpression orFilter = new PredicateExpression();
        orFilter.Add(DesktopShared.HelperClasses.ClientFields.Company % companyLike);
        orFilter.AddWithOr(DesktopShared.HelperClasses.ClientFields.Code % companyLike);

        clientsFilter.Add(orFilter);

        clientView.Filter = clientsFilter;
        
        List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(clientView.Count);

        for (int i =0; i < clientView.Count; i++)
        {
            RadComboBoxItemData itemData = new RadComboBoxItemData();
            itemData.Text = clientView[i].Company.Trim();
            itemData.Value = clientView[i].Pclient.ToString();

            result.Add(itemData);

        }



        return result.ToArray();
    }

    [WebMethod]
    public RadComboBoxItemData[] GetClients2(object context)
    {
        IDictionary<string, object> contextDictionary = (IDictionary<string, object>)context;
        string filterString = Uri.UnescapeDataString(((string)contextDictionary["FilterString"]).Trim());
        string companyLike = "%" + filterString + "%";

        DesktopShared.CollectionClasses.ClientCollection clients =
            new DesktopShared.CollectionClasses.ClientCollection();

        IPredicateExpression clientsFilter = new PredicateExpression();
        clientsFilter.Add(DesktopShared.HelperClasses.ClientFields.Active == "Y");
        clientsFilter.Add(DesktopShared.HelperClasses.ClientFields.Rectype == "c");

        IPredicateExpression orFilter = new PredicateExpression();
        orFilter.Add(DesktopShared.HelperClasses.ClientFields.Company % companyLike);
        orFilter.AddWithOr(DesktopShared.HelperClasses.ClientFields.Code % companyLike);
        clientsFilter.Add(orFilter);

        ISortExpression clientsSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        clientsSort.Add(DesktopShared.HelperClasses.ClientFields.Company | SortOperator.Ascending);

        clients.GetMulti(clientsFilter, 0, clientsSort);

        List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(clients.Count);

        foreach (DesktopShared.EntityClasses.ClientEntity client in clients)
        {
            RadComboBoxItemData itemData = new RadComboBoxItemData();
            itemData.Text = client.Company.Trim();
            itemData.Value = client.Pclient.ToString();

            result.Add(itemData);
        }

        return result.ToArray();
    }


    [WebMethod]
    public RadComboBoxItemData[] GetBackupClients(object context)
    {
        IDictionary<string, object> contextDictionary = (IDictionary<string, object>)context;
        string filterString = Uri.UnescapeDataString(((string)contextDictionary["FilterString"]).Trim());
        string companyLike = "%" + filterString + "%";

        #region get cached collection of clients

        DesktopShared.CollectionClasses.ClientCollection clients = null;
        if (HttpContext.Current.Cache["ClientCollection"] == null)
        {
            clients = DesktopShared.Client.GetAllActive();
            HttpContext.Current.Cache.Insert("ClientCollection", clients, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                TimeSpan.FromMinutes(120));
        }
        else
            clients = (DesktopShared.CollectionClasses.ClientCollection)HttpContext.Current.Cache["ClientCollection"];

        #endregion

        // Backup Clients
        DesktopShared.CollectionClasses.BackupSetCollection backups = null;
        backups = DesktopShared.Backup.GetAllActive();

        EntityView<DesktopShared.EntityClasses.BackupSetEntity> backupView =
            new EntityView<DesktopShared.EntityClasses.BackupSetEntity>(backups);

        ISortExpression backupSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        backupSort.Add(DesktopShared.HelperClasses.BackupSetFields.ClientId | SortOperator.Ascending);

        EntityView<DesktopShared.EntityClasses.ClientEntity> clientView =
            new EntityView<DesktopShared.EntityClasses.ClientEntity>(clients);

        IPredicateExpression clientsFilter = new PredicateExpression();

        IPredicateExpression orFilter = new PredicateExpression();
        orFilter.Add(DesktopShared.HelperClasses.ClientFields.Company % companyLike);
        orFilter.AddWithOr(DesktopShared.HelperClasses.ClientFields.Code % companyLike);

        clientsFilter.Add(orFilter);

        clientView.Filter = clientsFilter;
        backupView.Sorter = backupSort;

        List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(clientView.Count);

        for (int i = 0; i < clientView.Count; i++)
        {
            RadComboBoxItemData itemData = new RadComboBoxItemData();
            itemData.Text = clientView[i].Company.Trim();
            itemData.Value = clientView[i].Pclient.ToString();

            for (int j = 0; j < backupView.Count; j++)
            {

                if (backupView[j].Client.Company.Trim() == clientView[i].Company.Trim())
                {
                    if (j == 0)
                    {
                        result.Add(itemData);
                    }
                    else
                    { // avoid duplicate
                        if (backupView[j].Client.Company.Trim() != backupView[j-1].Client.Company.Trim())
                        {
                            result.Add(itemData);
                        }
                    }
                }
            }

        }



        return result.ToArray();
    }

    #endregion
    /// <summary>
    /// get list of clients based on search criteria
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [WebMethod]
    public List<Client> GetClients3(string query)
    {
        List<Client> _clients = new List<Client>();

        #region get clients collection

        DesktopShared.CollectionClasses.ClientCollection clients = new DesktopShared.CollectionClasses.ClientCollection();

        IPredicateExpression clientsFilter = new PredicateExpression();
        clientsFilter.Add(DesktopShared.HelperClasses.ClientFields.Active == "Y");
        //clientsFilter.Add(DesktopShared.HelperClasses.ClientFields.Rectype == "c");

        IPredicateExpression orFilter = new PredicateExpression();
        orFilter.AddWithOr(DesktopShared.HelperClasses.ClientFields.Company % String.Format("%{0}%", query.Trim()));
        orFilter.AddWithOr(DesktopShared.HelperClasses.ClientFields.Code % String.Format("%{0}%", query.Trim()));
        clientsFilter.Add(orFilter);

        ISortExpression clientsSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        clientsSort.Add(DesktopShared.HelperClasses.ClientFields.Company | SortOperator.Ascending);

        clients.GetMulti(clientsFilter, 0, clientsSort);

        #endregion

        foreach (var objClient in clients)
            _clients.Add(new Client { Name = objClient.Company.Trim(), Id = objClient.Pclient });

        return _clients;
    }

    public List<Client> GetClients4(string query)
    {
        List<Client> _clients = new List<Client>();

        #region get clients collection

        DesktopShared.CollectionClasses.ClientCollection clients = new DesktopShared.CollectionClasses.ClientCollection();

        IPredicateExpression clientsFilter = new PredicateExpression();
        clientsFilter.Add(DesktopShared.HelperClasses.ClientFields.Active == "Y");

        IPredicateExpression orFilter = new PredicateExpression();
        orFilter.AddWithOr(DesktopShared.HelperClasses.ClientFields.Company % String.Format("%{0}%", query.Trim()));
        orFilter.AddWithOr(DesktopShared.HelperClasses.ClientFields.Code % String.Format("%{0}%", query.Trim()));
        clientsFilter.Add(orFilter);

        ISortExpression clientsSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        clientsSort.Add(DesktopShared.HelperClasses.ClientFields.Company | SortOperator.Ascending);

        clients.GetMulti(clientsFilter, 0, clientsSort);

        #endregion

        foreach (var objClient in clients)
            _clients.Add(new Client { Name = objClient.Company.Trim(), Id = objClient.Pclient });

        return _clients;
    }

    /// <summary>
    /// client poco
    /// </summary>
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}

