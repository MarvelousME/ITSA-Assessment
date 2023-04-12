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
            animalTypeList.Add(item: new AnimalType() { Name = "Dog", IsDeleted = false, IsActive = true });
            animalTypeList.Add(item: new AnimalType() { Name = "Rabbit", IsDeleted = false, IsActive = true });
            animalTypeList.Add(item: new AnimalType() { Name = "Cat", IsDeleted = false, IsActive = true });

            return animalTypeList;
        }
    }
}
