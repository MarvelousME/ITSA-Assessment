using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Constants;
using VetClinic.DAL.DbContexts;
using VetClinic.DAL.Interfaces;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.Managers
{
    public class VetManager : IVetManager
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public VetManager(ApplicationDbContext context, IHttpContextAccessor httpAccessor, ILogger<VetManager> logger)
        {
            _context = context;
            _context.CurrentUserId = httpAccessor.HttpContext?.User.FindFirst(ClaimConstants.Subject)?.Value?.Trim();
            _logger = logger;
        }

        /// <summary>
        /// Get All Records
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="pageSize">size of records to show</param>
        /// <returns>List of Records</returns>
        public async Task<(List<Vet>, string[] Errors)> GetVetsAsync(int page, int pageSize)
        {
            IQueryable<Vet> vetsQuery;
            try
            {
                 vetsQuery = _context.Vets
                    .OrderBy(u => u.Name);


                if (page != -1)
                    vetsQuery = vetsQuery.Skip((page - 1) * pageSize);

                if (pageSize != -1)
                    vetsQuery = vetsQuery.Take(pageSize);

                return  (await vetsQuery.ToListAsync(), new string[] { "Vet Added" });
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);

                return (null, new string[] { "Error retrieving list" });
            }
        }

        /// <summary>
        /// Create a Vet
        /// </summary>
        /// <param name="vet"></param>
        /// <returns></returns>
        public async Task<(Vet vet, string[] Errors)> CreateVetAsync(Vet vet)
        {
            try
            {
                if (_context.Vets.Any(v => v.MedicalLicense == vet.MedicalLicense))
                    return (null, new string[] { "Vet already exists!" });

                var record = await _context.Vets.AddAsync(vet).ConfigureAwait(false);

                await _context.SaveChangesAsync();

                return (vet, new string[] { "Vet Added" });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (null, new string[] { ex.ToString() });
            }
        }

        /// <summary>
        /// Check if a record exists
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> CheckIfRecordExists(int Id)
        {
            return (await _context.Vets.FindAsync(Id).ConfigureAwait(false)) != null;
        }

        /// <summary>
        /// Select a record by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<(Vet vet, string[] Errors)> ReadVetAsync(int Id)
        {
            try
            {
                var result = await _context.Vets.FindAsync(Id).ConfigureAwait(false);
                if (result != null)
                {
                    return (result, new string[] { "Retrieved Vet!" });
                }
                else
                {
                    return (result, new string[] { "Error retrieving Vet!" });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (null, new string[] { "Cannot find entity" });
            }
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="vet"></param>
        /// <returns></returns>
        public async Task<(bool Succeeded, string[] Errors)> UpdateVetAsync(Vet vet)
        {
            try
            {
                if (await CheckIfRecordExists(vet.Id) == false)
                    return (false, new string[] { "Vet does not exist!" });

                var result = _context.Vets.Update(vet);

                await _context.SaveChangesAsync();


                return (true, new string[] { "Vet Updated!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (false, new string[] { ex.ToString() });
            }
        }

        /// <summary>
        /// Check if the Medical License for a Vet exists
        /// </summary>
        /// <param name="medicalLicense">Medical License Number</param>
        /// <returns></returns>
        public async Task<bool> CheckIfMedicalLicenseExists(string medicalLicense)
        {
            try
            {
                var result = await _context.Vets.AnyAsync(m => m.MedicalLicense == medicalLicense);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return false;
            }
        }

        /// <summary>
        /// Perform soft delete
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<(bool Succeeded, string[] Errors)> DeleteVetAsync(int Id,bool delete)
        {
            try
            {
                if (await CheckIfRecordExists(Id) == false)
                    return (false, new string[] { "Vet does not exist!" });

                var record = await ReadVetAsync(Id);

                if (record.vet != null)
                {
                    record.vet.IsDeleted = delete;

                    var result = _context.Vets.Update(record.vet);

                    var updated = await _context.SaveChangesAsync().ConfigureAwait(false);

                    var msg = delete == true ? "Deleted" : "Un-Deleted";

                    return (true, new string[] { $"Vet {msg}!" });
                }
                else
                {
                    return (false, new string[] { "Error deleting Vet!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (false, new string[] { ex.ToString() });
            }
        }

        /// <summary>
        /// Perform soft delete
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<(bool Succeeded, string[] Errors)> IsActiveVetAsync(int Id, bool active)
        {
            try
            {
                if (await CheckIfRecordExists(Id) == false)
                    return (false, new string[] { "Vet does not exist!" });

                var record = ReadVetAsync(Id).Result;

                if (record.vet != null)
                {
                    record.vet.IsActive = active;

                    var result = _context.Vets.Update(record.vet);

                    var updated = await _context.SaveChangesAsync();

                    var msg = active == true ? "Active" : "De-Activated";

                    return (true, new string[] { $"Vet {msg}!" });

                }
                else
                {
                    return (false, new string[] { "Error activating/de-activating Vet!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (false, new string[] { ex.ToString() });
            }
        }
    }
}
