using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cdc.storagecsp.Library.Helper;
using LibreriaStorageTest.Library.Helper;
using LibreriaStorageTest.Library.Helper.Exceptions.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LibreriaStorageTest.Library.Services
{
    public class StorageAzure : IStorageCloud
    {
        //CloudBlobClient CloudBlobClient { set;  get; }
        //CloudBlobContainer CloudBlobContainer { set; get; }
        //CloudBlobDirectory CloudBlobDirectory { set; get; }

        CloudStorageAccount StorageAccount { get; set; }

        public StorageAzure(IConfigurationRoot configuration)
        {
            //Connection string da configurazione
            var connectionString = configuration["AzureStorageAccount:ConnectionString"];
            // StorageAccount
            CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccountCloud);
            //Riferimento allo Storageaccount
            StorageAccount = storageAccountCloud;
        }

        public async Task<bool> CreateIfNotExistAsync(string containerName)
        {

            try
            {
                //Create Reference to Azure Blob
                CloudBlobClient blobClient = StorageAccount.CreateCloudBlobClient();

                //The next 2 lines create if not exists a container named "democontainer"
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                //Crea il container
                await container.CreateIfNotExistsAsync();

                //risultato
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> InsertBlobMessage(string containerName, string id, string message)
        {
            try
            {
                //Create Reference to Azure Blob
                CloudBlobClient blobClient = StorageAccount.CreateCloudBlobClient();

                //The next 2 lines create if not exists a container named "democontainer"
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                var blockBlob = container.GetBlockBlobReference(id);

                using (Stream data = new MemoryStream(Encoding.ASCII.GetBytes(message)))
                {
                    await blockBlob.UploadFromStreamAsync(data);
                }

                return blockBlob.Uri.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> GetBlobMessage(string containerName, string id)
        {
            try
            {

                Stream downloadStream = new MemoryStream();
                
                //Create Reference to Azure Blob
                CloudBlobClient blobClient = StorageAccount.CreateCloudBlobClient();

                //The next 2 lines create if not exists a container named "democontainer"
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                var blockBlob = container.GetBlockBlobReference(id);

                await blockBlob.DownloadToStreamAsync(downloadStream);
                downloadStream.Position = 0;

                StreamReader reader = new StreamReader(downloadStream);

                return reader.ReadToEnd();
               
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteBlobMessage(string containerName, string id)
        {
            try
            {

                Stream downloadStream = new MemoryStream();

                //Create Reference to Azure Blob
                CloudBlobClient blobClient = StorageAccount.CreateCloudBlobClient();

                //The next 2 lines create if not exists a container named "democontainer"
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                var blockBlob = container.GetBlockBlobReference(id);

                await blockBlob.DeleteIfExistsAsync();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }


        //public ResponseMessage GetContainer(string connectionString, string containerName)
        //{
        //    try
        //    {
        //        //controllo se la table name è null o stringa vuota
        //        connectionString.CheckString(connectionString.Trim(), "Connection String is null or empty");

        //        //controllo se la connection string è null o stringa vuota
        //        containerName.CheckString(containerName.Trim(), "Container Name is null or empty");

        //        //Connessione al repository
        //        CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccount);

        //        //Istanzio il Client
        //        CloudBlobClient = storageAccount.CreateCloudBlobClient();

        //        //Recupero il riferimento del Container
        //        CloudBlobContainer = CloudBlobClient.GetContainerReference($"{containerName}");

        //        return new ResponseMessage("Container found correctly.");
        //    }
        //    catch (StorageException storageException)
        //    {
        //        Console.WriteLine(storageException.ToString());
        //        throw new StorageCloudException(storageException.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        throw new StorageCloudException(ex.Message);
        //    }
        //}

        //public ResponseMessage GetDirectory(string directoryPath)
        //{
        //    try
        //    {
        //        //controllo se la table name è null o stringa vuota
        //        directoryPath.CheckString(directoryPath.Trim(), "Directory Path is null or empty");

        //        //controlli su CloudBlobClient se esiste
        //        CloudBlobClient.CheckAzureClient(CloudBlobClient, "Client vuoto. Errore");

        //        //controlli su CloudBlobContainer
        //        CloudBlobContainer.CheckAzureContainer(CloudBlobContainer, "Cloud Blob Container vuoto. Errore");

        //        //Riferimento alla directory/folder
        //        CloudBlobDirectory = CloudBlobContainer.GetDirectoryReference(directoryPath);

        //        return new ResponseMessage("Blob found correctly.");
        //    }
        //    catch (StorageException storageException)
        //    {
        //        Console.WriteLine(storageException.ToString());
        //        throw new StorageCloudException(storageException.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        throw new StorageCloudException(ex.Message);
        //    }
        //}

        //public async Task<string> GetItemAsync(string pathFile)
        //{
            
        //    try
        //    {
        //        //controlli su CloudBlobClient se esiste
        //        CloudBlobClient.CheckAzureClient(CloudBlobClient, "Client vuoto. Errore");

        //        //controlli su CloudBlobContainer
        //        CloudBlobContainer.CheckAzureContainer(CloudBlobContainer, "Cloud Blob Container vuoto. Errore");

        //        //controlli su CloudBlobDirectory
        //        CloudBlobDirectory.CheckAzureDirectory(CloudBlobDirectory, "Cloud Blob Directory vuoto. Errore");

        //        //controllo se la table name è null o stringa vuota
        //        pathFile.CheckString(pathFile.Trim(), "Path File is null or empty");

        //        //inizializzo l'oggetto string di ritorno per il file da convertire in base64
        //        string base64File = "";

        //        //preparo il memory stream da valorizzare
        //        MemoryStream blobResults;

        //        //inizializzo il token necessario per l'estrazione delle righe del blob
        //        var blobContinuationToken = new BlobContinuationToken();

        //        //estraggo i files all'interno del blob e li ritono come delle liste
        //        var blobFiles = await CloudBlobDirectory.ListBlobsSegmentedAsync(blobContinuationToken);
        //        var listBlob = blobFiles.Results.ToList();

        //        //se la lista di Blob è null chiamo un'eccezione
        //        if(listBlob.Count == 0)
        //        {
        //            throw new StorageRowRetrieveException("Can't retrieve the file.");
        //        }
                
        //        //itero la lista di blob che ritorna
        //        foreach (var blobItem in listBlob)
        //        {
        //            //estraggo il riferimento al blob
        //            var blobFile = CloudBlobDirectory.GetBlockBlobReference(pathFile);

        //            //Istanzio lo stream di appoggio
        //            blobResults = new MemoryStream();

        //            //Recupero lo stream ed il metodo lo mappa direttamente nella variabile per il memoryStream
        //            await blobFile.DownloadToStreamAsync(blobResults);

        //            //converto in array il memoryStream
        //            var memoryStreamToArray = blobResults.ToArray();

        //            //converto l'array in base64
        //            base64File = Convert.ToBase64String(memoryStreamToArray);
        //            break;
        //        }
        //        return base64File;
        //    }
        //    catch (StorageException storageException)
        //    {
        //        throw new StorageRowRetrieveException(storageException.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new StorageRowRetrieveException(ex.Message);
        //    }
        //}

        //public async Task<ResponseMessage> OnPostAggiungiFileAsync(string fileName, MemoryStream file )
        //{
        //    try
        //    {
        //        //controllo che il file name non sia null
        //        fileName.CheckString(fileName.Trim(), "File Name is null or empty");

        //        //controlli su CloudBlobClient se esiste
        //        CloudBlobClient.CheckAzureClient(CloudBlobClient, "Client vuoto. Errore");

        //        //controlli su CloudBlobContainer
        //        CloudBlobContainer.CheckAzureContainer(CloudBlobContainer, "Cloud Blob Container vuoto. Errore");

        //        //controlli su CloudBlobDirectory
        //        CloudBlobDirectory.CheckAzureDirectory(CloudBlobDirectory, "Cloud Blob Directory vuoto. Errore");

        //        if (file.Length == 0)
        //        {
        //            throw new ArgumentNullException("File is null or empty");
        //        }

        //        //punto al blob ed alla directory relativa, aggiungendo il filename del file che voglio caricare.
        //        var cloudBlockBlob = CloudBlobDirectory.GetBlockBlobReference(fileName);

        //        //il metodo di upload carica il file in formato stream
        //        await cloudBlockBlob.UploadFromStreamAsync(file);

        //        return new ResponseMessage("File uploaded correctly.");
        //    }
        //    catch (StorageException storageException)
        //    {
        //        throw new StorageInsertException(storageException.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new StorageInsertException(ex.Message);
        //    }
        //}

        //public async Task<ResponseMessage> RemoveFileAsync(string fileName)
        //{
        //    try
        //    {
        //        //controllo che il file name non sia null
        //        fileName.CheckString(fileName.Trim(), "File Name is null or empty");

        //        //controlli su CloudBlobClient se esiste
        //        CloudBlobClient.CheckAzureClient(CloudBlobClient, "Client vuoto. Errore");

        //        //controlli su CloudBlobContainer
        //        CloudBlobContainer.CheckAzureContainer(CloudBlobContainer, "Cloud Blob Container vuoto. Errore");

        //        //controlli su CloudBlobDirectory
        //        CloudBlobDirectory.CheckAzureDirectory(CloudBlobDirectory, "Cloud Blob Directory vuoto. Errore");

        //        //punto al blob ed alla directory relativa, aggiungendo il filename del file che voglio caricare.
        //        var cloudBlockBlob = CloudBlobDirectory.GetBlockBlobReference(fileName);

        //        //il metodo di delete cancella il file presente nel container e nel folder definito precedentemente
        //        await cloudBlockBlob.DeleteAsync();

        //        return new ResponseMessage("File removed correctly.");
        //    }
        //    catch (StorageException storageException)
        //    {
        //        throw new StorageRowDeleteException(storageException.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new StorageRowDeleteException(ex.Message);
        //    }
        //}

    }
}
