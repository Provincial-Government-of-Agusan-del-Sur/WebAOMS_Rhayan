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
    [Authorize(Roles ="Admin")]
    public class CodeGeneratorController : Controller
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        // GET: CodeGenerator
        public ActionResult Index_code_generator()
        {
            ViewBag.Title = "Code Gen";
            ViewBag.Title_mini = "Code Generator";
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a59";
            return View();
        }
        public ActionResult Index_flowchart()
        {

            return View();
        }
        public ActionResult LoadGrid_tsql([DataSourceRequest] DataSourceRequest request, string para)
        {
            DataTable rec;
            if (para == "84306230-966C-4C6A-B5AE-9A58867AB506")
            {
                rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), System.Data.CommandType.Text,
                    "select a.object_id,a.name,b.name as schema_name,a.type_desc from sys.objects as a inner join sys.schemas as b on a.schema_id = b.schema_id and type_desc in ('SQL_STORED_PROCEDURE','VIEW','SQL_SCALAR_FUNCTION','SQL_TABLE_VALUED_FUNCTION','SQL_INLINE_TABLE_VALUED_FUNCTION') order by b.name,a.name").Tables[0];
            }
            else
            {
                rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), System.Data.CommandType.Text,
               "select object_id,name,schema_id from sys.procedures where object_id=0 order by name").Tables[0];
            }
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult grid_tables([DataSourceRequest] DataSourceRequest request,string datatabasename="fmis")
        {
            DataTable crec;
            crec = ISfn.ToDatatable("execute Accounting.usp_code_get_tables @datatabasename", "@datatabasename", datatabasename);
            //return Json(crec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(crec.DTToList().ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult grid_coulmns([DataSourceRequest] DataSourceRequest request, string datatabasename = "fmis",Int64 object_id = 0)
        {
            DataTable crec;
            crec = ISfn.ToDatatable("execute Accounting.usp_code_get_columns @datatabasename,"+object_id, "@datatabasename",datatabasename);

            //return Json(crec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(crec.DTToList().ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public JsonResult DataSource_getDatabase()
        {
            DataTable rec;
            rec = ISfn.ToDatatable("select database_id,name from sys.databases where database_id > 6");
            return Json(rec.DTToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult code_script(string tablename="",string databasename="")
        {
            
            DataSet dset = new DataSet();
            string cmdStr = "execute Accounting.usp_code_gen_get_code @tablename ,@databasename";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@tablename", tablename);
                command.Parameters.AddWithValue("@databasename", databasename);
                connection.Open();

                SqlDataAdapter reader = new SqlDataAdapter(command);
                reader.Fill(dset);
            }
            connection.Close();            
            return PartialView("_code_script",dset);
        }
    }
}