using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using PDFSign.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFSign.Models;
using Conditions;

namespace PDFSign
{
    public class PDFSignerService : IPDFSignerService
    {
        public Stream SignPDF(Stream pdfstream, Certificate certificate)
        {
            pdfstream.Requires(nameof(pdfstream)).IsNotNull();
            certificate.Requires(nameof(certificate)).IsNotNull();

            var chain = certificate.Chain;
            var parameters = certificate.Parameters;

            IExternalSignature pks = new PrivateKeySignature(parameters, DigestAlgorithms.SHA256);

            using (PdfReader reader = new PdfReader(pdfstream))
            {
                if (reader.IsEncrypted())
                {
                    throw new Exception("[PDFEncryptedException] Target PDF is encrypted or owned, unlock PDF and try again.");
                }
                var outputpdf = new MemoryStream();
                using (PdfStamper st = PdfStamper.CreateSignature(reader, outputpdf, '\0', "tmp.pdf", true))
                {
                    PdfSignatureAppearance appearance = st.SignatureAppearance;

                    appearance.SignDate = DateTime.Now;

                    appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.NAME_AND_DESCRIPTION;

                    MakeSignature.SignDetached(appearance, pks, chain, null, null, null, 0, CryptoStandard.CMS);

                    var mb = outputpdf.ToArray();
                    return new MemoryStream(mb);
                }
            }
        }

        public bool IsSigned(Stream pdfstream)
        {
            using (PdfReader reader = new PdfReader(pdfstream))
            {
                AcroFields acroFields = reader.AcroFields;
                var a = acroFields.Fields["Signature1"];
                return a != null;
            }
        }
    }
}


