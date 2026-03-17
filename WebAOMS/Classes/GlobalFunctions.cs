
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using WebAOMS.Base;

public class GlobalFunctions
    {
    string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
    public static string CurrencyFormatString(double Amount)
        {
            return "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", Amount);
        }
        public static string CurrencyFormatStringNoSymbol(double Amount)
        {
            return string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", Amount);
        }
        
        public static string QRCodeValue(string PrintedBy, string ComputerIP)
        {
            return "SYSTEM GENERATED DOCUMENT" + Environment.NewLine
                                    + "Printed by : " + PrintedBy + Environment.NewLine
                                    + "Print Date : " + DateTime.Now + Environment.NewLine
                                    + "I.P. Address : " + ComputerIP + Environment.NewLine;
        }

    public static int getCurrentYear()
    {
        DataTable arec;
        int tyear = 0;
        arec = ISfn.ToDatatable("select year(getdate()) as tyear");
        tyear = Convert.ToInt32(arec.Rows[0]["tyear"]);
        return tyear;
    }
    
    public static string QR_globalstr { get; set; }
        public static int wfppreparer_sign { get; set; }
        public static int wfpdepthead_sign { get; set; }

}