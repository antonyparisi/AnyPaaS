using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Helper.Exceptions.Storage
{
    public class StorageRowRetrieveException : StorageCloudException
    {
        public StorageRowRetrieveException(string message)
            : base(message)
        {

        }
    }
}
