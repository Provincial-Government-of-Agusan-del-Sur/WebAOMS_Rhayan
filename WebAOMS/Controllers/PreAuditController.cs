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
using WebAOMS.Mod;
using WebAOMS.wsfmis;
namespace WebAOMS.Controllers
{
    public class PreAuditController : Controller
    {
        private fmisEntities fmisdb = new fmisEntities();
        WebServiceSoapClient ws = new WebServiceSoapClient();
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        // GET: PreAudit
        public ActionResult Index()
        {
            return View();
        }
public ActionResult Return_Liquidation()
        {
            return View();
        }

        public ActionResult USAD_CBMS_MAPPing()
        {
            return View();
        }

        public ActionResult Index_PO()
        {
            return View();
        }

        public ActionResult TransactionMonitoring()
        {
            return View();
        }
        public ActionResult ActivityTrainingDesignMonitoring()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult open_document(string refno)
        {
            try
            {
                ViewBag.refno = refno;
                //OleDbHelper.ExecuteNonQuery(dbcon_fmis, System.Data.CommandType.Text, "execute [Tracking].[usp_insert_status_byOffice] " + USER.C_swipeID + "," + USER.C_OfficeID + "");


                string sql = "execute Tracking.[usp_get_loghistory_activityTraining]  @r ";
                SqlConnection connection = new SqlConnection(fmisConn);
                DataSet ds = new DataSet();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@r", refno);
                    connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    connection.Close();
                }
                connection.Close();
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return PartialView("partial/_doc_public", null);
                }

