using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Helper.Exceptions
{
    public class TableException : Exception
    {
        public TableException(string message)
            : base(message)
        {

        }
    }
}
