using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TriodorArgeProject.Entities;
using TriodorArgeProject.Entities.EntityClasses;
using TriodorArgeProject.Service.Managers;

namespace TriodorArgeProject.WebUI.Controllers
{
    public class ProjectController : Controller
    {
        private int? maxBCode;
        private int? maxDCode;
        private int? maxPCode;
        private ProjectManager projectManager = new ProjectManager();
        private Projects projects;
        private PersonnelManager personnelManager = new PersonnelManager();
        private PersonnelProjectManager personnelProjectManager = new PersonnelProjectManager(); 
        private Personnels personnel = new Personnels();
        private ICollection<PersonnelProjectsTable> personnelProjectsList = new List<PersonnelProjectsTable>();
        private List<Projects> projectList = new List<Projects>();
        private PersonnelProjectsTable nwPersonnelPr;
        // GET: Project
        public ActionResult Index()
        {
            return View(projectManager.List());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PCode,DCode,BCode,Name,StartDate,EndDate,Cost")] Projects projects)
        {
            
            projectList = projectManager.List();
            foreach (var item in projectList)
            {
                maxBCode =0;
                maxDCode =0;
                maxPCode = 0;
                if (item.BCode>maxBCode)
                {
                    maxBCode = item.BCode;
                }
               
                if (item.DCode > maxDCode)
                {
                    maxDCode = item.DCode;
                }
                if (item.PCode > maxPCode)
                {
                    maxPCode = item.PCode;
                }
            }
            if (ModelState.IsValid)
            {
                if (projects.EndDate<DateTime.Now)
                {
                    maxBCode = maxBCode + 1;
                    projects.BCode = maxBCode;
                    projects.DCode = 0;
                    projects.PCode = 0;
                }
                else if(projects.StartDate>DateTime.Now)
                {
                    maxPCode = maxPCode + 1;
                    projects.PCode = maxPCode;
                    projects.DCode = 0;
                    projects.BCode = 0;

                }
                else if (projects.StartDate<DateTime.Now && projects.EndDate>DateTime.Now)
                {
                    maxDCode = maxDCode + 1;
                    projects.DCode = maxDCode;
                    projects.BCode = 0;
                    projects.PCode = 0;
                }
                projectManager.Insert(projects);
                return RedirectToAction("Index");
            }

            return View(projects);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = projectManager.Find(x=>x.ID==id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Projects projects)
            
        {
            Projects nwProjects = projectManager.Find(x => x.ID == projects.ID);
            maxBCode = 0;
            maxDCode = 0;
            maxPCode = 0;
            projectList = projectManager.List();
            foreach (var item in projectList)
            {
                
                if (item.BCode>maxBCode)
                {
                    maxBCode = item.BCode;
                }
               
                if (item.DCode > maxDCode)
                {
                    maxDCode = item.DCode;
                }
                if (item.PCode > maxPCode)
                {
                    maxPCode = item.PCode;
                }
            }
            if (ModelState.IsValid)
            {
                nwProjects.Name = projects.Name;
                nwProjects.StartDate = projects.StartDate;
                nwProjects.EndDate = projects.EndDate;
                nwProjects.Cost = projects.Cost;
                if (projects.EndDate < DateTime.Now &&projects.BCode<=0)
                {
                    maxBCode = maxBCode + 1;
                    nwProjects.BCode = maxBCode;
                }
                else if (projects.StartDate > DateTime.Now && projects.PCode <= 0)
                {
                    maxPCode = maxPCode + 1;
                    nwProjects.PCode = maxPCode;

                }
                else if (projects.StartDate < DateTime.Now && projects.EndDate > DateTime.Now && projects.DCode <= 0)
                {
                    maxDCode = maxDCode + 1;
                    nwProjects.DCode = maxDCode;
                }
                int res = projectManager.Update(nwProjects);
                return RedirectToAction("Index");
            }
            return View(projects);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = projectManager.Find(x=>x.ID==id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projects projects = projectManager.Find(x => x.ID == id);
            projectManager.Delete(projects);
            return RedirectToAction("Index");
        }
        public ActionResult ProjectList(int? projectID, string year, string month)
        {
            if (TempData["mesaj"]!=null)
            {
                TempData["mesaj"] = TempData["mesaj"];
            }
           
            if (projectID!=null&&year!=null&&month!=null)
            {
                personnelProjectsList = personnelProjectManager.List(x => x.ProjectID == projectID && x.Year == year && x.Month == month);
            }
            else {
                personnelProjectsList = personnelProjectManager.List();
            }
            
            return View(personnelProjectsList);
        }
        public ActionResult GetProjectList()
        {
            projectList = projectManager.List();
            List<PersonnelProjectModel> ppModelList = new List<PersonnelProjectModel>();
            PersonnelProjectModel ppModel;
            foreach (var item in projectList)
            {
                ppModel = new PersonnelProjectModel { ID = item.ID, Name = item.Name };
                ppModelList.Add(ppModel);
            }
            return Json(new { result = true, veri = ppModelList }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetProjectListLeftSide()
        {
            projectList = projectManager.List();
            return PartialView("_PartialProjectListLeftSide",projectList);
        }
      
        public PartialViewResult GetFilteredList(int projectID, string year,string month)
        {
            
            personnelProjectsList = personnelProjectManager.List(x => x.ProjectID == projectID && x.Year == year && x.Month == month);
            return PartialView("_ProjectTable", personnelProjectsList);

        }
        public PartialViewResult GetProjectListRightSide(int projectID) {
            if (projectID > 0)
            {
                TempData["project-id"] = projectID;
                projects = projectManager.Find(x => x.ID == projectID);
                if (projects!=null)
                {
                    personnelProjectsList = projects.PersonnelProjectsTable;
                    if (personnelProjectsList!=null)
                    {
                        return PartialView("_PartialProjectListRightSide", personnelProjectsList);
                    }
                    else
                    {
                        return PartialView("_PartialPersonnelNotFound");
                    }
                    
                }
            }
            return PartialView("_PartialPersonnelNotFound");

        }
        public ActionResult AddPersonnel()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPersonnel(string year,string month,int? projectid,int? personnelid,string workrate)
        {
            TempData["mesaj"] = "Personel başarılı bir şekilde eklendi.";
            if (year!=null && month!=null&&projectid!=null&&personnelid!=null&&workrate!=null)
            {
                PersonnelProjectsTable personnelProjectsTable = new PersonnelProjectsTable();
                personnelProjectsTable.Month = month;
                personnelProjectsTable.PersonnelID = personnelid;
                personnelProjectsTable.ProjectID = projectid;
                personnelProjectsTable.Year = year;
                personnelProjectsTable.WorkRate = workrate;
                personnelProjectManager.Insert(personnelProjectsTable);
                
            }

           return RedirectToAction("ProjectList");
        }
        //public ActionResult AddPersonnel(int id,string month,string year,string workRate)
        //{
        //    if (id>0)
        //    {
        //        int tmp = (int)TempData["project-id"];
        //        if (personnelProjectManager.Find(x=>x.PersonnelID==id &&x.ProjectID== tmp &&x.Year==year &&x.Month==month) !=null) //burda hata verebilir.
        //        {
        //            return Json(new { result = false, mesaj = "Bu personel zaten kayıtlı." }, JsonRequestBehavior.AllowGet);
        //        }
        //        personnel = personnelManager.Find(x => x.Id == id);
        //        if (personnel!=null)
        //        {

        //            nwPersonnelPr = new PersonnelProjectsTable();
        //            nwPersonnelPr.PersonnelID = id;
        //            nwPersonnelPr.WorkRate = workRate;
        //            nwPersonnelPr.Month = month;
        //            nwPersonnelPr.Year = year;
        //            nwPersonnelPr.ProjectID = (int)TempData["project-id"];
        //            TempData["project-id"] = tmp;
        //            var res = personnelProjectManager.Insert(nwPersonnelPr);
        //            if (res>0)
        //            {
        //                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        //            }
        //            return Json(new { result = false,mesaj="Hata ile Karşılaşıldı" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return Json(new { result = false, mesaj = "Hata ile Karşılaşıldı" }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult DeletePersonnel(int id)
        {
            if (id>0)
            {
                nwPersonnelPr = personnelProjectManager.Find(x => x.PersonnelID==id);
                if (nwPersonnelPr==null)
                {
                    return Json(new { result = false, mesaj = "Personel bulunamadı." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var res = personnelProjectManager.Delete(nwPersonnelPr);
                    if (res>0)
                    {
                        return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, mesaj = "Personel silinemedi." }, JsonRequestBehavior.AllowGet);
                    }
                }
               
            }
            return Json(new { result = false, mesaj = "Sunucuyla bağlantı kurulamadı." }, JsonRequestBehavior.AllowGet);
        }
    }
}