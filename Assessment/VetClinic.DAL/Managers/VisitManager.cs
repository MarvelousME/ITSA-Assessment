using Microsoft.AspNetCore.Http;
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
    public class VisitManager : IVisitManager
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public VisitManager(ApplicationDbContext context, IHttpContextAccessor httpAccessor, ILogger<VisitManager> logger)
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
        public async Task<(List<Visit>, string[] Errors)> GetVisitsAsync(int page, int pageSize)
        {
            IQueryable<Visit> visitsQuery;
            try
            {
                visitsQuery = _context.Visits
                   .OrderBy(u => u.Id);


                if (page != -1)
                    visitsQuery = visitsQuery.Skip((page - 1) * pageSize);

                if (pageSize != -1)
                    visitsQuery = visitsQuery.Take(pageSize);

                return (await visitsQuery.ToListAsync(), new string[] { "Retrieved All Visits" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (null, new string[] { "Error retrieving list" });
            }
        }


        /// <summary>
        /// Create a Visit
        /// </summary>
        /// <param name="visit"></param>
        /// <returns></returns>
        public async Task<(Visit visit, string[] Errors)> CreateVisitAsync(Visit visit)
        {
            try
            {
                var record = await _context.Visits.AddAsync(visit).ConfigureAwait(false);

                await _context.SaveChangesAsync();

                return (visit, new string[] { "Visit Added" });
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
            return (await _context.Visits.FindAsync(Id).ConfigureAwait(false)) != null;
        }

        /// <summary>
        /// Select a record by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<(Visit visit, string[] Errors)> ReadVisitAsync(int Id)
        {
            try
            {
                var result = await _context.Visits.FindAsync(Id).ConfigureAwait(false);
                if (result != null)
                {
                    return (result, new string[] { "Retrieved Visit!" });
                }
                else
                {
                    return (result, new string[] { "Error retrieving Visit!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return (null, new string[] { "Cannot find entity" });
            }
        }


        /// <summary>
        /// Perform soft delete
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<(bool Succeeded, string[] Errors)> DeleteVisitAsync(int Id, bool delete)
        {
            try
            {
                if (await CheckIfRecordExists(Id) == false)
                    return (false, new string[] { "Vet does not exist!" });

                var record = await ReadVisitAsync(Id);

                if (record.visit != null)
                {
                    record.visit.IsDeleted = delete;

                    var result = _context.Visits.Update(record.visit);

                    var updated = await _context.SaveChangesAsync().ConfigureAwait(false);

                    var msg = delete == true ? "Deleted" : "Un-Deleted";

                    return (true, new string[] { $"Visit {msg}!" });
                }
                else
                {
                    return (false, new string[] { "Error deleting Visit!" });
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
        public async Task<(bool Succeeded, string[] Errors)> IsActiveVisitAsync(int Id, bool active)
        {
            try
            {
                if (await CheckIfRecordExists(Id) == false)
                    return (false, new string[] { "Visit does not exist!" });

                var record = ReadVisitAsync(Id).Result;

                if (record.visit != null)
                {
                    record.visit.IsActive = active;

                    var result = _context.Visits.Update(record.visit);

                    var updated = await _context.SaveChangesAsync();

                    var msg = active == true ? "Active" : "De-Activated";

                    return (true, new string[] { $"Visit {msg}!" });

                }
                else
                {
                    return (false, new string[] { "Error activating/de-activating Visit!" });
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
