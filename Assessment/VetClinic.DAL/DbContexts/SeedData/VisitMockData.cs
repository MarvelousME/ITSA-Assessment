using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.SeedData
{
    public class VisitMockData
    {
        public static List<Visit> GetSampleVisitList()
        {
            List<Visit> visits = new List<Visit>();

            Visit visit = new Visit()
            {
                Id = 1,
                PetDetail= new PetDetail() { Id = 2 },
                Vet = new Vet() { Id = 2},
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = "Tester",
                UpdatedBy = "Tester"
            };
            visits.Add(visit);

            visit = new Visit()
            {
                Id = 1,
                PetDetail = new PetDetail() { Id = 3 },
                Vet = new Vet() { Id = 3 },
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = "Tester",
                UpdatedBy = "Tester"
            };
            visits.Add(visit);

            return visits;
        }
    }
}
