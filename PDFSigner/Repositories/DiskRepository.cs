using Conditions;
using System.IO;

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
