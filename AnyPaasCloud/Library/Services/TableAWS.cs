using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Cdc.storagecsp.Library.Helper;
using LibreriaStorageTest.Library.Helper;
using LibreriaStorageTest.Library.Helper.Exceptions;
using LibreriaStorageTest.Library.Model;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibreriaStorageTest.Library.Services
{
    public class TableAWS : ITableCloud
    {
        public Table CloudAWSTable { get; set; }

        public async Task<bool> CreateIfNotExist(string tableName)
        {

            return true;

        }


        public async Task<bool> InsertRow<T>(string tableName, T row)
        {

            return true;

        }

        public async Task<T> GetRow<T>(string tableName, string id)
        {
            T ss = default(T);

            return ss;

        }

        public async Task<bool> DeleteRow(string tableName, string partitionKey, string id)
        {

            return true;


        }

        public ResponseMessage GetTable(string tableName, string connectionstring)
        {
            try
            {
                //controllo se il nome della tabella è null
                tableName.CheckString(tableName.Trim(), "Table Name is null or empty");

                //credenziali AWS da commentare
                var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);

                //punto alla AWS Dynamo DB Table e ne eseguo il get
                CloudAWSTable = Table.LoadTable(client, tableName);
                
                return new ResponseMessage("CloudAWSTable found correctly.");
            }
            catch (AmazonServiceException amazonServiceException)
            {
                Console.WriteLine(amazonServiceException.ToString());
                throw new TableRetrieveException(amazonServiceException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new TableRetrieveException(ex.Message);
            }
        }

        public async Task<ResponseMessage> DeleteItemAsync(string partitionKeyValue, string rowKeyValue)
        {
            try
            {
                //controllo se l'AWS DynamoDB Table è null
                CloudAWSTable.CheckAWSTable(CloudAWSTable, "CloudAWSTable is null");

                //controllo se la partition key è null o stringa vuota
                partitionKeyValue.CheckString(partitionKeyValue.Trim(), "Partition Key is null or empty");

                //controllo se la rowKey è null o stringa vuota
                rowKeyValue.CheckString(rowKeyValue.Trim(), "Row Key is null or empty.");

                //mi salvo il result in una variabile
                var result = await CloudAWSTable.DeleteItemAsync(partitionKeyValue);
                
                //se ritorna null chiamo un'eccezione
                if(result==null)
                {
                    throw new TableRowDeleteException("Can't delete the item.");
                }

                //altrimenti ritorno un messaggio di conferma
                return new ResponseMessage("Row delete completed.");
            }
            catch(AmazonServiceException amazonServiceException)
            {
                Console.WriteLine(amazonServiceException.ToString());
                throw new TableRowDeleteException(amazonServiceException.Message);
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
                //controllo se l'AWS DynamoDB Table è null
                CloudAWSTable.CheckAWSTable(CloudAWSTable, "CloudAWSTable is null");

                //controllo se la partition key è null o stringa vuota
                partitionKeyValue.CheckString(partitionKeyValue.Trim(), "Partition Key is null or empty");

                //controllo se la rowKey è null o stringa vuota
                rowKeyValue.CheckString(rowKeyValue.Trim(), "Row Key is null or empty.");

                //inizializzo il document
                var document = new Document();

                //mappo i campi di document
                document["PartitionKey"] = partitionKeyValue;
                document["RowKey"] = rowKeyValue;

                //eseguo una put dell'oggetto Document
                var result = await CloudAWSTable.PutItemAsync(document);

                //se il risultato della put è null chiamo un'eccezione
                if (result == null)
                {
                    throw new TableInsertException("Can't save the item.");
                }

                //altrimenti ritorno un messaggio di conferma
                return new ResponseMessage("Item saved successfully");
            }
            catch(AmazonServiceException amazonServiceException)
            {
                Console.WriteLine(amazonServiceException.ToString());
                throw new TableInsertException(amazonServiceException.Message);
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
                //controllo se l'AWS DynamoDB Table è null
                CloudAWSTable.CheckAWSTable(CloudAWSTable, "CloudAWSTable is null");

                //controllo che il json non sia vuoto
                json.CheckString(json.Trim(), "Can't retrieve a row. Json is blank.");

                //inizializzazione dell'oggetto "ScanFilter" necessario per eseguire i filtri sulla tabella
                var scanFilter = new ScanFilter();

                //converto il json in un dizionario
                var dictionary = JsonToDictionary(json);

                //controllo se il dizionario è vuoto
                if (dictionary != null)
                {
                    //aggiungo le condizioni per i filtri della query
                    foreach (var d in dictionary)
                    {
                        scanFilter.AddCondition(d.Key, ScanOperator.Equal, d.Value);
                    }
                }
                else
                {
                    throw new NullReferenceException("Dictionary from json is null.");
                }

                //Il metodo "Scan" esegue una search sulla tabella dynamodb e ritorna i risultati. Se non si settano condition, ritornano tutti i records.
                var result = await CloudAWSTable.Scan(scanFilter).GetRemainingAsync();

                //ritorno un json chiamando il metodo di conversione
                return ConvertToJson(result);
            }
            catch(AmazonServiceException amazonServiceException)
            {
                Console.WriteLine(amazonServiceException.ToString());
                throw new TableRowRetrieveException(amazonServiceException.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new TableRowRetrieveException(ex.Message);
            }
        }
        public async Task<List<string>> GetAllItemsAsync()
        {
            try
            {
                //controllo se l'AWS DynamoDB Table è null
                CloudAWSTable.CheckAWSTable(CloudAWSTable, "CloudAWSTable is null");

                //inizializzazione dell'oggetto "ScanFilter" necessario per eseguire i filtri sulla tabella
                var scanFilter = new ScanFilter();

                //Il metodo "Scan" esegue una search sulla tabella dynamodb e ritorna i risultati. Se non si settano condition, ritornano tutti i records.
                var result = await CloudAWSTable.Scan(scanFilter).GetRemainingAsync();

                //ritorno un json chiamando il metodo di conversione
                return ConvertToJson(result);
            }
            catch (AmazonServiceException amazonServiceException)
            {
                Console.WriteLine(amazonServiceException.ToString());
                throw new TableRowRetrieveAllException(amazonServiceException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new TableRowRetrieveAllException(ex.Message);
            }
        }
        public async Task<ResponseMessage> SaveItemJsonAsync(string json)
        {
            try
            {
                //controllo se l'AWS DynamoDB Table è null
                CloudAWSTable.CheckAWSTable(CloudAWSTable, "CloudAWSTable is null");

                //controllo se il json è null o stringa vuota
                json.CheckString(json.Trim(), "Can't retrieve a row. Json is blank.");

                //converto il json in un dizionario
                var dictionary = JsonToDictionary(json);

                //inizializzo il document
                var document = new Document();

                //controllo se il dizionario è vuoto
                if (dictionary != null)
                {
                    foreach (var d in dictionary)
                    {
                        document[d.Key] = d.Value;
                    }
                }
                else
                {
                    throw new NullReferenceException("Dictionary from json is null.");
                }

                //eseguo una put dell'oggetto Document
                var result = await CloudAWSTable.PutItemAsync(document);

                //se il risultato della put è null chiamo un'eccezione
                if (result == null)
                {
                    throw new TableInsertException("Can't save the item.");
                }

                //altrimenti ritorno un messaggio di conferma
                return new ResponseMessage("Item saved successfully");
            }
            catch (AmazonServiceException amazonServiceException)
            {
                Console.WriteLine(amazonServiceException.ToString());
                throw new TableInsertException(amazonServiceException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new TableInsertException(ex.Message);
            }
        }
        public List<string> ConvertToJson(List<Document> listDocument)
        {
            var storageCloudTable = new TableCloud();
            foreach (Document document in listDocument)
            {
                var keys = new List<string>();
                var values = new List<string>();
                foreach (string k in document.Keys)
                {
                    keys.Add(k.ToString());
                }
                foreach (string v in document.Values)
                {
                    values.Add(v.ToString());
                }
                Dictionary<string, string> jsonDictionary = new Dictionary<string, string>();
                for (int i = 0; i < keys.Count; i++)
                {

                    jsonDictionary.Add(keys[i], values[i]);
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
