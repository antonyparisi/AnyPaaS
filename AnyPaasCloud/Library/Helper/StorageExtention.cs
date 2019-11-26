using Amazon;
using Amazon.S3;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.storagecsp.Library.Helper
{
    public static class StorageExtention
    {
        public static void CheckAzureClient(this CloudBlobClient i, CloudBlobClient clientAzure, string messageException)
        {
            if (clientAzure == null)
            {
                throw new NullReferenceException(messageException);
            }
        }
        public static void CheckAzureContainer(this CloudBlobContainer i, CloudBlobContainer containerAzure, string messageException)
        {
            if (containerAzure == null)
            {
                throw new NullReferenceException(messageException);
            }
        }
        public static void CheckAzureDirectory(this CloudBlobDirectory i, CloudBlobDirectory directoryAzure, string messageException)
        {
            if (directoryAzure == null)
            {
                throw new NullReferenceException(messageException);
            }
        }
        public static void CheckAmazonS3Client(this AmazonS3Client i, AmazonS3Client amazonS3Client, string messageException)
        {
            if (amazonS3Client == null)
            {
                throw new NullReferenceException(messageException);
            }
        }
        public static void CheckRegionEndpoint(this RegionEndpoint i, RegionEndpoint regionEndpoint, string messageException)
        {
            if (regionEndpoint == null)
            {
                throw new NullReferenceException(messageException);
            }
        }
    }
}
