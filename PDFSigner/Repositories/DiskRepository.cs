using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conditions;

namespace PDFSign.Repositories
{
    public class DiskRepository : IRepository<Stream>
    {
        public Stream Load(string path)
        {
            path.Requires(nameof(path))
                .IsNotEmpty()
                .Ensures(Path.GetFullPath(path));
            return new FileStream(path, FileMode.Open);
        }

        public void Delete(string path)
        {
            path.Requires(nameof(path))
                .IsNotEmpty()
                .Ensures(Path.GetFullPath(path));
            File.Delete(path);
        }
       
        public void Save(Stream stream)
        {
            //stream.
        }
    }
}
