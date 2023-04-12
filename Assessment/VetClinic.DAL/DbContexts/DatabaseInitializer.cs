using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Core.Enums;
using VetClinic.Core.Permissions;
using VetClinic.DAL.DbContexts.SeedData;
using VetClinic.DAL.Interfaces;
using VetClinic.DAL.Models;
using VetClinic.DAL.SeedData;

namespace VetClinic.DAL.DbContexts
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }



    public class DatabaseInitializer : IDatabaseInitializer
    {
        readonly ApplicationDbContext _context;
        readonly IAccountManager _accountManager;
        readonly ILogger _logger;

        public DatabaseInitializer(ApplicationDbContext context, IAccountManager accountManager, ILogger<DatabaseInitializer> logger)
        {
            _accountManager = accountManager;
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);
            await SeedDefaultUsersAsync();
            await SeedDataAsync();
        }

        private async Task SeedDefaultUsersAsync()
        {
            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Generating user accounts");

                const string adminRoleName = "administrator";
                const string userRoleName = "user";

                await EnsureRoleAsync(adminRoleName, "administrator", ApplicationPermissions.GetAllPermissionValues());
                await EnsureRoleAsync(userRoleName, "user", new string[] { });

                await CreateUserAsync("admin", "Admin@123", "Administrator", "admin@vetclinic.co.za", "+27 (011) 327-6234", new string[] { adminRoleName });
                await CreateUserAsync("staff", "User@123", "User also known as Employee", "user@vetclinic.co.za", "+27 (011) 327-6235", new string[] { userRoleName });

                _logger.LogInformation("Account generation completed");
            }
        }

        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if (await _accountManager.GetRoleByNameAsync(roleName) == null)
            {
                _logger.LogInformation($"Generating default role: {roleName}");

                ApplicationRole applicationRole = new ApplicationRole(roleName, description);

                var result = await _accountManager.CreateRoleAsync(applicationRole, claims);

                if (!result.Succeeded)
                    throw new Exception($"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(string userName, string password, string fullName, string email, string phoneNumber, string[] roles)
        {
            _logger.LogInformation($"Generating default user: {userName}");

            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                IsEnabled = true
            };

            var result = await _accountManager.CreateUserAsync(applicationUser, roles, password);

            if (!result.Succeeded)
                throw new Exception($"Seeding \"{userName}\" user failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");

            return applicationUser;
        }

        /// <summary>
        /// Seed Breed, AnimalType, PetOwner, PetDetail, Vet tables with dummy data
        /// </summary>
        /// <returns></returns>
        private async Task SeedDataAsync()
        {
            if (!await _context.PetOwners.AnyAsync() && !await _context.Vets.AnyAsync())
            {
                _logger.LogInformation("Seeding data");

                #region Vet Seed Data
                var Vets = VetMockData.GetSampleVetList();

                foreach(Vet record in Vets)
                {
                    await _context.Vets.AddAsync(record);
                    await _context.SaveChangesAsync();
                }
                #endregion

                #region PetOwner Seed Data
                var PetOwners = PetOwnerMockData.GetPetOwnerList();

                foreach (PetOwner record in PetOwners)
                {
                    await _context.PetOwners.AddAsync(record);
                    await _context.SaveChangesAsync();
                }
                #endregion

                #region AnimalType Seed Data
                var animalTypes = AnimalTypeMockData.GetAnimalTypeList();

                foreach (AnimalType record in animalTypes)
                {
                    await _context.AnimalTypes.AddAsync(record);
                    await _context.SaveChangesAsync();
                }
                #endregion

                #region Breed Seed Data
                var breeds = BreedMockData.GetBreedList();

                foreach (Breed record in breeds)
                {
                    await _context.Breeds.AddAsync(record);
                    await _context.SaveChangesAsync();
                }
                #endregion

                _logger.LogInformation("Seeding data completed");
            }
        }

    }
}
