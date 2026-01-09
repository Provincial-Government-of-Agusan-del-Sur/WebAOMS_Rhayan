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
using WebAOMS.wsfmis;

namespace WebAOMS.Controllers
{
    [Authorize(Roles = "Admin, Supervisor")]
    public class UserManageController : Controller
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private fmisEntities db = new fmisEntities();
        public ActionResult user_management(string Id)
        {
            ViewBag.Modal_title = "Edit Client Access";
            ViewBag.Title_mini = "User Management";
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.Id = Id;
           return PartialView();
        }
        public ActionResult user_management_menu(string Id)
        {
            ViewBag.Modal_title = "Edit Client Access menu";
            ViewBag.Title_mini = "User Management";
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.Id = Id;
           return PartialView();
        }
        public ActionResult user_management_office(string Id)
        {
            ViewBag.Modal_title = "Edit Client Access to office";
            ViewBag.Title_mini = "User Management";
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.Id = Id;
            return PartialView();
        }
        public JsonResult get_ref_Employee_byUser()
        {
            var users = db.AspNetUsers
                        .Select(u => new AspNetUserViewModel
                        {
                            Id = u.Id,
                            Email = u.UserName,
                        })
                        .Take(1000)
                        .ToList();
            return Json(users, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_role_byuserid(Int64 userID)
        {
            DataTable rec = ISfn.ExecuteDataset("SELECT [Id] ,[Name] FROM [fmis].[dbo].[AspNetRoles]");
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
 
        
        public JsonResult get_rolearray(string Id)
        {
            DataTable rec;
            string roleArray = "5";
            if (Id.Length > 0)
            {

                rec = ISfn.ToDatatable(
                "execute [Accounting].[usp_get_user_role] @Id", "@Id", Id);
                if (rec.Rows.Count > 0)
                {
                    if (rec.Rows[0]["roleArray"].ToString() == "")
                    {
                        roleArray = "5";
                    }
                    else
                    {
                        roleArray = rec.Rows[0]["roleArray"].ToString();
                    }
                                      
                }
                rec.Dispose();
            }

            var result = new { Result = roleArray.StringToListInt() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult save_user_role(string Id,string roleArray)
        {
            string cmdStr = "";

                cmdStr = "execute Accounting.[usp_Save_user_role] @Id,@roleArray";

            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@roleArray", roleArray);
                
                connection.Open();

                int rows = command.ExecuteNonQuery();
                connection.Close();
                return Json(new { code = 6, statusName = "Successfully Save..!" });
            }
        }
        //Access to office
        //MENU
        public ActionResult grid_user_office([DataSourceRequest] DataSourceRequest request, string Id)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_get_user_Office] (@Id) order by officename";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@Id", Id);
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult grid_available_office([DataSourceRequest] DataSourceRequest request, string Id)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_get_Office] (@Id) order by officename";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@Id", Id);
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        //End user access to office
        //MENU
        public ActionResult grid_user_menu([DataSourceRequest] DataSourceRequest request, string Id )
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_get_user_menu] (@Id) order by menuname";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@Id", Id);
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult grid_available_menu([DataSourceRequest] DataSourceRequest request, string Id)
        {
            DataTable rec = new DataTable();
            string cmdStr = "select * from [Accounting].[ufn_get_available_menu] (@Id) order by menuname";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@Id", Id);
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult add_usermenu(string eidString = "", int eid = 0)
        {
            if (eid > 0)
            {
                var eidstring = ',' + eid.ToString() + ")";
                string val;
                val = eidString.Replace("==", eidstring).Replace("##", "(");

                ISfn.ExcecuteNoneQuery("insert into [IFMIS].[dbo].[tbl_Payroll_UserOfficeAccess]( [officeID],[eid]) values " + val + "");
            }
            return Content("6");
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUsers([DataSourceRequest] DataSourceRequest request,string para)
        {
            var users = db.AspNetUsers
                          .Select(u => new AspNetUserViewModel
                          {
                              Id = u.Id,
                              UserID = u.UserID,
                              Email = u.Email,
                              EmailConfirmed = u.EmailConfirmed,
                              PhoneNumber = u.PhoneNumber,
                              UserName = u.UserName
                          })
                          .Take(1000)
                          .ToList();

            return Json(users.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateEmailConfirmed(string id, bool emailConfirmed)
        {
            var user = db.AspNetUsers.Find(id);
            
            if (user != null)
            {
                user.EmailConfirmed = emailConfirmed;
                db.SaveChanges();
                return Json(new { success = true,mgs="" });
            }
            return Json(new { success = false,mgs="" });
        }

    }
}