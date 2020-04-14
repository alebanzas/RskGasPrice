using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using GasPrice.Data.Services;

namespace GasPrice.Web.Controllers
{
    public class ConverterController : Controller
    {
        private readonly GasMeasurementService _gasService;
        private readonly ICryptoPriceService _cryptoService;
        public ConverterController()
        {
            _gasService = new GasMeasurementService();
            _cryptoService = new CoinMarketCapPriceService();
        }

        // GET: Converter
        public ActionResult Index()
        {
            var summary = _gasService.GetSummaryByDate(DateTime.UtcNow);

            return View(summary);
        }


        // GET: Converter
        [AllowCrossSite]
        public FileStreamResult CMC(string CMC_PRO_API_KEY)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", 
                string.IsNullOrWhiteSpace(CMC_PRO_API_KEY) ? 
                        ConfigurationManager.AppSettings["CMC_PRO_API_KEY"] : 
                        CMC_PRO_API_KEY);

            var r = client.GetStreamAsync("https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest?id=1,3626,3701").Result;

            return File(r, "application/json");
        }
    }
}