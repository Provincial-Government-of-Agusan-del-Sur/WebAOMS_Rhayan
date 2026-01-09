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
    public class COAController : Controller
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        fmisEntities fmisdb = new fmisEntities();
        // GET: COA
        public ActionResult AuditRCI()
        {
            return View();
        }
        public ActionResult grid_RCI_details([DataSourceRequest] DataSourceRequest request, string rcino)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_COA_cdj_getRCI_details]('" + rcino.AntiInject() + "') order by OrderNo";
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


        public ActionResult grid_RCI_transaction([DataSourceRequest] DataSourceRequest request, int fundid, int year, int month)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_COA_cdj_getRCI](" + fundid + "," + year + "," + month + ") order by RCINo";
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

        //public JsonResult Index([DataSourceRequest] DataSourceRequest request, int? id)
        //{
        //    var result = fmisConn..ToTreeDataSourceResult(request,
        //        e => e.EmployeeId,
        //        e => e.ReportsTo,
        //        e => id.HasValue ? e.ReportsTo == id : e.ReportsTo == null,
        //        e => e
        //    );All

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult All([DataSourceRequest] DataSourceRequest request, Int32 jevid)
        {

            var rec = fmisdb.ufn_COA_get_jev_byjevid(jevid).OrderByDescending(o => o.debit).ToTreeDataSourceResult(request,
                e => e.ChartAccountChildID,
                e => e.AccountChildParentID,
                e => e
            );

            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        public ActionResult view_jev_numbering(string dvno)
        {
            string nacode = "";

            if (dvno.Length < 5)
            {
                return Json(new { code = 5, statusName = "Invalid DVNO...!" });
            }
            string cmdStr = "Execute [Accounting].[usp_JEV_Get_Details] @dvno";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@dvno", dvno);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {

                    while (reader.Read())
                    {
                        ViewBag.jevid = reader["jevid"];
                        ViewBag.Obrno = reader["Obrno"];
                        ViewBag.Particular = reader["Particular"];
                        ViewBag.Gamount = reader["Gamount"];
                        ViewBag.Name = reader["Name"];
                        ViewBag.Jevdate = reader["Jevdate"];
                        ViewBag.rci = reader["RCI"];
                        ViewBag.checkno = reader["Checkno"];
                        ViewBag.ptvno = reader["ptvno"];
                        ViewBag.date_ = Convert.ToDateTime(reader["Date_"]).ToString("MM/dd/yyyy");
                        ViewBag.FmisVoucherno = reader["FmisVoucherno"];
                        ViewBag.Claimantcode = reader["Claimantcode"];
                        nacode = reader["nacode"].ToString();
                        ViewBag.RCenter = reader["RCenter"];
                        ViewBag.dvno = dvno;
                        ViewBag.Rdono = reader["Rdono"];
                        ViewBag.fundID = reader["fundID"];
                        ViewBag.NetAmount = reader["NetAmount"];
                        ViewBag.journalTypeID = reader["Transtype"];
                        ViewBag.JEVno = reader["JEVno"].ToString().Substring(0, 13);
                        ViewBag.jevseries = reader["JEVno"].ToString().Substring(13, 4);
                        ViewBag.isEdit = reader["isEdit"];
                        ViewBag.isApproved = 0;
                    }

                }
                else
                {
                    return Json(new { code = 5, statusName = "The DV number not found in the database..!" });
                }

                connection.Close();
            }
            return View("_JEV_details_entries", null);
        }
        public ActionResult _pv_reamkarkdetails(string jevid)
        {
            return View("_JEV_edit_remark");
        }

        public ActionResult SaveRemarks(int jevid, string remark, Byte isApprove)
        {
            string cmdStr = "Update Accounting.tbl_t_JEV_Audit_logs set isActive = 0 where jevid = "+jevid+"";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                    
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
                
            using (fmisEntities context = new fmisEntities())
            {
                tbl_t_JEV_Audit_logs saveApprove = new tbl_t_JEV_Audit_logs();

                saveApprove.jevid = jevid;

                saveApprove.UserID = User.Identity.GetUserId(); ;
                saveApprove.status = isApprove;
                saveApprove.Remarks = remark;
                saveApprove.isActive = true;
                saveApprove.dte = System.DateTime.Now;
                context.tbl_t_JEV_Audit_logs.Add(saveApprove);
                context.SaveChanges();
            }
            return Json(new { code = 6, statusName = "Successfully Save..!" });
            
        }
    }
}