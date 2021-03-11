using Moq;
using NUnit.Framework;
using PDFSign.Models;

namespace PDFSigner.Tests.Unit
{
    [TestFixture(Category = "Unit")]
    public class CertificateDataRepositoryTests
    {
        private ICertificateDataRepository _certificateDataRepository;
        [SetUp]
        public void SetUp()
        {
            _certificateDataRepository = new Mock<ICertificateDataRepository>().Object;
        }
    }
}