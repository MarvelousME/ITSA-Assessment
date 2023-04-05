using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class PetDetailManager : IPetDetailManager
    {
        private readonly ApplicationDbContext _context;

        public PetDetailManager(ApplicationDbContext context, IHttpContextAccessor httpAccessor)
        {
            _context = context;
            _context.CurrentUserId = httpAccessor.HttpContext?.User.FindFirst(ClaimConstants.Subject)?.Value?.Trim();
        }
    }
}
