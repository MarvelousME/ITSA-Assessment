using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Constants;
using VetClinic.DAL.DbContexts;
using VetClinic.DAL.Interfaces;

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
    }
}
