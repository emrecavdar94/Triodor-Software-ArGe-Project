using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TriodorArgeProject.Entities.EntityClasses;
namespace TriodorArgeProject.WebUI.Models
{
    public class DashboardViewModel
    {
        public List<Personnels> Personnels { get; set; }
        public List<Projects> Projects { get; set; }
        public List<SuppProjects> SuppProjects { get; set; }
        public List<SuppProjectCosts> SuppProjectCosts { get; set; }

    }
}