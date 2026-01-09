using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Kendo.Mvc.UI;
using System.Web.Script.Serialization;
using WebAOMS.Models;
using WebAOMS.Base;
using WebAOMS.ws_tracking;
using WebAOMS.epsws;
using WebAOMS.Mod;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WebAOMS.Controllers
{
    public class VourcherPrepController : Controller
    {
        // GET: VourcherPrep
        [Authorize(Roles = "Liaison, Admin, Accountant")]
        public ActionResult Index()
        {
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a57";
            ViewBag.Title = "Voucher Preparation";
            ViewBag.Title_mini = "List of Registered Disbursement Voucher";
            return View();
        }
        //public ActionResult data_grid_attach_doc([DataSourceRequest] DataSourceRequest request, int doc_form_id)
        //{
        //    DataTable rec = new DataTable();
        //    string cmdStr = "execute [Accounting].[usp_grid_attach_doc] " + doc_form_id + "";
        //    SqlConnection connection = new SqlConnection(fmisConn);

        //    using (SqlCommand command = new SqlCommand(cmdStr, connection))
        //    {
        //        connection.Open();

        //        SqlDataAdapter da = new SqlDataAdapter(command);
        //        da.Fill(rec);
        //        connection.Close();
        //        da.Dispose();
        //    }
        //    return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
    }
}