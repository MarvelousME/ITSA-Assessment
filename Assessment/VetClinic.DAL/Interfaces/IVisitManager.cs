using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.Interfaces
{
    public interface IVisitManager
    {
        Task<(List<Visit>, string[] Errors)> GetVisitsAsync(int page, int pageSize);
        Task<(Visit visit, string[] Errors)> CreateVisitAsync(Visit visit);
        Task<(Visit visit, string[] Errors)> ReadVisitAsync(int Id);
        Task<(bool Succeeded, string[] Errors)> DeleteVisitAsync(int Id, bool delete);
        Task<(bool Succeeded, string[] Errors)> IsActiveVisitAsync(int Id, bool active);
        Task<bool> CheckIfRecordExists(int Id);
    }
}
