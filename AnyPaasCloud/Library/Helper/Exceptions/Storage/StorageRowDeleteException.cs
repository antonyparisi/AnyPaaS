using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Helper.Exceptions.Storage
{
    public class StorageRowDeleteException : StorageCloudException
    {
        public StorageRowDeleteException(string message)
            : base(message)
        {

        }
    }
}
