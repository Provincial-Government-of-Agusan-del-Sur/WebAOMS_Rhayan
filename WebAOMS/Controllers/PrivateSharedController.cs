using System;
using System.Text;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using WebAOMS.Models;
using System.Web.Services;
using System.Web.Services.Protocols;
using Kendo.Mvc.UI;
using System.Web.Script.Serialization;
using System.Web.Security;
using WebAOMS.Base;

namespace WebAOMS.Controllers
{
    public class PrivateSharedController : Controller
    {
        //
        // GET: /PrivateShared/

        public ActionResult SearchEmployeeGrid(int id=0)
        {
            return View();
        }
        public ActionResult SearchEmployeeGrid_ClickName()
        {
            return View();
        }
      

        public ActionResult SearchEmployeeGrid_PayrollGeneration()
        {
            return View();
        }

        public ActionResult gridActiveEmployeeList([DataSourceRequest] DataSourceRequest request,int employmentstatusid = 0, int office = 0)
        {
            DataTable rec;

            if (employmentstatusid > 0)
            {
                rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text,
               "SELECT [eid],[EmpName] FROM [pmis].[dbo].[vwMergeAllEmployee] where employmentstatus_id = " + employmentstatusid + " order by empname").Tables[0];
            }
            else
            {
               rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text,
              "SELECT [eid],[EmpName] FROM [pmis].[dbo].[vwMergeAllEmployee] order by empname").Tables[0];
            }
            return Json(rec.ToDataSourceResult(request));
        }

        public JsonResult LoadComboDatasource(int datasourceid = 0, int para1 = 0,int para2 = 0)
        {
            para1 = USER.C_OfficeID;
            //DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, "execute [dbo].[sp_comboDatasource] " + datasourceid + "," + para1 + "," + userType).Tables[0];
            DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, "execute ifmis.[dbo].[sp_comboDatasource] @datasourceid =" + datasourceid + ",@para1=" + para1 + ",@userID = " + USER.C_eID + ",@para2 = " + para2).Tables[0];
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ComboPostedBatch(int employmentstatus_id = 0, int year = 0, int month = 0)
        {

            DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, "select * from  ifmis.[dbo].[fn_combo_PostedBatch] (" + year + "," + month + "," + employmentstatus_id + ")").Tables[0];
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadComboDatasource_public(int datasourceid = 0, int para1 = 0)
        {
            para1 = 0;
            //DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, "execute [dbo].[sp_comboDatasource] " + datasourceid + "," + para1 + "," + userType).Tables[0];
            DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, "execute ifmis.[dbo].[sp_comboDatasource] @datasourceid =" + datasourceid + ",@para1=" + para1 + ",@userID = 0").Tables[0];
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadGridDatasource([DataSourceRequest] DataSourceRequest request, int GridSourceID = 0, int office = 0, string para1 = "", string para2 = "", string para3 = "", string para4 = "", string para5 = "",string para6 = "")
        {
            DataTable rec;
                rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text,
               "EXECUTE ifmis.[dbo].[sp_GridDatasource] @GridDatasourceID = " + GridSourceID + " ,@para1 = '" + para1 + "',@para2 = '" + para2 + "',@para3 = '" + para3 + "',@para4 = '" + para4 + "',@para5 = '" + para5 + "',@para6 = '" + para6 + "'").Tables[0];
            return Json(rec.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadGridDeduction([DataSourceRequest] DataSourceRequest request, int eid = 0,int year = 0 , int month = 0)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text,
               "SELECT [DeductionCode],[Description],[Amount] FROM ifmis.[dbo].[vw_payroll_Deduction] where eid_new = " + eid + " and year_ = " + year + " and month_ = " + month + "").Tables[0];
            return Json(rec.ToDataSourceResult(request));
        }

        public ActionResult LoadGridPayroll([DataSourceRequest] DataSourceRequest request, int year = 0 , int month = 0,int officeID = 0)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text,
               "execute pmis.dbo.MPproc_ViewPayrollAndDeductions "+ month+ ","+ year +","+ officeID +",7,0").Tables[0];
            return Json(rec.ToDataSourceResult(request));
        }

        public ActionResult EmployeeListForAdd( int officeid)
        {
            ViewBag.officeid = officeid;
            return PartialView("_Payroll_Emplist");
        }

        public ActionResult EmployeeListForAdd_bonus(int officeid)
        {
            ViewBag.officeid = officeid;
            return PartialView("_Payroll_Emplist_bonus");
        }
        public ActionResult LoadGridFromWebservice([DataSourceRequest] DataSourceRequest request, int itemid = 2)
        {
            DataTable rec;
            
            epsws.serviceSoapClient ws = new epsws.serviceSoapClient();
           
            rec =  ws.POItems(itemid);
            
            return Json(rec.ToDataSourceResult(request));
        }
        public JsonResult Deduction_ActiveEmployee(int empStatusID = 0, int payrollofficeID = 0)
        {
            //DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, "execute [dbo].[sp_comboDatasource] " + datasourceid + "," + para1 + "," + userType).Tables[0];
            DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, "execute ifmis.dbo.usp_cmb_ActiveEmployee @empStatusID =" + empStatusID + ",@payrollofficeID=" + payrollofficeID).Tables[0];
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult cmb_ActiveEmployee(int empStatusID = 0, int payrollofficeID = 0)
        {
            //DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, "execute [dbo].[sp_comboDatasource] " + datasourceid + "," + para1 + "," + userType).Tables[0];
            DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, "execute ifmis.dbo.usp_cmb_ActiveEmployee @empStatusID =" + empStatusID + ",@payrollofficeID=" + payrollofficeID).Tables[0];
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult cmb_AllEmployee(int payrollofficeID = 0)
        {
            //DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, "execute [dbo].[sp_comboDatasource] " + datasourceid + "," + para1 + "," + userType).Tables[0];
            DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, "execute ifmis.dbo.[usp_cmb_AllEmployee] @payrollofficeID=" + payrollofficeID).Tables[0];
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Grid_deduction_smmary([DataSourceRequest] DataSourceRequest request, int eid = 0,int loanstatusoptionID = 0)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text,
               "execute ifmis.dbo.usp_Payroll_Deduction_summary " + eid + "," + loanstatusoptionID + "").Tables[0];
            return Json(rec.ToDataSourceResult(request));
        }
        public JsonResult LoadComboSourceOfFund(int fundcode = 0)
        {
            DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, "SELECT [fundID] as val,[SourceName] as txt FROM [IFMIS].[dbo].[tbl_reff_SourceOfFund] where fundcode = " + fundcode + "").Tables[0];
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }

    }
}
