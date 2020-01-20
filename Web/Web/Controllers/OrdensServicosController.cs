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

            VisualizarServicoViewModel model = new VisualizarServicoViewModel();
            #region Dados do serviço
            model.Id = ordensServicos.Id;
            model.CodigoOrdemServico = ordensServicos.CodigoOrdensServicos;
            if (ordensServicos.DataOrcamento != null)
            {
                model.DataOrcamento = ordensServicos.DataOrcamento.ToString("dd/MM/yyyy");
            }
            else
            {
                model.DataOrcamento = "--";
            }
            model.Status = ordensServicos.Status;
            if (model.Status == "F")
            {
                model.AprovacaoCliente = ordensServicos.AprovacaoCliente;
            }
            if (ordensServicos.ProblemaIdentificado != null)
            {
                model.ProblemaIdentificado = ordensServicos.ProblemaIdentificado;
            }
            else
            {
                model.ProblemaIdentificado = "--";
            }
            AspNetUsers funcionario = db.AspNetUsers.Find(ordensServicos.FuncionarioResponsavel);
            if (funcionario != null)
            {
                model.NomeFuncionarioResponsavel = funcionario.Nome;
            }
            else
            {
                model.NomeFuncionarioResponsavel = "--";
            }
            if (ordensServicos.DataFinal != null)
            {
                model.DataFinal = ordensServicos.DataFinal.Value.ToString("dd/MM/yyyy");
            }
            if (ordensServicos.DataInicial != null)
            {
                model.DataInicial = ordensServicos.DataInicial.Value.ToString("dd/MM/yyyy");
            }
            if (ordensServicos.DataInicialPrevista != null)
            {
                model.DataInicialPrevista = ordensServicos.DataOrcamento.ToString("dd/MM/yyyy");
            }
            else
            {
                model.DataInicialPrevista = "--";
            }
            model.Materiais = ordensServicos.OrdensServicosMateriais.ToList();
            model.Servicos = ordensServicos.OrdensServicosServicos.ToList();
            if (ordensServicos.ValorTotal == null)
            {
                model.ValorTotal = String.Format("R${0}", this.CalcularValorTotal(ordensServicos));
            }
            else
            {
                model.ValorTotal = String.Format("R${0}", ordensServicos.ValorTotal);
            }
            if (ordensServicos.FormaPagamento == null)
            {
                model.FormaPagamento = "--";
            }
            else
            {
                model.FormaPagamento = String.Format("R${0}", ordensServicos.FormaPagamento);
            }
            if (ordensServicos.ValorAPagar == null)
            {
                model.ValorAPagar = "--";
            }
            else
            {
                model.ValorAPagar = String.Format("R${0}", ordensServicos.ValorAPagar);
            }
            if (ordensServicos.ValorCartao == null)
            {
                model.ValorCartao = "--";
            }
            else
            {
                model.ValorCartao = String.Format("R${0}", ordensServicos.ValorCartao);
            }
            if (ordensServicos.ValorDinheiro == null)
            {
                model.ValorDinheiro = "--";
            }
            else
            {
                model.ValorDinheiro = String.Format("R${0}", ordensServicos.ValorDinheiro);
            }
            model.SubTotalMateriais = String.Format("R${0}", this.CalcularValorTotalDeMateriais(ordensServicos.OrdensServicosMateriais.ToList()));
            model.SubTotalServicos = String.Format("R${0}", this.CalcularValorTotalDeServicos(ordensServicos.OrdensServicosServicos.ToList()));
            #endregion
            #region Dados do cliente
            model.ClienteId = ordensServicos.ClienteId;
            model.NomeCliente = ordensServicos.Clientes.Nome.Trim();
            model.EmailCliente = ordensServicos.Clientes.Email.Trim();
            if (ordensServicos.Clientes.Telefone != null)
            {
                model.FonesCliente = String.Format("{0}/{1}", ordensServicos.Clientes.Telefone.Trim(), ordensServicos.Clientes.Celular.Trim());
            }
            else
            {
                model.FonesCliente = ordensServicos.Clientes.Celular.Trim();
            }
            model.VeiculoId = ordensServicos.VeiculosId;
            model.ModeloVeiculo = ordensServicos.Veiculos.Modelo.Trim();
            model.PlacaVeiculo = ordensServicos.Veiculos.Placa.Trim();
            model.AnoVeiculo = ordensServicos.Veiculos.Ano;
            #endregion
            return View(model);
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

        // POST: OrdensServicos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClienteId,VeiculosId,ProblemaIdentificado")] OrdensServicos viewModel)
        {
            if (ModelState.IsValid)
            {
                OrdensServicos ordensServicos = new OrdensServicos();
                ordensServicos.ClienteId = viewModel.ClienteId;
                ordensServicos.VeiculosId = viewModel.VeiculosId;
                ordensServicos.CodigoOrdensServicos = this.ObterCodigoOrdemServico();
                ordensServicos.FuncionarioResponsavel = Convert.ToInt32(Session["IdUser"]);
                ordensServicos.DataOrcamento = DateTime.Now;
                ordensServicos.Status = "O";
                if (viewModel.ProblemaIdentificado != null)
                {
                    ordensServicos.ProblemaIdentificado = viewModel.ProblemaIdentificado;
                }
                db.OrdensServicos.Add(ordensServicos);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = ordensServicos.Id });
            }

            var lista = new List<Models.Clientes>();
            lista.AddRange(db.Clientes.ToList());
            lista.Insert(0, new Clientes() { Id = -1, Nome = "--" });
            ViewBag.ClienteId = new SelectList(lista, "Id", "Nome");
            return View(viewModel);
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
            lista.Insert(0, new TODropDownListGenerico() { Id = -1, Texto = "--" });
            return Json(lista);
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

        private String ObterCodigoOrdemServico()
        {
            string novoCodigo = String.Empty;
            OrdensServicos ultimaOrdem = db.OrdensServicos.OrderByDescending(x => x.Id).Take(1).FirstOrDefault();
            if (ultimaOrdem == null)
            {
                novoCodigo = String.Format("{0}{1}", DateTime.Now.ToString("yyyyMM"), "0001");
            }
            else
            {
                string ultimoMes = ultimaOrdem.CodigoOrdensServicos.Substring(0, 6);
                if (ultimoMes == DateTime.Now.ToString("yyyyMM"))
                {
                    int contServicosMes = Convert.ToInt32(ultimaOrdem.CodigoOrdensServicos.Substring(6,4));
                    contServicosMes++;
                    novoCodigo = String.Format("{0}{1}", DateTime.Now.ToString("yyyyMM"), contServicosMes.ToString().PadLeft(4, '0'));
                }
                else
                {
                    novoCodigo = String.Format("{0}{1}", DateTime.Now.ToString("yyyyMM"), "0001");
                }
            }
            return novoCodigo;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public float CalcularValorTotal(OrdensServicos ordensServicos)
        {
            float valorTotal = 0;

            if (ordensServicos.OrdensServicosMateriais != null && ordensServicos.OrdensServicosMateriais.Count > 0)
            {
                foreach (OrdensServicosMateriais materiais in ordensServicos.OrdensServicosMateriais)
                {
                    valorTotal += materiais.PrecoTotal;
                }
            }
            if (ordensServicos.OrdensServicosServicos != null && ordensServicos.OrdensServicosServicos.Count > 0)
            {
                foreach (OrdensServicosServicos servico in ordensServicos.OrdensServicosServicos)
                {
                    valorTotal += servico.Valor;
                }
            }

            return valorTotal;
        }

        public float CalcularValorTotalDeMateriais(List<OrdensServicosMateriais> materiais)
        {
            float valorTotal = 0;
            if (materiais != null && materiais.Count > 0)
            {
                foreach (OrdensServicosMateriais material in materiais)
                {
                    valorTotal += material.PrecoTotal;
                }
            }
            return valorTotal;
        }

        public float CalcularValorTotalDeServicos(List<OrdensServicosServicos> servicos)
        {
            float valorTotal = 0;
            if (servicos != null && servicos.Count > 0)
            {
                foreach (OrdensServicosServicos servico in servicos)
                {
                    valorTotal += servico.Valor;
                }
            }
            return valorTotal;
        }
    }
}
