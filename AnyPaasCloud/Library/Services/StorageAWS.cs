using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Cdc.storagecsp.Library.Helper;
using LibreriaStorageTest.Library.Helper;
using LibreriaStorageTest.Library.Helper.Exceptions.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Services
{
    public class StorageAWS : IStorageCloud
    {
        AmazonS3Client AmazonS3Client { get; set; }

        public StorageAWS(IConfigurationRoot configuration)
        {
            //Connection string da configurazione
            //var accessKeyId = configuration["AWSService:AccessKeyId"];
            //var secretAccessKey = configuration["AWSService:SecretAccessKey"];
            //
            AmazonS3Client = new AmazonS3Client(null, null, RegionEndpoint.EUWest1);
        }



        public async Task<bool> CreateIfNotExistAsync(string bucketName)
        {
            try
            {
                if (!(await AmazonS3Util.DoesS3BucketExistV2Async(AmazonS3Client, bucketName)))
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };

                    PutBucketResponse putBucketResponse = await AmazonS3Client.PutBucketAsync(putBucketRequest);
                }

                // Retrieve the bucket location.
                //string bucketLocation = await FindBucketLocationAsync(s3Client);

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }




            return true;
        }

        public async Task<string> InsertBlobMessage(string containerName, string id, string message)
        {
            try
            {
                //preparo la richiesta di put attingendo dalla property
                var putObjectRequest = new PutObjectRequest()
                {
                    BucketName = containerName,
                    Key = id
                };

                byte[] byteArray = Encoding.ASCII.GetBytes(message);

                Stream stream = new MemoryStream(byteArray);

                //aggiungo il file nella request di put
                putObjectRequest.InputStream = stream;

                //lancio la put
                var putObjectResponse = await AmazonS3Client.PutObjectAsync(putObjectRequest);

                return putObjectResponse.ETag;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GetBlobMessage(string containerName, string id)
        {

            //preparo il memory stream da valorizzare
            var memoryStream = new MemoryStream();

            //preparo la richiesta di get attingendo dalla property
            var getObjectRequest = new GetObjectRequest()
            {
                BucketName = containerName,
                Key = id
            };

            //inizializzo il token necessario per l'estrazione delle righe del cancellationToken
            var cancellationToken = new CancellationToken();

            //estraggo il file all'interno dell'S3 e ritorno l'oggetto
            var getObjectResponse = await AmazonS3Client.GetObjectAsync(getObjectRequest, cancellationToken);
          

            StreamReader reader = new StreamReader(getObjectResponse.ResponseStream);

            return reader.ReadToEnd();
        }

        public async Task<bool> DeleteBlobMessage(string containerName, string id)
        {
            try
            {
                //preparo la richiesta di delete attingendo dalla property
                var deleteObjectRequest = new DeleteObjectRequest()
                {
                    BucketName = containerName,
                    Key = id
                };
                //lancio la delete
                var gggg = await AmazonS3Client.DeleteObjectAsync(deleteObjectRequest);

                return true;

            }
            catch (Exception)
            {
                return false; 
            }
           
        }
    }
}