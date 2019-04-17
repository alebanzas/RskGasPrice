using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using GasPrice.Data.Models;
using Newtonsoft.Json;

namespace GasPrice.Data.Services
{
    public class GasPriceService : IGasPriceService
    {
        private readonly ICryptoPriceService _cs;

        public GasPriceService() : this(new CoinMarketCapPriceService()) { }

        public GasPriceService(ICryptoPriceService cs)
        {
            _cs = cs;
        }

        public GasMeasurement GetGasMeasure()
        {
            var ethPrice = GetEthGasPrice();
            
            return new GasMeasurement
            {
                RSKMinGasPrice = GetRskMinGasPrice(),
                EthGasPriceLow = ethPrice.Item1,
                EthGasPriceStandard = ethPrice.Item2,
                EthGasPriceFast = ethPrice.Item3,
                BtcVsUsd = _cs.GetBtcInUsd(),
                EthVsUsd = _cs.GetEthInUsd(),
            };
        }

        public decimal GetRskMinGasPrice()
        {
            /*
             curl https://public-node.rsk.co 
                -X POST 
                -H "Content-Type: application/json" 
                --data '{"jsonrpc":"2.0","method":"eth_blockNumber","params":[],"id":1}'
            */

            var url = "https://public-node.rsk.co";
            var jsonData = "{\"jsonrpc\":\"2.0\",\"method\":\"eth_getBlockByNumber\",\"params\":[\"latest\",false],\"id\":1}";
            string r;

            using (var client = new WebClient())
            {
                client.Headers.Add("content-type", "application/json");
                r = client.UploadString(url, jsonData);
            }


            var o = JsonConvert.DeserializeObject<RskJsonRpcModel>(r);

            var mgp = decimal.Parse(o.result.minimumGasPrice);

            var mgpgw = mgp / 1000000000; //in gWei

            return mgpgw;
        }

        public Tuple<decimal, decimal, decimal> GetEthGasPrice()
        {
            var c = new HttpClient();
            //returns eth gas price in gWei
            var r = c.GetStringAsync("https://ethgasstation.info/json/ethgasAPI.json").Result;

            var o = JsonConvert.DeserializeObject<EthGasPriceDTOModel>(r);

            return new Tuple<decimal, decimal, decimal>(
                decimal.Parse(o.safeLow.ToString(CultureInfo.InvariantCulture)),
                decimal.Parse(o.average.ToString(CultureInfo.InvariantCulture)),
                decimal.Parse(o.fast.ToString(CultureInfo.InvariantCulture)));
        }
    }

    public interface IGasPriceService
    {
        GasMeasurement GetGasMeasure();
    }
}
