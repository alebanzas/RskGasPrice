using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private static string GetSummaryBlobName(DateTime d)
        {
            return $"{d.ToUniversalTime():yyyyMMdd}.json";
        }

        private CloudBlockBlob GetBlob(DateTime d)
        {
            return _container.GetBlockBlobReference(GetBlobName(d));
        }

        private CloudBlockBlob GetSummaryBlob(DateTime d)
        {
            return _container.GetBlockBlobReference(GetSummaryBlobName(d));
        }


        public List<GasMeasurement> Get(DateTime? date)
        {
            if (!date.HasValue) date = DateTime.UtcNow;

            var b = GetBlob(date.Value);
            if (!b.ExistsAsync().Result) return new List<GasMeasurement>();

            var s = b.DownloadTextAsync().Result;
            return JsonConvert.DeserializeObject<List<GasMeasurement>>(s);
        }

        public GasMeasurement GetSummary(DateTime? date)
        {
            if (!date.HasValue) date = DateTime.UtcNow;

            var b = GetSummaryBlob(date.Value);
            if (!b.ExistsAsync().Result) return new GasMeasurement();

            var s = b.DownloadTextAsync().Result;
            return JsonConvert.DeserializeObject<GasMeasurement>(s);
        }

        public void Save(GasMeasurement gas)
        {
            var l = Get(gas.Moment);
            l.Add(gas);

            GetBlob(gas.Moment).UploadTextAsync(JsonConvert.SerializeObject(l));

            try
            {
                SaveSummary(l);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error saving summary ({gas.Moment}).");
                Console.WriteLine(e);
            }
        }

        public async Task SaveSummary(IReadOnlyCollection<GasMeasurement> gasMeasurements)
        {
            var count = gasMeasurements.Count;
            var moment = gasMeasurements.OrderByDescending(x => x.Moment).First().Moment;
            var summary = new GasMeasurement
            {
                BtcVsUsd = gasMeasurements.Sum(x => x.BtcVsUsd) / count,
                EthGasPriceFast = gasMeasurements.Sum(x => x.EthGasPriceFast) / count,
                EthGasPriceLow = gasMeasurements.Sum(x => x.EthGasPriceLow) / count,
                EthGasPriceStandard = gasMeasurements.Sum(x => x.EthGasPriceStandard) / count,
                EthVsUsd = gasMeasurements.Sum(x => x.EthVsUsd) / count,
                RSKMinGasPrice = gasMeasurements.Sum(x => x.RSKMinGasPrice) / count,
                Moment = moment,
            };

            await GetSummaryBlob(moment).UploadTextAsync(JsonConvert.SerializeObject(summary));
        }
    }
}
