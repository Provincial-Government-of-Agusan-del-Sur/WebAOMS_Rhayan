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

    public class DocumentController : Controller
    {
        TrackingSoapClient ws = new TrackingSoapClient();
        serviceSoapClient eps = new serviceSoapClient();
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        string dbcon_fmis = ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString();
        fmisEntities fmisdb = new fmisEntities();


        // GET: Document
        [Authorize(Roles = "Liason, Admin, Tracking")]
        public ActionResult Tracking_Form_Index()
        {
            ViewBag.Title = "Tracking Form";
            ViewBag.Title_mini = "Document List";
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a61";
            return View();
        }
        public ActionResult LogPrinting()
        {
            ViewBag.Title = "Transaction Monitoring";
            ViewBag.Title_mini = "Transction List";
            ViewBag.rightSidebar_title = "TOMS";
            ViewBag.menuid = "a67";
            return View();
        }
        public ActionResult document_prep_Index()
        {
            ViewBag.Title = "Document List";
            ViewBag.Title_mini = "List of Document";
            ViewBag.rightSidebar_title = "Document Prep";
            ViewBag.menuid = "a62";
            return View();
        }
        public ActionResult IncomingTransaction()
        {
            ViewBag.Title = "Incoming Transaction";
            ViewBag.Title_mini = "List of Document";
            ViewBag.rightSidebar_title = "Receive Document";
            ViewBag.menuid = "a92";
            return View();
        }

        public ActionResult document_preparation_index()
        {
            ViewBag.Title = "Document List";
            ViewBag.Title_mini = "List of Document";
            ViewBag.rightSidebar_title = "Document Prep";
            ViewBag.menuid = "a85";
            return View();
        }

        [AllowAnonymous]
        public ActionResult print_preview_test()
        {
            return View();
        }
        public ActionResult Received_Logout_index()
        {
            ViewBag.Title = "Received/Logout";
            ViewBag.Title_mini = "Received/Logout Document";
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a63";
            return View();
        }
        public ActionResult Review_Logout_index()
        {
            ViewBag.Title = "Review/Logout";
            ViewBag.Title_mini = "Received/Logout Document";
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a89";
            return View();
        }
        public ActionResult grid_claimant_byClaimantType([DataSourceRequest] DataSourceRequest request, int claimantCode)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(dbcon_fmis, System.Data.CommandType.Text,
            "execute Accounting.usp_claimant_grid_byClaimantType " + claimantCode + "").Tables[0];
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult grid_claimant_byClaimantType_server_search([DataSourceRequest] DataSourceRequest request, int claimantCode, string name)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(dbcon_fmis, System.Data.CommandType.Text,
            "execute Accounting.usp_claimant_grid_byClaimantType_server_search " + claimantCode + ",'" + name.AntiInject() + "'").Tables[0];
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Add_doc(Int64 doc_form_id)
        {
            ViewBag.doc_form_id = doc_form_id;
            string cmdStr = @"SELECT d.*,isnull(status_code,0) as status_code from  Accounting.tbl_t_DocForm as d
  inner join Tracking.tbl_t_transactionDetails as b on b.Unique_refno = refno
  inner join Tracking.tbl_t_transactionDetails_log as e on e.trans_id = b.trans_id and e.isActive = 1
  where d.doc_form_id =  @doc_form_id";
            SqlConnection connection = new SqlConnection(fmisConn);
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@doc_form_id", SqlDbType.Int).Value = doc_form_id;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ViewBag.doc_form_id = reader["doc_form_id"];
                        ViewBag.officeid = reader["officeId"];
                        ViewBag.liaison_eid = reader["liaison_eid"];
                        ViewBag.particular = reader["Particular"];
                        ViewBag.gamount = reader["GAmount"];
                        ViewBag.phoneno = ISfn.Get_employee_phoneno(Convert.ToInt32(reader["liaison_eid"]));
                        ViewBag.refno = reader["refno"];
                        ViewBag.isEdit = 1;
                        ViewBag.status_code = reader["status_code"];
                    }
                }
                else
                {
                    ViewBag.doc_form_id = "0";
                    ViewBag.liaison_eid = USER.C_eID;
                    ViewBag.particular = "";
                    ViewBag.gamount = "0.00";
                    ViewBag.officeid = USER.C_OfficeID;
                    ViewBag.phoneno = USER.C_telephone;
                    ViewBag.refno = "";
                    ViewBag.isEdit = 0;
                    ViewBag.status_code = 0;
                }
            }
            connection.Close();
            return PartialView("_Add_doc_trans", null);
        }

        public ActionResult doc_review()
        {
            return View("Document_review");
        }
        public ActionResult review_for_received()
        {

            return PartialView("Document_review_for_received")
;
        }

        public ActionResult _Add_attachment(Int64 doc_form_id, int document_type_id, int id, int year)
        {
            if (document_type_id == 1)
            {
                string cmdStr = "execute  [Accounting].[usp_get_dv_details] @id";
                SqlConnection connection = new SqlConnection(fmisConn);
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            ViewBag.particular = reader["Particular"].ToString();
                            ViewBag.claimantcode = reader["Claimantcode"];
                            ViewBag.countperson = reader["countperson"];
                            ViewBag.gamount = reader["Gamount"];
                            ViewBag.transtype_id = reader["Transtype_id"];
                            ViewBag.rcenter = reader["RCenter"];
                            ViewBag.fundid = reader["fundID"];
                            ViewBag.doc_form_id = reader["doc_form_id"];
                            ViewBag.Name = reader["Name"];
                            ViewBag.ooe = reader["ooe"];
                            ViewBag.id = id;
                            ViewBag.isEdit = 1;
                        }
                    }
                    else
                    {
                        ViewBag.particular = "";
                        ViewBag.claimantcode = "";
                        ViewBag.ooe = "";
                        ViewBag.gamount = "";
                        ViewBag.transtype_id = "";
                        ViewBag.rcenter = "";
                        ViewBag.fundid = "";
                        ViewBag.doc_form_id = "";
                        ViewBag.Name = "";
                        ViewBag.id = 0;
                        ViewBag.isEdit = 0;
                    }
                }
                connection.Close();
                return PartialView("_Add_DV", null);
            }

            else if (document_type_id >= 7)
            {
                Int32 DGdoc_id = Convert.ToInt32(ISfn.ExecScalar("select Accounting.[fns_checkDocIfSign](" + id + ") as result"));

                Int32 doc_details_id = Convert.ToInt32(ISfn.ExecScalar("select [Accounting].[fns_check_idDocexists](" + doc_form_id + ") as result"));

                Int32 isSubmit = Convert.ToInt32(ISfn.ExecScalar("select [Accounting].[fns_check_iSForReview](" + doc_form_id + ") as result"));

                if (isSubmit == 1)
                {
                    return Json(new { code = 6, statusName = "The Document already submitted for review" });
                }

                if (doc_details_id == id)
                {
                    if (DGdoc_id > 0)
                    {
                        // return RedirectToAction("preview_dgsign", "DGSign", new { doc_form_id = DGdoc_id });
                        return Json(new { code = 8, statusName = getDGlink(DGdoc_id) });
                    }
                }


                else if (doc_details_id > 0 & document_type_id != 56)
                {
                    return Json(new { code = 5, statusName = "It seems that the document you're trying to attach is already included in the attachment. Please note that multiple attachments of the same document are not permitted." });
                }
                else if (id > 0 & document_type_id == 56)
                {
                    if (DGdoc_id > 0)
                    {
                        // return RedirectToAction("preview_dgsign", "DGSign", new { doc_form_id = DGdoc_id });
                        return Json(new { code = 8, statusName = getDGlink(DGdoc_id) });
                    }
                }

                ViewBag.report_id = document_type_id;
                ViewBag.doc_form_id = doc_form_id;

                string cmdStr = "execute [Accounting].[usp_Doc_htmlScript_base_v1] @report_id, @doc_details_id,@doc_form_id ,@userid,@year ";

                SqlConnection connection = new SqlConnection(fmisConn);
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@report_id", SqlDbType.Int).Value = document_type_id;
                    command.Parameters.Add("@doc_details_id", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@doc_form_id", SqlDbType.Int).Value = doc_form_id;
                    command.Parameters.Add("@userid", SqlDbType.Int).Value = USER.C_swipeID;
                    command.Parameters.Add("@year", SqlDbType.Int).Value = year;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            if (reader["status_code"].ToString() == "3")
                            {
                                return Json(new { code = 5, statusName = "It seems that the document you're trying to attach is already included in the attachment. Please note that multiple attachments of the same document are not permitted." });
                            }

                            ViewBag.html = reader["html"].ToString();
                            ViewBag.script = reader["script"].ToString().Replace("##style", ISfn.UrlStr("~/Content/styleEditor.css"));
                            ViewBag.doc_details_id = reader["doc_details_id"].ToString();
                            if (document_type_id == 46 || document_type_id == 47)
                            {
                                ViewBag.officeCharge = reader["officeCharge"].ToString();
                                ViewBag.wfpAccount = reader["wfpAccount"].ToString();
                                ViewBag.recommendingApprove = reader["recommendingApprove"].ToString();
                                ViewBag.approveby = reader["approveby"].ToString();

                            }
                            else if (document_type_id == 48 || document_type_id == 49 || document_type_id == 50)
                            {
                                ViewBag.officeCharge = reader["officeCharge"].ToString();
                                ViewBag.wfpAccount = reader["SpecificActivity_id"].ToString();
                                ViewBag.recommendingApprove = reader["recommendingApprove"].ToString();
                                ViewBag.approveby = reader["approveby"].ToString();
                                ViewBag.InclusiveDate = reader["InclusiveDate"].ToString();
                                ViewBag.Methodology = reader["Methodology"].ToString();
                                ViewBag.SpecificActivity_id = reader["SpecificActivity_id"].ToString();
                                ViewBag.EventId = reader["EventId"];
                                ViewBag.Activitytitle = reader["Title"];
                                ViewBag.InclusiveDate = reader["InclusiveDate"];
                                ViewBag.Venue = reader["Venue"];
                                ViewBag.Rationale = reader["Rationale"];
                                ViewBag.Objectives = reader["Objectives"];
                                ViewBag.ExpectedOutput = reader["ExpectedOutput"];
                                ViewBag.SourceOfFund = reader["SourceOfFund"];
                                ViewBag.Caterer = reader["Caterer"];
                                ViewBag.addCustom = reader["addCustom"];
                                ViewBag.EventType = reader["EventType"];
                                //ViewBag.officeID = reader["OfficeID"].ToString();
                            }

                            else if (document_type_id == 53 || document_type_id == 54 || document_type_id == 55 || document_type_id == 57 || document_type_id == 58 || document_type_id == 59)
                            {
                                ViewBag.officeCharge = reader["officeCharge"].ToString();
                                ViewBag.wfpAccount = reader["SpecificActivity_id"].ToString();
                                ViewBag.recommendingApprove = reader["recommendingApprove"].ToString();


                                ViewBag.approveby = reader["approveby"].ToString();
                                ViewBag.InclusiveDate = reader["InclusiveDate"].ToString();
                                ViewBag.Methodology = reader["Methodology"].ToString();

                                ViewBag.SpecificActivity_id = reader["SpecificActivity_id"].ToString();
                                ViewBag.EventId = reader["EventId"];
                                ViewBag.Activitytitle = reader["Title"];
                                ViewBag.InclusiveDate = reader["InclusiveDate"];
                                ViewBag.Venue = reader["Venue"];


                                ViewBag.Rationale = reader["Rationale"];
                                ViewBag.Objectives = reader["Objectives"];

                                ViewBag.LObjectives = reader["LObjectives"];
                                ViewBag.WAObjectives = reader["WAObjectives"];
                                ViewBag.RObjectives = reader["RObjectives"];
                                ViewBag.Evaluation = reader["Evaluation"];



                                ViewBag.ExpectedOutput = reader["ExpectedOutput"];
                                ViewBag.SourceOfFund = reader["SourceOfFund"];
                                ViewBag.Caterer = reader["Caterer"];
                                ViewBag.addCustom = reader["addCustom"];
                                ViewBag.EventType = reader["EventType"];


                                //ViewBag.officeID = reader["OfficeID"].ToString();
                            }
                            else if (document_type_id == 56)
                            {
                                ViewBag.officeCharge = reader["officeCharge"].ToString();
                                ViewBag.doc_details_id = reader["doc_details_id"].ToString();
                                ViewBag.event_title = reader["event_title"].ToString();
                                ViewBag.recommendingApprove = reader["recommendingApprove"].ToString();
                                ViewBag.approveby = reader["approveby"].ToString();


                                ViewBag.revision_id = reader["revision_Id"].ToString();
                                ViewBag.officeid = reader["officeid"].ToString();
                                ViewBag.eventid = reader["EventId"].ToString();

                                ViewBag.dte_from = reader["DTE_from"].ToString();
                                ViewBag.dte_to = reader["DTE_to"].ToString();
                                ViewBag.venue_from = reader["venue_from"].ToString();
                                ViewBag.venue_to = reader["venue_to"].ToString();
                                ViewBag.reason = reader["Reason"].ToString();
                                ViewBag.iscancel = reader["iscancel"].ToString();
                            }
                        }
                    }
                }

                connection.Close();
                if (document_type_id == 46 || document_type_id == 47)
                {
                    return PartialView("_Add_doc_details_new", null);
                }
                else if (document_type_id == 57 || document_type_id == 58)
                {
                    PopulateOffices();
                    return PartialView("_Add_document_details_nonProc_2025", null);
                }
                else if (document_type_id == 48 || document_type_id == 49)
                {
                    PopulateOffices();
                    return PartialView("_Add_document_details_nonProc", null);
                }
                else if (document_type_id == 50)
                {
                    PopulateOffices();
                    return PartialView("_Add_document_details_process_design", null);
                }

                else if (document_type_id == 53 || document_type_id == 54)
                {
                    PopulateOffices();
                    return PartialView("_Add_document_details_nonProc", null);
                }
                else if (document_type_id == 55)
                {
                    PopulateOffices();
                    return PartialView("_Add_document_details_process_design_nonProc", null);
                }
                else if (document_type_id == 56 )
                {
                    PopulateOffices();
                    return PartialView("_Add_document_details_request_form", null);
                }
                else
                {
                    return PartialView("_Add_doc_details", null);
                }

            }
            return PartialView("_Add_DV", null);
        }
        private void PopulateOffices()
        {
            using (var context = new fmisEntities())
            {
                var offices = context.tbl_l_PMISOffice.ToList();
                ViewBag.offices = offices;
            }
        }
        public string getDGlink(int doc_form_id)
        {
            string rurl = Request.Url.Host;
            string s;
            if (rurl == "localhost")
            {
                s = "https://192.168.101.56/dgsign/blank/getPDF_digital_only?pdfData=application/pdf&FormID=" + doc_form_id;
            }
            else
            {
                s = "https://pgas.ph/dgsign/blank/getPDF_digital_only?pdfData=application/pdf&FormID=" + doc_form_id;
            }
            return s;
        }

        public ActionResult _print_priview(Int64 doc_form_id, int document_type_id, int id, string refno)
        {
            string strUrl; string dvstr;
            string r = "";
            if (document_type_id == 1)
            {
                strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=12&dvid=" + id + "&refno=" + refno + "").ToString();

                r = ISfn.ExecScalar("SELECT transtype_id FROM [fmis].[Accounting].[tbl_t_DV_Details] where DVid = " + id + "").ToString();
                if (r == "13" || r == "15" || r == "14" || r == "19" || r == "8")
                {
                    dvstr = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=13&dvid=" + id + "&refno=" + refno + "").ToString();
                }
                else
                {
                    dvstr = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=11&dvid=" + id + "&refno=" + refno + "").ToString();
                }
                return JavaScript("window.open('" + strUrl + "');window.open('" + dvstr + "')");
            }
            else
            {
                if (document_type_id == 15)
                {
                    strUrl = ISfn.UrlStr("~/Document/print_preview_custom?doc_form_id=" + doc_form_id + "&document_type_id=" + document_type_id + "&id=" + id + "&refno=" + refno + "").ToString();
                }
                else if (document_type_id == 47 || document_type_id == 46 || document_type_id == 48 || document_type_id == 49 || document_type_id == 50 || document_type_id == 53 || document_type_id == 54 || document_type_id == 55 || document_type_id == 56 || document_type_id == 57 || document_type_id == 58 || document_type_id == 59)
                {
                    int docID = Convert.ToInt32(ISfn.ExecScalar("select Accounting.[fns_checkDocIfSign](" + id + ") as result"));
                    if (docID > 0)
                    {
                        return RedirectToAction("preview_dgsign", "DGSign", new { doc_form_id = docID });
                    }
                    else
                    {
                        strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + document_type_id + "&doc_details_id=" + id + "").ToString();
                    }

                }
                else if (document_type_id == 16 || document_type_id == 17 || document_type_id == 8)
                {
                    strUrl = ISfn.UrlStr("~/Document/print_preview_pre?doc_form_id=" + doc_form_id + "&document_type_id=" + document_type_id + "&id=" + id + "&refno=" + refno + "").ToString();
                }
                else
                {
                    strUrl = ISfn.UrlStr("~/Document/print_preview?doc_form_id=" + doc_form_id + "&document_type_id=" + document_type_id + "&id=" + id + "&refno=" + refno + "").ToString();
                }
                //strUrl = ISfn.UrlStr("~/Document/print_preview?doc_form_id=" + doc_form_id+ "&document_type_id="+ document_type_id + "&id="+id+"&refno="+refno+"").ToString();
                return JavaScript("window.open('" + strUrl + "')");
            }
            //return View();
        }
        public ActionResult load_reportdoc(string refno)
        {
            string id = "";
            string document_type_id = "";
            int dgsign_doc_id = 0;
            int status_code = 0;
            string strUrl = "";

            string cmdStr = "execute [Accounting].[usp_report_doc] @refno";
            SqlConnection connection = new SqlConnection(fmisConn);
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@refno", SqlDbType.VarChar, 16).Value = refno;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        id = reader["doc_details_id"].ToString();
                        document_type_id = reader["report_id"].ToString();
                        dgsign_doc_id = Convert.ToInt32(reader["dgSign_doc_id"]);
                        status_code = Convert.ToInt32(reader["status_code"]);
                    }
                }
            }
            connection.Close();
            int[] dgsignStatus = new int[] { 381, 382, 385 };

            if (dgsignStatus.Contains(status_code))
            {
                string rurl = Request.Url.Host;
                if (rurl == "localhost")
                {
                    strUrl = "https://192.168.101.56/dgsign/blank/getPDF_digital_only?pdfData=application/pdf&FormID=" + dgsign_doc_id;
                }
                else
                {
                    strUrl = "https://pgas.ph/dgsign/blank/getPDF_digital_only?pdfData=application/pdf&FormID=" + dgsign_doc_id;
                }
            }
            else
            {
                strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + document_type_id + "&doc_details_id=" + id + "").ToString();
            }

            return Content(strUrl);
        }
        public ActionResult printDVObr(int document_type_id, int id, string refno)
        {
            string strUrl;
            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + document_type_id + "&dvid=" + id + "&refno=" + refno + "").ToString();



            //r = ISfn.ExecScalar("SELECT transtype_id FROM [fmis].[Accounting].[tbl_t_DV_Details] where DVid = " + id + "").ToString();
            //if (r == "13" || r == "15" || r == "14" || r == "19" || r == "8")
            //{
            //    dvstr = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=13&dvid=" + id + "&refno=" + refno + "").ToString();
            //}

            //else
            //{
            //    dvstr = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=11&dvid=" + id + "&refno=" + refno + "").ToString();
            //}

            return JavaScript("window.open('" + strUrl + "')");

        }
        public ActionResult print_preview(Int64 doc_form_id, int document_type_id, int id, string refno)
        {
            ViewBag.report_id = document_type_id;
            ViewBag.doc_details_id = id;
            ViewBag.doc_form_id = doc_form_id;

            string cmdStr = "execute [Accounting].[usp_rpt_Doc_details] @report_id, @doc_details_id";
            SqlConnection connection = new SqlConnection(fmisConn);
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@report_id", SqlDbType.Int).Value = document_type_id;
                command.Parameters.Add("@doc_details_id", SqlDbType.Int).Value = id;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ViewBag.script = HttpUtility.HtmlDecode(reader["script"].ToString());
                        ViewBag.CompanyLogo = Url.Content(reader["CompanyLogo"].ToString());
                        ViewBag.refno = reader["refno"].ToString();
                        ISfn.SaveQRcode(ws.get_tracking_link(refno).ToString(), 4, refno);

                        string logopath = Url.Content(reader["officeLogo"].ToString());
                        if (System.IO.File.Exists(HttpContext.Server.MapPath(logopath)) == true)
                        {
                            ViewBag.officeLogo = logopath;
                        }
                        else
                        {
                            ViewBag.officeLogo = Url.Content("~/Content/office/0.png");
                        }
                        ViewBag.qrcode = Url.Content("~/Content/QR/" + refno + ".jpeg");
                    }
                }
            }
            connection.Close();
            return View();
        }
        public ActionResult print_activityTraining(int document_type_id, int doc_details_id)
        {
            string strUrl;
            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=" + document_type_id + "&doc_details_id=" + doc_details_id + "").ToString();
            return JavaScript("window.open('" + strUrl + "')");
        }
        public ActionResult print_preview_custom(Int64 doc_form_id, int document_type_id, int id, string refno)
        {
            ViewBag.report_id = document_type_id;
            ViewBag.doc_details_id = id;
            ViewBag.doc_form_id = doc_form_id;

            string cmdStr = "execute [Accounting].[usp_rpt_Doc_details] @report_id, @doc_details_id";
            SqlConnection connection = new SqlConnection(fmisConn);
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@report_id", SqlDbType.Int).Value = document_type_id;
                command.Parameters.Add("@doc_details_id", SqlDbType.Int).Value = id;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ViewBag.script = HttpUtility.HtmlDecode(reader["script"].ToString());
                        ViewBag.CompanyLogo = Url.Content(reader["CompanyLogo"].ToString());
                        ViewBag.refno = reader["refno"].ToString();
                        ISfn.SaveQRcode(ws.get_tracking_link(refno).ToString(), 4, refno);

                        string logopath = Url.Content(reader["officeLogo"].ToString());
                        if (System.IO.File.Exists(HttpContext.Server.MapPath(logopath)) == true)
                        {
                            ViewBag.officeLogo = logopath;
                        }
                        else
                        {
                            ViewBag.officeLogo = Url.Content("~/Content/office/0.png");
                        }
                        ViewBag.qrcode = Url.Content("~/Content/QR/" + refno + ".jpeg");
                    }
                }
            }
            connection.Close();
            return View();
        }
        public ActionResult print_preview_pre(Int64 doc_form_id, int document_type_id, int id, string refno)
        {
            ViewBag.report_id = document_type_id;
            ViewBag.doc_details_id = id;
            ViewBag.doc_form_id = doc_form_id;

            string cmdStr = "execute [Accounting].[usp_rpt_Doc_details_v2] @report_id, @doc_details_id";
            SqlConnection connection = new SqlConnection(fmisConn);
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@report_id", SqlDbType.Int).Value = document_type_id;
                command.Parameters.Add("@doc_details_id", SqlDbType.Int).Value = id;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ViewBag.script = HttpUtility.HtmlDecode(reader["script"].ToString());
                        ViewBag.CompanyLogo = Url.Content(reader["CompanyLogo"].ToString());
                        ViewBag.refno = reader["refno"].ToString();
                        ISfn.SaveQRcode(ws.get_tracking_link(refno).ToString(), 4, refno);

                        string logopath = Url.Content(reader["officeLogo"].ToString());
                        if (System.IO.File.Exists(HttpContext.Server.MapPath(logopath)) == true)
                        {
                            ViewBag.officeLogo = logopath;
                        }
                        else
                        {
                            ViewBag.officeLogo = Url.Content("~/Content/office/0.png");
                        }


                        ViewBag.qrcode = Url.Content("~/Content/QR/" + refno + ".jpeg");
                    }
                }
            }
            connection.Close();
            return View();
        }

        public ActionResult log_document(string refno)
        {
            ViewBag.refno = refno;
            OleDbHelper.ExecuteNonQuery(dbcon_fmis, System.Data.CommandType.Text, "execute [Tracking].[usp_insert_status_byOffice] " + USER.C_swipeID + "," + USER.C_OfficeID + "");


            string cmdStr = "execute [Tracking].[usp_tracking_details_byrefno] @refno";
            SqlConnection connection = new SqlConnection(fmisConn);
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@refno", SqlDbType.Char, 10).Value = refno;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ViewBag.doc_form_id = reader["doc_form_id"];
                        ViewBag.office = reader["office"];
                        ViewBag.liaison = reader["liaison"];
                        ViewBag.particular = reader["Particular"];
                        ViewBag.Status_code = reader["Status_code"];
                        ViewBag.refno = reader["refno"];
                    }
                }
                else
                {
                    return Json(new { code = 5, statusName = "Invalid Tracking Reference" });
                }
            }
            connection.Close();


            string sql = "execute Tracking.[usp_get_loghistory_activityTraining]  @r ";
            DataSet ds = new DataSet();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@r", refno);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                connection.Close();
            }
            connection.Close();
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return PartialView("_view_doc", null);
            }

            else
            {
                return PartialView("_view_doc", ds);
            }
        }

        public ActionResult _browse_claimant()
        {

            return PartialView("_browse_claimant", null);
        }
        public ActionResult _add_claimant()
        {

            return PartialView("_browse_claimant_new", null);
        }

        public ActionResult _configure_logo(Int32 doc_details_id)
        {
            string ID = ISfn.ExecScalar("select isnull(rightlogo,0) from [Accounting].[tbl_t_DocDetails] where doc_details_id = " + doc_details_id + "").ToString();
            ViewBag.ID = ID;
            return PartialView("_configure_logo", null);
        }
        public ActionResult _upload_logo()
        {
            return PartialView();
        }
        public ActionResult delete_docform(int doc_form_id, string remark, string refno)
        {
            TrackingController track = new TrackingController();
            try
            {
                int doc_detail_id = Convert.ToInt32(ISfn.ExecScalar("select [Accounting].[fns_checkDocExists](" + doc_form_id + ") as r"));
                if (doc_detail_id > 0)
                {
                    return Json(new { code = 5, statusName = "The tracking form you are trying to delete already has an attachment associated with it. To proceed with deleting the form, you will first need to remove the attachment." });
                }

                Int32 userid = Convert.ToInt32(USER.C_swipeID);
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute [Accounting].[usp_delete_DocForm] @doc_form_id,@userid";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@doc_form_id", SqlDbType.Int).Value = doc_form_id;
                    command.Parameters.Add("@userid", SqlDbType.Int).Value = userid;
                    connection.Open();

                    int rows = command.ExecuteNonQuery();
                    connection.Close();
                    track.save_status_log(refno, 73, remark, userid);
                    return Json(new { code = 6, statusName = "Successfully Deleted..!" });
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    code = e.HResult,
                    statusName = e.Message
                });
            }
        }
        public ActionResult delete_doc_details(int doc_details_id, string remark, int report_id)
        {
            try
            {
                if (check_if_DGsign(doc_details_id) > 0)
                {
                    return Json(new { code = 5, statusName = "The document has already been signed and cannot be modified or deleted." });
                }



                Int32 isSubmit = Convert.ToInt32(ISfn.ExecScalar("select [Accounting].[fns_check_VerifyForDelete](" + doc_details_id + ") as result"));

                if (isSubmit == 1)
                {
                    return Json(new { code = 5, statusName = "The Document already submitted for review, unable to delete the document" });
                }

                Int32 userid = Convert.ToInt32(USER.C_swipeID);
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute [Accounting].[usp_delete_Doc_details] @doc_details_id,@remark,@report_id,@userid";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@doc_details_id", SqlDbType.Int).Value = doc_details_id;
                    command.Parameters.Add("@report_id", SqlDbType.Int).Value = report_id;
                    command.Parameters.Add("@userid", SqlDbType.Int).Value = userid;
                    command.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = remark;
                    connection.Open();
                    int rows = command.ExecuteNonQuery();
                    connection.Close();
                    return Json(new { code = 6, statusName = "Successfully Deleted..!" });
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    code = e.HResult,
                    statusName = e.Message
                });
            }
        }
        public JsonResult DataSource_Getfundtype()
        {
            IEnumerable<tbl_l_Fundtype> fundtype;
            fundtype = fmisdb.tbl_l_Fundtype.Where(w => w.Actioncode == 2).OrderBy(o => o.FundName);
            return Json(fundtype.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_ooe()
        {
            IEnumerable<tblBMS_ObjectOfExpenditures> ooe;
            ooe = fmisdb.tblBMS_ObjectOfExpenditures.OrderBy(o => o.OOECode);
            return Json(ooe.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_logo()
        {
            IEnumerable<ufn_Doc_Logos_Result> fundtype;
            fundtype = fmisdb.ufn_Doc_Logos().OrderBy(o => o.OfficeName);
            return Json(fundtype.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult save_claimant(l_ClaimantDetails data)
        {
            int userid = Convert.ToInt32(USER.C_swipeID);
            int result = 0;
            if (data.clamantType == 2)
            {
                if (data.LastName == null || data.LastName == " " || data.LastName.Contains("  "))
                {
                    return Json(new { code = 5, statusName = "Invalid Lastname or Contain double white space" });
                }
                if (data.Firstname == null || data.Firstname == " " || data.Firstname.Contains("  "))
                {
                    return Json(new { code = 5, statusName = "Invalid Firstname or Contain double white space" });
                }
                //if (data.MI == null || data.MI == " " || data.MI.Contains("  "))
                //{
                //    return Json(new { code = 5, statusName = "Invalid Middle Name or Contain double white space" });
                //}
                if (data.Suffix == null)
                {
                    data.Suffix = "";
                }
                if (data.MI == null)
                {
                    data.MI = "";
                }
            }
            else if (data.clamantType == 3)
            {
                if (data.LastName == null || data.LastName == " " || data.LastName.Contains("  "))
                {
                    return Json(new { code = 5, statusName = "Invalid Lastname or Contain double white space" });
                }
                if (data.TIN == null || data.LastName == " " || data.LastName.Contains("  "))
                {
                    return Json(new { code = 5, statusName = "Invalid TIN No." });
                }
            }


            string Claimantcode = "";
            DataTable rec = new DataTable(); ;
            SqlConnection connection = new SqlConnection(fmisConn);


            string cmdStr = "execute [Accounting].[usp_Save_Claimant] @Firstname, @MI,@LastName , @Suffix, @Address, @ContactNo, @TIN, @clamantType";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@Firstname", SqlDbType.NVarChar, 200).Value = (object)data.Firstname ?? DBNull.Value;
                command.Parameters.Add("@MI", SqlDbType.NVarChar, 80).Value = (object)data.MI ?? DBNull.Value;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar, 500).Value = (object)data.LastName ?? DBNull.Value;
                command.Parameters.Add("@Suffix", SqlDbType.NVarChar, 200).Value = (object)data.Suffix ?? DBNull.Value;
                command.Parameters.Add("@Address", SqlDbType.NVarChar, 1000).Value = (object)data.Address ?? DBNull.Value;
                command.Parameters.Add("@ContactNo", SqlDbType.NVarChar, 200).Value = (object)data.ContactNo ?? DBNull.Value;
                command.Parameters.Add("@TIN", SqlDbType.NVarChar, 120).Value = (object)data.TIN ?? DBNull.Value;
                command.Parameters.Add("@clamantType", SqlDbType.NVarChar, 120).Value = (object)data.clamantType ?? DBNull.Value;
                connection.Open();

                SqlDataReader read = command.ExecuteReader();
                if (read.HasRows == true)
                {
                    while (read.Read())
                    {
                        Claimantcode = read["Claimantcode"].ToString();
                        result = Convert.ToInt32(read["result"]);
                    }
                }

                connection.Close();
                if (result == 6)
                {
                    return Json(new { code = 6, statusName = "Successfully Saved..!", Claimantcode = Claimantcode });
                }
                else
                {
                    return Json(new { code = 5, statusName = "Name already on the database..", Claimantcode = Claimantcode.AntiInject() });
                }

            }
        }
        public ActionResult save_DocForm(tbl_t_DocForm data)
        {
            TrackingController trackController = new TrackingController();
            //try
            //{
            int userid = Convert.ToInt32(USER.C_swipeID);
            int doc_form_id = 0;
            string refno = "";
            DataTable rec = new DataTable(); ;
            if (data.Particular == null)
            {
                return Json(new { code = 7, statusName = "The Particular is required" });
            }

            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute [Accounting].[usp_Save_docform] @doc_form_id,@officeId,@liaison_eid,@Particular,@GAmount,'',@UserID";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@doc_form_id", SqlDbType.Int).Value = data.doc_form_id;

                command.Parameters.AddWithValue("@officeid", data.officeId);
                command.Parameters.AddWithValue("@liaison_eid", data.liaison_eid);
                command.Parameters.AddWithValue("@particular", data.Particular);
                command.Parameters.AddWithValue("@gamount", data.GAmount);
                command.Parameters.AddWithValue("@userid", userid);
                connection.Open();

                SqlDataReader read = command.ExecuteReader();
                if (read.HasRows == true)
                {
                    while (read.Read())
                    {
                        doc_form_id = Convert.ToInt32(read["doc_form_id"]);
                        refno = trackController.get_unique_refno(read["doc_form_id"].ToString(), 49, "108A", data.Particular, data.GAmount.ToString());
                        ISfn.ToExecute2P("update [Accounting].[tbl_t_DocForm] set refno = @refno where [doc_form_id] = @doc_form_id ", "@refno", refno, "@doc_form_id", doc_form_id);
                        trackController.save_status_log(refno, 72, "", userid);
                    }
                }
                connection.Close();
                return Json(new { code = 6, statusName = "Successfully Saved..!", doc_form_id = doc_form_id, refno = refno });
            }
            //}
            //catch (Exception e)
            //{
            //    return Json(new { code = e.HResult, statusName = e.Message });
            //}
        }
        public int check_if_DGsign(int doc_details_id)
        {
            int r;
            r = Convert.ToInt32(ISfn.ExecScalar("select Accounting.[fns_checkDocIfSign](" + doc_details_id + ") as result"));
            return r;
        }
        public void SaveEvent(System.Web.Mvc.FormCollection data)
        {
            int eventid = Convert.ToInt32(data["doc_details_id"]);

            string cmdStr = "SELECT [EventId] FROM [fmis].[Accounting].[tbl_t_DocEvents] where EventID= @doc_details_id";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@doc_details_id", eventid);


                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == false)
                {

                    using (fmisEntities context = new fmisEntities())
                    {
                        tbl_t_DocEvents _SaveE = new tbl_t_DocEvents();

                        //_SaveE.InclusiveDate =data["InclusiveDate"];
                        _SaveE.Title = data["Title"];
                        _SaveE.InclusiveDate = data["InclusiveDate"];
                        _SaveE.Venue = data["Venue"];
                        //_SaveE.VenueID = data["Venue"];
                        _SaveE.Rationale = data["Rationale"];
                        _SaveE.Objectives = data["Objectives"];
                        _SaveE.ExpectedOutput = data["ExpectedOutput"];
                        _SaveE.RObjectives = data["RObjectives"];
                        _SaveE.WAObjectives = data["WAObjectives"];
                        _SaveE.LObjectives = data["LObjectives"];
                        _SaveE.Evaluation = data["Evaluation"];
                        

                        _SaveE.SourceOfFund = data["SourceOfFund"];
                        _SaveE.Caterer = data["Caterer"];
                        _SaveE.Methodology = data["Methodology"];

                        _SaveE.SpecificActivity_id = Convert.ToInt32(data["wpfAccount"]);
                        _SaveE.EventId = eventid;
                        context.tbl_t_DocEvents.Add(_SaveE);
                        context.SaveChanges();

                    }
                }
                else
                {
                    using (fmisEntities context = new fmisEntities())
                    {
                        var _SaveE = context.tbl_t_DocEvents.Where(m => m.EventId == eventid).FirstOrDefault();
                        _SaveE.Title = data["Title"];
                        _SaveE.Venue = data["Venue"];
                        _SaveE.InclusiveDate = data["InclusiveDate"];
                        _SaveE.Rationale = data["Rationale"];
                        _SaveE.Objectives = data["Objectives"];
                        _SaveE.ExpectedOutput = data["ExpectedOutput"];
                        _SaveE.SourceOfFund = data["SourceOfFund"];
                        _SaveE.Methodology = data["Methodology"];
                        _SaveE.RObjectives = data["RObjectives"];
                        _SaveE.WAObjectives = data["WAObjectives"];
                        _SaveE.LObjectives = data["LObjectives"];
                        _SaveE.Evaluation = data["Evaluation"];
                        _SaveE.SpecificActivity_id = Convert.ToInt32(data["wpfAccount"]);
                        _SaveE.Caterer = data["Caterer"];
                        _SaveE.EventId = eventid;
                        context.SaveChanges();

                    }
                }
            }
        }
        public void SaveEvent_revision(System.Web.Mvc.FormCollection data)
        {
            int userid = Convert.ToInt32(USER.C_swipeID);
            int doc_details_id = Convert.ToInt32(data["doc_details_id"]);

            string cmdStr = "SELECT [EventId] FROM [fmis].[Accounting].[tbl_t_DocEvents_revision] where [revision_Id]= @revision_Id";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@revision_Id", doc_details_id);


                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == false)
                {

                    using (fmisEntities context = new fmisEntities())
                    {
                        tbl_t_DocEvents_revision _SaveE = new tbl_t_DocEvents_revision();


                        _SaveE.DTE_from = data["DTE_from"];
                        _SaveE.DTE_to = data["DTE_to"];
                        _SaveE.EventId = Convert.ToInt32(data["EventId"]);
                        _SaveE.iscancel = Convert.ToInt32(data["iscancel"]);
                        //_SaveE.officeid = Convert.ToInt32(data["officeid"]);
                        _SaveE.Reason = data["Reason"];
                        _SaveE.revision_Id = doc_details_id;
                        _SaveE.userid = userid.ToString();
                        _SaveE.venue_from = data["venue_from"];
                        _SaveE.venue_to = data["venue_to"];
                        _SaveE.venue_to = data["venue_to"];

                        context.tbl_t_DocEvents_revision.Add(_SaveE);
                        context.SaveChanges();

                    }
                }
                else
                {
                    using (fmisEntities context = new fmisEntities())
                    {
                        var _SaveE = context.tbl_t_DocEvents_revision.Where(m => m.revision_Id == doc_details_id).FirstOrDefault();
                        _SaveE.DTE_from = data["DTE_from"];
                        _SaveE.DTE_to = data["DTE_to"];
                        _SaveE.EventId = Convert.ToInt32(data["EventId"]);
                        _SaveE.iscancel = Convert.ToInt32(data["iscancel"]);
                        //_SaveE.officeid = Convert.ToInt32(data["officeid"]);
                        _SaveE.Reason = data["Reason"];
                        _SaveE.userid = userid.ToString();
                        _SaveE.venue_from = data["venue_from"];
                        _SaveE.venue_to = data["venue_to"];

                        context.SaveChanges();

                    }
                }
            }
        }
        private bool IsNumeric(string value)
        {
            int result;
            return int.TryParse(value, out result);
        }
        public ActionResult save_Doc_details(System.Web.Mvc.FormCollection data)
        {

            var recommendApprovalValue = data["recommendApproval"];
            var SigApprovedValue = data["SigApproved"];


            TrackingController trackController = new TrackingController();
            int report_id = Convert.ToInt32(data["report_id"]);
            int doc_form_id = Convert.ToInt32(data["doc_form_id"]);
            int doc_details_id = Convert.ToInt32(data["doc_details_id"]);
            int officeChargewfp = 0;
            string refno = data["refno"];
            if (check_if_DGsign(doc_details_id) > 0)
            {
                return Json(new { code = 5, statusName = "The document has already been signed and cannot be modified or deleted." });
            }

            string wpfAccount = "";


            string signatories = "";
            string signatoriesType = "";


            string[] recommendApprovalArray;

            // Get the last value
            string lastValue;

            // Repeat the last value for the number of items in the array
            string recommendApprovalValuearray;






            if (report_id == 47 || report_id == 46)
            {
                //if (IsNumeric(recommendApprovalValue) == false)
                //{
                //    return Json(new { code = 5, statusName = "Please select a recommending approval in the dropdown list." });
                //}

                if (IsNumeric(SigApprovedValue) == false)
                {
                    return Json(new { code = 5, statusName = "Please select a approving signatory in the dropdown list." });
                }

                wpfAccount = data["wpfAccount"].ToString();

                signatories = USER.C_eID.ToString() + ',' + recommendApprovalValue + ',' + SigApprovedValue;







                if (data["officeChargewfp"].ToString() == "")
                {
                    return Json(new { code = 5, statusName = "Please select office charge...!" });
                }
                if (wpfAccount == "" || wpfAccount == null)
                {
                    return Json(new { code = 5, statusName = "WFP Account is required" });
                }

                //for upload
                officeChargewfp = Convert.ToInt32(data["officeChargewfp"]);
            }
            else if (report_id == 48 || report_id == 49 || report_id == 50)
            {

                wpfAccount = data["wpfAccount"].ToString();
                //if (IsNumeric(recommendApprovalValue) == false)
                //{
                //    return Json(new { code = 5, statusName = "Please select a recommending approval in the dropdown list." });
                //}
                if (IsNumeric(SigApprovedValue) == false)
                {
                    return Json(new { code = 5, statusName = "Please select a approving signatory in the dropdown list." });
                }

                signatories = USER.C_eID.ToString() + ',' + recommendApprovalValue + ',' + SigApprovedValue;


                if (data["officeChargewfp"].ToString() == "")
                {
                    return Json(new { code = 5, statusName = "Please select office charge...!" });
                }
                if (wpfAccount == "" || wpfAccount == null)
                {
                    return Json(new { code = 5, statusName = "WFP Account is required" });
                }
                if (recommendApprovalValue == "" || recommendApprovalValue == null)
                {
                    return Json(new { code = 5, statusName = "Recommending Approval signatory is required" });
                }
                if (SigApprovedValue == "" || SigApprovedValue == null)
                {
                    return Json(new { code = 5, statusName = "Approve signatory is required" });
                }

                //for upload
                officeChargewfp = Convert.ToInt32(data["officeChargewfp"]);
                SaveEvent(data);
                trackController.save_status_log(refno, 374, "", USER.C_eID);
            }
            else if (report_id == 53 || report_id == 54 || report_id == 55 || report_id == 57 || report_id == 58)
            {
                recommendApprovalArray = recommendApprovalValue.Split(',');

                // Get the last value
                lastValue = recommendApprovalArray.Last();

                // Repeat the last value for the number of items in the array
                recommendApprovalValuearray = string.Join(",", Enumerable.Repeat("2", recommendApprovalArray.Length));


                signatoriesType = "1," + recommendApprovalValuearray + ",3";

                int SpecificActivity_id;
                wpfAccount = data["SpecificActivity"].ToString();
                if (data["wpfAccount"].ToString() == "")
                {
                    SpecificActivity_id = 0;

                }
                else
                {
                    SpecificActivity_id = Convert.ToInt32(data["wpfAccount"]);
                }

                //if (IsNumeric(recommendApprovalValue) == false)
                //{
                //    return Json(new { code = 5, statusName = "Please select a recommending approval in the dropdown list." });
                //}
                if (IsNumeric(SigApprovedValue) == false)
                {
                    return Json(new { code = 5, statusName = "Please select a approving signatory in the dropdown list." });
                }
                signatories = USER.C_eID.ToString() + ',' + recommendApprovalValue + ',' + SigApprovedValue;




                if (data["officeChargewfp"].ToString() == "")
                {
                    return Json(new { code = 5, statusName = "Please select office charge...!" });
                }
                if (SpecificActivity_id < 1)
                {
                    return Json(new { code = 5, statusName = "WFP Account is required" });
                }

                //for upload
                officeChargewfp = Convert.ToInt32(data["officeChargewfp"]);
                SaveEvent(data);
                trackController.save_status_log(refno, 374, "", USER.C_eID);
                //ws.save_status_log()
            }
            else if (report_id == 56 || report_id == 59)
            {
                recommendApprovalArray = recommendApprovalValue.Split(',');

                // Get the last value
                lastValue = recommendApprovalArray.Last();

                // Repeat the last value for the number of items in the array
                recommendApprovalValuearray = string.Join(",", Enumerable.Repeat("2", recommendApprovalArray.Length));


                signatoriesType = "1," + recommendApprovalValuearray + ",3";

                signatories = USER.C_eID.ToString() + ',' + recommendApprovalValue + ',' + SigApprovedValue;


                //for upload
                SaveEvent_revision(data);
                trackController.save_status_log(refno, 411, "", USER.C_eID);
                //ws.save_status_log()
            }

            DataTable rec_column = new DataTable();

            rec_column = OleDbHelper.ExecuteDataset(dbcon_fmis, System.Data.CommandType.Text, "select doc_column_id,columnName,docDetails_value_id from fmis.[Accounting].[ufn_get_doc_details_column](" + report_id + "," + doc_details_id + ")").Tables[0];
            foreach (DataRow row in rec_column.Rows)
            {
                string columnName = row["columnName"].ToString();
                Int64 doc_column_id = Convert.ToInt64(row["doc_column_id"]);
                Int64 docDetails_value_id = Convert.ToInt64(row["docDetails_value_id"]);

                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "EXECUTE fmis.[Accounting].[usp_Save_DocDetailsWithValue_DGSign_v1]  @doc_details_id ,@report_id ,@doc_form_id ,@userid ,@docDetails_value_id ,@doc_column_id ,@value,@officeCharge,@wfp_details,@signatories,@signatoriesType";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@doc_details_id", SqlDbType.Int).Value = doc_details_id;

                    command.Parameters.Add("@report_id", SqlDbType.Int).Value = report_id;
                    command.Parameters.Add("@doc_form_id", SqlDbType.Int).Value = doc_form_id;
                    command.Parameters.Add("@userid", SqlDbType.Int).Value = USER.C_swipeID;
                    command.Parameters.Add("@docDetails_value_id", SqlDbType.Int).Value = docDetails_value_id;
                    command.Parameters.Add("@doc_column_id", SqlDbType.Int).Value = doc_column_id;
                    if (report_id == 48 || report_id == 49 || report_id == 50)
                    {
                        command.Parameters.AddWithValue("@value", "");
                    }
                    else if (report_id == 53 || report_id == 54 || report_id == 55 || report_id == 56 || report_id == 57 || report_id == 58 || report_id == 59)
                    {
                        command.Parameters.AddWithValue("@value", "");
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@value", data[columnName]);
                    }

                    command.Parameters.AddWithValue("@officeCharge", officeChargewfp);
                    command.Parameters.AddWithValue("@wfp_details", wpfAccount);
                    command.Parameters.AddWithValue("@signatories", signatories);
                    command.Parameters.AddWithValue("@signatoriesType", signatoriesType);
                    connection.Open();
                    SqlDataReader read = command.ExecuteReader();
                    if (read.HasRows == true)
                    {
                        while (read.Read())
                        {
                            doc_details_id = Convert.ToInt32(read["doc_details_id"]);
                        }
                    }
                }

                connection.Close();
            }

            Int32 docid = 0;
            docid = getdocid(data["refno"].ToString(), Convert.ToInt32(data["report_id"]));
            if (docid == 0)
            {
                try
                {
                    eps.SaveDocument(Convert.ToInt32(data["officeid"]), data["particular"].Replace("'", "''"), USER.C_eID, data["refno"]);
                }
                catch (Exception e)
                {
                    return Json(new { code = 6, statusName = "Successfully Save, but encounter error during sending the data to DTS..!", doc_details_id = doc_details_id });
                }
            }
            return Json(new { code = 6, statusName = "Successfully Saved..!", doc_details_id = doc_details_id });
        }


        private Int32 getdocid(string refno, Int32 report_id)
        {
            return Convert.ToInt32(ISfn.ExecScalar("select [Accounting].[ufn_checkIfExstsInDTS] ('" + refno.AntiInject() + "'," + report_id + ") as r"));
        }

        public ActionResult save_Doc_DV(tbl_t_DV_Details data)
        {
            int userid = Convert.ToInt32(USER.C_swipeID);
            Int32 dvid;

            if (data.Particular == null)
            {
                return Json(new { code = 7, statusName = "The Particular is required" });
            }
            if (data.Particular == null)
            {
                return Json(new { code = 7, statusName = "The Particular is required" });
            }
            if (data.ooe == null)
            {
                return Json(new { code = 7, statusName = "The allotment class is required" });
            }

            if (data.RCenter < 1)
            {
                return Json(new { code = 7, statusName = "Invalid Responsibility center" });
            }

            if (data.transtype_id < 1)
            {
                return Json(new { code = 7, statusName = "Invalid Transaction Type" });
            }

            data.Claimantcode = data.Claimantcode ?? "";

            //if (data.Claimantcode.ToString().Length < 1)
            //{
            //    return Json(new { code = 7, statusName = "Claimant Required" });
            //}

            data.fundID = data.fundID ?? 0;
            if (data.fundID < 2)
            {
                return Json(new { code = 7, statusName = "Fundtype Required" });
            }

            if (data.Gamount < 1)
            {
                return Json(new { code = 7, statusName = "Gross Amount Required" });
            }

            if (data.countperson == null)
            {
                data.countperson = 1;
            }

            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute [Accounting].[usp_Save_DV_details] @DVid,@Particular,@Claimantcode,@Gamount ,@Transtype_id ,@RCenter,@fundID,@UserID,@doc_form_id,@countperson,@ooe";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@DVid", data.DVid);
                command.Parameters.AddWithValue("@Particular", data.Particular);
                command.Parameters.AddWithValue("@Claimantcode", data.Claimantcode);
                command.Parameters.AddWithValue("@Gamount", data.Gamount);
                command.Parameters.AddWithValue("@Transtype_id", data.transtype_id);
                command.Parameters.AddWithValue("@RCenter", data.RCenter);
                command.Parameters.AddWithValue("@fundID", data.fundID);
                command.Parameters.AddWithValue("@UserID", userid);
                command.Parameters.AddWithValue("@doc_form_id", data.doc_form_id);
                command.Parameters.AddWithValue("@ooe", data.ooe);
                command.Parameters.AddWithValue("@countperson", data.countperson);
                connection.Open();
                dvid = (Int32)command.ExecuteScalar();

                connection.Close();
                return Json(new { code = 6, statusName = "Successfully Saved..!", dvid = dvid });
            }
        }
        public ActionResult save_head_logo(Int32 doc_details_id, int logoID, int isRightlogo)
        {
            int userid = Convert.ToInt32(USER.C_swipeID);

            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute [Accounting].[usp_Save_headlogos]  @doc_details_id,@logoID,@isRightlogo";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@doc_details_id", doc_details_id);
                command.Parameters.AddWithValue("@logoID", logoID);
                command.Parameters.AddWithValue("@isRightlogo", isRightlogo);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return Json(new { code = 6, statusName = "Successfully Saved..!" });
            }
        }

        public ActionResult update_status_log(string refno, int status_code, string remarks)
        {
            TrackingController trackController = new TrackingController();
            string statusName;
            int code;
            string result = trackController.save_status_AD_log(refno, status_code, remarks, Convert.ToInt32(USER.C_eID));

            if (result == "success")
            {
                statusName = "Successfully log";
                code = 6;
            }
            else if (result == "invalid")
            {
                statusName = "Invalid Tracking reference";
                code = 7;
            }
            else if (result == "already")
            {
                statusName = "Already Done";
                code = 7;
            }
            else
            {
                statusName = "Please update the status first to \" " + result + "\"";
                code = 7;
            }
            return Json(new { code = code, statusName = statusName });
        }

        public ActionResult Load_phoneno(int eid)
        {
            return Json(new { phoneno = ISfn.Get_employee_phoneno(eid) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult print_tracking(string refno)
        {
            string strUrl;
            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=6&refno=" + refno + "").ToString();
            return JavaScript("window.open('" + strUrl + "')");
        }
        public ActionResult print_log(DateTime from, DateTime to, int status_id)
        {
            string strUrl;
            strUrl = ISfn.UrlStr("~/Report/rpt_Viewer.aspx?rptid=21&from=" + from.ToString("MM/dd/yyyy") + "&to=" + to.ToString("MM/dd/yyyy") + "&status_id=" + status_id + "").ToString();
            return JavaScript("window.open('" + strUrl + "')");
        }
        public ActionResult data_grid_tracking_list([DataSourceRequest] DataSourceRequest request)
        {
            DataTable rec = new DataTable();
            string cmdStr = "Execute Tracking.[usp_grid_tracking_list] " + USER.C_swipeID + " ";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult data_grid_attach_doc([DataSourceRequest] DataSourceRequest request, int doc_form_id)
        {
            DataTable rec = new DataTable();
            string cmdStr = "execute [Accounting].[usp_grid_attach_doc] " + doc_form_id + "";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult data_grid_document([DataSourceRequest] DataSourceRequest request)
        {
            DataTable rec = new DataTable();
            string cmdStr = "execute [Accounting].[usp_grid_document] @userid";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult data_grid_tracking_list_Received([DataSourceRequest] DataSourceRequest request)
        {
            
            DataTable rec = new DataTable();
            string cmdStr = "execute Accounting.usp_grid_tracking_received " + USER.C_OfficeID + "";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult data_grid_tracking_list_review([DataSourceRequest] DataSourceRequest request, int statusCode)
        {
            DataTable rec = new DataTable();
            string cmdStr = "execute [Accounting].[usp_grid_tracking_review] " + statusCode + "";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult data_grid_tracking([DataSourceRequest] DataSourceRequest request, string parameter)
        {
            DataTable rec = new DataTable();
            string cmdStr = "execute [Accounting].[usp_grid_tracking] '" + parameter.AntiInject() + "'";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult data_grid_log_list([DataSourceRequest] DataSourceRequest request, DateTime from, DateTime to, int status_id)
        {
            IEnumerable<ufn_Log_status_list_Result> rec;
            rec = fmisdb.ufn_Log_status_list(from, to, status_id).OrderBy(o => o.Claimantname);
            return Json(rec.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult data_grid_tracking_list_logout([DataSourceRequest] DataSourceRequest request)
        {
            DataTable rec = new DataTable();
            string cmdStr = "execute Accounting.usp_grid_tracking_logout " + USER.C_OfficeID + "";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult data_grid_tracking_list_forRelease([DataSourceRequest] DataSourceRequest request)
        {
            DataTable rec = new DataTable();
            string cmdStr = "execute Accounting.[usp_grid_tracking_forRelease] " + USER.C_OfficeID + "";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DataSource_GetOffice()
        {
            IEnumerable<ufn_pmis_officename_Result> office;
            office = fmisdb.ufn_pmis_officename().OrderBy(o => o.OfficeName);
            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_GetOffice_fmis()
        {
            IEnumerable<ufn_fmis_officename_Result> office;
            office = fmisdb.ufn_fmis_officename().OrderBy(o => o.OfficeName);
            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_GetWFP_details(int year, int officeid)
        {
            IEnumerable<ufn_WFP_details_Result> office;
            office = fmisdb.ufn_WFP_details(year, officeid).OrderBy(o => o.wfp_description);

            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_GetVenue()
        {
            IEnumerable<ufn_venue_Result> office;
            office = fmisdb.ufn_venue().OrderBy(o => o.venue);
            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }

        public async System.Threading.Tasks.Task<ActionResult> datasource_activitySchedule(int year, int officeid, Boolean isPLC)
        {
            //try
            //{
            if (officeid == 0)
            {
                officeid = 9999999;
            }

            using (HttpClient httpClient = new HttpClient())
            {

                httpClient.BaseAddress = new Uri("http://192.168.101.56/spms/home/");
                HttpResponseMessage response = await httpClient.GetAsync($"getTentativeSchedules?year=2024&officeid={officeid}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<SpecificActivityInfo>>(responseData);
                    //var filteredData = data.Where(item => item.isplc == isPLC);
                    // Group data by SpecificActivity and AccountDenominationId
                    var groupedData = data.GroupBy(item => new { item.SpecificActivity, item.SpecificActivity_id })
                                          .Select(group => new
                                          {
                                              SpecificActivity = group.Key.SpecificActivity.Trim(),
                                              SpecificActivity_id = group.Key.SpecificActivity_id,
                                              Count = group.Count()
                                          });

                    return Json(groupedData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed to fetch data from the API", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult datasource_activitySchedule_2024(int year, int officeid, Boolean isPLC)
        {
            if (officeid == 0)
            {
                officeid = 9999999;
            }

            // Define the SQL query with parameters for year and officeid
            string sqlQuery = @"execute [Accounting].[usp_ActivityTraining_Budgitaries_2025] @year=@YearParam, @officeid = @OfficeIDParam";
            using (SqlConnection conn = new SqlConnection(fmisConn))
            {
                conn.Open();  // Changed to synchronous Open()

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    // Add parameters for year and officeid
                    cmd.Parameters.AddWithValue("@YearParam", year);
                    cmd.Parameters.AddWithValue("@OfficeIDParam", officeid);

                    using (SqlDataReader reader = cmd.ExecuteReader())  // Changed to synchronous ExecuteReader()
                    {
                        var data = new List<SpecificActivityInfo>();

                        while (reader.Read())  // Changed to synchronous Read()
                        {
                            data.Add(new SpecificActivityInfo
                            {
                                SpecificActivity_id = reader.GetInt32(reader.GetOrdinal("specid")),
                                SpecificActivity = reader.GetString(reader.GetOrdinal("SpecificActivity")).Trim()
                            });
                        }

                        // Group data by SpecificActivity and SpecificActivity_id
                        var groupedData = data.GroupBy(item => new { item.SpecificActivity, item.SpecificActivity_id })
                                              .Select(group => new
                                              {
                                                  SpecificActivity = group.Key.SpecificActivity,
                                                  SpecificActivity_id = group.Key.SpecificActivity_id,
                                                  Count = group.Count()
                                              });

                        return Json(groupedData, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }
        public ActionResult datasource_activitySchedule_2025(int year, int officeid, Boolean isPLC)
        {
            if (officeid == 0)
            {
                officeid = 9999999;
            }

            // Define the SQL query with parameters for year and officeid
            string sqlQuery = @"execute [Accounting].[usp_ActivityTraining_Budgitaries_2025] @year=@YearParam, @officeid = @OfficeIDParam";
            using (SqlConnection conn = new SqlConnection(fmisConn))
            {
                conn.Open();  // Changed to synchronous Open()

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    // Add parameters for year and officeid
                    cmd.Parameters.AddWithValue("@YearParam", year);
                    cmd.Parameters.AddWithValue("@OfficeIDParam", officeid);

                    using (SqlDataReader reader = cmd.ExecuteReader())  // Changed to synchronous ExecuteReader()
                    {
                        var data = new List<SpecificActivityInfo>();

                        while (reader.Read())  // Changed to synchronous Read()
                        {
                            data.Add(new SpecificActivityInfo
                            {
                                SpecificActivity_id = reader.GetInt32(reader.GetOrdinal("specid")),
                                SpecificActivity = reader.GetString(reader.GetOrdinal("SpecificActivity")).Trim()
                            });
                        }

                        // Group data by SpecificActivity and SpecificActivity_id
                        var groupedData = data.GroupBy(item => new { item.SpecificActivity, item.SpecificActivity_id })
                                              .Select(group => new
                                              {
                                                  SpecificActivity = group.Key.SpecificActivity,
                                                  SpecificActivity_id = group.Key.SpecificActivity_id,
                                                  Count = group.Count()
                                              });

                        return Json(groupedData, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }


        public ActionResult GetDocTitle(int officeid)
        {
            using (var db = new fmisEntities())
            {
                var query = @"SELECT EventId, refno + ' - ' + Title as Title
                      FROM [fmis].[Accounting].[tbl_t_DocEvents] as a
                      INNER JOIN Accounting.tbl_t_DocDetails as b ON b.doc_details_id = a.EventId AND Title IS NOT NULL
                      INNER JOIN Accounting.tbl_t_DocForm as c ON c.doc_form_id = b.doc_form_id
                      INNER JOIN Tracking.tbl_t_transactionDetails as d ON d.Unique_refno = refno
                      INNER JOIN Tracking.tbl_t_transactionDetails_log as e ON e.trans_id = d.trans_id AND e.status_code = 385 AND isActive = 1 AND officeId = " + officeid + "";

                var docEvents = db.Database.SqlQuery<DocTitile>(query).ToList();
                return Json(docEvents, JsonRequestBehavior.AllowGet);
            }
        }


        public async Task<ActionResult> datasource_items(int year, int officeid, Boolean isPLC)
        {
            //try
            //{
            using (HttpClient httpClient = new HttpClient())
            {

                httpClient.BaseAddress = new Uri("http://192.168.101.56/spms/home/");
                HttpResponseMessage response = await httpClient.GetAsync($"getTentativeSchedules?year=2024&officeid={officeid}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<activity_item>>(responseData);

                    var groupedData = data.Select(select => new
                    {
                        item = select.item,
                        itemid = select.itemid,
                        price = select.price,
                    });
                    return Json(groupedData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed to fetch data from the API", JsonRequestBehavior.AllowGet);
                }
            }
        }
        private async void Populateitems(int year, int officeid, Boolean isPLC, string specificActivity)
        {
            using (HttpClient httpClient = new HttpClient())
            {

                httpClient.BaseAddress = new Uri("http://192.168.101.56/spms/home/");
                HttpResponseMessage response = await httpClient.GetAsync($"getTentativeSchedules?year=2024&officeid={officeid}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<activity_item>>(responseData);

                    var filteredData = data.Where(item => item.isplc == isPLC);
                    var groupedData = filteredData.Select(select => new
                    {
                        item = select.item + $"(Price:{select.price})",
                        itemid = select.itemid,
                        price = select.price,
                    });
                    ViewBag.items = groupedData.ToList();
                }
            }
        }
        public async Task<ActionResult> datasource_Venue(int year, int officeid, Boolean isPLC)
        {
            //try
            //{
            using (HttpClient httpClient = new HttpClient())
            {

                httpClient.BaseAddress = new Uri("http://192.168.101.56/spms/home/");
                HttpResponseMessage response = await httpClient.GetAsync($"getTentativeSchedules?year=2024&officeid={officeid}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<data_venue>>(responseData);
                    var filteredData = data.Where(item => item.isplc == isPLC);
                    // Group data by SpecificActivity and AccountDenominationId
                    var groupedData = filteredData.GroupBy(item => new { item.venue, item.venueid })
                                          .Select(group => new
                                          {
                                              venue = group.Key.venue,
                                              venueid = group.Key.venueid,
                                              Count = group.Count()
                                          });
                    return Json(groupedData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed to fetch data from the API", JsonRequestBehavior.AllowGet);
                }
            }
        }
        //public async Task<ActionResult> grid_ActivityDetails(int year, int officeid, Boolean isPLC)
        //{
        //    //try
        //    //{
        //    using (HttpClient httpClient = new HttpClient())
        //    {

        //        httpClient.BaseAddress = new Uri("http://192.168.101.56/spms/home/");
        //        HttpResponseMessage response = await httpClient.GetAsync($"getTentativeSchedules?year=2024&officeid={officeid}");

        //        if (response.IsSuccessStatusCode)
        //        {
        //            string responseData = await response.Content.ReadAsStringAsync();
        //            var data = JsonConvert.DeserializeObject<List<grid_budgetary_requirments>>(responseData);
        //            var filteredData = data.Where(item => item.isplc == isPLC);
        //            // Group data by SpecificActivity and AccountDenominationId
        //            var groupedData = filteredData
        //                                  .Select(group => new
        //                                  {
        //                                      item = group.item,
        //                                      itemid = group.itemid,
        //                                      SpecificActivity = group.SpecificActivity,
        //                                      qty = group.qty,
        //                                      price = group.price,
        //                                  });
        //            return Json(groupedData, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("Failed to fetch data from the API", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}
        public JsonResult DataSource_GetWFP_details_new(int year, int officeid)
        {
            IEnumerable<ufn_WFP_details_new_Result> office;
            office = fmisdb.ufn_WFP_details_new(year, officeid).OrderBy(o => o.wfp_description);
            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_GetOffice_fmis_pmis()
        {
            string id = User.Identity.GetUserId();
            IEnumerable<ufn_fmisPmis_byID_Result> office;
            office = fmisdb.ufn_fmisPmis_byID(id).OrderBy(o => o.OfficeName);
            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_liaison(Int16 officeid)
        {
            IEnumerable<ufn_liaison_officer_Result> office;
            office = fmisdb.ufn_liaison_officer(officeid).OrderBy(o => o.NameFML);

            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_employee()
        {
            IEnumerable<ufn_liaison_officer_Result> office;
            office = fmisdb.ufn_liaison_officer(0).OrderBy(o => o.NameFML);
            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DataSource_employee_recommendingApprove()
        {
            IEnumerable<ufn_recommending_officer_Result> office;
            office = fmisdb.ufn_recommending_officer(0).OrderBy(o => o.NameFML);
            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_DGSignApproved(int doc_details_id, int officeid)
        {
            DataTable rec = new DataTable();

            rec = OleDbHelper.ExecuteDataset(dbcon_fmis, System.Data.CommandType.Text, "execute [Accounting].[usp_Sig_Approved_employee] " + doc_details_id + "," + officeid + "").Tables[0];
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddl_log_staus_name()
        {
            IEnumerable<ufn_Status_DescriptionByUser_Result> office;
            office = fmisdb.ufn_Status_DescriptionByUser(Convert.ToInt32(USER.C_eID)).OrderBy(o => o.Name);
            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ddl_status_nameByUser()
        {
            IEnumerable<ufn_Log_status_Description_Result> office;
            office = fmisdb.ufn_Log_status_Description(Convert.ToInt32(USER.C_swipeID)).OrderBy(o => o.Name);
            return Json(office.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddl_reportName()
        {
            IEnumerable<ufn_ReportName_Result> reportname;
            reportname = fmisdb.ufn_ReportName(0).OrderBy(o => o.report_id);

            return Json(reportname.ToList(), JsonRequestBehavior.AllowGet);
        }
        //public void LoadJson()
        //{
        //    using (StreamReader r = new StreamReader("file.json"))
        //    {
        //        string json = r.ReadToEnd();
        //        List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
        //    }
        //}


        public JsonResult DataSource_status_log()
        {
            DataTable rec = new DataTable();

            rec = OleDbHelper.ExecuteDataset(dbcon_fmis, System.Data.CommandType.Text, "execute [Tracking].[usp_list_status_byOffice] " + USER.C_OfficeID + "").Tables[0];
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_statusname(int status_code)
        {
            DataTable rec = new DataTable();

            rec = OleDbHelper.ExecuteDataset(dbcon_fmis, System.Data.CommandType.Text, "select * from Tracking.[ufn_StatusnameByUser] (" + USER.C_eID + "," + status_code + ")").Tables[0];
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DataSource_transaction_type()
        {
            DataTable rec = new DataTable();

            rec = OleDbHelper.ExecuteDataset(dbcon_fmis, System.Data.CommandType.Text, "SELECT [Transtype_id],[TransactionName] FROM[fmis].[Accounting].[tbl_l_Transaction_type] where isActive = 1").Tables[0];
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _pv_expense_details(Int32 dvid)
        {
            return View("_Add_DV_entries");
        }

        public ActionResult grid_coa([DataSourceRequest] DataSourceRequest request)
        {
            DataTable crec;
            crec = ISfn.ToDatatable("select * from Accounting.ufn_Accountname_byExpense(5)", "@null", "");
            return Json(crec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult grid_entries([DataSourceRequest] DataSourceRequest request, int dvid)
        {
            IEnumerable<ufn_Expense_details_Result> rec;
            rec = fmisdb.ufn_Expense_details(dvid);
            return Json(rec.ToDataSourceResult(request, o => new { AccountName = o.AccountName, ChartAccountID = o.ChartAccountID, Expense_id = o.Expense_id, Amount = o.Amount, Code = o.Code }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult insert_t_expense(Int32 Expense_id, Int32 dvid, Int32 ChartAccountID, Int32 functionid, string Amount)
        {

            int userid = Convert.ToInt32(USER.C_swipeID);

            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute [Accounting].[usp_Save_expense_details] @Expense_id ,@dvid ,@ChartAccountID  ,@functionid , @Amount ,@userid ";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@Expense_id", Expense_id);
                command.Parameters.AddWithValue("@dvid", dvid);
                command.Parameters.AddWithValue("@ChartAccountID", ChartAccountID);
                command.Parameters.AddWithValue("@functionid", functionid);
                command.Parameters.AddWithValue("@Amount", Amount);
                command.Parameters.AddWithValue("@userid", userid);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return Json(new { code = 6, statusName = "Successfully Saved..!" });
            }
        }
        public ActionResult updateToCO(int eid, int value)
        {
            try
            {
                ISfn.ExcecuteNoneQuery("execute [dbo].[usp_Save_Payroll_Set_Override] " + eid + "," + value + " ");
                return Content("6");
            }
            catch (Exception e)
            {
                string r = ISfn.errorLog(e.HResult, e.Message, "RegularPayroll/Save_Membership_Allowance");
                return Content(r);
            }
        }

        public ActionResult callExe()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = Url.Content("D:\\PGAS\\Published\\WEBAOMS\\HeloWorld.exe");
            info.Arguments = "";
            info.WindowStyle = ProcessWindowStyle.Normal;
            Process pro = Process.Start(info);
            pro.WaitForExit();
            return Content("");
        }

        public ActionResult doc_details(Int64 doc_form_id, int document_type_id, int id)
        {
            Int32 DGdoc_id = Convert.ToInt32(ISfn.ExecScalar("select Accounting.[fns_checkDocIfSign](" + id + ") as result"));

            Int32 doc_details_id = Convert.ToInt32(ISfn.ExecScalar("select [Accounting].[fns_check_idDocexists](" + doc_form_id + ") as result"));

            if (doc_details_id == id)
            {
                if (DGdoc_id > 0)
                {
                    // return RedirectToAction("preview_dgsign", "DGSign", new { doc_form_id = DGdoc_id });
                    return Json(new { code = 8, statusName = getDGlink(DGdoc_id) });
                }
            }
            else if (doc_details_id > 0)
            {
                return Json(new { code = 5, statusName = "It seems that the document you're trying to attach is already included in the attachment. Please note that multiple attachments of the same document are not permitted." });
            }

            ViewBag.report_id = document_type_id;
            ViewBag.doc_form_id = doc_form_id;

            string cmdStr = "execute [Accounting].[usp_Doc_htmlScript_base] @report_id, @doc_details_id,@doc_form_id ,@userid ";

            SqlConnection connection = new SqlConnection(fmisConn);
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@report_id", SqlDbType.Int).Value = document_type_id;
                command.Parameters.Add("@doc_details_id", SqlDbType.Int).Value = id;
                command.Parameters.Add("@doc_form_id", SqlDbType.Int).Value = doc_form_id;
                command.Parameters.Add("@userid", SqlDbType.Int).Value = USER.C_swipeID;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ViewBag.html = reader["html"].ToString();
                        ViewBag.script = reader["script"].ToString().Replace("##style", ISfn.UrlStr("~/Content/styleEditor.css"));
                        ViewBag.doc_details_id = reader["doc_details_id"].ToString();
                        if (document_type_id == 48 || document_type_id == 49 || document_type_id == 50 || document_type_id == 56)
                        {
                            ViewBag.officeCharge = reader["officeCharge"].ToString();
                            ViewBag.wfpAccount = reader["wfpAccount"].ToString();
                            ViewBag.recommendingApprove = reader["recommendingApprove"].ToString();
                            ViewBag.approveby = reader["approveby"].ToString();
                            ViewBag.InclusiveDate = reader["InclusiveDate"].ToString();
                            ViewBag.officeID = reader["OfficeID"].ToString();

                        }
                    }
                }
            }
            connection.Close();

            if (document_type_id == 48 || document_type_id == 49)
            {
                PopulateOffices();
                return PartialView("_Add_document_details", null);
            }
            if (document_type_id == 50 || document_type_id == 59)
            {
                PopulateOffices();
                return PartialView("_Add_document_details_process_design", null);
                //return PartialView("_Add_document_details_Final", null);
            }
            else
            {
                return PartialView("_Add_doc_details", null);
            }
        }
        public ActionResult get_grid_inclusive_date([DataSourceRequest] DataSourceRequest request, int doc_details_id)
        {
            string connectionString = fmisConn; // Replace with your actual connection string

            string query = @"SELECT [doc_details_id_incl], [InclusiveDate]
                             FROM [fmis].[Accounting].[tbl_t_DocDetails_inclusivedate] where doc_details_id = " + doc_details_id + " ORDER BY [InclusiveDate]";

            List<object> datesList = new List<object>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int docdetailsidincl = reader.GetInt32(0);
                    DateTime inclusiveDate = reader.GetDateTime(1);

                    // Create an object with the necessary properties
                    var dateObj = new
                    {
                        doc_details_id_incl = docdetailsidincl,
                        InclusiveDate = inclusiveDate.ToString("MM/dd/yyyy") // Format the date as needed
                    };

                    datesList.Add(dateObj);
                }

                reader.Close();
            }

            // Return the data as JSON
            return Json(datesList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        public ActionResult InsertInclusiveDate(DateTime InclusiveDate, int doc_details_id)
        {
            try
            {
                string query = @"INSERT INTO [fmis].[Accounting].[tbl_t_DocDetails_inclusivedate] (doc_details_id,[InclusiveDate])
                                 VALUES (@doc_details_id,@InclusiveDate)";
                using (SqlConnection connection = new SqlConnection(fmisConn))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@InclusiveDate", InclusiveDate);
                    command.Parameters.AddWithValue("@doc_details_id", doc_details_id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult RemoveInclusiveDate(int doc_details_id_incl)
        {
            try
            {
                string connectionString = fmisConn; // Replace with your actual connection string

                string query = @"DELETE FROM [fmis].[Accounting].[tbl_t_DocDetails_inclusivedate]
                                 WHERE [doc_details_id_incl] = @doc_details_id_incl";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@doc_details_id_incl", doc_details_id_incl);
                    connection.Open();

                    command.ExecuteNonQuery();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Action to fetch data from the database and send it to the Kendo UI Grid
        public JsonResult GetDocDetails(int doc_details_id)
        {
            using (SqlConnection connection = new SqlConnection(fmisConn))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT [doc_details_id_incl], [doc_details_id], [InclusiveDate] FROM [fmis].[Accounting].[tbl_t_DocDetails_inclusivedate] where doc_details_id = @doc_details_id", connection);
                command.Parameters.AddWithValue("@doc_details_id", doc_details_id);
                SqlDataReader reader = command.ExecuteReader();

                List<DocDetailsInclusiveDateModel> docDetailsList = new List<DocDetailsInclusiveDateModel>();

                while (reader.Read())
                {
                    DocDetailsInclusiveDateModel docDetails = new DocDetailsInclusiveDateModel();
                    docDetails.doc_details_id_incl = (int)reader["doc_details_id_incl"];
                    docDetails.doc_details_id = (int)reader["doc_details_id"];
                    docDetails.InclusiveDate = (DateTime)reader["InclusiveDate"];

                    docDetailsList.Add(docDetails);
                }

                return Json(docDetailsList, JsonRequestBehavior.AllowGet);
            }
        }

        // Action to handle the create operation
        public ActionResult CreateDocDetail(DocDetailsInclusiveDateModel docDetail)
        {
            using (SqlConnection connection = new SqlConnection(fmisConn))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO [fmis].[Accounting].[tbl_t_DocDetails_inclusivedate] ([doc_details_id], [InclusiveDate]) VALUES (@docDetailsId, @inclusiveDate)", connection);
                command.Parameters.AddWithValue("@docDetailsId", docDetail.doc_details_id);
                command.Parameters.AddWithValue("@inclusiveDate", docDetail.InclusiveDate);
                command.ExecuteNonQuery();

                return Json(new { success = true });
            }
        }

        // Action to handle the update operation
        public JsonResult UpdateDocDetail(DocDetailsInclusiveDateModel docDetail)
        {
            using (SqlConnection connection = new SqlConnection(fmisConn))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE [fmis].[Accounting].[tbl_t_DocDetails_inclusivedate] SET [InclusiveDate] = @inclusiveDate WHERE [doc_details_id_incl] = @docDetailsIdIncl", connection);
                command.Parameters.AddWithValue("@inclusiveDate", docDetail.InclusiveDate);
                command.Parameters.AddWithValue("@docDetailsIdIncl", docDetail.doc_details_id_incl);

                command.ExecuteNonQuery();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        // Action to handle the delete operation

        public JsonResult DeleteDocDetail(int docDetailsIdIncl)
        {
            using (SqlConnection connection = new SqlConnection(fmisConn))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM [fmis].[Accounting].[tbl_t_DocDetails_inclusivedate] WHERE [doc_details_id_incl] = @docDetailsIdIncl", connection);
                command.Parameters.AddWithValue("@docDetailsIdIncl", docDetailsIdIncl);

                command.ExecuteNonQuery();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult TPEditingInline_Read([DataSourceRequest] DataSourceRequest request, int eventId)
        {
            IEnumerable<tbl_t_DocEvents_TargetParticipants> targetPar;
            if (eventId == 0)
            {
                targetPar = fmisdb.tbl_t_DocEvents_TargetParticipants.Where(M => M.EventId == null);
            }
            else
            {
                targetPar = fmisdb.tbl_t_DocEvents_TargetParticipants.Where(M => M.EventId == eventId);
            }

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(targetPar.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;

        }

        public void insertEvent(int doc_details_id)
        {
            string cmdStr = "SELECT [EventId] FROM [fmis].[Accounting].[tbl_t_DocEvents] where EventID= @doc_details_id";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@doc_details_id", doc_details_id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == false)
                {

                    using (fmisEntities context = new fmisEntities())
                    {
                        tbl_t_DocEvents _SaveE = new tbl_t_DocEvents();
                        _SaveE.EventId = doc_details_id;
                        context.tbl_t_DocEvents.Add(_SaveE);
                        context.SaveChanges();
                    }
                }
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TPEditingInline_Create([DataSourceRequest] DataSourceRequest request, tbl_t_DocEvents_TargetParticipants targetparticipant, int doc_details_id)
        {
            insertEvent(doc_details_id);
            if (targetparticipant != null && ModelState.IsValid)
            {
                using (fmisEntities context = new fmisEntities())
                {
                    if (targetparticipant.ParticipantId == 0)
                    {
                        tbl_t_DocEvents_TargetParticipants _SaveTP = new tbl_t_DocEvents_TargetParticipants();
                        _SaveTP.EventId = doc_details_id;
                        _SaveTP.ParticipantId = targetparticipant.ParticipantId;
                        _SaveTP.Participant = targetparticipant.Participant;
                        _SaveTP.NoOfPax = targetparticipant.NoOfPax;
                        _SaveTP.OfficeID = targetparticipant.OfficeID;
                        context.tbl_t_DocEvents_TargetParticipants.Add(_SaveTP);
                        context.SaveChanges();
                        int savedParticipantId = _SaveTP.ParticipantId;
                        targetparticipant.ParticipantId = savedParticipantId;
                    }
                    else
                    {
                        var tp = context.tbl_t_DocEvents_TargetParticipants.Where(m => m.ParticipantId == targetparticipant.ParticipantId).FirstOrDefault();
                        tp.Participant = targetparticipant.Participant;
                        tp.NoOfPax = targetparticipant.NoOfPax;
                        tp.OfficeID = targetparticipant.OfficeID;
                        context.SaveChanges();
                    }

                }
            }

            return Json(new[] { targetparticipant }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TPEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, tbl_t_DocEvents_TargetParticipants targetparticipant)
        {
            if (targetparticipant != null)
            {
                using (fmisEntities context = new fmisEntities())
                {
                    var tp = context.tbl_t_DocEvents_TargetParticipants.Where(m => m.ParticipantId == targetparticipant.ParticipantId).FirstOrDefault();
                    context.tbl_t_DocEvents_TargetParticipants.Remove(tp);
                    context.SaveChanges();
                }
            }

            return Json(new[] { targetparticipant }.ToDataSourceResult(request, ModelState));
        }

        //Budgetary Requirement


        public ActionResult BREditingInline_Read([DataSourceRequest] DataSourceRequest request, int eventId)
        {
            IEnumerable<tbl_t_DocEvents_BudgetaryRequirements> budgetReg;
            if (eventId == 0)
            {
                budgetReg = fmisdb.tbl_t_DocEvents_BudgetaryRequirements.Where(M => M.EventId == 0);
            }
            else
            {
                budgetReg = fmisdb.tbl_t_DocEvents_BudgetaryRequirements.Where(M => M.EventId == eventId);
            }



            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(budgetReg.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AutoInsert_DocEventsAndBR(int doc_details_id, int specificActivityID, string specificactivity)
        {
            insertEvent(doc_details_id);
            string cmdStr = "execute [Accounting].[usp_insert_docEvetAndBR] @EventID=" + doc_details_id + ", @specificActivityID=" + specificActivityID + ",@specificactivity = '" + specificactivity.AntiInject() + "',@year = 2024 ";
            //@eventid int = 0, @specificActivityID int = 8155, @specificactivity varchar(400),@year int = 2024
            ISfn.ExcecuteNoneQuery(cmdStr);
            return Json(new { code = 6, statusName = "Successfully Saved..!" });
        }

        public ActionResult AutoInsert_DocEventsAndBR_2025(int doc_details_id, int specificActivityID, int officeID)
        {
            insertEvent(doc_details_id);
            string cmdStr = "execute [Accounting].[usp_insert_docEvetAndBR_2025] @EventID=" + doc_details_id + ", @specificActivityID=" + specificActivityID + ",@officeid = " + officeID + ",@year = 2026 ";
            //@eventid int = 0, @specificActivityID int = 8155, @specificactivity varchar(400),@year int = 2024
            ISfn.ExcecuteNoneQuery(cmdStr);
            return Json(new { code = 6, statusName = "Successfully Saved..!" });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BREditingInline_Create([DataSourceRequest] DataSourceRequest request, tbl_t_DocEvents_BudgetaryRequirements targetparticipant, int doc_details_id)
        {
            insertEvent(doc_details_id);

            if (targetparticipant.item_price > targetparticipant.OrginalPrice)
            {
                ModelState.AddModelError("Item Price", "Warning: Your item price must be less or equal to " + targetparticipant.OrginalPrice + "");
            }
            else if (targetparticipant.item_qty > targetparticipant.OriginalQty)
            {
                ModelState.AddModelError("Item Price", "Warning: Your quantity must be less or equal to " + targetparticipant.OriginalQty + ".");
            }
            else
            {
                if (targetparticipant != null && ModelState.IsValid)
                {
                    using (fmisEntities context = new fmisEntities())
                    {
                        if (targetparticipant.BudgetId == 0)
                        {
                            //tbl_t_DocEvents_BudgetaryRequirements _SaveBR = new tbl_t_DocEvents_BudgetaryRequirements();
                            //_SaveBR.EventId = doc_details_id;
                            //_SaveBR.BudgetId = targetparticipant.BudgetId;
                            //_SaveBR.Particular = targetparticipant.Particular;
                            //_SaveBR.Amount = targetparticipant.item_qty * targetparticipant.item_price;
                            //_SaveBR.item_qty = targetparticipant.item_qty;
                            //_SaveBR.item_id = targetparticipant.item_id;
                            //_SaveBR.item_price = targetparticipant.item_price;

                            //context.tbl_t_DocEvents_BudgetaryRequirements.Add(_SaveBR);
                            //context.SaveChanges();
                            //int savedBudgetId = _SaveBR.BudgetId;
                            //targetparticipant.BudgetId = savedBudgetId;
                        }
                        else
                        {
                            var _SaveBR = context.tbl_t_DocEvents_BudgetaryRequirements.Where(m => m.BudgetId == targetparticipant.BudgetId).FirstOrDefault();
                            _SaveBR.Particular = targetparticipant.Particular;
                            _SaveBR.Amount = targetparticipant.item_qty * targetparticipant.item_price;
                            _SaveBR.item_qty = targetparticipant.item_qty;
                            _SaveBR.item_id = targetparticipant.item_id;
                            _SaveBR.item_price = targetparticipant.item_price;
                            context.SaveChanges();
                        }

                    }
                }
            }


            return Json(new[] { targetparticipant }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BREditingInline_Destroy([DataSourceRequest] DataSourceRequest request, tbl_t_DocEvents_BudgetaryRequirements targetparticipant)
        {
            if (targetparticipant != null)
            {
                using (fmisEntities context = new fmisEntities())
                {
                    var tp = context.tbl_t_DocEvents_BudgetaryRequirements.Where(m => m.BudgetId == targetparticipant.BudgetId).FirstOrDefault();
                    context.tbl_t_DocEvents_BudgetaryRequirements.Remove(tp);
                    context.SaveChanges();
                }
            }

            return Json(new[] { targetparticipant }.ToDataSourceResult(request, ModelState));
        }

        ////IPS


        public ActionResult IPSEditingInline_Read([DataSourceRequest] DataSourceRequest request, int eventId)
        {
            IEnumerable<tbl_t_DocEvents_IPS> IPS_IdReg;
            if (eventId == 0)
            {
                IPS_IdReg = fmisdb.tbl_t_DocEvents_IPS.Where(M => M.EventId == 0);
            }
            else
            {
                IPS_IdReg = fmisdb.tbl_t_DocEvents_IPS.Where(M => M.EventId == eventId).OrderBy(o => o.orderby);
            }


            foreach (var item in IPS_IdReg)
            {
                if (item.Activity == null)
                {
                    item.Activity = ""; // Replace null with an empty string
                }

                if (item.DTE == null)
                {
                    item.DTE = ""; // Replace null with an empty string
                }

                if (item.AssignPerson == null)
                {
                    item.AssignPerson = ""; // Replace null with an empty string
                }
            }

            // Replace null values with white spaces


            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(IPS_IdReg.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;

        }
        [HttpPost]
        public JsonResult save_Venue(string venueName, string venueAddress)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(fmisConn))
                {
                    con.Open();
                    string query = "INSERT INTO [BookingDB].[dbo].[VENUE_LOOKUP] (VENUE_NAME, DESCRIPTION) OUTPUT INSERTED.VENUE_ID VALUES (@VenueName, @VenueAddress)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@VenueName", venueName);
                        cmd.Parameters.AddWithValue("@VenueAddress", venueAddress);

                        int venueId = (int)cmd.ExecuteScalar(); // Get the inserted Venue ID

                        return Json(new { code = 6, statusName = "Venue saved successfully", venueId = venueId });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 0, statusName = "Error: " + ex.Message });
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult IPSEditingInline_Create([DataSourceRequest] DataSourceRequest request, tbl_t_DocEvents_IPS ips, int doc_details_id)
        {
            insertEvent(doc_details_id);
            if (ips.orderby == 0 || ips.orderby == null)
            {
                ips.orderby = Convert.ToInt16(ISfn.ExecScalar("select isnull(max(orderby),0) + 1 as maxOrderby from Accounting.tbl_t_DocEvents_IPS where eventid=" + doc_details_id + ""));
            }
            if (ips != null && ModelState.IsValid)
            {
                using (fmisEntities context = new fmisEntities())
                {
                    if (ips.IPS_Id == 0)
                    {

                        tbl_t_DocEvents_IPS _SaveIPS = new tbl_t_DocEvents_IPS();
                        _SaveIPS.EventId = doc_details_id;
                        _SaveIPS.IPS_Id = ips.IPS_Id;
                        _SaveIPS.Activity = ips.Activity;
                        _SaveIPS.DTE = ips.DTE;
                        _SaveIPS.bold = ips.bold;
                        _SaveIPS.userid = USER.C_swipeID;
                        _SaveIPS.orderby = ips.orderby;
                        _SaveIPS.AssignPerson = ips.AssignPerson;
                        context.tbl_t_DocEvents_IPS.Add(_SaveIPS);
                        context.SaveChanges();
                        int savedIPS_Id = _SaveIPS.IPS_Id;
                        ips.IPS_Id = savedIPS_Id;
                    }
                    else
                    {
                        ISfn.ExcecuteNoneQuery("update Accounting.tbl_t_DocEvents_IPS set orderby = orderby+ 1 where eventid=" + doc_details_id + " and orderby >= " + ips.orderby + "");

                        var _SaveBR = context.tbl_t_DocEvents_IPS.Where(m => m.IPS_Id == ips.IPS_Id).FirstOrDefault();
                        _SaveBR.Activity = ips.Activity;
                        _SaveBR.DTE = ips.DTE;
                        _SaveBR.AssignPerson = ips.AssignPerson;
                        _SaveBR.bold = ips.bold;
                        _SaveBR.orderby = ips.orderby;

                        context.SaveChanges();
                    }
                }
            }
            return Json(new[] { ips }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult IPSEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, tbl_t_DocEvents_IPS ips)
        {
            if (ips != null)
            {
                using (fmisEntities context = new fmisEntities())
                {
                    var tp = context.tbl_t_DocEvents_IPS.Where(m => m.IPS_Id == ips.IPS_Id).FirstOrDefault();
                    context.tbl_t_DocEvents_IPS.Remove(tp);
                    context.SaveChanges();
                }
            }

            return Json(new[] { ips }.ToDataSourceResult(request, ModelState));
        }


        //Process Design
        public ActionResult t_DocEvents_process_designEditingInline_Read([DataSourceRequest] DataSourceRequest request, int eventId)
        {
            IEnumerable<tbl_t_DocEvents_process_design> t_DocEvents_process_designIE;
            t_DocEvents_process_designIE = fmisdb.tbl_t_DocEvents_process_design.Where(w => w.EventId == eventId);
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(t_DocEvents_process_designIE.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult t_DocEvents_process_designEditingInline_Create([DataSourceRequest] DataSourceRequest request, tbl_t_DocEvents_process_design t_DocEvents_process_design, int eventId)
        {
            if (t_DocEvents_process_design.orderby == 0 || t_DocEvents_process_design.orderby == null)
            {
                t_DocEvents_process_design.orderby = Convert.ToInt16(ISfn.ExecScalar("select isnull(max(orderby),0) + 1 as maxOrderby from Accounting.tbl_t_DocEvents_process_design where eventid=" + eventId + ""));
            }

            if (t_DocEvents_process_design != null && ModelState.IsValid)
            {
                using (fmisEntities context = new fmisEntities())
                {
                    if (t_DocEvents_process_design.processD_Id == 0)
                    {
                        tbl_t_DocEvents_process_design _Savet_DocEvents_process_design = new tbl_t_DocEvents_process_design();

                        _Savet_DocEvents_process_design.EventId = eventId;
                        _Savet_DocEvents_process_design.DTE = t_DocEvents_process_design.DTE;
                        _Savet_DocEvents_process_design.LearningObjectives = t_DocEvents_process_design.LearningObjectives;
                        _Savet_DocEvents_process_design.TopicsHighLights = t_DocEvents_process_design.TopicsHighLights;
                        _Savet_DocEvents_process_design.Activity = t_DocEvents_process_design.Activity;
                        _Savet_DocEvents_process_design.ExpectedOutput = t_DocEvents_process_design.ExpectedOutput;
                        _Savet_DocEvents_process_design.LearningMethodology = t_DocEvents_process_design.LearningMethodology;
                        _Savet_DocEvents_process_design.ResourcesNeeded = t_DocEvents_process_design.ResourcesNeeded;
                        _Savet_DocEvents_process_design.AssignPerson = t_DocEvents_process_design.AssignPerson;
                        _Savet_DocEvents_process_design.orderby = t_DocEvents_process_design.orderby;

                        context.tbl_t_DocEvents_process_design.Add(_Savet_DocEvents_process_design);
                        context.SaveChanges();
                        int savedprocessD_Id = _Savet_DocEvents_process_design.processD_Id;
                        t_DocEvents_process_design.processD_Id = savedprocessD_Id;
                    }
                    else
                    {
                        ISfn.ExcecuteNoneQuery("update Accounting.tbl_t_DocEvents_process_design set orderby = orderby+ 1 where eventid=" + eventId + " and orderby >= " + t_DocEvents_process_design.orderby + "");
                        var _Updatet_DocEvents_process_design = context.tbl_t_DocEvents_process_design.Where(m => m.processD_Id == t_DocEvents_process_design.processD_Id).FirstOrDefault();
                        _Updatet_DocEvents_process_design.DTE = t_DocEvents_process_design.DTE;
                        _Updatet_DocEvents_process_design.LearningObjectives = t_DocEvents_process_design.LearningObjectives;
                        _Updatet_DocEvents_process_design.Activity = t_DocEvents_process_design.Activity;
                        _Updatet_DocEvents_process_design.ExpectedOutput = t_DocEvents_process_design.ExpectedOutput;
                        _Updatet_DocEvents_process_design.TopicsHighLights = t_DocEvents_process_design.TopicsHighLights;
                        _Updatet_DocEvents_process_design.LearningMethodology = t_DocEvents_process_design.LearningMethodology;
                        _Updatet_DocEvents_process_design.ResourcesNeeded = t_DocEvents_process_design.ResourcesNeeded;
                        _Updatet_DocEvents_process_design.AssignPerson = t_DocEvents_process_design.AssignPerson;
                        _Updatet_DocEvents_process_design.userid = t_DocEvents_process_design.userid;
                        _Updatet_DocEvents_process_design.orderby = t_DocEvents_process_design.orderby;
                        context.SaveChanges();
                    }

                }
            }
            // reload with latest DB values
            //t_DocEvents_process_design = context.tbl_t_DocEvents_process_design
            //    .FirstOrDefault(x => x.processD_Id == entity.processD_Id);


            return Json(new[] { t_DocEvents_process_design }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult t_DocEvents_process_designEditingInline_Destroy([DataSourceRequest] DataSourceRequest request, tbl_t_DocEvents_process_design t_DocEvents_process_design)
        {
            if (t_DocEvents_process_design != null)
            {
                using (fmisEntities context = new fmisEntities())
                {
                    var t_DocEvents_process_designDelete = context.tbl_t_DocEvents_process_design.Where(m => m.processD_Id == t_DocEvents_process_design.processD_Id).FirstOrDefault();
                    context.tbl_t_DocEvents_process_design.Remove(t_DocEvents_process_designDelete);
                    context.SaveChanges();
                }
            }

            return Json(new[] { t_DocEvents_process_design }.ToDataSourceResult(request, ModelState));
        }

        //[HttpPost]
        public ActionResult DataGridTrackingListReview([DataSourceRequest] DataSourceRequest request, string obrNo)
        {
            //try
            //{
            List<vw_t_Incoming_ObligLiquidation> data = new List<vw_t_Incoming_ObligLiquidation>();

            using (SqlConnection conn = new SqlConnection(fmisConn))
            {
                string query = @"
                    SELECT TOP (1000) [trans_id],
                                      [AlobsNo],
                                      [OBRAmount],
                                      [Liquidation],
                                      [balance],
                                      [Particulars],
                                      [FundType],
                                      [functionID],
                                      [OfficeMedium]
                    FROM [fmis].[Accounting].[vw_t_Incoming_ObligLiquidation]
                    WHERE (@OBRNo IS NULL OR [AlobsNo] LIKE '%' + @OBRNo + '%')";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@OBRNo", string.IsNullOrEmpty(obrNo) ? (object)DBNull.Value : obrNo);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(new vw_t_Incoming_ObligLiquidation
                        {
                            trans_id = reader.GetInt64(reader.GetOrdinal("trans_id")),
                            AlobsNo = reader.GetString(reader.GetOrdinal("AlobsNo")),
                            OBRAmount = reader.GetDecimal(reader.GetOrdinal("OBRAmount")),
                            Liquidation = reader.GetDecimal(reader.GetOrdinal("Liquidation")),
                            balance = reader.GetDecimal(reader.GetOrdinal("balance")),
                            Particulars = reader.GetString(reader.GetOrdinal("Particulars")),
                            FundType = reader.GetString(reader.GetOrdinal("FundType")),
                            functionID = reader.GetInt32(reader.GetOrdinal("functionID")),
                            OfficeMedium = reader.GetString(reader.GetOrdinal("OfficeMedium"))
                        });
                    }
                }
            }

            // Apply Kendo's server-side filtering, sorting, and paging
            var result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            //}
        }

        public ActionResult DataGridTrackingListReview_dv([DataSourceRequest] DataSourceRequest request, string dvno)
        {
            //try
            //{
            List<vw_Incoming_Received> data = new List<vw_Incoming_Received>();

            using (SqlConnection conn = new SqlConnection(fmisConn))
            {
                string query = @"SELECT  [trans_id]
                  ,[DVNo]
                  ,[ObrNo]
                  ,[Name]
                  ,[GAmount]
                  ,[Particular]
                  ,[FundType]
                  ,[functionID]
                  ,[OfficeMedium]
                  ,[FMISOfficeID]
                  ,[OOE]
                  ,[ClaimantCode]
              FROM [fmis].[Accounting].[vw_Incoming_Received]
                    WHERE (@dvno IS NULL OR [dvno] LIKE '%' + @dvno + '%')";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@dvno", string.IsNullOrEmpty(dvno) ? (object)DBNull.Value : dvno);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(new vw_Incoming_Received
                        {
                            trans_id = reader.GetInt64(reader.GetOrdinal("trans_id")),
                            ObrNo = reader.GetString(reader.GetOrdinal("ObrNo")),
                            GAmount = reader.GetDecimal(reader.GetOrdinal("GAmount")),
                            DVNo = reader.GetString(reader.GetOrdinal("DVNo")),
                            Particular = reader.GetString(reader.GetOrdinal("Particular")),
                            FundType = reader.GetString(reader.GetOrdinal("FundType")),
                            functionID = reader.GetInt32(reader.GetOrdinal("functionID")),
                            OfficeMedium = reader.GetString(reader.GetOrdinal("OfficeMedium"))
                        });
                    }
                }
            }

            // Apply Kendo's server-side filtering, sorting, and paging
            var result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            //}
        }
        public ActionResult Receive_OBR(Int64 trans_id)
        {
            ViewBag.trans_id = trans_id;
            string cmdStr = "Select * from accounting.vw_t_Incoming_ObligLiquidation where trans_id = @trans_id";
            SqlConnection connection = new SqlConnection(fmisConn);
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@trans_id", SqlDbType.BigInt).Value = trans_id;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ViewBag.trans_id = reader["trans_id"];
                        ViewBag.alobsno = reader["AlobsNo"];
                        ViewBag.rcenter = reader["FMISOfficeID"];
                        ViewBag.obramount = reader["OBRAmount"];
                        ViewBag.liquidation = reader["Liquidation"];
                        ViewBag.balance = reader["balance"];
                        ViewBag.particulars = reader["Particulars"];
                        ViewBag.fundtype = reader["FundType"];
                        ViewBag.functionid = reader["functionID"];
                        ViewBag.ooe = reader["ModeOfExpenses_Code"];
                        ViewBag.fundid = reader["AlobsNo"] != DBNull.Value
    ? reader["AlobsNo"].ToString().Substring(0, Math.Min(3, reader["AlobsNo"].ToString().Length))
    : string.Empty;

                        ViewBag.isEdit = 1;
                    }
                }
                else
                {
                    ViewBag.trans_id = 0;
                    ViewBag.alobsno = "";
                    ViewBag.obramount = "";
                    ViewBag.liquidation = "";
                    ViewBag.rcenter = 0;
                    ViewBag.balance = "";
                    ViewBag.particulars = "";
                    ViewBag.fundtype = "";
                    ViewBag.functionid = "";
                    ViewBag.isEdit = 0;
                }

            }
            connection.Close();
            return PartialView("IncomingTransaction_DV", null);
        }
        public ActionResult Receive_OBR_dv(Int64 trans_id)
        {
            
            string cmdStr = "Select * FRom Accounting.vw_t_Incoming_Received where trans_id = @trans_id";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@trans_id", trans_id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ViewBag.trans_id = reader["trans_id"];
                        ViewBag.obrno = reader["ObrNo"];
                        ViewBag.name = reader["Name"];
                        ViewBag.gamount = reader["GAmount"];
                        ViewBag.particular = reader["Particular"];
                        ViewBag.fundtype = reader["FundType"];
                        ViewBag.functionid = reader["functionID"];
                        ViewBag.officemedium = reader["OfficeMedium"];
                        ViewBag.fmisofficeid = reader["FMISOfficeID"];
                        ViewBag.ooe = reader["OOE"];
                        ViewBag.claimantcode = reader["ClaimantCode"];
                    }
                }
                else
                {
                    ViewBag.trans_id = "0";
                    ViewBag.obrno = "";
                    ViewBag.name = "";
                    ViewBag.gamount = "";
                    ViewBag.particular = "";
                    ViewBag.fundtype = "";
                    ViewBag.functionid = "";
                    ViewBag.officemedium = "";
                    ViewBag.fmisofficeid = "";
                    ViewBag.ooe = "";
                    ViewBag.claimantcode = "";
                }
            }
            connection.Close();
            
            return PartialView("IncomingTransaction_DV_details", null);
        }
        public ActionResult save_DV(vw_t_Incoming_ObligLiquidation data)
        {
            int userid = Convert.ToInt32(USER.C_swipeID);
            

            data.obrno = data.obrno ?? "";

            if (data.Particulars == null)
            {
                return Json(new { code = 7, statusName = "The Particular is required" });
            }
            if (data.Particulars == null)
            {
                return Json(new { code = 7, statusName = "The Particular is required" });
            }
            if (data.ModeOfExpenses_Code == null)
            {
                return Json(new { code = 7, statusName = "The allotment class is required" });
            }

            if (data.FMISOfficeID < 1)
            {
                return Json(new { code = 7, statusName = "Invalid Responsibility center" });
            }

            if (data.obrno == "")
            {
                return Json(new { code = 7, statusName = "Invalid OBR No." });
            }

            data.ClaimantCode = data.ClaimantCode ?? "";

            if (data.ClaimantCode.ToString().Length < 1)
            {
                return Json(new { code = 7, statusName = "Claimant Required" });
            }

            //data.FundID = data.FundID ?? 0;
            //if (data.FundID < 2)
            //{
            //    return Json(new { code = 7, statusName = "Fundtype Required" });
            //}

            //if (data.balance < 1)
            //{
            //    return Json(new { code = 7, statusName = "Gross Amount Required" });
            //}

            if (data.countperson < 1)
            {
                data.countperson = 1;
            }


            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute [Accounting].[usp_Incoming_Save_Oblig] @trans_id,@alobsno,@obrno, @balance ,@particulars,@FundID,@fmisofficeid,@modeofexpenses_code,@claimantcode,@userid,@dvid OUTPUT,@dvno OUTPUT,@returncode OUTPUT";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@trans_id", data.trans_id);
                command.Parameters.AddWithValue("@alobsno", data.AlobsNo);
                command.Parameters.AddWithValue("@balance", data.balance);
                command.Parameters.AddWithValue("@particulars", data.Particulars);
                command.Parameters.AddWithValue("@FundID", data.FundID);
                command.Parameters.AddWithValue("@fmisofficeid", data.FMISOfficeID);
                command.Parameters.AddWithValue("@modeofexpenses_code", data.ModeOfExpenses_Code);
                command.Parameters.AddWithValue("@claimantcode", data.ClaimantCode);
                command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                command.Parameters.AddWithValue("@obrno", data.obrno);

                SqlParameter dvidParam = new SqlParameter("@dvid", SqlDbType.Int) { Direction = ParameterDirection.Output };
                SqlParameter dvnoParam = new SqlParameter("@dvno", SqlDbType.VarChar,20) { Direction = ParameterDirection.Output };
                SqlParameter returncodeParam = new SqlParameter("@returncode", SqlDbType.Int) { Direction = ParameterDirection.Output };

                command.Parameters.Add(dvidParam);
                command.Parameters.Add(dvnoParam);
                command.Parameters.Add(returncodeParam);

                // Execute command
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                // Retrieve output values
                Int32 dvid = (Int32)dvidParam.Value;
                string dvno = (string)dvnoParam.Value;
                Int32 returncode = (Int32)returncodeParam.Value;

                connection.Close();
                if (returncode == 7)
                {
                    return Json(new { code = 7, statusName = "The obligation amount is smaller than the DV amount.!", dvid = dvid, dvno = dvno, });
                }

                return Json(new { code = 6, statusName = "Successfully Saved..!", dvid = dvid, dvno = dvno, });
            
            //}
        }
        }
    }
}





