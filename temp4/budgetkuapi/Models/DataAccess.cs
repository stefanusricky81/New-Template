using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace budgetkuapi.Models
{
    public class DataAccess
    {
        #region connection
        private static string consCommonDb = "server=VSSQLJKT01\\K2Blackpearl;User id=sa;password=@dm1n;database=CommonDatabase;Pooling=false;";
        private static string consBISDb = "server=VSSQLJKT01\\K2Blackpearl;User id=sa;password=@dm1n;database=BIS;Pooling=false;";
        private static string consApp = "server=VSSQLJKT01\\K2Blackpearl;User id=sa;password=@dm1n;database=K2LeaveReq;Pooling=false;";
        public static SqlConnection GetConApp()
        {
            return new SqlConnection(consApp);
        }
        public static SqlConnection GetConCommonDb()
        {
            return new SqlConnection(consCommonDb);
        }
        public static SqlConnection GetConBISDb()
        {
            return new SqlConnection(consBISDb);
        }
        #endregion

        #region Get Data
        public static string GetLastUpdate(string datatype)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;
            string lastupdate = string.Empty;

            query = " SELECT * FROM [BIS].[dbo].[HistoryJob] where DataType = @datatype ";

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@datatype", SqlDbType.NVarChar).Value = datatype;

                    dt.Reset();
                    adp.Fill(dt);
                }
                if (dt.Rows.Count > 0)
                    lastupdate = dt.Rows[0]["JobDateTime"].ToString();
                else
                    lastupdate = "N/A";

                return lastupdate;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetZone(string companycode, string login, string level)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            if (level == "2")
            {
                query = " select ZoneID ID, Name = (select AmaName from [CommonDatabase].dbo.[mstr_AMA] where AmaID = S.ZoneID),  " +
                        " RegionID = (select RegionID from [CommonDatabase].dbo.[mstr_AMA] where AmaID = S.ZoneID) " +
                        " FROM[BIS].[dbo].[BudgetKuSecurity]  S " +
                        " where CompanyID = @companycode and LoginUser =  @login ";
            }
            else if (level == "1")
            {
                query = " select ZoneID ID, Name = (select EstateName from [CommonDatabase].dbo.[mstr_Estate] where Bacode = S.ZoneID ) " +
                        " FROM[BIS].[dbo].[BudgetKuSecurity]  S where CompanyID = @companycode and LoginUser = @login ";
            }
            else if (level == "4")
            {
                query = " select Z.AmaID ID, Name = (select top 1 DescZOne from [BIS].[dbo].[BudgetKuZone] Z2 where Z.amaID = Z2.Zone) "+
                        " FROM[BIS].[dbo].[BudgetKuSecurity] S INNER JOIN[BIS].[dbo].[BudgetKuZone] Z " +
                        " ON S.ZoneID = Z.Zone " +
                        " where S.CompanyID = @companycode and S.LoginUser = @login ";
            }
            else
            {
                query = " select ZoneID ID, Name = (Select Distinct DescZone FROM [BIS].[dbo].[BudgetKuZone] where CompanyId = S.CompanyID and Zone= S.ZoneID)  " +
                        " FROM[BIS].[dbo].[BudgetKuSecurity] S where CompanyID = @companycode and LoginUser = @login ";
            }

            try
            {
                using (GetConCommonDb())
                {
                    GetConCommonDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConCommonDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@companycode", SqlDbType.NVarChar).Value = companycode;
                    adp.SelectCommand.Parameters.Add("@login", SqlDbType.NVarChar).Value = login;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConCommonDb().Close();
            }
        }
        public static DataTable GetAMAByZone(string companycode, string zone)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            query = " Select RegionID, AmaID ID, Name = (SELECT AmaName From [CommonDatabase].dbo.[mstr_Ama] WHERE CompanyCode= Z.CompanyId " +
                    " and AmaID = Z.AmaID and RegionID = Z.RegionId) "+
                    " FROM[BIS].[dbo].[BudgetKuZone] Z where Zone = @zone and CompanyID = @companycode ";

            try
            {
                using (GetConCommonDb())
                {
                    GetConCommonDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConCommonDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@companycode", SqlDbType.NVarChar).Value = companycode;
                    adp.SelectCommand.Parameters.Add("@zone", SqlDbType.NVarChar).Value = zone;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConCommonDb().Close();
            }
        }
        public static DataTable GetEstateList()
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            query = " SELECT BACode, EstateName From [CommonDatabase].dbo.[mstr_Estate]  where AMA!='LSIP HO/RO' AND AMA!='BLRS' AND AMA!='REFINERY' order by EstateName asc ";

            try
            {
                using (GetConCommonDb())
                {
                    GetConCommonDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConCommonDb());
                    adp.SelectCommand.CommandType = CommandType.Text;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConCommonDb().Close();
            }
        }
        public static DataTable GetCompanyList()
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            query = " SELECT CompanyCode ID,CompanyName Name from [CommonDatabase].[dbo].[mstr_Company] order by CompanyCode ASC ";

            try
            {
                using (GetConCommonDb())
                {
                    GetConCommonDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConCommonDb());
                    adp.SelectCommand.CommandType = CommandType.Text;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConCommonDb().Close();
            }
        }
        public static DataTable GetRegionList(string companycode)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            query = " Select RegionID ID, RegionNameIn Name FROM [CommonDatabase].[dbo].[mstr_Region] " +
                    " where CompanyCode = @companycode order by CompanyCode,RegionNameIn ASC ";

            try
            {
                using (GetConCommonDb())
                {
                    GetConCommonDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConCommonDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@companycode", SqlDbType.NVarChar).Value = companycode;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConCommonDb().Close();
            }
        }
        public static DataTable GetBACodeName(string bacode)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            query = " select BACode ID, EstateName Name from[CommonDatabase].dbo.[mstr_Estate] where BACode = @bacode ";

            try
            {
                using (GetConCommonDb())
                {
                    GetConCommonDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConCommonDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@bacode", SqlDbType.NVarChar).Value = bacode;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConCommonDb().Close();
            }
        }
        public static DataTable GetAMAList(string companycode, string region)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            query = " select AmaID ID, AmaName Name From[CommonDatabase].dbo.[mstr_AMA]  " +
                    " where CompanyCode = @companycode and RegionId = @region ";

            try
            {
                using (GetConCommonDb())
                {
                    GetConCommonDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConCommonDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@companycode", SqlDbType.NVarChar).Value = companycode;
                    adp.SelectCommand.Parameters.Add("@region", SqlDbType.NVarChar).Value = region;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConCommonDb().Close();
            }
        }
        public static DataTable GetBAcodeList(string companycode, string region, string ama)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            query = " SELECT BACode ID, EstateName Name From [CommonDatabase].dbo.[mstr_Estate] " +
                    " where CompanyId = @companycode and Region = @region and AMA = @ama ";

            try
            {
                using (GetConCommonDb())
                {
                    GetConCommonDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConCommonDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@companycode", SqlDbType.NVarChar).Value = companycode;
                    adp.SelectCommand.Parameters.Add("@region", SqlDbType.NVarChar).Value = region;
                    adp.SelectCommand.Parameters.Add("@ama", SqlDbType.NVarChar).Value = ama;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConCommonDb().Close();
            }
        }
        public static DataTable GetCapexOpex(string companycode, string id)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            string[] strid = id.Split('#');
            int level = Convert.ToInt32(strid[0].ToString().Trim());
            string newid = strid[1].ToString().Trim();

            if (level == 1)
            {
                query = " SELECT ROUND(ValueOriCapex/1000000,0) ValueOriCapex, ROUND(ValueAdjusment/1000000,0) ValueAdjusment, " +
                        " ROUND(ValueCarryForward / 1000000, 0) ValueCarryForward, ROUND(CERA / 1000000, 0)CERA, LastUsedCERA,  " +
                        " ROUND(OPEXAOP / 1000000, 0)OPEXAOP, ROUND(OPEXACT / 1000000, 0)OPEXACT, LastUsedOPEX " +
                        " FROM[BIS].[dbo].[V_CapexOvex] where ZYEAR = YEAR(GETDATE()) and BACode = @newid ";
            }
            else if (level == 2)
                query = " select * from V_CapexOpexAMA where ZYEAR = YEAR(GETDATE()) and AMA = @newid ";
            else if (level == 3)
                query = " select * from V_CapexOpexRegion where ZYEAR = YEAR(GETDATE()) and CompanyID = @companycode and Region = @newid ";
            else if (level == 4)
                query = " select * from V_CapexOpexCompany where ZYEAR = YEAR(GETDATE()) and CompanyID = @companycode ";
            else if (level == 5)
                query = " select * from V_CapexOpexVP where ZYEAR = YEAR(GETDATE())  and CompanyId = @companycode and Zone= @newid "; 

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@newid", SqlDbType.NVarChar).Value = newid;
                    adp.SelectCommand.Parameters.Add("@companycode", SqlDbType.NVarChar).Value = companycode;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetOthersInfoHome(string id)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            string[] strid = id.Split('#');
            int level = Convert.ToInt32(strid[0].ToString().Trim());
            string newid = strid[1].ToString().Trim();

            if (level == 1)
            {
                query = " SELECT Category, CONVERT(VARCHAR,Total) as Dt FROM [BIS].[dbo].[TotalBlockDivision] " +
                    " WHERE BACode = @newid and ZYear = YEAR(GETDATE()) and Period = MONTH(GETDATE()) " +
                    " union all " +
                    " select Category, Dt from[BIS].[dbo].[V_Manager] where BACode = @newid ";
            }

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@newid", SqlDbType.NVarChar).Value = newid;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetOpex(string id)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            string[] strid = id.Split('#');
            int level = Convert.ToInt32(strid[0].ToString().Trim());
            string newid = strid[1].ToString().Trim();

            if (level == 1)
            {
                query = " select 0 as Seq, 'Total' as Category, ROUND(SUM(ACT)/1000000,0) ACT, ROUND(SUM(AOP)/1000000,0) as AOP " +
                    " FROM[BIS].dbo.[V_EstateCost] " +
                    " where ZYEar = YEAR(GETDATE()) and BACODE = @newid and Category != 'Total' AND Category!='DIRECT'  " +
                    " UNION ALL " +
                    " select Seq,Category, ROUND(SUM(ACT)/1000000,0) ACT, ROUND(SUM(AOP)/1000000,0) as AOP " +
                    " FROM[BIS].dbo.[V_EstateCost] " +
                    " where ZYEar = YEAR(GETDATE()) and BACODE = @newid and Category != 'Total' AND Category!='DIRECT'  " +
                    " group by Seq,Category order by seq asc ";
            }

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@newid", SqlDbType.NVarChar).Value = newid;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetOpexPerMonth(string id)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            string[] strid = id.Split('#');
            int level = Convert.ToInt32(strid[0].ToString().Trim());
            string newid = strid[1].ToString().Trim();

            if (level == 1)
            {
                query = " SELECT PERIOD, ROUND(SUM(ACT)/1000000,0) ACT, ROUND(SUM(AOP)/1000000,0) as AOP " +
                    " FROM[BIS].[dbo].[V_CostPerMonth] " +
                    " WHERE ZYear = YEAR(GETDATE()) and BACode = @newid " +
                    " GROUP BY PERIOD ";
            }

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@newid", SqlDbType.NVarChar).Value = newid;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetOpexDetail(string id, string category)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            string[] strid = id.Split('#');
            int level = Convert.ToInt32(strid[0].ToString().Trim());
            string newid = strid[1].ToString().Trim();

            if (level == 1)
            {
                query = " select Period, ROUND(ACT/1000000,0) as ACT, ROUND(AOP/1000000,0) as AOP  " +
                    " FROM[BIS].dbo.[V_EstateCost]  " +
                    " where ZYEar = YEAR(GETDATE()) and BACODE = @newid and Category != 'Total' AND Category!= 'DIRECT' and Category = @category " +
                    " order by Period ASC ";
            }

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@newid", SqlDbType.NVarChar).Value = newid;
                    adp.SelectCommand.Parameters.Add("@category", SqlDbType.NVarChar).Value = category;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetOpexUnit(string id)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            string[] strid = id.Split('#');
            int level = Convert.ToInt32(strid[0].ToString().Trim());
            string newid = strid[1].ToString().Trim();

            if (level == 1)
            {
                query = " select 0 as seq, 'Total' as Category, ROUND(SUM(ACT)/(select SUM(FFBProductionAct)  from [BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode),0) as ACT, " +
                    " ROUND(SUM(AOP) / (select SUM(FFBProductionAOP)  from [BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode),0)  as AOP " +
                    " FROM[BIS].dbo.[V_EstateCost] E " +
                    " where ZYEar = YEAR(GETDATE()) and BACODE = @newid and Category != 'Total' AND Category!= 'DIRECT' " +
                    " group by BACODE " +
                    " UNION ALL " +
                    " select seq, Category, (CASE WHEN (select SUM(FFBProductionAct)  from [BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode) = 0 THEN 0 ELSE ROUND(SUM(ACT) / (select SUM(FFBProductionAct)  from [BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode),0) END ) as ACT,  " +
                    " (CASE WHEN(select SUM(FFBProductionAOP)  from[BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode) = 0 THEN 0 ELSE ROUND(SUM(AOP) / (select SUM(FFBProductionAOP)  from[BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode), 0) END) as AOP " +
                    " FROM[BIS].dbo.[V_EstateCost] E  " +
                    " where ZYEar = YEAR(GETDATE()) and BACODE = @newid and Category != 'Total' AND Category!= 'DIRECT' " +
                    " group by seq, Category,BACODE order by seq asc ";
            }

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@newid", SqlDbType.NVarChar).Value = newid;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetOpexUnitPerMonth(string id)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            string[] strid = id.Split('#');
            int level = Convert.ToInt32(strid[0].ToString().Trim());
            string newid = strid[1].ToString().Trim();

            if (level == 1)
            {
                query = " select Period, (CASE WHEN (select SUM(FFBProductionAct)  from [BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode and Period = E.Period) = 0 THEN 0 ELSE ROUND(SUM(ACT) / (select SUM(FFBProductionAct)  from [BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode and Period = E.Period),0) END ) as ACT,  " +
                    " (CASE WHEN(select SUM(FFBProductionAOP)  from[BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode and Period = E.Period) = 0 THEN 0 ELSE ROUND(SUM(AOP) / (select SUM(FFBProductionAOP)  from[BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode and Period = E.Period), 0) END) as AOP " +
                    " FROM[BIS].dbo.[V_EstateCost] E " +
                    " where ZYEar = YEAR(GETDATE()) and BACODE = @newid and Category != 'Total' AND Category!= 'DIRECT' " +
                    " group by Period,BACODE order by Period ASC ";
            }

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@newid", SqlDbType.NVarChar).Value = newid;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetOpexUnitDetail(string id, string category)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            string[] strid = id.Split('#');
            int level = Convert.ToInt32(strid[0].ToString().Trim());
            string newid = strid[1].ToString().Trim();

            if (level == 1)
            {
                query = " select Period, (CASE WHEN (select SUM(FFBProductionAct)  from [BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode and Period = E.Period) = 0 THEN 0 ELSE ROUND(SUM(ACT) / (select SUM(FFBProductionAct)  from [BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode and Period = E.Period),0) END ) as ACT,  " +
                    " (CASE WHEN(select SUM(FFBProductionAOP)  from[BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode and Period = E.Period) = 0 THEN 0 ELSE ROUND(SUM(AOP) / (select SUM(FFBProductionAOP)  from[BIS].dbo.[Crop] where zYear = YEAR(GETDATE()) and Bacode = E.Bacode and Period = E.Period), 0) END) as AOP " +
                    " FROM[BIS].dbo.[V_EstateCost] E " +
                    " where ZYEar = YEAR(GETDATE()) and BACODE = @newid and Category != 'Total' AND Category!= 'DIRECT' and Category = @category " +
                    " group by Period,BACODE order by Period ASC ";
            }

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@newid", SqlDbType.NVarChar).Value = newid;
                    adp.SelectCommand.Parameters.Add("@category", SqlDbType.NVarChar).Value = category;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetCapex(string id)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            string[] strid = id.Split('#');
            int level = Convert.ToInt32(strid[0].ToString().Trim());
            string newid = strid[1].ToString().Trim();

            if (level == 1)
            {
                query = " select '' as AssetClass,'Total Capex' as Asset, ROUND(SUM(OriAOP)/1000000,0) as OriAOP, ROUND(SUM(Adjustment)/1000000,0) as Adjustment, " +
                    " ROUND(SUM(CF) / 1000000, 0) as CF, ROUND(SUM(Ytd) / 1000000, 0) as Ytd, ROUND(SUM(CERA) / 1000000, 0) as CERA from[BIS].dbo.[V_Capex] " +
                    " where zYear = YEAR(GETDATE()) and Bacode = @newid " +
                    " UNION ALL " +
                    " select AssetClass, Asset, ROUND(OriAOP / 1000000, 0) as OriAOP, ROUND(Adjustment / 1000000, 0) as Adjustment, ROUND(CF / 1000000, 0) as CF, " +
                    " ROUND(Ytd / 1000000, 0) as Ytd, ROUND(CERA / 1000000, 0) as CERA from[BIS].dbo.[V_Capex] " +
                    " where zYear = YEAR(GETDATE()) and Bacode = @newid   ";
            }

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@newid", SqlDbType.NVarChar).Value = newid;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetCapexDetail(string bacode, string assetCode)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;
            string temp = string.Empty;
            string otherAsset = string.Empty;

            if (assetCode == "50")
                otherAsset = "51";

            if (assetCode == "40")
                otherAsset = "41";

            if (otherAsset == "41" || otherAsset == "51")
                temp = " ((SUBSTRING(PosId, LEN(PosId) - 6, 2)) = @asset OR (SUBSTRING(PosId, LEN(PosId) - 6, 2)) = @otherasset ) ";
            else
                temp = " (SUBSTRING(PosId, LEN(PosId) - 6, 2)) = @asset ";


            query = " select 'AOP '+CONVERT(varchar,YEAR(GETDATE())) as Asset, ROUND( OriAOP/1000000,0) as Value from[BIS].dbo.[V_Capex] " +
                    " where zYear = YEAR(GETDATE()) and Bacode = @bacode and AssetClass = @asset " +
                    " UNION ALL " +
                    " select 'Carry Forward' as Asset, ROUND(CF/1000000,0) as Value from[BIS].dbo.[V_Capex] " +
                    " where zYear = YEAR(GETDATE()) and Bacode = @bacode and AssetClass = @asset " +
                    " UNION ALL " +
                    " select 'CERA' as Asset, ROUND(SUM(Total_Budget)/1000000,0) as Value from [BIS].[dbo].[tblCera]  " +
                    " where zYear = YEAR(GETDATE()) and Werks = @bacode and " + temp +
                    " UNION ALL " +
                    " select 'Release' as Asset, ROUND(SUM(WTJHR)/1000000,0) from [BIS].[dbo].[tblWBS]   " +
                    "  where Vorga = 'KBFR' and zYear = YEAR(GETDATE()) and Werks = @bacode and " + temp;

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@bacode", SqlDbType.NVarChar).Value = bacode;
                    if (otherAsset == "41" || otherAsset == "51")
                    {
                        adp.SelectCommand.Parameters.Add("@asset", SqlDbType.NVarChar).Value = assetCode;
                        adp.SelectCommand.Parameters.Add("@otherasset", SqlDbType.NVarChar).Value = otherAsset;
                    }
                    else
                        adp.SelectCommand.Parameters.Add("@asset", SqlDbType.NVarChar).Value = assetCode;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static decimal GetOtherAsset(string bacode, string assetCode)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;
            string otherAsset = string.Empty;
            decimal total = 0;

            if (assetCode == "50")
                otherAsset = "51";

            if (assetCode == "40")
                otherAsset = "41";

            query = " select  ROUND(SUM(Total_Budget) / 1000000, 0) as Total from [BIS].[dbo].[tblCera]    " +
                    " where zYear = YEAR(GETDATE()) and Werks = @bacode and (SUBSTRING(PosId, LEN(PosId) - 6, 2)) = @otherasset "  ;

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@bacode", SqlDbType.NVarChar).Value = bacode;
                    adp.SelectCommand.Parameters.Add("@otherasset", SqlDbType.NVarChar).Value = otherAsset;

                    dt.Reset();
                    adp.Fill(dt);
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Total"].ToString() == null)
                        total = 0;
                    else
                        total = Convert.ToDecimal(dt.Rows[0]["Total"].ToString());
                }
                    

                return total;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetCrop(string companycode, string id)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            string[] strid = id.Split('#');
            int level = Convert.ToInt32(strid[0].ToString().Trim());
            string newid = strid[1].ToString().Trim();

            if (level == 1)
                query = " select Category, AOP, ACT from TblCropBudgetKuEstate where bacode =  @newid ";
            else if (level == 2)
                query = " select Category, AOP, ACT from [TblCropBudgetKuAMA] where AMA = @newid ";
            else if (level == 3)
                query = " select Category, AOP, ACT from [TblCropBudgetKuRegion] where CompanyId = @companycode and Region = @newid ";
            else if (level == 4)
                query = " select Category, AOP, ACT from [TblCropBudgetKuCompany] where CompanyId = @companycode  ";
            else if (level==5)
                 query = " select Category, AOP, ACT from [TblCropBudgetKuVP] where Zone = @newid ";
            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@newid", SqlDbType.NVarChar).Value = newid;
                    adp.SelectCommand.Parameters.Add("@companycode", SqlDbType.NVarChar).Value = companycode;
                    adp.SelectCommand.CommandTimeout = 36000;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetFFBDetail(string id)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            string[] strid = id.Split('#');
            int level = Convert.ToInt32(strid[0].ToString().Trim());
            string newid = strid[1].ToString().Trim();

            if (level == 1)
                query = " select Period, ROUND(ACT/1000,0) as ACT, ROUND(AOP/1000,0) as AOP from [BIS].dbo.[V_FFBDetail] where Ba = @newid order by Period asc ";

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@newid", SqlDbType.NVarChar).Value = newid;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetAllowUser(string loginuser)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;

            query = " select CompanyCode,LevelId,Token,Imei "+
                    " from[BIS].dbo.[LoginApp] "+
                    " where LoginUser = @loginuser ";

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@loginuser", SqlDbType.NVarChar).Value = loginuser;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        public static DataTable GetEmail(string username, string companycode)
        {
            DataTable dt = new DataTable();
            string query, email = string.Empty;

            if (companycode == "LSIP")
            {
                query = " select Email as mail from [CommonDatabase].dbo.[AD_Employee] where LoginUser = @username ";
            }
            else
            {
                query = "Declare @displayname varchar(100) " +
                       " set @displayname = @username " +
                       " DECLARE @queryFormat VARCHAR(MAX) = ' " +
                       " SELECT* FROM OPENQUERY(ADSI_INDOAGRI, '' " +
                       "    SELECT manager, mail, department, title, cn, employeeID, samAccountName, userPrincipalName " +
                       "    FROM ''''LDAP://10.126.20.1/OU=Indofood,DC=Indofood,DC=corp'''' " +
                       "    WHERE objectClass = ''''User'''' AND objectCategory = ''''Person'''' AND samAccountName = ''''#p0'''''')'; " +
                       " DECLARE @sql VARCHAR(MAX) = ''; " +
                       "    SET @sql = @sql + replace(@queryFormat, '#p0', @displayname); " +
                       "    exec(@sql); ";
            }
            

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConBISDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;

                    dt.Reset();
                    adp.Fill(dt);
                }

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        #endregion

        #region For Update Function
        public static int UpdateLoginApp(string loginuser, string token)
        {
            DataTable dt = new DataTable();
            string query, email = string.Empty;

            query = " UPDATE [BIS].dbo.[LoginApp] set Token = @token, DateModified = GETDATE()" +
            " WHERE LoginUser = @loginuser ";

            try
            {
                using (GetConBISDb())
                {
                    GetConBISDb().Open();
                    SqlCommand cmd = new SqlCommand(query, GetConBISDb());
                    cmd.Parameters.Add("@token", SqlDbType.NVarChar).Value = token;
                    cmd.Parameters.Add("@loginuser", SqlDbType.NVarChar).Value = loginuser;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                GetConBISDb().Close();
            }
        }
        #endregion

        #region For Checking Function
        public static DataTable Level(string token)
        {
            DataTable dt = new DataTable();
            string query = " select CompanyCode ID, Name = (SELECT CompanyName From [CommonDatabase].dbo.[mstr_Company]  where CompanyCode = L.CompanyCode), LevelId "+
                           " from[BIS].dbo.[LoginApp] L where Token = @token ";
            try
            {
                using (GetConCommonDb())
                {
                    GetConCommonDb().Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, GetConCommonDb());
                    adp.SelectCommand.CommandType = CommandType.Text;
                    adp.SelectCommand.Parameters.Add("@token", SqlDbType.NVarChar).Value = token;

                    dt.Reset();
                    adp.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                GetConCommonDb().Close();
            }
        }
        public static string getMoneyFormat(string money)
        {
            string[] strAOP = money.Split('.');
            string tempAOP = strAOP[0];

            NumberFormatInfo nfi = new CultureInfo("id-ID", false).NumberFormat;
            nfi.CurrencyGroupSeparator = ".";
            nfi.CurrencySymbol = "Rp ";

            string strMoney = string.Empty;
            string tempMoney = string.Empty;
            string newString = Regex.Replace(tempAOP, "[^.0-9]", "");
            if (money == "N/A")
            {
                strMoney = "N/A";
            }
            else
            {
                tempMoney = Convert.ToDecimal(money).ToString("C", nfi) + " Juta";
            }

            return tempMoney;
        }       
        public static string getNumberFormat(string no)
        {
            string[] str = no.Split('.');

            NumberFormatInfo nfi = new CultureInfo("id-ID", false).NumberFormat;
            nfi.CurrencyGroupSeparator = ".";
            nfi.CurrencySymbol = "";

            string strNo = string.Empty;
            strNo = Convert.ToDecimal(str[0]).ToString("C", nfi);

            return strNo;
        }
        #endregion

    }
}