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
//    [Authorize(Roles = "Administradores")]
    public class VeiculosController : Controller
    {
        private entre_rodasEntities db = new entre_rodasEntities();

        // GET: Veiculos
        public ActionResult Index()
        {
            var veiculos = db.Veiculos.Include(v => v.Clientes).Include(v => v.MarcasCarros);
            return View(veiculos.ToList());
        }

        // GET: Veiculos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veiculos veiculos = db.Veiculos.Find(id);
            if (veiculos == null)
            {
                return HttpNotFound();
            }
            return View(veiculos);
        }

        // GET: Veiculos/Create
        public ActionResult Create(int? id)
        {
            Clientes cliente = db.Clientes.Find(id);
            ViewBag.ClienteId = id;
            ViewBag.NomeCliente = cliente.Nome;
            ViewBag.MarcaVeiculoId = new SelectList(db.MarcasCarros, "Id", "Nome");
            CombosGenericos combos = new CombosGenericos();
            ViewBag.TipoCombustivel = new SelectList(combos.ListarTipoCombustivel(), "Valor", "Texto");
            ViewBag.Ano = new SelectList(combos.ListarAnos(), "Valor", "Texto");
            return View();
        }

        // POST: Veiculos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClienteId,MarcaVeiculoId,Modelo,Ano,Placa,CategoriaCarro,TipoCombustivel,TipoMotor,QuilometragemAtual,Observacoes")] Veiculos veiculos)
        {
            if (ModelState.IsValid)
            {
                Veiculos veiculo = new Veiculos();
                veiculo.Ano = veiculos.Ano;
                veiculo.CategoriaCarro = veiculos.CategoriaCarro.Trim();
                veiculo.ClienteId = veiculos.ClienteId;
                veiculo.MarcaVeiculoId = veiculos.MarcaVeiculoId;
                veiculo.Modelo = veiculos.Modelo.Trim();
                if (veiculo.Observacoes != null)
                {
                    veiculo.Observacoes = veiculos.Observacoes.Trim();
                }
                veiculo.Placa = veiculos.Placa.Trim();
                if (veiculos.QuilometragemAtual == 0)
                {
                    veiculo.QuilometragemAtual = 1;
                }
                else
                {
                    veiculo.QuilometragemAtual = veiculos.QuilometragemAtual;
                }
                veiculo.TipoCombustivel = veiculos.TipoCombustivel.Trim();
                veiculo.TipoMotor = veiculos.TipoMotor.Trim();
                db.Veiculos.Add(veiculo);
                db.SaveChanges();
                return RedirectToAction("Details", "Clientes", new { id = veiculos.ClienteId });
            }

            return RedirectToAction("Details", "Clientes", new { id = veiculos.ClienteId });
        }

        // GET: Veiculos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veiculos veiculos = db.Veiculos.Find(id);
            if (veiculos == null)
            {
                return HttpNotFound();
            }
            Veiculos veiculo = new Veiculos()
            {
                Ano = veiculos.Ano,
                CategoriaCarro = veiculos.CategoriaCarro.Trim(),
                ClienteId = veiculos.ClienteId,
                Id = veiculos.Id,
                MarcaVeiculoId = veiculos.MarcaVeiculoId,
                Modelo = veiculos.Modelo.Trim(),
                Observacoes = veiculos.Observacoes,
                Placa = veiculos.Placa.Trim(),
                QuilometragemAtual = veiculos.QuilometragemAtual,                
            };
            if (veiculos.TipoMotor != null)
            {
                veiculo.TipoMotor = veiculos.TipoMotor;
            }
            Clientes cliente = db.Clientes.Find(veiculos.ClienteId);
            MarcasCarros marca = db.MarcasCarros.Find(veiculos.MarcaVeiculoId);
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nome", veiculos.ClienteId);
            ViewBag.NomeCliente = cliente.Nome;
            ViewBag.MarcaVeiculoId = new SelectList(db.MarcasCarros, "Id", "Nome", veiculos.MarcaVeiculoId);
            ViewBag.NomeMarca = marca.Nome;
            CombosGenericos combos = new CombosGenericos();
            ViewBag.TipoCombustivel = new SelectList(combos.ListarTipoCombustivel(), "Valor", "Texto", veiculos.TipoCombustivel.Trim());
            ViewBag.Ano = new SelectList(combos.ListarAnos(), "Valor", "Texto", veiculos.Ano);

            return View(veiculo);
        }

        // POST: Veiculos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClienteId,MarcaVeiculoId,Modelo,Ano,Placa,CategoriaCarro,TipoCombustivel,TipoMotor,Observacoes")] Veiculos veiculos)
        {

                if (ModelState.IsValid)
                {
                    db.Entry(veiculos).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "Clientes", new { id = veiculos.ClienteId });
                }
                ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nome", veiculos.ClienteId);
                ViewBag.MarcaVeiculoId = new SelectList(db.MarcasCarros, "Id", "Nome", veiculos.MarcaVeiculoId);
                return View(veiculos);
        }

        // GET: Veiculos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veiculos veiculos = db.Veiculos.Find(id);
            if (veiculos == null)
            {
                return HttpNotFound();
            }
            return View(veiculos);
        }

        // POST: Veiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Veiculos veiculos = db.Veiculos.Find(id);
            int clienteId = veiculos.Clientes.Id;
            db.Veiculos.Remove(veiculos);
            db.SaveChanges();
            return RedirectToAction("Details", "Clientes", new { id = clienteId });
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
