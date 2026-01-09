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
using WebAOMS.AES;
namespace WebAOMS.Controllers
{
    [Authorize]
    public class PayrollController : Controller
    {
        TrackingSoapClient ws = new TrackingSoapClient();
        pmisEntities pmisdb = new pmisEntities();
        // GET: Payroll
        
        #region special payroll
        public ActionResult Special_payroll_index()
        {
            ViewBag.menuid = "a73";
            return View();

        }

        public JsonResult get_batchDetails(int batchno)
        {
            DataTable rec;
            string employmentStatusArray = "0";
            int CompensationID = 0;
            string Pperiod = "";
            if (batchno > 0)
            {
                rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), System.Data.CommandType.Text,
                "SELECT [CompensationID],left(yearMonth,len([yearMonth]) -4) + '/16/' + right(yearMonth,4) as PPeriod,yearMonth,[employmentStatusArray],[employmentStatusText],[status_id],[CreateDate],[UserID] FROM [pmis].[epay].[tbl_t_Special_Payroll_batch] where t_compensation_batchID = " + batchno + "").Tables[0];
                if (rec.Rows.Count > 0)
                {
                    employmentStatusArray = rec.Rows[0]["employmentStatusArray"].ToString();
                    CompensationID = Convert.ToInt32(rec.Rows[0]["CompensationID"]);
                    Pperiod = rec.Rows[0]["PPeriod"].ToString();

                }
                rec.Dispose();
            }

