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
        public static string CacheKey = nameof(HomeController).ToLowerInvariant();

        public ActionResult Index()
        {
            if (_cache.Get<List<GasMeasurement>>(CacheKey) == null)
            {
                _cache.Put(CacheKey, GetGasInfo(), TimeSpan.FromMinutes(5));
            }

            return View(_cache.Get<List<GasMeasurement>>(CacheKey));
        }

        private static List<GasMeasurement> GetGasInfo()
        {
            var gap = new GasAzurePersistor(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);

            var d = DateTime.UtcNow;
            var l = gap.Get(d);
            return l;
        }
    }
}