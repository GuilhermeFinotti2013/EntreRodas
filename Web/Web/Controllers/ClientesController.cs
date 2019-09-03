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
    public class ClientesController : Controller
    {
        private entre_rodasEntities db = new entre_rodasEntities();

        // GET: Clientes
        public ActionResult Index()
        {
            return View(db.Clientes.ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Sobrenome,Sexo,DataNascimento,CPF,RG,Email,Telefone,Celular,EhWhats,CEP,Rua,Numero,Complemento,Bairro,Cidade,Observacao, Modelo, Ano, Marca, TipoMotor,ObservacaoCarro")] ClienteViewModel viewCliente,
                                    string cbxSexo, string cbxEhWhats, string cbxTipoCombustivel)
        {
            Veiculos veiculo = new Veiculos()
            {
                MarcaVeiculoId = 2,
                Modelo = viewCliente.Modelo,
                Ano = viewCliente.Ano,
                Placa = viewCliente.Placa,
                TipoCompustivel = cbxTipoCombustivel,
                TipoMotor = viewCliente.TipoMotor,
                CategoriaCarro = "suv",
                Observacoes = viewCliente.ObservacaoCarro
            };
            Clientes cliente = new Clientes()
            {
                Nome = viewCliente.Nome,
                Sobrenome = viewCliente.Sobrenome,
                DataNascimento = viewCliente.DataNascimento,
                Sexo = cbxSexo,
                CPF = viewCliente.CPF,
                RG = viewCliente.RG,
                Email = viewCliente.Email,
                Telefone = viewCliente.Telefone,
                Celular = viewCliente.Celular,
                EhWhats = cbxEhWhats,
                CEP = viewCliente.CEP,
                Rua = viewCliente.Rua,
                Numero = viewCliente.Numero,
                Complemento = viewCliente.Complemento,
                Bairro = viewCliente.Bairro,
                Cidade = viewCliente.Cidade,
            };
            cliente.Veiculos.Add(veiculo);

            if (ModelState.IsValid)
            {
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Sobrenome,Sexo,DataNascimento,CPF,RG,Email,Telefone,Celular,EhWhats,CEP,Rua,Numero,Complemento,Bairro,Cidade,Observacao")] Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clientes);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clientes clientes = db.Clientes.Find(id);
            db.Clientes.Remove(clientes);
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
