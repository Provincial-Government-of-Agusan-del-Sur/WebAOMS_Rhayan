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

namespace WebAOMS.Controllers
{
    public class PayrollDocCollectionController : Controller
    {
        TrackingSoapClient ws = new TrackingSoapClient();
        serviceSoapClient eps = new serviceSoapClient();
        pmisEntities pmisdb = new pmisEntities();
        // GET: PayrollDocCollection
        public ActionResult Index_Supporting_document_collection()
        {
            return View();
        }
        public ActionResult Supporting_document_lookup()
        {
            return View();
        }
        //Supporting Document Collection
        public ActionResult AttachmentEditingInline_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<tbl_Attachment> AttachmentIE;
            AttachmentIE = pmisdb.tbl_Attachment;
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(AttachmentIE.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AttachmentEditingInline_Create([DataSourceRequest] DataSourceRequest request, tbl_Attachment Attachment)
        {

            if (Attachment != null && ModelState.IsValid)
            {
                using (pmisEntities context = new pmisEntities())
                {
                    if (Attachment.attach_id == 0)
                    {
                        tbl_Attachment _SaveAttachment = new tbl_Attachment();
                        _SaveAttachment.attach_id = Attachment.attach_id;
                        _SaveAttachment.attach_name = Attachment.attach_name;
                        _SaveAttachment.attach_description = Attachment.attach_description;
                        _SaveAttachment.IsPrerequisite = Attachment.IsPrerequisite;
                        _SaveAttachment.orderby = Attachment.orderby;

                        context.tbl_Attachment.Add(_SaveAttachment);
                        context.SaveChanges();
                        int savedattach_id = _SaveAttachment.attach_id;
                        Attachment.attach_id = savedattach_id;
                    }
                    else
                    {
                        var _UpdateAttachment = context.tbl_Attachment.Where(m => m.attach_id == Attachment.attach_id).FirstOrDefault();
                        _UpdateAttachment.attach_id = Attachment.attach_id;
                        _UpdateAttachment.attach_name = Attachment.attach_name;
                        _UpdateAttachment.attach_description = Attachment.attach_description;
                        _UpdateAttachment.IsPrerequisite = Attachment.IsPrerequisite;
                        _UpdateAttachment.orderby = Attachment.orderby;
                        context.SaveChanges();
                    }

                }
            }

            return Json(new[] { Attachment }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AttachmentEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, tbl_Attachment Attachment)
        {
            if (Attachment != null)
            {
                using (pmisEntities context = new pmisEntities())
                {
                    var AttachmentDelete = context.tbl_Attachment.Where(m => m.attach_id == Attachment.attach_id).FirstOrDefault();
                    context.tbl_Attachment.Remove(AttachmentDelete);
                    context.SaveChanges();
                }
            }

            return Json(new[] { Attachment }.ToDataSourceResult(request, ModelState));
        }
    }
}