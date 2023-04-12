using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.DbContexts.SeedData
{
    public class AnimalTypeMockData
    {
        public static List<AnimalType> GetAnimalTypeList()
        {
            List<AnimalType> animalTypeList = new List<AnimalType>();
            animalTypeList.Add(item: new AnimalType() { Id = 1, Name = "Dog", IsDeleted = false, IsActive = true });
            animalTypeList.Add(item: new AnimalType() { Id = 2, Name = "Rabbit", IsDeleted = false, IsActive = true });
            animalTypeList.Add(item: new AnimalType() { Id = 3, Name = "Cat", IsDeleted = false, IsActive = true });

            return animalTypeList;
        }
    }
}
