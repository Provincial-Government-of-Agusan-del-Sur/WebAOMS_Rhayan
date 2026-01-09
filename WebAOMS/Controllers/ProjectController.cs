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
    public class ProjectController : Controller
    {
        //
        // GET: /Project/
        public string cstr = ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString();
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        ifmisEntities ifmisdb = new ifmisEntities();
        pmisEntities pmisdb = new pmisEntities();
        public ActionResult ProjectList()
        {
            return View();
        }
        public ActionResult ProjectCost(Int32 id)
        {
            ViewBag.projectid = id;
            return PartialView("_gridProjectCost",null);
        }
        public ActionResult _Details(Int32 projectid)
        {
            ViewBag.status_code = projectid;
            string cmdStr = "Select * FRom ifmis.dbo.tbl_reff_Project where projectID = @projectID";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@projectID", projectid);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        ViewBag.projectid = reader["projectID"];
                        ViewBag.project_year = reader["project_year"];
                        ViewBag.date_nadai = reader["date_NADAI"];
                        ViewBag.date_completion = reader["date_completion"];
                        ViewBag.programtypeid = reader["programTypeId"];
                        ViewBag.mechanismid = reader["mechanismId"];
                        ViewBag.project_name = reader["project_name"];
                        ViewBag.location = reader["Location"];
                        ViewBag.fundid = reader["fundID"];
                        ViewBag.userid = reader["userID"];
                        ViewBag.modifieddate = reader["ModifiedDate"];
                        ViewBag.active = reader["Active"];
                    }
                }
                else
                {
                    ViewBag.projectid = "0";
                    ViewBag.project_year = "";
                    ViewBag.date_nadai = "";
                    ViewBag.programtypeid = "";
                    ViewBag.mechanismid = "";
                    ViewBag.project_name = "";
                    ViewBag.location = "";
                    ViewBag.fundid = "";
                    ViewBag.userid = "";
                    ViewBag.modifieddate = "";
                    ViewBag.active = "";
                }
            }
            connection.Close();
            return PartialView("_Details", null);
        }
        public JsonResult dp_mechanism()
        {
            IEnumerable<tbl_reff_Project_mechanism> reff_Project_mechanism;
            reff_Project_mechanism = ifmisdb.tbl_reff_Project_mechanism.OrderBy(o => o.mechanismName);
            return Json(reff_Project_mechanism, JsonRequestBehavior.AllowGet);
        }
        public JsonResult dp_projectType()
        {
            IEnumerable<tbl_reff_Project_programType> reff_Project_programType;
            reff_Project_programType = ifmisdb.tbl_reff_Project_programType.OrderBy(o => o.ProgramTypeName);
            return Json(reff_Project_programType, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetgridProject([DataSourceRequest] DataSourceRequest request, string remarks = "")
        {
            IEnumerable<vw_Project_grid> _SOF = ifmisdb.vw_Project_grid.Where(M => M.project_name.ToLower().Contains(remarks));
            return Json(_SOF.ToDataSourceResult(request));
        }
        public ActionResult SaveProject(tbl_reff_Project data)
        {
            try
            {
                if (data.project_year == 0 || string.IsNullOrWhiteSpace(data.project_name) || string.IsNullOrWhiteSpace(data.Location) || data.fundID == 0)
                {
                    return Json(new { success = false, message = "Please fill in the required information before submitting." });
                }
                if (data.projectID == 0)
                {
                    IEnumerable<tbl_reff_Project> _recProject = ifmisdb.tbl_reff_Project.Where(M => M.project_name == data.project_name);
                    if (_recProject.ToList().Count() == 0)
                    {
                        tbl_reff_Project _SaveProject = new tbl_reff_Project();
                        _SaveProject.project_name = data.project_name;
                        _SaveProject.project_year = data.project_year;
                        _SaveProject.Location = data.Location;
                        _SaveProject.fundID = data.fundID;
                        _SaveProject.date_NADAI = data.date_NADAI;
                        _SaveProject.date_completion = data.date_completion;

                        _SaveProject.mechanismId = data.mechanismId;
                        _SaveProject.programTypeId = data.programTypeId;
                        _SaveProject.userID = USER.C_eID;
                        ifmisdb.tbl_reff_Project.Add(_SaveProject);
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
                    IEnumerable<tbl_reff_Project> _recUpdateProject = ifmisdb.tbl_reff_Project.Where(M => M.projectID == data.projectID);
                    
                    if (_recUpdateProject.ToList().Count() > 0)
                    {
                        tbl_reff_Project _UpdateProject = ifmisdb.tbl_reff_Project.Single(M => M.projectID == data.projectID);
                        ISfn.LogData(User.Identity.Name, "Updating Data", data.projectID, "Update tbl_reff_Project ", JsonConvert.SerializeObject(_UpdateProject));
                        _UpdateProject.project_name = data.project_name;
                        _UpdateProject.project_year = data.project_year;
                        _UpdateProject.programTypeId = data.programTypeId;
                        _UpdateProject.date_NADAI = data.date_NADAI;
                        _UpdateProject.date_completion = data.date_completion;

                        _UpdateProject.mechanismId = data.mechanismId;
                        _UpdateProject.project_year = data.project_year;
                        _UpdateProject.Location = data.Location;
                        _UpdateProject.userID = 0;

                        ifmisdb.Entry(_UpdateProject).State = EntityState.Modified;
                        ifmisdb.SaveChanges();
                        ifmisdb.SaveChanges();

                        return Json(new { success = true, message = "Successfully Save...!" }); 
                    }
                    else
                    {
                        return Json(new { success = false, message = "An error occurred while deleting data." });
                    }
                }
            }
            catch (Exception e)
            {
                ISfn.LogData(User.Identity.Name, "Error saving data", data.projectID, e.Message, JsonConvert.SerializeObject(data));
                return Json(new { success = false, message = "An error occurred while saving data." });
            }
        }

        public PartialViewResult EstimatedProjectCostDetails(int ProjectEstimatedCostID=0)
        {
            
            string cmdStr = "Select * FRom ifmis.dbo.tbl_B_ProjectEstimatedCost where ProjectEstimatedCostID = @ProjectEstimatedCostID";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@ProjectEstimatedCostID", ProjectEstimatedCostID);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {

                        ViewBag.ProjectEstimatedCostID = reader["ProjectEstimatedCostID"];
                        ViewBag.fundid = reader["fundID"];
                        ViewBag.ProjectID = reader["ProjectID"];
                        ViewBag.ProjectCostID = reader["ProjectCostID"];
                        ViewBag.EstimatedAmount = reader["EstimatedAmount"];
                        ViewBag.Tranche = reader["Tranche"];
                        ViewBag.DateReceived = reader["DateReceived"];
                        ViewBag.active = reader["active"];
                        ViewBag.userid = reader["UserID"];
                        ViewBag.modifieddate = reader["ModifiedDate"];
                    }
                }
                else
                {
                    ViewBag.ProjectEstimatedCostID = "0";
                    ViewBag.fundid = "";
                    ViewBag.ProjectID = "";
                    ViewBag.ProjectCostID = "";
                    ViewBag.ProjectCostID = "";
                    ViewBag.Tranche = "";
                    ViewBag.DateReceived = "";
                    ViewBag.active = "";
                    ViewBag.userid = "";
                    ViewBag.modifieddate = "";
                }
            }
            connection.Close();
            return PartialView("_DetailsProjectCost_add",null);
          
        }

        public ActionResult DeleteProject(int projectID = 0,string remarks="")
        {
            try
            {
                IEnumerable<tbl_reff_Project> _project = ifmisdb.tbl_reff_Project.Where(M => M.projectID == projectID);
                //log data
                
                if (_project.ToList().Count() > 0)
                {
                    tbl_reff_Project _SOFDelete = ifmisdb.tbl_reff_Project.Single(M => M.projectID == projectID);
                    
                    ifmisdb.tbl_reff_Project.Remove(_SOFDelete);
                    ifmisdb.SaveChanges();
                    ISfn.LogData(User.Identity.Name, "Deleting Data", projectID, remarks, JsonConvert.SerializeObject(_project));
                }
                return Content("6");
            }
            catch (Exception e)
            {
                string r = ISfn.errorLog(e.HResult, e.Message, "Project/DeleteProject");
                return Content(r);
            }
        }

        //-----------------Project Cost--------------------------------------------------------------------
        public ActionResult GetgridProjectCost([DataSourceRequest] DataSourceRequest request, string porjectCostName = "", int projectID = 0)
        {
            IEnumerable<vw_projectEstimatedCost> _SOF;
            if ((projectID > 0) && (porjectCostName != ""))
            {
                _SOF = ifmisdb.vw_projectEstimatedCost.Where(M => M.projectID == projectID);
            }
            else
            {
                _SOF = ifmisdb.vw_projectEstimatedCost.Where(M => M.ProjectCostName.ToLower().Contains(porjectCostName) && M.projectID == projectID);
            }

            return Json(_SOF.ToDataSourceResult(request, m => new { ProjectEstimatedCostID = m.ProjectEstimatedCostID, projectCostID = m.projectCostID, ProjectCostName = m.ProjectCostName, EstimatedAmount = m.EstimatedAmount, DateReceived = m.DateReceived, Tranche = m.Tranche, trancheDesc = m.trancheDesc }));
        }

        public ActionResult SaveProjectCost(tbl_B_ProjectEstimatedCost data)
        {
            try
            {
                if (data.ProjectID == 0 || data.ProjectCostID == 0 || data.EstimatedAmount == 0 || data.Tranche == null)
                {
                    return Json(new { success = false, message = "Please fill in the required information before submitting." });
                }
                if (data.ProjectEstimatedCostID == 0)
                {
                    //IEnumerable<tbl_B_ProjectEstimatedCost> _recProject = ifmisdb.tbl_B_ProjectEstimatedCost.Where(M => M.ProjectID == ProjectID);
                    //if (_recProject.ToList().Count() == 0)
                    //{
                        tbl_B_ProjectEstimatedCost _SaveProject = new tbl_B_ProjectEstimatedCost();
                        _SaveProject.ProjectCostID = data.ProjectCostID;
                        _SaveProject.ProjectID = data.ProjectID;
                        _SaveProject.active = 1;
                        _SaveProject.Tranche = data.Tranche;
                        _SaveProject.DateReceived =  data.DateReceived;
                        _SaveProject.EstimatedAmount = data.EstimatedAmount;
                    _SaveProject.UserID = USER.C_eID;

                    ifmisdb.tbl_B_ProjectEstimatedCost.Add(_SaveProject);
                    ifmisdb.SaveChanges();
                    return Json(new { success = true, message = "Successfully Save...!" });
                }
                else
                {
                    IEnumerable<tbl_B_ProjectEstimatedCost> _recUpdateProject = ifmisdb.tbl_B_ProjectEstimatedCost.Where(M => M.ProjectEstimatedCostID == data.ProjectEstimatedCostID);
                    if (_recUpdateProject.ToList().Count() > 0)
                    {
                        tbl_B_ProjectEstimatedCost _UpdateProject = ifmisdb.tbl_B_ProjectEstimatedCost.Single(M => M.ProjectEstimatedCostID == data.ProjectEstimatedCostID);
                        _UpdateProject.ProjectCostID = data.ProjectCostID;
                        _UpdateProject.ProjectID = data.ProjectID;
                        _UpdateProject.EstimatedAmount = data.EstimatedAmount;
                        _UpdateProject.UserID =USER.C_eID ;
                        _UpdateProject.Tranche = data.Tranche;
                        _UpdateProject.DateReceived = data.DateReceived;

                        ifmisdb.Entry(_UpdateProject).State = EntityState.Modified;
                        ifmisdb.SaveChanges();

                        return Json(new { success = true, message = "Successfully Save...!" }); 
                    }
                    else
                    {
                        return Json(new { success = false, message = "An error occurred while deleting data." });
                    }
                }
            }
            catch (Exception e)
            {
                string r = ISfn.errorLog(e.HResult, e.Message, "Project/SaveProjectCost");
                return Json(new { success = false, message = "An error occurred while saving data." });
            }
        }

        public ActionResult DeleteProjectCost(int ProjectEstimatedCostID = 0,string remarks="")
        {
            try
            {
                IEnumerable<tbl_B_ProjectEstimatedCost> _project = ifmisdb.tbl_B_ProjectEstimatedCost.Where(M => M.ProjectEstimatedCostID == ProjectEstimatedCostID);
                if (_project.ToList().Count() > 0)
                {
                    tbl_B_ProjectEstimatedCost _SOFDelete = ifmisdb.tbl_B_ProjectEstimatedCost.Single(M => M.ProjectEstimatedCostID == ProjectEstimatedCostID);
                    ISfn.LogData(User.Identity.Name, "Deleting Data tbl_B_ProjectEstimatedCost where ProjectEstimatedCostID = "+ ProjectEstimatedCostID + "", ProjectEstimatedCostID, remarks, JsonConvert.SerializeObject(_SOFDelete));
                    ifmisdb.tbl_B_ProjectEstimatedCost.Remove(_SOFDelete);
                    ifmisdb.SaveChanges();
                }
                return Json(new { success = true, message = "Deleted Successfully" });
            }
            catch (Exception e)
            {
                string r = ISfn.errorLog(e.HResult, e.Message, "Project/DeleteProjectCost");
                return Json(new { success = false, message = "An error occurred while deleting data." });
            }
        }

        public ActionResult AjaxContent()
        {
            return View();
        }
    }
}
