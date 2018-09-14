using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using GasPrice.Data.AzureStorage;
using GasPrice.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasPrice.Tests.DataTests
{
    [TestClass]
    public class AzurePersistorTests
    {
        [TestMethod]
        public void WhenSaveAMeasurementThenSaves()
        {
            var gap = new GasAzurePersistor(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);

            const int rsk = 1;
            var d = DateTime.UtcNow;

            gap.Save(new GasMeasurement { Moment = d, RSKMinGasPrice = rsk });

            var l = gap.Get(d);

            Assert.AreEqual(rsk, l.Last().RSKMinGasPrice);
        }

        [TestMethod]
        public void GenerateSummary()
        {
            var gap = new GasAzurePersistor(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);

            var startDate = new DateTime(2018, 08, 14);
            var endDate = DateTime.UtcNow;

            foreach (var d in EachDay(startDate, endDate))
            {
                var l = gap.Get(d);

                gap.SaveSummary(l).Wait();

                var s = gap.GetSummary(d);

                Console.WriteLine($"OK: {d:d} (E: {s.EthGasPriceStandard} | R: {s.RSKMinGasPrice})");

                Assert.AreNotEqual(0, s.BtcVsUsd);
                Assert.AreNotEqual(0, s.EthGasPriceFast);
                Assert.AreNotEqual(0, s.EthGasPriceLow);
                Assert.AreNotEqual(0, s.EthGasPriceStandard);
                Assert.AreNotEqual(0, s.EthVsUsd);
                Assert.AreNotEqual(0, s.RSKMinGasPrice);
            }
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}