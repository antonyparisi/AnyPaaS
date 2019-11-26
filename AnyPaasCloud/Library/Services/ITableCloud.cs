using Amazon.DynamoDBv2.DocumentModel;
using LibreriaStorageTest.Library.Helper;
using LibreriaStorageTest.Library.Model;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Services
{
    public interface ITableCloud
    {

        Task<bool> CreateIfNotExist(string tableName);

        Task<bool> InsertRow<T>(string tableName, T row);

        Task<T> GetRow<T>(string tableName, string id);

        Task<bool> DeleteRow(string tableName, string partitionKey, string id);



        //ResponseMessage GetTable(string tableName, string connectionstring);
        //Task<List<string>> GetAllItemsAsync();
        //Task<List<string>> GetItemAsync(string json);
        //Task<ResponseMessage> SaveItemAsync(string partitionKeyValue, string rowKeyValue);
        //Task<ResponseMessage> SaveItemJsonAsync(string json);
        //Task<ResponseMessage> DeleteItemAsync(string partitionKeyValue, string rowKeyValue);

    }
}