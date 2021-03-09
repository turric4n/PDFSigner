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
        [Option('i', "id", Required = false, HelpText = "Set business id")]
        public int Id { get; set; }        
        [Option('p', "pdfpath", Required = false, HelpText = "Set absolute pdfpath")]
        public string PdfPath { get; set; }
        [Option('s', "setup", Required = false, HelpText = "Setup flag, to configure certificate data")]
        public bool Setup { get; set; }
    }
}
