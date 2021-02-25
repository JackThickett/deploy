using System;
using Xunit;
using SoftwareDesign.Controllers;
using SoftwareDesign.Data;
using Microsoft.EntityFrameworkCore;
using SoftwareDesign.Models;
using System.Collections.Generic;

namespace SoftwareDesign.Tests
{
    public class UnitTest1
    {
        public static int quoteID;

        [Fact]
        public void TestGetAllProspectusQuotes()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var options = optionsBuilder.UseInMemoryDatabase(databaseName: "database_name").Options;

            using (var context = new ApplicationDbContext(options))
            {
                HomeController quoteTests = new HomeController(context);

                List<Quote> prospectusQuotes = quoteTests.GetAllProspectusQuotes();

                foreach (Quote quote in prospectusQuotes)
                {
                    Assert.Equal("Prospectus", quote.Status);
                }
            }
        }

        [Fact]
        public void TestGetActiveQuotes()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var options = optionsBuilder.UseInMemoryDatabase(databaseName: "database_name").Options;

            using (var context = new ApplicationDbContext(options))
            {
                QuoteController quoteTests = new QuoteController(context);

                List<Quote> prospectusQuotes = quoteTests.GetAllActiveQuotes("3da38009-cfcf-464d-b6ca-dbf4d0b92143");

                foreach (Quote quote in prospectusQuotes)
                {
                    Assert.Equal("Active", quote.Status);
                }
            }
        }

        [Fact]
        public void TestGetQuoteHistory()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var options = optionsBuilder.UseInMemoryDatabase(databaseName: "database_name").Options;

            using (var context = new ApplicationDbContext(options))
            {
                QuoteController quoteTests = new QuoteController(context);

                int quoteID = 123;

                List<QuoteLog> prospectusQuotes = quoteTests.GetQuoteHistory(quoteID);

                foreach (QuoteLog quote in prospectusQuotes)
                {
                    Assert.Equal(quoteID, quote.quoteID);
                }
            }
        }

        [Fact]
        public void TestRespondApprove()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var options = optionsBuilder.UseInMemoryDatabase(databaseName: "database_name").Options;

            using (var context = new ApplicationDbContext(options))
            {

                Quote q = new Quote() { userID = "TestQuote"};

                context.Quotes.Add(q);
                context.SaveChanges();
                QuoteController quoteTests = new QuoteController(context);
                
                Quote quote = quoteTests.GetQuote(q.ID);
                QuoteResponseViewModel qvm = new QuoteResponseViewModel();
                qvm.respondQuote = quote;
                qvm.respondQuote.Status = "Approved";
                qvm.Comment = "Test Comment";
                quoteTests.Respond(qvm);

                Quote amendedQuote = quoteTests.GetQuote(q.ID);

                Assert.Equal("Approved", amendedQuote.Status);
            }
        }

        [Fact]
        public void TestRespondDeny()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var options = optionsBuilder.UseInMemoryDatabase(databaseName: "database_name").Options;

            using (var context = new ApplicationDbContext(options))
            {
                Quote q = new Quote() { userID = "TestQuote" };

                context.Quotes.Add(q);
                context.SaveChanges();

                QuoteController quoteTests = new QuoteController(context);
                QuoteResponseViewModel qvm = new QuoteResponseViewModel();
                qvm.respondQuote = q;
                qvm.respondQuote.Status = "Denied";
                qvm.Comment = "Test Comment";
                quoteTests.Respond(qvm);

                Quote amendedQuote = quoteTests.GetQuote(q.ID);

                Assert.Equal("Denied", amendedQuote.Status);
            }
        }

        [Fact]
        public void CreateQuote1()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var options = optionsBuilder.UseInMemoryDatabase(databaseName: "database_name").Options;

            using (var context = new ApplicationDbContext(options))
            {
                Quote q = new Quote() {
                    userID = "TestQuote",
                    Postcode = "S11 1BB",
                    SelectedContractType = "Electricity",
                    EstimatedElectricityUsage = 999,
                    SelectedMeterType = "Smart",
                    SelectedPaymentFrequency = "Monthly",
                };

                QuoteController quoteTests = new QuoteController(context);
                quoteTests.CreateStep1(q);

                Quote amendedQuote = quoteTests.GetQuote(q.ID);
                quoteID = q.ID;
                Assert.Equal("Incomplete", amendedQuote.Status);
            }
        }

        [Fact]
        public void CreateQuote2()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var options = optionsBuilder.UseInMemoryDatabase(databaseName: "database_name").Options;

            using (var context = new ApplicationDbContext(options))
            {
                Quote q = new Quote()
                {
                    userID = "TestQuote",
                    Postcode = "S11 1BB",
                    ContractID = 0,
                    SelectedContractType = "Electricity",
                    EstimatedElectricityUsage = 999,
                    SelectedMeterType = "Smart",
                    SelectedPaymentFrequency = "Monthly",
                };

                QuoteController quoteTests = new QuoteController(context);
                List<Contract> contracts = quoteTests.GetAllContracts();

                ContractViewModel cvm = new ContractViewModel() { 
                    QuoteID = quoteID,
                    ContractID = "1",
                    Contracts = contracts
                };

                quoteTests.Create2Submit(cvm);

                Quote amendedQuote = quoteTests.GetQuote(quoteID);
                QuoteLog ql = quoteTests.GetQuoteHistory(quoteID)[0];

                Assert.Equal("Prospectus", amendedQuote.Status);
                Assert.Equal("Quote Created", ql.Comment);
            }
        }
    }
}
