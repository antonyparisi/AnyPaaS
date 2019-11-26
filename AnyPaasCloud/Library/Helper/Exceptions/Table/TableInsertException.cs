using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Helper.Exceptions
{
    public class TableInsertException : TableException
    {
        public TableInsertException(string message)
            : base(message)
        {

        }
    }
}