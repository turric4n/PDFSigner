using Moq;
using NUnit.Framework;
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
    public class CertificateTests
    {
        private string _pfxpath;
        private string _p12path;
        private const string _CERTPASS = "apples";
        private const string _CERTP12PASS = "test";
        [SetUp]
        public void Init()
        {
            _pfxpath = Path.Combine(TestContext.CurrentContext.TestDirectory, "test.pfx");
            _p12path = Path.Combine(TestContext.CurrentContext.TestDirectory, "test.p12");
        }
        [Test]
        public void Certificate_instance_should_be_returned_given_valid_parameters()
        {
            // Arrange
            var streammock = new Mock<Stream>().Object;

            var password = "test";

            // Act
            var certificate = new Certificate(streammock, password);

            // Assert
            Assert.NotNull(certificate);
        }

        [Test]
        public void Given_right_certificate_pfx_parameters_will_return_certficate_instance()
        {
            // Arrange
            var id = 1;
            var password = "1234";
            var path = "certificate.pfx";
            var business = "test";

            // Act
            var certificatedata = new CertificateData(id, password, path, business);

            // Assert
            Assert.NotNull(certificatedata);
        }

        [Test]
        public void Given_right_certificate_p12_parameters_will_return_certficate_instance()
        {
            // Arrange
            var id = 1;
            var password = "1234";
            var path = "certificate.p12";
            var business = "test";

            // Act
            var certificatedata = new CertificateData(id, password, path, business);

            // Assert
            Assert.NotNull(certificatedata);
        }

        [Test]
        public void Given_invalid_certificate_parameters_will_not_return_certficate_instance()
        {
            // Arrange
            var id = 0;
            var password = "1234";
            var path = "certificate.pfx";
            var business = "";

            // Act // Assert 
            Assert.Throws<ArgumentException>(() => { var cert = new CertificateData(id, password, path, business); });                       
        }

        [Test]
        public void Given_valid_certificate_pfx_parameters_will_init_and_return_right_certificate_property()
        {
            // Arrange
            var id = 1;
            var path = _pfxpath;
            var business = "test";

            var certdata = new CertificateData(id, _CERTPASS, path, business);

            using (var certificatestream = new FileStream(_pfxpath, FileMode.Open))
            {
                var certificate = new Certificate(certificatestream, certdata.Password);
                //Act
                Assert.DoesNotThrow(() =>
                {
                    certificate.Init();
                });

                //Assert
                Assert.IsNotEmpty(certificate.Chain);
                Assert.IsNotNull(certificate.Parameters);
            }
        }

        [Test]
        public void Given_valid_certificate_p12_parameters_will_init_and_return_right_certificate_property()
        {
            // Arrange
            var id = 1;
            var path = _p12path;
            var business = "test";

            var certdata = new CertificateData(id, _CERTP12PASS, path, business);

            using (var certificatestream = new FileStream(_p12path, FileMode.Open))
            {
                var certificate = new Certificate(certificatestream, certdata.Password);
                //Act
                Assert.DoesNotThrow(() =>
                {
                    certificate.Init();
                });

                //Assert
                Assert.IsNotEmpty(certificate.Chain);
                Assert.IsNotNull(certificate.Parameters);
            }
        }

        [Test]
        public void Given_invalid_certificate_password_PFX_will_not_return_certificate_property()
        {
            // Arrange
            var id = 1;
            var path = _pfxpath;
            var business = _CERTPASS;
            var password = "1234";

            var certdata = new CertificateData(id, password, path, business);

            using (var certificatestream = new FileStream(_pfxpath, FileMode.Open))
            {
                var certificate = new Certificate(certificatestream, certdata.Password);
                //Act //Assert
                Assert.Throws<System.IO.IOException>(() =>
                {
                    certificate.Init();
                });
            }
        }

        [Test]
        public void Given_invalid_certificate_password_P12_will_not_return_certificate_property()
        {
            // Arrange
            var id = 1;
            var path = _p12path;
            var business = _CERTPASS;
            var password = "1234";

            var certdata = new CertificateData(id, password, path, business);

            using (var certificatestream = new FileStream(_pfxpath, FileMode.Open))
            {
                var certificate = new Certificate(certificatestream, certdata.Password);
                //Act //Assert
                Assert.Throws<System.IO.IOException>(() =>
                {
                    certificate.Init();
                });
            }
        }
    }
}
