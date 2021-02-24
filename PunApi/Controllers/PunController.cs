using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using PunDataAccess;

namespace PunApi.Controllers
{
    [EnableCors(origins: "https://punapi.azurewebsites.net", headers: "*", methods: "*")]
    public class PunController : ApiController
    {
        public IEnumerable<Pun> Get()
        {
            using (PunDBEntities entities = new PunDBEntities())
            {
                return entities.Pun.ToList();
            }
        }
        public Pun Get(int id)
        {
            using (PunDBEntities entities = new PunDBEntities())
            {
                return entities.Pun.FirstOrDefault(e => e.Id == id);
            }
        }
        public IHttpActionResult Post(Pun pun)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            using (PunDBEntities entities = new PunDBEntities())
            {
                entities.Pun.Add(new Pun()
                {
                    Question = pun.Question,
                    Answer = pun.Answer
                });

                entities.SaveChanges();
            }

            return Ok();
        }
        public IHttpActionResult Put(Pun pun)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Not a valid model");
            }

            using (PunDBEntities entities = new PunDBEntities())
            {
                var existingPun = entities.Pun.Where(e => e.Id == pun.Id).FirstOrDefault<Pun>();
                if (existingPun != null)
                {
                    existingPun.Question = pun.Question;
                    existingPun.Answer = pun.Answer;

                    entities.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Not a valid id");
            }

            using (PunDBEntities entities = new PunDBEntities())
            {
                var pun = entities.Pun.Where(e => e.Id == id).FirstOrDefault();

                entities.Entry(pun).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }

            return Ok();
        }
    }
}
