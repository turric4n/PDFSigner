using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFSign.Models
{
    public interface IRepository<T>
    {
        T GetById(int Id);
        IEnumerable<T> GetAll();
        T GetByName(string Name);
        void Add(T Entity);
        void Delete(T Entity);
        void Update(T Entity);
    }
}
