using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace demo_generating_pdf_from_html.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpGet]
        [Route("ConvertHTMLtoPDF")]
        public async Task<FileResult> ConvertHTMLtoPDF([FromQuery] string html)
        {
            var diskLocation = System.IO.File.ReadAllText(@"D:\documento.html"); 
            var codeLocation = System.IO.File.ReadAllText("documento.html"); 
            var stream = await convertHTMLtoPDF(codeLocation);
            //var stream = await convertHTMLtoPDF(html);
            return File(stream, MediaTypeNames.Application.Pdf, $"{Guid.NewGuid().ToString()}.pdf");
        }

        private void createPdf(string html, string dest) => HtmlConverter.ConvertToPdf(html, new FileStream(dest, FileMode.Create));

        private async Task<MemoryStream> convertHTMLtoPDF(string html)
        {
            MemoryStream ms = new MemoryStream();
            using (PdfWriter writer = new PdfWriter(ms))
            using (PdfDocument pdfDocument = new PdfDocument(writer))
            using (Document document = HtmlConverter.ConvertToDocument(html, pdfDocument, new ConverterProperties()))
            {
                writer.SetCloseStream(false);
            }

            ms.Position = 0;
            await ms.FlushAsync();
            return ms;
        }

    }
}
