using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for Tools
/// </summary>
public class Tools
{
    private static Random random = new Random();

	public Tools()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void WriteLog(string str, string dir)
    {
        dir = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMMM") + "\\" + dir;
        string dirPath = "C:\\Logs\\TopModelIndo\\" + dir + "\\";
        string filePath = dirPath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

        try
        {
            if (!Directory.Exists(dirPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(dirPath);
            }

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + "," + str);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public static string RandomChar(int length)
    {
        string chars = "abcdefghijkl0123456789mnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}