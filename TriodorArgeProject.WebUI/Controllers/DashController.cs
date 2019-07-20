using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TriodorArgeProject.Entities.EntityClasses;
using TriodorArgeProject.Service.Managers;
using TriodorArgeProject.WebUI.Models;

namespace TriodorArgeProject.WebUI.Controllers
{
    public class DashController : Controller
    {
        private ProjectManager projectManager = new ProjectManager();
        private PersonnelManager personnelManager = new PersonnelManager();
        private SupportProjectManager supportProjectManager = new SupportProjectManager();
        private SupportProjectsCostManager supportProjectCostManager = new SupportProjectsCostManager();
        DashboardViewModel dashboardViewModel = new DashboardViewModel();
        // GET: Dash
        public ActionResult Dashboard()
        {
            dashboardViewModel.Projects = projectManager.List();
            dashboardViewModel.SuppProjectCosts = supportProjectCostManager.List();
            dashboardViewModel.SuppProjects = supportProjectManager.List();
            dashboardViewModel.Personnels = personnelManager.List();
            return View(dashboardViewModel);
        }
    }
}