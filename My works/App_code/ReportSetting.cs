using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;

/// <summary>
/// Summary description for ReportSetting
/// </summary>
public class ReportSetting
{
    public ReportSetting()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public class ARReport
    {
        private static void excel(DataSet ds, string filename)
        {
            try
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = filename;
                savefile.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

                //ds.Tables[0].Columns.Add("No", typeof(Int32));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    StreamWriter wr = new StreamWriter(savefile.FileName);
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        wr.Write(ds.Tables[0].Columns[i].ToString().ToUpper() + "\t");
                    }
                    wr.Write("Total" + "\t");
                    wr.WriteLine();

                    //write rows to excel file
                    double sum30days = 0, sum60days = 0, sum90days = 0, summorethan90dyas = 0, sumtotal = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        double sumcolumn = 0;
                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (ds.Tables[0].Rows[i][j] != null)
                            {
                                wr.Write(Convert.ToString(ds.Tables[0].Rows[i][j]) + "\t");

                                if (j == 5)//30days
                                    sum30days += Convert.ToDouble(ds.Tables[0].Rows[i][j]);
                                if (j == 6)//31-60days
                                    sum60days += Convert.ToDouble(ds.Tables[0].Rows[i][j]);
                                if (j == 7)//61-90days
                                    sum90days += Convert.ToDouble(ds.Tables[0].Rows[i][j]);
                                if (j == 8)//>90days
                                    summorethan90dyas += Convert.ToDouble(ds.Tables[0].Rows[i][j]);

                                if (j == 5 || j == 6 || j == 7 || j == 8)// total for all column
                                    sumcolumn += Convert.ToDouble(ds.Tables[0].Rows[i][j]);

                                sumtotal += sumcolumn;
                            }
                            else
                                wr.Write("\t");
                        }
                        wr.Write(sumcolumn + "\t");
                        //go to next line
                        wr.WriteLine();
                    }
                    wr.Write("Total" + "\t\t\t\t\t");
                    wr.Write(sum30days + "\t");
                    wr.Write(sum60days + "\t");
                    wr.Write(sum90days + "\t");
                    wr.Write(summorethan90dyas + "\t");
                    wr.Write(sumtotal + "\t");
                    wr.WriteLine();

                    //close file
                    wr.Close();
                    BitByBit.Logging.WriteMessage(String.Format("Data saved in Excel format at location {0}", savefile.FileName));
                }
                else
                    BitByBit.Logging.WriteMessage("No Data");
            }
            catch (Exception ex)
            {
                BitByBit.Logging.WriteMessage(String.Format("Create Excel:{0}", ex));
            }
            finally
            {

            }
        }
    }

    public class Email
    {
        static void sendemail(string bodyemail, string _emailto, string from, string fileattach1, bool _cc, string cc, string bcc)
        {
            try
            {
                System.Net.Mail.Attachment attachment1;
                bodyemail += GetFooter();

                BitByBit.Logging.WriteMessage("---- start sendemail");
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                System.Net.Mail.MailMessage _mailMessage = new System.Net.Mail.MailMessage();
                System.Net.Mail.MailAddress _mailFrom = new System.Net.Mail.MailAddress(from);
                System.Net.Mail.SmtpClient _client = new System.Net.Mail.SmtpClient();

                _mailMessage.From = _mailFrom;
                _mailMessage.To.Add(_emailto);
                if (_cc)
                    _mailMessage.CC.Add(cc);

                _mailMessage.Bcc.Add(bcc);

                _mailMessage.Subject = " Accounts Receivable Report";
                _mailMessage.Body = bodyemail;
                if (File.Exists(fileattach1))
                {
                    attachment1 = new System.Net.Mail.Attachment(fileattach1);
                    _mailMessage.Attachments.Add(attachment1);
                }
                _mailMessage.IsBodyHtml = true;

                //_client.Send(_mailMessage);
                BitByBit.Logging.WriteMessage("---- end sendemail");
                BitByBit.Logging.WriteMessage(string.Format("Email Delivered to {0}", _emailto));
            }
            catch (Exception ex)
            {
                BitByBit.Logging.WriteMessage(String.Format("sendemail exception:{0}", ex));
                BitByBit.Logging.WriteMessage("---- end sendemail");
                return;
            }
        }

        private static string GetFooter()
        {
            var sb = new StringBuilder();

            sb.Append("<p>");
            sb.Append(String.Format("<a href=\"https://{0}\">{0}</a>", "www.bitxbit.com"));
            sb.Append("<br />866-391-1566");
            sb.Append("</p>");

            sb.Append(GetDisclaimer());

            return sb.ToString();
        }

        private static string GetDisclaimer(bool putInHtmlParagraph = true)
        {
            var sb = new StringBuilder();
            if (putInHtmlParagraph)
                sb.Append("<p>");
            sb.Append("Confidentiality Note: This e-mail, and any attachment to it, contains privileged and confidential information intended only for the use of the individual(s) or entity named on the e-mail. If the reader of this e-mail is not the intended recipient, ");
            sb.Append("or the employee or agent responsible for delivering it to the intended recipient, you are hereby notified that reading it is strictly prohibited. If you have received this e-mail in error, please immediately return it to the sender and delete ");
            sb.Append("it from your system.");
            if (putInHtmlParagraph)
                sb.Append("</p>");

            return sb.ToString();
        }
    }
}