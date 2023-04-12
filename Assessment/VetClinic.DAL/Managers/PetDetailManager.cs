using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Constants;
using VetClinic.DAL.DbContexts;
using VetClinic.DAL.Interfaces;
using VetClinic.DAL.Models;

namespace VetClinic.DAL.Managers
{
    public class PetDetailManager : IPetDetailManager
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public PetDetailManager(ApplicationDbContext context, IHttpContextAccessor httpAccessor, ILogger<PetDetailManager> logger)
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
        /// <param name="PetOwnerId">Pet Owner Id</param>
        /// <returns>List of Records</returns>
        public async Task<(List<PetDetail>, string[] Errors)> GetPetDetailsAsync(int page, int pageSize)
        {
            IQueryable<PetDetail> petDetailsQuery;
            try
            {
                petDetailsQuery = _context.PetDetails
                   .OrderBy(u => u.Name);


                if (page != -1)
                    petDetailsQuery = petDetailsQuery.Skip((page - 1) * pageSize);

                if (pageSize != -1)
                    petDetailsQuery = petDetailsQuery.Take(pageSize);

                return (await petDetailsQuery.ToListAsync(), new string[] { "Pet Detail List" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (null, new string[] { "Error retrieving list" });
            }
        }

        /// <summary>
        /// Create a Pet Owner's Pet Detail
        /// </summary>
        /// <param name="PetDetail"></param>
        /// <returns></returns>
        public async Task<(PetOwner petOwner, List<PetDetail> petDetails, string[] Errors)> CreatePetDetailAsync(int petOwnerId, List<PetDetail> petDetails)
        {
            try
            {
                if (!_context.PetOwners.Any(v => v.Id == petOwnerId))
                    return (null, null, new string[] { "Pet Owner doesn't exist!" });

                var petOwner = _context.PetOwners.FirstOrDefault(v => v.Id == petOwnerId);  

                if(petDetails.Count() > 0)
                {
                    foreach (var petDetail in petDetails)
                    {
                        var record = await _context.PetDetails.AddAsync(petDetail).ConfigureAwait(false);

                        await _context.SaveChangesAsync();
                    }
                }

                return (petOwner, petDetails, new string[] { "Pet Owner pet details added" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (null, null, new string[] { ex.ToString() });
            }
        }

        /// <summary>
        /// Check if a record exists
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> CheckIfRecordExists(int Id)
        {
            return (await _context.PetDetails.FindAsync(Id).ConfigureAwait(false)) != null;
        }

        /// <summary>
        /// Select a record by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<(PetDetail petDetail, string[] Errors)> ReadPetDetailAsync(int Id)
        {
            try
            {
                var result = await _context.PetDetails.FindAsync(Id).ConfigureAwait(false);
                if (result != null)
                {
                    return (result, new string[] { "Retrieved Pet Detail!" });
                }
                else
                {
                    return (result, new string[] { "Error retrieving Pet Detail!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (null, new string[] { "Cannot find entity" });
            }
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="PetDetail"></param>
        /// <returns></returns>
        public async Task<(bool Succeeded, string[] Errors)> UpdatePetDetailAsync(PetDetail petDetail)
        {
            try
            {
                if (await CheckIfRecordExists(petDetail.Id) == false)
                    return (false, new string[] { "Pet Detail does not exist!" });

                var result = _context.PetDetails.Update(petDetail);

                await _context.SaveChangesAsync();


                return (true, new string[] { "Pet Detail Updated!" });
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
        /// <param name="Id">Id of PetDetail<</param>
        /// <param name="delete">Indicator to delete or un-delete</param>
        /// <returns></returns>
        public async Task<(bool Succeeded, string[] Errors)> DeletePetDetailAsync(int Id, bool delete)
        {
            try
            {
                if (await CheckIfRecordExists(Id) == false)
                    return (false, new string[] { "Vet does not exist!" });

                var record = await ReadPetDetailAsync(Id);

                if (record.petDetail != null)
                {
                    record.petDetail.IsDeleted = delete;

                    var result = _context.PetDetails.Update(record.petDetail);

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
        /// Activate or De-activate
        /// </summary>
        /// <param name="Id">Id of PetDetail</param>
        /// <param name="active">Indicator to activate or de-activate</param>
        /// <returns></returns>
        public async Task<(bool Succeeded, string[] Errors)> IsActivePetDetailAsync(int Id, bool active)
        {
            try
            {
                if (await CheckIfRecordExists(Id) == false)
                    return (false, new string[] { "Vet does not exist!" });

                var record = ReadPetDetailAsync(Id).Result;

                if (record.petDetail != null)
                {
                    record.petDetail.IsActive = active;

                    var result = _context.PetDetails.Update(record.petDetail);

                    var updated = await _context.SaveChangesAsync();

                    var msg = active == true ? "Active" : "De-Activated";

                    return (true, new string[] { $"Pet Detail {msg}!" });

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
