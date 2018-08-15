using System;
using System.Configuration;
using System.Linq;
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

            gap.Save(new GasMeasurement {Moment = d, RSKMinGasPrice = rsk});

            var l = gap.Get(d);

            Assert.AreEqual(rsk, l.Last().RSKMinGasPrice);
        }
    }
}