                else
                {
                    return PartialView("partial/_view_doc_public", ds);
                }
            }
            catch (Exception e)
            {
                return PartialView("partial/_doc_public", null);
            }
            
        }

        public ActionResult _partial_view_transdetails(string dvno)
        {
            TrackingController trackController = new TrackingController();
            if (dvno == null)
            {
                dvno = "000-00-00-0000";
            }
            System.Net.ServicePointManager.Expect100Continue = false;
            DataTable rec = trackController.getTransactions(dvno.Replace("'", "''").Replace("--", "")).Tables[0];
            if (rec.Rows.Count > 0)
            {
                ViewBag.claimantname = rec.Rows[0]["claimantname"].ToString();
                ViewBag.Particular = rec.Rows[0]["Particular"].ToString();
                ViewBag.GAmount = rec.Rows[0]["GAmount"].ToString();
                ViewBag.dvno = dvno;
            }
            return View("partial/_Transaction_details", trackController.getTransactions(dvno.Replace("'", "''").Replace("--", "")));
        }

        public ActionResult Get_pei_summary_current()
        {
            
            //    string pie = "$(\"#chart\").kendoChart({" +
            //    "title: {" +
            //    "    position: \"bottom\"," +
            //    "    text: \"List of Transaction in Pre-Audit\"" +
            //    "}," +
            //    "legend: {" +
            //    "    visible: false" +
            //    "}," +
            //    "chartArea: {" +
            //    "    background: \"\"" +
            //    "}," +
            //    "seriesDefaults: {" +
            //    "    labels: {" +
            //    "        visible: true," +
            //    "        background: \"transparent\"," +
            //    "        template: \"#= category #:  #= value#\"" +
            //    "    }" +
            //    "}," +
            //    "series: [{" +
            //    "    type: \"pie\"," +
            //    "    startAngle: 0," +
            //    "    data: [{" +
            //    "        category: \"1.) Received\"," +
            //    "        value: 11," +
            //    "        color: \"#9de219\"" +
            //    "    },{" +
            //    "        category: \"2.) ForAudit\"," +
            //    "        value: 19," +
            //    "        color: \"#90cc38\"" +
            //    "    },{" +
            //    "        category: \"3.) Audited\"," +
            //    "        value: 1," +
            //    "        color: \"#068c35\"" +
            //    "    },{" +
            //    "        category: \"4.) Approved And Logout\"," +
            //    "        value: 5," +
            //    "        color: \"#006634\"" +
            //    "    },{" +
            //    "        category: \"5.) Returned\"," +
            //    "        value: 3," +
            //    "        color: \"#004d38\"" +
            //    "    }]" +
            //    "}]," +
            //    "tooltip: {" +
            //    "    visible: true," +
            //    "    format: \"{0}\"" +
            //    "} " +
            //"});";
            
            return PartialView("partial/_pie_summary_current", null);
        }
        public ActionResult Audit_monitoring_summary_read([DataSourceRequest] DataSourceRequest request,DateTime from,DateTime to)
        {
            IEnumerable<ufn_PreAudit_summary_Result> AuditList;

            AuditList = fmisdb.ufn_PreAudit_summary(from, to).OrderBy(o => o.Audited);

          
            return Json(AuditList.ToTreeDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Audit_monitoring_read([DataSourceRequest] DataSourceRequest request,DateTime from,DateTime to)
        {
            IEnumerable<ufn_PreAudit_Trans_Monitoring_Result> AuditList;
            //DateTime dte = (DateTime.Now.TimeOfDay > Convert.ToDateTime("")) ? 1 : 1 / 0;
            AuditList = fmisdb.ufn_PreAudit_Trans_Monitoring(from,to,DateTime.Now);
            
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(AuditList.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }

        public ActionResult PO_monitoring_read([DataSourceRequest] DataSourceRequest request, int year)
        {
            IEnumerable<ufn_PO_Trans_Monitoring_Result> POList;
            //DateTime dte = (DateTime.Now.TimeOfDay > Convert.ToDateTime("")) ? 1 : 1 / 0;
            POList = fmisdb.ufn_PO_Trans_Monitoring(year).OrderBy(m => m.TransactionDate);

            var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };
            var result = new ContentResult();
           
            result.Content = serializer.Serialize(POList.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }

        public ActionResult Audit_List_read([DataSourceRequest] DataSourceRequest request,DateTime from,DateTime to)
        {
            IEnumerable<ufn_PreAudit_Returned_Result> AuditList;

            AuditList = fmisdb.ufn_PreAudit_Returned(from,to);

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(AuditList.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult TrackingDuration_List_read([DataSourceRequest] DataSourceRequest request, DateTime from, DateTime to,string dvno,int searchtype,int claimanttype,string claimantcode,int grid_type)
        {
            //tracking setup
            string qry = "";
            if (grid_type == 1)
            {
               qry= "execute Accounting.usp_grid_transaction_tracking '" + from + "','" + to + "','" + dvno.AntiInject() + "'," + searchtype + "," + claimanttype + ",'" + claimantcode.AntiInject() + "'";
            }
            else if (grid_type == 2)
            {
               qry= "execute Accounting.usp_grid_transaction_duration '" + from + "','" + to + "','" + dvno.AntiInject() + "'," + searchtype + "," + claimanttype + ",'" + claimantcode.AntiInject() + "'";
            }
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text,
            qry).Tables[0];

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(rec.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }

        public ActionResult grid_USAD_CBMS_mapping([DataSourceRequest] DataSourceRequest request)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["cbms2dot70"].ToString(), System.Data.CommandType.Text,
            "select * from tbl_USAD_CBMS_ForMapping where UsadID not in (select enrolleid from [192.168.2.1\\PGAS].[Usad].[dbo].[tbl_USAD_CMBS_ID])").Tables[0];
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult returned_liquidation_List_read([DataSourceRequest] DataSourceRequest request, DateTime from, DateTime to)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text,
            "execute Accounting.usp_data_returned_liquidation '" + from + "','" + to + "'").Tables[0];
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_claimant_name_byClamantmode(int claimantcode)
        {
            DataTable rec = ISfn.ExecuteDataset("execute Accounting.usp_get_claimant_byClaimantcode " + claimantcode + "");
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult mapped_cbms_usad(Int64 cbmsid,Int64 usadid, Int64 munid)
        {            
            OleDbHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["cbms2dot70"].ToString(), System.Data.CommandType.Text,
            "exec usp_CBMS2015_save_to_USAD @resid  = "+cbmsid+",@munid  = "+munid+",@enrolleid  = "+usadid+"");
            return Content("6");
        }


        //DVNUMBERING
        public ActionResult data_grid_DV_doc([DataSourceRequest] DataSourceRequest request, int doc_form_id)
        {
            DataTable rec = new DataTable();
            string cmdStr = "execute [Accounting].[usp_grid_attach_doc] " + doc_form_id + "";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
    }
}