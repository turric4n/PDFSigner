using System;
using System.IO;
using Conditions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using PDFSign.Models;

namespace PDFSign.Services
{
    public class PdfSignerService : IPdfSignerService
    {
        public Stream SignPdf(Stream pdfStream, Certificate certificate)
        {
            pdfStream.Requires(nameof(pdfStream)).IsNotNull();
            certificate.Requires(nameof(certificate)).IsNotNull();

            var chain = certificate.Chain;
            var parameters = certificate.Parameters;

            IExternalSignature pks = new PrivateKeySignature(parameters, DigestAlgorithms.SHA256);

            using (PdfReader reader = new PdfReader(pdfStream))
            {
                if (reader.IsEncrypted())
                {
                    throw new Exception("[PDFEncryptedException] Target PDF is encrypted or owned, unlock PDF and try again.");
                }
                var memoryStream = new MemoryStream();

                using (PdfStamper st = PdfStamper.CreateSignature(reader, memoryStream, '\0', "tmp.pdf", true))
                {
                    PdfSignatureAppearance appearance = st.SignatureAppearance;

                    appearance.SignDate = DateTime.Now;

                    appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.NAME_AND_DESCRIPTION;

                    MakeSignature.SignDetached(appearance, pks, chain, null, null, null, 0, CryptoStandard.CMS);

                    var mb = memoryStream.ToArray();
                    return new MemoryStream(mb);
                }
            }
        }

        public bool IsSigned(Stream pdfStream)
        {
            using (PdfReader reader = new PdfReader(pdfStream))
            {
                AcroFields acroFields = reader.AcroFields;
                var a = acroFields.Fields["Signature1"];
                return a != null;
            }
        }
    }
}


