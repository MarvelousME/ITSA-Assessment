using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.Core.Interfaces
{
    /// <summary>
    /// Interface signitures for a Data i.e AnimalType, Breed
    /// </summary>
    public interface IDataManager
    {
        Task<(List<AnimalType>, string[] Errors)> GetAnimalTypesAsync(int page, int pageSize);

        Task<(List<AnimalType>, string[] Errors)> GetBreedsAsync(int page, int pageSize);
    }
}
