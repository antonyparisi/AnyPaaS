using LibreriaStorageTest.Library.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Services
{
    public interface IStorageCloud
    {

        Task<bool> CreateIfNotExistAsync(string containerName);

        Task<string> InsertBlobMessage(string containerName, string id, string message);

        Task<string> GetBlobMessage(string containerName, string id);

        Task<bool> DeleteBlobMessage(string containerName, string id);


        //ResponseMessage GetContainer(string connectionString, string containerName);
        //ResponseMessage GetDirectory(string directoryPath);
        //Task<string> GetItemAsync(string pathFile);
        //Task<ResponseMessage> OnPostAggiungiFileAsync(string pathFile, MemoryStream file);
        //Task<ResponseMessage> RemoveFileAsync(string pathFile);

    }
}
