using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftwareDesign.Models;
using SoftwareDesign.Data;

namespace SoftwareDesign.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Quote> quoteList = _context.Quotes.Where(x => x.Status == "Prospectus" ).ToList();
            quoteList.OrderBy(x => x.Created);
            var qlMostRecent = quoteList.Take(5).ToList();
            var countProspectus = quoteList.Count();
           // var username = ;

            List<Contract> contracts = _context.Contract.ToList();

            HomepageViewModel homepageVM = new HomepageViewModel();

            homepageVM.quoteList = qlMostRecent;
            homepageVM.countProspectus = countProspectus;
            homepageVM.Contracts = contracts;
            homepageVM.username = User.Identity.Name;

            bool isCustomer = HttpContext.User.IsInRole("Customer");
            ViewData["IsCustomer"] = isCustomer;

            bool isAdmin = HttpContext.User.IsInRole("Admin");
            ViewData["IsAdmin"] = isAdmin;

            return View(homepageVM);
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [Route("../Identity/Account/Register",
                Name = "accountregister")]
        public IActionResult Register() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<Quote> GetAllProspectusQuotes()
        {
            return _context.Quotes.Where(x => x.Status == "Prospectus").ToList();
        }
    }
}
