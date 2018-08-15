using System.Collections.Generic;

namespace GasPrice.Data.Models
{
    public class RskJsonRpcResultModel
    {
        public string number { get; set; }
        public string hash { get; set; }
        public string parentHash { get; set; }
        public string sha3Uncles { get; set; }
        public string logsBloom { get; set; }
        public string transactionsRoot { get; set; }
        public string stateRoot { get; set; }
        public string receiptsRoot { get; set; }
        public string miner { get; set; }
        public string difficulty { get; set; }
        public string totalDifficulty { get; set; }
        public string extraData { get; set; }
        public string size { get; set; }
        public string gasLimit { get; set; }
        public string gasUsed { get; set; }
        public string timestamp { get; set; }
        public List<string> transactions { get; set; }
        public List<string> uncles { get; set; }
        public string minimumGasPrice { get; set; }
    }

    public class RskJsonRpcModel
    {
        public string jsonrpc { get; set; }
        public int id { get; set; }
        public RskJsonRpcResultModel result { get; set; }
    }
}
