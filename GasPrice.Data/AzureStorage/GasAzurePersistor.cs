using System;
using System.Collections.Generic;
using GasPrice.Data.Interfaces;
using GasPrice.Data.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace GasPrice.Data.AzureStorage
{
    public class GasAzurePersistor : IGasPersistor
    {
        private readonly CloudBlobContainer _container;

        public GasAzurePersistor(string connstr)
        {
            var storageAccount = CloudStorageAccount.Parse(connstr);

            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(nameof(GasMeasurement).ToLowerInvariant());
            _container.CreateIfNotExistsAsync();
        }

        private static string GetBlobName(DateTime d)
        {
            return $"{d.ToUniversalTime():yyyyMMdd}.log";
        }

        private CloudBlockBlob GetBlob(DateTime d)
        {
            return _container.GetBlockBlobReference(GetBlobName(d));
        }


        public List<GasMeasurement> Get(DateTime? date)
        {
            if (!date.HasValue) date = DateTime.UtcNow;

            var b = GetBlob(date.Value);
            if (!b.ExistsAsync().Result) return new List<GasMeasurement>();

            var s = b.DownloadTextAsync().Result;
            return JsonConvert.DeserializeObject<List<GasMeasurement>>(s);
        }

        public void Save(GasMeasurement gas)
        {
            var l = Get(gas.Moment);
            l.Add(gas);

            GetBlob(gas.Moment)
                .UploadTextAsync(JsonConvert.SerializeObject(l));
        }
    }
}
