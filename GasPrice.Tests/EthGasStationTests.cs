using System;
using GasPrice.Data.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasPrice.Tests
{
    [TestClass]
    public class EthGasStationTests
    {
        [TestMethod]
        public void WhenNavigateToStationThenGetThePrices()
        {
            var gps = new GasPriceService();

            var gp = gps.GetEthGasPrice();

            Console.WriteLine(gp.Item1);
            Console.WriteLine(gp.Item2);
            Console.WriteLine(gp.Item3);

        }
    }
}
