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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
namespace WebAOMS.Controllers
{
    
    [Authorize]
    public class ChartOfAccountsController : Controller
    {
        
        fmisEntities fmisdb = new fmisEntities();
        // GET: ChartOfAccounts
        public ActionResult GL_Index()
        {
            ViewBag.menuid = "a5";
            return View();
        }
        public ActionResult SL_Index()
        {
            ViewBag.menuid = "a6";
            return View();
        }
        public ActionResult COA_read([DataSourceRequest] DataSourceRequest request, int? chartAccountID)
        {
            IEnumerable<tbl_l_ChartOfAccountsChild> AccountList;
            if (chartAccountID == 0)
            {
                AccountList = fmisdb.tbl_l_ChartOfAccountsChild.Where(M => M.AccountChildParentID == null);
            }
            else
            {
                AccountList = fmisdb.tbl_l_ChartOfAccountsChild.Where(M => M.AccountChildParentID == chartAccountID);
            }

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(AccountList.ToDataSourceResult(request, o => new { code = o.code, AccountChildName = o.AccountChildName.Replace("'","`"), ChartAccountChildID = o.ChartAccountChildID, ChildCode = o.ChildCode, AccountChildParentID=o.AccountChildParentID }));
            result.ContentType = "application/json";
            return result;
        }

        public ActionResult _partial_view_subaccount(int ChartAccountChildID,int code,string AccountChildName,string ChildCode)
        {
            ViewBag.ChartAccountChildID = ChartAccountChildID;
            ViewBag.code = code;
            ViewBag.AccountChildName = AccountChildName;
            ViewBag.ChildCode = ChildCode;
            return PartialView("_account_sub", null);
        }
        public class imported_ChartOfAccountsChild
        {
            public Int32 code { get; set; }
            public Int32 AccountChildParentID { get; set; }
            public string AccountChildName { get; set; }
            public string ChildCode { get; set; }
            public string ChartAccountChildCode { get; set; }
            public Int16 levelNo { get; set; }
            public string isActive { get; set; }
            public string hasChild { get; set; }
            public DateTime ChangeDate { get; set; }
            public string ModifiedByID { get; set; }
        }
        public class SaveImportedDataModel
        {
            public int ParentId { get; set; }
            public List<imported_ChartOfAccountsChild> ImportedData { get; set; }
        }
        [HttpPost]
        public JsonResult SaveImportedData(SaveImportedDataModel request)
        {
            using (var db = new fmisEntities())
            {
                if (request.ImportedData != null && request.ImportedData.Any())
                {
                    // Get the LevelNo of the selected Parent ID
                    var parentLevel = db.tbl_l_ChartOfAccountsChild
                                        .Where(x => x.ChartAccountChildID == request.ParentId)
                                        .Select(x => x.levelNo)
                                        .FirstOrDefault();

                    int newLevelNo = Convert.ToInt16(parentLevel) + 1;

                    // Add all imported data with incremented LevelNo
                    var newEntries = request.ImportedData.Select(item => new tbl_l_ChartOfAccountsChild
                    {
                        AccountChildParentID = request.ParentId, // Assign the selected parent ID
                        code = item.code,
                        AccountChildName = item.AccountChildName,
                        levelNo = (byte)newLevelNo // Increment level number
                    }).ToList();

                    db.tbl_l_ChartOfAccountsChild.AddRange(newEntries);
                    db.SaveChanges();
                }
            }
            return Json(new { success = true });
        }

