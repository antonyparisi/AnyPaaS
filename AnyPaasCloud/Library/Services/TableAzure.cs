using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Cdc.storagecsp.Library.Helper;
using DTEConverter;
using LibreriaStorageTest.Library.Helper;
using LibreriaStorageTest.Library.Helper.Exceptions;
using LibreriaStorageTest.Library.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace LibreriaStorageTest.Library.Services
{
    public class TableAzure : ITableCloud
    {
        CloudTable CloudTableAzure{ get; set; }

        CloudStorageAccount StorageAccount { get; set; }

        public TableAzure(IConfigurationRoot configuration)
        {
            //Connection string da configurazione
            var connectionString = configuration["AzureStorageAccount:ConnectionString"];
            
            // StorageAccount
            CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccountCloud);

            //
            StorageAccount = storageAccountCloud;

        }

        public async Task<bool> CreateIfNotExist(string tableName)
        {
            try
            {
                // Client
                CloudTableClient tableClient = StorageAccount.CreateCloudTableClient();

                // Table
                var table = tableClient.GetTableReference(tableName);

                // Crea se non esiste
                await table.CreateIfNotExistsAsync();

                //Create
                return true;
            }
            catch (Exception)
            {
                //Non Creata
                return false;
            }
        }

        public async Task<bool> InsertRow<T>(string tableName, T row)
        {

            try
            {
                var rowTableEntity = (ITableEntity)row;
                
                // Client
                CloudTableClient tableClient = StorageAccount.CreateCloudTableClient();
                // Table
                var table = tableClient.GetTableReference(tableName);

                // Preparo l'operazione di inserimento
                var tableInsertOperation = TableOperation.Insert(rowTableEntity);

                // inserisco
                var returnAnagraficaTable = await table.ExecuteAsync(tableInsertOperation);

                //
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<T> GetRow<T>(string tableName, string id)
        {

            // Client
            CloudTableClient tableClient = StorageAccount.CreateCloudTableClient();
            
            // Table
            var table = tableClient.GetTableReference(tableName);

            // preparo l'operazione di recupero
            var tableRetriveOperation = TableOperation.Retrieve(tableName, id);

            // risultato dell'operazione
            var tableResult = await table.ExecuteAsync(tableRetriveOperation);

            T entity = DynamicTableEntityConverter.ConvertToPOCO<T>(tableResult.Result as DynamicTableEntity);
            
            //riporto il tipo
            return entity;

        }

        public async Task<bool> DeleteRow(string tableName, string partitionKey, string id)
        {

            try
            {
                // Client
                CloudTableClient tableClient = StorageAccount.CreateCloudTableClient();

                // Table
                var table = tableClient.GetTableReference(tableName);

                var tableEntity = new TableEntity(partitionKey, id);
                tableEntity.ETag = "*";

                // preparo l'operazione di cancellazione
                var tableDeleteOperation = TableOperation.Delete(tableEntity);

                //eseguo l'operazione definita
                var response = await table.ExecuteAsync(tableDeleteOperation);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }







        public ResponseMessage GetTable(string tableName, string connectionstring)
        {
            try
            {
                //controllo se la table name è null o stringa vuota
                tableName.CheckString(tableName.Trim(), "Table Name is null or empty");

                //controllo se la connection string è null o stringa vuota
                connectionstring.CheckString(connectionstring.Trim(), "Connection String is null or empty");

                //Account
                CloudStorageAccount.TryParse(connectionstring, out CloudStorageAccount storageAccount);
                //Client
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                //Table
                CloudTableAzure = tableClient.GetTableReference(tableName);

                return new ResponseMessage("CloudAzureTable found correctly.");
            }
            catch (StorageException storageException)
            {
                Console.WriteLine(storageException.ToString());
                throw new TableRetrieveException(storageException.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new TableRetrieveException(ex.Message);
            }
        }
        public async Task<ResponseMessage> DeleteItemAsync(string partitionKeyValue, string rowKeyValue)
        {
            try
            {
                //controllo se la Storage Account Table è null 
                CloudTableAzure.CheckAzureTable(CloudTableAzure, "CloudTableAzure is null");

                //controllo se la partition key è null o stringa vuota
                partitionKeyValue.CheckString(partitionKeyValue.Trim(), "Partition Key is null or empty");

                //controllo se la row key è null o stringa vuota
                rowKeyValue.CheckString(rowKeyValue.Trim(), "Row Key is null or empty");

                //setto la TableEntity che tramite la coppia di chiavi "partitionKey" "rowKey" estraggono la row della table
                var tableEntity = new TableEntity(partitionKeyValue, rowKeyValue);
                tableEntity.ETag = "*";

                //definisco l'operazione da eseguire sulla table
                var tableOperation = TableOperation.Delete(tableEntity);

                //eseguo l'operazione definita
                var response = await CloudTableAzure.ExecuteAsync(tableOperation);

                //ritorno un messaggio
                return new ResponseMessage("Row delete completed.");
            }
            catch(StorageException storageException)
            {
                Console.WriteLine(storageException.ToString());
                throw new TableRowDeleteException(storageException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new TableRowDeleteException(ex.Message);
            }
        }
        public async Task<ResponseMessage> SaveItemAsync(string partitionKeyValue, string rowKeyValue)
        {
            try
            {
                //controllo se la Storage Account Table è null 
                CloudTableAzure.CheckAzureTable(CloudTableAzure, "CloudTableAzure is null");

                //controllo se la partition key è null o stringa vuota
                partitionKeyValue.CheckString(partitionKeyValue.Trim(), "Partition Key is null or empty");

                //controllo se la row key è null o stringa vuota
                rowKeyValue.CheckString(rowKeyValue.Trim(), "Row Key is null or empty.");

                //setto la TableEntity che tramite la coppia di chiavi "partitionKey" "rowKey" estraggono la row della table
                var tableEntity = new TableEntity(partitionKeyValue, rowKeyValue);

                //definisco l'operazione da eseguire sulla table
                var tableOperation = TableOperation.InsertOrReplace(tableEntity);

                //eseguo l'operazione definita
                var response = await CloudTableAzure.ExecuteAsync(tableOperation);

                //ritorno un messaggio
                return new ResponseMessage("Item saved successfully");
            }
            catch (StorageException storageException)
            {
                Console.WriteLine(storageException.ToString());
                throw new TableInsertException(storageException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new TableInsertException(ex.Message);
            }
        }
        public async Task<ResponseMessage> SaveItemJsonAsync(string json)
        {
            try
            {
                //controllo se la Storage Account Table è null 
                CloudTableAzure.CheckAzureTable(CloudTableAzure, "CloudTableAzure is null");

                //controllo se il json è vuoto
                json.CheckString(json.Trim(), "Json is null or empty.");

                //setto la TableEntity che tramite la coppia di chiavi "partitionKey" "rowKey" estraggono la row della table
                var tableEntity = new TableEntity();

                var dictionary = JsonToDictionary(json);

                //definisco l'operazione da eseguire sulla table
                var tableOperation = TableOperation.InsertOrReplace(tableEntity);

                //eseguo l'operazione definita
                var response = await CloudTableAzure.ExecuteAsync(tableOperation);

                //ritorno un messaggio
                return new ResponseMessage("Item saved successfully");
            }
            catch (StorageException storageException)
            {
                Console.WriteLine(storageException.ToString());
                throw new TableInsertException(storageException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new TableInsertException(ex.Message);
            }
        }
        public async Task<List<string>> GetItemAsync(string json)
        {
            try
            {
                //controllo se la Storage Account Table è null 
                CloudTableAzure.CheckAzureTable(CloudTableAzure, "CloudTableAzure is null");

                //controllo se il json è vuoto
                json.CheckString(json.Trim(), "Can't retrieve a row. Json is blank.");
                
                //converto il json in dizionario
                var dictionary = JsonToDictionary(json);

                //preparo la generic query
                var genericQuery = new TableQuery();
                string query = "";

                //itero il dizionario per eseguire la query
                foreach (var d in dictionary)
                {
                    if (query.Trim() != "")
                    {
                        query = query + " and " + TableQuery.GenerateFilterCondition(d.Key, QueryComparisons.Equal, d.Value.Trim());
                    }
                    else
                    {
                        query = TableQuery.GenerateFilterCondition(d.Key, QueryComparisons.Equal, d.Value.Trim());
                    }

                }
                //definisco le condizioni di where della generic query
                genericQuery = genericQuery.Where(query);
                
                var TableContinuationToken = new TableContinuationToken();

                //eseguo la query preparata precedentemente
                var tableResult = await CloudTableAzure.ExecuteQuerySegmentedAsync(genericQuery, TableContinuationToken);

                //ritorno il risultato della query come Json
                return ConvertToJson(tableResult.Results);
            }
            catch (StorageException storageException)
            {
                Console.WriteLine(storageException.ToString());
                throw new TableRowRetrieveException(storageException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new TableRowRetrieveException(ex.Message);
            }
        }
        public async Task<List<string>> GetAllItemsAsync()
        {
            try
            {
                //controllo se la Storage Account Table è null 
                CloudTableAzure.CheckAzureTable(CloudTableAzure, "CloudTableAzure is null");

                var TableContinuationToken = new TableContinuationToken();

                //eseguo una query che estrae tutte le righe delle tabelle
                var tableResult = await CloudTableAzure.ExecuteQuerySegmentedAsync(new TableQuery(), TableContinuationToken);

                return ConvertToJson(tableResult.Results);
            }
            catch (StorageException storageException)
            {
                Console.WriteLine(storageException.ToString());
                throw new TableRowRetrieveAllException(storageException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new TableRowRetrieveAllException(ex.Message);
            }
        }
        public List<string> ConvertToJson(IList<DynamicTableEntity> listDynamicTableEntities)
        {
            var storageCloudTable = new TableCloud();

            foreach (var dynamicTableEntity in listDynamicTableEntities)
            {
                Dictionary<string, string> jsonDictionary = new Dictionary<string, string>();
                jsonDictionary.Add("PartitionKey", dynamicTableEntity.PartitionKey);
                jsonDictionary.Add("RowKey", dynamicTableEntity.RowKey);
                foreach (var property in dynamicTableEntity.Properties)
                {
                    jsonDictionary.Add(property.Key, property.Value.StringValue);
                }
                storageCloudTable.JSON.Add(JsonConvert.SerializeObject(jsonDictionary, Formatting.Indented));
            }
            return storageCloudTable.JSON;
        }
        public Dictionary<string, string> JsonToDictionary(string json)
        {
            Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dictionary;
        }
    }
}
