using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftwareDesign.Data;
using SoftwareDesign.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace SoftwareDesign.Controllers
{
    public class QuoteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
            ViewData["SuccessMessage"]
            ViewData["SuccessMessageStatus"]

            Use these before calling a page with notification partial included 
            to display a notification message relating to the user's last action

         */

        // GET: Quote

        public ActionResult Index()
        {
            return View();
        }

        //GET: Sort Order
        public List<Quote> SortQuotes(List<Quote> quoteRequest, string sortOrder)
        {
            ViewData["CreatedSortParam"] = String.IsNullOrEmpty(sortOrder) ? "Created_desc" : "";
            ViewData["PostcodeSortParam"] = sortOrder == "Postcode" ? "Postcode_desc" : "Postcode";

            switch (sortOrder)
            {
                case "Created_desc":
                    return quoteRequest.OrderByDescending(x => x.Created).ToList();

                case "Postcode":
                    return quoteRequest.OrderBy(x => x.Postcode).ToList();

                case "Postcode_desc":
                    return quoteRequest.OrderByDescending(x => x.Postcode).ToList();

                default:
                    return quoteRequest.OrderBy(x => x.Created).ToList();

            }
        }


        // GET: Quote/Details/5
        public ActionResult View(int ID)
        {
            Quote quoteRequest = GetQuote(ID);
            List<Quote> quoteList = new List<Quote>();
            quoteList.Add(quoteRequest);
            bool isActive = quoteRequest.Status == "Active";
            List<QuoteViewModel> qvmL = GetContractDetails(quoteList, isActive);
            QuoteViewModel quote = qvmL[0];
            ShowNotificationIfNeeded();
            return View(quote);
        }

        [Authorize]
        public ActionResult Contracts()
        {
            // Show customer's active contracts
            ShowNotificationIfNeeded();

            string userID = GetCurrentUserID();

            List<Quote> qRequests = GetAllActiveQuotes(userID);

            List<QuoteViewModel> qvm = GetContractDetails(qRequests, true);
            return View(qvm);
        }

        public string GetCurrentUserID()
        {
            string name = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(x => x.UserName == name);
            return user.Id;
        }

        public List<Quote> GetAllActiveQuotes(string userID)
        {
            List<Quote> qRequests = _context.Quotes.Where(x => x.userID == userID && x.Status == "Active").ToList();
            return qRequests;
        }

        [Authorize]
        public async Task<ActionResult> List()
        {
            // Show customer's open quotes
            string name = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(x => x.UserName == name);
            string sortOrder = "";
            ShowNotificationIfNeeded();


            if (User.IsInRole("Customer")) 
            {
                // Get where match userid && Status != "Active"
                List<Quote> qRequests = _context.Quotes.Where(x => x.userID == user.Id && x.Status!="Incomplete" && x.Status != "Active").ToList();
                List<Quote> qRequestsOrdered = SortQuotes(qRequests, sortOrder);
                List <QuoteViewModel> qvmList = GetContractDetails(qRequestsOrdered);
                return View(qvmList);
            }
            else
            {
                // Get where Status == "Prospectus"
                List<Quote> qRequests = _context.Quotes.Where(x=>x.Status == "Prospectus").ToList();
                List<Quote> qRequestsOrdered = SortQuotes(qRequests, sortOrder);
                List<QuoteViewModel> qvmList = GetContractDetails(qRequestsOrdered);
                return View(qvmList);
            }  
        }

        public List<QuoteViewModel> GetContractDetails(List<Quote> quoteList, bool isActive=false)
        {

            List<QuoteViewModel> qvmList = new List<QuoteViewModel>();
            List<Contract> contractList = _context.Contract.Where(x=>x.ID != 0).ToList();
            quoteList.Reverse();
            foreach (Quote q in quoteList)
            {
                QuoteViewModel qvm = new QuoteViewModel();
                qvm.quote = q;
                qvm.contractName = contractList.Where(x => x.ID == qvm.quote.ContractID).FirstOrDefault().ContractName.ToString();
                
                if(isActive)
                {
                    // Yearly or monthly
                    var gasRate = contractList.Where(x => x.ID == qvm.quote.ContractID).FirstOrDefault().GasRate;
                    var electricityRate = contractList.Where(x => x.ID == qvm.quote.ContractID).FirstOrDefault().ElectricityRate;

                    double GasPriceD = Math.Truncate((gasRate * (double)(q.EstimatedGasUsage)) * 100);
                    double GasPriceYearly = (GasPriceD * 12) / 0.9;

                    double ElectricityPriceD = Math.Truncate((electricityRate * (double)(q.EstimatedElectricityUsage)) * 100);
                    double ElectricityPriceYearly = (ElectricityPriceD * 12) / 0.9;

                    // Calculate total of electricity and gas
                    if (q.SelectedPaymentFrequency == "Monthly")
                    {
                        qvm.nextPayment = ((GasPriceD + ElectricityPriceD)/100).ToString("#.00");

                        // Calculate next payment date
                        int monthsSinceStart = Math.Abs((q.StartDate.Year - DateTime.Now.Year) + q.StartDate.Month - DateTime.Now.Month);
                        qvm.nextPaymentDate = qvm.quote.StartDate.AddMonths(monthsSinceStart + 1).ToString();
                    } else // Yearly
                    {
                        qvm.nextPayment = ((GasPriceYearly + ElectricityPriceYearly) / 100).ToString("#.00");
                        // Calculate next payment date
                        int yearsSinceStart = Math.Abs(q.StartDate.Year - DateTime.Now.Year);
                        qvm.nextPaymentDate = qvm.quote.StartDate.AddYears(yearsSinceStart + 1).ToString();
                    }
                }
                qvmList.Add(qvm);
            }
            return qvmList;
        }

        // GET: Quote/Create
        public ActionResult Create()
        {
            return View();
        }
        public List<Contract> GetAllContracts()
        {
            return _context.Contract.Where(x => !x.ID.Equals("-1")).ToList();
        }
        // GET: Quote/Create
        public ActionResult CreateQuote2()
        {
            // Show create quote step 2
            int quoteID = Int32.Parse(TempData["QuoteID"].ToString());
            Quote quote = GetQuote(quoteID);
            List<Contract> contracts = GetAllContracts();
            ContractViewModel contractsUpdated = new ContractViewModel();
            TempData["QuoteID"] = TempData["QuoteID"];
            ShowNotificationIfNeeded();
            contractsUpdated.Contracts = contracts;
            contractsUpdated.QuoteID = quoteID;
            contractsUpdated.PaymentFrequency = quote.SelectedPaymentFrequency;
            contractsUpdated.ContractType = quote.SelectedContractType;
            contractsUpdated.EstimatedElectricityUsage = quote.EstimatedElectricityUsage;
            contractsUpdated.EstimatedGasUsage = quote.EstimatedGasUsage;

            return View(contractsUpdated);
        }

        public ActionResult Activate(int ID)
        {
            try
            {
                Quote quote = _context.Quotes.Where(x => x.ID == ID).FirstOrDefault();

                quote.Status = "Active";
                var now = DateTime.Now;
                quote.StartDate = now;
                if (quote.ContractID == 2)
                {
                    quote.EndDate = now.AddYears(1);
                } else if (quote.ContractID == 3) {
                    quote.EndDate = now.AddYears(2);
                } 

                QuoteLog quoteLog = CopyPrimaryQuoteAttributes(quote, "Quote Activated");

                _context.Quotes.Update(quote);
                _context.QuotesLog.Add(quoteLog);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Contract activated successfully.\n Please allow 48 hours for a member of our team to get back to you.";
                TempData["SuccessMessageStatus"] = "Success";

                return RedirectToAction("Contracts");
            } catch (Exception e)
            {
                TempData["SuccessMessage"] = "Contract activation failed, Please review your quote and try agian.";
                TempData["SuccessMessageStatus"] = "Error";

                return RedirectToAction("View", new { ID = ID});
            }

        }

        [HttpPost]
        public ActionResult Create2Submit(ContractViewModel contract)
        {
            // Submit quote step 2
            // Assign type to quote
            if(ModelState.IsValid)
            {
                if (contract.ContractID != null)
                {
                    // Complete quote object and create inital quote log                    
                    Quote quote = GetQuote(contract.QuoteID);

                    quote.ContractID = Int32.Parse(contract.ContractID);
                    quote.Status = "Prospectus";

                    QuoteLog quoteLog = CopyPrimaryQuoteAttributes(quote, "Quote Created");

                    _context.Quotes.Update(quote);
                    _context.QuotesLog.Add(quoteLog);
                    _context.SaveChanges();

                    if(TempData != null)
                    {
                        TempData["SuccessMessage"] = "Quote submitted successfully.\n Please allow 48 hours for a member of our team to get back to you.";
                        TempData["SuccessMessageStatus"] = "Success";
                    }

                    return RedirectToAction("List");
                }
                else
                {
                    TempData["SuccessMessage"] = "Quote creation failed. Please try again";
                    TempData["SuccessMessageStatus"] = "Error";

                    return RedirectToAction("List");
                }
            }
            else
            {
                TempData["SuccessMessage"] = "Quote creation failed. Please select a contract type.";
                TempData["SuccessMessageStatus"] = "Error";

                TempData["QuoteID"] = TempData["QuoteID"];

                return RedirectToAction("CreateQuote2");
            }
        }


        // POST: Quote/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStep1([FromForm]Quote quote)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Create quote step 1
                    string userID;
                    if (User != null)
                    {
                        string name = User.Identity.Name;
                        userID = _context.Users.FirstOrDefault(x => x.UserName == name).Id;
                    }
                    else
                        userID = "TestQuote";

                    // Fill Initial Quote object
                    quote.Status = "Incomplete";
                    quote.Created = DateTime.Now;
                    quote.userID = userID;

                    _context.Quotes.Add(quote);
                    _context.SaveChanges();

                    if(User != null)
                    {
                        TempData["QuoteID"] = quote.ID;
                    }

                    return RedirectToAction("CreateQuote2");

                }
                else
                {
                    TempData["SuccessMessage"] = "Quote creation failed. Please try again";
                    TempData["SuccessMessageStatus"] = "Error";

                    return RedirectToAction("List");
                }
            }
            catch
            {
                TempData["SuccessMessage"] = "Quote creation failed. Please try again";
                TempData["SuccessMessageStatus"] = "Error";

                return RedirectToAction("List");
            }
        }

        public List<QuoteLog> GetQuoteHistory(int ID)
        {
            List<QuoteLog> quoteHistory = _context.QuotesLog.Where(x => x.quoteID == ID).ToList();
            List<QuoteLog> sorted = quoteHistory.OrderByDescending(x => x.Occured).ToList();
            return sorted;
        }
        public Quote GetQuote(int ID)
        {
            Quote q = _context.Quotes.Where(x => x.ID == ID).FirstOrDefault();
            return q;
        }

        [HttpGet]
        public ActionResult History(int ID)
        {
            // Get Quote history
            List<QuoteLog> sorted = GetQuoteHistory(ID);
            List<Contract> contractList = _context.Contract.Where(x => x.ID != 0).ToList();
            List<QuoteLogViewModel> qlvmL = new List<QuoteLogViewModel>();
            // Sort history by most recent first
            foreach (QuoteLog quoteLog in sorted)
            {
                QuoteLogViewModel qlvm = new QuoteLogViewModel();
                qlvm.quoteLog = quoteLog;
                qlvm.contractName = contractList.Where(x => x.ID == quoteLog.ContractID).FirstOrDefault().ContractName.ToString();
                qlvmL.Add(qlvm);
            }

            return View(qlvmL);
        }

        // GET: Quote/Edit/5
        public ActionResult Edit(int ID)
        {
            ShowNotificationIfNeeded();
            Quote quoteRequest = _context.Quotes.Where(x => x.ID == ID).FirstOrDefault();
            return View(quoteRequest);
        }

        // POST: Quote/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromForm]Quote quoteRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Edit Quote
                    Quote oldQuote = GetQuote(quoteRequest.ID);
                    Quote newQuote = oldQuote;
                    newQuote.Postcode = quoteRequest.Postcode;
                    newQuote.EstimatedElectricityUsage = quoteRequest.EstimatedElectricityUsage;
                    newQuote.EstimatedGasUsage = quoteRequest.EstimatedGasUsage;
                    newQuote.SelectedContractType = quoteRequest.SelectedContractType;
                    newQuote.SelectedMeterType = quoteRequest.SelectedMeterType;
                    newQuote.SelectedPaymentFrequency = quoteRequest.SelectedPaymentFrequency;
                    newQuote.ContractID = quoteRequest.ContractID;
                    newQuote.Status = "Prospectus";

                    _context.Quotes.Update(newQuote);

                    QuoteLog log = CopyPrimaryQuoteAttributes(newQuote, "Quote Modified by User");

                    _context.QuotesLog.Add(log);

                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Quote edited successfully";
                    TempData["SuccessMessageStatus"] = "Success";

                    return RedirectToAction("List");
                } else
                {
                    TempData["SuccessMessage"] = "Quote edited failed, please try again";
                    TempData["SuccessMessageStatus"] = "Error";
                    return View();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e.ToString());
                TempData["SuccessMessage"] = "Quote edited failed, please try again";
                TempData["SuccessMessageStatus"] = "Error";
                return View();
            }
        }

        // GET: Quote/Respond/
        [Authorize(Roles = "Admin")]
        public ActionResult Respond(int ID)
        {
            QuoteResponseViewModel response = new QuoteResponseViewModel();
            ShowNotificationIfNeeded();
            Quote quoteRequest = GetQuote(ID);
            response.respondQuote = quoteRequest;
            return View(response);
        }

        // POST: Quote/Respond/
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Respond([FromForm] QuoteResponseViewModel quoteRequest)
        {
            try
            {
                string userID;
                if (User != null)
                {
                    string name = User.Identity.Name;
                    var user = _context.Users.FirstOrDefault(x => x.UserName == name);
                    userID = user.Id;
                } else
                {
                    userID = "3da38009-cfcf-464d-b6ca-dbf4d0b92143";
                }

                Quote oldQuote = GetQuote(quoteRequest.respondQuote.ID);
                Quote newQuote = oldQuote;

                newQuote.Status = quoteRequest.respondQuote.Status;

                _context.Quotes.Update(newQuote);

                QuoteLog log = CopyPrimaryQuoteAttributes(newQuote, "Quote Responded");

                log.Comment = quoteRequest.Comment;
                log.staffID = userID;
                _context.QuotesLog.Add(log);

                _context.SaveChanges();
                if (User != null)
                {
                    TempData["SuccessMessage"] = "Quote Responded successfully";
                    TempData["SuccessMessageStatus"] = "Success";
                }

                return RedirectToAction("List");

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e.ToString());
                TempData["SuccessMessage"] = "Quote response failed, please try again";
                TempData["SuccessMessageStatus"] = "Error";
                return View();
            }
        }


        // POST: Quote/Delete/5
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int ID)
        {
            try
            {
                // Delete quote of given ID and all relating QuoteLogs
                Quote quoteRequest = GetQuote(ID);
                QuoteLog[] quoteRequestLog = _context.QuotesLog.Where(x => x.ID == ID).ToArray();

                _context.Remove(quoteRequest);
                foreach (var logItem in quoteRequestLog)
                {
                    _context.Remove(logItem);
                }
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Quote deleted successfully";
                TempData["SuccessMessageStatus"] = "Success";

                return RedirectToAction("List");
            }
            catch
            {
                TempData["SuccessMessage"] = "Quote delete failed, Please try again.";
                TempData["SuccessMessageStatus"] = "Error";
                return View();
            }
        }

        public QuoteLog CopyPrimaryQuoteAttributes(Quote quoteRequest, string comment="", string staffID="")
        {
            // Helper function to get a copy to Quote
            QuoteLog log = new QuoteLog();
            log.quoteID = quoteRequest.ID;
            log.Postcode = quoteRequest.Postcode;
            log.ContractID = quoteRequest.ContractID;
            log.EstimatedElectricityUsage = quoteRequest.EstimatedElectricityUsage;
            log.EstimatedGasUsage = quoteRequest.EstimatedGasUsage;
            log.Created = quoteRequest.Created;
            log.EndDate = quoteRequest.EndDate;
            log.StartDate = quoteRequest.StartDate;
            log.Status = quoteRequest.Status;
            log.userID = quoteRequest.userID;
            log.SelectedContractType = quoteRequest.SelectedContractType;
            log.SelectedMeterType = quoteRequest.SelectedMeterType;
            log.SelectedPaymentFrequency= quoteRequest.SelectedPaymentFrequency;
            log.staffID = staffID;
            log.Occured = DateTime.Now;
            log.Comment = comment;

            return log;
        }

        public void ShowNotificationIfNeeded()
        {
            // Pass message to view for to notify user of completed action 
            if (TempData["SuccessMessage"] != null)
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
                ViewData["SuccessMessageStatus"] = TempData["SuccessMessageStatus"];
            }
            else
            {
                ViewData["SuccessMessage"] = "";
            }
        }
    }
}