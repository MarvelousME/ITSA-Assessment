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
            breedList.Add(item: new Breed() { Name = "American PitBull", IsDeleted = false, IsActive = true });
            breedList.Add(item: new Breed() { Name = "Poodle", IsDeleted = false, IsActive = true });
            breedList.Add(item: new Breed() { Name = "Cat", IsDeleted = false, IsActive = true });

            return breedList;
        }
    }
}
