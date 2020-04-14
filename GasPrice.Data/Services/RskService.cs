using System.Configuration;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace GasPrice.Data.Services
{
    public class RskService
    {
        internal Web3 Web3Provider { get; set; }

        public RskService(string fromPk = "")
        {
            if (string.IsNullOrWhiteSpace(fromPk)) 
                fromPk = ConfigurationManager.AppSettings["FAUCET_KEY"];

            var account = new Account(fromPk);
            Web3Provider = new Web3(account, "https://public-node.testnet.rsk.co");
        }
        
        public async Task<string> SendTransaction(string to, decimal amount = 0.05m, decimal gas = 0.06m)
        {
            var transaction = await Web3Provider.Eth.GetEtherTransferService()
                            .TransferEtherAsync(to, amount, gas);

            return transaction;
        }

    }
}
