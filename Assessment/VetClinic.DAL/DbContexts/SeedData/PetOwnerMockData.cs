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
        public static List<PetOwner> GetPetOwnerList()
        {
            ICollection<PetDetail> petDetails= new List<PetDetail>();
            petDetails.Add(item: new PetDetail() { Id = 1 });
            petDetails.Add(item: new PetDetail() { Id = 2 });
            petDetails.Add(item: new PetDetail() { Id = 3 });

            ICollection<PetDetail> petDetails2 = new List<PetDetail>();
            petDetails.Add(item: new PetDetail() { Id = 1 });
            petDetails.Add(item: new PetDetail() { Id = 2 });
            petDetails.Add(item: new PetDetail() { Id = 3 });

            ICollection<PetDetail> petDetails3 = new List<PetDetail>();
            petDetails.Add(item: new PetDetail() { Id = 1 });
            petDetails.Add(item: new PetDetail() { Id = 2 });
            petDetails.Add(item: new PetDetail() { Id = 3 });

            List<PetOwner> petOwners = new List<PetOwner>();

            PetOwner petOwner = new PetOwner()
            {
                Id = 1,
                Name = "Marvin",
                Surname = "Saunders",
                Email = "marvin.saunders@gmail.com",
                IDNumber = "8305045248080",
                PetDetails = petDetails,
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = "Tester",
                UpdatedBy = "Tester"
            };
            petOwners.Add(petOwner);

             petOwner = new PetOwner()
            {
                Id = 1,
                Name = "Niquole",
                Surname = "Pieters",
                Email = "niquole.pieters@gmail.com",
                IDNumber = "8305045248080",
                PetDetails = petDetails2,
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = "Tester",
                UpdatedBy = "Tester"
            };
            petOwners.Add(petOwner);

             petOwner = new PetOwner()
            {
                Id = 3,
                Name = "Bradley",
                Surname = "Wade",
                Email = "bradley.wade@gmail.com",
                IDNumber = "8305045248080",
                PetDetails = petDetails3,
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = "Tester",
                UpdatedBy = "Tester"
            };
            petOwners.Add(petOwner);

            return petOwners;
        }
    }
}
