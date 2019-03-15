using System;
using System.Web.Mvc;
using GasPrice.Data.Services;

namespace GasPrice.Web.Controllers
{
    public class ConverterController : Controller
    {
        private readonly GasMeasurementService _gasService;
        public ConverterController()
        {
            _gasService = new GasMeasurementService();
        }

        // GET: Converter
        public ActionResult Index()
        {
            var summary = _gasService.GetSummaryByDate(DateTime.UtcNow);

            return View(summary);
        }
    }
}