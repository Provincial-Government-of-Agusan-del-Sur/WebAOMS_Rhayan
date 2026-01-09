using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
using Microsoft.AspNet.Identity;
using Telerik.Reporting;
using Telerik.Reporting.Processing;

namespace WebAOMS.Controllers
{
    public class DGSignController : Controller
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        TrackingController trackController = new TrackingController();
        public ActionResult SignViaDG_reconstruct(int doc_details_id, string code, int reportid)
        {
            var r = "";
           

            var res_source = new InstanceReportSource();

            if (reportid == 48 || reportid == 49)
            {
                var report_separation = new Report.Design.Document.rpt_document_activityDesign_noneproc(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;
            }
            else if (reportid == 50)
            {
                var report_separation = new Report.Design.Document.rpt_document_process_design(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;
            }
            else if (reportid == 53 || reportid == 54)
            {
                var report_separation = new Report.Design.Document.rpt_document_activityDesign_noneproc(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;
            }
            else if (reportid == 57)
            {
                var report_separation = new Report.Design.Document.rpt_document_activityDesign_noneproc2025(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;
            
            }
            else if (reportid == 58)
            {
                var report_separation = new Report.Design.Document.rpt_document_trainingDesign2025(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;
            }
            else if (reportid == 56)
            {
                var report_separation = new Report.Design.Document.rpt_document_acivityDesign_revision(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;
            }
            else
            {
                var report_separation = new Report.Design.Document.rpt_document_training_design(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;

            }
            //PDF processing
            ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", res_source, null);
            var file = ISfn.createPDFBinary(result);

            using (SqlConnection connection = new SqlConnection(fmisConn))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("exec [Accounting].[usp_DGsign_Post_final_reconstruct] @userid, @Code,@File,@doc_details_id", connection))
                {
                    command.Parameters.AddWithValue("@userid", USER.C_eID);

                    command.Parameters.AddWithValue("@Code", code);
                    command.Parameters.AddWithValue("@File", file);
                    command.Parameters.AddWithValue("@doc_details_id", doc_details_id);
                    command.ExecuteNonQuery();
                }
                r = "Document forwarded to DGSign";



                //trackController.save_status_log(refno.AntiInject(), 381, "", USER.C_eID);
                connection.Close();
            }

            return Json(new { code = 6, statusName = r },JsonRequestBehavior.AllowGet);
        }
        public ActionResult SignViaDG(int doc_details_id, string code, int reportid)
        {
            var r = "";
            int recount = Convert.ToInt32(ISfn.ExecScalar("select Accounting.[fns_checkDocIfSign](" + doc_details_id + ") as result"));
            if (recount > 0)
            {
                return Json(new { code = 5, statusName = "The document has already been signed" });
            }

            if (Convert.ToInt32(ISfn.ExecScalar("select Accounting.[fns_checkDocIfHaveSignatory](" + doc_details_id + ") as result")) == 0)
            {
                return Json(new { code = 5, statusName = "Before you can sign in DGSign, please ensure that you have saved the document. This will help ensure that any changes you have made are properly recorded and that the document is ready for signing. Thank you for your cooperation." });
            }

            //verify inclusuve date
            if (Convert.ToInt32(ISfn.ExecScalar("execute [Accounting].[usp_verify_inclusiveDate] "+reportid+"," + doc_details_id + ",'"+USER.C_swipeID+"'")) == 0)
            {
                return Json(new { code = 5, statusName = "The inclusive date must be at least 14 days in advance for acceptance." });
            }


            if (Convert.ToInt32(ISfn.ExecScalar("select Accounting.[fns_checkDocIfHaveSignatory](" + doc_details_id + ") as result")) == 0)
            {
                return Json(new { code = 5, statusName = "Before you can sign in DGSign, please ensure that you have saved the document. This will help ensure that any changes you have made are properly recorded and that the document is ready for signing. Thank you for your cooperation." });
            }
            
            var res_source = new InstanceReportSource();
            
            if (reportid == 48 || reportid ==49 )
            {
                var report_separation = new Report.Design.Document.rpt_document_activityDesign_noneproc(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;
            }
            else if (reportid == 50)
            {
                var report_separation = new Report.Design.Document.rpt_document_process_design(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;
            }
            else if (reportid == 53 || reportid == 54)
            {
                var report_separation = new Report.Design.Document.rpt_document_activityDesign_noneproc(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;
            }
            else if (reportid == 57)
            {
                var report_separation = new Report.Design.Document.rpt_document_activityDesign_noneproc2025(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;
            }
            else if (reportid == 58)
            {
                var report_separation = new Report.Design.Document.rpt_document_trainingDesign2025(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;
            }
           
            else if (reportid == 56)
            {
                var report_separation = new Report.Design.Document.rpt_document_acivityDesign_revision(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;
            }
            else
            {
                var report_separation = new Report.Design.Document.rpt_document_training_design(reportid, doc_details_id);
                res_source.ReportDocument = report_separation;

            }
            //PDF processing
            ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", res_source, null);
            var file = ISfn.createPDFBinary(result);

            using (SqlConnection connection = new SqlConnection(fmisConn))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("exec [Accounting].[usp_DGsign_Post_final] @userid, @Code,@File,@doc_details_id", connection))
                {
                    command.Parameters.AddWithValue("@userid", USER.C_eID);

                    command.Parameters.AddWithValue("@Code", code);
                    command.Parameters.AddWithValue("@File", file);
                    command.Parameters.AddWithValue("@doc_details_id", doc_details_id);
                    command.ExecuteNonQuery();
                }
                r = "Document forwarded to DGSign";



                //trackController.save_status_log(refno.AntiInject(), 381, "", USER.C_eID);
                connection.Close();
            }

            return Json(new { code = 6, statusName = r });
        }

        public ActionResult preview_dgsign(int doc_form_id)
        {
            string rurl= Request.Url.Host;
            string s;
            if (rurl == "localhost")
            {
                 s= "https://192.168.101.56/dgsign/blank/getPDF_digital_only?pdfData=application/pdf&FormID=" + doc_form_id;
            }
            else
            {
                s = "https://pgas.ph/dgsign/blank/getPDF_digital_only?pdfData=application/pdf&FormID=" + doc_form_id;
            }

            
            return JavaScript("window.open('" + s + "')");
        }
        public ActionResult ForReview(int doc_details_id, string refno, int reportid)
        {
            var r = "";
            int recount = Convert.ToInt32(ISfn.ExecScalar("select Accounting.[fns_checkDocIfSign](" + doc_details_id + ") as result"));
            if (recount > 0)
            {
                return Json(new { code = 5, statusName = "The document has already been signed" });
            }

            if (Convert.ToInt32(ISfn.ExecScalar("select Accounting.[fns_checkDocIfHaveSignatory](" + doc_details_id + ") as result")) == 0)
            {
                return Json(new { code = 5, statusName = "Before you can sign in DGSign, please ensure that you have saved the document. This will help ensure that any changes you have made are properly recorded and that the document is ready for signing. Thank you for your cooperation." });
            }

            //verify inclusuve date
            if (Convert.ToInt32(ISfn.ExecScalar("execute [Accounting].[usp_verify_inclusiveDate] " + reportid + "," + doc_details_id + ",'" + USER.C_swipeID + "'")) == 0)
            {
                return Json(new { code = 5, statusName = "The inclusive date must be at least 14 days in advance for acceptance." });
            }

            int nextStatusID = Convert.ToInt32(ISfn.ExecScalar("select [Accounting].[fns_getNextStatusCode]('"+ refno.AntiInject() +"') as result"));


            trackController.save_status_log(refno.AntiInject(), nextStatusID, "", USER.C_eID);
            r = "Document forwarded to Reviewer";
            return Json(new { code = 6, statusName = r });
        }
        public ActionResult ForReview_revision(int doc_details_id, string refno, int reportid)
        {
            var r = "";
            int recount = Convert.ToInt32(ISfn.ExecScalar("select Accounting.[fns_checkDocIfSign](" + doc_details_id + ") as result"));
            if (recount > 0)
            {
                return Json(new { code = 5, statusName = "The document has already been signed" });
            }

            if (Convert.ToInt32(ISfn.ExecScalar("select Accounting.[fns_checkDocIfHaveSignatory](" + doc_details_id + ") as result")) == 0)
            {
                return Json(new { code = 5, statusName = "Before you can sign in DGSign, please ensure that you have saved the document. This will help ensure that any changes you have made are properly recorded and that the document is ready for signing. Thank you for your cooperation." });
            }

            //verify inclusuve date
            if (Convert.ToInt32(ISfn.ExecScalar("execute [Accounting].[usp_verify_inclusiveDate] " + reportid + "," + doc_details_id + ",'" + USER.C_swipeID + "'")) == 0)
            {
                return Json(new { code = 5, statusName = "The inclusive date must be at least 14 days in advance for acceptance." });
            }

            int nextStatusID = Convert.ToInt32(ISfn.ExecScalar("select [Accounting].[fns_getNextStatusCode]('" + refno.AntiInject() + "') as result"));


            trackController.save_status_log(refno.AntiInject(), nextStatusID, "", USER.C_eID);
            r = "Document forwarded to Reviewer";
            return Json(new { code = 6, statusName = r });
        }
        private int getnextStatusCode(string refno){


            return 1;
        }
        
    }

}