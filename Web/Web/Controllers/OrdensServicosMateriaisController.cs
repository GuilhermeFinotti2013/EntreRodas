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
    public class OrdensServicosMateriaisController : Controller
    {
        private entre_rodasEntities db = new entre_rodasEntities();

        // GET: OrdensServicosMateriais/Create
        public ActionResult Create(int? id)
        {
            OrdensServicos ordem = db.OrdensServicos.Find(id);
            ViewBag.OrdensServicosId = id;
            ViewBag.NomeCliente = ordem.Clientes.Nome;
            ViewBag.ModeloCarro = String.Format("{0} {1} Ano {2}", ordem.Veiculos.MarcasCarros.Nome.Trim(),
                                                 ordem.Veiculos.Modelo.Trim(), ordem.Veiculos.Ano);
            return View();
        }

        // POST: OrdensServicosMateriais/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrdensServicosId,Descricao,PrecoUnitario,Quantidade")] OrdensServicosMateriais ordensServicosMateriais)
        {
            if (ModelState.IsValid)
            {
                OrdensServicosMateriais material = new OrdensServicosMateriais();
                material.Descricao = ordensServicosMateriais.Descricao.Trim();
                material.Quantidade = ordensServicosMateriais.Quantidade;
                material.PrecoUnitario = ordensServicosMateriais.PrecoUnitario;
                float valor = ordensServicosMateriais.PrecoUnitario * ordensServicosMateriais.Quantidade;
                material.PrecoTotal = valor;
                material.OrdensServicosId = ordensServicosMateriais.OrdensServicosId;
                db.OrdensServicosMateriais.Add(material);
                db.SaveChanges();
                return RedirectToAction("Details", "OrdensServicos", new { id = material.OrdensServicosId });
            }

            OrdensServicos ordem = db.OrdensServicos.Find(ordensServicosMateriais.OrdensServicosId);
            ViewBag.OrdensServicosId = ordensServicosMateriais.OrdensServicosId;
            ViewBag.NomeCliente = ordem.Clientes.Nome;
            ViewBag.ModeloCarro = String.Format("{0} {1} Ano {2}", ordem.Veiculos.MarcasCarros.Nome.Trim(),
                                                 ordem.Veiculos.Modelo.Trim(), ordem.Veiculos.Ano);
            return View(ordensServicosMateriais);
        }

        // GET: OrdensServicosMateriais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdensServicosMateriais ordensServicosMateriais = db.OrdensServicosMateriais.Find(id);
            if (ordensServicosMateriais == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrdensServicosId = new SelectList(db.OrdensServicos, "Id", "CodigoOrdensServicos", ordensServicosMateriais.OrdensServicosId);
            return View(ordensServicosMateriais);
        }

        // POST: OrdensServicosMateriais/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrdensServicosId,Descricao,PrecoUnitario,Quantidade,PrecoTotal")] OrdensServicosMateriais ordensServicosMateriais)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordensServicosMateriais).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrdensServicosId = new SelectList(db.OrdensServicos, "Id", "CodigoOrdensServicos", ordensServicosMateriais.OrdensServicosId);
            return View(ordensServicosMateriais);
        }

        // GET: OrdensServicosMateriais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdensServicosMateriais ordensServicosMateriais = db.OrdensServicosMateriais.Find(id);
            if (ordensServicosMateriais == null)
            {
                return HttpNotFound();
            }
            return View(ordensServicosMateriais);
        }

        // POST: OrdensServicosMateriais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrdensServicosMateriais ordensServicosMateriais = db.OrdensServicosMateriais.Find(id);
            db.OrdensServicosMateriais.Remove(ordensServicosMateriais);
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