        public ActionResult _partial_view_subaccount_edit(Int32 ChartAccountChildID, Int32 AccountChildParentID)
        {
            if (ISfn.ExecScalar("select [Accounting].[fn_check_ifAccountNameMapped](" + AccountChildParentID + ")") == "1")
            {
                ViewBag.DeniedStatus = "Unable to add or edit the subsidiary ledger due to the account being mapped.";
                return PartialView("_AccessDenied", null);
            }

            if (ChartAccountChildID == 0)
            {
                tbl_l_ChartOfAccountsChild rec_charts = fmisdb.tbl_l_ChartOfAccountsChild.Single(M => M.ChartAccountChildID == AccountChildParentID);
                ViewBag.ChartAccountChildID = 0;
                ViewBag.code = "";
                ViewBag.AccountChildParentID = AccountChildParentID;
                ViewBag.AccountChildName = "";
                ViewBag.ChildCode = rec_charts.ChildCode;
               // ViewBag.ChartAccountChildCode = rec_charts.ChartAccountChildID.ToString();
                ViewBag.levelNo = rec_charts.levelNo + 1;
                ViewBag.hasChild = 0;
            
                //ViewBag.ModifiedByID = User.Identity.Name;
                ViewBag.isActive = true;
                ViewBag.isEdit = 0;
                ViewBag.ClaimantTypeID = 0;
                ViewBag.enable = false;
                return PartialView("_account_sub_edit", null);
            }
            else
            {
                tbl_l_ChartOfAccountsChild rec_charts = fmisdb.tbl_l_ChartOfAccountsChild.Single(M => M.ChartAccountChildID == ChartAccountChildID);
                ViewBag.ChartAccountChildID = rec_charts.ChartAccountChildID;
                ViewBag.code = rec_charts.code;
                ViewBag.AccountChildParentID = rec_charts.AccountChildParentID;
                ViewBag.AccountChildName = rec_charts.AccountChildName;
                ViewBag.ChildCode = rec_charts.ChildCode;
                //ViewBag.ChartAccountChildCode = rec_charts.ChartAccountChildCode;
                ViewBag.levelNo = rec_charts.levelNo;
                ViewBag.hasChild = rec_charts.hasChild;
                ViewBag.ChangeDate = rec_charts.ChangeDate;
                ViewBag.ModifiedByID = rec_charts.ModifiedByID;
                ViewBag.isActive = rec_charts.isActive;
                ViewBag.ClaimantTypeID = rec_charts.ClaimantTypeID;
                ViewBag.isEdit = 1;
                return PartialView("_account_sub_edit", null);
            }
        }

        public ActionResult IUChartOfAccountsChild(tbl_l_ChartOfAccountsChild updateCoa)
        {
            string results="";
            var currentUserId = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            string userid = currentUser.UserID.ToString();
            using (fmisEntities context = new fmisEntities())
            {
                if (updateCoa.ChartAccountChildID == 0)
                {
                    tbl_l_ChartOfAccountsChild _SaveCoa = new tbl_l_ChartOfAccountsChild();
                    _SaveCoa.ChildCode = updateCoa.ChildCode + "-" + updateCoa.code;
                    _SaveCoa.ModifiedByID = USER.C_swipeID;
                    _SaveCoa.ChangeDate = DateTime.Now;
                    _SaveCoa.AccountChildName = updateCoa.AccountChildName;
                    _SaveCoa.AccountChildParentID = updateCoa.AccountChildParentID;
                    _SaveCoa.code = updateCoa.code;
                    _SaveCoa.levelNo = updateCoa.levelNo;
                    _SaveCoa.isActive = updateCoa.isActive;
                    _SaveCoa.hasChild = updateCoa.hasChild;
                    _SaveCoa.ClaimantTypeID = updateCoa.ClaimantTypeID;
                    context.tbl_l_ChartOfAccountsChild.Add(_SaveCoa);
                    context.SaveChanges();
                    results = "6";
                }
                else
                {
                    var coa = context.tbl_l_ChartOfAccountsChild.Where(m=> m.ChartAccountChildID == updateCoa.ChartAccountChildID).FirstOrDefault();
                    coa.AccountChildName = updateCoa.AccountChildName;
                    coa.code = updateCoa.code;
                    coa.ChildCode = updateCoa.ChildCode + "-" + updateCoa.code;
                    coa.ChangeDate = DateTime.Now;
                    coa.isActive = updateCoa.isActive;
                    coa.ModifiedByID = USER.C_swipeID;
                    coa.ClaimantTypeID = updateCoa.ClaimantTypeID;
                    context.SaveChanges();
                    results = "6";
                }
                ISfn.ToExecute2P("execute [Accounting].[usp_JEV_Mapped_accountname] @accountname,@claimantTypeID", "@accountname",updateCoa.AccountChildName, "@claimantTypeID",Convert.ToInt16(updateCoa.ClaimantTypeID));
            }


            return Content(results);
        }

        public ActionResult deleteChartOfAccountsChild(Int32 ChartAccountChildID)
        {
            
            string result = "";
            result =ISfn.ExecScalar("execute Accounting.usp_l_ChartOfAccountsChild_delete @ChartAccountChildID = "+ ChartAccountChildID +", @userid = '"+ USER.C_swipeID +"'");
            return Content(result);
        }


