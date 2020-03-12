using System;
using System.IO;
using GasPrice.Data.AzureStorage;
using GasPrice.Data.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace GasPrice.Job
{
    public class Functions
    {
        public static void GasRunner([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo timerInfo, TextWriter log)
        {
            log.WriteLine($"C# Timer trigger function executed at: {DateTime.Now}");

            var gs = new GasPriceService();
            var gm = gs.GetGasMeasure();

            var gp = new GasAzurePersistor(AmbientConnectionStringProvider.Instance.GetConnectionString(ConnectionStringNames.Storage));
            gp.Save(gm);

            log.WriteLine($"Result: RSK={gm.RskGasPriceInUsd()} - ETH={gm.EthGasPriceInUsd()}");
        }
    }
}
