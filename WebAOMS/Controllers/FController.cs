using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace Transfinder.Controllers
{

    public class FController : Controller
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["fmisConn"].ToString();
      public ActionResult r(string r ="")
        {
            string sql = "execute Tracking.[usp_get_loghistory]  @r ";
            SqlConnection connection = new SqlConnection(fmisConn);
            DataSet ds = new DataSet();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@r", r);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                ViewBag.r = r;
                connection.Close();
            }
            return View("r", ds);
        }
      public ActionResult TransactionInfo(string r = "")
      {
          string sql = "execute [Tracking].[usp_get_transctionInfo]  @r ";
          string particular = "-",apprefno = "-",app = "-",amount ="-";

          SqlConnection connection = new SqlConnection(fmisConn);
          DataSet ds = new DataSet();
          using (SqlCommand command = new SqlCommand(sql, connection))
          {
              command.Parameters.AddWithValue("@r", r);
              connection.Open();
              SqlDataReader read = command.ExecuteReader();

              if (read.HasRows == true)
              {
                  while (read.Read())
                  {
                      particular = read["particular"].ToString();
                      app = read["is_name"].ToString();
                      amount = read["amount"].ToString();
                      apprefno = read["trans_refno"].ToString();
                  }
              }
             
              connection.Close();
          }
          return Json(new {Particular = particular, Apprefno = apprefno, App = app, Amount = amount});
      }
        public ActionResult _r(string r="")
        {
            string sql = "execute Tracking.[usp_get_loghistory]  @r ";
            SqlConnection connection = new SqlConnection(fmisConn);
            DataSet ds = new DataSet();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@r", r);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                ViewBag.r = r;
                connection.Close();
            }

            return PartialView("_r", ds);
        }
    }
}