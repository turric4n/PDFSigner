using NUnit.Framework;
using PDFSign;
using PDFSign.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Assert = NUnit.Framework.Assert;

namespace PDFSigner.Tests.Unit
{
    [TestFixture(Category = "Unit")]
    public class PDFSignin
    {
        private string _pdfpath;
        private string _pfxpath;
        private string _p12path;
        private string _signedPdfPath;
        private const string _CERTPASS = "apples";
        private const string _CERTPASSP12 = "test";
        [SetUp]
        public void Init()
        {
            _pdfpath = Path.Combine(TestContext.CurrentContext.TestDirectory, "test.pdf");
            _pfxpath = Path.Combine(TestContext.CurrentContext.TestDirectory, "test.pfx");
            _p12path = Path.Combine(TestContext.CurrentContext.TestDirectory, "test.p12");
            _signedPdfPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "signed_test.pdf");
        }

        private void SavePdfFile(Stream stream, string path)
        {
            using (var newfile = new FileStream(path, FileMode.OpenOrCreate))
            {
                stream.CopyTo(newfile);
            }
        }

        private void DeleteOldSigned(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }
            File.Delete(path);
        }

        [Test]
        public void Given_valid_PDF_file_and_PFX_certificate_will_be_signed()
        {
            //Arrange
            using (var certificatestream = new FileStream(_pfxpath, FileMode.Open))
            {
                var certificate = new Certificate(certificatestream, _CERTPASS);
                certificate.Init();
                var signerservice = new PDFSignerService();
                using (var stream = new FileStream(_pdfpath, FileMode.Open))
                {
                    //Act //Assert
                    Assert.DoesNotThrow(() => { signerservice.SignPDF(stream, certificate); });
                }
            }            
        }

        [Test]
        public void Given_valid_PDF_file_and_P12_certificate_will_be_signed()
        {
            //Arrange
            using (var certificatestream = new FileStream(_p12path, FileMode.Open))
            {
                var certificate = new Certificate(certificatestream, _CERTPASSP12);
                certificate.Init();
                var signerservice = new PDFSignerService();
                using (var stream = new FileStream(_pdfpath, FileMode.Open))
                {
                    //Act //Assert
                    Assert.DoesNotThrow(() => { signerservice.SignPDF(stream, certificate); });
                }
            }
        }

        [Test]
        public void Given_valid_PDF_file_and_certificate_will_be_signed_and_exist()
        {
            //Arrange
            using (var certificatestream = new FileStream(_pfxpath, FileMode.Open))
            {
                var certificate = new Certificate(certificatestream, _CERTPASS);
                certificate.Init();
                var signerservice = new PDFSignerService();
                using (var stream = new FileStream(_pdfpath, FileMode.Open))
                {
                    //Act //Assert
                    Assert.DoesNotThrow(() => { signerservice.SignPDF(stream, certificate); });
                    Assert.IsTrue(File.Exists(_pdfpath));
                }
            }
        }

        [Test]
        public void Given_valid_PDF_file_and_certificate_will_be_signed_and_validated()
        {
            //Arrange
            using (var certificatestream = new FileStream(_pfxpath, FileMode.Open))
            {
                var certificate = new Certificate(certificatestream, _CERTPASS);
                certificate.Init();
                var signerservice = new PDFSignerService();
                using (var stream = new FileStream(_pdfpath, FileMode.Open))
                {
                    Stream signed = null;
                    //Act //Assert
                    Assert.DoesNotThrow(() => { signed = signerservice.SignPDF(stream, certificate); });
                    signerservice.IsSigned(signed);
                }
            }
        }

        [Test]
        public void Given_valid_PDF_file_and_certificate_will_be_signed_and_validated_2_times()
        {
            
        } 
    }
}
