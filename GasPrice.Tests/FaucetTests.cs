using System;
using GasPrice.Data.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasPrice.Tests
{
    [TestClass]
    public class FaucetTests
    {
        [TestMethod]
        public void WhenDispenseThenSendTx()
        {
            var rskService = new RskService();

            var transaction = rskService.SendTransaction("0xcb11cec9e4f3f6a9eaa880e2a030e571b744cfa1", 0.0001m).Result;

            Console.WriteLine(transaction);
        }

    }
}
