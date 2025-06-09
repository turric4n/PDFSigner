using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using System.Collections;
using System.IO;

namespace PDFSign.Models
{
    public class Certificate
    {
        private readonly Stream _stream;
        private readonly string _password;
        public Org.BouncyCastle.X509.X509Certificate[] Chain { get; private set;  }
        public RsaPrivateCrtKeyParameters Parameters { get; private set; }

        public Certificate(Stream stream, string password)
        {
            _stream = stream;
            _password = password;            
        }
        
        public void Init()
        {
            string alias = null;

            var pk12 = new Pkcs12Store(_stream, _password.ToCharArray());

            IEnumerator i = pk12.Aliases.GetEnumerator();

            while (i.MoveNext())
            {
                alias = ((string)i.Current);
                if (pk12.IsKeyEntry(alias))
                    break;
            }

            var akp = pk12.GetKey(alias);
            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            Chain = new Org.BouncyCastle.X509.X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
                Chain[k] = ce[k].Certificate;


            Parameters = akp.Key as RsaPrivateCrtKeyParameters;
        }
    }
}
