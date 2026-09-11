using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PhoneFormat
/// </summary>
public class PhoneFormat
{
    public PhoneFormat()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public class CellPhone
    {
        public static string cellformat(string phonenumber)
        {
            try
            {
                string newformat = string.Empty;

                long cellphone = Convert.ToInt64(phonenumber.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""));

                if (cellphone.ToString().Length > 3 && cellphone.ToString().Length <= 7)
                {
                    if (phonenumber.Substring(0, 1) == "0")
                        newformat = "0" + cellphone.ToString("###-####");
                    else
                        newformat = cellphone.ToString("###-####");
                }
                else if (cellphone.ToString().Length > 7 && cellphone.ToString().Length <= 9)
                {
                    if (phonenumber.Substring(0, 1) == "0")
                        newformat = "0" + cellphone.ToString("###-###-###");
                    else
                        newformat = cellphone.ToString("###-###-###");
                }
                else if (cellphone.ToString().Length > 9 && cellphone.ToString().Length <= 10)
                {
                    if (phonenumber.Substring(0, 1) == "0")
                        newformat = "0" + cellphone.ToString("###-###-####");
                    else
                        newformat = cellphone.ToString("###-###-####");
                }
                else if (cellphone.ToString().Length > 10 && cellphone.ToString().Length <= 11)
                {
                    if (phonenumber.Substring(0, 1) == "0")
                        newformat = "0" + cellphone.ToString("###-###-#####");
                    else
                        newformat = cellphone.ToString("###-###-#####");
                }
                else if (cellphone.ToString().Length > 11 && cellphone.ToString().Length <= 13)
                {
                    if (phonenumber.Substring(0, 1) == "0")
                        newformat = "0" + cellphone.ToString("###-###-######");
                    else
                        newformat = cellphone.ToString("###-###-#######");
                }
                else
                    newformat = phonenumber;

                return newformat;
            }
            catch (Exception ex)
            {
               return phonenumber;
            }
        }
    }
}