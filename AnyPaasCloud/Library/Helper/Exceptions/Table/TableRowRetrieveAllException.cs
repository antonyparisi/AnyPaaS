using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Helper.Exceptions
{
    public class TableRowRetrieveAllException : TableException
    {
        public TableRowRetrieveAllException(string message)
            : base(message)
        {

        }
    }
}
