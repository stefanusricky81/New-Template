using System;
using System.Collections;

using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dundas.Charting.WebControl;
using System.Data;

public partial class JimReport : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        

        DataTable dtReport2 = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcShowCurrentOpenTicketsByEmployee();
        DataView view = dtReport2.DefaultView;

        string email = "";
        

     
        

  
        NameValueCollection sdEmail = new NameValueCollection();
        # region setup nvc for unique email addresses
        foreach (DataRowView dr in view)
        {
            email = dr[0].ToString();
          //  ResponseDebug("<LI><FONT COLOR='RED'>" + email + "</FONT>");
            if (email.Trim().Length > 0)
            {
                if (!sdEmail.AllKeys.Contains(email))
                    sdEmail.Add(email, email);
            }
//            status = dr[1].ToString();
//            statusCount = Int32.Parse(dr[2].ToString());
           // ResponseDebug("<LI>" + email);   
        }
        #endregion

        

        
        SortedList ldSeries = new SortedList();
        # region setup SortedList for stats
        NameValueCollection nvSeries = new NameValueCollection();
        string statusName = "";
       
        
        foreach (DataRowView dr in view)
        {
            statusName = dr[1].ToString();
            if (statusName.Trim().Length > 0)
            {
                if (!ldSeries.Contains(statusName))
                {
                    Series s = new Series(statusName);
                    s.Type = SeriesChartType.StackedColumn;
                    
                    s.ShowLabelAsValue = true;
                    ldSeries.Add(statusName, s);
                }
            }
        }
        # endregion


        string lastEmail = "";
        string status = "";
        int statusCount = 0;
        int ctr = 0;
        # region Now build up the series, fill in 0's where an element is missing for each
        foreach (DataRowView dr in view)
        {

            email = dr[0].ToString();
            ResponseDebug("<LI><FONT COLOR='brown'>" + email + "</FONT>");
            status = dr[1].ToString();
            statusCount = Int32.Parse(dr[2].ToString());

            if (email.Trim().Length > 0)
            {
                if (lastEmail.Length == 0)
                {
                    
                    lastEmail = email;
                    
                 
                }

                if (lastEmail != email)
                {
                    ResponseDebug("<LI>Begin alignment for " + lastEmail);
                    // verify that 
                    // iterate through Series, verify the length of each series is at least same as ctr
                    // if not add zero value
                    
                    foreach (DictionaryEntry de in ldSeries)
                    {
                        Series s = (Series)de.Value;
                        if (s != null)
                            if (s.Points.Count <= ctr)
                            {
                                ResponseDebug("<LI><FONT COLOR='GREEN'>Aligning: " + s.Name + "</FONT></LI>");
                                s.Points.AddY(0);
                                s.Points[s.Points.Count - 1].ShowLabelAsValue = false;
                            }
                            else ResponseDebug("<LI><FONT COLOR='RED'>Points match ctr for: " + s.Name + "</FONT>");
                    }
                    lastEmail = email;
                    ctr++;
                }
                
                
                if (status != null)
                {
                    Series s  = (Series)ldSeries[status];
                    //Series s = (Series)de.Value;

                    s.Points.AddY(statusCount);
                    ResponseDebug("<LI>Adding Point to " + status + " : " + statusCount);
                }  else ResponseDebug("<LI>NULL");
            
            
            }
            //            status = dr[1].ToString();
            //            statusCount = Int32.Parse(dr[2].ToString());


        }
        #endregion




        

        


        #region snag first series and assign xAxis label
        Series sFirst = (Series)ldSeries.GetByIndex(0);
        ctr = 0;
        ResponseDebug("<LI><FONT COLOR='red'>Number Emails : " + sdEmail.Count + "</FONT>");
        ResponseDebug("<LI><FONT COLOR='red'>Points : " + sFirst.Points.Count + "</FONT>");

        foreach (string de2 in sdEmail)
       {
            sFirst.Points[ctr].AxisLabel = de2;
            ctr++;
        }


        
        #endregion

        #region assign series to the chart
        foreach (DictionaryEntry de in ldSeries)
        {
         
            
            Series s = (Series)de.Value;
            ResponseDebug("<LI><FONT COLOR='BLUE'>" + s.Name + " POINTS: " + s.Points.Count + "</FONT>");
            Chart1.Series.Add(s);
            


        }
        #endregion
        Chart1.Palette = ChartColorPalette.Dundas;
        Chart1.ChartAreas["Default"].AxisX.Interval = 1;
        
        

        //        for (int i = 0; i < sdEmail.Count; i++)
//            ResponseDebug("<LI>" + sdEmail[i]);




        
        /*
        foreach(DictionaryEntry de in sdEmail)
        {
            ResponseDebug("<LI>Email: " + de.Key);
        }
         * */

  //      Chart1.DataBindTable(view);

        /*
         * 
                   <Series>
                <dcwc:Series Name="Column" BorderColor="Black" ShadowOffset="2">
                    <Points>
                        <dcwc:DataPoint YValues="45"></dcwc:DataPoint>
                        <dcwc:DataPoint YValues="34"></dcwc:DataPoint>
                        <dcwc:DataPoint YValues="67"></dcwc:DataPoint>
                        <dcwc:DataPoint YValues="31"></dcwc:DataPoint>
                        <dcwc:DataPoint YValues="27"></dcwc:DataPoint>
                        <dcwc:DataPoint YValues="87"></dcwc:DataPoint>
                        <dcwc:DataPoint YValues="45"></dcwc:DataPoint>
                        <dcwc:DataPoint YValues="32"></dcwc:DataPoint>
                    </Points>
                </dcwc:Series>
            </Series>

         * 
         * 
         * 
         * 
         * */
    }

    void ResponseDebug(string s)
    {
       // Response.Write(s);
    }

}