            var result = new { Result = employmentStatusArray.StringToListInt(), CompensationID = CompensationID,PPeriod= Pperiod };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_column_datatype(int col_id)
        {
            DataTable rec;
            int datatype_id = 0;
            if (col_id > 0)
            {
                rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), System.Data.CommandType.Text,
                "SELECT Datatype FROM [pmis].[epay].[tbl_l_Compensation_Column] where col_id = "+col_id+"").Tables[0];
                if (rec.Rows.Count > 0)
                {
                    datatype_id = Convert.ToInt32(rec.Rows[0]["Datatype"]);
                }
                rec.Dispose();
            }
            var result = new { datatype_id = datatype_id };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult delete_employee_deduction_special(int special_payroll_deductionID)
        {
            try
            {
                ISfn.ExcecuteNoneQuery("execute pmis.epay.[usp_delete_employee_deduction_special] @special_payroll_deductionID= " + special_payroll_deductionID + ",@userid = " + USER.C_eID + "");
                return Content("6");
            }
            catch (Exception e)
            {
                string r = ISfn.errorLog(e.HResult, e.Message, "RegularPayroll/delete_employee_deduction_special");
                return Content(r);
            }
        }
        public ActionResult LoadGrid_Deduction_special([DataSourceRequest] DataSourceRequest request, int eid, int batchno)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), System.Data.CommandType.Text,
           "execute pmis.Epay.[usp_ref_Employee_Deduction_special_grid] " + eid + "," + batchno + "").Tables[0];
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadGrid_Income_special([DataSourceRequest] DataSourceRequest request, int eid, int batchno)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), System.Data.CommandType.Text,
           "execute pmis.[epay].[usp_ref_Employee_Income_special_grid] " + eid + "," + batchno + "").Tables[0];
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult get_netAount_special([DataSourceRequest] DataSourceRequest request, int eid, int batchno)
        {
            string amount;
            amount = ISfn.ExecScalar("select pmis.Epay.fn_getNetAmount_special(" + eid + "," + batchno + ")");
            return Content(amount);
        }
        public JsonResult get_ref_CompensationName()
        {
            DataTable rec = ISfn.ExecuteDataset("execute pmis.[epay].[usp_ref_CompensationName_byuser] "+USER.C_swipeID+"");
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_ref_CompensationColName(int compensationID)
        {
            DataTable rec = ISfn.ExecuteDataset("execute  pmis.[epay].[usp_ref_CompensationCol]  " + compensationID + "");
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _compensation_birthday()
        {
            DataTable recbatchno;
            recbatchno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), System.Data.CommandType.Text,
            "execute pmis.[epay].usp_get_atm_batchno " + USER.C_eID + "").Tables[0];
            if (recbatchno.Rows.Count > 0)
            {
                ViewBag.ATMBatchNo = recbatchno.Rows[0]["batchno"].ToString();
            }
            recbatchno.Dispose();
            return PartialView("_grid_special_payroll", null);
        }

        public ActionResult _employee_list_toadd(int compensationID, int year, int month)
        {
            //DataTable recbatchno

            //recbatchno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), System.Data.CommandType.Text,
            //"execute [epay].usp_get_atm_batchno " + USER.C_eID + "").Tables[0];
            //if (recbatchno.Rows.Count > 0)
            //{
            //    ViewBag.ATMBatchNo = recbatchno.Rows[0]["batchno"].ToString();
            //}
            //recbatchno.Dispose();
            ViewBag.reffno = (USER.C_eID.ToString() + '-' + compensationID.ToString() + '-' + year.ToString() + month.ToString()).Encrypt();
            return PartialView("_employee_list_toadd", null);
        }
        public ActionResult get_sp_batchno(int CompensationID, int yearMonth, string employmentStatusArray, string employmentStatusText, int status_id)
        {
            TrackingController track = new TrackingController();
            DataTable recbatchno;
            string batchno = "";
            string u_refno = "";
            if (employmentStatusArray.Contains("9999") & employmentStatusArray.Length > 4)
            {
                return Content("7");
            }
            recbatchno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), System.Data.CommandType.Text,
            "exec [epay].[usp_Get_batchNumber] " + CompensationID + "," + yearMonth + ",'" + employmentStatusArray + "','" + employmentStatusText + "',1," + USER.C_eID + "").Tables[0];
            if (recbatchno.Rows.Count > 0)
            {
                batchno = recbatchno.Rows[0]["t_compensation_batchID"].ToString();
                u_refno = track.get_unique_refno(batchno,50, "E3G7", "Special Payroll with batchno " + batchno.ToString(),"");
                ws.save_status_log(u_refno,311,"",USER.C_eID);
            }

            recbatchno.Dispose();
            return Content(batchno);
        }

        public ActionResult LoadGrid_Compensation_list_toAdd([DataSourceRequest] DataSourceRequest request, int batchno, int officeID, int col_id)
        {
            DataTable rec= new DataTable();
            //rec = OleDbHelper.ExecuteDataset(, System.Data.CommandType.Text,
            //"exec [epay].[usp_GetRegularEmployeeByUserID_forCompensation] @userid,@batchno,@officeid,@col_id").Tables[0];
            //rec.ToModel();

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["fmisconn"].ToString());
            string cmdStr = "execute pmis.[epay].[usp_GetRegularEmployeeByUserID_forCompensation] @userid,@batchno,@officeid,@col_id";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@userid", USER.C_eID);
                command.Parameters.AddWithValue("@batchno", batchno);
                command.Parameters.AddWithValue("@officeid", officeID);
                command.Parameters.AddWithValue("@col_id", col_id);
     
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                
                da.Fill(rec);
                connection.Close();
            }
                return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadGrid_takehome_employee([DataSourceRequest] DataSourceRequest request, int year, int month, int officeID)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), System.Data.CommandType.Text,
            "exec [epay].[usp_GetRegularEmployeeByUserID_forNetPay]  " + USER.C_eID + "," + officeID + "," + year + "," + month + "").Tables[0];
            rec.ToModel();
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEmployeetoList_compensation(string eidArray, int batchno, int col_id)
        {
            var yM = ',' + batchno.ToString() + ',' + USER.C_eID.ToString();
            string val;
            //val = eidArray.Replace(",##", yM).Replace(",=","").formatNumber();
            ISfn.ExcecuteNoneQuery("execute pmis.epay.usp_special_payroll_addemp_new '" + eidArray + "'," + batchno + "," + col_id + "," + USER.C_eID + "");
            return Content("6");
        }
        public ActionResult PopulateEmployeetoList_compensation(int batchno, int amountType, int col_id, string value)
        {
            ISfn.ExcecuteNoneQuery("exec pmis.[epay].[usp_generate_compensation_partial] " + USER.C_eID + "," + batchno + "," + amountType + "," + col_id + ",'" + value.AntiInject() + "'");
            return Content("6");
        }
        public ActionResult removeEmployeetoList_compensation(int batchno)
        {
            ISfn.ExcecuteNoneQuery("exec pmis.[epay].[usp_delete_compensation_partial] " + USER.C_eID + "," + batchno + "");
            return Content("6");
        }
        public ActionResult removeEmployeeInditoList_compensation(int t_compensation_tempID, int col_id)
        {
            ISfn.ExcecuteNoneQuery("exec pmis.[epay].[usp_delete_compensation_indi_partial] " + t_compensation_tempID + "," + col_id + "");
            return Content("6");
        }
        public ActionResult LoadGrid_Special_Payroll_create([DataSourceRequest] DataSourceRequest request, int batchID, int col_id)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), System.Data.CommandType.Text,
           "exec pmis.[epay].[usp_grid_special_payroll_Income]  " + batchID + "," + col_id + "," + USER.C_eID + "").Tables[0];
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadGrid_Special_Payroll([DataSourceRequest] DataSourceRequest request, int compensationID)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), System.Data.CommandType.Text,
           "exec pmis.[epay].[usp_load_Special_PayrLoadGrid_Income_specialPayroll] @userid = " + USER.C_eID + ",@compensationID="+ compensationID + "").Tables[0];
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult _partial_edit_compensation_amount(int eid, string empname, int Compensation_col_temp_id, int batchno)
        {
            DataTable rec = ISfn.ExecDataset("SELECT * FROM [pmis].[epay].[fn_getNetAmountGross_special] (" + eid + "," + batchno + ")").Tables[0];
            if (rec.Rows.Count > 0)
            {
                ViewBag.GAmount = rec.Rows[0]["GAmount"];
                ViewBag.NetAmount = rec.Rows[0]["NetAmount"];

            }
            else
            {
                ViewBag.GAmount = 0;
                ViewBag.NetAmount = "";
            }

            ViewBag.eid = eid;
            ViewBag.EmpName = empname;
            ViewBag.Compensation_col_temp_id = Compensation_col_temp_id.ToString().SEncrypt();
            return PartialView("_edit_compensation", null);
        }

        public JsonResult loand_payrollOffice()
        {
            //DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, "execute [dbo].[sp_comboDatasource] " + datasourceid + "," + para1 + "," + userType).Tables[0];
            DataTable rec = ISfn.ExecDataset("select * from pmis.Epay.fn_get_PayrollOffice(" + USER.C_eID + ")").Tables[0];
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult save_edit_special(string Compensation_col_temp_id, double Amount, Int64 eid, Int64 batchno)
        {
            string isdecrypt = Compensation_col_temp_id.SDecrypt();
            if (isdecrypt == "0")
            {
                ISfn.ExcecuteNoneQuery("execute pmis.epay.usp_update_Special_Payroll_col @value = '" + Amount + "',@batchno = '" + batchno + "',@eid = '" + eid + "'");
                return JavaScript("toastr.success('Amount successfully saved...', 'Success');");
            }
            else
            {
                ISfn.ExcecuteNoneQuery("update [pmis].[epay].[tbl_t_Special_Payroll_col] set Value = '" + Amount + "'  where t_Compensation_col_temp_id = " + Compensation_col_temp_id.SDecrypt() + "");
                return JavaScript("toastr.success('Amount updated saved...', 'Success');");
            }
        }

        public ActionResult save_deduction_special(string special_payroll_deductionID, double Amount, int eid, int deduction_id, int batchno)
        {
            if (special_payroll_deductionID == "0")
            {
                ISfn.ExcecuteNoneQuery("execute pmis.epay.[usp_update_Special_Payroll_deduction] @eid =" + eid + ",@Amount =" + Amount + ",@deduction_id =" + deduction_id + ",@user_eid = " + USER.C_eID + ",@batchno =" + batchno + "");
                return JavaScript("toastr.success('Amount successfully saved...', 'Success');");
            }
            else
            {
                ISfn.ExcecuteNoneQuery("update [pmis].[epay].[tbl_t_Special_Payroll_Deduction] set Amount = '" + Amount + "'  where [special_payroll_deductionID] = " + special_payroll_deductionID + "");
                return JavaScript("toastr.success('Amount updated saved...', 'Success');");
            }
        }

        public ActionResult save_income_special(int t_Compensation_col_temp_id, double Amount,string value,int col_id, int col_type_id,int eid,int batchno)
        {
            if (t_Compensation_col_temp_id == 0)
            {
                ISfn.ExcecuteNoneQuery("execute [pmis].[epay].[usp_Save_special_payroll_indi] @col_type_id =" + col_type_id + ", @value_str='" + value.AntiInject() + "', @value ='" + Amount + "', @t_Compensation_col_temp_id =" + t_Compensation_col_temp_id + ",@eid="+eid+ ",@batchno=" + batchno + ",@col_id="+col_id+"");
                return JavaScript("toastr.success('Amount successfully saved...', 'Success');");
            }
            else
            {
                ISfn.ExcecuteNoneQuery("execute [pmis].[epay].[usp_Save_special_payroll] @col_type_id ="+ col_type_id + ", @value_str='"+value.AntiInject()+"', @value ='"+Amount+"', @t_Compensation_col_temp_id ="+t_Compensation_col_temp_id+"");
                return JavaScript("toastr.success('Amount updated saved...', 'Success');");
            }
        }
        public ActionResult LoanDetails_special(int special_payroll_deductionID = 0, int eid = 0)
        {
            if (eid == 0)
            {
                return Content("8");
            }

            if (special_payroll_deductionID == 0)
            {
                ViewBag.special_payroll_deductionID = 0;
                ViewBag.eid = eid;
                ViewBag.DeductionID = 0;
                ViewBag.year = 2018;
                ViewBag.month = 5;
                ViewBag.Amount = 0;
                ViewBag.isEdit = 0;
                return PartialView("_loanDetails_special", null);
            }
            else
            {
                tbl_t_Special_Payroll_Deduction recDeductions = pmisdb.tbl_t_Special_Payroll_Deduction.Single(M => M.special_payroll_deductionID == special_payroll_deductionID);
                ViewBag.special_payroll_deductionID = special_payroll_deductionID;
                ViewBag.eid = eid;
                ViewBag.DeductionID = recDeductions.Deduction_ID;
                ViewBag.year = recDeductions.year_;
                ViewBag.month = recDeductions.month_;
                ViewBag.Amount = recDeductions.Amount;
                ViewBag.isEdit = 1;
                return PartialView("_loanDetails_special", null);
            }
        }
        public JsonResult ddl_compensationName()
        {
            IEnumerable<tbl_l_Compensation> compensationName;
            compensationName = pmisdb.tbl_l_Compensation.OrderBy(o => o.CompensatioName);
            return Json(compensationName.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult IncomeDetails_special(int t_Compensation_col_temp_id = 0, int eid = 0,int col_id = 0, int col_type_id=0)
        {
            if (eid == 0)
            {
                return Content("8");
            }

            if (t_Compensation_col_temp_id == 0)
            {
                ViewBag.special_payroll_deductionID = t_Compensation_col_temp_id;
                ViewBag.eid = eid;
                ViewBag.DeductionID = 0;
                ViewBag.Amount = 0;
                ViewBag.col_id = col_id;
                ViewBag.isEdit = 1;
                ViewBag.col_type_id = col_type_id;
                ViewBag.t_Compensation_col_temp_id = 0;
                
                return PartialView("_Income_Details_special", null);
            }
            else
            {
                
                tbl_t_Special_Payroll_col recDeductions = pmisdb.tbl_t_Special_Payroll_col.Single(M => M.t_Compensation_col_temp_id == t_Compensation_col_temp_id);
                ViewBag.t_Compensation_col_temp_id = t_Compensation_col_temp_id;
                ViewBag.eid = eid;
                
                ViewBag.col_id = recDeductions.Compensation_col_id;
                
                ViewBag.isEdit = 1;

                tbl_l_Compensation_Column col_compen = pmisdb.tbl_l_Compensation_Column.Single(M => M.Col_id == recDeductions.Compensation_col_id);
                ViewBag.col_type_id = col_compen.Datatype;

                if (col_compen.Datatype == 1)
                {

                    ViewBag.Amount = recDeductions.Value;

                }
                else if (col_compen.Datatype == 3)
                {

                    ViewBag.Amount = recDeductions.Value;

                }
                else
                {
                    ViewBag.Amount = recDeductions.value_str;
                }

                return PartialView("_Income_Details_special", null);
            }
        }
        public JsonResult get_ref_employmentStatus(int compensationid)
        {
            DataTable rec = ISfn.ExecuteDataset("execute pmis.[epay].[usp_ref_EmploymentStatus_byCompensation] " + compensationid + "");
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_ref_Employee_Income_special(int compensationID, int col_id, int isEdit)
        {
            int office = USER.C_OfficeID;
            DataTable rec = ISfn.ExecuteDataset("execute pmis.epay.[usp_ref_Employee_Income_special] " + compensationID + "," + col_id + "," + isEdit + "");
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_ref_Employee_Deduction_special(int eid, int year, int month, int isEdit)
        {
            int office = USER.C_OfficeID;
            DataTable rec = ISfn.ExecuteDataset("execute pmis.epay.[usp_ref_Employee_Deduction_special] " + eid + "," + year + "," + month + "," + isEdit + "");
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult print_pay_special(int batchno, int bankid, int compensationid, int officeID)
        {
            int rptid = 0;

            string strUrl = "";

                if (compensationid == 3)
                {
                    rptid = 24;
                    strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + rptid + "&batchno=" + batchno + "&OfficeID=" + officeID + "&compensationid=" + compensationid).ToString();
                }
                else if (compensationid == 7)
                {
                    rptid = 26;
                    strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + rptid + "&batchno=" + batchno + "&OfficeID=" + officeID + "&compensationid=" + compensationid).ToString();
                }
                else if (compensationid == 11)
                {
                    rptid = 30;
                    strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + rptid + "&batchno=" + batchno + "&OfficeID=" + officeID + "&compensationid=" + compensationid).ToString();
                }
                else if (compensationid == 12)
                {
                    rptid = 31;
                    strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + rptid + "&batchno=" + batchno + "&OfficeID=" + officeID + "&compensationid=" + compensationid).ToString();
                }
                else if (compensationid == 13)
                {
                    rptid = 32;
                    strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + rptid + "&batchno=" + batchno + "&OfficeID=" + officeID + "&compensationid=" + compensationid).ToString();
                }
                else
                {
                    rptid = 20;
                    strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + rptid + "&batchno=" + batchno + "&OfficeID=" + officeID + "&bankID=" + bankid + "").ToString();
                }
           
            return JavaScript("window.open('" + strUrl + "')");
        }
        #endregion
        #region other individual
  
        #endregion
    }
}