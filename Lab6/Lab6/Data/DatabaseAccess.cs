using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lab6.Models;
using System.Data.Entity;

namespace Lab6.Data
{
    public class DatabaseAccess : DatabaseAccessI
    {
        private readonly ApplicationDbContext _databaseContext;

        public DatabaseAccess() : base()
        {
            //DBContext is the Entity framework auto-genned database
            _databaseContext = new ApplicationDbContext();
        }


        public List<User> GetAllUsers()
        {
            return _databaseContext.CustomUsers.ToList();

        }

        public void AddNewUser(User user)
        {
            foreach (var aPet in user.MyPets)
            {
                _databaseContext.Pets.Attach(aPet);
            }
            _databaseContext.CustomUsers.Add(user);
            _databaseContext.SaveChanges();
        }

        public User GetAUserByID(int id)
        {
            return _databaseContext.CustomUsers.Find(id);
        }

        public void UpdateUser(User uza)
        {
            _databaseContext.Entry(uza).State = EntityState.Modified;
            _databaseContext.SaveChanges();
        }

        public void RemoveUser(User uza)
        {
            _databaseContext.CustomUsers.Remove(uza);
            _databaseContext.SaveChanges();
        }




        /// <summary>
        /// Could bump out models DB Access into seperate implementations or
        /// seperate interface&implementations
        /// </summary>
        /// <returns></returns>
        public List<Pet> GetAllPets()
        {
            return _databaseContext.Pets.ToList();
        }

        public void AddNewPet(Pet newPet) {
            foreach (var uza in newPet.Users)
            {
                _databaseContext.CustomUsers.Attach(uza);
            }
            _databaseContext.Pets.Add(newPet);
            _databaseContext.SaveChanges();
        }

        public Pet GetAPetByID(int id)
        {
            return _databaseContext.Pets.Find(id);
        }

        public void UpdatePet(Pet pyet) {
            _databaseContext.Entry(pyet).State = EntityState.Modified;
            _databaseContext.SaveChanges();
        }

        public void RemovePet(Pet pyet) {
            _databaseContext.Pets.Remove(pyet);
            _databaseContext.SaveChanges();
        }
    }
}