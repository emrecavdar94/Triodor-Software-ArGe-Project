using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TriodorArgeProject.Entities;
using TriodorArgeProject.Entities.EntityClasses;
using TriodorArgeProject.Service.Managers;

namespace TriodorArgeProject.WebUI.Controllers
{

    public class PersonnelController : Controller
    {
        private PersonnelManager personnelManager = new PersonnelManager();
        private ProjectManager projectManager = new ProjectManager();
        private PersonnelProjectManager personnelProjectManager = new PersonnelProjectManager();
        private List<Personnels> personnelList = new List<Personnels>();
        private List<PersonnelModels> personnelModelsList = new List<PersonnelModels>();
        private UserManager userManager = new UserManager();
        private PositionManager positionManager = new PositionManager();
        private Personnels personnels;
        PersonnelModels personnelModels;
        // GET: Personnel
        public ActionResult GetPersonnelList()
        {
            personnelList = personnelManager.List();
            foreach (var item in personnelList)
            {
                personnelModels = new PersonnelModels { ID = item.Id, Name = item.Name + " " + item.Surname };
                personnelModelsList.Add(personnelModels);
            }

            return Json(new { result = true, veri = personnelModelsList }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PersonnelList()
        {
            return View(personnelManager.List());
        }
        public ActionResult Create()
        {
            ViewBag.POSITION = positionManager.List();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string firstname,string lastname,string registerno,string startdate,string leavedate,string position,string username,string password,string repassword)
        {
           
            if (firstname!=null && lastname !=null &&registerno!=null&&startdate!=null &&position!=null &&password!=null&&password==repassword)
            {
                personnels = new Personnels();
                personnels.Name = firstname;
                personnels.Surname = lastname;
                personnels.RegisterNo = registerno;
                personnels.StartDate= DateTime.Parse(startdate);
                if (leavedate!="")
                {
                    personnels.LeaveDate = DateTime.Parse(leavedate);
                    personnels.IsActive = false;
                }
                else
                {
                    personnels.IsActive = true;
                }
                personnels.PositionID=positionManager.Find(x => x.Position == position).ID;
                
               Users user = new Users();
               user.Username = username;
               user.Password = password;
                user.RoleID = null;
                personnels.Users = user;

                var res = personnelManager.Insert(personnels);
                if (res>0)
                {
                    TempData["personelmesaj"] = "Personel Başarılı Bir Şekilde Eklendi.";
                }

                return RedirectToAction("PersonnelList");



            }
            else
            {
                TempData["personelmesaj"] = "Lütfen tüm alanları doldurunuz.";
            }
            return View(personnels);

        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnels personnel = personnelManager.Find(x => x.Id == id);
            if (personnel == null)
            {
                return HttpNotFound();
            }

            return View(personnel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Personnels personnels)
        {
            if (ModelState.IsValid)
            {
                var res = personnelManager.Update(personnels);
                if (res > 0)
                {
                    TempData["mesaj"] = "Personel başarılı bir şekilde güncellendi.";
                }
                TempData["mesaj"] = "Personel güncellenirken hata ile karşılaşıldı.";
                return RedirectToAction("Index");
            }
            return View(personnels);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnels personnels = personnelManager.Find(x => x.Id == id);
            if (personnels==null)
            {
                return HttpNotFound();

            }
            return View(personnels);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Personnels personnels = personnelManager.Find(x => x.Id == id);
            personnelManager.Delete(personnels);
            return RedirectToAction("PersonnelList");
        }
        public ActionResult Detail(int? id ,string year="2010",int projectID=10)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personnels = personnelManager.Find(x => x.Id == id);
            
            List<PersonnelProjectsTable> personnelProject = personnelProjectManager.List(x => x.PersonnelID == id && x.Year == year && x.ProjectID == projectID) ;
            List<string> monthList = new List<string>();
            string projectName = projectManager.Find(x => x.ID == projectID).Name;
            List<int> workRate = new List<int>();

            foreach (var item in personnelProject)
            {
                monthList.Add(item.Month);
                workRate.Add(Int32.Parse(item.WorkRate));
            }
            if (personnels == null)
            {
                
                return HttpNotFound();

            }
            ViewBag.WORKRATES = workRate;
            ViewBag.MONTHS = monthList;
            ViewBag.PROJECTNAME = projectName;

            return View(personnels);
        }
        public ActionResult GetAddEducation()
        {
            return PartialView("_PartialAddEducation");
        }

        public ActionResult GetAddAbility()
        {
            return PartialView("_PartialAddAbility");
        }

        public ActionResult GetAddWorkExperience()
        {
            return PartialView("_PartialAddWorkExperience");
        }

        public ActionResult GetAddCertificate()
        {
            return PartialView("_PartialAddCertificate");
        }

        public ActionResult GetEducationInfo(int id)
        {

            return PartialView("_PartialEducationInfo",personnelManager.Find(x=>x.Id==id).PersonnelEducations);
        }

        public ActionResult GetAbilityInfo(int id)
        {

            return PartialView("_PartialAbilityInfo", personnelManager.Find(x => x.Id == id).PersonnelAbility);
        }

        public ActionResult GetWorkExperienceInfo(int id)
        {

            return PartialView("_PartialWorkExperienceInfo", personnelManager.Find(x => x.Id == id).WorkExperiences);
        }

        public ActionResult GetCertificateInfo(int id)
        {

            return PartialView("_PartialCertificateInfo", personnelManager.Find(x => x.Id == id).PersonnelCertificates);
        }

        public ActionResult PersonnelInfo(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnels personnel = personnelManager.Find(x => x.Id == id);
            if (personnel==null)
            {
                return HttpNotFound();
            }
            return View(personnel);
        }
          public ActionResult AddEducation(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEducation(int? id,PersonnelEducations education)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }
    }
}