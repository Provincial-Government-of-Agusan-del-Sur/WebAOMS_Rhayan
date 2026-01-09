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
using Microsoft.AspNet.Identity;

namespace WebAOMS.Controllers
{
    public class FinancialController : Controller
    {
        // GET: Financial
        fmisEntities db = new fmisEntities();
        public ActionResult index_trial_balance()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a32";
            return View();
        }
        public ActionResult index_schedule()
        {
            ViewBag.Title = "Financial Report";
            ViewBag.Title_mini = "Supporting Schedule";
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a38";
            return View();
        }
        public ActionResult index_cashflow()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.Title = "Financial Report";
            ViewBag.Title_mini = "Cash Flow";
            ViewBag.menuid = "a37";
            return View();
        }
        public ActionResult index_rrr()
        {
            ViewBag.Title = "Financial Report";
            ViewBag.Title_mini = "Report of Revenue and Receipts";
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a36";
            return View();
        }
        public ActionResult index_cne()
        {
            ViewBag.Title = "Financial Report";
            ViewBag.Title_mini = "Changes in Net Assets/ Equity";
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a35";
            return View();
        }
        
        public ActionResult print_trial_balance(Int16 fundid, int IsPreClosing, DateTime to)
        {
            string strUrl;
            
            var userId = User.Identity.GetUserId(); // ApplicationUser.Id

            // If your AspNetUserRoles.RoleId is INT:
            bool hasRole11 = db.AspNetUserRoles.Any(ur => ur.UserId == userId && ur.RoleId == "11");

            // If your AspNetUserRoles.RoleId is STRING, use:
            // bool hasRole11 = db.AspNetUserRoles.Any(ur => ur.UserId == userId && ur.RoleId == "11");

            if (hasRole11)
            {
                if (ISfn.ExecScalar("select accounting.ufns_check_if_posteddate('" + to + "')") == "0")
                {
                    return JavaScript("toastr.warning('Not yet posted..!')");
                }
            }

            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=3&IsPreClosing="+IsPreClosing+"&to="+to.ToString("MM/dd/yyyy")+"&fundid="+fundid+"").ToString();
            return JavaScript("window.open('" + strUrl + "')");
        }

        public ActionResult print_schedule(Int16 fundid, DateTime to)
        {
            string strUrl;

            if (USER.C_swipeID == "99999")
            {
                if (ISfn.ExecScalar("select accounting.ufns_check_if_posteddate('" + to + "')") == "0")
                {
                    return JavaScript("toastr.warning('Not yet posted..!')");
                }
            }

            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=51&to=" + to.ToString("MM/dd/yyyy") + "&fundid=" + fundid + "").ToString();
            return JavaScript("window.open('" + strUrl + "')");
        }
        public ActionResult index_financial_position()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a33";
            return View();
        }
        public ActionResult print_financial_position(Int16 fundid, int IsPreClosing, DateTime to)
        {
            string strUrl;
            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=4&Isdetailed=" + IsPreClosing + "&to=" + to.ToString("MM/dd/yyyy") + "&fundid=" + fundid + "").ToString();
            return JavaScript("window.open('" + strUrl + "')");
        }
        public ActionResult index_financial_performance()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a34";
            return View();
        }
        public ActionResult print_financial_performance(Int16 fundid, int IsPreClosing, DateTime to)
        {
            string strUrl;
            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=5&Isdetailed=" + IsPreClosing + "&to=" + to.ToString("MM/dd/yyyy") + "&fundid=" + fundid + "").ToString();
            return JavaScript("window.open('" + strUrl + "')");
        }
    }
}