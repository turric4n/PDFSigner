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
                //Activate MultiSignatures

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
    }
}


