using System;

namespace PDFSign.Exceptions
{
    public class RepositoryNotFoundException : Exception
    {
        public RepositoryNotFoundException()
        {
        }

        public RepositoryNotFoundException(string message) : base(message)
        {
        }

        public RepositoryNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
