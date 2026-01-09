//using WebAOMS.Classes.Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


    public class GlobalFunctions
    {
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
        //public static string getCurrentYear() 
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //        {
        //            SqlCommand com = new SqlCommand(@"select [Value] from tbl_R_BMSReportTextBoxes where ReportID = " + ReportID + " and ActionCode = 1 and FieldID = " + FieldID + "", con);
        //            con.Open();
        //            return com.ExecuteScalar().ToString();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return "";
        //    }
        //}
        public static string QR_globalstr { get; set; }
        public static int wfppreparer_sign { get; set; }
        public static int wfpdepthead_sign { get; set; }

}