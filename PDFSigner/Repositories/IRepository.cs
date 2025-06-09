namespace PDFSign.Repositories
{
    public interface IRepository<TStream>
    {
        void Save(TStream stream);
        TStream Load(string path);
    }
}
