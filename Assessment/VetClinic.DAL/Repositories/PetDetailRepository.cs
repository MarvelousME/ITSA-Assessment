using Microsoft.EntityFrameworkCore;
using VetClinic.DAL.DbContexts;
using VetClinic.DAL.Models;
using VetClinic.DAL.Repositories.Interfaces;

namespace VetClinic.DAL.Repositories
{
    public class PetDetailRepository : Repository<PetDetail>, IPetDetailRepository
    {
        public PetDetailRepository(DbContext context) : base(context)
        { }

        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
    }
}
