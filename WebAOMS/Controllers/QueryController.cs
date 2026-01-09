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
    [Authorize(Roles = "Accountant, Admin")]
    public class QueryController : Controller
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        fmisEntities fmisdb = new fmisEntities();
        // GET: Query
        public ActionResult Index_AP()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a65";
            return View();
        }
        [AllowAnonymous]
    public ActionResult Payroll_tracking_history()
        {
            
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a75";
            return View();
        }
        public ActionResult registry_of_Appropriation_Allotment_Obligation_Utilization()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a66";
            return View();
        }
        public ActionResult bir_remittance_list_by_year()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a70";
            return View();
        }
        public JsonResult get_criteria()
        {
            DataTable rec = ISfn.ExecuteDataset("select * from [Accounting].[ufn_Query_Criteria]()");
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_criteria_raaou()
        {
            DataTable rec = ISfn.ExecuteDataset("select * from [Accounting].[ufn_Query_Criteria_raaou]()");
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_criteria_bir_remit()
        {
            DataTable rec = ISfn.ExecuteDataset("select * from [Accounting].[ufn_Query_Criteria_bir_remit]()");
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Result_Criteria_read([DataSourceRequest] DataSourceRequest request, DateTime to, int ID)
        {
            string qry = "";

            qry = "execute [Accounting].[usp_Query_Criteria] '1/31/2019','" + to + "'," + ID + "";

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
        public ActionResult Result_Criteria_raaou_read([DataSourceRequest] DataSourceRequest request, DateTime to, int ID,bool isreset)
        {
            //string qry = "",refno,filename ;
            //DataSet dt = new DataSet();
            //refno = "Q_RAAO" + ID.ToString("000") + to.ToString("yyyyMM");
            //filename = ISfn.xmlpath(20) + refno;
            //bool ifexists =false;
            //ifexists = System.IO.File.Exists(ISfn.xmlpath(20) + refno);

            //if (ifexists == true && Convert.ToInt16(to.ToString("yyyy")) < 2020 && isreset == false)
            //{
            //    dt.ReadXml(filename);
            //}
            //else
            //{
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute [Accounting].[usp_Query_Criteria_raaou] '1/31/2019','" + to + "'," + ID + "";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.CommandTimeout = 0;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

            //    dt.WriteXml(filename, XmlWriteMode.WriteSchema);
            //}


            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            ////DataTable rec = dt.Tables[0];
            if (ID == 1)
            {
                IEnumerable<ufn_Query_RAAOU_summary_Result> list_summary;
                list_summary = fmisdb.ufn_Query_RAAOU_summary(to.Year, to.Month);
                result.Content = serializer.Serialize(list_summary.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
            }
            else if (ID == 2)
            {
                IEnumerable<tbl_t_RAAOUP> list;
                list = fmisdb.tbl_t_RAAOUP.Where(w => w.year == to.Year && w.month_ == to.Month);
                result.Content = serializer.Serialize(list.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
            }

            else if (ID == 3)
            {
                IEnumerable<ufn_Query_RAAOU_bySector_Result> list_sector;
                list_sector = fmisdb.ufn_Query_RAAOU_bySector(to.Month, to.Year).OrderBy(o=> o.functionID);
                result.Content = serializer.Serialize(list_sector.ToDataSourceResult(request));
                result.ContentType = "application/json";
                //File.WriteAllText(@"c:\movie.json", JsonConvert.SerializeObject(movie));

                //// serialize JSON directly to a file
                //using (StreamWriter file = File.CreateText(@"c:\movie.json"))
                //{
                //    serializer.Serialize(file, list_sector);
                //}
                return result;

                //string qry = "select * from [Accounting].[ufn_Query_RAAOU_bySector] ("+ to.Month + ","+to.Year+")";

                //DataTable rec;
                //rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text,
                //qry).Tables[0];
                //result.Content = serializer.Serialize(list_sector.ToDataSourceResult(request));
                //result.ContentType = "application/json";
                //return result;
            }
           
            else
            {
                result.ContentType = "application/json";
                return result;
            }
            
        }
        
        public ActionResult Result_Criteria_bir_remit_read([DataSourceRequest] DataSourceRequest request, DateTime to,int criteria)
        {

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            string qry = "execute [Accounting].[usp_Query_bir_remittance_list]  " + to.Year + ","+criteria+"";

            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text,
            qry).Tables[0];
            result.Content = serializer.Serialize(rec.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;

        }
        [AllowAnonymous]
        public ActionResult Result_Criteria_pyroll_tracking_log_read([DataSourceRequest] DataSourceRequest request, int year, int month)
        {
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            string qry = "execute Accounting.usp_Payroll_budget_ATMdownload_log "+year+","+month+"";
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text,
            qry).Tables[0];
            result.Content = serializer.Serialize(rec.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult get_raauo_url(int fundid, DateTime to ,int rptid = 32)
        {
            int hideAccount=0;int hideOOOE=0;
            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute [Accounting].[usp_Query_Criteria_raaou] '1/31/2019','" + to + "',4";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.CommandTimeout = 0;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            if (rptid==34)
            {
                hideAccount = 1;
                rptid = 33;
            }
            else if (rptid == 35)
            {
                hideOOOE = 1;
                hideAccount = 1;
                rptid = 33;
            }
            else if (rptid == 37)
            {
                hideAccount = 1;
                rptid = 36;
            }
            else if (rptid == 38)
            {
                hideOOOE = 1;
                hideAccount = 1;
                rptid = 36;
            }

            else if (rptid == 40)
            {
                hideAccount = 1;
                rptid = 39;
            }
            else if (rptid == 41)
            {
                hideOOOE = 1;
                hideAccount = 1;
                rptid = 39;
            }
            else if (rptid == 43)
            {
                hideAccount = 1;
                rptid = 42;
            }
            else if (rptid == 44)
            {
                hideOOOE = 1;
                hideAccount = 1;
                rptid = 42;
            }


            string strUrl;
            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + rptid + "&fundid=" + fundid + "&to=" + to + "&hideAccountname="+ hideAccount + "&hideOOEname="+ hideOOOE + "").ToString();
            return Content(strUrl);
        }
        public ActionResult print_raauo(int fundid,DateTime to, int rptid = 32)
        {
            
            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute [Accounting].[usp_Query_Criteria_raaou] '1/31/2019','" + to + "',4";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.CommandTimeout = 0;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            string strUrl;
            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + rptid + "&fundid=" + fundid + "&to="+to+"").ToString();
            return JavaScript("window.open('" + strUrl + "')");
        }

        [HttpPost]
        public ActionResult Excel_Export_Save_RAAOUP(string contentType, string base64, string fileName)
        {
            fileName = DateTime.Today.ToString("MMyyyy") + fileName + "";
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        #region Expenses per office
        [AllowAnonymous]
        public ActionResult index_expenses() {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a71";
            ViewBag.date = DateTime.Now.ToShortDateString();
            return View();
        }
        [AllowAnonymous]
        public ActionResult index_expenses_details(int year,string  childcode,int rcenterid ,string accountname)
        {
            WebAOMS.Base.USERMENU_public.SetUserMenu_public();
            ViewBag.year = year;
            ViewBag.childcode = childcode;
            ViewBag.accountname = accountname;
            ViewBag.rcenterid = rcenterid;
            return PartialView();
        }
        [AllowAnonymous]

        public JsonResult dropdown_get_expenses()
        {
            IEnumerable<ufn_chartOfAccounts_expenses_Result> office;
            office = fmisdb.ufn_chartOfAccounts_expenses().OrderBy(o => o.AccountName);
            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult Result_expenses_byyear_read([DataSourceRequest] DataSourceRequest request, DateTime to, bool isReset,string childcode)
        {
            //try
            //{
            //if (isReset == true && USER.C_swipeID == "8500")
            //{
            //    SqlConnection connection = new SqlConnection(fmisConn);
            //    string cmdStr = "exec  [Accounting].[usp_Oblig_recreate_RAAU_byOffice1] " + to.Year + "," + to.Month + "," + officeid + "";
            //    using (SqlCommand command = new SqlCommand(cmdStr, connection))
            //    {
            //        command.CommandTimeout = 0;
            //        connection.Open();
            //        command.ExecuteNonQuery();
            //    }
            //}

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;

            if (childcode == "50299020")
            {
                IEnumerable<ufn_expenses_byyear_printing_publication_Result> list_expenses;
                list_expenses = fmisdb.ufn_expenses_byyear_printing_publication(to.Year);
                result.Content = serializer.Serialize(list_expenses.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
            }
            else
            {
                IEnumerable<ufn_expenses_byyear_cbms_Result> list_expenses_cbms;
                list_expenses_cbms = fmisdb.ufn_expenses_byyear_cbms(to.Year);
                result.Content = serializer.Serialize(list_expenses_cbms.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
            }

        }
        [AllowAnonymous]
        public ActionResult Result_expenses_byyear_summary_read([DataSourceRequest] DataSourceRequest request, DateTime to, bool isReset, string childcode)
        {
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;

            if (childcode == "50299020")
            {
                IEnumerable<ufn_expenses_byyear_printing_publication_summary_Result> list_expenses;
                list_expenses = fmisdb.ufn_expenses_byyear_printing_publication_summary(to.Year);
                result.Content = serializer.Serialize(list_expenses.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
            }
            else
            {
                IEnumerable<ufn_expenses_byyear_cbms_summary_Result> list_expenses_cbms;
                list_expenses_cbms = fmisdb.ufn_expenses_byyear_cbms_summary(to.Year);
                result.Content = serializer.Serialize(list_expenses_cbms.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
            }

        }
        [AllowAnonymous]
        public ActionResult _expenses_details_byoffice([DataSourceRequest] DataSourceRequest request, int year, string childcode, int rcenterid,string accountname)
        {
           
            //var serializer = new JavaScriptSerializer();
            //var result = new ContentResult();
            //serializer.MaxJsonLength = Int32.MaxValue;

            //IEnumerable<ufn_expenses_byyear_details_Result> list_expenses;
            //list_expenses = fmisdb.ufn_expenses_byyear_details(year,rcenterid, childcode);
            //result.Content = serializer.Serialize(list_expenses.ToDataSourceResult(request));
            //result.ContentType = "application/json";
            //return result;

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;

            if (childcode == "50299020")
            {
                IEnumerable<ufn_expenses_byyear_details_printing_publication_Result> list_expenses;
                list_expenses = fmisdb.ufn_expenses_byyear_details_printing_publication(year, rcenterid, accountname);
                result.Content = serializer.Serialize(list_expenses.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
            }
            else
            {
                IEnumerable<ufn_expenses_byyear_details_cbms_Result> list_expenses_cbms;
                list_expenses_cbms = fmisdb.ufn_expenses_byyear_details_cbms(year, rcenterid, accountname);
                result.Content = serializer.Serialize(list_expenses_cbms.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
            }
        }
        #endregion

    }
}