        public ActionResult updateChartAccountID(Int32 ChartAccountChildID,Int32 ChangeChartAccountChildID)
        {

            string result = "";
            result = ISfn.ExecScalar("execute [Accounting].[usp_update_AccoutingEtries_newChartAccountid] @ChartAccountChildID = " + ChartAccountChildID + ",@ChangeChartAccountChildID= " + ChangeChartAccountChildID + ", @userid = '" + USER.C_swipeID + "'");
            return Content(result);
        }

        public JsonResult DataSource_Get_AccountToChange(string parentcode)
        {
            IEnumerable<ufn_ComboSource_AccountToChange_Result> fundtype;
            fundtype = fmisdb.ufn_ComboSource_AccountToChange(parentcode).OrderBy(o => o.acountname);

            return Json(fundtype.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_Get_COA()
        {
            IEnumerable<tbl_l_ChartOfAccountsParent> chartOfAccountsparent;
            chartOfAccountsparent = fmisdb.tbl_l_ChartOfAccountsParent.Where(w=>w.LevelNo==5).OrderBy(o => o.AccountName);
            return Json(chartOfAccountsparent.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_Get_ClaimantType(string parentcode)
        {
            IEnumerable<ufn_ComboSource_ClaimantType_Result> fundtype;
            fundtype = fmisdb.ufn_ComboSource_ClaimantType().OrderBy(o => o.ClaimantType);
            return Json(fundtype.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult import_account_excel()
        {
            return View("_import_account_excel");
        }

        public ActionResult SaveData(IEnumerable<tbl_l_ChartOfAccountsChild> data)
        {
            // Check if the data is valid
            if (data != null && ModelState.IsValid)
            {
                try
                {
                    // Convert the GridDataModel to the entity representing the table
                    var entities = data.Select(d => new tbl_l_ChartOfAccountsChild
                    {
                        code = d.code,
                        AccountChildName = d.AccountChildName
                    });

                    // Save the entities to the SQL Server table
                    using (var context = new fmisEntities()) // Replace YourDbContext with your actual DbContext class
                    {
                        context.tbl_l_ChartOfAccountsChild.AddRange(entities);
                        context.SaveChanges();
                    }

                    // Return a success message
                    return Json(new { success = true, message = "Data saved successfully." });
                }
                catch (Exception ex)
                {
                    // Return an error message if there was an exception
                    return Json(new { success = false, message = "An error occurred while saving the data." });
                }
            }
            else
            {
                // Return an error message if the data is invalid
                return Json(new { success = false, message = "Invalid data." });
            }
        }
        #region Cash Flow Classification
        public ActionResult IUCFClass(tbl_l_ChartOfAccounts_cashFlow updateCoa)
        {
            string results = "";
            var currentUserId = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            string userid = currentUser.UserID.ToString();
            using (fmisEntities context = new fmisEntities())
            {
                if (updateCoa.CFAccountID == 0)
                {
                    tbl_l_ChartOfAccounts_cashFlow _SaveCoa = new tbl_l_ChartOfAccounts_cashFlow();
                    _SaveCoa.ModifiedByID =Convert.ToInt16( USER.C_swipeID);
                    _SaveCoa.ChangeDate = DateTime.Now;
                    _SaveCoa.AccountName = updateCoa.AccountName;
                    _SaveCoa.CFAccountParentID = updateCoa.CFAccountParentID;
                    _SaveCoa.Code = updateCoa.Code;
                    _SaveCoa.LevelNo = updateCoa.LevelNo;
                    context.tbl_l_ChartOfAccounts_cashFlow.Add(_SaveCoa);
                    context.SaveChanges();
                    results = "6";
                }
                else
                {
                    var coa = context.tbl_l_ChartOfAccounts_cashFlow.Where(m => m.CFAccountID == updateCoa.CFAccountID).FirstOrDefault();
                    coa.AccountName = updateCoa.AccountName;
                    coa.Code = updateCoa.Code;                    
                    coa.ChangeDate = DateTime.Now;                    
                    coa.ModifiedByID = Convert.ToInt16(USER.C_swipeID);
                    context.SaveChanges();
                    results = "6";
                }
            }


            return Content(results);
        }
        #endregion
    }
}