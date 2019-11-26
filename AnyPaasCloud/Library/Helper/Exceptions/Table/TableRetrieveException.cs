using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Helper.Exceptions
{
    public class TableRetrieveException : TableException
    {
        public TableRetrieveException(string message)
            : base(message)
        {

        }
    }
}
