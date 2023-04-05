using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.DAl.Models;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.Core.Interfaces
{
    public interface IVetClinicManager
    {
        Task<(PetOwner User, string[] Roles)?> GetUserAndRolesAsync(string userId);
    }
}
