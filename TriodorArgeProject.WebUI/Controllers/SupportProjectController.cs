using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TriodorArgeProject.Entities;
using TriodorArgeProject.Entities.EntityClasses;
using TriodorArgeProject.Service.Managers;
using TriodorArgeProject.WebUI.Models;

namespace TriodorArgeProject.WebUI.Controllers
{
    public class SupportProjectController : Controller
    {
        private SupportProjectManager suppProjectManager = new SupportProjectManager();
        private static SuppProjects globalSupportProject = new SuppProjects();
        private SupportProjectsCostManager supportProjectsCostManager = new SupportProjectsCostManager();
        private SupportProjectPersonnelsManager suppProjectPersonnelsManager = new SupportProjectPersonnelsManager();
        private FundingSchemesManager fundingSchemesManager = new FundingSchemesManager();
        private SuppProjectsStatusManager projectsStatusManager = new SuppProjectsStatusManager();
        private PersonnelManager personnelManager = new PersonnelManager();
        private SupportProjectPersonnelsManager supportProjectPersonnelsManager = new SupportProjectPersonnelsManager();
        private ArgeCostManager argeCostManager = new ArgeCostManager();
        private EquipmentCostManager equipmentCostManager = new EquipmentCostManager();
        private MaterialCostManager materialCostManager = new MaterialCostManager();
        private PersonnelCostManager personnelCostManager = new PersonnelCostManager();
        private ServiceCostManager serviceCostManager = new ServiceCostManager();
        private TravelCostManager travelCostManager = new TravelCostManager();
        private SuppProjectCosts suppProjectCostGlb;
        private string year;
        private string period;
        private string[] splitData;
        // GET: SupportProject
        public ActionResult Index()
        {

            return View(suppProjectManager.List());
        }
        public ActionResult Create()
        {
            
            ViewBag.FUNDINGSCHEMES = fundingSchemesManager.List();
            ViewBag.STATUS = projectsStatusManager.List();
            return View(); 
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(SuppProjects suppProject/*,decimal Planned,decimal Actual,decimal Accepted*/)
        {
            if (ModelState.IsValid)
            {
                SuppProjectCosts suppProjectCosts = new SuppProjectCosts();
                //suppProjectCosts.Planned = Planned;
                //suppProjectCosts.Actual = Actual;
                //suppProjectCosts.Accepted = Accepted;
                //suppProjectCosts.Total = 0;
                //suppProject.SuppProjectCosts = suppProjectCosts;
                var res=suppProjectManager.Insert(suppProject);
                if (res>0)
                {
                    TempData["mesaj"] = "Created.";
                    return RedirectToAction("Index");
                    
                }

            }
            ViewBag.FUNDINGSCHEMES = fundingSchemesManager.List();
            ViewBag.STATUS = projectsStatusManager.List();
            TempData["mesaj"] = "Error. Please check fields.";
            return View(suppProject);
        }


        public ActionResult Detail(int? id,string yearperioddata)
        {
            double years;
            List<YearPeriodModel> yearPeriodModels = new List<YearPeriodModel>();
            YearPeriodModel yearPeriod = new YearPeriodModel();
            globalSupportProject = suppProjectManager.Find(x => x.ID == id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (yearperioddata!=null)
            {
                splitData = yearperioddata.Split('/');
                int i = 0;
                foreach (var item in splitData)
                {
                    if (i==0)
                    {
                        year = item.Trim();
                        i++;
                    }
                    else if(i==1){
                        period = item.Trim();
                        i++;
                    }
                    
                  
                }

            }


            SuppProjects projects = suppProjectManager.Find(x => x.ID == id);
            

         
            
            DateTime startDate = projects.SignedDate.Value;
            DateTime endDate = projects.SuppEndDate.Value;
            if (endDate!=null)
            {
                var timeSpan = (endDate - startDate);
                 years = timeSpan.TotalDays / 365;
            }
            else
            {
                endDate = DateTime.Now;
                var timeSpan = endDate - startDate;
                years = timeSpan.TotalDays / 365;
            }
            while (startDate<=endDate)
            {
               
                    if (startDate.Month < 7)
                    {
                        yearPeriod.Period = "1";
                        yearPeriod.Year = startDate.Year.ToString();
                        yearPeriod.YearPeriod = (startDate.Year.ToString() + "/" + " 1");
                        yearPeriodModels.Add(yearPeriod);
                    yearPeriod = new YearPeriodModel();

                    }
                    else if (startDate.Month > 6)
                    {
                        yearPeriod.Period = "2";
                        yearPeriod.Year = startDate.Year.ToString();
                        yearPeriod.YearPeriod = (startDate.Year.ToString() + "/" + " 2");
                        yearPeriodModels.Add(yearPeriod);
                    yearPeriod = new YearPeriodModel();
                }

                
                startDate =startDate.AddMonths(6).AddDays(-1);
            }
            ViewBag.YEARPERIOD = yearPeriodModels;
            ViewBag.SELECTEDPERIOD = yearperioddata;
            if (year != null && period != null)
            {
                SuppProjects tempProject = new SuppProjects();
                tempProject.SuppProjectCosts.Clear();
                tempProject.AppealDate = projects.AppealDate;
                tempProject.Budget = projects.Budget;
                tempProject.FundingSchemes = projects.FundingSchemes;
                tempProject.FundingSchemesID = projects.FundingSchemesID;
                tempProject.ID = projects.ID;
                tempProject.PlannedManMonth = projects.PlannedManMonth;
                tempProject.ProjectNo = projects.ProjectNo;
                tempProject.SignedDate = projects.SignedDate;
                tempProject.StatusID = projects.StatusID;
                tempProject.SuppEndDate = projects.SuppEndDate;
                tempProject.SupportRate = projects.SupportRate;
                tempProject.SuppProjectPersonnels = projects.SuppProjectPersonnels;
                tempProject.SuppProjectsStatus = projects.SuppProjectsStatus;
                tempProject.SuppStartDate = projects.SuppStartDate;

                if (projects.SuppProjectCosts != null)
                {
                    foreach (var cost in projects.SuppProjectCosts.ToList())
                    {
                        if (cost.Year == year && cost.Period == period)
                        {
                            tempProject.SuppProjectCosts.Add(cost);

                        }
                    }
                }
                return View(tempProject);
            }
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }
      
       
        public ActionResult AddPersonnel(int id)
        {
            List<PersonnelModels> personnelModels = new List<PersonnelModels>();
            List<Personnels> personnels = personnelManager.List();
            PersonnelModels personnelModel;
            if (personnels != null)
            {
                foreach (var item in personnels)
                {
                    personnelModel = new PersonnelModels();
                    personnelModel.ID = item.Id;
                    personnelModel.Name = item.Name + " " + item.Surname;
                    personnelModels.Add(personnelModel);
                }
            }
            ViewBag.PERSONNELS = personnelModels;
            TempData["SuppProjectID"] = id;
       
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddPersonnel(int? PersonnelID,string position,int projectID)
        {
            
            if (PersonnelID==null ||position==null || projectID <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnels personnels = personnelManager.Find(x => x.Id == PersonnelID);
            if (personnels == null) 
            {
                return HttpNotFound();
            }
            SuppProjectPersonnels suppProjectPersonnels ;
            suppProjectPersonnels = suppProjectPersonnelsManager.Find(x => x.Personnels.Id == PersonnelID&& x.SuppProjects.ID==projectID);
            if (suppProjectPersonnels!=null)
            {
                ViewBag.MESAJ = "Personnel already available";
                return View(suppProjectPersonnels);
            }
            suppProjectPersonnels = new SuppProjectPersonnels();
            suppProjectPersonnels.Personnels = personnels;
            suppProjectPersonnels.Position = position;
            suppProjectPersonnels.SuppProjectD = projectID;
            var res = supportProjectPersonnelsManager.Insert(suppProjectPersonnels);
            if (res>0)
            {
                TempData["mesaj"] = "Added";
               
            }
            else
            {
                ViewBag.MESAJ = "Error. Please check fields.";
                return View(suppProjectPersonnels);
            }
            return RedirectToAction("Detail",new { id=projectID});
        }
       
        public ActionResult DeletePersonnel(int? id)
        {
            if (id==null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            SuppProjectPersonnels personnel = supportProjectPersonnelsManager.Find(x => x.Personnels.Id == id);
            if (personnel==null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            var res= supportProjectPersonnelsManager.Delete(personnel);
            if (res>0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult Edit(int? id)
        {
            ViewBag.FUNDINGSCHEMES = fundingSchemesManager.List();
            ViewBag.STATUS = projectsStatusManager.List();
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuppProjects suppProject = suppProjectManager.Find(x => x.ID == id);
            if (suppProject==null)
            {
                return HttpNotFound();
            }
            return View(suppProject);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? projectID,string ProjectNo,int FundingSchemesID,int StatusID ,DateTime AppealDate,DateTime SignedDate,DateTime SuppStartDate,DateTime SuppEndDate,string PlannedManMonth,decimal Budget,int SupportRate)
        {
            SuppProjects suppPr = new SuppProjects();
            if (ModelState.IsValid)
            {
               
                if (projectID==null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                suppPr = suppProjectManager.Find(x => x.ID == projectID);
                if (ProjectNo!=null)
                {
                    suppPr.ProjectNo = ProjectNo;
                }
                if (FundingSchemesID >0)
                {
                    suppPr.FundingSchemesID = FundingSchemesID;
                }
                if (StatusID>0)
                {
                    suppPr.StatusID = StatusID;
                }
                if (AppealDate!=null)
                {
                    suppPr.AppealDate = AppealDate;
                }
                if (SignedDate!=null)
                {
                    suppPr.SignedDate = SignedDate;
                }
                if (SuppStartDate!=null)
                {
                    suppPr.SuppStartDate = SuppStartDate;
                }
                if (SuppEndDate!=null)
                {
                    suppPr.SuppEndDate = SuppEndDate;
                }
                if (PlannedManMonth!=null)
                {
                    suppPr.PlannedManMonth = PlannedManMonth;
                }
                if (Budget>0)
                {
                    suppPr.Budget = Budget;
                }
                if (SupportRate>0)
                {
                    suppPr.SupportRate = SupportRate;
                }
                
                var res = suppProjectManager.Update(suppPr);
                if (res>0)
                {
                    ViewBag.MESAJ = "Updated";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.MESAJ = "Error. Please check fields.";
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                }
            }
            ViewBag.MESAJ = "Server Error.";
            return View(suppPr);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            SuppProjects suppProject = suppProjectManager.Find(x => x.ID == id);
            if (suppProject == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            var res = suppProjectManager.Delete(suppProject);
            if (res > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult AddCost(int? id)
        {
            SuppProjects projects = suppProjectManager.Find(x => x.ID == id);
            return View(projects);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddCost(int? id,int actual,int planned,int accepted,string year,string period)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuppProjects projects = suppProjectManager.Find(x => x.ID == id);
            if (projects==null)
            {
                return HttpNotFound();
            }

            suppProjectCostGlb = supportProjectsCostManager.Find(x => x.ProjectID == id && x.Year == year && x.Period == period);
            if (suppProjectCostGlb==null)
            {
                SuppProjectCosts projectCosts = new SuppProjectCosts();
                projectCosts.Accepted = accepted;
                projectCosts.Actual = actual;
                projectCosts.Planned = planned;
                projectCosts.ProjectID = id;
                
                projectCosts.Year = year;
                projectCosts.Period=period;
                
                var res = supportProjectsCostManager.Insert(projectCosts);
               
                if (res>0)
                {
                    ViewBag.COSTMESSAGE = "Cost Added";
                    return View(projects);
                }
                else
                {
                    ViewBag.COSTMESSAGE = "Error";
                    return View(projects);
                }
            }
            else
            {
                suppProjectCostGlb.Actual = actual;
                suppProjectCostGlb.Accepted = accepted;
                suppProjectCostGlb.Planned = planned;
               var res= supportProjectsCostManager.Update(suppProjectCostGlb);
                if (res > 0)
                {
                    ViewBag.COSTMESSAGE = "Cost Already Exist.Updated.";
                    return View(projects);
                }
                else
                {
                    ViewBag.COSTMESSAGE = "Error";
                    return View(projects);
                }
            }
        }
        public ActionResult AddArgeCost(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData["suppProjectID"] = id;
            return View();
        }
        public ActionResult EditArgeCost(int? id)
        {
            ArgeCost argeCost = argeCostManager.Find(x => x.ID == id);
            if (argeCost==null)
            {
                return HttpNotFound();
            }
            return View(argeCost);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditArgeCost(ArgeCost argeCost)
        {
            ArgeCost argeCostNW = argeCostManager.Find(model => model.ID == argeCost.ID);
            argeCostNW.Accepted = argeCost.Accepted;
            argeCostNW.Actual = argeCost.Actual;
            argeCostNW.Description = argeCost.Description;
            argeCostNW.Notes = argeCost.Description;
            argeCostNW.Planned = argeCost.Planned;
            
            SuppProjectCosts costs = supportProjectsCostManager.Find(x=>x.ID==argeCost.CostID);
            //argeCostNW.SuppProjectCosts = costs;
            //argeCost.SuppProjectCosts.Accepted = costs.Accepted;
            //argeCost.SuppProjectCosts.Actual = costs.Actual;
            //argeCost.SuppProjectCosts.EquipmentCosts = costs.EquipmentCosts;
            //argeCost.SuppProjectCosts.ID = costs.ID;
            //argeCost.SuppProjectCosts.MaterialCosts = costs.MaterialCosts;
            //argeCost.SuppProjectCosts.Period = costs.Period;
            //argeCost.SuppProjectCosts.Year = costs.Year;
            //argeCost.SuppProjectCosts.PersonnelCosts = costs.PersonnelCosts;
            //argeCost.SuppProjectCosts.Planned = costs.Planned;
            //argeCost.SuppProjectCosts.ProjectID = costs.ProjectID;
            //argeCost.SuppProjectCosts.ServiceCosts = costs.ServiceCosts;
            //argeCost.SuppProjectCosts.SuppProjects = costs.SuppProjects;
            //argeCost.SuppProjectCosts.TravelCosts = costs.TravelCosts;
            //argeCost.SuppProjectCosts.ArgeCost = costs.ArgeCost;
            

            if (argeCost!=null)
            {
                if (argeCost.ID <=0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var res = argeCostManager.Update(argeCostNW);
                // ModelState.
                //argeCostNW = new ArgeCost();
                //argeCostNW.Accepted = argeCost.Accepted;
                //argeCostNW.Actual = argeCost.Actual;
                //argeCostNW.CostID = argeCost.CostID;
               
                //argeCostNW.Description = argeCost.Description;
                //argeCostNW.ID = argeCost.ID;
                //argeCostNW.Notes = argeCost.Notes;
                //argeCostNW.Planned = argeCost.Planned;
                //var res = argeCostManager.Update(argeCostNW);
                //try
                //{
                //    argeCostManager.Update(argeCost);
                //}
                //catch (Exception e)
                //{

                    
                //}
                if (res>0)
                {
                    ViewBag.ARGEMESSAGE = "Updated";
                }
                ViewBag.ARGEMESSAGE = "Error.";
                    
            }
            return View(argeCost);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddArgeCost(ArgeCost argeCost,string Year,string Period)
        {
            int? projectID = (int)TempData["suppProjectID"];

            if (ModelState.IsValid)
            {


                if (projectID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SuppProjectCosts suppProjectCost = supportProjectsCostManager.Find(x => x.ProjectID == projectID && x.Year == Year && x.Period == Period);
                if (suppProjectCost == null)
                {
                    suppProjectCost = new SuppProjectCosts();

                    suppProjectCost.ProjectID = projectID;
                    //suppProjectCost.Total = 0;
                    suppProjectCost.Year = Year;

                    suppProjectCost.Period = Period;
                    var res = supportProjectsCostManager.Insert(suppProjectCost);

                    if (res > 0)
                    {
                        argeCost.CostID = suppProjectCost.ID;
                        suppProjectCost.ArgeCost.Add(argeCost);
                        //suppProjectCost.Total = materialCosts.Actual + suppProjectCost.Total;
                        supportProjectsCostManager.Update(suppProjectCost);
                        return RedirectToAction("Detail", new { id = projectID });
                    }

                }
                else
                {

                    argeCost.CostID = suppProjectCost.ID;

                    suppProjectCost.ArgeCost.Add(argeCost);
                    //suppProjectCost.Total = materialCosts.Actual + suppProjectCost.Total;
                    supportProjectsCostManager.Update(suppProjectCost);
                    return RedirectToAction("Detail", new { id = projectID });
                }

            }
            return View(argeCost);
        }
        public ActionResult AddTravelCost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData["suppProjectID"] = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTravelCost(TravelCosts travelCosts,string Year,string Period)
        {
            int? projectID = (int)TempData["suppProjectID"];

            if (ModelState.IsValid)
            {


                if (projectID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SuppProjectCosts suppProjectCost = supportProjectsCostManager.Find(x => x.ProjectID == projectID && x.Year == Year && x.Period == Period);
                if (suppProjectCost == null)
                {
                    suppProjectCost = new SuppProjectCosts();

                    suppProjectCost.ProjectID = projectID;
                    //suppProjectCost.Total = 0;
                    suppProjectCost.Year = Year;

                    suppProjectCost.Period = Period;
                    var res = supportProjectsCostManager.Insert(suppProjectCost);

                    if (res > 0)
                    {
                        travelCosts.CostID = suppProjectCost.ID;
                        suppProjectCost.TravelCosts.Add(travelCosts);
                        //suppProjectCost.Total = materialCosts.Actual + suppProjectCost.Total;
                        supportProjectsCostManager.Update(suppProjectCost);
                        return RedirectToAction("Detail", new { id = projectID });
                    }

                }
                else
                {

                    travelCosts.CostID = suppProjectCost.ID;

                    suppProjectCost.TravelCosts.Add(travelCosts);
                    //suppProjectCost.Total = materialCosts.Actual + suppProjectCost.Total;
                    supportProjectsCostManager.Update(suppProjectCost);
                    return RedirectToAction("Detail", new { id = projectID });
                }

            }
            return View(travelCosts);
        }
        public ActionResult EditTravelCost(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            TravelCosts travelCosts = travelCostManager.Find(x => x.ID == id);
            if (travelCosts==null)
            {
                return HttpNotFound();
            }
            return View(travelCosts);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditTravelCost(TravelCosts travelCosts)
        {
            return View();
        }


        public ActionResult AddServiceCost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData["suppProjectID"] = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddServiceCost(ServiceCosts serviceCosts, string Year,string Period)
        {
            int? projectID = (int)TempData["suppProjectID"];

            if (ModelState.IsValid)
            {


                if (projectID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SuppProjectCosts suppProjectCost = supportProjectsCostManager.Find(x => x.ProjectID == projectID&&x.Year==Year&&x.Period==Period);
                if (suppProjectCost == null)
                {
                    suppProjectCost = new SuppProjectCosts();
                   
                    suppProjectCost.ProjectID = projectID;
                    //suppProjectCost.Total = 0;
                    suppProjectCost.Year = Year;
                  
                    suppProjectCost.Period = Period;
                    var res = supportProjectsCostManager.Insert(suppProjectCost);

                    if (res > 0)
                    {
                        serviceCosts.CostID = suppProjectCost.ID;
                        suppProjectCost.ServiceCosts.Add(serviceCosts);
                        //suppProjectCost.Total = serviceCosts.Actual + suppProjectCost.Total;
                        supportProjectsCostManager.Update(suppProjectCost);
                        return RedirectToAction("Detail", new { id = projectID });
                    }

                }
                else
                {

                    serviceCosts.CostID = suppProjectCost.ID;

                    suppProjectCost.ServiceCosts.Add(serviceCosts);
                //suppProjectCost.Total = serviceCosts.Actual + suppProjectCost.Total;
                supportProjectsCostManager.Update(suppProjectCost);
                return RedirectToAction("Detail", new { id = projectID });
                }
                
            }
            return View(serviceCosts);
        }
        public ActionResult EditServiceCost(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceCosts serviceCosts = serviceCostManager.Find(x => x.ID == id);
            if (serviceCosts==null)
            {
                return HttpNotFound();
            }
            return View(serviceCosts);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditServiceCost(ServiceCosts serviceCosts)
        {
            return View();
        }
        public ActionResult AddMaterialCost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData["suppProjectID"] = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMaterialCost(MaterialCosts materialCosts,string Year,string Period)
        {
            int? projectID = (int)TempData["suppProjectID"];

            if (ModelState.IsValid)
            {


                if (projectID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SuppProjectCosts suppProjectCost = supportProjectsCostManager.Find(x => x.ProjectID == projectID && x.Year == Year && x.Period == Period);
                if (suppProjectCost == null)
                {
                    suppProjectCost = new SuppProjectCosts();

                    suppProjectCost.ProjectID = projectID;
                    //suppProjectCost.Total = 0;
                    suppProjectCost.Year = Year;

                    suppProjectCost.Period = Period;
                    var res = supportProjectsCostManager.Insert(suppProjectCost);

                    if (res > 0)
                    {
                        materialCosts.CostID = suppProjectCost.ID;
                        suppProjectCost.MaterialCosts.Add(materialCosts);
                        //suppProjectCost.Total = materialCosts.Actual + suppProjectCost.Total;
                        supportProjectsCostManager.Update(suppProjectCost);
                        return RedirectToAction("Detail", new { id = projectID });
                    }

                }
                else
                {

                    materialCosts.CostID = suppProjectCost.ID;

                    suppProjectCost.MaterialCosts.Add(materialCosts);
                    //suppProjectCost.Total = materialCosts.Actual + suppProjectCost.Total;
                    supportProjectsCostManager.Update(suppProjectCost);
                    return RedirectToAction("Detail", new { id = projectID });
                }

            }
            return View(materialCosts);
        }
        public ActionResult EditMaterialCost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialCosts materialCosts = materialCostManager.Find(x => x.ID == id);
            if (materialCosts == null)
            {
                return HttpNotFound();
            }
            return View(materialCosts);
        }
        public ActionResult EditMaterialCost(MaterialCosts materialCosts)
        {
            return View();
        }
        public ActionResult AddEquipmentCost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData["suppProjectID"] = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEquipmentCost(EquipmentCosts equipmentCosts,string Year,string Period)
        {
            int? projectID = (int)TempData["suppProjectID"];

            if (ModelState.IsValid)
            {


                if (projectID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SuppProjectCosts suppProjectCost = supportProjectsCostManager.Find(x => x.ProjectID == projectID && x.Year == Year && x.Period == Period);
                if (suppProjectCost == null)
                {
                    suppProjectCost = new SuppProjectCosts();

                    suppProjectCost.ProjectID = projectID;
                    //suppProjectCost.Total = 0;
                    suppProjectCost.Year = Year;

                    suppProjectCost.Period = Period;
                    var res = supportProjectsCostManager.Insert(suppProjectCost);

                    if (res > 0)
                    {
                        equipmentCosts.CostID = suppProjectCost.ID;
                        suppProjectCost.EquipmentCosts.Add(equipmentCosts);
                        //suppProjectCost.Total = serviceCosts.Actual + suppProjectCost.Total;
                        supportProjectsCostManager.Update(suppProjectCost);
                        return RedirectToAction("Detail", new { id = projectID });
                    }

                }
                else
                {

                    equipmentCosts.CostID = suppProjectCost.ID;

                    suppProjectCost.EquipmentCosts.Add(equipmentCosts);
                    //suppProjectCost.Total = serviceCosts.Actual + suppProjectCost.Total;
                    supportProjectsCostManager.Update(suppProjectCost);
                    return RedirectToAction("Detail", new { id = projectID });
                }

            }
            return View(equipmentCosts);
        }
        public ActionResult AddPersonnelCost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData["suppProjectID"] = id;
            return View();
        }
        public ActionResult EditEquipmentCost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentCosts equipmentCosts = equipmentCostManager.Find(x => x.ID == id);
            if (equipmentCosts == null)
            {
                return HttpNotFound();
            }
            return View(equipmentCosts);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditEquipmentCost(EquipmentCosts equipmentCosts)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPersonnelCost(PersonnelCosts personnelCosts,string Year,string Period)
        {
            int? projectID = (int)TempData["suppProjectID"];

            if (ModelState.IsValid)
            {


                if (projectID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SuppProjectCosts suppProjectCost = supportProjectsCostManager.Find(x => x.ProjectID == projectID && x.Year == Year && x.Period == Period);
                if (suppProjectCost == null)
                {
                    suppProjectCost = new SuppProjectCosts();

                    suppProjectCost.ProjectID = projectID;
                    //suppProjectCost.Total = 0;
                    suppProjectCost.Year = Year;

                    suppProjectCost.Period = Period;
                    var res = supportProjectsCostManager.Insert(suppProjectCost);

                    if (res > 0)
                    {
                        personnelCosts.CostID = suppProjectCost.ID;
                        suppProjectCost.PersonnelCosts.Add(personnelCosts);
                        //suppProjectCost.Total = serviceCosts.Actual + suppProjectCost.Total;
                        supportProjectsCostManager.Update(suppProjectCost);
                        return RedirectToAction("Detail", new { id = projectID });
                    }

                }
                else
                {

                    personnelCosts.CostID = suppProjectCost.ID;

                    suppProjectCost.PersonnelCosts.Add(personnelCosts);
                    //suppProjectCost.Total = serviceCosts.Actual + suppProjectCost.Total;
                    supportProjectsCostManager.Update(suppProjectCost);
                    return RedirectToAction("Detail", new { id = projectID });
                }

            }
            return View(personnelCosts);
        }
        public ActionResult EditPersonnelCost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonnelCosts personnelCosts = personnelCostManager.Find(x => x.ID == id);
            if (personnelCosts== null)
            {
                return HttpNotFound();
            }
            return View(personnelCosts);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditPersonnelCost(PersonnelCosts personnelCosts)
        {
            return View();
        }
        public ActionResult DeletePersonnelCost(int? id)
        {
            if (id == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            PersonnelCosts personnelCost = personnelCostManager.Find(x => x.ID == id);
            if (personnelCost == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            var res = personnelCostManager.Delete(personnelCost);
            if (res > 0)
            {
               
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DeleteArgeCost(int? id)
        {
            if (id == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            ArgeCost argeCost = argeCostManager.Find(x => x.ID == id);
            if (argeCost == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            var res = argeCostManager.Delete(argeCost);
            if (res > 0)
            {
                SuppProjectCosts suppProjectCost = supportProjectsCostManager.Find(x => x.ID == argeCost.CostID);
                //suppProjectCost.Total = suppProjectCost.Total - argeCost.Actual;
                supportProjectsCostManager.Update(suppProjectCost);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DeleteEquipmentCost(int? id)
        {
            if (id == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            EquipmentCosts equipmentCosts = equipmentCostManager.Find(x => x.ID == id);
            if (equipmentCosts == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            var res = equipmentCostManager.Delete(equipmentCosts);
            if (res > 0)
            {
                SuppProjectCosts suppProjectCost = supportProjectsCostManager.Find(x => x.ID == equipmentCosts.CostID);
                //suppProjectCost.Total = suppProjectCost.Total - equipmentCosts.Actual;
                supportProjectsCostManager.Update(suppProjectCost);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DeleteMaterialCost(int? id)
        {
            if (id == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            MaterialCosts materialCosts = materialCostManager.Find(x => x.ID == id);
            if (materialCosts == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            var res = materialCostManager.Delete(materialCosts);
            if (res > 0)
            {
                SuppProjectCosts suppProjectCost = supportProjectsCostManager.Find(x => x.ID == materialCosts.CostID);
                //suppProjectCost.Total = suppProjectCost.Total - materialCosts.Actual;
                supportProjectsCostManager.Update(suppProjectCost);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                
            }
            else
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DeleteServiceCost(int? id)
        {
            if (id == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            ServiceCosts serviceCosts = serviceCostManager.Find(x => x.ID == id);
            if (serviceCosts == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            var costID = serviceCosts.CostID;
            var res = serviceCostManager.Delete(serviceCosts);
            if (res > 0)
            {
                SuppProjectCosts suppProjectCost = supportProjectsCostManager.Find(x => x.ID == costID);
                //suppProjectCost.Total = suppProjectCost.Total - serviceCosts.Actual;
                supportProjectsCostManager.Update(suppProjectCost);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DeleteTravelCost(int? id)
        {
            if (id == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            TravelCosts travelCosts = travelCostManager.Find(x => x.ID == id);
            if (travelCosts == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
            var res = travelCostManager.Delete(travelCosts);
            if (res > 0)
            {
                SuppProjectCosts suppProjectCost = supportProjectsCostManager.Find(x => x.ID == travelCosts.CostID);
                //suppProjectCost.Total = suppProjectCost.Total - travelCosts.Actual;
                supportProjectsCostManager.Update(suppProjectCost);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}