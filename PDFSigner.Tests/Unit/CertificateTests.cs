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
        private const string _CERTPASS = "apples";
        [SetUp]
        public void Init()
        {
            _pfxpath = Path.Combine(TestContext.CurrentContext.TestDirectory, "test.pfx");
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
        public void Given_right_certificate_parameters_will_return_certficate_instance()
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
        public void Given_valid_certificate_parameters_will_init_and_return_right_certificate_property()
        {
            // Arrange
            var id = 1;
            var path = _pfxpath;
            var business = _CERTPASS;

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
        public void Given_invalid_certificate_password_will_not_return_certificate_property()
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
    }
}
