using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.Interfaces
{
    public interface IVetManager
    {
        Task<(List<Vet>, string[] Errors)> GetVetsAsync(int page, int pageSize);
        Task<(Vet vet, string[] Errors)> CreateVetAsync(Vet vet);
        Task<(Vet vet, string[] Errors)> ReadVetAsync(int Id);
        Task<(bool Succeeded, string[] Errors)> UpdateVetAsync(Vet vet);
        Task<(bool Succeeded, string[] Errors)> DeleteVetAsync(int Id, bool delete);
        Task<(bool Succeeded, string[] Errors)> IsActiveVetAsync(int Id, bool active);
        Task<bool> CheckIfRecordExists(int Id);
        Task<bool> CheckIfMedicalLicenseExists(string medicalLicense);
    }
}
