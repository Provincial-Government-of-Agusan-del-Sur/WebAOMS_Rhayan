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
    [Authorize(Roles = "Tracking, Admin")]
    public class TrackingController : Controller
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        fmisEntities fmisdb = new fmisEntities();
        
        // GET: Tracking
        public ActionResult Index()
        {
            ViewBag.menuid = "a55";
            ViewBag.rightSidebar_title = "Track And Trace";
            return View();
        }
        public ActionResult _Add(Int64 id)
        {
            ViewBag.status_code = id;
            string cmdStr = "Select * FRom [Tracking].[tbl_l_StatusDescription] where status_code = @status_code";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@status_code", id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ViewBag.status_code = reader["status_code"];
                        ViewBag.prerequisite_id = reader["prerequisite_id"];
                        ViewBag.Status_name = reader["Status_name"];
                        ViewBag.Status_Description = reader["Status_Description"];
                        ViewBag.Status_location = reader["Status_location"];
                        ViewBag.MinutesDone = reader["MinutesDone"];
                        ViewBag.orderby = reader["orderby"];
                    }
                }
                else
                {
                    ViewBag.status_code = 0;
                    ViewBag.prerequisite_id = 0;
                    ViewBag.Status_name = "";
                    ViewBag.Status_Description = "";
                    ViewBag.Status_location = "";
                    ViewBag.MinutesDone = 0;
                    ViewBag.orderby = 0;
                }
            }
            connection.Close();
            return PartialView("_Add",null);
        }
        public ActionResult save_status_details(l_StatusDescription data)
        {
            try
            {
                if (data.Status_name == null)
                {
                    return Json(new { code = 7, statusName = "The Status name is required" });
                }

                //if (data.orderby == 0)
                //{
                //    return Json(new { code = 7, statusName = "The Status name is required" });
                //}

                if (data.Status_location == null)
                {
                    return Json(new { code = 7, statusName = "The Status Location is required" });
                }

                if (data.Status_name.Length < 2)
                {
                    return Json(new { code = 7, statusName = "The Status name must be at least 2 characters long." });
                }

                if (data.Status_location.Length < 2)
                {
                    return Json(new { code = 7, statusName = "The Status Location must be at least 2 characters long." });
                }

                if (data.Status_Description == null)
                {
                    data.Status_Description = " ";
                }

                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute [Tracking].[usp_Save_StatusLogs] @status_code,@IS_id,@prerequisite_id,@Status_name,@Status_Description,@Status_location,@MinutesDone,@user_eid,@orderby";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@status_code", data.status_code);
                    command.Parameters.AddWithValue("@IS_id", data.IS_id);
                    command.Parameters.AddWithValue("@prerequisite_id", data.prerequisite_id);
                    command.Parameters.AddWithValue("@Status_name", data.Status_name);
                    command.Parameters.AddWithValue("@Status_Description", data.Status_Description);
                    command.Parameters.AddWithValue("@Status_location", data.Status_location);
                    command.Parameters.AddWithValue("@MinutesDone", data.MinutesDone);
                    command.Parameters.AddWithValue("@user_eid", USER.C_swipeID);
                    command.Parameters.AddWithValue("@orderby", data.orderby);
                    connection.Open();

                    int rows = command.ExecuteNonQuery();
                    connection.Close();

                    return Json(new { code = 6, statusName = "Successfully Saved..!" });

                }
            }
            catch (Exception e) {
                return Json(new { code = e.HResult, statusName = e.Message });
            }
        }
        public ActionResult delete_status_details(int status_code)
        {
            try
            {

            if (ISfn.ExecScalar("if exists( SELECT log_id FROM [fmis].[Tracking].[tbl_t_transactionDetails_log] as a where status_code = "+status_code+") begin select 1 as isUsed; end else begin select 0 as isUsed end") == "1")
            {
                return Json(new { code = 7, statusName = "Unable to delete the transaction, already in used" });
            }

            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "delete from Tracking.tbl_l_StatusDescription where status_code = @status_code";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@status_code", status_code);
                connection.Open();

                int rows = command.ExecuteNonQuery();
                connection.Close();
                return Json(new { code = 6, statusName = "Successfully Deleted..!" });
            }
        }
            catch (Exception e) {
                return Json(new { code = e.HResult, statusName = e.Message
        });
            }
        }
        public ActionResult grid_Statusname([DataSourceRequest] DataSourceRequest request,int IS_id)
        {
            DataTable crec;
            crec = ISfn.ToDatatable("SELECT [status_code] ,[prerequisite_id],[Status_name],[Status_Description],[Status_location],[MinutesDone],orderby FROM [fmis].[Tracking].[tbl_l_StatusDescription] where IS_id = "+IS_id+"");
            return Json(crec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_getISName()
        {
            IEnumerable<tbl_l_InformationSystem> IS;
            IS = fmisdb.tbl_l_InformationSystem.OrderBy(o => o.is_name);
            return Json(IS.Select(tx => new { is_name = tx.is_name +  "("+ tx.IS_id.ToString() + ")"+ " (" + tx.key_code + ")",IS_id = tx.IS_id}).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataSource_prerequisite(int is_id,int status_code)
        {
            IEnumerable<ufn_ComboSource_prerequisite_Result> IS;
            IS = fmisdb.ufn_ComboSource_prerequisite(status_code, is_id).OrderBy(o => o.Status_name);
            return Json(IS.Select(tx => new { status_name = tx.Status_name, status_code = tx.status_code }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public string save_status_log(string unique_refno, int status_code, string remarks, int userid)
        {
            DGSignController dg = new DGSignController();
            if (status_code < 1)
            {
                return "invalid";
            }

            string result = check_unique_refno(unique_refno, status_code);
            if (result != "success")
            {
                return result;
            }

            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute [Tracking].[usp_Save_StatusLogs_Transaction] @refno,@status_code,@remarks,@userid";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@refno", unique_refno);
                command.Parameters.AddWithValue("@status_code", status_code);
                command.Parameters.AddWithValue("@remarks", remarks);
                command.Parameters.AddWithValue("@userid", userid);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                if (status_code == 401)
                {
                    string id = ISfn.ExecScalar(@"SELECT max(doc_details_id)
                      FROM [fmis].[Accounting].[tbl_t_DocForm] as a 
                      inner join Accounting.tbl_t_DocDetails as e on e.doc_form_id = a.doc_form_id
                      inner join Accounting.tbl_t_DocEvents as f on f.EventId = e.doc_details_id
                      where a.refno = '" + unique_refno + "' and e.isDeleted is null").ToString();

                    string document_type_id = ISfn.ExecScalar(@"SELECT report_id
                      FROM [fmis].[Accounting].[tbl_t_DocForm] as a 
                      inner join Accounting.tbl_t_DocDetails as e on e.doc_form_id = a.doc_form_id
                      inner join Accounting.tbl_t_DocEvents as f on f.EventId = e.doc_details_id
                      where a.refno = '" + unique_refno + "' and e.isDeleted is null").ToString();
                    dg.SignViaDG(Convert.ToInt32(id), unique_refno, Convert.ToInt32(document_type_id));
                }
                return "success";
            }
            
        }
        public string save_status_AD_log(string unique_refno, int status_code, string remarks, int userid)
        {
            DGSignController dg = new DGSignController();
            if (status_code < 1)
            {
                return "invalid";
            }

            string result = check_unique_refno(unique_refno, status_code);
            if (result != "success")
            {
                return result;
            }

            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute [Tracking].[usp_Save_StatusLogs_Transaction] @refno,@status_code,@remarks,@userid";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@refno", unique_refno);
                command.Parameters.AddWithValue("@status_code", status_code);
                command.Parameters.AddWithValue("@remarks", remarks);
                command.Parameters.AddWithValue("@userid", userid);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                if (status_code == 401 || status_code == 414)
                {
                    string doc_details_id = ISfn.ExecScalar(@"SELECT max(doc_details_id)
                      FROM [fmis].[Accounting].[tbl_t_DocForm] as a 
                      inner join Accounting.tbl_t_DocDetails as e on e.doc_form_id = a.doc_form_id
                      where a.refno = '" + unique_refno + "' and e.isDeleted is null").ToString();

                    string document_type_id = ISfn.ExecScalar(@"SELECT report_id
                      FROM [fmis].[Accounting].[tbl_t_DocForm] as a 
                      inner join Accounting.tbl_t_DocDetails as e on e.doc_form_id = a.doc_form_id
                      where e.doc_details_id = '" + doc_details_id + "' and e.isDeleted is null").ToString();

                    //ISfn.ExcecuteNoneQuery("");
                    dg.SignViaDG(Convert.ToInt32(doc_details_id), unique_refno, Convert.ToInt32(document_type_id));



                }

                return "success";
            }

        }

        public string check_unique_refno(string unique_refno, int status_code)
        {
            string result = "";
            int prerequisite = 0;
            int ActiveStatus = 0;
            DataTable rec = new DataTable();
            string cmdStr = @"SELECT b.status_code,isnull(b.prerequisite_id,0) as prerequisite_id,d.Status_name,isnull(c.status_code,0) as ActiveStatus
                            FROM [fmis].[Tracking].[tbl_t_transactionDetails] as a
                            left join tracking.tbl_l_StatusDescription as b on b.IS_id = a.IS_id and b.status_code = @status_code
                            left join Tracking.tbl_l_StatusDescription as d on d.status_code = b.prerequisite_id
                            left join tracking.tbl_t_transactionDetails_log as c on c.trans_id = a.trans_id and isActive = 1
                          where Unique_refno  = @unique_refno";

            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@unique_refno", unique_refno);
                command.Parameters.AddWithValue("@status_code", status_code);

                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }

            if (rec.Rows.Count > 0)
            {
                prerequisite = Convert.ToInt16(rec.Rows[0]["prerequisite_id"]);
                ActiveStatus = Convert.ToInt16(rec.Rows[0]["ActiveStatus"]);

                if (prerequisite == ActiveStatus)
                {
                    return "success";
                }
                else if (ActiveStatus == 0 || prerequisite == 0)
                {
                    return "success";
                }
                else if (ActiveStatus == status_code)
                {
                    return "already";
                }
                else if (ActiveStatus != prerequisite)
                {
                    return rec.Rows[0]["Status_name"].ToString();
                }
            }
            else
            {
                result = "invalid";
            }

            return result;
        }
        public string get_unique_refno(string IS_reffno, int IS_ID, string key_code, string particular, string amount)
        {
            string reffno = "invalid";

            SqlConnection connection = new SqlConnection(fmisConn);
            connection.Open();
            //check the details          

            using (SqlCommand command = new SqlCommand("SELECT [IS_id],[key_code] FROM [fmis].[Tracking].[tbl_l_InformationSystem] where IS_id= @IS_ID and key_code = @key_code", connection))
            {
                command.Parameters.AddWithValue("@IS_ID", IS_ID);
                command.Parameters.AddWithValue("@key_code", key_code);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == false)
                {
                    return "invalid";
                }
                reader.Dispose();
                command.Dispose();
            }
            //try
            //{
                reffno = generate_refno(IS_ID);
                using (SqlCommand cmd = new SqlCommand("execute Tracking.usp_check_refno @IS_ID,@IS_reffno,@Unique_refno,@particular,@amount", connection))
                {

                    cmd.Parameters.AddWithValue("@IS_ID", IS_ID);
                    cmd.Parameters.AddWithValue("@Unique_refno", reffno);
                    cmd.Parameters.AddWithValue("@particular", particular);
                    cmd.Parameters.AddWithValue("@IS_reffno", IS_reffno);
                    cmd.Parameters.AddWithValue("@amount", amount);

                    SqlDataReader Creader = cmd.ExecuteReader();
                    if (Creader.HasRows)
                    {
                        Creader.Read();
                        reffno = Creader["Unique_refno"].ToString();
                    }
                }

            //}
            //catch (Exception e)
            //{
            //    connection.Close();
            //    if (e.HResult == -2146232060)
            //    {
            //        reffno = "";//get_unique_refno(IS_reffno, IS_ID, key_code, particular, amount);
            //    }
            //}

            return reffno;
        }
        private static string generate_refno(int IS_id)
        {
            string results = "";
            results = IS_id.ToString("00") + DateTime.Now.Year.ToString().Substring(2, 2) + RandomString(6); //Guid.NewGuid().ToString().Substring(24, 6);
            return results;
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [WebMethod]
        public DataSet get_log_transactions(string unique_refno)
        {
            DataSet rec = new DataSet();
            string cmdStr = @"execute [Tracking].[usp_get_loghistory]  @Unique_refno";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@unique_refno", unique_refno);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return rec;

        }

        [WebMethod]
        public DataTable get_obrdetails(string unique_refno)
        {
            DataTable rec = new DataTable();
            string cmdStr = @"execute  [Accounting].[usp_rpt_OBR_details_byrefno]  @Unique_refno";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@unique_refno", unique_refno);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return rec;

        }
        [WebMethod]
        public DataSet getTransactions(string dvno)
        {
            string sql = "SELECT * FROM fmis.dbo.fn_searchTransaction(" + dvno.Replace("-", "").Replace("'", "''") + ") order by DTE desc ";
            SqlDataAdapter da = new SqlDataAdapter(sql, ConfigurationManager.ConnectionStrings["fmisConn"].ToString());

            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        [WebMethod]
        public string get_tracking_link(string unique_refno)
        {
            return check_if_exist_unique_refno(unique_refno);
        }
        public string check_if_exist_unique_refno(string unique_refno)
        {
            string result = "invalid";
            DataTable rec = new DataTable();
            string cmdStr = @"SELECT * FROM [fmis].[Tracking].[tbl_t_transactionDetails] where Unique_refno = @unique_refno";

            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@unique_refno", unique_refno);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }

            if (rec.Rows.Count > 0)
            {
                result = "https://pgas.ph/aoms/f/r?r=" + unique_refno + "";
            }
            return result;
        }

    }
}