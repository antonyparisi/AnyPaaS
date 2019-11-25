using LibreriaStorageTest.Library.Helper.Exceptions;
using LibreriaStorageTest.Library.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnyPaasCloud
{
    public class ProviderLocator
    {

        IConfigurationRoot Configuration { set; get; }

        public ProviderLocator()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }



        public ITableCloud InizializeTableProvider()
        {
            try
            {
 
                switch (Configuration["ProviderName"])
                {
                    case "Azure":
                        return new TableAzure(Configuration);
                    case "AWS":
                        return new TableAWS();
                    default:
                        throw new ProviderNullException("Provider is empty or null.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception not managed: " + ex);
                return null;
            }
        }
        public IStorageCloud InizializeBlobProvider()
        {
            try
            {
                //tramite "GetEnvironmentVariable" ci estraiamo il ProviderName settato con la pipeline di deploy

                switch (Configuration["ProviderName"])
                {
                    case "Azure":
                        return new StorageAzure(Configuration);
                    case "AWS":
                        return new StorageAWS(Configuration);
                    default:
                        throw new ProviderNullException("Provider is empty or null.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception not managed: " + ex);
                return null;
            }
        }
    }
}
