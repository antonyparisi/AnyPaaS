using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Model
{
    public class TableCloud
    {
        public List<string> JSON { get; set; }

        public TableCloud()
        {
            JSON = new List<string>();
        }
        
    }
}
