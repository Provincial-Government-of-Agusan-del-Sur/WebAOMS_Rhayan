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
using Kendo.Mvc.UI;
using System.Web.Script.Serialization;
using System.Web.Security;
using WebAOMS.Base;
using Newtonsoft.Json;

namespace WebAOMS.Controllers
{
    public class SourceOfFundController : Controller
    {
        //
        // GET: /SourceOfFund/
       
        public string cstr = ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString();
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        ifmisEntities ifmisdb = new ifmisEntities();
        pmisEntities pmisdb = new pmisEntities();
        public ActionResult Index(int id)
        {
            return PartialView("_grid",null);
        }
        public ActionResult _Add_reff_SourceOfFund(Int64 fundid)
        {
            
            string cmdStr = "Select * FRom ifmis.dbo.tbl_reff_SourceOfFund where fundID = @fundid";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@fundid", fundid);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ViewBag.fundID = reader["fundID"];
                        ViewBag.SourceName = reader["SourceName"];
                        ViewBag.fundcode = reader["fundcode"];
                        ViewBag.code = reader["code"];
                    }
                }
                else
                {
                    ViewBag.fundID = "0";
                    ViewBag.SourceName = "";
                    ViewBag.fundcode = "";
                    ViewBag.code = "";

                }
            }
            connection.Close();
            return PartialView("_Details", null);
        }
        public ActionResult GetgridSource([DataSourceRequest] DataSourceRequest request, string remarks = "")
        {
            IEnumerable<tbl_reff_SourceOfFund> _SOF = ifmisdb.tbl_reff_SourceOfFund.Where(M => M.SourceName.ToLower().Contains(remarks));
            return Json(_SOF.ToDataSourceResult(request, m => new { fundID = m.fundID,SourceName = m.SourceName,fundcode = m.fundcode,code = m.code }));            
        }
        public ActionResult SaveSOF(tbl_reff_SourceOfFund data)
        {
            try
            {
            int? n = null;
            if (data.fundcode == 0 || data.SourceName == "")
                {
                    return Json(new { success = false, message = "Please fill in the required information before submitting." });
                }
            if (data.fundID == 0)
                {
                
                    IEnumerable<tbl_reff_SourceOfFund> _recSourceOfFund = ifmisdb.tbl_reff_SourceOfFund.Where(M => M.SourceName == data.SourceName && M.fundcode == data.code);
                    if (_recSourceOfFund.ToList().Count() == 0)
                    {
                        tbl_reff_SourceOfFund _SaveSourceOfFund = new tbl_reff_SourceOfFund();
                        _SaveSourceOfFund.SourceName = data.SourceName;
                        _SaveSourceOfFund.fundcode = data.fundcode;
                        _SaveSourceOfFund.code = data.code;
                        _SaveSourceOfFund.userID = USER.C_eID;
                        ifmisdb.tbl_reff_SourceOfFund.Add(_SaveSourceOfFund);
                        ifmisdb.SaveChanges();
                        return Json(new { success = true, message = "Successfully Save...!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "An error occurred while deleting data." });
                    }
                }
            else
                {
                    IEnumerable<tbl_reff_SourceOfFund> _recUpdateSOF = ifmisdb.tbl_reff_SourceOfFund.Where(M => M.fundID == data.fundID);
                    if (_recUpdateSOF.ToList().Count() > 0)
                    {
                        tbl_reff_SourceOfFund _UpdateSourceOfFund = ifmisdb.tbl_reff_SourceOfFund.Single(M => M.fundID == data.fundID);
                        _UpdateSourceOfFund.SourceName = data.SourceName;
                        _UpdateSourceOfFund.fundcode = data.fundcode;
                        _UpdateSourceOfFund.code = data.code;
                        _UpdateSourceOfFund.userID = USER.C_eID;

                        ifmisdb.Entry(_UpdateSourceOfFund).State = EntityState.Modified;
                        ifmisdb.SaveChanges();

                        return Json(new { success = true, message = "Successfully Save...!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "An error occurred while updating a data." });
                    }
                }
            }
            catch (Exception e)
            {               
                string r = ISfn.errorLog(e.HResult, e.Message, "SourceOfFund/SaveSOF");
                return Json(new { success = false, message = "An error occurred while saving a data." });
            }
        }

        [HttpPost]
        public ActionResult DeleteSOF(int fundID = 0)
        {
            try
            {
                IEnumerable<tbl_reff_SourceOfFund> _SourceOfFund = ifmisdb.tbl_reff_SourceOfFund.Where(M => M.fundID == fundID);
                if (_SourceOfFund.ToList().Count() > 0)
                {
                    tbl_reff_SourceOfFund _SOFDelete = ifmisdb.tbl_reff_SourceOfFund.Single(M => M.fundID == fundID);
                    ISfn.LogData(User.Identity.Name, "deleting Data", fundID, "deleting tbl_reff_SourceOfFund ", JsonConvert.SerializeObject(_SOFDelete));
                    ifmisdb.tbl_reff_SourceOfFund.Remove(_SOFDelete);
                    ifmisdb.SaveChanges();
                }
                return Json(new { success = true, message = "Successfully deleted...!" });
            }
            catch(Exception e)
            {
                ISfn.LogData(User.Identity.Name, "Deleting Data", fundID, "deleting tbl_reff_SourceOfFund ", "");
                return Json(new { success = false, message = "An error occurred while deleting data." });
            }
        }
    }
}
