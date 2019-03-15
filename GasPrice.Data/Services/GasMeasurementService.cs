using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using GasPrice.Data.AzureStorage;
using GasPrice.Data.Models;

namespace GasPrice.Data.Services
{
    public class GasMeasurementService
    {

        private readonly WebCache _cache = new WebCache();
        private readonly GasAzurePersistor _gasAzurePersistor = new GasAzurePersistor(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);

        public IEnumerable<GasMeasurement> GetByDate(DateTime d)
        {
            var ck = CacheKey(d);

            if (_cache.Get<IEnumerable<GasMeasurement>>(ck) == null)
            {
                _cache.Put(CacheKey(d), GetByDateFromStorage(d), TimeSpan.FromMinutes(5));
            }

            return _cache.Get<IEnumerable<GasMeasurement>>(ck);
        }

        public IEnumerable<GasMeasurement> GetSummaryByDate(DateTime from, DateTime to)
        {
            return EachDay(from, to).Select(GetSummaryByDate);
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public GasMeasurement GetSummaryByDate(DateTime d)
        {
            var ck = SummaryCacheKey(d);

            if (_cache.Get<GasMeasurement>(ck) == null)
            {
                _cache.Put(SummaryCacheKey(d), GetSummaryByDateFromStorage(d), TimeSpan.FromHours(24));
            }

            return _cache.Get<GasMeasurement>(ck);
        }

        private string CacheKey(DateTime d)
        {
            return $"{nameof(GasMeasurementService).ToLowerInvariant()}.{d:yy-MM-dd}";
        }

        private string SummaryCacheKey(DateTime d)
        {
            return $"summary{nameof(GasMeasurementService).ToLowerInvariant()}.{d:yy-MM-dd}";
        }

        private IEnumerable<GasMeasurement> GetByDateFromStorage(DateTime d)
        {
            return _gasAzurePersistor.Get(d);
        }
        
        private GasMeasurement GetSummaryByDateFromStorage(DateTime d)
        {
            return _gasAzurePersistor.GetSummary(d);
        }
    }
}
