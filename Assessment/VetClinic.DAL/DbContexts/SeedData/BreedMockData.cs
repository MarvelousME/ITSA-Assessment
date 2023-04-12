using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.DbContexts.SeedData
{
    public class BreedMockData
    {
        public static List<Breed> GetBreedList()
        {
            List<Breed> breedList = new List<Breed>();
            breedList.Add(item: new Breed() { Id = 1, Name = "American PitBull", IsDeleted = false, IsActive = true });
            breedList.Add(item: new Breed() { Id = 2, Name = "Poodle", IsDeleted = false, IsActive = true });
            breedList.Add(item: new Breed() { Id = 3, Name = "Cat", IsDeleted = false, IsActive = true });

            return breedList;
        }
    }
}
