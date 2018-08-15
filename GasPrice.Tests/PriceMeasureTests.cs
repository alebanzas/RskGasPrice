using System;
using GasPrice.Data.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasPrice.Tests
{
    [TestClass]
    public class PriceMeasureTests
    {
        [TestMethod]
        public void WhenGetMeasureThenAllData()
        {
            var ms = new GasPriceService();

            var m = ms.GetGasMeasure();

            Console.WriteLine("ETH: " + m.EthGasPriceInUsd());
            Console.WriteLine("RSK: " + m.RskGasPriceInUsd());
        }
    }
}
