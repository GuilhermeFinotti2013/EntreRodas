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
            ViewBag.OrdensServicosId = id;
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
                OrdensServicosServicos servico = new OrdensServicosServicos();
                servico.Descricao = ordensServicosServicos.Descricao.Trim();
                servico.Valor = ordensServicosServicos.Valor;
                servico.OrdensServicosId = ordensServicosServicos.OrdensServicosId;
                db.OrdensServicosServicos.Add(servico);
                db.SaveChanges();
                return RedirectToAction("Details", "OrdensServicos", new { id = servico.OrdensServicosId });
            }

            OrdensServicos ordem = db.OrdensServicos.Find(ordensServicosServicos.OrdensServicosId);
            ViewBag.OrdensServicosId = ordensServicosServicos.OrdensServicosId;
            ViewBag.NomeCliente = ordem.Clientes.Nome;
            ViewBag.ModeloCarro = String.Format("{0} {1} Ano {2}", ordem.Veiculos.MarcasCarros.Nome.Trim(),
                                                 ordem.Veiculos.Modelo.Trim(), ordem.Veiculos.Ano);
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
            ViewBag.OrdensServicosId = ordensServicosServicos.OrdensServicosId;
            ViewBag.NomeCliente = ordensServicosServicos.OrdensServicos.Clientes.Nome;
            ViewBag.ModeloCarro = String.Format("{0} {1} Ano {2}", ordensServicosServicos.OrdensServicos.Veiculos.MarcasCarros.Nome.Trim(),
                                                 ordensServicosServicos.OrdensServicos.Veiculos.Modelo.Trim(), ordensServicosServicos.OrdensServicos.Veiculos.Ano);
            OrdensServicosServicos servico = new OrdensServicosServicos();
            servico.Id = ordensServicosServicos.Id;
            servico.Descricao = ordensServicosServicos.Descricao.Trim();
            servico.Valor = ordensServicosServicos.Valor;
            servico.OrdensServicosId = ordensServicosServicos.OrdensServicosId;
            return View(servico);
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
                return RedirectToAction("Details", "OrdensServicos", new { id = ordensServicosServicos.OrdensServicosId });
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
            int ordensServicosId = ordensServicosServicos.OrdensServicosId;
            db.OrdensServicosServicos.Remove(ordensServicosServicos);
            db.SaveChanges();
            return RedirectToAction("Details", "OrdensServicos", new { id = ordensServicosId });
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
