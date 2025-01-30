using System;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Valvetwebb.Kontroller
{
    public class PDFFooter : PdfPageEventHelper
    //public class PDFFooter
    {
        public string HeaderText { get; set; }


        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.SpacingAfter = 10F;
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            cell = new PdfPCell(new Phrase("Valvlista"));
            cell.Border = Rectangle.NO_BORDER;
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, 150, document.Top, writer.DirectContent);
        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            //float cellHeight = document.TopMargin;
            //Rectangle page = document.PageSize;
            //PdfPTable table = new PdfPTable(1) { TotalWidth = page.Width };
            //table.AddCell(new PdfPCell(new Phrase(HeaderText))
            //{
            //    Border = PdfPCell.NO_BORDER,
            //    FixedHeight = cellHeight,
            //    HorizontalAlignment = Element.ALIGN_CENTER
            //});
            //table.WriteSelectedRows(
            //    0, -1, 0,
            //    page.Height - cellHeight + table.TotalHeight,
            //    writer.DirectContent
            //);
            DateTime horario = DateTime.Now;
            base.OnEndPage(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            cell = new PdfPCell(new Phrase("TEST" + " - " + horario));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, 150, document.Bottom, writer.DirectContent);
        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }
    }
}