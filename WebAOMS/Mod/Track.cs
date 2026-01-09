using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc;
using System.Text;
using System.Data.Entity;
using Kendo.Mvc.Extensions;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web.Services;
using System.Web.Services.Protocols;
using Kendo.Mvc.UI;
using System.Web.Script.Serialization;
using System.Web.Security;
using WebAOMS.Models;
using WebAOMS.Base;
namespace WebAOMS.Mod
{
    public static class Track
    {
        static string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        
        public static string get_tracking_link(string unique_refno)
        {
            return check_if_exist_unique_refno(unique_refno);
        }

        public static string check_if_exist_unique_refno(string unique_refno)
        {
            string result = "invalid";
            DataTable rec = new DataTable();
            string cmdStr = @"SELECT * FROM [fmis].[Tracking].[tbl_t_transactionDetails] where Unique_refno = @unique_refno";

            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@unique_refno", unique_refno);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }

            if (rec.Rows.Count > 0)
            {
                result = "https://pgas.ph/aoms/f/r?r=" + unique_refno + "";
            }
            return result;
        }
    }
}