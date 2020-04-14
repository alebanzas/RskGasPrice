using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.HdWallet;
using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Configuration;
using GasPrice.Data.Services;
using GasPrice.Web.Models;

namespace GasPrice.Web.Controllers
{
    public class FaucetController : Controller
    {
        // GET: Faucet
        public ActionResult Index()
        {


            return View(new FaucetViewModel());
        }

        // GET: Faucet
        [HttpPost]
        public async Task<ActionResult> Index(FaucetViewModel model)
        {
            if (!model.To.IsValidEthereumAddressHexFormat())
            {
                ModelState.AddModelError(nameof(model.To), "Invalid address");

                return View(model);
            }

            var rskService = new RskService();

            var transaction = await rskService.SendTransaction(model.To, 0.001m);

            model.TransactionHash = transaction;

            return View(model);
        }
    }
}