using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace cs_forest
{
    public class AddOn
    {
        Tools tl = new Tools();

        public void StopChitik(string msisdn)
        {
            string strSQL = "UPDATE [dbo].[Subscriber] SET [subs_status]=@subs_status,[subs_nextepisode]=@subs_nextepisode WHERE subs_msisdn=@subs_msisdn AND subs_keyword=@subs_keyword AND subs_provid=@subs_provid";
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=192.168.28.142;Initial Catalog=DigiSyok;Persist Security Info=True;User ID=dbuser;Password=celcom2@@8"))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSQL, cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("subs_msisdn", SqlDbType.NVarChar).Value = msisdn;
                        cmd.Parameters.Add("subs_keyword", SqlDbType.NVarChar).Value = "CHITIK";
                        cmd.Parameters.Add("subs_provid", SqlDbType.Int).Value = 1324;
                        cmd.Parameters.Add("subs_status", SqlDbType.Int).Value = 1;
                        cmd.Parameters.Add("subs_nextepisode", SqlDbType.Int).Value = 2;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                tl.process_log( DateTime.Now.ToString() + "| "+ ex.Message, "Err");
            }
        }
    }
}