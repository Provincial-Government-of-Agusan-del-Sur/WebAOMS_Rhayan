using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Reporting;
using WebAOMS.Base;
using WebAOMS.Report.Design;
using WebAOMS.Report.Accounting;
using WebAOMS.Report.Design.Document;
namespace WebAOMS.Report
{
    public partial class rpt_Viewer : System.Web.UI.Page
    {

        InstanceReportSource rs = new InstanceReportSource();

        protected void Page_Load(object sender, EventArgs e)
        {

            int rptid = Convert.ToInt32(Request["rptid"]);
            int IsDetailed;
            Int16 fundid;
            DateTime to;
            DateTime from;
            string refno;
            int obligid;
            Int32 dvid;
            int hideAccountname; int hideOOEname;
            switch (rptid)
            {

                case 1:
                    Int64 jevid = Convert.ToInt64(Request["jevid"]);
                    rs.ReportDocument = new rpt_JEV(jevid);
                    break;
                case 2:
                    from = Convert.ToDateTime(Request["from"]);
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    string childcode = Request["childcode"].ToString();
                    rs.ReportDocument = new rpt_Ledger_subsidiary(fundid, from, to, childcode);
                    break;
                case 3:
                    int IsPreClosing = Convert.ToInt16(Request["IsPreClosing"]);
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    if (fundid == 0)
                    {
                        rs.ReportDocument = new Trial_Balance_Consolidated(fundid, IsPreClosing, to);
                    }
                    else
                    {
                        rs.ReportDocument = new Trial_Balance(fundid, IsPreClosing, to);
                    }
                    break;
                case 4:
                    IsDetailed = Convert.ToInt16(Request["IsDetailed"]);
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    rs.ReportDocument = new Financial_Position(fundid, IsDetailed, to);
                    break;
                case 5:
                    IsDetailed = Convert.ToInt16(Request["IsDetailed"]);
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    rs.ReportDocument = new Financial_Performance(fundid, IsDetailed, to);
                    break;
                case 6:
                    refno = Request["refno"].ToString();
                    rs.ReportDocument = new rpt_tracking_form(refno);
                    break;
                case 7:
                    rs.ReportDocument = new rpt_doc();
                    break;
                case 47:
                case 46:
                    long doc_details_id = Convert.ToInt64(Request["doc_details_id"]);
                    rs.ReportDocument = new rpt_document_training_design(rptid,doc_details_id);
                    break;
                case 48:
                    long eventid = Convert.ToInt64(Request["doc_details_id"]);
                    rs.ReportDocument = new rpt_document_activityDesign_noneproc(rptid, eventid);
                    break;
                case 49:
                    long eventid1 = Convert.ToInt64(Request["doc_details_id"]);
                    rs.ReportDocument = new rpt_document_activityDesign_noneproc(rptid, eventid1);
                    break;
                case 50:
                    long eventid2 = Convert.ToInt64(Request["doc_details_id"]);
                    rs.ReportDocument = new rpt_document_process_design(rptid, eventid2);
                    break;
                case 53:
                    long eventid3 = Convert.ToInt64(Request["doc_details_id"]);
                    rs.ReportDocument = new rpt_document_activityDesign_noneproc(rptid, eventid3);
                    break;
                case 54:
                    long eventid4 = Convert.ToInt64(Request["doc_details_id"]);
                    rs.ReportDocument = new rpt_document_activityDesign_noneproc(rptid, eventid4);
                    break;
                case 57:
                    rs.ReportDocument = new rpt_document_activityDesign_noneproc2025(rptid, Convert.ToInt64(Request["doc_details_id"]));
                    break;
                case 58:
                    rs.ReportDocument = new rpt_document_trainingDesign2025(rptid, Convert.ToInt64(Request["doc_details_id"]));
                    break;
                case 55:
                    long eventid5 = Convert.ToInt64(Request["doc_details_id"]);
                    rs.ReportDocument = new rpt_document_process_design(rptid, eventid5);
                    break;
                case 56:
                    long eventid6 = Convert.ToInt64(Request["doc_details_id"]);
                    rs.ReportDocument = new rpt_document_acivityDesign_revision(rptid, eventid6);
                    break;
                case 12:
                    dvid = Convert.ToInt32(Request["dvid"]);
                    int fundcode = Convert.ToInt32(ISfn.ExecScalar("SELECT fundID FROM [fmis].[Accounting].[tbl_t_DV_Details] where DVid = "+dvid+""));
                    
                    refno = Request["refno"].ToString();
                    if (fundcode == 301)
                    {

                        rs.ReportDocument = new rpt_FURS(dvid, refno);
                    }
                    else
                    {
                        rs.ReportDocument = new rpt_cafoa(dvid, refno);
                    }
                    
                    break;
                case 11:
                    dvid = Convert.ToInt32(Request["dvid"]);
                    refno = Request["refno"].ToString();
                    rs.ReportDocument = new rpt_DV_2021(dvid, refno);
                    break;
                case 13:
                    dvid = Convert.ToInt32(Request["dvid"]);
                    refno = Request["refno"].ToString();
                    rs.ReportDocument = new rpt_DV_2021(dvid, refno);
                    break;
                case 20:
                    
                    int batchno = Convert.ToInt32(Request["batchno"].ToString());
                    int bankID = Convert.ToInt32(Request["bankID"].ToString());
                    int OfficeID = Convert.ToInt32(Request["OfficeID"].ToString());
                    rs.ReportDocument = new Design.Payroll.rpt_Special_Payroll_dynamic(batchno, bankID, OfficeID);
                    rp.ReportSource = rs;
                    break;
                case 21:
                    from = Convert.ToDateTime(Request["from"]);
                    to = Convert.ToDateTime(Request["to"]);
                    int status_id = Convert.ToInt16(Request["status_id"]);
                    rs.ReportDocument = new rpt_log_printed(from,to, status_id);
                    break;
                case 23:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    rs.ReportDocument = new Status_AAOU(fundid, to);
                    break;
                case 24:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    rs.ReportDocument = new rpt_Journal_CashReceips(fundid, to);
                    break;
                case 26:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    rs.ReportDocument = new rpt_Journal_CheckD(fundid, to);
                    break;
                case 28:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    int transtype = Convert.ToInt16(Request["transtype"]);
                    int isConso = Convert.ToInt16(Request["isConso"]);
                    rs.ReportDocument = new rpt_Journal_Recap(fundid, to,transtype,isConso);
                    break;
                case 25:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    rs.ReportDocument = new rpt_Journal_CashD(fundid, to);
                    break;
                case 27:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    rs.ReportDocument = new rpt_Journal_GeneralJ(fundid, to);
                    break;
                case 29:
                    obligid = Convert.ToInt32(Request["obligid"]);
                    rs.ReportDocument = new rpt_oblig_cafoa(obligid);
                    break;
                case 30:
                    obligid = Convert.ToInt32(Request["obligid"]);
                    rs.ReportDocument = new rpt_Oblig_SAA(obligid);
                    break;
                case 31:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    rs.ReportDocument = new rpt_Journal_ADA(fundid, to);
                    break;
                case 32:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    rs.ReportDocument = new rpt_query_SCBAA(fundid, to);
                    break;
                case 33:
                    
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    hideAccountname = Convert.ToInt16(Request["hideAccountname"]);
                    hideOOEname = Convert.ToInt16(Request["hideOOEname"]);
                    rs.ReportDocument = new rpt_Query_saaocc(fundid, to, hideAccountname, hideOOEname,rptid);
                    break;
                case 36:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    hideAccountname = Convert.ToInt16(Request["hideAccountname"]);
                    hideOOEname = Convert.ToInt16(Request["hideOOEname"]);
                    rs.ReportDocument = new rpt_query_saaou(fundid, to, hideAccountname, hideOOEname,rptid); 
                    break;
                case 39:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    hideAccountname = Convert.ToInt16(Request["hideAccountname"]);
                    hideOOEname = Convert.ToInt16(Request["hideOOEname"]);
                    rs.ReportDocument = new rpt_Query_saaocc(fundid, to, hideAccountname, hideOOEname, rptid);
                    break;
                case 45:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    hideAccountname = Convert.ToInt16(Request["hideAccountname"]);
                    hideOOEname = Convert.ToInt16(Request["hideOOEname"]);
                    rs.ReportDocument = new rpt_Query_saaocc(fundid, to, hideAccountname, hideOOEname, rptid);
                    break;
                case 42:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    hideAccountname = Convert.ToInt16(Request["hideAccountname"]);
                    hideOOEname = Convert.ToInt16(Request["hideOOEname"]);
                    rs.ReportDocument = new rpt_query_saaou(fundid, to, hideAccountname, hideOOEname, rptid);
                    break;
                case 51:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    rs.ReportDocument = new rpt_Financial_schedules(fundid, to);
                    break;
                default:
                    to = Convert.ToDateTime(Request["to"]);
                    fundid = Convert.ToInt16(Request["fundid"]);
                    hideAccountname = Convert.ToInt16(Request["hideAccountname"]);
                    hideOOEname = Convert.ToInt16(Request["hideOOEname"]);
                    rs.ReportDocument = new rpt_query_saaou(fundid, to, hideAccountname, hideOOEname, rptid);
                    break;
            }
            rp.ReportSource = rs;
        }
    }
}