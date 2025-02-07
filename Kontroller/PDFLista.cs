using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Drawing;
using Valvetwebb.Aktivitet;
using Valvetwebb.Objekt;
using iText.Bouncycastle;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using System.Web;

namespace Valvetwebb.Kontroller
{
    public class PDFLista
    {
        private string PdfFileName { get; set; }

        public object PdfPTabletableLayout { get; set; }

        public object tableLayout { get; set; }

        public static Anvandare WebUser { get; set; }

        protected PageSize pageSize { get; set; }

        PDFFooter pDFFooter = null;

        public PDFLista()
        {

        }

        /// <summary>
        /// Exportera en pdf 
        /// </summary>
        public string ExportToPdf()
        {
            string pdfFilename = "Valvlista.pdf";
            string fileName = GetPdfFilename(pdfFilename);
            GeneratePdf(fileName);
            return fileName;
        }

        /// <summary>
        /// Hämta data från databasen
        /// </summary>
        /// <returns>Lsta med valvposter</returns>
        private List<ValvPost> GetData()
        {
            List<ValvPost> valvpostList = null;
            ValvPostAktivitet ValvpostAktivitet = new ValvPostAktivitet();
            valvpostList = ValvpostAktivitet.SökValvPost(WebUser.Konto, string.Empty);

            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("PostID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Postnummer", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Postnamn", typeof(string)));
            int postnummer = 0;

            if (valvpostList.Count > 0)
            {
                foreach (ValvPost valvpost in valvpostList)
                {
                    postnummer++;
                    dr = dt.NewRow();
                    dr[0] = valvpost.PostID;
                    dr[1] = postnummer;
                    dr[2] = valvpost.Postnamn;
                    dt.Rows.Add(dr);
                }
            }
            else
            {
            }

            DataView dv = new DataView(dt);
            return valvpostList;
        }

        /// <summary>
        /// Generera en DataTable från objektlista
        /// </summary>
        /// <param name="dt">Objektlista</param>
        /// <returns></returns>
        private static DataTable GenereraDataTable(List<ValvPost> dt)
        {
            var table = new DataTable();
            var columns = table.Columns;
            columns.Add("Postnamn", typeof(string));
            columns.Add("Usernamn", typeof(string));
            columns.Add("Losenord", typeof(string));
            return table;
        }

        /// <summary>
        /// Genererar en PDF
        /// </summary>
        /// <param name="dest">Filenamn för pdf:en</param>
        private void GeneratePdf(string dest)
        {
            List<ValvPost> valvpostList = GetData();
            DataTable dt = GenereraDataTable(valvpostList);
            FontStyle fontStyle = FontStyle.Italic;
            float fontSize = 8f;
            float fontSizeH = 12f;
            PdfFileName = dest;
            FileInfo file = new FileInfo(dest);
            file.Directory.Create();

            PdfWriter writer = new PdfWriter(dest); 
            PdfDocument pdfDocument = new PdfDocument(writer); 
            Document document = new Document(pdfDocument);
            pageSize = new PageSize(500, 800);

            document = new Document(pdfDocument, pageSize, false);
            document.SetMargins(12, 12, 36, 12);
            float[] columnWidthsman = { 9, 9, 9};

            Table tableman = new Table(UnitValue.CreatePercentArray(columnWidthsman));
            PdfFont fman = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont detail = PdfFontFactory.CreateFont(StandardFonts.COURIER);

            pDFFooter = new PDFFooter();

            Cell cellman = new Cell(1, 3)
                        .Add(new Paragraph($"Datum: { DateTime.Now.ToString("yyyy-MM-dd") } | Valvlista"))
                        .SetFont(fman)
                        .SetFontSize(12)
                        .SetFontColor(DeviceGray.WHITE)
                        .SetBackgroundColor(DeviceGray.BLACK)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            // Add Header cell.
            tableman.AddHeaderCell(cellman);

            Cell cellman1 = new Cell(1, 1)
                        .Add(new Paragraph("Postnamn"))
                        .SetFont(fman)
                        .SetFontSize(fontSize)
                        .SetFontColor(DeviceGray.BLACK)
                        .SetBackgroundColor(new DeviceGray(0.75f))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            // Add cell 1.
            tableman.AddHeaderCell(cellman1);

            Cell cellman2 = new Cell(1, 1)
                        .Add(new Paragraph("Usernamn"))
                        .SetFont(fman)
                        .SetFontSize(fontSize)
                        .SetFontColor(DeviceGray.BLACK)
                        .SetBackgroundColor(new DeviceGray(0.75f))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            // Add cell 2.
            tableman.AddHeaderCell(cellman2);

            Cell cellman3 = new Cell(1, 1)
                        .Add(new Paragraph("Losenord"))
                        .SetFont(fman)
                        .SetFontSize(fontSize)
                        .SetFontColor(DeviceGray.BLACK)
                        .SetBackgroundColor(new DeviceGray(0.75f))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            // Add cell 3.
            tableman.AddHeaderCell(cellman3);

            // Populate table with report data
            foreach (var item in valvpostList)
            {
                tableman.AddCell(new Cell().Add(new Paragraph(item.Postnamn)).SetFont(detail).SetFontSize(fontSize));
                tableman.AddCell(new Cell().Add(new Paragraph(item.Usernamn.ToString())).SetFont(detail).SetFontSize(fontSize));
                tableman.AddCell(new Cell().Add(new Paragraph(item.Losenord.ToString())).SetFont(detail).SetFontSize(fontSize));
            }

            // Add table to the document
            document.Add(tableman);

            AddPageNumbers(document);

            // Close the document
            document.Close();

        }

        /// <summary>
        /// Hämta alla sidor och visa i footer
        /// </summary>
        /// <param name="doc">Documentet som ska genereras</param>
        private void AddPageNumbers(Document doc)
        {
            var totalPages = doc.GetPdfDocument().GetNumberOfPages();
            float coordX = ((pageSize.GetLeft() + doc.GetLeftMargin())
                  + (pageSize.GetRight() - doc.GetRightMargin())) / 2;
            float footerY = doc.GetBottomMargin();

            for (int i = 1; i <= totalPages; i++)
            {
                //Paragraph date = createDate();
                //date.SetFixedPosition(i, 15, 25, 600);
                //doc.Add(date);
                Paragraph pagenumber = createPageNumber(i, totalPages);
                pagenumber.SetFixedPosition(i, 225, 10, 600);
                doc.Add(pagenumber);
            }
        }

        /// <summary>
        /// Genererar objekt med sidnummer
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="totalPages"></param>
        /// <returns>Objekt med sidnummer</returns>
        private Paragraph createPageNumber(int pageNumber, int totalPages)
        {
            PdfFont footer = PdfFontFactory.CreateFont(StandardFonts.COURIER);
            Paragraph p = new Paragraph();
            p.SetFontSize(8);
            p.SetFont(footer);
            p.SetFontColor(DeviceGray.BLACK);
            p.Add("Sida ").Add(pageNumber.ToString()).Add(" av ").Add(totalPages.ToString());
            return p;
        }

        /// <summary>
        /// Generera filnamn för listan
        /// </summary>
        /// <param name="fileName">Det genererade filnamnet</param>
        /// <returns></returns>
        private string GetPdfFilename(string fileName)
        {
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", fileName);
            return filePath;
        }
    }
}