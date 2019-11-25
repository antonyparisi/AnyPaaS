using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Helper.Exceptions.Storage
{
    public class StorageInsertException : StorageCloudException
    {
        public StorageInsertException(string message)
            : base(message)
        {

        }
    }
}
