using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using WebAOMS.Models; // Use the correct namespace for your models

namespace WebAOMS.Controllers
{
    public class TransactionTypesController : Controller
    {
        private fmisEntities db = new fmisEntities(); // Your DbContext

        // GET: TransactionTypes
        public ActionResult Index()
        {

            ViewData["AccountChildList"] = db.tbl_l_ChartOfAccountsParent
                             .Where(a => a.LevelNo == 5)
                             .Select(a => new
                             {
                                 a.ChartAccountID,
                                 AccountName = a.AccountName,
                                 AccountCode = a.AccountCode
                             }).ToList();
            return View();
        }

        // Read action for Kendo Grid
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var transactionTypes = db.tbl_l_Transaction_type.ToList();
            return Json(transactionTypes.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
        }

        // Create action
        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, tbl_l_Transaction_type transactionType)
        {
            if (ModelState.IsValid)
            {
                // Check if the TransactionName already exists
                bool exists = db.tbl_l_Transaction_type.Any(t => t.TransactionName == transactionType.TransactionName);
                if (string.IsNullOrWhiteSpace(transactionType.TransactionName))
                {
                    ModelState.AddModelError("TransactionName", "Transaction Name cannot be empty.");
                }
                else if (exists)
                {
                    ModelState.AddModelError("TransactionName", "Transaction Name already exists. Entry denied.");
                }
                else
                {
                    db.tbl_l_Transaction_type.Add(transactionType);
                    db.SaveChanges();
                }
            }
            return Json(new[] { transactionType }.ToDataSourceResult(request, ModelState));
        }

        // Update action
        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, tbl_l_Transaction_type transactionType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactionType).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new[] { transactionType }.ToDataSourceResult(request, ModelState));
        }

        // Delete action
        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, tbl_l_Transaction_type transactionType)
        {
            if (transactionType != null)
            {
                var entity = db.tbl_l_Transaction_type.Find(transactionType.Transtype_id);
                if (entity != null)
                {
                    db.tbl_l_Transaction_type.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(new[] { transactionType }.ToDataSourceResult(request, ModelState));
        }

        // Read action for Kendo Grid of CheckList
        public ActionResult ReadCheckList([DataSourceRequest] DataSourceRequest request, int transtypeId)
        {
            var checkLists = db.tbl_l_Transaction_CheckList
                               .Where(c => c.Transtype_id == transtypeId)
                               .ToList();
            return Json(checkLists.ToDataSourceResult(request));
        }

        // Create action for CheckList
        [HttpPost]
        public ActionResult CreateCheckList([DataSourceRequest] DataSourceRequest request, tbl_l_Transaction_CheckList checkList, int transtypeId)
        {
            if (ModelState.IsValid)
            {
                checkList.Transtype_id = transtypeId; // Set the Transtype_id from the selected row
                db.tbl_l_Transaction_CheckList.Add(checkList);
                db.SaveChanges();
            }
            return Json(new[] { checkList }.ToDataSourceResult(request, ModelState));
        }

        // Update action for CheckList
        [HttpPost]
        public ActionResult UpdateCheckList([DataSourceRequest] DataSourceRequest request, tbl_l_Transaction_CheckList checkList, int transtypeId)
        {
            if (ModelState.IsValid)
            {
                checkList.Transtype_id = transtypeId; // Ensure Transtype_id remains correct
                db.Entry(checkList).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new[] { checkList }.ToDataSourceResult(request, ModelState));
        }

        // Delete action for CheckList
        [HttpPost]
        public ActionResult DeleteCheckList([DataSourceRequest] DataSourceRequest request, tbl_l_Transaction_CheckList checkList)
        {
            if (checkList != null)
            {
                var entity = db.tbl_l_Transaction_CheckList.Find(checkList.checklist_id);
                if (entity != null)
                {
                    db.tbl_l_Transaction_CheckList.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(new[] { checkList }.ToDataSourceResult(request, ModelState));
        }

        // Read action for ExpenseDetails Grid
        public ActionResult ReadExpenseDetails([DataSourceRequest] DataSourceRequest request, int transtypeId)
        {
       

            var expenseDetails = db.tbl_l_Transaction_ExpenseDetails
           .Where(e => e.Transtype_id == transtypeId)
           .Join(db.tbl_l_ChartOfAccountsParent,
               expenseDetail => expenseDetail.ChartAccountID,
               chartAccount => chartAccount.ChartAccountID,
               (expenseDetail, chartAccount) => new
               {
                   expenseDetail.expenseDetails_id,
                   chartAccount.AccountName,
                   chartAccount.AccountCode,
                   expenseDetail.ChartAccountID,
                   expenseDetail.isActive,
                   expenseDetail.Transtype_id
               })
           .ToList();

            return Json(expenseDetails.ToDataSourceResult(request));
        }

        // Create action for ExpenseDetails
        [HttpPost]
        public ActionResult CreateExpenseDetails([DataSourceRequest] DataSourceRequest request, tbl_l_Transaction_ExpenseDetails expenseDetail, int transtypeId)
        {
            if (ModelState.IsValid)
            {
                expenseDetail.Transtype_id = transtypeId; // Set the Transtype_id from the selected row
                db.tbl_l_Transaction_ExpenseDetails.Add(expenseDetail);
                db.SaveChanges();
            }
            return Json(new[] { expenseDetail }.ToDataSourceResult(request, ModelState));
        }

        // Update action for ExpenseDetails
        [HttpPost]
        public ActionResult UpdateExpenseDetails([DataSourceRequest] DataSourceRequest request, tbl_l_Transaction_ExpenseDetails expenseDetail, int transtypeId)
        {
            if (ModelState.IsValid)
            {
                expenseDetail.Transtype_id = transtypeId; // Ensure Transtype_id remains correct
                db.Entry(expenseDetail).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new[] { expenseDetail }.ToDataSourceResult(request, ModelState));
        }

        // Delete action for ExpenseDetails
        [HttpPost]
        public ActionResult DeleteExpenseDetails([DataSourceRequest] DataSourceRequest request, tbl_l_Transaction_ExpenseDetails expenseDetail)
        {
            if (expenseDetail != null)
            {
                var entity = db.tbl_l_Transaction_ExpenseDetails.Find(expenseDetail.expenseDetails_id);
                if (entity != null)
                {
                    db.tbl_l_Transaction_ExpenseDetails.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(new[] { expenseDetail }.ToDataSourceResult(request, ModelState));
        }

        // Fetch Chart of Accounts for dropdown
        public JsonResult GetChartAccounts()
        {
            var accounts = db.tbl_l_ChartOfAccountsParent
                             .Where(a => a.LevelNo == 5)
                             .Select(a => new
                             {
                                 a.ChartAccountID,
                                 AccountName = a.AccountName,
                                 AccountCode = a.AccountCode
                             }).ToList();

            return Json(accounts, JsonRequestBehavior.AllowGet);
        }
    }
}
