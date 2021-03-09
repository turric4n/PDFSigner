using NUnit.Framework;
using PDFSign;
using PDFSign.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFSigner.Tests.Unit
{
    [TestFixture(Category = "Unit")]
    public class PDFSignin
    {
        [Test]
        public void Given_valid_PDF_file_and_certificate_will_be_signed()
        {
            //Arrange
            using (var certificatestream = new FileStream("cert.pfx", FileMode.Open))
            {
                var certificate = new Certificate(certificatestream, "");
                var signerservice = new PDFSignerService();
                using (var stream = new FileStream("test.pdf", FileMode.Open))
                {
                    //Act //Assert
                    Assert.DoesNotThrow(() => { signerservice.SignPDF(stream, certificate); });
                }
            }            
        }
    }
}
