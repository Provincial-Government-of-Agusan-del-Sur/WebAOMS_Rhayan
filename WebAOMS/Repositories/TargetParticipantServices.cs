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
    public class TargetParticipantServices : IDisposable
    {

        private static bool UpdateDatabase = false;
        private fmisEntities entities;

        public TargetParticipantServices(fmisEntities entities)
        {
            this.entities = entities;
        }

        public IList<tbl_t_DocEvents_TargetParticipants> GetAll()
        {
            var result = HttpContext.Current.Session["targetParticipants"] as IList<tbl_t_DocEvents_TargetParticipants>;

            if (result == null || UpdateDatabase)
            {
                result = entities.tbl_t_DocEvents_TargetParticipants.Select(targetparticipant => new tbl_t_DocEvents_TargetParticipants
                {
                    EventId = targetparticipant.EventId,
                    ParticipantId = targetparticipant.ParticipantId,
                    NoOfPax = targetparticipant.NoOfPax.HasValue ? targetparticipant.NoOfPax.Value : default(int),
                }).ToList();

                HttpContext.Current.Session["targetParticipants"] = result;
            }

            return result;
        }

        public IEnumerable<tbl_t_DocEvents_TargetParticipants> Read()
        {
            return GetAll();
        }

        public void Create(tbl_t_DocEvents_TargetParticipants targetparticipant)
        {
            if (!UpdateDatabase)
            {
                var first = GetAll().OrderByDescending(e => e.ParticipantId).FirstOrDefault();
                var id = (first != null) ? first.ParticipantId : 0;

                targetparticipant.ParticipantId = id + 1;
              
                GetAll().Insert(0, targetparticipant);
            }
            else
            {
                var entity = new tbl_t_DocEvents_TargetParticipants();

                entity.Participant = targetparticipant.Participant;
                entity.NoOfPax = targetparticipant.NoOfPax;
                entity.OfficeID = targetparticipant.OfficeID;
     
                entities.tbl_t_DocEvents_TargetParticipants.Add(entity);
                entities.SaveChanges();

                targetparticipant.ParticipantId = entity.ParticipantId;
            }
        }

        public void Update(tbl_t_DocEvents_TargetParticipants targetparticipant)
        {
            if (!UpdateDatabase)
            {
                var target = One(e => e.ParticipantId == targetparticipant.ParticipantId);

                if (target != null)
                {
                    target.Participant = targetparticipant.Participant;
                    target.NoOfPax = targetparticipant.NoOfPax;
                    target.EventId = targetparticipant.EventId;
                    target.OfficeID = targetparticipant.OfficeID;
                }
            }
            else
            {
                var entity = new tbl_t_DocEvents_TargetParticipants();

                entity.ParticipantId = targetparticipant.ParticipantId;
                entity.Participant = targetparticipant.Participant;
                entity.NoOfPax = targetparticipant.NoOfPax;
                
                entity.OfficeID = targetparticipant.OfficeID;
               

                entities.tbl_t_DocEvents_TargetParticipants.Attach(entity);
                entities.Entry(entity).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }

        public void Destroy(tbl_t_DocEvents_TargetParticipants targetparticipant)
        {
            if (!UpdateDatabase)
            {
                var target = GetAll().FirstOrDefault(p => p.ParticipantId == targetparticipant.ParticipantId);
                if (target != null)
                {
                    GetAll().Remove(target);
                }
            }
            else
            {
                var entity = new tbl_t_DocEvents_TargetParticipants();

                entity.ParticipantId = targetparticipant.ParticipantId;

                entities.tbl_t_DocEvents_TargetParticipants.Attach(entity);

                entities.tbl_t_DocEvents_TargetParticipants.Remove(entity);
                

                entities.SaveChanges();
            }
        }

        public tbl_t_DocEvents_TargetParticipants One(Func<tbl_t_DocEvents_TargetParticipants, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}