using System.Collections.Generic;
using GasPrice.Data.Models;

namespace GasPrice.Web.Models
{
    public class IndexViewModel
    {
        public GasMeasurement TodaySummary { get; set; }

        public IEnumerable<GasMeasurement> TodayList { get; set; }

        public IEnumerable<GasMeasurement> SummaryList { get; set; }

    }
}