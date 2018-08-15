using System.Net.Http;
using GasPrice.Data.Models;
using Newtonsoft.Json;

namespace GasPrice.Data.Services
{
    public class CoinMarketCapPriceService : ICryptoPriceService
    {
        /*
         COINMARKETCAP
        Please limit requests to no more than 30 per minute.
        Endpoints update every 5 minutes.
        BTC: https://api.coinmarketcap.com/v2/ticker/1/
        ETH: https://api.coinmarketcap.com/v2/ticker/1027/
        */

        public decimal GetBtcInUsd()
        {
            var c = new HttpClient();

            var r = c.GetStringAsync("https://api.coinmarketcap.com/v2/ticker/1/").Result;

            var o = JsonConvert.DeserializeObject<CoinMarketCapResult>(r);

            return o.data.quotes.USD.price;
        }

        public decimal GetEthInUsd()
        {
            var c = new HttpClient();

            var r = c.GetStringAsync("https://api.coinmarketcap.com/v2/ticker/1027/").Result;

            var o = JsonConvert.DeserializeObject<CoinMarketCapResult>(r);

            return o.data.quotes.USD.price;
        }
    }

    public interface ICryptoPriceService
    {
        decimal GetBtcInUsd();

        decimal GetEthInUsd();
    }
}
