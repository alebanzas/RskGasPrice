using System;
using System.Collections.Generic;
using GasPrice.Data.Models;

namespace GasPrice.Web.Models
{
    public class HistoryViewModel
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public IEnumerable<GasMeasurement> DayList { get; set; } 
    }
}