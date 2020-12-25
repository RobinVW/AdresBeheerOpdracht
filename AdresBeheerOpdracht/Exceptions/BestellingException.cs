using System;
using System.Collections.Generic;
using System.Text;

namespace AdresBeheerOpdracht.Exceptions
{
    public class BestellingException : Exception
    {
        public BestellingException(string message) : base(message)
        {
        }

        public BestellingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
