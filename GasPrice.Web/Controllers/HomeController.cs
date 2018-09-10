using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using GasPrice.Data.AzureStorage;
using GasPrice.Data.Models;
using GasPrice.Data.Services;

namespace GasPrice.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebCache _cache = new WebCache();

        public static string CacheKey(DateTime d)
        {
            return $"{nameof(HomeController).ToLowerInvariant()}.{0}";
        }

        public ActionResult Index(DateTime? date)
        {
            var d = date ?? DateTime.UtcNow;
            var ck = CacheKey(d);

            if (_cache.Get<List<GasMeasurement>>(ck) == null)
            {
                _cache.Put(CacheKey(date ?? DateTime.UtcNow), GetGasInfo(d), TimeSpan.FromMinutes(5));
            }

            return View(_cache.Get<List<GasMeasurement>>(ck));
        }

        private static List<GasMeasurement> GetGasInfo(DateTime? d)
        {
            var gap = new GasAzurePersistor(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);

            var l = gap.Get(d ?? DateTime.UtcNow);
            return l;
        }
    }
}