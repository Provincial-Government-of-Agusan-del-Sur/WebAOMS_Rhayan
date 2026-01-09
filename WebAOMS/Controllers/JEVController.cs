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

namespace WebAOMS.Controllers
{
    [Authorize]
    public class JEVController : Controller
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        fmisEntities fmisdb = new fmisEntities();
        // GET: JEV
        public ActionResult JEV_Entries()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a9";
            return View();
        }

        public ActionResult JEV_Numbering()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a49";
            return View();
        }
        public ActionResult JEV_Numbering_CDJ()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a21";
            return View();
        }
        public ActionResult JEV_Numbering_CashJ()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a53";
            return View();
        }
        public ActionResult JEV_Numbering_CashR()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a68";
            return View();
        }
        public ActionResult JEV_Liquidation()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a50";
            return View();
        }
        //JEV Preparation
        #region JEV PRE
        public ActionResult _view_jev_details(string checkno)
        {
            if (checkno.Length < 5)
            {
                return Json(new { code = 5, statusName = "Invalid Checkno...!" });

            }
            //check the log trans status
            DataTable crec;
            crec = ISfn.ToDatatable("SELECT a.status,b.status as statusName FROM [fmis].[dbo].[tblAMIS_Logtrans] as a inner join tblCMS_TransStatusMap as b on b.indx = a.status where checkno = @checkno  and a.Actioncode = 1", "@checkno", checkno);
            if (crec.Rows.Count > 0)
            {
                int status = Convert.ToInt16(crec.Rows[0]["status"]);

                if (status == 15 || status == 13 || status == 22 || status == 14)
                {
                    //do nothing
                }
                else
                {
                    // return JavaScript("toastr.warning(')");
                    return Json(new { code = 5, statusName =  crec.Rows[0]["statusName"].ToString() + ", Unable to Process the transaction..!" });
                }
            }
            else
            {
                return Json(new { code = 5, statusName = "The Checkno not found in the database..!" });
            }


            string cmdStr = "select * from [Accounting].[ufn_JEV_get_Transaction_byCheckno](@checkno)";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@checkno", checkno);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ViewBag.ObrNo = reader["ObrNo"];
                        ViewBag.Particular = reader["Particular"];
                        ViewBag.GAmount = reader["GAmount"];
                        ViewBag.Name = reader["Name"];
                        ViewBag.FundType = reader["FundType"];

                        ViewBag.officename = reader["officename"];
                        ViewBag.RCenterCode = reader["RCenterCode"];
                        ViewBag.jevid = reader["jevid"];
                        ViewBag.NetAmount = reader["NetAmount"];
                        ViewBag.VatAmount = reader["VatAmount"];
                        ViewBag.VatAmount = reader["CheckNo"];
                        checkno = reader["CheckNo"].ToString();
                        ViewBag.dvno = reader["Dvno"];
                    }
                    DataTable chrec;
                    chrec = ISfn.ToDatatable("SELECT ChkNo FROM [fmis].[dbo].[tblAMIS_AccountantAdvice] where ChkNo = @CheckNo", "@CheckNo", checkno);
                    if (chrec.Rows.Count == 0)
                    {
                        return Json(new { code = 5, statusName = "No Accountant advice, Please prepare first the accountant advice..!" });
                    }
                }
                else {
                    return Json(new { code = 5, statusName = "The Checkno number not found in the database..!" });
                }
                
                connection.Close();
            }
            return View("_JEV_details", null);
        }
        
       
        public ActionResult generate_jevno(int journalTypeID, DateTime postdate ,int fundID ) {
            string cmdStr = "execute Accounting.[usp_JEV_generate_jevno] "+journalTypeID+ ",@postdate,"+fundID+",@userid";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@postdate", postdate);
                command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        return Json(new { jevseries = reader["jevseries"], jevno = reader["jevno"],code = 6 });
                    }
                }
                else
                {
                    return Json(new { code = 5, statusName = "Unable to generate jevno..!" });
                }

                connection.Close();
            }
            return Json(new { code = 5, statusName = "Unable to generate jevno..!" });
        }
        public ActionResult _pv_jevdetails(string dvno)
        {

            //if (dvno == null)
            //{
            //    dvno = "000-00-00-0000";
            //}
            //System.Net.ServicePointManager.Expect100Continue = false;
            //DataTable rec = ws.getTransactions(dvno.Replace("'", "''").Replace("--", "")).Tables[0];
            //if (rec.Rows.Count > 0)
            //{
            //    ViewBag.claimantname = rec.Rows[0]["claimantname"].ToString();
            //    ViewBag.Particular = rec.Rows[0]["Particular"].ToString();
            //    ViewBag.GAmount = rec.Rows[0]["GAmount"].ToString();
            //    ViewBag.dvno = dvno;
            //}

            return View("_JEV_entries");
        }
        public ActionResult _pv_jevdetails_sub(Int32 jevid,Int32 ChartAccountChildID)
        {
            tbl_l_ChartOfAccountsChild rec_charts = fmisdb.tbl_l_ChartOfAccountsChild.Single(M => M.ChartAccountChildID == ChartAccountChildID);
            ViewBag.ChartAccountChildID = rec_charts.ChartAccountChildID;
            ViewBag.code = rec_charts.code;
            ViewBag.AccountChildParentID = rec_charts.AccountChildParentID;
            ViewBag.AccountChildName = rec_charts.AccountChildName.Replace("'","`");
            ViewBag.ChildCode = rec_charts.ChildCode;
            //ViewBag.ChartAccountChildCode = rec_charts.ChartAccountChildCode;
            ViewBag.levelNo = rec_charts.levelNo;
            ViewBag.hasChild = rec_charts.hasChild;
            ViewBag.ChangeDate = rec_charts.ChangeDate;
            ViewBag.ModifiedByID = rec_charts.ModifiedByID;
            ViewBag.isActive = rec_charts.isActive;
            ViewBag.isEdit = 1;
            return View("_JEV_entries_sub");
        }
         public ActionResult JEV_transaction()
        {
            return View("_JEV_transaction");
        }

        public ActionResult _pv_reamkarkdetails(string dvno)
        {
            DataTable arec;
            arec = ISfn.ToDatatable("SELECT [Particular],Convert(nvarchar,Gamount,101) as Gamount FROM [Accounting].[tbl_t_JEV_Details] where Dvno = @dvno", "@dvno", dvno);
            if (arec.Rows.Count > 0)
            {
                ViewBag.particular = arec.Rows[0]["Particular"].ToString();
                ViewBag.gamount = arec.Rows[0]["Gamount"].ToString();
                return View("_JEV_edit_particular");
            }
            else
            {
                return Json(new { code = 5, statusName = "Please first the transaction before you can edit the particular." });
            }
        }
        public ActionResult grid_import_empname([DataSourceRequest] DataSourceRequest request,int month , int year , int officeid,int payrollempstatus)
        {
            DataTable crec;
            crec = ISfn.ToDatatable("execute [Accounting].[usp_import_payroll_grid] " + month + "," + year + ",@officeid," + payrollempstatus + "", "@officeid", officeid.ToString());
            return Json(crec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult grid_import_AP([DataSourceRequest] DataSourceRequest request, int year, int fundcode)
        {
            IEnumerable<ufn_JEV_import_accounts_payable_Result> _recentries;
           _recentries = fmisdb.ufn_JEV_import_accounts_payable(fundcode,year);
            return Json(_recentries.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult grid_import_postclosing([DataSourceRequest] DataSourceRequest request, int year, int fundcode,int incomeExpenseID)
        {
            IEnumerable<ufn_JEV_import_postclosing_Result> _recentries;
            _recentries = fmisdb.ufn_JEV_import_postclosing(fundcode, year,incomeExpenseID);

            var jsonResult = Json(_recentries.ToDataSourceResult(request));
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult grid_import_empname_byaccount([DataSourceRequest] DataSourceRequest request, int month, int year, string officearrayid, int payrollempstatus, int payreffid)
        {

            DataTable crec;
            crec = ISfn.ToDatatable("execute [Accounting].[usp_import_payroll_grid_byAccount] " + month + "," + year + ",@officearrayid," + payrollempstatus + "," + payreffid + "", "@officearrayid", officearrayid);
            return Json(crec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

public ActionResult grid_import_null_grid([DataSourceRequest] DataSourceRequest request)
        {

            DataTable dt = new DataTable();
            DataRow dr;
            int colsNum = 4;
            int rowsNum = 0;
            string colName = "ID";

 
                dt.Columns.Add(String.Format("{0}{1}", "eid", 1));
            dt.Columns.Add(String.Format("{0}{1}", "Empname", 2));
            dt.Columns.Add(String.Format("{0}{1}", "Amount", 3));

            for (int i = 1; i <= rowsNum; i++)
            {
                dr = dt.NewRow();

                for (int k = 1; k <= colsNum; k++)
                {
                    dr[String.Format("{0}{1}", colName, k)] = String.Format("{0}{1} Row{2}", colName, k, i);
                }
                dt.Rows.Add(dr);
            }
            return Json(dt.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult grid_browse_claimant([DataSourceRequest] DataSourceRequest request, string Accountname)
        {
            string arec;
            arec = ISfn.ToStringp("SELECT b.[Query] FROM [fmis].[Accounting].[tbl_l_AddDataFromClaimant] as a inner join [Accounting].[tbl_l_ClaimantType] as b on b.ClaimantTypeID = a.ClaimantTypeID where Whatname = @Accountname", "@Accountname", Accountname);
            DataTable crec;
            crec = ISfn.ToDatatable(arec, "@claimantcode", Accountname);


            var jsonResult = Json(crec.ToDataSourceResult(request));
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult _pv_browse_claimant(string Accountname, Int32 ChartAccountChildID)
        {
            ViewBag.Accountname = Accountname;
            ViewBag.ChartAccountChildID = ChartAccountChildID;
            return View("_JEV_browse_claimant");
        }
        public ActionResult _pv_btn_addclaimant(string Accountname, Int32 ChartAccountChildID)
        {
            DataTable arec;
            arec = ISfn.ToDatatable("SELECT [Whatname] FROM [fmis].[Accounting].[tbl_l_AddDataFromClaimant] where Whatname= @Accountname", "@Accountname", Accountname);
            if (arec.Rows.Count > 0)
            {
                return Content("<button  type=\"button\" class=\"btn btn-default\" style=\"cursor:pointer\" onclick=\"add_claimant_tocoa(" + ChartAccountChildID + ")\">Add Current Claimant</button><button  type=\"button\" class=\"btn btn-default\" style=\"cursor:pointer\" onclick=\"browse_claimant(" + ChartAccountChildID + ")\">Browse Claimant</button><button type=\"button\" class=\"btn btn-default\" style=\"cursor: pointer\" onclick=\"close_mdl_entries()\">Close</button>");
            }
            else
            {
                return Content("<button type=\"button\" class=\"btn btn-default\" style=\"cursor: pointer\" onclick=\"close_mdl_entries()\">Close</button>");
            }
        }

        public ActionResult Saveparticular(int jevid, string  particular, string gamount)
        {
        using (fmisEntities context = new fmisEntities())
            {
                IEnumerable<tbl_t_JEV_Details> _recentries = context.tbl_t_JEV_Details.Where(M => M.jevid == jevid);
                if (_recentries.ToList().Count() > 0)
                {
                    tbl_t_JEV_Details updateparticular = context.tbl_t_JEV_Details.Single(M => M.jevid == jevid);
                    updateparticular.Particular = particular;
                    updateparticular.Gamount = Convert.ToDecimal(gamount);
                    context.SaveChanges();
                    return Json(new { code = 6, statusName = "Successfully Save..!" });
                }
                else
                {
                    return Json(new { code = 5, statusName = "An error occured during updating...!" });
                }
            }
        }
        public ActionResult delelete_entries_byGL(Int32 jevid, int GLparentID) {
            if (isPosted(jevid) == true)
            {
                return Json(new { code = 5, statusName = "The Transaction already approved by the Accountant, unable to modify." });
            }

            if (ISfn.ToExecute2P("execute Accounting.usp_jev_delete_GLEntries_byjevid @jevid, @GLparentID,"+USER.C_swipeID+"", "@jevid", jevid.ToString(), "@GLparentID", GLparentID) == 6)
            {
                return Json(new { code = 6, statusName = "Successfully Deleted..!" });
            }
            else
            {
                return Json(new { code = 5, statusName = "An error occured during deletion...!" });
            }
            
        }
        public ActionResult addclaimant_toCOA(string dvno, int ChartAccountChildID,string parentName) {
            string result = ISfn.ToExecute3P("execute [Accounting].[usp_add_claimant_toCOA]  @dvno, @GLparentID,'" + USER.C_swipeID + "',@parentName", "@dvno", dvno, "@GLparentID", ChartAccountChildID, "@parentName", parentName);
            if (result == "6")
            {
                return Json(new { code = 6, statusName = "Successfully Added..!" });
            }
            else if (result == "5")
            {
                return Json(new { code = 5, statusName = "Claimant already on the database, Try search...!" });
            }
            else {
                return Json(new { code = 7, statusName = "Claimant type not match on the parent account name, unable to add the claimant." });
            }
            
        }
        public ActionResult add_browse_claimant_toCOA(int ChartAccountChildID,string Accountname,int parentID)
        {
            string result = ISfn.ToExecute3P("execute [Accounting].[usp_add_name_toCOA]  "+@ChartAccountChildID+"," + parentID + ",'" + USER.C_swipeID + "',@Accountname","@Accountname", Accountname,"@null",0, "@null2", "");
            if (result == "6")
            {
                return Json(new { code = 6, statusName = "Successfully Added..!" });
            }
            else if (result == "5")
            {
                return Json(new { code = 5, statusName = "Claimant already on the database, Try search...!" });
            }
            else
            {
                return Json(new { code = 7, statusName = "Claimant type not match on the parent account name, unable to add the claimant." });
            }
        }
        public ActionResult addToCashflow(int jevid,string Accountcode)
        {
            string result = ISfn.ToExecute3P("execute [Accounting].[usp_add_accountToCashflow] " + jevid+",@Accountcode,'" + USER.C_swipeID + "'", "@Accountcode",Accountcode,"@null",0, "@null2", "");
            if (result == "6")
            {
                return Json(new { code = 6, statusName = "Successfully Added..!" });
            }
            else if (result == "5")
            {
                return Json(new { code = 5, statusName = "Account code already on the cash flow entry...!" });
            }
            else
            {
                return Json(new { code = 7, statusName = "Error:invalid exception" });
            }
        }

        public ActionResult SaveCashflow(int jevid)
        {
            string result = ISfn.ToExecute3P("execute [Accounting].[usp_JEV_Save_cashflow]  " + jevid + "", "@null3", "", "@null", 0, "@null2", "");
            if (result == "6")
            {
                return Json(new { code = 6, statusName = "Successfully Saved..!" });
            }
            else if (result == "5")
            {
                return Json(new { code = 5, statusName = "Invalid cash flow entry...!" });
            }
            else
            {
                return Json(new { code = 7, statusName = "Error:invalid exception" });
            }
        }
        private Boolean isPosted(Int64 jevid) {
            Boolean result = false;
            if (jevid > 0){
                tbl_t_JEV_Details jv = fmisdb.tbl_t_JEV_Details.Single(m => m.jevid == jevid);
                if (jv.Posted == 1) {
                    result = false;
                }
            }
            return result;
        }

        public ActionResult SaveAmount(string reffno,Int32 ChartAccountChildID,Int32 GLParentID,string amount,int type,Int64 jevid)
        {
            try
            {
                string results = "";

                if (isPosted(jevid) == true) {
                    return Json(new { code = 5, statusName = "The Transaction already approved by the Accountant, unable to modify." });
                }

                using (fmisEntities context = new fmisEntities())
                {
                    tbl_t_AccoutingEntries updateAmount = new tbl_t_AccoutingEntries();
                    IEnumerable<tbl_t_AccoutingEntries> _recentries = fmisdb.tbl_t_AccoutingEntries.Where(M => M.jevID == jevid && M.ChartAccountChildID == ChartAccountChildID);
                    if (_recentries.ToList().Count() == 0)
                    {
                        updateAmount.GLChartAccountChildID = GLParentID;
                        if (type == 0)
                        {
                            updateAmount.credit =Convert.ToDecimal(amount);
                        }
                        else
                        {
                            updateAmount.debit = Convert.ToDecimal(amount);
                        }
                        updateAmount.ChartAccountChildID = ChartAccountChildID;
                        updateAmount.reffNo = reffno;
                        updateAmount.Changeby = USER.C_swipeID;
                        updateAmount.DTE = DateTime.Now;
                        updateAmount.jevID = jevid;
                        context.tbl_t_AccoutingEntries.Add(updateAmount);
                        context.SaveChanges();
                        results = "6";
                    }
                    else
                    {
                        if (Convert.ToDecimal(amount) == 0)
                        {
                            tbl_t_AccoutingEntries rec_entries = context.tbl_t_AccoutingEntries.Single(M => M.ChartAccountChildID == ChartAccountChildID && M.jevID ==jevid);
                            if (type == 0)
                            {
                                if (rec_entries.debit > 0)
                                {
                                    rec_entries.credit = null;
                                    rec_entries.DTE = DateTime.Now;
                                    rec_entries.Changeby = USER.C_swipeID;
                                    context.SaveChanges();
                                    results = "6";
                                }
                                else
                                {
                                    context.tbl_t_AccoutingEntries.Attach(rec_entries);
                                    context.tbl_t_AccoutingEntries.Remove(rec_entries);
                                    context.SaveChanges();
                                    results = "6";
                                }
                            }
                            else
                            {
                                if (rec_entries.credit > 0)
                                {
                                    rec_entries.debit = null;
                                    rec_entries.DTE = DateTime.Now;
                                    rec_entries.Changeby = USER.C_swipeID;
                                    context.SaveChanges();
                                    results = "6";
                                }
                                else
                                {
                                    context.tbl_t_AccoutingEntries.Attach(rec_entries);
                                    context.tbl_t_AccoutingEntries.Remove(rec_entries);
                                    context.SaveChanges();
                                    results = "6";
                                }
                            }
                        }
                        else
                        {
                            var coa = context.tbl_t_AccoutingEntries.Where(m => m.ChartAccountChildID == ChartAccountChildID && m.jevID == jevid).FirstOrDefault();
                            if (type == 0)
                            {
                                coa.credit = Convert.ToDecimal(amount);
                            }
                            else
                            {
                                coa.debit = Convert.ToDecimal(amount);
                            }
                            coa.DTE = DateTime.Now;
                            coa.Changeby = USER.C_swipeID;
                            context.SaveChanges();
                            results = "6";
                        }
                    }
                }
                return Content(results);
            }
            catch (Exception e)
            {
                string r = ISfn.errorLog(e.HResult, e.Message, "JEV/SaveAmount");
                return Content(r);
            }
        }
        public ActionResult SaveCFAmount(Int64 jevid, Int32 id, string amount, int type,string childcode)
        {
            try
            {
                string results = "";



                using (fmisEntities context = new fmisEntities())
                {
                    tbl_t_AccoutingEntries_CashFlow updateAmount = new tbl_t_AccoutingEntries_CashFlow();
                    IEnumerable<tbl_t_AccoutingEntries_CashFlow> _recentries = fmisdb.tbl_t_AccoutingEntries_CashFlow.Where(M => M.jevID == jevid && M.Accountcode == childcode);
                    if (_recentries.ToList().Count() == 0)
                    {
                        if (type == 0)
                        {
                            updateAmount.credit = Convert.ToDecimal(amount);
                        }
                        else
                        {
                            updateAmount.debit = Convert.ToDecimal(amount);
                        }
                        updateAmount.Accountcode = childcode;
                        updateAmount.jevID = jevid;
                        updateAmount.Changeby = USER.C_swipeID;
                        updateAmount.DTE = DateTime.Now;
                        context.tbl_t_AccoutingEntries_CashFlow.Add(updateAmount);
                        context.SaveChanges();
                        results = "6";
                    }
                    else
                    {
                        if (Convert.ToDecimal(amount) == 0)
                        {
                            tbl_t_AccoutingEntries_CashFlow rec_entries = context.tbl_t_AccoutingEntries_CashFlow.Single(M => M.jevID == jevid && M.Accountcode == childcode);
                            if (type == 0)
                            {
                                if (rec_entries.debit > 0)
                                {
                                    rec_entries.credit = null;
                                    rec_entries.DTE = DateTime.Now;
                                    rec_entries.Changeby = USER.C_swipeID;
                                    context.SaveChanges();
                                    results = "6";
                                }
                                else
                                {
                                    context.tbl_t_AccoutingEntries_CashFlow.Attach(rec_entries);
                                    context.tbl_t_AccoutingEntries_CashFlow.Remove(rec_entries);
                                    context.SaveChanges();
                                    results = "6";
                                }
                            }
                            else
                            {
                                if (rec_entries.credit > 0)
                                {
                                    rec_entries.debit = null;
                                    rec_entries.DTE = DateTime.Now;
                                    rec_entries.Changeby = USER.C_swipeID;
                                    context.SaveChanges();
                                    results = "6";
                                }
                                else
                                {
                                    context.tbl_t_AccoutingEntries_CashFlow.Attach(rec_entries);
                                    context.tbl_t_AccoutingEntries_CashFlow.Remove(rec_entries);
                                    context.SaveChanges();
                                    results = "6";
                                }
                            }

                        }
                        else
                        {
                            var coa = context.tbl_t_AccoutingEntries_CashFlow.Where(m => m.jevID == jevid && m.Accountcode==childcode).FirstOrDefault();
                            if (type == 0)
                            {
                                coa.credit = Convert.ToDecimal(amount);
                            }
                            else
                            {
                                coa.debit = Convert.ToDecimal(amount);
                            }
                            coa.DTE = DateTime.Now;
                            coa.Changeby = USER.C_swipeID;
                            context.SaveChanges();
                            results = "6";
                        }

                    }
                }
                return Content(results);
            }
            catch (Exception e)
            {
                string r = ISfn.errorLog(e.HResult, e.Message, "JEV/SaveCFAmount");
                return Content(r);
            }
        }
        public ActionResult Save_transaction(string reffno)
        {
            try
            {
                if (reffno.Length < 5)
                {
                    return Json(new { code = 5, statusName = "Invalid DVNO...!" });
                }

                DataTable arec;
                arec = ISfn.ToDatatable("SELECT  isnull(sum([debit]),0) as sdebit,isnull(sum([credit]),0) as scredit FROM [fmis].[Accounting].[tbl_t_AccoutingEntries] where reffno = @dvno", "@dvno", reffno);
                if (arec.Rows.Count > 0)
                {
                    double sdebit =Convert.ToDouble(arec.Rows[0]["sdebit"]);
                    double scredit =Convert.ToDouble(arec.Rows[0]["scredit"]);

                    if (sdebit != scredit)
                    {
                        return Json(new { code = 5, statusName = "The total of debit and credit amount are not equal, Unable to save the transaction..!" });
                    }
                    else if (sdebit == 0 && scredit == 0) {
                        return Json(new { code = 5, statusName = "Please add accounting entries first to save the transaction...!" });
                    }
                }
                else
                {
                    return Json(new { code = 5, statusName = "Please add accounting entries first to save the transaction...!" });
                }


                //check the log trans status
                DataTable crec;
                crec = ISfn.ToDatatable("SELECT a.status,b.status as statusName FROM [fmis].[dbo].[tblAMIS_Logtrans] as a inner join tblCMS_TransStatusMap as b on b.indx = a.status where dvno = @dvno  and a.Actioncode = 1", "@dvno", reffno);
                if (crec.Rows.Count > 0)
                {
                    int status = Convert.ToInt16(crec.Rows[0]["status"]);

                    if (status == 15 || status == 13 || status == 22 || status == 14)
                    {
                        //do nothing
                    }
                    else
                    {
                        // return JavaScript("toastr.warning(')");
                        return Json(new { code = 5, statusName = crec.Rows[0]["statusName"].ToString() + ", Unable to save the transaction..!" });
                    }
                }
                else
                {
                    return Json(new { code = 5, statusName = "The DV number not found in the database..!" });
                }

                
                DataTable rec = new DataTable();
                string cmdStr = "execute[Accounting].[usp_JEV_save_transaction] @dvno,@userid";
                SqlConnection connection = new SqlConnection(fmisConn);

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@dvno", reffno);
                    command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    return Json(new { code = 6, statusName = "Successfully Save..!" });
                }
                
            }
            catch (Exception e)
            {
                string r = ISfn.errorLog(e.HResult, e.Message, "JEV/Save_transaction");
                return Json(new { code = 5, statusName = e.Message});
            }
        }

        public ActionResult Save_transaction_through_checkno(Int32 jevid,string checkno)
        {
            try
            {
                //if (checkno.Length < 5)
                //{
                //    return Json(new { code = 5, statusName = "Invalid checkno...!" });
                //}

                if (isPosted(jevid) == true)
                {
                    return Json(new { code = 5, statusName = "The Transaction already approved by the Accountant, unable to modify." });
                }

                DataTable arec;
                if (jevid > 0)
                {
                    arec = ISfn.ToDatatable("SELECT  isnull(sum([debit]),0) as sdebit,isnull(sum([credit]),0) as scredit FROM [fmis].[Accounting].[tbl_t_AccoutingEntries] where jevid = @jevid", "@jevid", jevid.ToString());
                    if (arec.Rows.Count > 0)
                    {
                        double sdebit = Convert.ToDouble(arec.Rows[0]["sdebit"]);
                        double scredit = Convert.ToDouble(arec.Rows[0]["scredit"]);

                        if (sdebit != scredit)
                        {
                            return Json(new { code = 5, statusName = "The total of debit and credit amount are not equal, Unable to save the transaction..!" });
                        }
                        else if (sdebit == 0 && scredit == 0)
                        {
                            return Json(new { code = 5, statusName = "Please add accounting entries first to save the transaction...!" });
                        }
                    }
                    else
                    {
                        return Json(new { code = 5, statusName = "Please add accounting entries first to save the transaction...!" });
                    }
                }


                //check the log trans status
                DataTable crec;
                crec = ISfn.ToDatatable("SELECT a.status,b.status as statusName FROM [fmis].[dbo].[tblAMIS_Logtrans] as a inner join tblCMS_TransStatusMap as b on b.indx = a.status where checkno = @checkno  and a.Actioncode = 1", "@checkno", checkno);
                if (crec.Rows.Count > 0)
                {
                    int status = Convert.ToInt16(crec.Rows[0]["status"]);

                    if (status == 15 || status == 13 || status == 22 || status == 14)
                    {
                        //do nothing
                    }
                    else
                    {
                        // return JavaScript("toastr.warning(')");
                        return Json(new { code = 5, statusName = crec.Rows[0]["statusName"].ToString() + ", Unable to save the transaction..!" });
                    }
                }
                else
                {
                    return Json(new { code = 5, statusName = "The DV number not found in the database..!" });
                }


                DataTable rec = new DataTable();
                string cmdStr = "execute[Accounting].[usp_JEV_save_transaction_bycheckno] @checkno,@userid";
                SqlConnection connection = new SqlConnection(fmisConn);

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@checkno", @checkno);
                    command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            return Json(new { code = 6, statusName = "Successfully Save..!", newjevID = reader["newjevID"].ToString() });
                        }
                    }
                    connection.Close();
                }
                return Json(new { code = 6, statusName = "Successfully Save..!", newjevID = jevid });
            }
            catch (Exception e)
            {
                string r = ISfn.errorLog(e.HResult, e.Message, "JEV/Save_transaction");
                return Json(new { code = 5, statusName = e.Message });
            }
        }
        public ActionResult grid_entries([DataSourceRequest] DataSourceRequest request, int jevid)
        {
            IEnumerable<ufn_JEV_get_jev_byjevid_Result> rec;
            rec = fmisdb.ufn_JEV_get_jev_byjevid(jevid);
            return Json(rec.ToDataSourceResult(request, o=> new  { AccountChildName = o.AccountChildName, ChartAccountChildID = o.GLChartAccountChildID, GLChartAccountChildID = o.ChartAccountChildID, reffno = o.reffNo, credit = o.credit,debit = o.debit,code=o.code, ChildCode = o.ChildCode }),JsonRequestBehavior.AllowGet);
        }
        public ActionResult grid_entries_byjevid([DataSourceRequest] DataSourceRequest request, Int32 jevid)
        {
            IEnumerable<ufn_JEV_get_jev_byjevid_Result> rec;
            rec = fmisdb.ufn_JEV_get_jev_byjevid(jevid);
            return Json(rec.ToDataSourceResult(request, o => new { AccountChildName = o.AccountChildName, ChartAccountChildID = o.GLChartAccountChildID, GLChartAccountChildID = o.ChartAccountChildID, reffno = o.reffNo, credit = o.credit, debit = o.debit, code = o.code, ChildCode = o.ChildCode }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult grid_entries_cashflow([DataSourceRequest] DataSourceRequest request, Int64 jevid)
        {
            IEnumerable<ufn_JEV_CashFlow_entry_Result> rec;
            rec = fmisdb.ufn_JEV_CashFlow_entry(jevid);
            return Json(rec.ToDataSourceResult(request, o => new { AccountName = o.AccountName, Accountcode = o.Accountcode, credit = o.credit, debit = o.debit, id = o.t_AccoutingEntries_cf_id }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult grid_sub_entries([DataSourceRequest] DataSourceRequest request, Int32 jevid, Int32 parentID, Int32 GlParentID)
        {
            IEnumerable<ufn_JEV_get_jev_sub_byjevid_Result> rec;
            rec = fmisdb.ufn_JEV_get_jev_sub_byjevid(jevid, parentID, GlParentID).OrderBy(o => o.orderBy).ThenByDescending(d => d.ChartAccountChildID);

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(rec.ToDataSourceResult(request, o => new { AccountChildName = o.AccountChildName.Replace("'", "`"), ChartAccountChildID = o.ChartAccountChildID, jevid = o.jevid, credit = o.credit, debit = o.debit, code = o.code, ChildCode = o.ChildCode, orderBy = o.orderBy, hasChild = o.hasChild }));
            result.ContentType = "application/json";

            return result;
        }


        public ActionResult grid_coa([DataSourceRequest] DataSourceRequest request, int journalType)
        {
            DataTable crec;
            crec = ISfn.ToDatatable("execute [Accounting].[usp_JEV_get_coa] @journalType", "@journalType", journalType.ToString());
            return Json(crec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult grid_transaction_bydate([DataSourceRequest] DataSourceRequest request,DateTime from,DateTime to)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_JEV_get_transaction_byDate](@from,@to) order by jevid";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@from", from);
                command.Parameters.AddWithValue("@to", to);
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region JEV Numbering
        public ActionResult JEV_datepost(string postdate)
        {

            DataTable arec;
            arec = ISfn.ToDatatable("select format(Datepost,'MMMM dd, yyyy') as datepost from [Accounting].[tbl_t_DatePost] where userid = @userid", "@userid", USER.C_swipeID);
            if (arec.Rows.Count > 0)
            {
                ViewBag.postdate = arec.Rows[0]["datepost"].ToString(); ;
            }
            else
            {
                ViewBag.postdate = DateTime.Now;
            }

            return View("_JEV_datepost");
        }
        public ActionResult print_jev(Int64 jevid)
        {
            int rptid = 1;
            string strUrl;
            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + rptid + "&jevid="+jevid+"&userid=" + USER.C_swipeID + "").ToString();
            return JavaScript("window.open('" + strUrl + "')");
        }
        public ActionResult import_entries()
        {
            DataTable arec;
            arec = ISfn.ToDatatable("select format(Datepost,'MMMM, yyyy') as datepost from [Accounting].[tbl_t_DatePost] where userid = @userid", "@userid", USER.C_swipeID);
            if (arec.Rows.Count > 0)
            {
                ViewBag.postdate = arec.Rows[0]["datepost"].ToString(); ;
            }
            else
            {
                ViewBag.postdate = DateTime.Now;
            }
            return View("_JEV_import");
        }
        public ActionResult import_entrie_AP()
        {
            DataTable arec;
            arec = ISfn.ToDatatable("select format(Datepost,'MMMM, yyyy') as datepost from [Accounting].[tbl_t_DatePost] where userid = @userid", "@userid", USER.C_swipeID);
            if (arec.Rows.Count > 0)
            {
                ViewBag.postdate = arec.Rows[0]["datepost"].ToString(); ;
            }
            else
            {
                ViewBag.postdate = DateTime.Now;
            }
            return View("_JEV_import_AP");
        }

        public ActionResult import_entries_manual()
        {
            DataTable arec;
            arec = ISfn.ToDatatable("select format(Datepost,'MMMM, yyyy') as datepost from [Accounting].[tbl_t_DatePost] where userid = @userid", "@userid", USER.C_swipeID);
            if (arec.Rows.Count > 0)
            {
                ViewBag.postdate = arec.Rows[0]["datepost"].ToString();
            }
            else
            {
                ViewBag.postdate = DateTime.Now;
            }
            return View("_JEV_import_manual");
        }
        public ActionResult import_entries_excel()
        {
            DataTable arec;
            arec = ISfn.ToDatatable("select format(Datepost,'MMMM, yyyy') as datepost from [Accounting].[tbl_t_DatePost] where userid = @userid", "@userid", USER.C_swipeID);
            if (arec.Rows.Count > 0)
            {
                ViewBag.postdate = arec.Rows[0]["datepost"].ToString();
            }
            else
            {
                ViewBag.postdate = DateTime.Now;
            }

            return View("_JEV_import_excel");
        }
        public ActionResult import_entries_postclosing()
        {
            DataTable arec;
            arec = ISfn.ToDatatable("select format(Datepost,'MMMM, yyyy') as datepost from [Accounting].[tbl_t_DatePost] where userid = @userid", "@userid", USER.C_swipeID);
            if (arec.Rows.Count > 0)
            {
                ViewBag.postdate = arec.Rows[0]["datepost"].ToString();
            }
            else
            {
                ViewBag.postdate = DateTime.Now;
            }
            return View("_JEV_import_postclosing");
        }

        public ActionResult auto_generate_entries(Int32 jevid)
        {
          string  cmdStr = "execute [Accounting].[usp_JEV_import_payroll_AutoGenerate] @userid,@jevid";
       

        SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@jevid", jevid);
                command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                connection.Open();

                int rows = command.ExecuteNonQuery();
                connection.Close();
                if (rows > 1)
                {
                    return Json(new { code = 6, statusName = "Successfully Imported..!" });
                }
                else
                {
                    return Json(new { code = 5, statusName = "Unable to generate Accounting entries..!" });
                }
            }
        }
        public ActionResult delete_JEV_and_Entries(Int64 jevid,string remarks)
        {
            string cmdStr = "execute [Accounting].[usp_Delete_JEV] @jevid,@userid,@remarks";


            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@jevid", jevid);
                command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                command.Parameters.AddWithValue("@remarks", remarks);
                connection.Open();

                int rows = command.ExecuteNonQuery();
                connection.Close();
                //if (rows > 0)
                //{
                    return Json(new { code = 6, statusName = "Successfully Deleted..!" });
                //}
                //else
                //{
                //    return Json(new { code = 5, statusName = "Unable to delete JEV..!" });
                //}
            }
        }
        public JsonResult DataSource_Get_EmploymentStatus()
        {
            IEnumerable<tbl_l_PayrollEmpStatus> empstatus;
            empstatus = fmisdb.tbl_l_PayrollEmpStatus.OrderBy(o => o.name);
            return Json(empstatus.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_Get_Accountname_payroll_mapped(int PayrollType)
        {
            if (PayrollType == 1)
            {
                IEnumerable<ufn_ComboSource_Accountname_payrollMapped_regular_Result> empstatus;
                empstatus = fmisdb.ufn_ComboSource_Accountname_payrollMapped_regular(PayrollType).OrderBy(o => o.Accountname);
                return Json(empstatus.ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                IEnumerable<ufn_ComboSource_Accountname_payrollMapped_JOCOS_Result> empstatus;
                empstatus = fmisdb.ufn_ComboSource_Accountname_payrollMapped_JOCOS(PayrollType).OrderBy(o => o.Accountname);
                return Json(empstatus.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DataSource_Get_batchno(int year, int month, int payrollempstatus)
        {
            IEnumerable<ufn_get_payrollOfficeBatchno_Result> Get_batchno;
            Get_batchno = fmisdb.ufn_get_payrollOfficeBatchno(month, year).Where(w => w.payrollempstatus == payrollempstatus).OrderBy(o => o.OfficeName);
            return Json(Get_batchno.ToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult grid_Get_batchno([DataSourceRequest] DataSourceRequest request, int year, int month, int payrollempstatus)
        {
            DataTable rec = new DataTable();
            string cmdStr = "execute [Accounting].[usp_JEV_grid_payrollBatchno] "+month+","+year+","+payrollempstatus+"";
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

        public ActionResult grid_transaction_liquidation_bydate([DataSourceRequest] DataSourceRequest request, DateTime from, DateTime to)
        {
            //DAL dal = Singleton<DAL>.Instance;
            DataTable rec = new DataTable();

            string cmdStr = "select * from [Accounting].[ufn_JEV_transaction_liquidation_byDate]('"+from.ToString("MM/dd/yyyy")+ "','" + to.ToString("MM/dd/yyyy") + "',"+USER.C_swipeID+") order by jevid desc";
            SqlConnection connection = new SqlConnection(fmisConn);
            SqlCommand command = new SqlCommand(cmdStr, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(rec);
            connection.Close();
            da.Dispose();
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult grid_search_trans_byfield([DataSourceRequest] DataSourceRequest request, string field,string value )
        {
            //DAL dal = Singleton<DAL>.Instance;
            DataTable rec = new DataTable();

            string cmdStr = "execute Accounting.usp_search_transaction_byField '"+field.AntiInject()+"','"+value.AntiInject()+"'";
            SqlConnection connection = new SqlConnection(fmisConn);
            SqlCommand command = new SqlCommand(cmdStr, connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(rec);
            connection.Close();
            da.Dispose();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(rec.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;

            //return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult grid_RCI_transaction([DataSourceRequest] DataSourceRequest request,int fundid, int year, int month)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_JEV_cdj_getRCI]("+fundid+","+year+","+month+") order by RCINo";
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
        public ActionResult grid_FMISNO_transaction([DataSourceRequest] DataSourceRequest request, int fundid, int year, int month)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_JEV_CashJ_getFMISNO](" + fundid + "," + year + "," + month + ") order by RecordID";
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
        public ActionResult grid_RCDList_transaction([DataSourceRequest] DataSourceRequest request, int fundid, int year, int month)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_JEV_CashR_byPTVNo](" + fundid + "," + year + "," + month + ") order by dvno";
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
        public ActionResult grid_RCI_details([DataSourceRequest] DataSourceRequest request, string rcino)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_JEV_cdj_getRCI_details]('"+rcino.AntiInject()+"') order by OrderNo";
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
        public ActionResult grid_fmisno_details([DataSourceRequest] DataSourceRequest request, string RecordID)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_JEV_CashJ_getFMISNO_details]('"+RecordID.AntiInject()+"') order by RecordID";
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
        public ActionResult grid_approved_jev_bydate([DataSourceRequest] DataSourceRequest request, DateTime from, DateTime to)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_JEV_Approved_byDate]('" + from.ToString("MM/dd/yyyy") + "','" + to.ToString("MM/dd/yyyy") + "'," + USER.C_swipeID + ") order by jevid desc";
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

        public ActionResult grid_nocashflow([DataSourceRequest] DataSourceRequest request, int year, int month)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_JEV_NoCashFlow_entry] ("+year+","+month+") order by dvno";
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
        public ActionResult Save_transaction_liquidation(string reffno,int? rcenter,int? fundID,string particular,decimal? gamount,string jevno,string jevsereis,int? journalTypeID,Int64? jevid,DateTime? jevdate,int? isadjustment)
        {
            //try
            //{
            int jtype = Convert.ToInt16(jevno.Substring(10, 2));
            if (reffno.Length < 5)
            {
                return Json(new { code = 5, statusName = "Invalid DVNO...!" });

            }

            if (jevno.Substring(0, 3) == "000")
            {

                return Json(new { code = 5, statusName = "Invalid JEV Number..!" });
            }

            if (jevsereis.Length !=4)
            {

                return Json(new { code = 5, statusName = "JEV Series Number must be 4 digits..!" });
            }

            if (fundID != Convert.ToInt16(jevno.Substring(0, 3))) {

                return Json(new { code = 5, statusName = "JEV Number not match in fund type..!" });
            }

            if (journalTypeID != jtype)
            {

                return Json(new { code = 5, statusName = "JEV Number not match in journal type..!" });
            }

            if (particular == "") {
                return Json(new { code = 5, statusName = "Invalid Particular..!" });
            }
            if (gamount == 0)
            {
                return Json(new { code = 5, statusName = "Gross amount must be greater than 0..!" });
            }


            //if (jevid==0)
            //{
                DataTable jrec;
                jrec = ISfn.ToDatatable("SELECT jevno from [fmis].[Accounting].[tbl_t_JEV_Details] where jevno = @jevno and jevid !="+jevid+"", "@jevno", jevno + jevsereis);
                if (jrec.Rows.Count > 0)
                {
                    return Json(new { code = 8, statusName = "JEV number already in the database, Please click generate button to generate new jevno..!" });
                }
            //}

            if (isadjustment == 1 && jevid ==0)
            {


            }
            else
            {
                DataTable arec;
                arec = ISfn.ToDatatable("SELECT  isnull(sum([debit]),0) as sdebit,isnull(sum([credit]),0) as scredit FROM [fmis].[Accounting].[tbl_t_AccoutingEntries] where reffno = @dvno", "@dvno", reffno);
                if (arec.Rows.Count > 0)
                {
                    double sdebit = Convert.ToDouble(arec.Rows[0]["sdebit"]);
                    double scredit = Convert.ToDouble(arec.Rows[0]["scredit"]);

                    if (sdebit != scredit)
                    {
                        return Json(new { code = 5, statusName = "The total of debit and credit amount are not equal, Unable to save the transaction..!" });
                    }
                    else if (sdebit == 0 && scredit == 0)
                    {
                        return Json(new { code = 5, statusName = "Please add accounting entries first to save the transaction...!" });
                    }
                }
                else
                {
                    return Json(new { code = 5, statusName = "Please add accounting entries first to save the transaction...!" });
                }
            }
                DataTable rec = new DataTable();
                string cmdStr = "execute [Accounting].[usp_JEV_save_transaction_numbering_test] @dvno,@rcenter ,@fundID ,@particular ,@gamount ,@jevno ,@jevsereis ,@journalTypeID ,@jevdate ,@userid, @jevid,@isadjustment";
                SqlConnection connection = new SqlConnection(fmisConn);

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@dvno", reffno);
                    command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                    command.Parameters.AddWithValue("@rcenter", rcenter);
                    command.Parameters.AddWithValue("@fundID", fundID);
                    command.Parameters.AddWithValue("@particular", particular);
                    command.Parameters.AddWithValue("@gamount", gamount);
                    command.Parameters.AddWithValue("@jevno", jevno + jevsereis);
                    command.Parameters.AddWithValue("@jevsereis", jevsereis);
                    command.Parameters.AddWithValue("@journalTypeID", journalTypeID);
                    command.Parameters.AddWithValue("@jevid", jevid);
                    command.Parameters.AddWithValue("@jevdate", jevdate);
                    command.Parameters.AddWithValue("@isadjustment", isadjustment);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            jevid =Convert.ToInt32( reader["newjevID"]);
                            connection.Close();
                        return Json(new { code = 6, statusName = "Successfully Save..!", newjevID = jevid,newdvno = reader["newdvno"].ToString()});

                        }
                    }
                    connection.Close();
                    return Json(new { code = 6, statusName = "Successfully Save..!" });
                }
        }
        [Authorize]
        public ActionResult Save_JEV_Transactions(usp_JEV_save_transaction_numbering jev)
        {
            //try
            //{
            if (isPosted(jev.jevid) == true)
            {
                return Json(new { code = 5, statusName = "The Transaction already approved by the Accountant, unable to modify." });
            }

            string fulljevno = jev.JEVno + jev.JevSeriesNo;
            int jtype = Convert.ToInt16(jev.JEVno.Substring(10, 2));
            if (jev.Dvno.Length < 5)
            {
                return Json(new { code = 5, statusName = "Invalid DVNO...!" });
            }

            if (jev.JEVno.Substring(0, 3) == "000")
            {

                return Json(new { code = 5, statusName = "Invalid JEV Number..!" });
            }

            if (jev.JevSeriesNo.Length != 4)
            {

                return Json(new { code = 5, statusName = "JEV Series Number must be 4 digits..!" });
            }

            if (jev.fundID != Convert.ToInt16(jev.JEVno.Substring(0, 3)))
            {

                return Json(new { code = 5, statusName = "JEV Number not match in fund type..!" });
            }

            if (jev.Transtype != jtype)
            {
                return Json(new { code = 5, statusName = "JEV Number not match in journal type..!" });
            }

            if (jev.Particular == "" || jev.Particular == null)
            {
                return Json(new { code = 5, statusName = "Invalid Particular..!" });
            }
            if (jev.Gamount == 0)
            {
                return Json(new { code = 5, statusName = "Gross amount must be greater than 0..!" });
            }


            //if (jevid==0)
            //{
            DataTable jrec;
            jrec = ISfn.ToDatatable("SELECT jevno from [fmis].[Accounting].[tbl_t_JEV_Details] where jevno = @jevno and jevid !=" + jev.jevid + "", "@jevno", jev.JEVno + jev.JevSeriesNo);
            if (jrec.Rows.Count > 0)
            {
                return Json(new { code = 8, statusName = "JEV number already in the database, Please click generate button to generate new jevno..!" });
            }
            //}

            if (jev.jevid > 0)
            {
                DataTable arec;
                arec = ISfn.ToDatatable("SELECT  isnull(sum([debit]),0) as sdebit,isnull(sum([credit]),0) as scredit FROM [fmis].[Accounting].[tbl_t_AccoutingEntries] where jevid = @jevid", "@jevid", jev.jevid.ToString());
                if (arec.Rows.Count > 0)
                {
                    double sdebit = Convert.ToDouble(arec.Rows[0]["sdebit"]);
                    double scredit = Convert.ToDouble(arec.Rows[0]["scredit"]);

                    if (sdebit != scredit)
                    {
                        return Json(new { code = 5, statusName = "The total of debit and credit amount are not equal, Unable to save the transaction..!" });
                    }
                    else if (sdebit == 0 && scredit == 0)
                    {
                        return Json(new { code = 5, statusName = "Please add accounting entries first to save the transaction...!" });
                    }
                }
                else
                {
                    return Json(new { code = 5, statusName = "Please add accounting entries first to save the transaction...!" });
                }
            }
            DataTable rec = new DataTable();
            string cmdStr = "execute [Accounting].[usp_JEV_Save_details] @jevid,@Date_,@RCI  ,@Checkno  ,@Particular  ,@JEVno  ,@Claimantcode  ,@Gamount ,@Transtype ,@FmisVoucherno  ,@Dvno  ,@Obrno  ,@RCenter ,@Rdono,@Jevdate,@Ptvno,@JevSeriesNo,@PClosing ,@HaveDoc,@isContinuing ,@isAdjustment ,@isCA ,@fundID,@userid";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@Date_", SqlDbType.Date).Value = jev.Date_;
                command.Parameters.Add("@Jevdate", SqlDbType.Date).Value = jev.Jevdate;
                command.Parameters.Add("@HaveDoc", SqlDbType.TinyInt).Value = (object)jev.HaveDoc ?? DBNull.Value;
                command.Parameters.Add("@isContinuing", SqlDbType.TinyInt).Value = (object)jev.isContinuing ?? DBNull.Value;
                command.Parameters.Add("@isAdjustment", SqlDbType.TinyInt).Value = (object)jev.isAdjustment ?? DBNull.Value; 
                command.Parameters.Add("@isCA", SqlDbType.TinyInt).Value = (object)jev.isCA ?? DBNull.Value;
                command.Parameters.Add("@fundID", SqlDbType.SmallInt).Value = jev.fundID;
                command.Parameters.Add("@Transtype", SqlDbType.Int).Value = jev.Transtype;
                command.Parameters.Add("@RCenter", SqlDbType.Int).Value = (object)jev.RCenter ?? DBNull.Value;
                command.Parameters.Add("@userid", SqlDbType.Int).Value = USER.C_swipeID;
                command.Parameters.Add("@Gamount", SqlDbType.Money).Value = jev.Gamount;
                command.Parameters.Add("@PClosing", SqlDbType.Bit).Value = (object)jev.PClosing ?? DBNull.Value;
                command.Parameters.Add("@jevid", SqlDbType.BigInt).Value = jev.jevid;
                command.Parameters.Add("@JevSeriesNo", SqlDbType.BigInt).Value = jev.JevSeriesNo;
                command.Parameters.Add("@RCI", SqlDbType.VarChar, 50).Value = (object)jev.RCI ?? DBNull.Value;
                command.Parameters.Add("@Checkno", SqlDbType.VarChar, 25).Value = (object)jev.Checkno ?? DBNull.Value;
                command.Parameters.Add("@JEVno", SqlDbType.VarChar, 30).Value = fulljevno;
                command.Parameters.Add("@Claimantcode", SqlDbType.VarChar, 20).Value = (object)jev.Claimantcode ?? DBNull.Value;
                command.Parameters.Add("@FmisVoucherno", SqlDbType.VarChar, 50).Value = (object)jev.FmisVoucherno ?? DBNull.Value;
                command.Parameters.Add("@Dvno", SqlDbType.VarChar, 50).Value = (object)jev.Dvno ?? DBNull.Value;
                command.Parameters.Add("@Obrno", SqlDbType.VarChar, 25).Value = (object)jev.Obrno ?? DBNull.Value;
                command.Parameters.Add("@Rdono", SqlDbType.VarChar, 50).Value = (object)jev.Rdono ?? DBNull.Value;
                command.Parameters.Add("@Ptvno", SqlDbType.VarChar, 50).Value = (object)jev.Ptvno ?? DBNull.Value;
                command.Parameters.Add("@Particular", SqlDbType.NVarChar, 4000).Value = (object)jev.Particular ?? DBNull.Value;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        return Json(new { code = 6, statusName = "Successfully Save..!", newjevID = reader["newjevID"].ToString(), newdvno = reader["newdvno"].ToString() });
                    }
                }
                connection.Close();
                return Json(new { code = 6, statusName = "Successfully Save..!" });
            }
        }
        public ActionResult save_import_entries(int year,int month,Int32 jevid,int dp_batchno,int employmentid)
        {
            string cmdStr = "";
            if (isPosted(jevid) == true)
            {
                return Json(new { code = 5, statusName = "The Transaction already approved by the Accountant, unable to modify." });
            }

            if (employmentid == 1)
            {
                cmdStr = "execute Accounting.[usp_JEV_import_payroll_Regular] " + year + ","+month+ ","+ dp_batchno +",@userid,@jevid," + employmentid + "";
            }
            if (employmentid == 4)
            {
                cmdStr = "execute Accounting.[usp_JEV_import_payroll_Regular_differential] " + year + "," + month + "," + dp_batchno + ",@userid,@jevid," + employmentid + "";
            }
            else if (employmentid == 2)
            {
                cmdStr = "execute Accounting.[usp_JEV_import_payroll_CasualJOCOS] " + dp_batchno + ",@userid,@jevid";
            }

            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@jevid", jevid);
                command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                connection.Open();

                int rows = command.ExecuteNonQuery();
                connection.Close();
                if (rows > 1)
                {
                    return Json(new { code = 6, statusName = "Successfully Imported..!" });
                }
                else
                {
                    return Json(new { code = 5, statusName = "Unable to generate Accounting entries..!" });
                }
                
            }
        }
        public ActionResult save_import_AP(int year, int fundcode,int isdebit,Int32 jevid)
        {
            if (isPosted(jevid) == true)
            {
                return Json(new { code = 5, statusName = "The Transaction already approved by the Accountant, unable to modify." });
            }

            if (jevid == 0 )
            {
                return Json(new { code = 5, statusName = "Please Save first before you can import." });
            }
  
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand("execute [Accounting].[usp_Save_import_AP] @fundcode,@year,@isdebit,@jevid,@userid", connection))
            {
                command.Parameters.AddWithValue("@fundcode", fundcode);
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@isdebit", isdebit);
                command.Parameters.AddWithValue("@jevid", jevid);
                command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                connection.Open();

                int rows = command.ExecuteNonQuery();
                connection.Close();
                return Json(new { code = 6, statusName = "Successfully Imported..!" });
            }
        }
        public ActionResult save_import_postclosing(int year, int fundcode, int incomeExpenseid, Int32 jevid)
        {
            if (isPosted(jevid) == true)
            {
                return Json(new { code = 5, statusName = "The Transaction already approved by the Accountant, unable to modify." });
            }

            if (jevid == 0)
            {
                return Json(new { code = 5, statusName = "Please Save first before you can import." });
            }

            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand("execute [Accounting].[usp_Save_import_postclosing] @fundcode,@year,@incomeExpenseid,@jevid,@userid", connection))
            {
                command.Parameters.AddWithValue("@fundcode", fundcode);
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@incomeExpenseid", incomeExpenseid);
                command.Parameters.AddWithValue("@jevid", jevid);
                command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                connection.Open();

                int rows = command.ExecuteNonQuery();
                connection.Close();
                return Json(new { code = 6, statusName = "Successfully Imported..!" });
            }
        }
        public ActionResult save_import_entries_byAccount(int year, int month, Int32 jevid, string dp_batchno, int employmentid,int isdebit,int deductionid)
        {
            string cmdStr = "";
            if (isPosted(jevid) == true)
            {
                return Json(new { code = 5, statusName = "The Transaction already approved by the Accountant, unable to modify." });
            }

            if (employmentid == 1)
            {
                cmdStr = "execute [Accounting].[usp_JEV_import_payroll_Regular_byAccount] " + year + "," + month + ",@dp_batchno,@userid,@jevid," + employmentid + "," + deductionid + "," + isdebit + "";
            }
            else if (employmentid == 2)
            {
                cmdStr = "execute [Accounting].[usp_JEV_import_payroll_CasualJOCOS_byBatch] " + year + "," + month + ",@dp_batchno,@userid,@jevid," + employmentid + "," + deductionid + "," + isdebit + "";
            }
            else
            {
                cmdStr = "execute [Accounting].[usp_JEV_import_payroll] " + year + "," + month + ",@dp_batchno,@userid,@jevid," + employmentid + "," + deductionid + "," + isdebit + ""; 
            }


            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@jevid", jevid);
                command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                command.Parameters.AddWithValue("@dp_batchno", dp_batchno);
                connection.Open();

                int rows = command.ExecuteNonQuery();
                connection.Close();
                if (rows > 1)
                {
                    return Json(new { code = 6, statusName = "Successfully Imported..!" });
                }
                else
                {
                    return Json(new { code = 5, statusName = "Unable to generate Accounting entries..!" });
                }
            }
        }
        public ActionResult _search_transaction() {
            return View();
        }
        public JsonResult DataSource_search_field()
        {
            IEnumerable<vw_search_field> search_field;
            search_field = fmisdb.vw_search_field.OrderBy(o => o.name);

            return Json(search_field.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult _view_jev_liquidation(string dvno,int isadjustment)
        {
            string nacode = "";
            if (isadjustment == 1 || isadjustment == 2 || isadjustment == 3)
            {
                ViewBag.jevid = 0;
                ViewBag.Obrno = "";
                ViewBag.Particular = "";
                ViewBag.Gamount = "";
                ViewBag.Name = "";
                ViewBag.Jevdate = DateTime.Now.ToString("MM/dd/yyyy");
                nacode = "";
                ViewBag.RCenter = 0;
                ViewBag.date_ = DateTime.Now.ToString("MM/dd/yyyy");
                ViewBag.dvno = dvno;
                ViewBag.fundID = 0;
                ViewBag.NetAmount = "";
                ViewBag.journalTypeID = 4;
                ViewBag.JEVno = "000-00-00-00-";
                ViewBag.jevseries = "0000";
                ViewBag.isEdit = 0;
                ViewBag.isAdjustment = isadjustment;
            }
            else if (isadjustment == 4 || isadjustment == 5 || isadjustment == 6 || isadjustment == 7 || isadjustment == 8 || isadjustment == 9)
            {
                int journalType=0;
                if (isadjustment == 4)
                {
                    journalType = 1;
                }
                else if (isadjustment == 5)
                {
                    journalType = 2;
                }
                else if(isadjustment == 6)
                {
                    journalType = 3;
                }
                else if(isadjustment == 7)
                {
                    journalType = 4;
                }
                else if (isadjustment == 8)
                {
                    journalType = 5;
                }
                else if(isadjustment == 9)
                {
                    journalType = 6;
                }

                ViewBag.jevid = 0;
                ViewBag.Obrno = "";
                ViewBag.Particular = "";
                ViewBag.Gamount = "";
                ViewBag.Name = "";
                ViewBag.Jevdate = DateTime.Now.ToString("MM/dd/yyyy");
                nacode = "";
                ViewBag.RCenter = 0;
                ViewBag.date_ = DateTime.Now.ToString("MM/dd/yyyy");
                ViewBag.dvno = dvno;
                ViewBag.fundID = 0;
                ViewBag.NetAmount = "";
                ViewBag.journalTypeID = journalType;
                ViewBag.JEVno = "000-00-00-00-";
                ViewBag.jevseries = "0000";
                ViewBag.isEdit = 0;
                ViewBag.isAdjustment = 0;
            }
            else
            {
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
                            ViewBag.Jevdate = Convert.ToDateTime( reader["Jevdate"]).ToString("MM/dd/yyyy");
                            nacode = reader["nacode"].ToString();
                            ViewBag.RCenter = reader["RCenter"];
                            ViewBag.dvno = dvno;
                            ViewBag.Rdono = reader["Rdono"];
                            ViewBag.FmisVoucherno = reader["FmisVoucherno"];
                            ViewBag.Claimantcode = reader["Claimantcode"];
                            ViewBag.fundID = reader["fundID"];
                            ViewBag.NetAmount = reader["NetAmount"];
                            ViewBag.journalTypeID = reader["Transtype"];
                            ViewBag.JEVno = reader["JEVno"].ToString().Substring(0,13);
                            ViewBag.jevseries = reader["JEVno"].ToString().Substring(13, 4);
                            ViewBag.isEdit = reader["isEdit"];
                            ViewBag.isAdjustment = reader["isEdit"];
                        }

                    }
                    else
                    {
                        return Json(new { code = 5, statusName = "The DV number not found in the database..!" });
                    }

                    connection.Close();
                }
            }
            return View("_JEV_details_entries", null);
        }
        public JsonResult DataSource_Getfundtype()
        {
            IEnumerable<tbl_l_Fundtype> fundtype;
            fundtype = fmisdb.tbl_l_Fundtype.Where(w=> w.Actioncode == 2).OrderBy(o => o.FundName);

            return Json(fundtype.ToList(), JsonRequestBehavior.AllowGet);
        }

        

        public JsonResult DataSource_GetOffice()
        {
            IEnumerable<tblREF_AIS_Offices> fundtype;
            fundtype = fmisdb.tblREF_AIS_Offices.OrderBy(o => o.OfficeName);

            return Json(fundtype.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult view_approve_jev(Int64 jevid)
        {
            string cmdStr = "Execute [Accounting].[usp_JEV_Get_approval_jev] @jevid";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@jevid", jevid);
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
                        ViewBag.RCenter = reader["RCenter"];
                        ViewBag.fundID = reader["fundID"];
                        
                        ViewBag.journalTypeID = reader["Transtype"];
                        ViewBag.JEVno = reader["JEVno"].ToString();
                    }
                }
                else
                {
                    return Json(new { code = 5, statusName = "The DV number not found in the database..!" });
                }
                connection.Close();
            }
            return View("_JEV_approval",null);
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
        public ActionResult view_jev_numbering_byjevid(Int32 jevid)
        {
            string nacode = "";
            string cmdStr = "Execute [Accounting].[usp_JEV_Get_Details_byjevid] @jevid";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@jevid", jevid);
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
                        ViewBag.Claimantcode = reader["Claimantcode"];
                        ViewBag.ptvno = reader["ptvno"];
                        ViewBag.date_ = Convert.ToDateTime(reader["Date_"]).ToString("MM/dd/yyyy");
                        ViewBag.FmisVoucherno = reader["FmisVoucherno"];
                        nacode = reader["nacode"].ToString();
                        ViewBag.RCenter = reader["RCenter"];
                        ViewBag.Dvno = reader["Dvno"];
                        ViewBag.Rdono = reader["Rdono"];
                        ViewBag.fundID = reader["fundID"];
                        ViewBag.NetAmount = reader["NetAmount"];
                        ViewBag.journalTypeID = reader["Transtype"];
                        ViewBag.JEVno = reader["JEVno"].ToString().Substring(0, 13);
                        ViewBag.jevseries = reader["JEVno"].ToString().Substring(13, 4);
                        ViewBag.isEdit = reader["isEdit"];
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
        public ActionResult JEV_Entries_consolidated([DataSourceRequest] DataSourceRequest request, Int64 jevid)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_JEV_Entries_consolidated](@jevid)";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@jevid", jevid);
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult approved_jev(Int64 jevid)
        {

            if (ISfn.ToExecute2P("execute [Accounting].[usp_Approve_JEV] " + jevid+",1, '"+USER.C_swipeID+"'", "", "", "@aaaa",0) == 6)
            {
                return Json(new { code = 6, statusName = "Successfully Approved..!" });
            }
            else
            {
                return Json(new { code = 5, statusName = "An error occured during aprroval...!" });
            }
        }
        static SqlConnection connection;
        public JEVController()
        {
            connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        }

        public DataTable GetDataTableByAdapter(string query)
        {
            connection.Open();
            DateTime start = DateTime.Now;
            DataTable dt = new DataTable();
            new SqlDataAdapter(query, connection).Fill(dt);
            TimeSpan ts = DateTime.Now.Subtract(start);
            System.Diagnostics.Trace.Write("Time Elapsed in Adapter: " + ts.TotalMilliseconds);
            connection.Close();
            return dt;
        }

        public DataTable GetDataTableFast(string query)
        {
            connection.Open();
            IDataReader rdr = new SqlCommand(query, connection).ExecuteReader();
            DataTable resultTable = GetDataTableFromDataReader(rdr);
            connection.Close();
            return resultTable;
        }
        public DataTable GetDataTableFastv2(DateTime from,DateTime to)
        {
            connection.Open();

            string cmdStr = "select * from [Accounting].[ufn_JEV_transaction_liquidation_byDate](@from,@to,@userid) order by jevid desc";
            //SqlConnection connection = new SqlConnection(fmisConn);

            SqlCommand command = new SqlCommand(cmdStr, connection);

            //command.Parameters.AddWithValue("@from", from);
            //command.Parameters.AddWithValue("@to", to);
            //command.Parameters.AddWithValue("@userID", USER.C_swipeID);

            //command.Parameters.Add("@from", SqlDbType.Date, 30).Value = from;
            //command.Parameters.Add("@to", SqlDbType.Date, 30).Value = to;
            //command.Parameters.Add("@userID", SqlDbType.VarChar, 30).Value = USER.C_swipeID;

            //SqlDataReader da 

            IDataReader rdr = command.ExecuteReader();
            DataTable resultTable = GetDataTableFromDataReader(rdr);
            connection.Close();
            return resultTable;
        }
        public DataTable GetDataTableFromDataReader(IDataReader dataReader)
        {
            DataTable schemaTable = dataReader.GetSchemaTable();
            DataTable resultTable = new DataTable();

            foreach (DataRow dataRow in schemaTable.Rows)
            {
                DataColumn dataColumn = new DataColumn();
                dataColumn.ColumnName = dataRow["ColumnName"].ToString();
                dataColumn.DataType = Type.GetType(dataRow["DataType"].ToString());
                dataColumn.ReadOnly = (bool)dataRow["IsReadOnly"];
                dataColumn.AutoIncrement = (bool)dataRow["IsAutoIncrement"];
                dataColumn.Unique = (bool)dataRow["IsUnique"];
                resultTable.Columns.Add(dataColumn);
            }

            while (dataReader.Read())
            {
                DataRow dataRow = resultTable.NewRow();
                for (int i = 0; i < resultTable.Columns.Count - 1; i++)
                {
                    dataRow[i] = dataReader[i];
                }
                resultTable.Rows.Add(dataRow);
            }

            return resultTable;
        }
        #endregion
    }
}

//command.Parameters.AddWithValue("@from", from);
//command.Parameters.AddWithValue("@to", to);
//command.Parameters.AddWithValue("@userID", USER.C_swipeID);

//command.Parameters.Add("@from", SqlDbType.Date, 30).Value = from;
//command.Parameters.Add("@to", SqlDbType.Date, 30).Value = to;
//command.Parameters.Add("@userID", SqlDbType.VarChar, 30).Value = USER.C_swipeID;

//SqlDataReader da 
