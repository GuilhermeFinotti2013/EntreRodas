using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Web.Util;

namespace Web.Controllers
{
    public class OrdensServicosController : Controller
    {
        private entre_rodasEntities db = new entre_rodasEntities();

        // GET: OrdensServicos
        public ActionResult Index(string Pesquisar = "")
        {
            var query = db.OrdensServicos.AsQueryable();
            if (!string.IsNullOrEmpty(Pesquisar))
            {
                query = query.Where(c => c.Clientes.Nome.Contains(Pesquisar));
            }
            query = query.OrderBy(c => c.Clientes.Nome);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_OrdensServicos", query.ToList());
            }
            return View(query.ToList());
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
            var lista = new List<Models.Clientes>();
            lista.AddRange(db.Clientes.ToList());
            lista.Insert(0, new Clientes() { Id = -1, Nome = "--" });
            ViewBag.ClienteId = new SelectList(lista, "Id", "Nome");
            return View();
        }

        [HttpPost]
        public ActionResult ObterVeiculos(int idCliente)
        {
            List<Veiculos> listaBd = db.Veiculos.Where(c => c.ClienteId == idCliente).ToList();
            List<TODropDownListGenerico> lista = new List<TODropDownListGenerico>();
            foreach (Veiculos item in listaBd)
            {
                lista.Add(new TODropDownListGenerico() { Id = item.Id, Texto = item.Modelo.Trim() });
            }
            return Json(lista);
        }

        // POST: OrdensServicos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string cmd_cliente, string cmd_veiculo)
        {
            /*if (ModelState.IsValid)
            {
                db.OrdensServicos.Add(ordensServicos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Responsavel = new SelectList(db.AspNetUsers, "Id", "Nome", ordensServicos.Responsavel);
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nome", ordensServicos.ClienteId);
            ViewBag.VeiculosId = new SelectList(db.Veiculos, "Id", "Modelo", ordensServicos.VeiculosId);
            return View(ordensServicos);*/
            return View("Index");
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
            ViewBag.VeiculosId = new SelectList(db.Veiculos, "Id", "Modelo", ordensServicos.VeiculosId);
            return View(ordensServicos);
        }

        // POST: OrdensServicos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CodigoOrdensServicos,ClienteId,Responsavel,DataOrcamento,DataInicialPrevista,DataFinalPrevista,Status,SubTotalServicos,SubTotalMateriais,ValorTotal,ValorAPagar,FormaPagamento,InformacoesAdicionais,VeiculosId")] OrdensServicos ordensServicos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordensServicos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Responsavel = new SelectList(db.AspNetUsers, "Id", "Nome", ordensServicos.Responsavel);
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nome", ordensServicos.ClienteId);
            ViewBag.VeiculosId = new SelectList(db.Veiculos, "Id", "Modelo", ordensServicos.VeiculosId);
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
