using System;
using System.Collections.Generic;
using System.Text;

namespace AdresBeheerOpdracht.Exceptions
{
    public class BestellingManagerException : Exception
    {
        public BestellingManagerException(string message) : base(message)
        {
        }

        public BestellingManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
