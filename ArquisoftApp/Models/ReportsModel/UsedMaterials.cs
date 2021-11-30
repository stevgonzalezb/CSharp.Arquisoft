using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArquisoftApp.Models.ReportsModel
{
    public class UsedMaterials
    {
        public Users CurrentUser { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyId { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public string SelectedDates { get; set; }
        public List<string> Materials { get; set; }
    }
}