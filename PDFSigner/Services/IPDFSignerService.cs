using PDFSign.Models;
using System.IO;

namespace PDFSign.Services
{
    public interface IPdfSignerService
    {
        Stream SignPdf(Stream pdfStream, Certificate certificate);
        bool IsSigned(Stream pdfStream);
    }
}
