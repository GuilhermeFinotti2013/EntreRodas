using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Web.Util;
using Web.Infra;
using System.Net.Mail;
using System.IO;

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

            VisualizarServicoViewModel model = ConfigurVisualizacao(ordensServicos, null);
            return View(model);
        }

        private VisualizarServicoViewModel ConfigurVisualizacao(OrdensServicos ordensServicos, List<String> errors)
        {
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
                model.DataInicialPrevista = ordensServicos.DataInicialPrevista.Value.ToString("dd/MM/yyyy");
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
                if (ordensServicos.FormaPagamento.Trim() == "DI")
                {
                    model.FormaPagamento = "Dinheiro";
                }
                else if (ordensServicos.FormaPagamento.Trim() == "CQ")
                {
                    model.FormaPagamento = "Cheque";
                }
                else if (ordensServicos.FormaPagamento.Trim() == "CA")
                {
                    model.FormaPagamento = "Cartão de crédito";
                }
                else if (ordensServicos.FormaPagamento.Trim() == "TD")
                {
                    model.FormaPagamento = "Todos";
                }
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
            model.InformacoesAdicionais = ordensServicos.InformacoesAdicionais;
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
            #region Erros
            if (errors != null)
            {
                if (errors.Count > 0)
                {
                    model.Erros = errors;
                }
            }
            #endregion
            return model;
        }

        // GET: OrdensServicos/Details/5
        public ActionResult GerarOrcamento(int? id)
        {
            OrdensServicos ordensServicos = db.OrdensServicos.Find(id);
            if (ordensServicos == null)
            {
                return HttpNotFound();
            }
            #region Validações
            List<String> errors = new List<string>();
            if (ordensServicos.ProblemaIdentificado == null)
            {
                errors.Add("Para gerar o orçamento ao cliente é preciso, primeiro, informar o problema identificado!");
            }
            if (ordensServicos.DataInicialPrevista == null)
            {
                errors.Add("Para gerar o orçamento ao cliente é preciso, primeiro, informar a data prevista para o início do trabalho!");
            }
            if (ordensServicos.OrdensServicosServicos == null || ordensServicos.OrdensServicosServicos.Count == 0 ||
                ordensServicos.OrdensServicosMateriais == null || ordensServicos.OrdensServicosMateriais.Count == 0)
            {
                errors.Add("Para gerar o orçamento ao cliente é preciso ter, ao menos, um serviço e um materia associado à ele!");
            }
            #endregion
            if (errors.Count == 0)
            {
                GeradorDePDF geradorDePDF = new GeradorDePDF(Response, Request);
                geradorDePDF.GerarOrcamentoDownload(ordensServicos);
            }
            VisualizarServicoViewModel model = ConfigurVisualizacao(ordensServicos, errors);
            return View("Details", model);
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
            EditarInformacoesDoServicoViewModel editarInformacoes = new EditarInformacoesDoServicoViewModel();
            if (ordensServicos.InformacoesAdicionais != null)
            {
                editarInformacoes.InformacoesAdicionais = ordensServicos.InformacoesAdicionais.Trim();
            }
            editarInformacoes.OrdensServicosId = ordensServicos.Id;
            if (ordensServicos.ProblemaIdentificado != null)
            {
                editarInformacoes.ProblemaIdentificado = ordensServicos.ProblemaIdentificado.Trim();
            }
            return View(editarInformacoes);
        }
        // POST: OrdensServicos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrdensServicosId,ProblemaIdentificado,InformacoesAdicionais")] EditarInformacoesDoServicoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                OrdensServicos ordensServicos = db.OrdensServicos.Find(viewModel.OrdensServicosId);
                if (ordensServicos == null)
                {
                    return HttpNotFound();
                }
                ordensServicos.ProblemaIdentificado = viewModel.ProblemaIdentificado;
                ordensServicos.InformacoesAdicionais = viewModel.InformacoesAdicionais;
                db.Entry(ordensServicos).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        sb.Append(error.ErrorMessage + "\n");
                    }
                }
                TempData["Errors"] = sb.ToString();
            }
            return RedirectToAction("Details", "OrdensServicos", new { id = viewModel.OrdensServicosId });
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

        // GET: OrdensServicos/Edit/5
        public ActionResult EntregarServico(int? id)
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
            FinalizarServicoViewModel viewModel = new FinalizarServicoViewModel();
            viewModel.OrdensServicosId = ordensServicos.Id;
            viewModel.NomeCliente = ordensServicos.Clientes.Nome;
            viewModel.ModeloVeiculo = String.Format("{0} {1} Ano {2}", ordensServicos.Veiculos.MarcasCarros.Nome.Trim(),
                ordensServicos.Veiculos.Modelo.Trim(), ordensServicos.Veiculos.Ano);
            viewModel.SubTotalMateriais = String.Format("R${0}", this.CalcularValorTotalDeMateriais(ordensServicos.OrdensServicosMateriais.ToList()));
            viewModel.SubTotalServicos = String.Format("R${0}", this.CalcularValorTotalDeServicos(ordensServicos.OrdensServicosServicos.ToList()));
            viewModel.ValorTotal = String.Format("R$ {0}", ordensServicos.ValorTotal);
            viewModel.Materiais = ordensServicos.OrdensServicosMateriais.ToList();
            viewModel.Servicos = ordensServicos.OrdensServicosServicos.ToList();
            CombosGenericos combos = new CombosGenericos();
            ViewBag.FormaPagamento = new SelectList(combos.ListarFormasPagamento(), "Valor", "Texto");
            return View("FinalizaServico", viewModel);
        }

        // POST: OrdensServicos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarFormaPagamento([Bind(Include = "OrdensServicosId,FormaPagamento,ValorAPagar,FormaPagamento,ValorDinheiro,ValorCartao,InformacoesAdicionais")] FinalizarServicoViewModel editarForma)
        {
            if (ModelState.IsValid)
            {
                OrdensServicos ordensServicos = db.OrdensServicos.Find(editarForma.OrdensServicosId);
                if (ordensServicos == null)
                {
                    return HttpNotFound();
                }
                ordensServicos.FormaPagamento = editarForma.FormaPagamento;
                ordensServicos.ValorAPagar = editarForma.ValorAPagar;
                if (editarForma.FormaPagamento == "AM")
                {
                    ordensServicos.ValorDinheiro = editarForma.ValorDinheiro;
                }
                else if (editarForma.FormaPagamento == "DI")
                {
                    ordensServicos.ValorDinheiro = editarForma.ValorDinheiro;
                }
                else
                {
                    ordensServicos.ValorCartao = editarForma.ValorCartao;
                }
                ordensServicos.InformacoesAdicionais = editarForma.InformacoesAdicionais;
                db.Entry(ordensServicos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "OrdensServicos", new { id = editarForma.OrdensServicosId });
            }
            return View(editarForma);
        }

        // GET: OrdensServicos/AgendarInicio/5
        public ActionResult AgendarInicio(int? id)
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
            AgendarServicoViewModel viewModel = new AgendarServicoViewModel();
            viewModel.OrdensServicosId = ordensServicos.Id;
            return View(viewModel);
        }

        // POST: OrdensServicos/AgendarInicio/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgendarInicio([Bind(Include = "DataInicial,OrdensServicosId")] AgendarServicoViewModel viewModel)
        {
            OrdensServicos ordensServicos = db.OrdensServicos.Find(viewModel.OrdensServicosId);
            List<String> errors = new List<string>();
            if (ordensServicos == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                ordensServicos.DataInicialPrevista = viewModel.DataInicial;
                db.Entry(ordensServicos).State = EntityState.Modified;
                db.SaveChanges();
                ordensServicos = db.OrdensServicos.Find(viewModel.OrdensServicosId);
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
            }
            VisualizarServicoViewModel model = ConfigurVisualizacao(ordensServicos, errors);
            return View("Details", model);
        }

        // GET: OrdensServicos/Details/5
        public ActionResult EnviarOrcamento(int? id)
        {
            OrdensServicos ordensServicos = db.OrdensServicos.Find(id);
            if (ordensServicos == null)
            {
                return HttpNotFound();
            }
            #region Validações

            List<String> errors = this.ValidarEnviarOrcamento(ordensServicos);
            #endregion
            if (errors.Count == 0)
            {
                ordensServicos.Status = "OE";
                db.Entry(ordensServicos).State = EntityState.Modified;
                db.SaveChanges();
                ordensServicos = db.OrdensServicos.Find(id);
                String caminhoArquivo = String.Format("{0}\\EntreRodas_{1}_{2}.pdf", Directory.GetCurrentDirectory(),
                DateTime.Now.Year, ordensServicos.CodigoOrdensServicos.Trim());
                GeradorDePDF geradorDePDF = new GeradorDePDF(Response, Request);
                geradorDePDF.GerarOrcamentoPasta(ordensServicos, caminhoArquivo);

                Email email = new Email();
                email.EnviarOrcamento(ordensServicos.Clientes.Email.Trim(), caminhoArquivo, ordensServicos.Clientes.Nome, ordensServicos.Veiculos.MarcasCarros.Nome,
                    ordensServicos.Veiculos.Modelo, ordensServicos.Veiculos.Ano);
            }
            VisualizarServicoViewModel model = ConfigurVisualizacao(ordensServicos, errors);
            return View("Details", model);
        }

        // GET: OrdensServicos/Details/5
        public ActionResult ReenviarOrcamento(int? id)
        {
            OrdensServicos ordensServicos = db.OrdensServicos.Find(id);
            if (ordensServicos == null)
            {
                return HttpNotFound();
            }
            List<String> errors = this.ValidarEnviarOrcamento(ordensServicos);
            if (errors.Count == 0)
            {
                String caminhoArquivo = String.Format("{0}\\EntreRodas_{1}_{2}.pdf", Directory.GetCurrentDirectory(),
                DateTime.Now.Year, ordensServicos.CodigoOrdensServicos.Trim());
                GeradorDePDF geradorDePDF = new GeradorDePDF(Response, Request);
                geradorDePDF.GerarOrcamentoPasta(ordensServicos, caminhoArquivo);

                Email email = new Email();
                email.EnviarOrcamento(ordensServicos.Clientes.Email.Trim(), caminhoArquivo, ordensServicos.Clientes.Nome, ordensServicos.Veiculos.MarcasCarros.Nome,
                    ordensServicos.Veiculos.Modelo, ordensServicos.Veiculos.Ano);
            }
            VisualizarServicoViewModel model = ConfigurVisualizacao(ordensServicos, errors);
            return View("Details", model);
        }

        // GET: OrdensServicos/Details/5
        public ActionResult IniciaServico(int? id)
        {
            OrdensServicos ordensServicos = db.OrdensServicos.Find(id);
            if (ordensServicos == null)
            {
                return HttpNotFound();
            }
            ordensServicos.Status = "EE";
            ordensServicos.DataInicial = DateTime.Now;
            db.Entry(ordensServicos).State = EntityState.Modified;
            db.SaveChanges();

            VisualizarServicoViewModel model = ConfigurVisualizacao(ordensServicos, null);
            return View("Details", model);
        }

        // GET: OrdensServicos/Details/5
        public ActionResult AguardoPecas(int? id)
        {
            OrdensServicos ordensServicos = db.OrdensServicos.Find(id);
            if (ordensServicos == null)
            {
                return HttpNotFound();
            }
            ordensServicos.Status = "AP";
            db.Entry(ordensServicos).State = EntityState.Modified;
            db.SaveChanges();

            VisualizarServicoViewModel model = ConfigurVisualizacao(ordensServicos, null);
            return View("Details", model);
        }

        // GET: OrdensServicos/Details/5
        public ActionResult CancelarServico(int? id)
        {
            OrdensServicos ordensServicos = db.OrdensServicos.Find(id);
            if (ordensServicos == null)
            {
                return HttpNotFound();
            }
            ordensServicos.Status = "SC";
            db.Entry(ordensServicos).State = EntityState.Modified;
            db.SaveChanges();

            VisualizarServicoViewModel model = ConfigurVisualizacao(ordensServicos, null);
            return View("Details", model);
        }

        // GET: OrdensServicos/Details/5
        public ActionResult ChegouPeca(int? id)
        {
            OrdensServicos ordensServicos = db.OrdensServicos.Find(id);
            if (ordensServicos == null)
            {
                return HttpNotFound();
            }
            ordensServicos.Status = "EE";
            db.Entry(ordensServicos).State = EntityState.Modified;
            db.SaveChanges();

            VisualizarServicoViewModel model = ConfigurVisualizacao(ordensServicos, null);
            return View("Details", model);
        }

        // GET: OrdensServicos/Details/5
        public ActionResult FinalizaServico(int? id)
        {
            OrdensServicos ordensServicos = db.OrdensServicos.Find(id);
            if (ordensServicos == null)
            {
                return HttpNotFound();
            }
            ordensServicos.ValorTotal = this.CalcularValorTotal(ordensServicos);
            ordensServicos.Status = "PT";
            db.Entry(ordensServicos).State = EntityState.Modified;
            db.SaveChanges();

            VisualizarServicoViewModel model = ConfigurVisualizacao(ordensServicos, null);
            return View("Details", model);
        }

        #region Métodos auxiliares
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
                    int contServicosMes = Convert.ToInt32(ultimaOrdem.CodigoOrdensServicos.Substring(6, 4));
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
        #endregion

        #region Métodos de validacao
        private List<String> ValidarEnviarOrcamento(OrdensServicos ordensServicos)
        {
            List<String> errors = new List<string>();
            if (ordensServicos.ProblemaIdentificado == null)
            {
                errors.Add("Para o envio do orçamento ao cliente é preciso, primeiro, informar o problema identificado!");
            }
            if (ordensServicos.DataInicialPrevista == null)
            {
                errors.Add("Para o envio do orçamento ao cliente é preciso, primeiro, informar a data prevista para o início do trabalho!");
            }
            if (ordensServicos.OrdensServicosServicos == null || ordensServicos.OrdensServicosServicos.Count == 0 ||
                ordensServicos.OrdensServicosMateriais == null || ordensServicos.OrdensServicosMateriais.Count == 0)
            {
                errors.Add("Para o envio do orçamento ao cliente é preciso ter, ao menos, um serviço e um materia associado à ele!");
            }
            return errors;
        }
        #endregion

    }
}
