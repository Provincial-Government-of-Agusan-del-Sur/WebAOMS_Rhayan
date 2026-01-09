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
using WebAOMS.wsfmis;

namespace WebAOMS.Controllers
{
    [Authorize]
    public class LedgerController : Controller
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        fmisEntities fmisdb = new fmisEntities();
        // GET: Ledger
        public ActionResult Index()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a30";
            return View();
        }
 public ActionResult Index_gl()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a51";
            return View();
        }

        public ActionResult Index_UnfilteredCashFlow()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a52";
            return View();
        }
        public ActionResult grid_cash_flow_trial_balance([DataSourceRequest] DataSourceRequest request,DateTime to, int fundid)
        {
            IEnumerable<ufn_RF_CashFlow_TrialBalance_Result> rec;
            rec = fmisdb.ufn_RF_CashFlow_TrialBalance(fundid, to);
            return Json(rec.ToDataSourceResult(request, o => new { AccountName = o.AccountName, Credit = o.Credit, Debit = o.Debit, Accountcode = o.Accountcode }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult generateCF(int year,int month) {
            DataTable arec;
          
            arec = ISfn.ToDatatable("select jevid from [Accounting].[ufn_JEV_NoCashFlow_entry]("+year+","+month+") ", "@null", "");
            if (arec.Rows.Count > 0)
            {
                foreach (DataRow r in arec.Rows) {
                    ISfn.ToExecute2P("execute  Accounting.[usp_GetCashFlowEntry] " + r["jevid"] + "","@null", "", "@null2", 0);
                }

                return Json(new { code = 6, statusName = "Cash flow generation done...!" });
            }
            else
            {
                return Json(new { code = 5, statusName = "No record found(s)...!" });
            }
        }
        public ActionResult grid_sub_entries([DataSourceRequest] DataSourceRequest request, string childcode, DateTime from, DateTime to,int fundid)
        {
            string C_swipeID = USER.C_swipeID;
            if (C_swipeID == "99999")
            {
                if (ISfn.ExecScalar("select accounting.ufns_check_if_posteddate('" + to + "')") == "0")
                {
                    return JavaScript("toastr.warning('Not yet posted..!')");
                }
            }

            Int64 ChartAccountChildID = 0;
            int NM = 0;
            string cmdStr = @"select ChartAccountChildID,case [ChartAccountID] when 572 then 2 else NormalBalance end as NormalBalance from [Accounting].[tbl_l_ChartOfAccountsChild]  as a
                            inner join Accounting.tbl_l_ChartOfAccountsParent as b on b.AccountCode = left(childcode, 8)
                            where childcode = @childcode";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@childcode", childcode);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ChartAccountChildID = Convert.ToInt64(reader["ChartAccountChildID"]);
                        NM = Convert.ToInt16(reader["NormalBalance"]);
                    }
                }
                connection.Close();
            }


            //DataTable rec = new DataTable();
            //string qry = "select * from  Accounting.ufn_Ledger_Subsidiary_byDate(@childcode,@from,@to,@fundid,@nm) order by orderby";

            //using (SqlCommand command = new SqlCommand(qry, connection))
            //{
            //    command.Parameters.AddWithValue("@childcode", childcode);
            //    command.Parameters.AddWithValue("@from", from);
            //    command.Parameters.AddWithValue("@to", to);
            //    command.Parameters.AddWithValue("@fundid", fundid);
            //    command.Parameters.AddWithValue("@nm", NM);
            //    connection.Open();
            //    command.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter(command);
            //    da.Fill(rec);
            //    connection.Close();
            //    da.Dispose();
            //}


            var result = new ContentResult();
            var serializer = new JavaScriptSerializer();


            IEnumerable<ufn_Ledger_Subsidiary_byDate_Result> rec;

            rec = fmisdb.ufn_Ledger_Subsidiary_byDate(childcode, from, to, fundid, NM);

            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(rec.ToDataSourceResult(request, o => new { Date = o.Date, Particular = o.Particular, JEVno = o.JEVno, debit=o.debit, credit = o.credit, Obrno = o.Obrno, Dvno=o.Dvno, Checkno=o.Checkno, Balance = o.Balance,ADBNo = o.ADBNo }));
            result.ContentType = "application/json";
            return result;

            //return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }


        public ActionResult grid_gl_entries([DataSourceRequest] DataSourceRequest request, string childcode, DateTime from, DateTime to, int fundid)
        {

            string C_swipeID = USER.C_swipeID;
            if (C_swipeID == "99999")
            {
                if (ISfn.ExecScalar("select accounting.ufns_check_if_posteddate('" + to + "')") == "0")
                {
                    return JavaScript("toastr.warning('Not yet posted..!')");
                }
            }
            Int64 ChartAccountChildID = 0;
            string cmdStr = "select ChartAccountChildID from [Accounting].[tbl_l_ChartOfAccountsChild] where childcode = @childcode";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@childcode", childcode);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ChartAccountChildID = Convert.ToInt64(reader["ChartAccountChildID"]);
                    }
                }
                else
                {

                }
                connection.Close();
            }

            IEnumerable<ufn_Ledger_Subsidiary_GL_byDate_Result> rec;
            rec = fmisdb.ufn_Ledger_Subsidiary_GL_byDate(ChartAccountChildID, from, to, fundid);
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult view_account()
        {
            return PartialView("_SL_Index", null);
        }
        public ActionResult COA_read([DataSourceRequest] DataSourceRequest request, int? chartAccountID)
        {
            IEnumerable<tbl_l_ChartOfAccountsChild> AccountList;
            if (chartAccountID == 0)
            {
                AccountList = fmisdb.tbl_l_ChartOfAccountsChild.Where(M => M.AccountChildParentID == null);
            }
            else
            {
                AccountList = fmisdb.tbl_l_ChartOfAccountsChild.Where(M => M.AccountChildParentID == chartAccountID);
            }

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(AccountList.ToDataSourceResult(request, o => new { code = o.code, AccountChildName = o.AccountChildName.Replace("'", "`"), ChartAccountChildID = o.ChartAccountChildID, ChildCode = o.ChildCode, AccountChildParentID = o.AccountChildParentID }));
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult print_ledger_sl(Int16 fundID, DateTime from ,DateTime to, string childcode = "")
        {
            string C_swipeID = USER.C_swipeID;
            if (C_swipeID == "99999")
            {
                if (ISfn.ExecScalar("select accounting.ufns_check_if_posteddate('" + to + "')") == "0")
                {
                    return JavaScript("toastr.warning('Not yet posted..!')");
                }
            }
            string strUrl;
            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=2&from="+from.ToString("MM/dd/yyyy")+"&to="+to.ToString("MM/dd/yyyy") + "&fundid="+fundID+"&childcode="+childcode+"").ToString();
            return JavaScript("window.open('" + strUrl + "')");
        }

    }
}



//@* sdf
//sdf
//sdf
//sdf
//sd
//sdf
//sdfs
//df*@