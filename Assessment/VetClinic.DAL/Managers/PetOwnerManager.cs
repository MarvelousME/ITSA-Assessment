using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext _context;

        public PetOwnerManager(ApplicationDbContext context, IHttpContextAccessor httpAccessor)
        {
            _context = context;
            _context.CurrentUserId = httpAccessor.HttpContext?.User.FindFirst(ClaimConstants.Subject)?.Value?.Trim();
        }

        public async Task<List<(PetOwner petOwner, string[] PetDetailIds)>> GetPetOwnersAndPetDetailsAsync(int page, int pageSize)
        {
            IQueryable<PetOwner> petownersQuery = _context.PetOwners
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

        public async Task<(PetOwner petOwner, string[] PetDetailIds)?> GetPetOwnerAndPetDetailsAsync(int petownerId)
        {
            var petowner = await _context.PetOwners
                .Include(u => u.PetDetails)
                .Where(u => u.Id == petownerId)
                .SingleOrDefaultAsync();

            if (petowner == null)
                return null;

            var petownerPetDetailIds = petowner.PetDetails.Select(r => r.Id).ToList();

            var petDetails = await _context.PetDetails
                .Where(r => petownerPetDetailIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToArrayAsync();

            return (petowner, petDetails);
        }

        //public async Task<(bool Succeeded, string[] Errors)> CreateUserAsync(ApplicationUser user, IEnumerable<string> roles, string password)
        //{
        //    var result = await _userManager.CreateAsync(user, password);
        //    if (!result.Succeeded)
        //        return (false, result.Errors.Select(e => e.Description).ToArray());


        //    user = await _userManager.FindByNameAsync(user.UserName);

        //    try
        //    {
        //        result = await this._userManager.AddToRolesAsync(user, roles.Distinct());
        //    }
        //    catch
        //    {
        //        await DeleteUserAsync(user);
        //        throw;
        //    }

        //    if (!result.Succeeded)
        //    {
        //        await DeleteUserAsync(user);
        //        return (false, result.Errors.Select(e => e.Description).ToArray());
        //    }

        //    return (true, new string[] { });
        //}


        //public async Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(ApplicationUser user)
        //{
        //    return await UpdateUserAsync(user, null);
        //}


        //public async Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(ApplicationUser user, IEnumerable<string> roles)
        //{
        //    var result = await _userManager.UpdateAsync(user);
        //    if (!result.Succeeded)
        //        return (false, result.Errors.Select(e => e.Description).ToArray());


        //    if (roles != null)
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(user);

        //        var rolesToRemove = userRoles.Except(roles).ToArray();
        //        var rolesToAdd = roles.Except(userRoles).Distinct().ToArray();

        //        if (rolesToRemove.Any())
        //        {
        //            result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
        //            if (!result.Succeeded)
        //                return (false, result.Errors.Select(e => e.Description).ToArray());
        //        }

        //        if (rolesToAdd.Any())
        //        {
        //            result = await _userManager.AddToRolesAsync(user, rolesToAdd);
        //            if (!result.Succeeded)
        //                return (false, result.Errors.Select(e => e.Description).ToArray());
        //        }
        //    }

        //    return (true, new string[] { });
        //}

    }
}
