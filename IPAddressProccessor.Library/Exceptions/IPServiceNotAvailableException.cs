using System;
using System.Collections.Generic;
using System.Text;

namespace IPAddressProccessor.Library.Exceptions
{
    public class IPServiceNotAvailableException: Exception
    {
        public IPServiceNotAvailableException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public IPServiceNotAvailableException(string message)
            : base(message)
        {

        }
    }
}
