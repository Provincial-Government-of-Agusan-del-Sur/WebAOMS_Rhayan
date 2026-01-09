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

namespace WebAOMS.Repositories
{
    public class BusgetaryRequirementsServices:IDisposable
    {
        private static bool UpdateDatabase = false;
        private fmisEntities entities;

        public BusgetaryRequirementsServices(fmisEntities entities)
        {
            this.entities = entities;
        }

        public IList<tbl_t_DocEvents_BudgetaryRequirements> GetAll()
        {
            var result = HttpContext.Current.Session["budgetaryRequirement"] as IList<tbl_t_DocEvents_BudgetaryRequirements>;

            if (result == null || UpdateDatabase)
            {
                result = entities.tbl_t_DocEvents_BudgetaryRequirements.Select(budgetaryrequirement => new tbl_t_DocEvents_BudgetaryRequirements
                {
                    EventId = budgetaryrequirement.EventId,
                    BudgetId = budgetaryrequirement.BudgetId,
                    Amount = budgetaryrequirement.Amount.HasValue ? budgetaryrequirement.Amount.Value : default(decimal),
                }).ToList();

                HttpContext.Current.Session["budgetaryRequirement"] = result;
            }

            return result;
        }

        public IEnumerable<tbl_t_DocEvents_BudgetaryRequirements> Read()
        {
            return GetAll();
        }

        public void Create(tbl_t_DocEvents_BudgetaryRequirements budgetaryrequirement)
        {
            if (!UpdateDatabase)
            {
                var first = GetAll().OrderByDescending(e => e.BudgetId).FirstOrDefault();
                var id = (first != null) ? first.BudgetId : 0;

                budgetaryrequirement.BudgetId = id + 1;

                GetAll().Insert(0, budgetaryrequirement);
            }
            else
            {
                var entity = new tbl_t_DocEvents_BudgetaryRequirements();

                entity.Particular = budgetaryrequirement.Particular;
                entity.Amount = budgetaryrequirement.Amount;
                

                entities.tbl_t_DocEvents_BudgetaryRequirements.Add(entity);
                entities.SaveChanges();

                budgetaryrequirement.BudgetId = entity.BudgetId;
            }
        }

        public void Update(tbl_t_DocEvents_BudgetaryRequirements budgetaryrequirement)
        {
            if (!UpdateDatabase)
            {
                var target = One(e => e.BudgetId == budgetaryrequirement.BudgetId);

                if (target != null)
                {
                    target.Particular = budgetaryrequirement.Particular;
                    target.Amount = budgetaryrequirement.Amount;
                    target.EventId = budgetaryrequirement.EventId;
                    
                }
            }
            else
            {
                var entity = new tbl_t_DocEvents_BudgetaryRequirements();

                entity.BudgetId = budgetaryrequirement.BudgetId;
                entity.Particular = budgetaryrequirement.Particular;
                entity.Amount = budgetaryrequirement.Amount;

                


                entities.tbl_t_DocEvents_BudgetaryRequirements.Attach(entity);
                entities.Entry(entity).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }

        public void Destroy(tbl_t_DocEvents_BudgetaryRequirements budgetaryrequirement)
        {
            if (!UpdateDatabase)
            {
                var target = GetAll().FirstOrDefault(p => p.BudgetId == budgetaryrequirement.BudgetId);
                if (target != null)
                {
                    GetAll().Remove(target);
                }
            }
            else
            {
                var entity = new tbl_t_DocEvents_BudgetaryRequirements();

                entity.BudgetId = budgetaryrequirement.BudgetId;

                entities.tbl_t_DocEvents_BudgetaryRequirements.Attach(entity);

                entities.tbl_t_DocEvents_BudgetaryRequirements.Remove(entity);


                entities.SaveChanges();
            }
        }

        public tbl_t_DocEvents_BudgetaryRequirements One(Func<tbl_t_DocEvents_BudgetaryRequirements, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}