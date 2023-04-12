using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.SeedData
{
    public class PetOwnerMockData
    {
        public static IEnumerable<PetOwner> GetPetOwnerList()
        {
            IList<PetOwner> petOwners = new List<PetOwner>();

            PetOwner petOwner = new PetOwner()
            {
                Name = "Marvin",
                Surname = "Saunders",
                Phone = "0813340625",
                Email = "marvin.saunders@gmail.com",
                IDNumber = "8305045248080",
                AccountNumber = "0123456789",
                PetDetails = null,
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = "1",
                UpdatedBy = "1"
            };
            petOwners.Add(petOwner);

             petOwner = new PetOwner()
            {
                Name = "Niquole",
                Surname = "Pieters",
                 Phone = "0813340625",
                 Email = "niquole.pieters@gmail.com",
                IDNumber = "8305045248080",
                 AccountNumber = "0123456789",
                 PetDetails = null,
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = "1",
                UpdatedBy = "1"
            };
            petOwners.Add(petOwner);

             petOwner = new PetOwner()
            {
                Name = "Bradley",
                Surname = "Wade",
                Phone = "0813340625",
                Email = "bradley.wade@gmail.com",
                 AccountNumber = "0123456789",
                 IDNumber = "8305045248080",
                PetDetails = null,
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = "1",
                UpdatedBy = "1"
            };
            petOwners.Add(petOwner);

            return petOwners;
        }
    }
}
