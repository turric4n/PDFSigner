using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFSign.Models
{
    public class ApplicationParameters
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
        [Option('i', "id", Required = true, HelpText = "Set business id")]
        public int Id { get; set; }        
        [Option('p', "pdfpath", Required = true, HelpText = "Set absolute pdfpath")]
        public string PdfPath { get; set; }
        [Option('s', "setup", Required = true, HelpText = "Setup flag, to configure certificate data")]
        public bool Setup { get; set; }
    }
}
