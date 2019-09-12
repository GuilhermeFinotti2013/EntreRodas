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
            ClienteViewModel clienteView = new ClienteViewModel();
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            clienteView.Bairro = clientes.Bairro;
            clienteView.Celular = clientes.Celular;
            clienteView.CEP = clientes.CEP;
            clienteView.Cidade = clientes.Cidade;
            clienteView.Complemento = clientes.Complemento;
            clienteView.CPF = clientes.CPF;
            clienteView.DataNascimento = clientes.DataNascimento;
            clienteView.EhWhats = clientes.EhWhats;
            clienteView.Email = clientes.Email;
            clienteView.Id = clientes.Id;
            clienteView.Nome = clientes.Nome;
            clienteView.Numero = clientes.Numero;
            clienteView.Observacao = clientes.Observacao;
            clienteView.RG = clientes.RG;
            clienteView.Rua = clientes.Rua;
            clienteView.Sexo = clientes.Sexo;
            clienteView.Sobrenome = clientes.Sobrenome;
            clienteView.Telefone = clientes.Telefone;
            clienteView.VeiculosDoCliente = clientes.Veiculos.ToList();
            return View(clienteView);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            ClienteViewModel clienteView = new ClienteViewModel();
            clienteView.MarcasCarros = db.MarcasCarros.ToList();
            return View(clienteView);
        }

        // POST: Clientes/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Sobrenome,Sexo,DataNascimento,CPF,RG,Email,Telefone,Celular,EhWhats,CEP,Rua,Numero,Complemento,Bairro,Cidade,Observacao, MarcaSelecionada, Modelo, Ano, Placa, CategoriaCarro, TipoMotor,ObservacaoCarro")] ClienteViewModel viewCliente,
                                    string cbxSexo, string cbxEhWhats, string cbxTipoCombustivel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Veiculos veiculo = new Veiculos();
                    veiculo.Ano = 200;
                    veiculo.CategoriaCarro = "suv";
                    veiculo.MarcaVeiculoId = 2;//viewCliente.MarcaSelecionada;
                    veiculo.Modelo = "q1";
                    veiculo.Observacoes = viewCliente.ObservacaoCarro;
                    veiculo.Placa = viewCliente.Placa;
                    veiculo.TipoCombustivel = cbxTipoCombustivel;
                    veiculo.TipoMotor = viewCliente.TipoMotor;
                    veiculo.CategoriaCarro = viewCliente.CategoriaCarro;


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

                    cliente.Veiculos = new List<Veiculos>();
                    cliente.Veiculos.Add(veiculo);

                    db.Clientes.Add(cliente);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch (DbEntityValidationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            ClienteViewModel clienteView = new ClienteViewModel();
            clienteView.MarcasCarros = db.MarcasCarros.ToList();
            return View(viewCliente);
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
