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
    public class JournalsController : Controller
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        fmisEntities fmisdb = new fmisEntities();
        // GET: Journals
        public ActionResult Index()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a2";
            ViewBag.dateJournal = ISfn.ExecScalar("SELECT cast(max([yearMonth]) + '28' as date) as YearMonth FROM [fmis].[Accounting].[tbl_t_PostedJournal] ");
            return View();
        }
        public ActionResult grid_journals_bydate([DataSourceRequest] DataSourceRequest request, DateTime from, DateTime to,int fundid,int journaltypeid)
        {
            DataTable rec = new DataTable();

            string cmdStr="";

            if (journaltypeid == 1)
            {
                cmdStr = "SELECT * FROM [Accounting].[ufn_Journals_CashReceipts_byDate](@from,@to,@userid,@fundid)";
            }
            else if (journaltypeid == 2)
            {
                cmdStr = "select * from [Accounting].[ufn_Journals_CheckDJ_byDate](@from,@to,@userid,@fundid)";
            }
            else if (journaltypeid == 3)
            {
                cmdStr = "select * from [Accounting].[ufn_Journals_CashDJ_byDate](@from,@to,@userid,@fundid)";
            }
            else if (journaltypeid == 4)
            {
                cmdStr = "select * from [Accounting].[ufn_Journals_GeneralJournal_byDate](@from,@to,@userid,@fundid)";
            }

            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@from", from);
                command.Parameters.AddWithValue("@to", to);
                command.Parameters.AddWithValue("@userID", USER.C_swipeID);
                command.Parameters.AddWithValue("@fundid", fundid);
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult grid_journals_recap_bydate([DataSourceRequest] DataSourceRequest request, DateTime from, DateTime to, int fundid, int journaltypeid)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from Accounting.ufn_Journals_Recap_byDate(@from,@to,@userid,@journaltypeid, @fundid)";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@from", from);
                command.Parameters.AddWithValue("@to", to);
                command.Parameters.AddWithValue("@journaltypeid", journaltypeid);
                command.Parameters.AddWithValue("@fundid", fundid);
                command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }

            //IEnumerable<ufn_Journals_Recap_byDate_Result> rec;
            //rec = fmisdb.ufn_Journals_Recap_byDate(from, to, USER.C_swipeID, journaltypeid, fundid);
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_Getfundtype()
        {
            IEnumerable<ufn_Fund_Result> fundtype;
            fundtype = fmisdb.ufn_Fund(0).OrderBy(o => o.orderby);

            return Json(fundtype.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult print_Journal(int journalType,int fundid, DateTime date)
        {
            if (USER.C_swipeID == "99999")
            {
                if (ISfn.ExecScalar("select accounting.ufns_check_if_posteddate('" + date + "')") == "0")
                {
                    return JavaScript("toastr.warning('Not yet posted..!')");
                }
            }

            int rptid = 0;
            if (journalType == 1)
            {
                rptid = 24;
            }
            else if (journalType == 2)
            {
                rptid = 26;

            }
            else if (journalType == 3)
            {
                rptid = 25;
            }
            else if (journalType == 6)
            {
                rptid = 31;
            }
            else
            {
                rptid = 27;
            }
            string strUrl;
            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + rptid + "&fundid=" + fundid + "&to=" + date + "").ToString();
            return JavaScript("window.open('" + strUrl + "')");
        }


        public ActionResult print_recap(int journalType, int fundid, DateTime date,int isConso)
        {
            if (USER.C_swipeID == "99999")
            {
                if (ISfn.ExecScalar("select accounting.ufns_check_if_posteddate('" + date + "')") == "0")
                {
                    return JavaScript("toastr.warning('Not yet posted..!')");
                }
            }
            int rptid=28;
            string strUrl;
            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + rptid + "&fundid=" + fundid + "&to=" + date + "&transtype="+journalType+"&isConso="+isConso+"").ToString();
            return JavaScript("window.open('" + strUrl + "')");
        }
    }
}