using System;
using System.Web.Mvc;
using GasPrice.Data.Services;
using GasPrice.Web.Models;

namespace GasPrice.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly GasMeasurementService _gasService;
        public HomeController()
        {
            _gasService = new GasMeasurementService();
        }

        public ActionResult Index(DateTime? date)
        {
            var d = date ?? DateTime.UtcNow;
            var list = _gasService.GetByDate(d);
            var summary = _gasService.GetSummaryByDate(d);
            var dayList = _gasService.GetSummaryByDate(DateTime.UtcNow.AddDays(-14), DateTime.UtcNow);

            var model = new IndexViewModel
            {
                TodaySummary = summary,
                TodayList = list,
                SummaryList = dayList
            };

            return View(model);
        }

        public ActionResult History(DateTime? from, DateTime? to)
        {
            var f = from ?? DateTime.UtcNow.AddDays(-7);
            var t = to ?? DateTime.UtcNow;

            var dayList = _gasService.GetSummaryByDate(f, t);

            var model = new HistoryViewModel
            {
                From = f,
                To = t,
                DayList = dayList
            };

            return View(model);
        }
    }
}