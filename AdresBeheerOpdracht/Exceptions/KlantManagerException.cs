using System;
using System.Collections.Generic;
using System.Text;

namespace AdresBeheerOpdracht.Exceptions
{
    public class KlantManagerException : Exception
    {
        public KlantManagerException(string message) : base(message)
        {
        }

        public KlantManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
