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
    [Authorize]
    public class ReconciliationController : Controller
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        fmisEntities fmisdb = new fmisEntities();
        // GET: Reconciliation
        public ActionResult Index()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a9";
            return View();
        }
        public JsonResult get_criteria()
        {
            DataTable rec = ISfn.ExecuteDataset("select * from [Accounting].[ufn_Recon_Criteria] ()");
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Result_Criteria_read([DataSourceRequest] DataSourceRequest request, DateTime from, DateTime to, int ID)
        {
            //tracking setup
            string qry = "";

            qry = "execute [Accounting].[usp_Recon_Criteria] '"+from+"','"+to+"',"+ID+"";
          
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
    }
    
}