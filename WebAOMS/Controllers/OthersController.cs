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
    public class OthersController : Controller
    {
        // GET: Others
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult v_List_read([DataSourceRequest] DataSourceRequest request, string name)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["vConnString"].ToString(), System.Data.CommandType.Text,
            "execute [Accounting].[usp_V_search] '" + name.Replace("'", "''").Replace("--", "") + "'").Tables[0];
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
    }
}