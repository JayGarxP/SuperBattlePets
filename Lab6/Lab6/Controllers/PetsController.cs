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

namespace Lab6.Controllers
{
    [Authorize]
    public class PetsController : Controller
    {
        //private DBContext db = new DBContext();

        private readonly DatabaseAccessI _dataRepository;
        private readonly I_PetServices _petServices;
        //public PetsController() {
        //    //_dataRepository = new Data.DatabaseAccess();
        //}

        public PetsController(DatabaseAccessI dataRepository, I_PetServices petServices)
        {
            _dataRepository = dataRepository;
            _petServices = petServices;
        }

        // GET: Pets; 
        public ActionResult Index()
        {
            var petz = _dataRepository.GetAllPets();
            //foreach (var pet in petz)
            //{
            //    //determine daily skill 
            //    pet.MerchantSkill = _petServices.GenerateDailyShopAbility(pet);
            //}
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
            //??? Should this be Users view instead??
            var pets = _dataRepository.GetAllPets();
            ViewBag.GroupList = new MultiSelectList(pets, "PetID", "Nickname");
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
                pet.Users = new List<User>();
                //HL: many to many ORM ???
                foreach (var aUserRef in UserIds)
                {
                    pet.Users.Add(
                        new User { PersonID = aUserRef }
                        );
                }

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
