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
    public class PayrollMappedController : Controller
    {
        private readonly fmisEntities _context = new fmisEntities();

        public ActionResult Index()
        {
            var accountChildList = _context.tbl_l_ChartOfAccountsChild
      .Select(x => new { ChartAccountChildID = x.ChartAccountChildID, AccountChildName = x.AccountChildName })
      .ToList();

            ViewData["AccountChildList"] = accountChildList;
            return View();
        }

        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var payrollMappedList = _context.tbl_l_PayrollMapped.ToList();
            return Json(payrollMappedList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAccountChildList()
        {
            var accountChildList = (from child in _context.tbl_l_ChartOfAccountsChild
                                    join payroll in _context.tbl_l_PayrollMapped
                                    on child.ChildCode equals payroll.childcode // Adjust this join condition based on your key
                                    select new
                                    {
                                        childcode = child.ChildCode,
                                        AccountChildName = child.AccountChildName,
                                        // Add any additional fields from tbl_l_PayrollMapped as needed
                                    })
                        .Take(100) // Limit the results to the top 100
                        .ToList();

            return Json(accountChildList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, tbl_l_PayrollMapped payrollMapped)
        {
            if (payrollMapped != null && ModelState.IsValid)
            {
                _context.tbl_l_PayrollMapped.Add(payrollMapped);
                _context.SaveChanges();
            }
            return Json(new[] { payrollMapped }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, PayrollMapped payrollMapped)
        {
            if (payrollMapped != null && ModelState.IsValid)
            {
                var entity = _context.tbl_l_PayrollMapped.Find(payrollMapped.trnno);
                if (entity != null)
                {
                    _context.Entry(entity).CurrentValues.SetValues(payrollMapped);
                    _context.SaveChanges();
                }
            }
            return Json(new[] { payrollMapped }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, PayrollMapped payrollMapped)
        {
            if (payrollMapped != null)
            {
                var entity = _context.tbl_l_PayrollMapped.Find(payrollMapped.trnno);
                if (entity != null)
                {
                    _context.tbl_l_PayrollMapped.Remove(entity);
                    _context.SaveChanges();
                }
            }
            return Json(new[] { payrollMapped }.ToDataSourceResult(request, ModelState));
        }
    
    }
}