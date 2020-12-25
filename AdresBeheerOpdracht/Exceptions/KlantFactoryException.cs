using System;
using System.Collections.Generic;
using System.Text;

namespace AdresBeheerOpdracht.Exceptions
{
    public class KlantFactoryException : Exception
    {
        public KlantFactoryException(string message) : base(message)
        {
        }

        public KlantFactoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
