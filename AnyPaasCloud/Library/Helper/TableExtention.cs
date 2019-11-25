using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.storagecsp.Library.Helper
{
    public static class TableExtention
    {
        public static void CheckAzureTable(this CloudTable i, CloudTable cloudTable, string messageException)
        {
            if (cloudTable == null)
            {
                throw new NullReferenceException(messageException);
            }
        }
        public static void CheckAWSTable(this Table i, Table cloudTable, string messageException)
        {
            if (cloudTable == null)
            {
                throw new NullReferenceException(messageException);
            }
        }
    }
}
