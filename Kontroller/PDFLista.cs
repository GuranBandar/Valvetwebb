using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Drawing;
using Valvetwebb.Aktivitet;
using Valvetwebb.Objekt;
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
        private string PDFFileName { get; set; }

        public object PdfPTabletableLayout { get; set; }

        public object tableLayout { get; set; }

        public static Anvandare WebUser { get; set; }

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

        public void ExportToPdf()
        {
            string dest = HttpContext.Current.Server.MapPath("~/Files/Valvlista.pdf");
            GeneratePdf(dest);
        }

            //public static MemoryStream CreatePdf()
            //{
            //    List<ValvPost> valvpostList = GetData();

            //    FontStyle fontStyle = FontStyle.Italic;
            //    MemoryStream memoryStream = new MemoryStream();
            //    PdfWriter writer = new PdfWriter(memoryStream);
            //    PdfDocument pdf = new PdfDocument(writer);
            //PdfFont fontH = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);
            //PdfFont font = PdfFontFactory.CreateRegisteredFont("Verdana", PdfEncodings.CP1252);
            //float fontSize = 11f;
            //float fontSizeH = 12f;
            //Document document = new Document(pdf).SetFont(font).SetFontSize(fontSize);

            //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(@"C:\\Mina program\\Valvet\\" + "ValvetLista.pdf", FileMode.Create));
            //DataTable dataTable = GenereraDataTable(valvpostList);

            // Add header to the document
            //Paragraph header = new Paragraph("Valvetlista")
            //.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
            //.SetFontSize(fontSizeH);
            // New line
            //Paragraph newline = new Paragraph(new Text("\n"));

            //document.Add(newline);
            //document.Add(header);

            //iText.Layout.Font.FontSet fontH = new iText.Layout.Font.FontSet();

            //var header = new PDFFooter();
            //document.Open();
            //writer.PageEvent = header;
            //header.HeaderText = "Valvlista";
            //iTextSharp.text.Font fontH = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 7, 2);
            //iTextSharp.text.Font fontP = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 6);
            //iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 5);

            //fontH.Color = BaseColor.GRAY;

            //Table table = new Table(dataTable.Columns.Count);
            //table.HeaderRows = 1; /*---->> this property repeats the headers of an iTextSharp PdfPTable on each page */
            //PdfPRow row = null;
            //float[] widths = new float[dataTable.Columns.Count];
            //for (int i = 0; i < dataTable.Columns.Count; i++)
            //    widths[i] = 4f;

            //table.SetWidths(widths);
            //table.WidthPercentage = 100;
            //int iCol = 0;
            //string colname = "";
            //PdfPCell cell = new PdfPCell(new Phrase("Valvposter"));

            //cell.Colspan = dataTable.Columns.Count;

            //foreach (DataColumn c in dataTable.Columns)
            //{
            //    table.AddHeaderCell(new Cell().Add(new Paragraph(c.ColumnName)).SetFontSize(fontSize));
            //}

            //if (valvpostList.Count > 0)
            //{
            //    foreach (ValvPost valvpost in valvpostList)
            //    {
            //        table.AddCell(new Cell().Add(new Paragraph(valvpost.Postnamn.ToString())).SetFontSize(fontSize));
            //        table.AddCell(new Cell().Add(new Paragraph(valvpost.Usernamn.ToString())).SetFontSize(fontSize));
            //        table.AddCell(new Cell().Add(new Paragraph(valvpost.Losenord.ToString())).SetFontSize(fontSize));
            //        table.AddCell(new Cell().Add(new Paragraph(valvpost.Anteckningar.ToString())).SetFontSize(fontSize));
            //    }
            //}

            //document.Add(table);
            //document.Close();

            //return memoryStream;
            // Return the PDF file
            //return File(memoryStream.ToArray(), "application/pdf", $"Report.pdf");
            //}

            private static DataTable GenereraDataTable(List<ValvPost> dt)
        {
            var table = new DataTable();
            var columns = table.Columns;
            columns.Add("Postnamn", typeof(string));
            columns.Add("Usernamn", typeof(string));
            columns.Add("Losenord", typeof(string));
            //columns.Add("Anteckningar", typeof(string));
            return table;
        }

        private void GeneratePdf(string dest)
        {
            List<ValvPost> valvpostList = GetData();
            DataTable dt = GenereraDataTable(valvpostList);
            FontStyle fontStyle = FontStyle.Italic;
            float fontSize = 10f;
            float fontSizeH = 12f;
            PDFFileName = dest;
            FileInfo file = new FileInfo(dest);
            file.Directory.Create();

            //PdfDocument pdfDocument = new PdfDocument(new PdfWriter(filename));

            PdfWriter writer = new PdfWriter(dest); 
            PdfDocument pdfDocument = new PdfDocument(writer); 
            Document document = new Document(pdfDocument);

            document = new Document(pdfDocument, new PageSize(500, 825));
            document.SetMargins(0, 0, 0, 0);
            float[] columnWidthsman = { 9, 9, 9};
            Table tableman = new Table(UnitValue.CreatePercentArray(columnWidthsman));
            PdfFont fman = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont detail = PdfFontFactory.CreateFont(StandardFonts.COURIER);

            Cell cellman = new Cell(1, 3)
                        .Add(new Paragraph("Header"))
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
                        .SetFontSize(12)
                        .SetFontColor(DeviceGray.BLACK)
                        .SetBackgroundColor(new DeviceGray(0.75f))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            // Add cell 1.
            tableman.AddHeaderCell(cellman1);

            Cell cellman2 = new Cell(1, 1)
                        .Add(new Paragraph("Usernamn"))
                        .SetFont(fman)
                        .SetFontSize(12)
                        .SetFontColor(DeviceGray.BLACK)
                        .SetBackgroundColor(new DeviceGray(0.75f))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            // Add cell 2.
            tableman.AddHeaderCell(cellman2);

            Cell cellman3 = new Cell(1, 1)
                        .Add(new Paragraph("Losenord"))
                        .SetFont(fman)
                        .SetFontSize(12)
                        .SetFontColor(DeviceGray.BLACK)
                        .SetBackgroundColor(new DeviceGray(0.75f))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            // Add cell 3.
            tableman.AddHeaderCell(cellman3);

            //Cell cellman4 = new Cell(1, 1)
            //            .Add(new Paragraph("Anteckningar"))
            //            .SetFont(fman)
            //            .SetFontSize(13)
            //            .SetFontColor(DeviceGray.BLACK)
            //            .SetBackgroundColor(new DeviceGray(0.75f))
            //            .SetTextAlignment(TextAlignment.CENTER)
            //            .SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            //// Add cell 4.
            //tableman.AddHeaderCell(cellman4);

            // Populate table with report data
            foreach (var item in valvpostList)
            {
                tableman.AddCell(new Cell().Add(new Paragraph(item.Postnamn)).SetFont(detail).SetFontSize(fontSize));
                tableman.AddCell(new Cell().Add(new Paragraph(item.Usernamn.ToString())).SetFont(detail).SetFontSize(fontSize));
                tableman.AddCell(new Cell().Add(new Paragraph(item.Losenord.ToString())).SetFont(detail).SetFontSize(fontSize));
                //tableman.AddCell(new Cell().Add(new Paragraph(item.Anteckningar.ToString())));
            }

            // Add table to the document
            document.Add(tableman);

            // Close the document
            document.Close();
        }
    }
}