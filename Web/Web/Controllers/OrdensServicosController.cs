using iTextSharp.text;
using iTextSharp.text.pdf;
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

namespace Web.Controllers
{
    //    [Authorize(Roles = "Administradores")]
    public class OrdensServicosController : Controller
    {
        private entre_rodasEntities db = new entre_rodasEntities();
        private Font fonteSubtitulos = new Font(Font.FontFamily.TIMES_ROMAN, 13, Font.BOLD);
        private Font fonteNegrito = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD);
        private Font fonteNormal = new Font(Font.FontFamily.TIMES_ROMAN, 10);
        private float larguraBordas = 1.0f;

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
        public ActionResult EnviarOrcamento(int? id)
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
            #endregion
            if (errors.Count == 0)
            {
                ordensServicos.Status = "OE";
                db.Entry(ordensServicos).State = EntityState.Modified;
                db.SaveChanges();
                GerarOrcamentoPdf(id);
                ordensServicos = db.OrdensServicos.Find(id);

            }
            VisualizarServicoViewModel model = ConfigurVisualizacao(ordensServicos, errors);
            return View("Details", model);
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
                GerarOrcamentoPdf(id);
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
        public ActionResult EditarFormaPagamento(int? id)
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
            EditarFormaPagamentoViewModel viewModel = new EditarFormaPagamentoViewModel();
            viewModel.OrdensServicosId = ordensServicos.Id;
            CombosGenericos combos = new CombosGenericos();
            ViewBag.FormaPagamento = new SelectList(combos.ListarFormasPagamento(), "Valor", "Texto", ordensServicos.FormaPagamento.Trim());
            return View(viewModel);
        }

        // POST: OrdensServicos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarFormaPagamento([Bind(Include = "FormaPagamento,OrdensServicosId")] EditarFormaPagamentoViewModel editarForma)
        {
            if (ModelState.IsValid)
            {
                OrdensServicos ordensServicos = db.OrdensServicos.Find(editarForma.OrdensServicosId);
                if (ordensServicos == null)
                {
                    return HttpNotFound();
                }
                ordensServicos.FormaPagamento = editarForma.FormaPagamento;
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

        #region Gerar PDF
        
        //GET: 4
        public void GerarOrcamentoPdf(int? id)
        {
            OrdensServicos ordem = db.OrdensServicos.Find(id);

            Document pdfDoc = new Document(PageSize.A4, 10, 10, 80, 80);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            Image imagemLogo = Image.GetInstance(Request.MapPath("~/Content/Imagens/logo.png"));
            Image imagemGmail = Image.GetInstance(Request.MapPath("~/Content/Imagens/gmail.png"));
            Image imagemWhats = Image.GetInstance(Request.MapPath("~/Content/Imagens/whats.png"));
            Image imagemMaps = Image.GetInstance(Request.MapPath("~/Content/Imagens/maps.png"));
            pdfWriter.PageEvent = new ITextEvents() { ImagemLogo = imagemLogo, ImagemEmail = imagemGmail, ImagemWhats = imagemWhats, ImagemMaps = imagemMaps };
            pdfDoc.Open();




            PdfPCell celunaVazia;
            PdfPCell celunaConteudo;

            #region Cabeçalho
            PdfPTable tabela;
            PdfPCell celula;
            /* = new PdfPTable(1);
            tabela.WidthPercentage = 100;
            tabela.HorizontalAlignment = 0;
            tabela.SpacingAfter = 7f;
            PdfPCell celula = new PdfPCell(new Phrase("Entre Rodas - Assistência técnica automotiva", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD)));
            celula.Border = 0;
            tabela.AddCell(celula);
            pdfDoc.Add(tabela);*/
            //Table
            tabela = new PdfPTable(1);
            tabela.WidthPercentage = 100;
            tabela.SpacingBefore = 5f;
            //Cell
            celula = new PdfPCell(new Phrase(String.Format("ORÇAMENTO {0}", ordem.CodigoOrdensServicos), new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.BOLD)));
            celula.FixedHeight = 30.0f;
            celula.Border = 0;
            celula.BackgroundColor = new BaseColor(221, 221, 221);
            celula.VerticalAlignment = Element.ALIGN_TOP;
            tabela.AddCell(celula);
            pdfDoc.Add(tabela);

            #endregion
            #region Informações da ordem de serviço
            pdfDoc.Add(this.MontarLinhaSubtitulo("INFORMAÇÕES DA ORDEM DE SERVIÇOS", 0, true));
            tabela = new PdfPTable(4);
            tabela.WidthPercentage = 100;
            float[] larguras = new float[] { 6f, 44f, 10f, 20f };
            tabela.SetWidths(larguras);
            celula = new PdfPCell(new Phrase("Cliente:", fonteNegrito));
            celula.Border = 0;
            celula.BorderWidthLeft = larguraBordas;
            tabela.AddCell(celula);
            celula = new PdfPCell(new Phrase(ordem.Clientes.Nome, fonteNormal));
            celula.Border = 0;
            tabela.AddCell(celula);
            pdfDoc.Add(tabela);
            celula = new PdfPCell(new Phrase("Modelo/Ano:", fonteNegrito));
            celula.Border = 0;
            tabela.AddCell(celula);
            celula = new PdfPCell(new Phrase(String.Format("{0} {1}", ordem.Veiculos.Modelo, ordem.Veiculos.Ano), fonteNormal));
            celula.Border = 0;
            celula.BorderWidthRight = larguraBordas;
            tabela.AddCell(celula);
            pdfDoc.Add(tabela);

            tabela = new PdfPTable(4);
            tabela.WidthPercentage = 100;
            larguras = new float[] { 16f, 30f, 14f, 20 };
            tabela.SetWidths(larguras);
            celula = new PdfPCell(new Phrase("Funcionário responsável:", fonteNegrito));
            celula.Border = 0;
            celula.BorderWidthLeft = larguraBordas;
            tabela.AddCell(celula);
            celula = new PdfPCell(new Phrase("Alan", fonteNormal));
            celula.Border = 0;
            tabela.AddCell(celula);
            pdfDoc.Add(tabela);
            celula = new PdfPCell(new Phrase("Data do orçamento:", fonteNegrito));
            celula.Border = 0;
            tabela.AddCell(celula);
            celula = new PdfPCell(new Phrase(ordem.DataInicialPrevista.Value.ToString("dd/MM/yyyy"), fonteNormal));
            celula.Border = 0;
            celula.BorderWidthRight = 1.0f;
            tabela.AddCell(celula);
            pdfDoc.Add(tabela);
            tabela = new PdfPTable(1);
            tabela.WidthPercentage = 100;
            celula = new PdfPCell(new Phrase("Problema identificado junto ao cliente:", fonteNegrito));
            celula.Border = 0;
            celula.BorderWidthLeft = larguraBordas;
            celula.BorderWidthRight = larguraBordas;
            tabela.AddCell(celula);
            pdfDoc.Add(tabela);
            tabela = new PdfPTable(1);
            tabela.WidthPercentage = 100;
            celula = new PdfPCell(new Phrase(ordem.ProblemaIdentificado, new Font(Font.FontFamily.TIMES_ROMAN, 12)));
            celula.Border = 0;
            celula.BorderWidthBottom = larguraBordas;
            celula.BorderWidthLeft = larguraBordas;
            celula.BorderWidthRight = larguraBordas;
            celula.PaddingBottom = 8f;
            tabela.AddCell(celula);
            pdfDoc.Add(tabela);
            #endregion
            #region Serviços            
            pdfDoc.Add(this.MontarLinhaSubtitulo("SERVIÇOS", 390.00, true));
            pdfDoc.Add(this.MontarCabecalhoTabelaDeItens(TipoItens.Servicos));
            tabela = new PdfPTable(4);
            tabela.WidthPercentage = 100;
            larguras = new float[] { 1f, 75f, 10f, 1f };
            tabela.SetWidths(larguras);
            //Percorre a lista de serviços
            for (int i = 0; i < ordem.OrdensServicosServicos.Count; i++)
            {
                celunaVazia = new PdfPCell();
                celunaVazia.FixedHeight = 15.0f;
                celunaVazia.Border = 0;
                celunaVazia.BorderWidthLeft = larguraBordas;
                if (i + 1 == ordem.OrdensServicosServicos.Count)
                {
                    celunaVazia.BorderWidthBottom = larguraBordas;
                }
                tabela.AddCell(celunaVazia);
                celunaConteudo = new PdfPCell(new Phrase(ordem.OrdensServicosServicos.ToList()[i].Descricao, fonteNormal));
                celunaConteudo.FixedHeight = 15.0f;
                celunaConteudo.Border = 0;
                celunaConteudo.BorderWidthBottom = larguraBordas;
                tabela.AddCell(celunaConteudo);
                celunaConteudo = new PdfPCell(new Phrase(String.Format("R${0}", ordem.OrdensServicosServicos.ToList()[i].Valor), fonteNormal));
                celunaConteudo.FixedHeight = 15.0f;
                celunaConteudo.Border = 0;

                celunaConteudo.HorizontalAlignment = Element.ALIGN_RIGHT;
                celunaConteudo.BorderWidthBottom = larguraBordas;
                tabela.AddCell(celunaConteudo);
                celunaVazia = new PdfPCell();
                celunaVazia.FixedHeight = 15.0f;
                celunaVazia.Border = 0;
                celunaVazia.BorderWidthRight = larguraBordas;
                if (i + 1 == ordem.OrdensServicosServicos.Count)
                {
                    celunaVazia.BorderWidthBottom = larguraBordas;
                }
                tabela.AddCell(celunaVazia);
            }
            pdfDoc.Add(tabela);
            #endregion
            #region Materiais
            pdfDoc.Add(this.MontarLinhaSubtitulo("MATERIAIS", 900.12, true));
            pdfDoc.Add(this.MontarCabecalhoTabelaDeItens(TipoItens.Materiais));
            tabela = new PdfPTable(6);
            tabela.WidthPercentage = 100;
            larguras = new float[] { 1f, 45f, 15f, 15f, 10f, 1f };
            tabela.SetWidths(larguras);
            //Percorre a lista de serviços
            for (int i = 0; i < ordem.OrdensServicosMateriais.Count; i++)
            {
                celunaVazia = new PdfPCell();
                celunaVazia.FixedHeight = 15.0f;
                celunaVazia.Border = 0;
                celunaVazia.BorderWidthLeft = larguraBordas;
                if (i + 1 == ordem.OrdensServicosMateriais.Count)
                {
                    celunaVazia.BorderWidthBottom = larguraBordas;
                }
                tabela.AddCell(celunaVazia);
                celunaConteudo = new PdfPCell(new Phrase(ordem.OrdensServicosMateriais.ToList()[i].Descricao, fonteNormal));
                celunaConteudo.FixedHeight = 15.0f;
                celunaConteudo.Border = 0;
                celunaConteudo.BorderWidthBottom = larguraBordas;
                tabela.AddCell(celunaConteudo);
                celunaConteudo = new PdfPCell(new Phrase(ordem.OrdensServicosMateriais.ToList()[i].Quantidade.ToString(), fonteNormal));
                celunaConteudo.FixedHeight = 15.0f;
                celunaConteudo.Border = 0;
                celunaConteudo.BorderWidthBottom = larguraBordas;
                tabela.AddCell(celunaConteudo);
                celunaConteudo = new PdfPCell(new Phrase(String.Format("R${0}", ordem.OrdensServicosMateriais.ToList()[i].PrecoUnitario), fonteNormal));
                celunaConteudo.FixedHeight = 15.0f;
                celunaConteudo.Border = 0;
                celunaConteudo.BorderWidthBottom = larguraBordas;
                tabela.AddCell(celunaConteudo);
                celunaConteudo = new PdfPCell(new Phrase(String.Format("R${0}", ordem.OrdensServicosMateriais.ToList()[i].PrecoTotal), fonteNormal));
                celunaConteudo.FixedHeight = 15.0f;
                celunaConteudo.Border = 0;
                celunaConteudo.HorizontalAlignment = Element.ALIGN_RIGHT;
                celunaConteudo.BorderWidthBottom = larguraBordas;
                tabela.AddCell(celunaConteudo);
                celunaVazia = new PdfPCell();
                celunaVazia.FixedHeight = 15.0f;
                celunaVazia.Border = 0;
                celunaVazia.BorderWidthRight = larguraBordas;
                if (i + 1 == ordem.OrdensServicosMateriais.Count)
                {
                    celunaVazia.BorderWidthBottom = larguraBordas;
                }
                tabela.AddCell(celunaVazia);
            }
            pdfDoc.Add(tabela);
            #endregion
            pdfDoc.Add(this.MontarLinhaSubtitulo("Total", 1290.12, false));
            #region Informações adicionais
            tabela = new PdfPTable(1);
            tabela.WidthPercentage = 100;
            celula = new PdfPCell(new Phrase("Informações adicionais", fonteNegrito));
            celula.Border = 0;
            tabela.AddCell(celula);
            pdfDoc.Add(tabela);
            tabela = new PdfPTable(1);
            tabela.WidthPercentage = 100;
            celula = new PdfPCell(new Phrase(ordem.InformacoesAdicionais, new Font(Font.FontFamily.TIMES_ROMAN, 12)));
            celula.Border = 0;
            tabela.AddCell(celula);
            pdfDoc.Add(tabela);
            #endregion
            tabela = new PdfPTable(1);
            tabela.WidthPercentage = 100;
            tabela.SpacingBefore = 10f;
            celula = new PdfPCell(new Phrase(String.Format("São Leopoldo, {0}", DateTime.Now.ToString("dd/MM/yyyy"), fonteNormal)));
            celula.Border = 0;
            celula.HorizontalAlignment = Element.ALIGN_CENTER;
            tabela.AddCell(celula);
            pdfDoc.Add(tabela);
            tabela = new PdfPTable(1);
            tabela.WidthPercentage = 100;
            tabela.SpacingBefore = 10f;
            celula = new PdfPCell(new Phrase("Entre Rodas Assistência Técnica Automotiva", fonteNegrito));
            celula.Border = 0;
            celula.HorizontalAlignment = Element.ALIGN_CENTER;
            tabela.AddCell(celula);
            pdfDoc.Add(tabela);
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            String nomeArquivo = String.Format("EntreRodas_{0}_{1}", DateTime.Now.Year, ordem.CodigoOrdensServicos);
            Response.AddHeader("content-disposition", String.Format("attachment;filename={0}.pdf", nomeArquivo));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();

        }

        private PdfPTable MontarLinhaSubtitulo(String titulo, Double valorTotal, bool ehTabela)
        {
            PdfPTable tabela = new PdfPTable(2);
            tabela.WidthPercentage = 100;
            float[] larguras = new float[] { 65f, 20f };
            tabela.SetWidths(larguras);
            tabela.SpacingBefore = 5f;
            PdfPCell celula = new PdfPCell(new Phrase(titulo, fonteSubtitulos));
            celula.FixedHeight = 20.0f;
            celula.Border = 0;
            if (ehTabela)
            {
                celula.BorderWidthTop = larguraBordas;
                celula.BorderWidthLeft = larguraBordas;
            }
            celula.BackgroundColor = new BaseColor(242, 242, 242);
            celula.VerticalAlignment = Element.ALIGN_TOP;
            tabela.AddCell(celula);
            if (valorTotal == 0)
            {
                celula = new PdfPCell();
            }
            else
            {
                if (ehTabela)
                {
                    celula = new PdfPCell(new Phrase(String.Format("Subtotal: R${0}", valorTotal), fonteNegrito));
                }
                else
                {
                    celula = new PdfPCell(new Phrase(String.Format("R${0}", valorTotal), fonteNegrito));
                }
            }
            celula.FixedHeight = 20.0f;
            celula.Border = 0;
            if (ehTabela)
            {
                celula.BorderWidthTop = larguraBordas;
                celula.BorderWidthRight = larguraBordas;
            }
            celula.BackgroundColor = new BaseColor(242, 242, 242);
            tabela.AddCell(celula);
            return tabela;
        }

        private PdfPTable MontarCabecalhoTabelaDeItens(TipoItens tipo)
        {
            PdfPTable tabela = new PdfPTable(6);
            tabela.WidthPercentage = 100;
            float[] larguras = new float[] { 1f, 45f, 15f, 15f, 10f, 1f };
            tabela.SetWidths(larguras);
            PdfPCell celula = new PdfPCell();
            celula.FixedHeight = 15.0f;
            celula.Border = 0;
            celula.BorderWidthLeft = larguraBordas;
            tabela.AddCell(celula);
            celula = new PdfPCell(new Phrase("Descrição", fonteNegrito));
            celula.FixedHeight = 15.0f;
            celula.Border = 0;
            tabela.AddCell(celula);
            if (tipo == TipoItens.Materiais)
            {
                celula = new PdfPCell(new Phrase("Preço unitário", fonteNegrito));
            }
            else
            {
                celula = new PdfPCell();
            }
            celula.FixedHeight = 15.0f;
            celula.Border = 0;
            tabela.AddCell(celula);
            if (tipo == TipoItens.Materiais)
            {
                celula = new PdfPCell(new Phrase("Quantidade", fonteNegrito));
            }
            else
            {
                celula = new PdfPCell();
            }
            celula.FixedHeight = 15.0f;
            celula.Border = 0;
            tabela.AddCell(celula);
            celula = new PdfPCell(new Phrase("Valor", fonteNegrito));
            celula.FixedHeight = 15.0f;
            celula.Border = 0;
            celula.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabela.AddCell(celula);
            celula = new PdfPCell();
            celula.FixedHeight = 15.0f;
            celula.Border = 0;
            celula.BorderWidthRight = larguraBordas;
            tabela.AddCell(celula);

            return tabela;
        }

        #endregion
    }
}
