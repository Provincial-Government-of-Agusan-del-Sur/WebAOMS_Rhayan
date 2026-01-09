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

namespace WebAOMS.Controllers
{
    public class ProjectCostTypeController : Controller
    {
        public string cstr = ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString();
        ifmisEntities ifmisdb = new ifmisEntities();
        pmisEntities pmisdb = new pmisEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetgridProjectCostType([DataSourceRequest] DataSourceRequest request, string remarks = "")
        {
            IEnumerable<tbl_reff_ProjectCostType> _projectCostType = ifmisdb.tbl_reff_ProjectCostType.Where(M => M.ProjectCostName.ToLower().Contains(remarks));
            return Json(_projectCostType.ToDataSourceResult(request, m => new { projectCostID = m.projectCostID, ParentProjectCostID = m.ParentProjectCostID, ProjectCostName = m.ProjectCostName}));


        }
        public ActionResult SaveProjectCostType(int projectCostID = 0, int ParentProjectCostID = 0, string ProjectCostName = "")
        {
            try
            {
                if (ProjectCostName == "")
                {
                    return Content("8");
                }

                if (projectCostID == 0)
                {
                    IEnumerable<tbl_reff_ProjectCostType> _recProject = ifmisdb.tbl_reff_ProjectCostType.Where(M => M.ProjectCostName == ProjectCostName);
                    if (_recProject.ToList().Count() == 0)
                    {
                        
                        tbl_reff_ProjectCostType _SaveProjectCostType = new tbl_reff_ProjectCostType();
                        _SaveProjectCostType.ProjectCostName = ProjectCostName;
                        if (ParentProjectCostID > 0)
                        {
                            _SaveProjectCostType.ParentProjectCostID = ParentProjectCostID;
                        }
                        ifmisdb.tbl_reff_ProjectCostType.Add(_SaveProjectCostType);
                        ifmisdb.SaveChanges();
                        return Content("6");
                    }
                    else
                    {
                        return Content("4");
                    }
                }
                else
                {
                    IEnumerable<tbl_reff_ProjectCostType> _recUpdateProjectCostType = ifmisdb.tbl_reff_ProjectCostType.Where(M => M.projectCostID == projectCostID);
                    if (_recUpdateProjectCostType.ToList().Count() > 0)
                    {
                        tbl_reff_ProjectCostType _UpdateProjectCostType = ifmisdb.tbl_reff_ProjectCostType.Single(M => M.projectCostID == projectCostID);
                        _UpdateProjectCostType.ProjectCostName = ProjectCostName;
                        if (ParentProjectCostID  > 0) {
                            _UpdateProjectCostType.ParentProjectCostID = ParentProjectCostID;
                        }
                        else{
                            _UpdateProjectCostType.ParentProjectCostID = null;
                        }
                        _UpdateProjectCostType.userID = 0;

                        ifmisdb.Entry(_UpdateProjectCostType).State = EntityState.Modified;
                        ifmisdb.SaveChanges();

                        return Content("6");
                    }
                    else
                    {
                        return Content("4");
                    }
                }
            }
            catch (Exception e)
            {
                string r = ISfn.errorLog(e.HResult, e.Message, "ProjectCostType/DeleteProjectCostType");
                return Content(r);
            }
        }

        public ActionResult DeleteProjectCostType(int projectCostID = 0)
        {
            try
            {
                IEnumerable<tbl_reff_ProjectCostType> _project = ifmisdb.tbl_reff_ProjectCostType.Where(M => M.projectCostID == projectCostID);
                if (_project.ToList().Count() > 0)
                {
                    tbl_reff_ProjectCostType _DeleteProjectCostType = ifmisdb.tbl_reff_ProjectCostType.Single(M => M.projectCostID == projectCostID);
                    ifmisdb.tbl_reff_ProjectCostType.Remove(_DeleteProjectCostType);
                    ifmisdb.SaveChanges();
                }
                return Content("6");

            }
            catch (Exception e)
            {
                string r = ISfn.errorLog(e.HResult, e.Message, "ProjectCostType/DeleteProjectCostType");
                return Content(r);
            }
        }
    }
}
