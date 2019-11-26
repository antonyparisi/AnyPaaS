using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Helper.Exceptions
{
    public class TableRowDeleteException : TableException
    {
        public TableRowDeleteException(string message)
            : base(message)
        {

        }
    }
}