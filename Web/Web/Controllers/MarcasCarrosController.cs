using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
//    [Authorize(Roles = "Administradores")]
    public class MarcasCarrosController : Controller
    {
        private entre_rodasEntities db = new entre_rodasEntities();

        // GET: MarcasCarros
        public ActionResult Index(string Pesquisar = "")
        {
            var query = db.MarcasCarros.AsQueryable();
            if (!string.IsNullOrEmpty(Pesquisar))
            {
                query = query.Where(c => c.Nome.Contains(Pesquisar));
            }
            query = query.OrderBy(c => c.Nome);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_MarcasCarros", query.ToList());
            }
            return View(query.ToList());
        }

        // GET: MarcasCarros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarcasCarros marcasCarros = db.MarcasCarros.Find(id);
            if (marcasCarros == null)
            {
                return HttpNotFound();
            }
            return View(marcasCarros);
        }

        // GET: MarcasCarros/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MarcasCarros/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome")] MarcasCarros marcasCarros)
        {
            if (ModelState.IsValid)
            {
                marcasCarros.Nome = marcasCarros.Nome.Trim();
                db.MarcasCarros.Add(marcasCarros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(marcasCarros);
        }

        // GET: MarcasCarros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarcasCarros marcasCarros = db.MarcasCarros.Find(id);
            if (marcasCarros == null)
            {
                return HttpNotFound();
            }
            marcasCarros.Nome = marcasCarros.Nome.Trim();
            return View(marcasCarros);
        }

        // POST: MarcasCarros/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome")] MarcasCarros marcasCarros)
        {
            try
            {
                marcasCarros.Nome = marcasCarros.Nome.Trim();
                if (ModelState.IsValid)
                {
                    db.Entry(marcasCarros).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(marcasCarros);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        // GET: MarcasCarros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarcasCarros marcasCarros = db.MarcasCarros.Find(id);
            if (marcasCarros == null)
            {
                return HttpNotFound();
            }
            return View(marcasCarros);
        }

        // POST: MarcasCarros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MarcasCarros marcasCarros = db.MarcasCarros.Find(id);
            db.MarcasCarros.Remove(marcasCarros);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
