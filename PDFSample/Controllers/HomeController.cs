using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using PDFSample.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace PDFSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult TestPDF()
        {
            //Dummy data for Invoice (Bill).
            int orderNo = 2303;
            string fileName = fileName = "Invoice_" + DateTime.Now.ToString("ddMMyyyyhhMMss") + ".pdf";

            string filePath = Server.MapPath("/TempPDF/");

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            string fullPath = Path.Combine(filePath, fileName);

            Image logo = Image.GetInstance(Server.MapPath("/Content/Images/Logo3.png"));
            //logo.ScaleAbsolute(500, 300);

            Document pdfDoc = new Document();

            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(fullPath, FileMode.Create));

            pdfDoc.Open();

            PdfPTable tbl1 = new PdfPTable(4);
            tbl1.SpacingAfter = 5f;

            tbl1.AddCell(GetCell(string.Empty, false, logo, colSpan: 1));
            tbl1.AddCell(GetCell("Bill To", false, addTopPadding: true, alignPosition: Element.ALIGN_RIGHT, useBoldFont: true, paddRight: 10));
            tbl1.AddCell(GetCell("Customer Name", false, addTopPadding: true, colSpan: 2, alignPosition: Element.ALIGN_RIGHT));

            tbl1.AddCell(GetCell("", colSpan: 2));
            tbl1.AddCell(GetCell("Tel: +60 18XXXXXX", colSpan: 2, addTopPadding: true, paddingTop: -6, alignPosition: Element.ALIGN_RIGHT));

            tbl1.AddCell(GetCell("", colSpan: 2));
            tbl1.AddCell(GetCell("Email: abc@abc.com", colSpan: 2, alignPosition: Element.ALIGN_RIGHT));

            pdfDoc.Add(tbl1);

            pdfDoc.Add(GetLine());

            PdfPTable subHeadingTable = new PdfPTable(4);
            subHeadingTable.SpacingAfter = 5f;

            subHeadingTable.AddCell(GetCell("Customer #", hasBorder: false, useBoldFont: true));
            subHeadingTable.AddCell(GetCell("1234", hasBorder: false));
            subHeadingTable.AddCell(GetCell("Invoice #", hasBorder: false, useBoldFont: true, alignPosition: Element.ALIGN_CENTER));
            subHeadingTable.AddCell(GetCell("1234", hasBorder: false));

            subHeadingTable.AddCell(GetCell("Method", hasBorder: false, useBoldFont: true));
            subHeadingTable.AddCell(GetCell("PayPal", hasBorder: false));
            subHeadingTable.AddCell(GetCell("Invoice Date", hasBorder: false, useBoldFont: true, alignPosition: Element.ALIGN_CENTER));
            subHeadingTable.AddCell(GetCell(DateTime.Now.ToShortDateString(), hasBorder: false));

            pdfDoc.Add(subHeadingTable);

            pdfDoc.Add(GetLine());

            PdfPTable invoiceSummary = new PdfPTable(4);
            invoiceSummary.SpacingBefore = 5f;
            invoiceSummary.SpacingAfter = 5f;

            invoiceSummary.AddCell(GetCell("Invoice Summary", hasBorder: !true, useBoldFont: true, borderTop: true, borderLeft: true, colSpan: 3));
            invoiceSummary.AddCell(GetCell("", hasBorder: !true, borderTop: true, borderLeft: false, borderRight: true, borderBottom: false));

            invoiceSummary.AddCell(GetCell("Subscription Annual 123", borderLeft: true, colSpan: 3, hasBorder: !true));
            invoiceSummary.AddCell(GetCell("160.00", borderRight: true, hasBorder: !true, alignPosition: Element.ALIGN_RIGHT));

            invoiceSummary.AddCell(GetCell("Taxes", borderLeft: true, colSpan: 3, hasBorder: !true));
            invoiceSummary.AddCell(GetCell("0.00", borderRight: true, hasBorder: !true, alignPosition: Element.ALIGN_RIGHT));

            invoiceSummary.AddCell(GetCell("Total", colSpan: 3, useBoldFont: true, addTopPadding: true, paddingTop: 20));
            invoiceSummary.AddCell(GetCell("MYR 160.00", colSpan: 1, alignPosition: Element.ALIGN_RIGHT, addTopPadding: true, paddingTop: 20));

            pdfDoc.Add(invoiceSummary);

            pdfDoc.Add(GetLine());

            pdfDoc.Close();

            return Content("OK");
        }

        private PdfPCell GetCell(string text, bool hasBorder = true, Image img = null, bool addTopPadding = false, int? alignPosition = null, bool useNormalFont = true, bool useBoldFont = false, int? colSpan = 0, bool borderTop = false, bool borderBottom = false, bool borderLeft = false, bool borderRight = false, int paddingTop = 0, int paddRight = 0)
        {
            PdfPCell pdfCell = null;

            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

            if (img != null)
            {
                pdfCell = new PdfPCell(img, true);
            }
            else
            {
                pdfCell = new PdfPCell(new Phrase(new Chunk(text, useBoldFont ? boldFont : normalFont)));
            }

            if (addTopPadding)
            {
                pdfCell.PaddingTop = 5f;

                if (paddingTop != 0)
                    pdfCell.PaddingTop = paddingTop;
            }

            if (paddRight != 0)
                pdfCell.PaddingRight = paddRight;

            if (colSpan != null)
            {
                pdfCell.Colspan = colSpan.GetValueOrDefault();
            }

            if (alignPosition != null)
                pdfCell.HorizontalAlignment = alignPosition.GetValueOrDefault();

            if (!hasBorder)
                pdfCell.Border = Rectangle.NO_BORDER;
            else
                pdfCell.UseVariableBorders = true;

            if (borderBottom)
                pdfCell.BorderColorBottom = BaseColor.BLACK;
            else
                pdfCell.BorderColorBottom = BaseColor.WHITE;

            if (borderLeft)
                pdfCell.BorderColorLeft = BaseColor.BLACK;
            else
                pdfCell.BorderColorLeft = BaseColor.WHITE;

            if (borderRight)
                pdfCell.BorderColorRight = BaseColor.BLACK;
            else
                pdfCell.BorderColorRight = BaseColor.WHITE;

            if (borderTop)
                pdfCell.BorderColorTop = BaseColor.BLACK;
            else
                pdfCell.BorderColorTop = BaseColor.WHITE;

            return pdfCell;
        }

        private LineSeparator GetLine()
        {
            return new LineSeparator(0.45f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
        }

        private LineSeparator GetBlankLine()
        {
            return new LineSeparator(1f, 100f, BaseColor.WHITE, Element.ALIGN_LEFT, 1);
        }
    }
}