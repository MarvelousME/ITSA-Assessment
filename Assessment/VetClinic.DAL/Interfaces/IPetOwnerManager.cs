using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.Interfaces
{
    public interface IPetOwnerManager
    {
        Task<List<(PetOwner petOwner, string[] PetDetailIds)>> GetPetOwnersAndPetDetailsAsync(int page, int pageSize);

        Task<(PetOwner petOwner, string[] PetDetailIds)?> GetPetOwnerAndPetDetailsAsync(int petownerId);
    }
}
