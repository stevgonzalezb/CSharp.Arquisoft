using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArquisoftApp.Models.ReportsModel
{
    public class BudgetReport
    {
        public int ReportId { get; set; }
        public Users CurrentUser { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyId { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public Clients Customer { get; set; }
        public Projects Project { get; set; }
        public List<BudgetLines> BudgetLines { get; set; }
        public Budgets Budget { get; set; }

    }
}