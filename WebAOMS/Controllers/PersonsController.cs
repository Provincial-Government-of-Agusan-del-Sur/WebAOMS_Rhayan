using System;
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
using System.Web.Services;
using System.Web.Services.Protocols;
using Kendo.Mvc.UI;
using System.Web.Script.Serialization;
using System.Web.Security;
using WebAOMS.Models;
using WebAOMS.Base;
using WebAOMS.Mod;
using WebAOMS.wsfmis;

namespace WebAOMS.Controllers
{
    [Authorize]
    public class PersonsController : Controller
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        string dbcon_fmis = ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString();
        fmisEntities fmisdb = new fmisEntities();
        // GET: Persons
        public ActionResult OtherIndividual()
        {
            ViewBag.menuid = "a74";
            return View();
        }
        public ActionResult data_grid_persons([DataSourceRequest] DataSourceRequest request, int groupid)
        {
            IEnumerable<vw_Persons> _persons;
            _persons = fmisdb.vw_Persons.Where(w => w.person_group_id == groupid);
            var jsonResult = Json(_persons.ToDataSourceResult(request));
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult get_person_group()
        {
            IEnumerable<tbl_l_Persons_group> person_group;
            person_group = fmisdb.tbl_l_Persons_group;
            return Json(person_group.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_person_address()
        {
            IEnumerable<tbl_l_Address> _address;
            _address = fmisdb.tbl_l_Address;
            return Json(_address.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult _add_person(string claimantcode)
        {
            var persons = fmisdb.tbl_l_Persons.Where(M => M.ClaimantCode == claimantcode).ToList();

            if (persons.Count > 0)
            {
                ViewBag.isEdit = 1; // Assuming you want to set this to 1 if there are any matching records

                // Assuming you want to handle only the first matching record
                var firstPerson = persons.First();
                ViewBag.lname = firstPerson.lname;
                ViewBag.fname = firstPerson.fname;
                ViewBag.mname = firstPerson.mname;
                ViewBag.Name = firstPerson.lname + ", " + firstPerson.fname + ' ' + firstPerson.mname;
                ViewBag.suffix = firstPerson.suffix;
                ViewBag.brgyid = firstPerson.brgyid;
                ViewBag.ClaimantCode = claimantcode;
                ViewBag.ContactNo = firstPerson.ContactNo;
                ViewBag.Email = firstPerson.Email;
                ViewBag.AccountNo = firstPerson.AccountNo;
            }
            else
            {
                // Handle case where no matching records are found
                ViewBag.isEdit = 0;
            }
            return PartialView("_add_person", null);
            
        }

        public ActionResult save_person(tbl_l_Persons data)
        {
            int userid = Convert.ToInt32(USER.C_swipeID);
            int result = 0;

            string Claimantcode = "";
            DataTable rec = new DataTable();
            SqlConnection connection = new SqlConnection(fmisConn);


            string cmdStr = "execute [Accounting].[usp_Save_persons] @claimantcode,@brgyid,@ContactNo ,@AccountNo,@groupid,@email";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@claimantcode", SqlDbType.NVarChar, 10).Value = (object)data.ClaimantCode ?? DBNull.Value;
                command.Parameters.Add("@brgyid", SqlDbType.NVarChar, 1000).Value = (object)data.brgyid ?? DBNull.Value;
                command.Parameters.Add("@ContactNo", SqlDbType.NVarChar, 25).Value = (object)data.ContactNo ?? DBNull.Value;
                command.Parameters.Add("@AccountNo", SqlDbType.NVarChar, 12).Value = (object)data.AccountNo ?? DBNull.Value;
                command.Parameters.Add("@email", SqlDbType.NVarChar, 50).Value = (object)data.Email ?? DBNull.Value;
                command.Parameters.Add("@groupid", SqlDbType.Int).Value = (object)data.person_group_id ?? DBNull.Value; 
                connection.Open();

                SqlDataReader read = command.ExecuteReader();
                if (read.HasRows == true)
                {
                    while (read.Read())
                    {
                        result = Convert.ToInt16(read["result"]);
                    }
                }

                connection.Close();
                if (result == 6)
                {
                    return Json(new { code = 6, statusName = "Successfully Saved..!", Claimantcode = Claimantcode });
                }
                else
                {
                    return Json(new { code = 5, statusName = "Name already on the database..", Claimantcode = Claimantcode });
                }

            }
        }

    }
}