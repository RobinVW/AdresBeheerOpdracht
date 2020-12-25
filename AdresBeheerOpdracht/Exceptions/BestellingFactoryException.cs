using System;
using System.Collections.Generic;
using System.Text;

namespace AdresBeheerOpdracht.Exceptions
{
    public class BestellingFactoryException : Exception
    {
        public BestellingFactoryException(string message) : base(message)
        {
        }

        public BestellingFactoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
