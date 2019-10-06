using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Content
{
    public class OrdensServicosController : Controller
    {
        private entre_rodasEntities db = new entre_rodasEntities();

        // GET: OrdensServicos
        public ActionResult Index()
        {
            var ordensServicos = db.OrdensServicos.Include(o => o.AspNetUsers).Include(o => o.Clientes);
            return View(ordensServicos.ToList());
        }

        // GET: OrdensServicos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdensServicos ordensServicos = db.OrdensServicos.Find(id);
            if (ordensServicos == null)
            {
                return HttpNotFound();
            }
            return View(ordensServicos);
        }

        // GET: OrdensServicos/Create
        public ActionResult Create()
        {
            ViewBag.Responsavel = new SelectList(db.AspNetUsers, "Id", "Nome");
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nome");
            return View();
        }

        // POST: OrdensServicos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClienteId,Responsavel,DataOrcamento,DataInicialPrevista,DataFinalPrevista,Status,SubTotalServicos,SubTotalMateriais,ValorTotal,ValorAPagar,FormaPagamento,InformacoesAdicionais")] OrdensServicos ordensServicos)
        {
            if (ModelState.IsValid)
            {
                db.OrdensServicos.Add(ordensServicos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Responsavel = new SelectList(db.AspNetUsers, "Id", "Nome", ordensServicos.Responsavel);
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nome", ordensServicos.ClienteId);
            return View(ordensServicos);
        }

        // GET: OrdensServicos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdensServicos ordensServicos = db.OrdensServicos.Find(id);
            if (ordensServicos == null)
            {
                return HttpNotFound();
            }
            ViewBag.Responsavel = new SelectList(db.AspNetUsers, "Id", "Nome", ordensServicos.Responsavel);
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nome", ordensServicos.ClienteId);
            return View(ordensServicos);
        }

        // POST: OrdensServicos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClienteId,Responsavel,DataOrcamento,DataInicialPrevista,DataFinalPrevista,Status,SubTotalServicos,SubTotalMateriais,ValorTotal,ValorAPagar,FormaPagamento,InformacoesAdicionais")] OrdensServicos ordensServicos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordensServicos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Responsavel = new SelectList(db.AspNetUsers, "Id", "Nome", ordensServicos.Responsavel);
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nome", ordensServicos.ClienteId);
            return View(ordensServicos);
        }

        // GET: OrdensServicos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdensServicos ordensServicos = db.OrdensServicos.Find(id);
            if (ordensServicos == null)
            {
                return HttpNotFound();
            }
            return View(ordensServicos);
        }

        // POST: OrdensServicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrdensServicos ordensServicos = db.OrdensServicos.Find(id);
            db.OrdensServicos.Remove(ordensServicos);
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
