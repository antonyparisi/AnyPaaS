using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Helper.Exceptions
{
    public class TableRowRetrieveException : TableException
    {
        public TableRowRetrieveException(string message)
            : base(message)
        {

        }
    }
}