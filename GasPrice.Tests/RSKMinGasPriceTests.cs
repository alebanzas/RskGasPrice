using System;
using GasPrice.Data.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasPrice.Tests
{
    [TestClass]
    public class RSKMinGasPriceTests
    {
        [TestMethod]
        public void WhenGetMinGasPriceThenReturns()
        {
            GasPriceService gps = new GasPriceService();

            var mgpgw = gps.GetRskMinGasPrice();

            Console.WriteLine(mgpgw);
        }

    }
}
