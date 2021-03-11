using PDFSign.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFSign.Services
{
    public interface IPDFSignerService
    {
        Stream SignPDF(Stream pdfstream, Certificate certificate);
        bool IsSigned(Stream pdfstream);
    }
}
