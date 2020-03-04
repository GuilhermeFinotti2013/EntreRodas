using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Util
{
    public class ITextEvents : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;

        public Image ImagemLogo { get; set; }
        public Image ImagemWhats { get; set; }
        public Image ImagemEmail { get; set; }
        public Image ImagemMaps { get; set; }

        #region Fields
        private string _header;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
            }
            catch (System.IO.IOException ioe)
            {
            }
        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);
            iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font fonteNormal = new Font(Font.FontFamily.TIMES_ROMAN, 7, Font.BOLD);

            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(3);
            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 100;
            float[] larguras = new float[] { 2f, 0.5f, 7f };
            pdfTab.SetWidths(larguras);
            PdfPCell celulaIcone;
            PdfPCell celulaTexto;
            PdfPCell celulaLogo;

            celulaLogo = new PdfPCell();
            ImagemLogo.ScaleAbsolute(100, 50);
            celulaLogo.AddElement(ImagemLogo);
            celulaLogo.Border = 0;
            celulaLogo.Rowspan = 3;
            pdfTab.AddCell(celulaLogo);
            celulaIcone = new PdfPCell();
            celulaIcone.Border = 0;
            ImagemEmail.ScaleAbsolute(12, 12);
            celulaIcone.AddElement(ImagemEmail);
            pdfTab.AddCell(celulaIcone);
            celulaTexto = new PdfPCell(new Phrase("entrerodasautomotiva@gmail.com", fonteNormal));
            celulaTexto.Border = 0;
            pdfTab.AddCell(celulaTexto);

            celulaIcone = new PdfPCell();
            celulaIcone.Border = 0;
            ImagemWhats.ScaleAbsolute(12, 12);
            celulaIcone.AddElement(ImagemWhats);
            pdfTab.AddCell(celulaIcone);
            celulaTexto = new PdfPCell(new Phrase("(51) 00035-3739", fonteNormal));
            celulaTexto.Border = 0;
            pdfTab.AddCell(celulaTexto);

            celulaIcone = new PdfPCell();
            celulaIcone.Border = 0;
            ImagemMaps.ScaleAbsolute(12, 12);
            celulaIcone.AddElement(ImagemMaps);
            pdfTab.AddCell(celulaIcone);
            celulaTexto = new PdfPCell(new Phrase("Rua Hoefel Sander, 165, Fazenda São Borja - São Leopoldo - RS | CEP 93044-830", fonteNormal));
            celulaTexto.Border = 0;
            pdfTab.AddCell(celulaTexto);

            //Add paging to footer
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.SetTextMatrix(document.PageSize.GetLeft(190), document.PageSize.GetBottom(30));
                cb.ShowText("Entre Rodas Assistência Técnica Automotiva");
                cb.EndText();
                float len = bf.GetWidthPoint("text", 12);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(80) + len, document.PageSize.GetBottom(30));
            }
            pdfTab.WriteSelectedRows(0, -1, 110, document.PageSize.Height - 10, writer.DirectContent);
            //Move the pointer and draw line to separate header section from rest of page
            cb.MoveTo(40, document.PageSize.Height - 70);
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 70);
            cb.Stroke();
            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            cb.Stroke();
        }
    }
}