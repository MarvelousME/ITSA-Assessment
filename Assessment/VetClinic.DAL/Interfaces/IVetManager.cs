using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.Interfaces
{
    public interface IVetManager
    {
        Task<List<Vet>> GetVetsAsync(int page, int pageSize);
        Task<(bool Succeeded, string[] Errors)> CreateVetAsync(Vet vet);
        Task<Vet> ReadVetsAsync(int Id);
        Task<(bool Succeeded, string[] Errors)> UpdateVetAsync(Vet vet);
        Task<(bool Succeeded, string[] Errors)> DeleteVetAsync(int Id);
        Task<bool> CheckIfRecordExists(int Id);
    }
}
