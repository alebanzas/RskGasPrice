using System;
using GasPrice.Data.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasPrice.Tests
{
    [TestClass]
    public class CoinMarketCapTests
    {
        [TestMethod]
        public void WhenLookingForBTCWithUSDRateThenGetTheRate()
        {
            var s = new CoinMarketCapPriceService();

            Console.WriteLine(s.GetBtcInUsd());

        }


        [TestMethod]
        public void WhenLookingForETHWithUSDRateThenGetTheRate()
        {
            var s = new CoinMarketCapPriceService();

            Console.WriteLine(s.GetEthInUsd());

        }

        [TestMethod]
        public void WhenLookingForCryptoWithUSDRateThenGetTheRate()
        {
            var s = new CoinMarketCapPriceService();
            var r = s.GetInUsd();
            Console.WriteLine($"{r.Item1} {r.Item2}");
        }
    }
}
