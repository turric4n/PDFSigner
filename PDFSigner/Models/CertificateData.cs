using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conditions;

namespace PDFSign.Models
{
    public class CertificateData
    {
        public long Id { get; private set; }
        public string BusinessName { get; private set; }
        public string Password { get; private set; }
        public string Path { get; private set; }

        public CertificateData()
        {
        }

        public CertificateData(long id, string password, string path, string businessName)
        {
            SetCertificatePassWord(password);
            SetPath(path);
            SetBusinessName(businessName);
        }

        public void SetCertificatePassWord(string password)
        {
            password.Requires(password)
                .IsNotEmpty();
            Password = password;
        }

        public void SetPath(string path)
        {
            path.Requires(path)
                .IsNotEmpty();
            Path = path;
        }

        public void SetBusinessName(string businessname)
        {
            businessname.Requires(businessname)
                .IsNotEmpty();
            BusinessName = businessname;
        }
    }
}
