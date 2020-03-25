using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Web.Models;
using Web.Util;

namespace Web.Infra
{
    public class GeradorDePDF : Controller
    {
        private HttpResponseBase response;
        private HttpRequestBase request;
        private Font fonteSubtitulos = new Font(Font.FontFamily.TIMES_ROMAN, 13, Font.BOLD);
        private Font fonteNegrito = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD);
        private Font fonteNormal = new Font(Font.FontFamily.TIMES_ROMAN, 10);
        private float larguraBordas = 1.0f;

        public GeradorDePDF(HttpResponseBase response, HttpRequestBase request)
        {
            this.response = response;
            this.request = request;
        }

        public void GerarOrcamento(OrdensServicos ordem)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 80, 80);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, response.OutputStream);
            Image imagemLogo = Image.GetInstance(request.MapPath("~/Content/Imagens/logo.png"));
            Image imagemGmail = Image.GetInstance(request.MapPath("~/Content/Imagens/gmail.png"));
            Image imagemWhats = Image.GetInstance(request.MapPath("~/Content/Imagens/whats.png"));
            Image imagemMaps = Image.GetInstance(request.MapPath("~/Content/Imagens/maps.png"));
            pdfWriter.PageEvent = new ITextEvents() { ImagemLogo = imagemLogo, ImagemEmail = imagemGmail, ImagemWhats = imagemWhats, ImagemMaps = imagemMaps };
            pdfDoc.Open();

            PdfPCell celunaVazia;
            PdfPCell celunaConteudo;

            #region Cabeçalho
            PdfPTable tabela;
            PdfPCell celula;
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
            response.Buffer = true;
            response.ContentType = "application/pdf";
            String nomeArquivo = String.Format("EntreRodas_{0}_{1}", DateTime.Now.Year, ordem.CodigoOrdensServicos);
            response.AddHeader("content-disposition", String.Format("attachment;filename={0}.pdf", nomeArquivo));
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Write(pdfDoc);
            response.End();
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
    }
}