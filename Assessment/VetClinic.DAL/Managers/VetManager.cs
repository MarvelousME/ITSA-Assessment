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
    public class VetManager : IVetManager
    {
        private readonly ApplicationDbContext _context;

        public VetManager(ApplicationDbContext context, IHttpContextAccessor httpAccessor)
        {
            _context = context;
            _context.CurrentUserId = httpAccessor.HttpContext?.User.FindFirst(ClaimConstants.Subject)?.Value?.Trim();
        }

        /// <summary>
        /// Get All Records
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="pageSize">size of records to show</param>
        /// <returns>List of Records</returns>
        public async Task<List<Vet>> GetVetsAsync(int page, int pageSize)
        {
            IQueryable<Vet> vetsQuery = _context.Vets
                .OrderBy(u => u.Name);


            if (page != -1)
                vetsQuery = vetsQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                vetsQuery = vetsQuery.Take(pageSize);


            return  await vetsQuery
                    .ToListAsync();
        }

        public async Task<(bool Succeeded, string[] Errors)> CreateVetAsync(Vet vet)
        {
            try
            {
                if (_context.Vets.Any(v => v.MedicalLicense == vet.MedicalLicense))
                    return (false, new string[] { "Vet already exists!" });

                var result = await _context.Vets.AddAsync(vet);

                await _context.SaveChangesAsync();


                return (true, new string[] { "Vet Added" });
            }
            catch(Exception ex)
            {
                return (false, new string[] { ex.ToString() });
            }
        }

        /// <summary>
        /// Check if a record exists
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> CheckIfRecordExists(int Id)
        {
            return (await _context.Vets.FindAsync(Id)) != null;
        }

        /// <summary>
        /// Select a record by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Vet> ReadVetsAsync(int Id)
        {
            return (Vet) (await _context.Vets.FindAsync(Id).ConfigureAwait(false));
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
                return (false, new string[] { ex.ToString() });
            }
        }

        /// <summary>
        /// Perform soft delete
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<(bool Succeeded, string[] Errors)> DeleteVetAsync(int Id)
        {
            try
            {
                if (await CheckIfRecordExists(Id) == false)
                    return (false, new string[] { "Vet does not exist!" });

                var vet = ReadVetsAsync(Id).Result;

                var result = _context.Vets.Update(vet);

                await _context.SaveChangesAsync();


                return (true, new string[] { "Vet Deleted!" });
            }
            catch (Exception ex)
            {
                return (false, new string[] { ex.ToString() });
            }
        }
    }
}
