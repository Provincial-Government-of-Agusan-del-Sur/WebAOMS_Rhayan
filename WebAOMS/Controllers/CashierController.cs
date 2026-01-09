using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
using Microsoft.AspNet.Identity;

namespace WebAOMS.Controllers
{

    [Authorize]
    public class CashierController : Controller
    {
        // GET: Cashier
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        fmisEntities fmisdb = new fmisEntities();

        public ActionResult Cashbook()
        {
            ViewBag.rightSidebar_title = "TOMS";
            return View();
        }
        public JsonResult DataSource_Getfundtype()
        {
            IEnumerable<ufn_fundtype_Result> fundtype;
            fundtype = fmisdb.ufn_fundtype().OrderBy(o => o.FundName);
            return Json(fundtype.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult grid_Accounts([DataSourceRequest] DataSourceRequest request, int fundid, int year, int month)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from Treasury.[ufn_CashBook_Accounts](" + fundid + "," + year + "," + month + ") order by BankAccountNo";
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
        public ActionResult grid_CashBook_details([DataSourceRequest] DataSourceRequest request, int compositionCode, int year, int month, int isclosed, int byYear)
        {
            DataTable rec = new DataTable();
            string cmdStr = "execute [Treasury].[usp_CashBook] " + compositionCode + "," + month + "," + year + "," + isclosed + "," + byYear + "";
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
        public ActionResult view_cashbook_details(Int64 trnno)
        {
            string cmdStr = "execute [Treasury].[usp_CashBook_Details]  @trnno";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@trnno", trnno);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ViewBag.trnno = trnno;
                        ViewBag.transactiondate = reader["TransactionDate"];
                        ViewBag.recordid = reader["RecordID"];
                        ViewBag.particular = reader["Particular"];
                        ViewBag.checkno = reader["ChkNumber"];
                        ViewBag.compositioncode = reader["CompositionCode"];
                        ViewBag.dvno = reader["DVNO"];
                        ViewBag.ptvno = reader["PTVNO"];
                        ViewBag.fundcode = reader["FundCode"];
                        ViewBag.debitcredit = reader["DebitCredit"];
                        ViewBag.amount = reader["Amount"];
                        ViewBag.rcino = reader["RCINo"];
                        ViewBag.claimantname = reader["ClaimantName"];
                    }
                }
                else
                {
                    ViewBag.trnno = 0;
                    ViewBag.transactiondate = "";
                    ViewBag.recordid = "";
                    ViewBag.particular = "";
                    ViewBag.chknumber = "";
                    ViewBag.dvno = "";
                    ViewBag.ptvno = "";
                    ViewBag.compositioncode = "";
                    ViewBag.fundcode = "";
                    ViewBag.debitcredit = "";
                    ViewBag.amount = "";
                    ViewBag.rcino = "";

                }
            }
            connection.Close();
            return View("_Cashbook_details", null);
        }
        public ActionResult _pv_reamkarkdetails(Int64 trnno)
        {
            return View("_approval_remarks");
        }
        [Authorize(Roles = "CashierAdmin")]
        public ActionResult SaveRemarks(Int32 trnno, string remark, Byte isApprove)
        {


            string cmdStr = "Update dbo.tblCMS_CDCheckBook_Audit_logs set isActive = 0 where trnno = " + trnno + "";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            using (fmisEntities context = new fmisEntities())
            {
                tblCMS_CDCheckBook_Audit_logs saveApprove = new tblCMS_CDCheckBook_Audit_logs();

                saveApprove.trnno = trnno;

                saveApprove.UserID = User.Identity.GetUserId();
                saveApprove.status = isApprove;
                saveApprove.Remarks = remark;
                saveApprove.isActive = true;
                saveApprove.dte = System.DateTime.Now;
                context.tblCMS_CDCheckBook_Audit_logs.Add(saveApprove);
                context.SaveChanges();
            }
            return Json(new { code = 6, statusName = "Successfully Save..!" });
              
        }
    }
}