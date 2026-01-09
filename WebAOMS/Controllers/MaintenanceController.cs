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
using WebAOMS.ws_tracking;
using WebAOMS.epsws;
using Microsoft.AspNet.Identity;
using WebAOMS.Mod;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Kendo.Mvc.Infrastructure;
using System.ServiceModel;
using System.Linq.Expressions;
using System.Reflection;

namespace WebAOMS.Controllers
{

    [Authorize(Roles = "Admin, Accountant")]
    public class MaintenanceController : Controller
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        fmisEntities fmisdb = new fmisEntities();
        TrackingSoapClient ws = new TrackingSoapClient();

        serviceSoapClient eps = new serviceSoapClient();
        string dbcon_fmis = ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString();
        // GET: Maintenance
        [Authorize(Roles = "Admin, Accountant")]
        public ActionResult UserManagement_Index()
        {
            return View();
        }
        public ActionResult _process_mapping()
        {
            return PartialView("_Claimant_browse_mapping");
        }
        public ActionResult Eproc_Item_Mapping()
        {
            return PartialView("ProItemMappingToCOA");
        }
        public ActionResult ClaimantMapping()
        {
            return View();
        }
        public ActionResult ClaimantMappingToEproc()
        {
            return View();
        }


        public ActionResult _Save_claimant_mapp(string claimantStr,string claimantcodePrimary)
        {
            try
            {
                Int32 userid = Convert.ToInt32(USER.C_swipeID);
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute [Accounting].[usp_claimant_save_mapping] @claimantStr, @claimantcodePrimary,@updateBy";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@claimantcodePrimary", SqlDbType.VarChar, 20).Value = claimantcodePrimary;
                    command.Parameters.Add("@claimantStr", SqlDbType.VarChar, 500).Value = claimantStr;
                    command.Parameters.Add("@updateBy", SqlDbType.VarChar, 20).Value = USER.C_swipeID;

                    connection.Open();
                    int rows = command.ExecuteNonQuery();
                    connection.Close();
                    return Json(new { code = 6, statusName = "Successfully Updated..!" });
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

        public ActionResult _Save_claimant_mapp_eproc(string supplierid_array, string claimantcode)
        {
            try
            {
                List<string> supplierid_list = supplierid_array.Split(',').ToList();
                Int32 claimantID = getClaimantID_bycode(claimantcode);
                Int32 userid = Convert.ToInt32(USER.C_swipeID);
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute [Accounting].[usp_claimant_save_mapping_eproc] @supplierid_array, @claimantcode,@updateBy";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@claimantcode", SqlDbType.VarChar, 20).Value = claimantcode;
                    command.Parameters.Add("@supplierid_array", SqlDbType.VarChar, 500).Value = supplierid_array;
                    command.Parameters.Add("@updateBy", SqlDbType.VarChar, 20).Value = USER.C_swipeID;

                    connection.Open();
                    int rows = command.ExecuteNonQuery();
                    connection.Close();

                    for (var i = 0; i < supplierid_list.Count; i++)
                    {
                        eps.SetSupplierID(Convert.ToInt32(supplierid_list[i]), claimantID);
                    }
                    return Json(new { code = 6, statusName = "Successfully Updated..!" });
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
        private static Int32 getClaimantID_bycode(string claimantcode) {
            Int32 claimantid;
            claimantid = Convert.ToInt32(ISfn.ExecScalar("select trnno from tblCMS_CDClaimantDetails where ClaimantCode = '" + claimantcode + "'"));

            return claimantid;
        }
        public ActionResult grid_claimant_byClaimantType_server_search([DataSourceRequest] DataSourceRequest request, int claimantCode, string name)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(dbcon_fmis, System.Data.CommandType.Text,
            "execute Accounting.usp_claimant_grid_byClaimantType_server_search_formapping '" + claimantCode + "','" + name.AntiInject() + "'").Tables[0];
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public ActionResult grid_proc_item_server_search([DataSourceRequest] DataSourceRequest request)
        {
            DataTable recItemCat;
           // DataTable recCOA;

            recItemCat = eps.ItemCategory();
            //recCOA = ToDataTable(fmisdb.tbl_l_ChartOfAccountsParent.Where(w => w.LevelNo == 5).OrderBy(o => o.AccountName).ToList());

            //var joinResult = from t1 in recItemCat.AsEnumerable()
            //                 join t2 in recCOA.AsEnumerable() on t1.Field<Int32>("accountcode") equals t2.Field<Int32>("accountcode")
            //                 select new
            //                 {
            //                     ID = t1.Field<int>("itemgroupid"),
            //                     Column1 = t1.Field<string>("itemcategory"),
            //                     Column2 = t2.Field<string>("accountcode")
            //                 };

            //DataTable result = joinResult.CopyToDataTable();
            return Json(recItemCat.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
        }

        public ActionResult updateAccountCodeInCategory(Int32 itemgroupid,Int32 accountcode)
        {
            var r = "";
            var code = 6;
            
            try
            {
                eps.updateCategoryAccountCode(itemgroupid, accountcode.ToString());
                r = "Successfully updated..!";
            }
            catch (Exception ex)
            {
                r = ex.Message;
                code = 5;
            }

            return Json(new { code = code, statusName = r });
        }
        public ActionResult grid_claimant_byClaimantType_server_search_eproc([DataSourceRequest] DataSourceRequest request, int claimantCode, string name)
        {
            //DataTable rec;
            //rec = eps.GetSuppliers();


            //return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(dbcon_fmis, System.Data.CommandType.Text,
            "execute Accounting.usp_claimant_grid_byClaimantType_server_search_formapping_eproc '" + claimantCode + "','" + name.AntiInject() + "'").Tables[0];
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult insert_eproc_supplier([DataSourceRequest] DataSourceRequest request)
        {
            DataTable rec;
            rec = eps.GetSuppliers();

            foreach (DataRow dataRow in rec.Rows)
            {
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute [Accounting].[usp_Insert_Supprlier_from_Eproc]  @supplierid,@supplier,@Address";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@supplier", SqlDbType.VarChar, 200).Value = dataRow["supplier"].ToString();
                    command.Parameters.Add("@Address", SqlDbType.VarChar, 500).Value = dataRow["Address"].ToString();
                    command.Parameters.Add("@supplierid", SqlDbType.Int).Value = dataRow["supplierid"].ToString(); ;

                    connection.Open();
                    int rows = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return Json(new { code = 6, statusName = "Successfully Inserted..!" });
        }

        public ActionResult insert_eproc_transaction([DataSourceRequest] DataSourceRequest request)
        {
            DataTable rec;
            rec = eps.SupplierPO();

            foreach (DataRow dataRow in rec.Rows)
            {
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute [Accounting].[usp_Eproc_Insert_Transaction]  @refno,@prno,@pono,@supplierid,@supplier,@pramount,@poamount";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@refno", SqlDbType.VarChar, 25).Value = dataRow["refno"].ToString();
                    command.Parameters.Add("@prno", SqlDbType.VarChar, 25).Value = dataRow["prno"].ToString();
                    command.Parameters.Add("@pono", SqlDbType.VarChar, 25).Value = dataRow["pono"].ToString();
                    command.Parameters.Add("@supplier", SqlDbType.VarChar, 25).Value = dataRow["supplier"].ToString();
                    command.Parameters.Add("@pramount", SqlDbType.Money).Value = dataRow["pramount"].ToString();
                    command.Parameters.Add("@poamount", SqlDbType.Money).Value = dataRow["poamount"].ToString();
                    command.Parameters.Add("@supplierid", SqlDbType.Int).Value = dataRow["supplierid"].ToString(); ;

                    connection.Open();
                    int rows = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return Json(new { code = 6, statusName = "Successfully Inserted..!" });
        }
        #region transactiontype
        public ActionResult index_transaction_type()
        {
            ViewBag.Title = "Transaction type Buildup";
            ViewBag.Title_mini = "Transaction Buildup";
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a58";
            return View();
        }
        public JsonResult DataSource_COA()
        {
            IEnumerable<tbl_l_ChartOfAccountsParent> COA;
            COA = fmisdb.tbl_l_ChartOfAccountsParent.OrderBy(o => o.AccountName);
            return Json(COA.Select(tx => new { AccountName = tx.AccountName, ChartAccountID = tx.ChartAccountID }).ToList(), JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult _Add_Transaction_type(Int64 id)
        {
            ViewBag.status_code = id;
            string cmdStr = "Select * FRom [Tracking].[tbl_l_StatusDescription] where Transtype_id = @Transtype_id";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@Transtype_id", id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {

                        ViewBag.Transtype_id = reader["Transtype_id"];
                        ViewBag.TransactionName = reader["TransactionName"];
                        ViewBag.isDebit = reader["isDebit"];
                        ViewBag.ChartAccountID = reader["ChartAccountID"];
                    }
                }
                else
                {
                    ViewBag.Transtype_id = 0;
                    ViewBag.TransactionName = "";
                    ViewBag.isDebit = 0;
                    ViewBag.ChartAccountID = 0;
                }
            }
            connection.Close();
            return PartialView("_Add", null);
        }

        public ActionResult grid_transaction_type([DataSourceRequest] DataSourceRequest request)
        {
            DataTable crec;
            crec = ISfn.ToDatatable("select * from Accounting.ufn_grid_transaction_type()");
            return Json(crec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        #endregion transaction type

        public JsonResult get_menu(int? id = 0)
        {
            var dataContext = new fmisEntities();
            var employees = from e in dataContext.w_UserMenu
                            where (id.HasValue ? e.parent_id == id : e.parent_id == 0)
                            select new
                            {
                                id = e.menu_id,
                                Name = e.MenuName,
                                hasChildren = false
                            };
            
            return Json(employees, JsonRequestBehavior.AllowGet);
        }

        public ActionResult add_usermenu(string menuStr, string Id)
        {

            try
            {
                
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute [Accounting].[usp_Save_Menu_user] @menuStr, @Id";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@menuStr", SqlDbType.VarChar, 300).Value = menuStr;
                    command.Parameters.Add("@Id", SqlDbType.NVarChar,128).Value = Id;
                    connection.Open();
                    int rows = command.ExecuteNonQuery();
                    connection.Close();

                }
                return Json(new { code = 6, statusName = "Successfully Deleted..!" });
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
        public ActionResult add_user_office(string officeStr, string Id)
        {

            try
            {

                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute [Accounting].[usp_Save_user_office] @officeStr, @Id";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@officeStr", SqlDbType.VarChar, 300).Value = officeStr;
                    command.Parameters.Add("@Id", SqlDbType.NVarChar, 128).Value = Id;
                    connection.Open();
                    int rows = command.ExecuteNonQuery();
                    connection.Close();

                }
                return Json(new { code = 6, statusName = "Added Successfully..!" });
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

        public ActionResult remove_user_office(string officeStr, string Id)
        {

            try
            {
                Int32 userid = Convert.ToInt32(USER.C_swipeID);
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute [Accounting].[usp_delete_office_user] @officeStr, @Id";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@officeStr", SqlDbType.VarChar, 300).Value = officeStr;
                    command.Parameters.Add("@Id", SqlDbType.NVarChar, 128).Value = Id;
                    connection.Open();
                    int rows = command.ExecuteNonQuery();
                    connection.Close();

                }
                return Json(new { code = 6, statusName = "Successfully Deleted..!" });
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
        public ActionResult remove_usermenu(string menuStr, string Id)
        {

            try
            {
                Int32 userid = Convert.ToInt32(USER.C_swipeID);
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute [Accounting].[usp_delete_Menu_user] @menuStr, @Id";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.Add("@menuStr", SqlDbType.VarChar, 2000).Value = menuStr;
                    command.Parameters.Add("@Id", SqlDbType.NVarChar,128).Value = Id;
                    connection.Open();
                    int rows = command.ExecuteNonQuery();
                    connection.Close();

                }
                return Json(new { code = 6, statusName = "Successfully Deleted..!" });
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

        //[HttpPost]
        //public ActionResult Upload(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        // Read the file into a byte array
        //        var fileBytes = new byte[file.ContentLength];
        //        file.InputStream.Read(fileBytes, 0, file.ContentLength);

        //        // Save the file to the database
        //        var fileModel = new FilesUpload
        //        {
        //            Name = file.FileName,
        //            Data = fileBytes
        //        };
        //        using (var db = new ApplicationDbContext())
        //        {
        //            db.Files.Add(fileModel);
        //            db.SaveChanges();
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}

    }
}

//pol : SupplierPO
//returns datatable(refno, prno, pono, supplierid, supplier, pramount, poamount)