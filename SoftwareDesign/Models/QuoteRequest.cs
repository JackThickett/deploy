using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareDesign.Models
{
    public class Quote
    {
        public int ID { get; set; }
        public string userID { get; set; }

        [Required]
        [RegularExpression(@"^((([A-PR-UWYZ][0-9])|([A-PR-UWYZ][0-9][0-9])|([A-PR-UWYZ][A-HK-Y][0-9])|([A-PR-UWYZ][A-HK-Y][0-9][0-9])|([A-PR-UWYZ][0-9][A-HJKSTUW])|([A-PR-UWYZ][A-HK-Y][0-9][ABEHMNPRVWXY]))\s?([0-9][ABD-HJLNP-UW-Z]{2})|(GIR)\s?(0AA))$", ErrorMessage = "Postcode must be capitalised")]
        public string Postcode { get; set; }
        public DateTime Created { get; set; }
        public int ContractID { get; set; }
        public string Status { get; set; }

        [Display(Name= "Estimated Electricity Usage (KW/h)")]
        public int EstimatedElectricityUsage { get; set; }
        [Display(Name = "Estimated Gas Usage")]

        public int EstimatedGasUsage { get; set; }
        public DateTime StartDate { get; set; } // When the contract gets made active
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Contract Type")]

        public string SelectedContractType { get; set; }
        [Required]
        [Display(Name = "Meter Type")]

        public string SelectedMeterType { get; set; }
        [Required]
        [Display(Name = "Payment Frequency")]

        public string SelectedPaymentFrequency { get; set; }


        public static SelectList PopulateContractTypeDropDownList(object selectedDepartment = null)
        {
            return new SelectList(
                new List<SelectListItem>
                {
                     new SelectListItem { Selected = true, Text = "-- Select Contract Type --", Value = ""},
                     new SelectListItem { Selected = false, Text = "Electricity", Value = "Electricity"},
                     new SelectListItem { Selected = false, Text = "Gas", Value = "Gas"},
                     new SelectListItem { Selected = false, Text = "Dual Tariff", Value = "Dual Tariff"}
                }, "Value", "Text", 1);
        }

        public static SelectList PopulateMeterTypeDropDownList(object selectedDepartment = null)
        {
            return new SelectList(
                new List<SelectListItem>
                {
                     new SelectListItem { Selected = true, Text = "-- Select Your Current Meter --", Value = ""},
                     new SelectListItem { Selected = false, Text = "Standard", Value = "Standard"},
                     new SelectListItem { Selected = false, Text = "Economy", Value = "Economy"},
                     new SelectListItem { Selected = false, Text = "Smart", Value = "Smart"}
                }, "Value", "Text", 1);
        }
        public static SelectList PopulatePaymentFrequencyDropDownList(object selectedDepartment = null)
        {
            return new SelectList(
                new List<SelectListItem>
                {
                     new SelectListItem { Selected = true, Text = "-- Select Payment Frequency --", Value = ""},
                     new SelectListItem { Selected = false, Text = "Monthly", Value = "Monthly"},
                     new SelectListItem { Selected = false, Text = "Yearly", Value = "Yearly"},
                }, "Value", "Text", 1);
        }
        public static SelectList PopulateStatusDropDownList(object selectedDepartment = null)
        {
            return new SelectList(
                new List<SelectListItem>
                {
                     new SelectListItem { Selected = true, Text = "-- Update Status --", Value = ""},
                     new SelectListItem { Selected = false, Text = "Approved", Value = "Approved"},
                     new SelectListItem { Selected = false, Text = "Denied", Value = "Denied"}
                }, "Value", "Text", 1);
        }

        public static SelectList PopulateContractDropDownList(object selectedDepartment = null)
        {
            return new SelectList(
                new List<SelectListItem>
                {
                     new SelectListItem { Selected = true, Text = "-- Select Tariff --", Value = ""},
                     new SelectListItem { Selected = false, Text = "12 Month Fixed", Value = "2"},
                     new SelectListItem { Selected = false, Text = "24 Month Fixed", Value = "3"},
                     new SelectListItem { Selected = false, Text = "Flexible Rates", Value = "4"},
                     new SelectListItem { Selected = false, Text = "Flexible Green", Value = "6"}
                }, "Value", "Text", 1);
        }

    }

    public class ContractViewModel
    {
        public List<Contract> Contracts { get; set; }
        public string ContractType { get; set; }
        public int QuoteID { get; set; }
        [Required]
        public string ContractID { get; set; }
        public string PaymentFrequency { get; set; }
        public int EstimatedGasUsage { get; set; }
        public int EstimatedElectricityUsage { get; set; }

    }


    public class HomepageViewModel
    {
        public List<Quote> quoteList { get; set; }
        public List<Contract> Contracts { get; set; }
        public int countProspectus { get; set; }
        public string username { get; set; }
    }
    
    public class QuoteResponseViewModel
    {
        public Quote respondQuote { get; set; }
        [Required]
        public string Comment { get; set; }
        
    }

    public class QuoteViewModel
    {
        public Quote quote { get; set;}

        [Display(Name = "Tariff Type")]
        public string contractName { get; set; }

        [Display(Name = "Next Payment")]
        public string nextPayment { get; set; }

        [Display(Name = "Next Payment Date")]
        public string nextPaymentDate { get; set; }
    }


    public class Contract
    {
        public int ID { get; set; }
        public float GasRate { get; set; }
        public float ElectricityRate { get; set; }
        public string ContractName { get; set; }
        public string Description { get; set; }
    }

    public class QuoteLog
    {
        public int ID { get; set; }
        public string userID { get; set; }
        public int quoteID { get; set; }
        public string Postcode { get; set; }
        public DateTime Created { get; set; }
        public int ContractID { get; set; }
        public string Status { get; set; }
        public int EstimatedElectricityUsage { get; set; }
        public int EstimatedGasUsage { get; set; }
        public DateTime StartDate { get; set; } // When the contract gets made active
        public DateTime EndDate { get; set; }
        public string SelectedContractType { get; set; }
        public string SelectedMeterType { get; set; }
        public string SelectedPaymentFrequency { get; set; }
        public string Comment { get; set; }
        public string staffID { get; set; }
        public DateTime Occured { get; set; }
    }

    public class QuoteLogViewModel
    {
        public QuoteLog quoteLog { get; set; }

        [Display(Name = "Tariff Type")]
        public string contractName { get; set; }

        [Display(Name = "Next Payment")]
        public string nextPayment { get; set; }

        [Display(Name = "Next Payment Date")]
        public string nextPaymentDate { get; set; }

    }
}
