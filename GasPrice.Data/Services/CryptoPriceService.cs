using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using GasPrice.Data.Models;
using Newtonsoft.Json;

namespace GasPrice.Data.Services
{
    public class CoinMarketCapPriceService : ICryptoPriceService
    {
        /*
         COINMARKETCAP
        Please limit requests to no more than 333 per day. (4´30´´ MAX)
        Endpoints update every 5 minutes.
        https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest
        */

        public decimal GetBtcInUsd()
        {
            return GetInUsd().Item1;
        }

        public decimal GetEthInUsd()
        {
            return GetInUsd().Item2;
        }

        public Tuple<decimal, decimal> GetInUsd()
        {
            var c = new WebClient();
            c.Headers.Add("X-CMC_PRO_API_KEY", ConfigurationManager.AppSettings["CMC_PRO_API_KEY"]);

            var r = c.DownloadString("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?start=1&limit=2&convert=USD");

            var o = JsonConvert.DeserializeObject<CoinMarketCapResult>(r);

            return new Tuple<decimal, decimal>((decimal)o.data[0].quote.USD.price,
                                               (decimal)o.data[1].quote.USD.price);

        }
    }

    public interface ICryptoPriceService
    {
        Tuple<decimal, decimal> GetInUsd();

        decimal GetBtcInUsd();

        decimal GetEthInUsd();
    }
}
