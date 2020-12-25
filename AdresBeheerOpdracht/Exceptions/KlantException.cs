using System;
using System.Collections.Generic;
using System.Text;

namespace AdresBeheerOpdracht.Exceptions
{
    public class KlantException : Exception
    {
        public KlantException(string message) : base(message)
        {
        }

        public KlantException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
