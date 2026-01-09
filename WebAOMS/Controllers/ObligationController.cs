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
using WebAOMS.ws_tracking;
using WebAOMS.epsws;
using Microsoft.AspNet.Identity;
using WebAOMS.Mod;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace WebAOMS.Controllers
{
    public class ObligationController : Controller
    {

        TrackingSoapClient ws = new TrackingSoapClient();
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        string dbcon_fmis = ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString();
        fmisEntities fmisdb = new fmisEntities();
        // GET: Document
        [Authorize(Roles = "Admin,Obligation")]
        public ActionResult Transaction_control_list()
        {
            ViewBag.Title_mini = "Status of Appropriation, Allotment, Obligation and Utilization";
            ViewBag.Title = "Summary of RAAOD";
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a69";
            ViewBag.date = DateTime.Now.ToShortDateString();
            return View();
        }
        public ActionResult Result_RAAU_read([DataSourceRequest] DataSourceRequest request, DateTime to,int officeid, bool isReset)
        {
            try
            {
                if (isReset == true && USER.C_swipeID == "8500")
                {
                    SqlConnection connection = new SqlConnection(fmisConn);
                    string cmdStr = "exec  [Accounting].[usp_Oblig_recreate_RAAU_byOffice1] " + to.Year + "," + to.Month + "," + officeid + "";
                    using (SqlCommand command = new SqlCommand(cmdStr, connection))
                    {
                        command.CommandTimeout = 0;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                var serializer = new JavaScriptSerializer();
                var result = new ContentResult();
                serializer.MaxJsonLength = Int32.MaxValue;

                IEnumerable<ufn_Oblig_grid_RAAU_Result> list_sector;
                list_sector = fmisdb.ufn_Oblig_grid_RAAU(to.Month, to.Year, officeid);
                result.Content = serializer.Serialize(list_sector.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
            }
            catch (Exception e)
            {
                var serializer = new JavaScriptSerializer();
                var result = new ContentResult();
                serializer.MaxJsonLength = Int32.MaxValue;

                IEnumerable<ufn_Oblig_grid_RAAU_Result> list_sector;
                list_sector = fmisdb.ufn_Oblig_grid_RAAU(to.Month, to.Year, officeid);
                result.Content = serializer.Serialize(list_sector.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
            }
        }

        public ActionResult Result_oblig_transaction_read([DataSourceRequest] DataSourceRequest request, DateTime to, int officeid)
        {
                var serializer = new JavaScriptSerializer();
                var result = new ContentResult();
                serializer.MaxJsonLength = Int32.MaxValue;

                IEnumerable<ufn_oblig_transaction_grid_Result> list_sector;
                list_sector = fmisdb.ufn_oblig_transaction_grid(to.Year,to.Month,officeid);
                result.Content = serializer.Serialize(list_sector.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
        }
        public JsonResult DataSource_GetOffice_fmis(int year)
        {
            IEnumerable<ufn_fmis_officename_byYear_Result> office;
            office = fmisdb.ufn_fmis_officename_byYear(year).OrderBy(o => o.OfficeName);
            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_remainingAmount(int year, int officeid ,string officeProgramAccountid ,int obligID)
        {
            decimal amount = 0;
            string cmdStr = "select * from  [Accounting].[ufn_Oblig_RemainingAmount] (@year ,@officeid ,@officeProgramAccountid ,@obligID)";
            SqlConnection connection = new SqlConnection(fmisConn);
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@obligID", SqlDbType.Int).Value = obligID;
                command.Parameters.Add("@year", SqlDbType.Int).Value = year;
                command.Parameters.Add("@officeProgramAccountid", SqlDbType.VarChar,20).Value = officeProgramAccountid;
                command.Parameters.Add("@officeid", SqlDbType.Int).Value = officeid;
                
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                       amount = Convert.ToDecimal(reader["RAmount"]);
                    }
                }
                else
                {
                    amount = 0;
                   
                }
            }
            return Json(new { RAmount = amount });
        }
        public static string stripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>\\", string.Empty);
        }

        public ActionResult _Add_obligation(Int64 obligID)
        {
            string cmdStr = "execute  [Accounting].[usp_oblig_get_details] @obligID";
                SqlConnection connection = new SqlConnection(fmisConn);
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@obligID", SqlDbType.Int).Value = obligID;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            ViewBag.obligid = reader["obligID"];
                            ViewBag.refno = reader["refno"];
                            ViewBag.transactiondate = reader["TransactionDate"];
                            ViewBag.particular = reader["Particular"];
                            ViewBag.claimantcode = reader["Claimantcode"];
                            ViewBag.transtype_id = reader["transtype_id"];
                            ViewBag.obrno = reader["Obrno"];
                            ViewBag.officeID = reader["officeID"];
                            ViewBag.programid = reader["programID"];
                            ViewBag.accountid = reader["accountID"];
                            ViewBag.fundid = reader["fundID"];
                            ViewBag.userid = reader["UserID"];
                            ViewBag.dte = reader["DTE"];
                            ViewBag.isdeleted = reader["isDeleted"];
                            ViewBag.countperson = reader["countperson"];
                            ViewBag.Name = reader["Name"];
                            ViewBag.officeProgramAccountID = reader["officeProgramAccountID"].ToString().StringToList();
                            ViewBag.ooe = reader["ooe"];
                            ViewBag.isEdit = 1;
                        }
                    }
                    else
                    {
                        ViewBag.obligid = 0;
                        ViewBag.refno = "";
                        ViewBag.transactiondate = "";
                        ViewBag.particular = "";
                        ViewBag.claimantcode = "";
                        ViewBag.transtype_id = "";
                        ViewBag.obrno = "";
                        ViewBag.officeID = 0;
                        ViewBag.programid = 0;
                        ViewBag.accountid = 0;
                        ViewBag.fundid = "";
                        ViewBag.userid = "";
                        ViewBag.dte = "";
                        ViewBag.isdeleted = "";
                        ViewBag.Name ="";
                        ViewBag.countperson = "";
                        ViewBag.ooe = "";
                        ViewBag.officeProgramAccountID = "";
                        ViewBag.isEdit = 0;
                    }
                }
                connection.Close();
            return PartialView("_Add_Oblig_new", null);
        }

        public ActionResult delete_oblig_transaction(int obligid, string remark)
        {
            try
            {
                Int32 userid = Convert.ToInt32(USER.C_swipeID);
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute [Accounting].[usp_delete_oblig_transaction] @obligid,@remark,@userid";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@obligid", SqlDbType.Int).Value = obligid;
                    command.Parameters.Add("@userid", SqlDbType.Int).Value = userid;
                    command.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = remark;
                    connection.Open();
                    int rows = command.ExecuteNonQuery();
                    connection.Close();
                    return Json(new { code = 6, statusName = "Successfully Deleted..!" });
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    code = e.HResult,
                    statusName = e.Message
                });
            }
        }
        public JsonResult get_oblig_selected_accnt(int obligid)
        {
            DataTable rec;
            string officeProgramAccountID = "";
            if (obligid > 0)
            {
                rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text,
                "select officeProgramAccountID  from  Accounting.tbl_t_Oblig_Transaction where obligid = " + obligid + "").Tables[0];
                if (rec.Rows.Count > 0)
                {
                    officeProgramAccountID = rec.Rows[0]["officeProgramAccountID"].ToString();
                }
                rec.Dispose();
            }

            var result = new { Result = officeProgramAccountID.StringToList()};
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DataSource_officeProgram(Int16 officeid,int year)
        {
            IEnumerable<fn_Oblig_Accountname_Result> office;
            office = fmisdb.fn_Oblig_Accountname(officeid,year).OrderBy(o => o.officeProgramAccount_id);

            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult print_cafoa_SAAO(int document_type_id, int obligid)
        {
            string strUrl;
            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + document_type_id + "&obligid=" + obligid + "").ToString();



            //r = ISfn.ExecScalar("SELECT transtype_id FROM [fmis].[Accounting].[tbl_t_DV_Details] where DVid = " + id + "").ToString();
            //if (r == "13" || r == "15" || r == "14" || r == "19" || r == "8")
            //{
            //    dvstr = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=13&dvid=" + id + "&refno=" + refno + "").ToString();
            //}

            //else
            //{
            //    dvstr = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=11&dvid=" + id + "&refno=" + refno + "").ToString();
            //}

            return JavaScript("window.open('" + strUrl + "')");

        }
        public ActionResult grid_expenses([DataSourceRequest] DataSourceRequest request, int obligID)
        {
            IEnumerable<ufn_Oblig_Expense_grid_Result> rec;
            rec = fmisdb.ufn_Oblig_Expense_grid(obligID);
            return Json(rec.ToDataSourceResult(request, o => new { AccountName = o.AccountName, ChartAccountID = o.ChartAccountID, oblig_expense_detail_id = o.oblig_expense_detail_id, Amount = o.Amount, Code = o.Code }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult _oblig_expense_details(Int32 obligid)
        {
            return View("_Add_Oblig_entries");
        }
        public ActionResult insert_oblig_expense(Int32 oblig_expense_detail_id, Int32 obligid, Int32 ChartAccountID, Int32 functionid, string Amount, string officeProgramAccountid )
        {

            int userid = Convert.ToInt32(USER.C_swipeID);
            if ((officeProgramAccountid == null || officeProgramAccountid == "")  & oblig_expense_detail_id == 0)
            {

                return Json(new { code = 7, statusName = "The Program/Account is required" });
            }
            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute [Accounting].[usp_Save_Oblig_expense_details] @oblig_expense_detail_id ,@obligid ,@ChartAccountID  ,@functionid , @Amount ,@userid, @officeProgramAccountid";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@oblig_expense_detail_id", oblig_expense_detail_id);
                command.Parameters.AddWithValue("@obligid", obligid);
                command.Parameters.AddWithValue("@ChartAccountID", ChartAccountID);
                command.Parameters.AddWithValue("@functionid", functionid);
                command.Parameters.AddWithValue("@Amount", Amount);
                command.Parameters.AddWithValue("@userid", userid);
                command.Parameters.AddWithValue("@officeProgramAccountid", officeProgramAccountid);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return Json(new { code = 6, statusName = "Successfully Saved..!" });
            }
        }

        public ActionResult save_oblig(tbl_t_Oblig_Transaction data)
        {
            int userid = Convert.ToInt32(USER.C_swipeID);
            Int32 obligID = 0;

            if (data.Particular == null)
            {
                return Json(new { code = 7, statusName = "The Particular is required" });
            }
            //if (data.officeProgramAccountID == null)
            //{
            //    return Json(new { code = 7, statusName = "Program/Accountname is required" });
            //}
            if (data.officeID == null)
            {
                return Json(new { code = 7, statusName = "Office charge is required" });
            }
            if (data.ooe == null)
            {
                return Json(new { code = 7, statusName = "The allotment class is required" });
            }

            if (data.officeID < 1)
            {
                return Json(new { code = 7, statusName = "Invalid Responsibility center" });
            }

            if (data.transtype_id < 1)
            {
                return Json(new { code = 7, statusName = "Invalid Transaction Type" });
            }

            data.Claimantcode = data.Claimantcode ?? "";

            data.fundID = data.fundID ?? 0;
            if (data.fundID < 2)
            {
                return Json(new { code = 7, statusName = "Fundtype Required" });
            }


            if (data.countperson == null)
            {
                data.countperson = 1;
            }

            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute [Accounting].[usp_Save_Oblig_transaction] @obligID  ,@Particular   ,@Claimantcode   ,@transtype_id  ,@officeprogramaccountid,@officeID ,@fundID ,@UserID ,@countperson,@ooe";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@obligid", data.obligID);
                command.Parameters.AddWithValue("@transtype_id", data.transtype_id);
                command.Parameters.AddWithValue("@particular", data.Particular);
                command.Parameters.AddWithValue("@claimantcode", data.Claimantcode);
                command.Parameters.AddWithValue("@countperson", data.countperson);
                command.Parameters.AddWithValue("@fundid", data.fundID);
                command.Parameters.AddWithValue("@ooe", data.ooe);
                
                command.Parameters.AddWithValue("@officeID", data.officeID);
                command.Parameters.AddWithValue("@officeprogramaccountid", "");
                
                command.Parameters.AddWithValue("@userid", USER.C_eID);
                
               
                connection.Open();
                SqlDataReader read = command.ExecuteReader();
                if (read.HasRows == true)
                {
                    while (read.Read())
                    {
                        obligID = Convert.ToInt32(read["obligID"]);
                    }
                }

                connection.Close();
                return Json(new { code = 6, statusName = "Successfully Saved..!", obligid = obligID });
            }
        }
    }
}