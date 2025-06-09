using System.Collections.Generic;

namespace PDFSign.Models
{
    public interface IRepository<T>
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        T GetByName(string name);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
