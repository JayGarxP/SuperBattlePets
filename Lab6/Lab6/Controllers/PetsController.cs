using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lab6.Models;
using Lab6.Data;
using Lab6.App_Start;
using Lab6.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab6.Controllers
{
    [Authorize]
    public class PetsController : Controller
    {
        private readonly DatabaseAccessI _dataRepository;
        private readonly I_PetServices _petServices;

        public PetsController(DatabaseAccessI dataRepository, I_PetServices petServices)
        {
            _dataRepository = dataRepository;
            _petServices = petServices;
        }

        // GET: Pets; 
        public ActionResult Index()
        {
            var petz = _dataRepository.GetAllPets(System.Web.HttpContext.Current.User.Identity.GetUserName());
            return View(petz);
        }

        // GET: Pets/Details/5 Set today's merchant skill for pet
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = _dataRepository.GetAPetByID(id.Value);
            pet.MerchantSkill = _petServices.GenerateDailyShopAbility(pet);

            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        // GET: Pets/Create
        public ActionResult Create()
        {
            string usrname = System.Web.HttpContext.Current.User.Identity.GetUserName();

            var pets = _dataRepository.GetAllPets(usrname);
            ViewBag.GroupList = new MultiSelectList(pets, "PetID", "Nickname");
            ViewBag.UserCashDD = _dataRepository.GetListItemofUsersCash(usrname);
            return View();
        }

        // POST: Pets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PetID,NickName")] Pet pet,
            List<int>UserIds)
        {
            if (ModelState.IsValid)
            {
                string creator = System.Web.HttpContext.Current.User.Identity.GetUserName();
                //ApplicationUser _user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                if (UserIds != null) {
                    pet.Users = new List<User>();
                    //many to many ORM ???
                    foreach (var aUserRef in UserIds)
                {
                    pet.Users.Add(
                        new User { PersonID = aUserRef }
                        );
                }
                }
                
                pet.Creator = creator;
                _dataRepository.AddNewPet(pet);
                

                return RedirectToAction("Index");
            }
            return View(pet);
        }

        // GET: Pets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = _dataRepository.GetAPetByID(id.Value);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PetID,NickName")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                _dataRepository.UpdatePet(pet);
                return RedirectToAction("Index");
            }
            return View(pet);
        }

        // GET: Pets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = _dataRepository.GetAPetByID(id.Value);
            if (pet == null)
            {
                return HttpNotFound();
            }

            _dataRepository.RemovePet(pet);

            return RedirectToAction("Index");
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pet pet = _dataRepository.GetAPetByID(id);
            return RedirectToAction("Index");
        }
    }
}
