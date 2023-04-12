using System.Threading.Tasks;
using System;
using VetClinic.DAL.DbContexts;
using VetClinic.DAL.Managers;
using VetClinic.DAL.Models;
using System.Collections.Generic;

namespace VetClinic.DAL.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IPetDetailManager
    {
        Task<(List<PetDetail>, string[] Errors)> GetPetDetailsAsync(int page, int pageSize);
        Task<(PetOwner petOwner, List<PetDetail> petDetails, string[] Errors)> CreatePetDetailAsync(int petOwnerId, List<PetDetail> petDetails);
        Task<bool> CheckIfRecordExists(int Id);
        Task<(PetDetail petDetail, string[] Errors)> ReadPetDetailAsync(int Id);
        Task<(bool Succeeded, string[] Errors)> UpdatePetDetailAsync(PetDetail petDetail);
        Task<(bool Succeeded, string[] Errors)> IsActivePetDetailAsync(int Id, bool active);


    }
}
