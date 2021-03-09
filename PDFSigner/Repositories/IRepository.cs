using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFSign.Repositories
{
    public interface IRepository<Stream>
    {
        void Save(Stream stream);
        Stream Load(string path);
    }
}
