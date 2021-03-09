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

            // Act
            var certificatedata = new CertificateData(id, password, path, "");

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

            // Act // Assert 
            Assert.Throws<ArgumentException>(() => { var cert = new CertificateData(id, password, path, ""); });                       
        }
    }
}
