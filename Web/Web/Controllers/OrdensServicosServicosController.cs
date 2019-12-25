using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class OrdensServicosServicosController : Controller
    {
        private entre_rodasEntities db = new entre_rodasEntities();
        // GET: OrdensServicosServicos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdensServicosServicos ordensServicosServicos = db.OrdensServicosServicos.Find(id);
            if (ordensServicosServicos == null)
            {
                return HttpNotFound();
            }
            return View(ordensServicosServicos);
        }

        // GET: OrdensServicosServicos/Create
        public ActionResult Create(int? id)
        {
            OrdensServicos ordem = db.OrdensServicos.Find(id);
            ViewBag.OrdemServicoId = id;
            ViewBag.NomeCliente = ordem.Clientes.Nome;
            ViewBag.ModeloCarro = String.Format("{0} {1} Ano {2}", ordem.Veiculos.MarcasCarros.Nome.Trim(),
                                                 ordem.Veiculos.Modelo.Trim(), ordem.Veiculos.Ano);
            return View();
        }

        // POST: OrdensServicosServicos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrdensServicosId,Descricao,Valor")] OrdensServicosServicos ordensServicosServicos)
        {
            if (ModelState.IsValid)
            {
                db.OrdensServicosServicos.Add(ordensServicosServicos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrdensServicosId = new SelectList(db.OrdensServicos, "Id", "CodigoOrdensServicos", ordensServicosServicos.OrdensServicosId);
            return View(ordensServicosServicos);
        }

        // GET: OrdensServicosServicos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdensServicosServicos ordensServicosServicos = db.OrdensServicosServicos.Find(id);
            if (ordensServicosServicos == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrdensServicosId = new SelectList(db.OrdensServicos, "Id", "CodigoOrdensServicos", ordensServicosServicos.OrdensServicosId);
            return View(ordensServicosServicos);
        }

        // POST: OrdensServicosServicos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrdensServicosId,Descricao,Valor")] OrdensServicosServicos ordensServicosServicos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordensServicosServicos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrdensServicosId = new SelectList(db.OrdensServicos, "Id", "CodigoOrdensServicos", ordensServicosServicos.OrdensServicosId);
            return View(ordensServicosServicos);
        }

        // GET: OrdensServicosServicos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdensServicosServicos ordensServicosServicos = db.OrdensServicosServicos.Find(id);
            if (ordensServicosServicos == null)
            {
                return HttpNotFound();
            }
            return View(ordensServicosServicos);
        }

        // POST: OrdensServicosServicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrdensServicosServicos ordensServicosServicos = db.OrdensServicosServicos.Find(id);
            db.OrdensServicosServicos.Remove(ordensServicosServicos);
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
