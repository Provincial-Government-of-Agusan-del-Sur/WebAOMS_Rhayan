﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using WebAOMS.Models;

namespace WebAOMS.Controllers
{
    public class CompensationColumnController : Controller
    {
        private pmisEntities db = new pmisEntities();

        public ActionResult Compensation()
        {
            return View();
        }

        public ActionResult tbl_l_Compensation_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<tbl_l_Compensation> tbl_l_compensation = db.tbl_l_Compensation;
            DataSourceResult result = tbl_l_compensation.ToDataSourceResult(request, tbl_l_Compensation => new {
                compensationID = tbl_l_Compensation.compensationID,
                CompensatioName = tbl_l_Compensation.CompensatioName,
                Col_Name = tbl_l_Compensation.Col_Name,
                period = tbl_l_Compensation.period,
                HRSignatory = tbl_l_Compensation.HRSignatory,
                istaxable = tbl_l_Compensation.istaxable,
                width = tbl_l_Compensation.width,
                height = tbl_l_Compensation.height
            });

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult tbl_l_Compensation_Create([DataSourceRequest]DataSourceRequest request, tbl_l_Compensation tbl_l_Compensation)
        {
            if (ModelState.IsValid)
            {
                var entity = new tbl_l_Compensation
                {
                    CompensatioName = tbl_l_Compensation.CompensatioName,
                    Col_Name = tbl_l_Compensation.Col_Name,
                    period = tbl_l_Compensation.period,
                    HRSignatory = tbl_l_Compensation.HRSignatory,
                    istaxable = tbl_l_Compensation.istaxable,
                    width = tbl_l_Compensation.width,
                    height = tbl_l_Compensation.height
                };

                db.tbl_l_Compensation.Add(entity);
                db.SaveChanges();
                tbl_l_Compensation.compensationID = entity.compensationID;
            }
            return Json(new[] { tbl_l_Compensation }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult tbl_l_Compensation_Update([DataSourceRequest]DataSourceRequest request, tbl_l_Compensation tbl_l_Compensation)
        {
            if (ModelState.IsValid)
            {
                var entity = new tbl_l_Compensation
                {
                    compensationID = tbl_l_Compensation.compensationID,
                    CompensatioName = tbl_l_Compensation.CompensatioName,
                    Col_Name = tbl_l_Compensation.Col_Name,
                    period = tbl_l_Compensation.period,
                    HRSignatory = tbl_l_Compensation.HRSignatory,
                    istaxable = tbl_l_Compensation.istaxable,
                    width = tbl_l_Compensation.width,
                    height = tbl_l_Compensation.height
                };
                db.tbl_l_Compensation.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new[] { tbl_l_Compensation }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult tbl_l_Compensation_Destroy([DataSourceRequest]DataSourceRequest request, tbl_l_Compensation tbl_l_Compensation)
        {
            if (ModelState.IsValid)
            {
                var entity = new tbl_l_Compensation
                {
                    compensationID = tbl_l_Compensation.compensationID,
                    CompensatioName = tbl_l_Compensation.CompensatioName,
                    Col_Name = tbl_l_Compensation.Col_Name,
                    period = tbl_l_Compensation.period,
                    HRSignatory = tbl_l_Compensation.HRSignatory,
                    istaxable = tbl_l_Compensation.istaxable,
                    width = tbl_l_Compensation.width,
                    height = tbl_l_Compensation.height
                };

                db.tbl_l_Compensation.Attach(entity);
                db.tbl_l_Compensation.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { tbl_l_Compensation }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
