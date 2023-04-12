using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.Interfaces
{
    public interface IPetOwnerManager
    {
        Task<(List<PetOwner>, string[] Errors)> GetPetOwnersAsync(int page, int pageSize);
        Task<List<(PetOwner petOwner, string[] PetDetailIds)>> GetPetOwnerAndPetDetailsAsync(int page, int pageSize);
        Task<(PetOwner petOwner, string[] Errors)> CreatePetOwnerAsync(PetOwner vet);
        Task<(PetOwner petOwner, string[] Errors)> ReadPetOwnerAsync(int Id);
        Task<(bool Succeeded, string[] Errors)> UpdatePetOwnerAsync(PetOwner petOwner);
        Task<(bool Succeeded, string[] Errors)> DeletePetOwnerAsync(int Id, bool delete);
        Task<(bool Succeeded, string[] Errors)> IsActivePetOwnerAsync(int Id, bool active);
        Task<(bool Succeeded, string[] Errors)> AddPetDetailToPetOwnerAsync(int Id, PetDetail petdetail);
        Task<(bool Succeeded, string[] Errors)> RemovePetDetailFromPetOwnerAsync(int Id, int petdetailId);
        Task<(bool Succeeded, string[] Errors)> AddPetsToPetOwnerAsync(int Id, List<PetDetail> petdetails);
        Task<(bool Succeeded, string[] Errors)> RemovePetDetailsFromPetOwnerAsync(int Id, List<int> petdetailIds);
        Task<bool> CheckIfRecordExists(int Id);
    }
}
