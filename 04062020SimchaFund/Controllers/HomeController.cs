using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _04062020SimchaFund.Models;
using SimchaFund.data;

namespace _04062020SimchaFund.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string _conn = @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=True;";


        public IActionResult Index()
        {
            SimchaManager sm = new SimchaManager(_conn);
            SimchaViewModel vm = new SimchaViewModel();
            vm.Simchos = sm.GetSimchos();
            vm.TotalContributorCount = sm.GetContrCount();
           
            if (TempData["success-message"] != null)
            {
                vm.Message = (string)TempData["success-message"];
            }
            
            return View(vm);
        }

        [HttpPost]
        public IActionResult AddSimcha(Simcha s)
        {
            SimchaManager sm = new SimchaManager(_conn);
            sm.AddSimcha(s);
            TempData["success-message"] = $"Simcha {s.Name} successfully!";
            return Redirect("/Home/index");
        }
        public IActionResult Contributors()
        {
            SimchaManager sm = new SimchaManager(_conn);
            ContribViewModel vm = new ContribViewModel();
            vm.Contributors = sm.GetContributors();
            vm.TotalBalance = sm.GetTotalBalance();
            return View(vm);
        }

        [HttpPost]
        public IActionResult AddContributor(Contributor c, Decimal Amount)
        {
            SimchaManager sm = new SimchaManager(_conn);
            sm.AddContributor(c);

            if (Amount > 0)
            {
                Transaction t = new Transaction
                {
                    ContrId = c.Id,
                    SimchaId = -1, // -1 is the placheholder simchaid for deposits
                    Amount = Amount,
                    Date = c.DateCreated,
                    TranType = "Deposit"
                };
                sm.AddTransaction(t);//insert the initial deposit 
            }
           
            return Redirect("/Home/Contributors");
        }

        public IActionResult EditContributor(Contributor c)
        {
            SimchaManager sm = new SimchaManager(_conn);
            sm.UpdateContributor(c);

            return Redirect("/Home/Contributors");
        }

        public IActionResult AddTransaction(Transaction t)
        {
            SimchaManager sm = new SimchaManager(_conn);
            sm.AddTransaction(t);
            return Redirect("/Home/Contributors");
        }

        public IActionResult Transactions(int contrId)
        {
            SimchaManager sm = new SimchaManager(_conn);
            TransViewModel vm = new TransViewModel();
            vm.Transactions = sm.GetTransactions(@contrId);
            vm.Name = sm.GetContributorById(contrId).Name;
            vm.Balance = sm.GetBalance(contrId);

            return View(vm);
        
        }

        public IActionResult Contributions(int simchaId, string simchaName)
        {
            SimchaManager sm = new SimchaManager(_conn);
            ContributionViewModel vm = new ContributionViewModel();
            vm.Contributions = sm.GetContributions(simchaId);
            vm.SimchaName = simchaName;
            vm.SimchaId = simchaId;
            return View(vm);
        }

        [HttpPost]
        public IActionResult UpdateContributions (List<Contribution> contributions, int simchaId, string simchaName)
        {
            SimchaManager sm = new SimchaManager(_conn);
            sm.UpdateContribution(contributions, simchaId);
          
            TempData ["success-message"] = $"Contributions for {simchaName} updated successfully!";
            return Redirect("/Home/Index");
        }
    }
}
