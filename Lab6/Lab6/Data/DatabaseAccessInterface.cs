using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab6.Models;
namespace Lab6.Data
{
    public interface DatabaseAccessI
    {
        List<User> GetAllUsers();
        void AddNewUser(User person);
        User GetAUserByID(int id);
        void UpdateUser(User id);
        void RemoveUser(User id);

        List<Pet> GetAllPets();
        //Get list of all pets that belong to given ApplicationUser ' username
        List<Pet> GetAllPets(string username);
        void AddNewPet(Pet newPet);
        Pet GetAPetByID(int id);
        void UpdatePet(Pet id);
        void RemovePet(Pet id);
    }
}
