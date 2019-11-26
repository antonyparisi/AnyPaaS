using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Helper.Exceptions
{
    public class ProviderNullException : Exception
    {
        public ProviderNullException(string message)
            :base(message)
        {

        }
    }
}
