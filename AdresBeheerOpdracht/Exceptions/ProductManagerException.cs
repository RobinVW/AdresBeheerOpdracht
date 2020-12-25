using System;
using System.Collections.Generic;
using System.Text;

namespace AdresBeheerOpdracht.Exceptions
{
    public class ProductManagerException : Exception
    {
        public ProductManagerException(string message) : base(message)
        {
        }

        public ProductManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
