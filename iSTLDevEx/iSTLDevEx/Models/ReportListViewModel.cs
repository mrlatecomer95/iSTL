using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iSTLDevEx.Models
{
    public class ReportListViewModel
    {
        public int ID { get; set; }
        public string ReportName { get; set; }
    }

    public class ReportIndexViewModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateFilter { get; set; }
        [Display(Name = "Agent")]
        public string AgentID { get; set; }
        public List<SelectListItem> AgentList { get; set; }
    }

    public class ReportItems
    {
        public string AgentID { get; set; }
        public string DateRec { get; set; }
        public string DrawResult { get; set; }
        public string ForDraw { get; set; }
    }


    public class sp_rptAgentViewModel
    {
        public string AgentName { get; set; }
    }
}