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
    public class PetOwnerManager : IPetOwnerManager
    {
        private readonly IAccountManager accountManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public readonly ApplicationUser currentUser;
        public PetOwnerManager(ApplicationDbContext context, IHttpContextAccessor httpAccessor, IAccountManager accountManager, ILogger<PetOwnerManager> logger)
        {
            _context = context;
            _context.CurrentUserId = httpAccessor.HttpContext?.User.FindFirst(ClaimConstants.Subject)?.Value?.Trim();
            _logger = logger;

            currentUser = accountManager.GetUserByIdAsync(_context.CurrentUserId).Result;
        }

        /// <summary>
        /// Get All Records
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="pageSize">size of records to show</param>
        /// <returns>List of Records</returns>
        public async Task<(List<PetOwner>, string[] Errors)> GetPetOwnersAsync(int page, int pageSize)
        {
            IQueryable<PetOwner> petownersQuery;
            try
            {
                petownersQuery = _context.PetOwners
                   .OrderBy(u => u.Name);


                if (page != -1)
                    petownersQuery = petownersQuery.Skip((page - 1) * pageSize);

                if (pageSize != -1)
                    petownersQuery = petownersQuery.Take(pageSize);

                return (await petownersQuery.ToListAsync(), new string[] { "Pet Owner Added" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (null, new string[] { "Error retrieving list" });
            }
        }

        /// <summary>
        /// Get All Records of Pet Owners and associated Pet Detail Ids
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<(PetOwner petOwner, string[] PetDetailIds)>> GetPetOwnerAndPetDetailsAsync(int page, int pageSize)
        {
            IQueryable<PetOwner> petownersQuery;
            try
            {
                petownersQuery = _context.PetOwners
                   .Include(u => u.PetDetails)
                   .OrderBy(u => u.Name);

                if (page != -1)
                    petownersQuery = petownersQuery.Skip((page - 1) * pageSize);

                if (pageSize != -1)
                    petownersQuery = petownersQuery.Take(pageSize);

                var petowners = await petownersQuery.ToListAsync();

                var petdetailIds = petowners.SelectMany(u => u.PetDetails.Select(r => r.Id)).ToList();

                var petdetails = await _context.PetDetails
                    .Where(r => petdetailIds.Contains(r.Id))
                    .ToArrayAsync();

                return petowners
                    .Select(u => (u, petdetails.Where(r => u.PetDetails.Select(ur => ur.Id).Contains(r.Id)).Select(r => r.Name).ToArray()))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return null;
            }
        }

        /// <summary>
        /// Create a Pet Owner
        /// </summary>
        /// <param name="petOwner"></param>
        /// <returns></returns>
        public async Task<(PetOwner petOwner, string[] Errors)> CreatePetOwnerAsync(PetOwner petOwner)
        {
            try
            {
                if (_context.PetOwners.Any(v => v.IDNumber == petOwner.IDNumber))
                    return (null, new string[] { "Pet Owner already exists!" });

                var record = await _context.PetOwners.AddAsync(petOwner).ConfigureAwait(false);

                await _context.SaveChangesAsync();

                return (petOwner, new string[] { "Pet Owner Added" });
            }
            catch (Exception ex)
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
            return (await _context.PetOwners.FindAsync(Id).ConfigureAwait(false)) != null;
        }

        /// <summary>
        /// Select a record by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<(PetOwner petOwner, string[] Errors)> ReadPetOwnerAsync(int Id)
        {
            try
            {
                var result = await _context.PetOwners.FindAsync(Id).ConfigureAwait(false);
                if (result != null)
                {
                    return (result, new string[] { "Retrieved Pet Owner!" });
                }
                else
                {
                    return (result, new string[] { "Error retrieving Pet Owner!" });
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
        /// <param name="petOwner"></param>
        /// <returns></returns>
        public async Task<(bool Succeeded, string[] Errors)> UpdatePetOwnerAsync(PetOwner petOwner)
        {
            try
            {
                if (await CheckIfRecordExists(petOwner.Id) == false)
                    return (false, new string[] { "Pet Owner does not exist!" });

                var result = _context.PetOwners.Update(petOwner);

                await _context.SaveChangesAsync();


                return (true, new string[] { "Pet Owner Updated!" });
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
        /// <param name="Id">Id of PetOwner<</param>
        /// <param name="delete">Indicator to delete or un-delete</param>
        /// <returns></returns>
        public async Task<(bool Succeeded, string[] Errors)> DeletePetOwnerAsync(int Id, bool delete)
        {
            try
            {
                if (await CheckIfRecordExists(Id) == false)
                    return (false, new string[] { "Pet Owner does not exist!" });

                var record = await ReadPetOwnerAsync(Id);

                if (record.petOwner != null)
                {
                    record.petOwner.IsDeleted = delete;

                    var result = _context.PetOwners.Update(record.petOwner);

                    var updated = await _context.SaveChangesAsync().ConfigureAwait(false);

                    var msg = delete == true ? "Deleted" : "Un-Deleted";

                    return (true, new string[] { $"Pet Owner {msg}!" });
                }
                else
                {
                    return (false, new string[] { "Error deleting Pet Owner!" });
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
        /// <param name="Id">Id of PetOwner</param>
        /// <param name="active">Indicator to activate or de-activate</param>
        /// <returns></returns>
        public async Task<(bool Succeeded, string[] Errors)> IsActivePetOwnerAsync(int Id, bool active)
        {
            try
            {
                if (await CheckIfRecordExists(Id) == false)
                    return (false, new string[] { "Pet Owner does not exist!" });

                var record = ReadPetOwnerAsync(Id).Result;

                if (record.petOwner != null)
                {
                    record.petOwner.IsActive = active;

                    var result = _context.PetOwners.Update(record.petOwner);

                    var updated = await _context.SaveChangesAsync();

                    var msg = active == true ? "Active" : "De-Activated";

                    return (true, new string[] { $"Pet Owner {msg}!" });

                }
                else
                {
                    return (false, new string[] { "Error activating/de-activating Pet Owner!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (false, new string[] { ex.ToString() });
            }
        }

        public async Task<(bool Succeeded, string[] Errors)> AddPetDetailToPetOwnerAsync(int Id, PetDetail petdetail)
        {
            try
            {
                if (await CheckIfRecordExists(Id) == false)
                    return (false, new string[] { "Pet Owner does not exist!" });

                var owner = await ReadPetOwnerAsync(Id);

                petdetail.Owner = $"{owner.petOwner.Name} {owner.petOwner.Surname}";
                petdetail.PetOwner = owner.petOwner;
                //petdetail.CreatedBy = $"{currentUser.FullName}";
                //petdetail.UpdatedBy = $"{currentUser.FullName}";
                petdetail.CreatedDate = DateTime.UtcNow;
                petdetail.UpdatedDate = DateTime.UtcNow;

                var result = _context.PetDetails.Add(petdetail);

                await _context.SaveChangesAsync();


                return (true, new string[] { "Added Pet to Pet Owner successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (false, new string[] { ex.ToString() });
            }
        }

        public async Task<(bool Succeeded, string[] Errors)> RemovePetDetailFromPetOwnerAsync(int Id, int petdetailId)
        {
            try
            {
                if (await CheckIfRecordExists(Id) == false)
                    return (false, new string[] { "Pet Owner does not exist!" });

                var petDetail = await _context.PetDetails.FindAsync(petdetailId);

                if (petDetail == null) 
                    return (false, new string[] { "Pet Detail does not exist!" });

                petDetail.IsDeleted = true;

                var result = _context.PetDetails.Update(petDetail);

                await _context.SaveChangesAsync();

                return (true, new string[] { "Removed Pet from Pet Owner successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (false, new string[] { ex.ToString() });
            }
        }

        public async Task<(bool Succeeded, string[] Errors)> AddPetsToPetOwnerAsync(int Id, List<PetDetail> petdetails)
        {
            try
            {
                if (await CheckIfRecordExists(Id) == false)
                    return (false, new string[] { "Pet Owner does not exist!" });

                var owner = await ReadPetOwnerAsync(Id);

                int count = 0;
                foreach (var petdetail in petdetails)
                {
                    petdetail.Owner = $"{owner.petOwner.Name} {owner.petOwner.Surname}";
                    petdetail.PetOwner = owner.petOwner;
                    petdetail.CreatedBy = $"{currentUser.FullName}";
                    petdetail.UpdatedBy = $"{currentUser.FullName}";
                    petdetail.CreatedDate = DateTime.UtcNow;
                    petdetail.UpdatedDate = DateTime.UtcNow;

                    var result = _context.PetDetails.Add(petdetail);

                    await _context.SaveChangesAsync();

                    count++;
                }


                return (true, new string[] { $"Added {count} Pet(s) to Pet Owner successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (false, new string[] { ex.ToString() });
            }
        }

        public async Task<(bool Succeeded, string[] Errors)> RemovePetDetailsFromPetOwnerAsync(int Id, List<int> petdetailIds)
        {
            try
            {
                if (await CheckIfRecordExists(Id) == false)
                    return (false, new string[] { "Pet Owner does not exist!" });

                int count = 0;
                foreach (var petdetailId in petdetailIds)
                {
                    var petDetail = await _context.PetDetails.FindAsync(petdetailId);

                    if (petDetail == null)
                        return (false, new string[] { "Pet Detail does not exist!" });

                    petDetail.IsDeleted = true;

                    var result = _context.PetDetails.Update(petDetail);

                    await _context.SaveChangesAsync();

                    count++;
                }

                return (true, new string[] { $"Removed {count} Pet(s) from Pet Owner successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (false, new string[] { ex.ToString() });
            }
        }
    }
}
